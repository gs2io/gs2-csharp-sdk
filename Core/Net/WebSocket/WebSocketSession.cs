#if UNITY_WEBGL && !UNITY_EDITOR
#define GS2_USE_HYBRID_WEBSOCKET
#endif

using System;
using System.Threading;
using System.Threading.Tasks;
using Gs2.Core.Util;
#if GS2_USE_HYBRID_WEBSOCKET
using Gs2.HybridWebSocket;
#else
using System.Net.Security;
using Gs2.Util.WebSocketSharp;
#endif

namespace Gs2.Core.Net
{
    public class WebSocketSession : IDisposable
    {
        public enum StateEnum
        {
            Connecting,
            Connected,
            Closing,
            Closed,
        }

#if GS2_USE_HYBRID_WEBSOCKET
        private readonly WebSocket _session;

        public event Action OnOpen
        {
            add => _session.OnOpen += value.Invoke;
            remove => _session.OnOpen -= value.Invoke;
        }

        public event Action<string> OnMessage
        {
            add => _session.OnMessage += value.Invoke;
            remove => _session.OnMessage -= value.Invoke;
        }

        public event Action OnClose;

        public event Action<ErrorEventArgs> OnError
        {
            add => _session.OnError += value.Invoke;
            remove => _session.OnError -= value.Invoke;
        }

        public WebSocketSession(string url)
        {
            _session = WebSocketFactory.CreateInstance(url);

            _session.OnClose += HandleClose;
        }

        public virtual void Connect() => _session.Connect();

        public virtual void Close() => _session.Close();

        public virtual void Send(string message) => _session.Send(message);

        public virtual void Dispose() => _session.Dispose();

        public virtual bool Ping() => true;

        public virtual StateEnum GetState()
        {
            return _session.GetState() switch
            {
                WebSocketState.Connecting => StateEnum.Connecting,
                WebSocketState.Open => StateEnum.Connected,
                WebSocketState.Closing => StateEnum.Closing,
                WebSocketState.Closed => StateEnum.Closed,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        private void HandleClose(CloseEventArgs e)
        {
            OnClose?.Invoke();
        }
#else
        private readonly WebSocket _session;
        private int _pingInFlight;
        [ThreadStatic] private static WebSocketSession _activeSendSession;
        [ThreadStatic] private static ErrorEventArgs _activeSendError;

        public event Action OnOpen;
        public event Action<string> OnMessage;
        public event Action OnClose;
        public event Action<ErrorEventArgs> OnError;

        public WebSocketSession(string url) : this(url, true)
        {
        }

        public WebSocketSession(string url, bool checkCertificateRevocation)
        {
            _session = new WebSocket(url);

            // ★TLS の設定は wss:// のときだけ触る。websocket-sharp の SslConfiguration は
            // 非 TLS（ws://）の接続で読むと InvalidOperationException を投げるので、
            // ws://（Steady の基点を http:// にしたローカルの試験・開発。
            // Gs2WebSocketSession.SteadyWebSocketUrl が作る）では接続を張る前に落ちていた。
            if (_session.IsSecure)
            {
                _session.SslConfiguration.CheckCertificateRevocation = checkCertificateRevocation;
                _session.SslConfiguration.ServerCertificateValidationCallback =
                    (sender, certificate, chain, sslPolicyErrors) => sslPolicyErrors == SslPolicyErrors.None;
            }

            _session.OnOpen += HandleOpen;
            _session.OnMessage += HandleMessage;
            _session.OnClose += HandleClose;
            _session.OnError += HandleError;
        }

        public virtual void Connect()
        {
            try
            {
                _session.ConnectAsync();
            }
            catch (PlatformNotSupportedException)
            {
                _session.Connect();
            }
        }

        public virtual void Close() => _session.Close();

        public virtual void Send(string message)
        {
            var previousSession = _activeSendSession;
            var previousError = _activeSendError;
            _activeSendSession = this;
            _activeSendError = null;
            try
            {
                SendCore(message);
                var sendError = _activeSendError;
                if (sendError != null)
                {
                    throw new InvalidOperationException(sendError.Message, sendError.Exception);
                }
            }
            finally
            {
                _activeSendSession = previousSession;
                _activeSendError = previousError;
            }
        }

        protected virtual void SendCore(string message) => _session.Send(message);

        public virtual void Dispose()
        {
        }

        /// <remarks>
        /// websocket-sharp の Ping() は pong 受信を最大 WaitTime(既定5秒) 同期待ちするため、
        /// 呼び出しスレッド(多くの場合 Unity のメインスレッド)をブロックしないようバックグラウンドで送信する。
        /// 戻り値は「ping の送信を開始した」ことのみを表し、pong 受信の成否は表さない。
        /// </remarks>
        public virtual bool Ping()
        {
            if (_session.ReadyState != WebSocketState.Open)
            {
                return false;
            }
            return StartPing(() => _session.Ping());
        }

        internal bool StartPing(Func<bool> ping)
        {
            if (Interlocked.CompareExchange(ref _pingInFlight, 1, 0) != 0)
            {
                return false;
            }
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        ping();
                    }
                    finally
                    {
                        Volatile.Write(ref _pingInFlight, 0);
                    }
                }).Forget();
                return true;
            }
            catch
            {
                Volatile.Write(ref _pingInFlight, 0);
                throw;
            }
        }

        public virtual StateEnum GetState()
        {
            return _session.ReadyState switch
            {
                WebSocketState.New => StateEnum.Connecting,
                WebSocketState.Connecting => StateEnum.Connecting,
                WebSocketState.Open => StateEnum.Connected,
                WebSocketState.Closing => StateEnum.Closing,
                WebSocketState.Closed => StateEnum.Closed,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void HandleOpen(object sender, EventArgs eventArgs)
        {
            RaiseOpen();
        }

        private void HandleMessage(object sender, MessageEventArgs messageEventArgs)
        {
            if (!messageEventArgs.IsText) {
                return;
            }

            DispatchMessage(messageEventArgs.Data);
        }

        private void HandleClose(object sender, CloseEventArgs e)
        {
            RaiseClose();
        }

        private void HandleError(object sender, ErrorEventArgs errorEventArgs)
        {
            RaiseError(errorEventArgs);
        }

        protected void RaiseOpen() => OnOpen?.Invoke();
        protected void RaiseMessage(string message) => OnMessage?.Invoke(message);
        protected void DispatchMessage(string message)
        {
            try
            {
                RaiseMessage(message);
            }
            catch (System.Exception)
            {
                // websocket-sharp converts subscriber exceptions into transport OnError events.
                // Application callbacks are not transport failures and must not disconnect the session.
            }
        }
        protected void RaiseClose() => OnClose?.Invoke();
        protected void RaiseError(ErrorEventArgs error)
        {
            if (ReferenceEquals(_activeSendSession, this))
            {
                _activeSendError = error;
            }
            OnError?.Invoke(error);
        }
#endif
    }
}

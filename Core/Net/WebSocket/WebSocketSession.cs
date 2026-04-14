#if UNITY_WEBGL && !UNITY_EDITOR
#define GS2_USE_HYBRID_WEBSOCKET
#endif

using System;
#if GS2_USE_HYBRID_WEBSOCKET
using Gs2.HybridWebSocket;
#else
using System.Net.Security;
using Gs2.Util.WebSocketSharp;
#endif

namespace Gs2.Core.Net
{
    public class WebSocketSession
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

        public void Connect() => _session.Connect();

        public void Close() => _session.Close();

        public void Send(string message) => _session.Send(message);

        public bool Ping() => true;

        public StateEnum GetState()
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

        public event Action OnOpen;
        public event Action<string> OnMessage;
        public event Action OnClose;
        public event Action<ErrorEventArgs> OnError;

        public WebSocketSession(string url)
        {
            _session = new WebSocket(url);

            _session.SslConfiguration.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => sslPolicyErrors == SslPolicyErrors.None;

            _session.OnOpen += HandleOpen;
            _session.OnMessage += HandleMessage;
            _session.OnClose += HandleClose;
            _session.OnError += HandleError;
        }

        public void Connect()
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

        public void Close() => _session.Close();

        public void Send(string message) => _session.Send(message);

        public bool Ping() => _session.Ping();

        public StateEnum GetState()
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
            OnOpen?.Invoke();
        }

        private void HandleMessage(object sender, MessageEventArgs messageEventArgs)
        {
            if (!messageEventArgs.IsText) {
                return;
            }

            OnMessage?.Invoke(messageEventArgs.Data);
        }

        private void HandleClose(object sender, CloseEventArgs e)
        {
            OnClose?.Invoke();
        }

        private void HandleError(object sender, ErrorEventArgs errorEventArgs)
        {
            OnError?.Invoke(errorEventArgs);
        }
#endif
    }
}
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using Gs2.Core.Control;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Result;
using Gs2.Core.Util;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
#if UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Gs2.Core.Net
{
    public abstract class Gs2SessionTask<TRequest, TResult>  : TaskFuture<TResult>
        where TRequest : Gs2Request<TRequest>
        where TResult : IResult
    {
        protected abstract IGs2SessionRequest CreateRequest(TRequest request);

        protected Gs2SessionTaskId TaskId { get; set; }

        protected TRequest Request { get; set; }

        protected IGs2Session Session;

        protected virtual TimeSpan RequestTimeout => TimeSpan.FromSeconds(10);

        protected Gs2SessionTask(IGs2Session session, TRequest request)
        {
            this.Session = session;
            Request = request;
            TaskId = Gs2SessionTaskId.Generator.Issue();
        }

#if GS2_ENABLE_UNITASK
        protected override async UniTask<TResult> InvokeImpl()
#else
        protected override async Task<TResult> InvokeImpl()
#endif
        {
            var request = CreateRequest(Request);
            request.TaskId = TaskId;

            // ★見る順は IsDisconnected → IsCanceled。接続が切れた（切れると印が付いた）ときは
            // 必ず SessionNotOpenException で返す。閉じる途中は State が CancellingTasks にもなるので、
            // 先に IsCanceled を見ると切断が「利用者の取り消し」に化け、Unity の Gs2Connection が
            // 繋ぎ直さない（繋ぎ直しの合図は SessionNotOpenException だけ）。
            // 利用者が自分で閉じたときは切断の印が付かないので、これまでどおり UserCancelException になる。
            if (this.Session.IsDisconnected())
            {
                throw new SessionNotOpenException("Session no longer open.");
            }

            if (this.Session.IsCanceled())
            {
                throw new UserCancelException(Array.Empty<RequestError>());
            }

            try
            {
                try
                {
                    Telemetry.StartRequest(request.TaskId, Request);
                }
                catch (System.Exception)
                {
                    // Telemetry observers must not change the request outcome.
                }

                var requestTimer = Stopwatch.StartNew();
                await this.Session.SendAsync(request);

                while (true)
                {
                    if (this.Session is IRequestTrackingSession trackingSession)
                    {
                        var trackingState = trackingSession.GetRequestTrackingState(request);
                        if (trackingState == RequestTrackingState.Completed)
                        {
                            break;
                        }
                        if (trackingState == RequestTrackingState.Abandoned)
                        {
                            throw new SessionNotOpenException("Session no longer open.");
                        }
                    }
                    else if (this.Session.IsCompleted(request))
                    {
                        break;
                    }
                    // ★ここも IsDisconnected を先に見る。サーバーが応答の前に接続を閉じた場合
                    // （gateway の setUserId が自分自身の接続を切る形、ノードの停止、ネットワーク断）、
                    // 待ち中の要求は必ず SessionNotOpenException で終わる。
                    // 以前は切断の後片付けで State が CancellingTasks になった瞬間に当たると
                    // UserCancelException になり、繋ぎ直しが起きなかった（どちらになるかは競争次第）。
                    if (this.Session.IsDisconnected())
                    {
                        throw new SessionNotOpenException("Session no longer open.");
                    }
                    if (this.Session.IsCanceled())
                    {
                        throw new UserCancelException(Array.Empty<RequestError>());
                    }
                    if (requestTimer.Elapsed >= RequestTimeout)
                    {
                        throw new RequestTimeoutException(Array.Empty<RequestError>());
                    }
                    await TaskUtilities.Yield();
                }
                var response = this.Session.MarkRead(request);
                if (response == null)
                {
                    throw new SessionNotOpenException("Session no longer open.");
                }

                try
                {
                    Telemetry.EndRequest(request.TaskId, Request, response);
                }
                catch (System.Exception)
                {
                    // Telemetry observers must not change the request outcome.
                }

                if (response.IsSuccess) {
                    var transactionResult = Gs2.Core.Result.TransactionResult.FromJson(response.Body);
                    if (transactionResult != null) {
                        if (transactionResult.TransactionId != null &&
                            (transactionResult.AutoRunStampSheet ?? false)) {
                            try
                            {
                                Telemetry.StartTransaction(transactionResult.TransactionId, Request);
                            }
                            catch (System.Exception)
                            {
                                // Telemetry observers must not change the request outcome.
                            }
                        }
                    }
                    return (TResult)typeof(TResult).GetMethod("FromJson")?.Invoke(null, new object[] { response.Body });
                }
                else
                {
                    throw response.Error;
                }
            }
            catch
            {
                if (this.Session is IRequestTrackingSession trackingSession)
                {
                    try
                    {
                        trackingSession.Forget(request);
                    }
                    catch (System.Exception)
                    {
                        // Preserve the request failure if transport tracking cleanup also fails.
                    }
                }
                throw;
            }
        }
    }
}

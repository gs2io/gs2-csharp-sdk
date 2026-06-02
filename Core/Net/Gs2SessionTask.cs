using System;
using System.Collections;
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

            if (this.Session.IsCanceled())
            {
                throw new UserCancelException(Array.Empty<RequestError>());
            }

            if (this.Session.IsDisconnected())
            {
                throw new SessionNotOpenException("Session no longer open.");
            }

            Telemetry.StartRequest(request.TaskId, Request);

            await this.Session.SendAsync(request);

            var begin = DateTime.Now;
            while (!this.Session.IsCompleted(request))
            {
                if ((DateTime.Now - begin).Seconds > 10)
                {
                    throw new RequestTimeoutException(Array.Empty<RequestError>());
                }
                if (this.Session.IsCanceled())
                {
                    throw new UserCancelException(Array.Empty<RequestError>());
                }
                await TaskUtilities.Yield();
            }
            var response = this.Session.MarkRead(request);
            
            Telemetry.EndRequest(request.TaskId, Request, response);

            if (response.IsSuccess) {
                var transactionResult = Gs2.Core.Result.TransactionResult.FromJson(response.Body);
                if (transactionResult != null) {
                    if (transactionResult.TransactionId != null && 
                        (transactionResult.AutoRunStampSheet ?? false)) {
                        Telemetry.StartTransaction(transactionResult.TransactionId, Request);
                    }
                }
                return (TResult)typeof(TResult).GetMethod("FromJson")?.Invoke(null, new object[] { response.Body });
            }
            else
            {
                throw response.Error;
            }
        }
    }
}
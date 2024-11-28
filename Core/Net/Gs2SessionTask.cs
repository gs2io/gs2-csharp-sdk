using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Result;
using Gs2.Core.Util;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
#if UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine;
#endif

namespace Gs2.Core.Net
{
    public abstract class Gs2SessionTask<TRequest, TResult>  : TaskFuture<TRequest, TResult>
        where TRequest : IRequest
        where TResult : IResult
    {
        protected abstract IGs2SessionRequest CreateRequest(TRequest request);

        protected new Gs2SessionTaskId TaskId { get; set; }

        protected new TRequest Request { get; set; }

        protected IGs2Session Session;

        protected Gs2SessionTask(IGs2Session session, TRequest request)
        {
            this.Session = session;
            Request = request;
            TaskId = Gs2SessionTaskId.Generator.Issue();
        }

        public override IEnumerator Action()
        {
            var request = CreateRequest(Request);
            request.TaskId = TaskId;
            
            if (this.Session.IsCanceled())
            {
                OnError(new UserCancelException(Array.Empty<RequestError>()));
                yield break;
            }

            if (this.Session.IsDisconnected())
            {
                OnError(new SessionNotOpenException("Session no longer open."));
                yield break;
            }
            
            Telemetry.StartRequest(request.TaskId, Request);

            yield return this.Session.Send(request);
            var begin = DateTime.Now;
            while (!this.Session.IsCompleted(request))
            {
                if ((DateTime.Now - begin).Seconds > 10)
                {
                    OnError(new RequestTimeoutException(Array.Empty<RequestError>()));
                    yield break;
                }
                if (this.Session.IsCanceled())
                {
                    OnError(new UserCancelException(Array.Empty<RequestError>()));
                    yield break;
                }

#if UNITY_WEBGL && !UNITY_EDITOR
                yield return new WaitForSeconds(0.005f);
#endif
            }
            
            var response = this.Session.MarkRead(request);
            
            Telemetry.EndRequest(request.TaskId, Request, response);

            if (response.IsSuccess)
            {
                var transactionResult = Gs2.Core.Result.TransactionResult.FromJson(response.Body);
                if (transactionResult != null) {
                    if (transactionResult.TransactionId != null && 
                        (transactionResult.AutoRunStampSheet ?? false)) {
                        Telemetry.StartTransaction(transactionResult.TransactionId, Request);
                    }
                }
                OnComplete((TResult)typeof(TResult).GetMethod("FromJson")?.Invoke(null, new object[] { response.Body }));
            }
            else
            {
                OnError(response.Error);
            }
        }

        public override async Task<TResult> Invoke()
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

#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            this.Session.Send(request);
#else
            await this.Session.SendAsync(request);
#endif
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
                await Task.Delay(5);
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
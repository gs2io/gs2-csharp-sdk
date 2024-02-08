using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif

namespace Gs2.Core.Net
{
    public abstract class Gs2SessionTask<TRequest, TResult>  : TaskFuture<TRequest, TResult>
        where TRequest : IRequest
        where TResult : IResult
    {
        protected abstract IGs2SessionRequest CreateRequest(TRequest request);
        public new Gs2SessionTaskId TaskId { get; set; }
        public new TRequest Request { get; set; }

        protected IGs2Session Session;
        
        public Gs2SessionTask(IGs2Session session, TRequest request)
        {
            Session = session;
            Request = request;
            TaskId = Gs2SessionTaskId.Generator.Issue();
        }

        public override IEnumerator Action()
        {
            var request = CreateRequest(Request);
            request.TaskId = TaskId;
            
            if (Session.IsCanceled())
            {
                OnError(new UserCancelException(new RequestError[0]));
                yield break;
            }

            if (Session.IsDisconnected())
            {
                OnError(new SessionNotOpenException("Session no longer open."));
                yield break;
            }

            yield return Session.Send(request);
            var begin = DateTime.Now;
            while (!Session.IsCompleted(request))
            {
                if ((DateTime.Now - begin).Seconds > 10)
                {
                    OnError(new RequestTimeoutException(new RequestError[0]));
                    yield break;
                }
                if (Session.IsCanceled())
                {
                    OnError(new UserCancelException(new RequestError[0]));
                    yield break;
                }

#if UNITY_WEBGL && !UNITY_EDITOR
                yield return new WaitForSeconds(0.05f);
#endif
            }
            var response = Session.MarkRead(request);
            if (response.IsSuccess)
            {
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

            if (Session.IsCanceled())
            {
                throw new UserCancelException(new RequestError[0]);
            }

            if (Session.IsDisconnected())
            {
                throw new SessionNotOpenException("Session no longer open.");
            }

#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            Session.Send(request);
#else
            await Session.SendAsync(request);
#endif
            var begin = DateTime.Now;
            while (!Session.IsCompleted(request))
            {
                if ((DateTime.Now - begin).Seconds > 10)
                {
                    throw new RequestTimeoutException(new RequestError[0]);
                }
                if (Session.IsCanceled())
                {
                    throw new UserCancelException(new RequestError[0]);
                }
                await Task.Delay(50);
            }
            var response = Session.MarkRead(request);
            if (response.IsSuccess)
            {
                return (TResult)typeof(TResult).GetMethod("FromJson")?.Invoke(null, new object[] { response.Body });
            }
            else
            {
                throw response.Error;
            }
        }
    }
}
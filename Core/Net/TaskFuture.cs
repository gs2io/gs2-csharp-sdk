using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;

namespace Gs2.Core.Net
{
    public abstract class TaskFuture<TRequest, TResult> : ITaskFuture<TRequest, TResult>
        where TRequest : IRequest
        where TResult : IResult
    {
        public Gs2SessionTaskId TaskId { get; set; }
        public TRequest Request { get; set; }
        public TResult Result { get; set; }
        public Gs2Exception Error { get; set; }
        
        private readonly IEnumerator _inflightAction;

        public TaskFuture()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            _inflightAction = Action();
        }

        public abstract IEnumerator Action();

        public abstract Task<TResult> Invoke();

        public bool MoveNext()
        {
            return _inflightAction?.MoveNext() ?? false;
        }

        public void Reset()
        {
            _inflightAction?.Reset();
        }

        public object Current => _inflightAction?.Current;

        public bool IsComplete()
        {
            return Result != null || Error != null;
        }

        public virtual void OnError(Gs2Exception error)
        {
            Error = error;
        }

        public virtual void OnComplete(TResult result)
        {
            Result = result;
        }
    }
}
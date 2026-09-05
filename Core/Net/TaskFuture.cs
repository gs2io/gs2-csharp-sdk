using System;
using System.Collections;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Util;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
    #if UNITY_2017_1_OR_NEWER
using System.Runtime.CompilerServices;
    #endif
#endif

namespace Gs2.Core.Net
{
    public abstract class TaskFuture<TResult> : ITaskFuture<TResult>
        where TResult : IResult
    {
        public TResult Result { get; private set; }
        public Gs2Exception Error { get; private set; }

        private volatile bool _isInvoked;
        private volatile bool _isOnProgress;
        private volatile bool _isComplete;

#if GS2_ENABLE_UNITASK
        private readonly AsyncLazy<TResult> _invokeTask;
#else
        private readonly Lazy<Task<TResult>> _invokeTask;
#endif

        protected TaskFuture()
        {
#if GS2_ENABLE_UNITASK
            _invokeTask = UniTask.Lazy(InvokeOnce);
#else
            _invokeTask = new Lazy<Task<TResult>>(InvokeOnce);
#endif
        }

        public IEnumerator Action() => Invoke().ToCoroutine((Action<AsyncResult<TResult>>)null);

#if GS2_ENABLE_UNITASK
        public UniTask<TResult> Invoke() => _invokeTask.Task;
#else
        public Task<TResult> Invoke() => _invokeTask.Value;
#endif

#if GS2_ENABLE_UNITASK
        private async UniTask<TResult> InvokeOnce()
#else
        private async Task<TResult> InvokeOnce()
#endif
        {
            try
            {
                _isInvoked = true;
                _isOnProgress = true;
                OnComplete(await InvokeImpl());
                return Result;
            }
            catch (System.Exception exception)
            {
                OnError(exception as Gs2Exception ?? new UnknownException(exception.Message, exception));
                throw Error;
            }
            finally
            {
                _isOnProgress = false;
            }
        }

#if GS2_ENABLE_UNITASK
        protected abstract UniTask<TResult> InvokeImpl();
#else
        protected abstract Task<TResult> InvokeImpl();
#endif

        public bool MoveNext()
        {
            if (!_isInvoked) Invoke().Forget();
            return _isOnProgress;
        }

        public void Reset()
        {
            throw new InvalidOperationException();
        }

        public object Current => null;

        public bool IsComplete()
        {
            return _isComplete;
        }

        public virtual void OnError(Gs2Exception error)
        {
            Error = error;
            _isComplete = true;
        }

        public virtual void OnComplete(TResult result)
        {
            Result = result;
            _isComplete = true;
        }

#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
        public TaskAwaiter<TResult> GetAwaiter() => Invoke().GetAwaiter();
#endif
    }
}

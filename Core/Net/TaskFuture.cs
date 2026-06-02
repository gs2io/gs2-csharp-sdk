using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Util;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Gs2.Core.Net
{
    public abstract class TaskFuture<TResult> : ITaskFuture<TResult>
        where TResult : IResult
    {
        public TResult Result { get; private set; }
        public Gs2Exception Error { get; private set; }

        private bool _isInvoked;
        private bool _isOnProgress;

        public IEnumerator Action() => Invoke().ToCoroutine((Action<AsyncResult<TResult>>)null);

#if GS2_ENABLE_UNITASK
        public async UniTask<TResult> Invoke()
#else
        public async Task<TResult> Invoke()
#endif
        {
            try
            {
                _isInvoked = true;
                _isOnProgress = true;
                OnComplete(await InvokeImpl());
                return Result;
            }
            catch (Gs2Exception gs2Exception)
            {
                OnError(gs2Exception);
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
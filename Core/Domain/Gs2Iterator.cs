using System;
using System.Collections;
using Gs2.Core.Exception;

namespace Gs2.Core.Domain
{
    public abstract class Gs2Iterator<TResult>
    {
        public TResult Current;
        public Gs2Exception Error;

        public abstract bool HasNext();
        protected abstract IEnumerator Next(Action<AsyncResult<TResult>> callback);
        
        public IEnumerator Next()
        {
            if (HasNext()) {
                AsyncResult<TResult> result = null;
                yield return Next(r => result = r);
                if (result == null) {
                    Current = default;
                    Error = default;
                    yield break;
                }
                if (result.Error != null) {
                    Current = default;
                    Error = result.Error;
                    yield break;
                }
                Current = result.Result;
                Error = default;
            }
            else
            {
                Current = default;
                Error = default;
            }
        }
    }
}
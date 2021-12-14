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
        protected abstract IEnumerator Next(Action<TResult> callback);
        
        public IEnumerator Next()
        {
            if (HasNext())
            {
                yield return Next(r => Current = r);
            }
            else
            {
                Current = default;
            }
        }
    }
}
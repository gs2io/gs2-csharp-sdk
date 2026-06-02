using System.Collections;
using Gs2.Core.Exception;
using Gs2.Core.Model;

namespace Gs2.Core.Net
{
    public interface IFuture<TResult> : IEnumerator
    {
        TResult Result { get; }
        Gs2Exception Error { get; }

        bool IsComplete();
        void OnError(Gs2Exception error);
        void OnComplete(TResult result);
    }
}
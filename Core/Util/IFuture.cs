using System.Collections;
using Gs2.Core.Exception;
using Gs2.Core.Model;

namespace Gs2.Core.Net
{
    public interface IFuture<TResult> : IEnumerator
    {
        TResult Result { set; get; }
        Gs2Exception Error { set; get; }

        IEnumerator Action();

        bool IsComplete();
        void OnError(Gs2Exception error);
        void OnComplete(TResult result);
    }
}
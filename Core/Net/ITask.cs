using System.Threading.Tasks;
using Gs2.Core.Model;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Gs2.Core.Net
{
    public interface ITask<TResult> 
        where TResult : IResult
    {
#if GS2_ENABLE_UNITASK
        UniTask<TResult> Invoke();
#else
        Task<TResult> Invoke();
#endif
    }

    public interface ITaskFuture<TResult> : ITask<TResult>, IFuture<TResult>
        where TResult : IResult
    {
        
    }
}
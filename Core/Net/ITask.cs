using System.Threading.Tasks;
using Gs2.Core.Model;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Gs2.Core.Net
{
    public interface ITask<TRequest, TResult> 
        where TRequest : IRequest
        where TResult : IResult
    {
        Gs2SessionTaskId TaskId { set; get; }
        TRequest Request { set; get; }

#if GS2_ENABLE_UNITASK
        UniTask<TResult> Invoke();
#else
        Task<TResult> Invoke();
#endif
    }

    public interface ITaskFuture<TRequest, TResult> : ITask<TRequest, TResult>, IFuture<TResult>
        where TRequest : IRequest
        where TResult : IResult
    {
        
    }
}
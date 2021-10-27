using System.Threading.Tasks;
using Gs2.Core.Model;

namespace Gs2.Core.Net
{
    public interface ITask<TRequest, TResult> 
        where TRequest : IRequest
        where TResult : IResult
    {
        Gs2SessionTaskId TaskId { set; get; }
        TRequest Request { set; get; }

        Task<TResult> Invoke();
    }

    public interface ITaskFuture<TRequest, TResult> : ITask<TRequest, TResult>, IFuture<TResult>
        where TRequest : IRequest
        where TResult : IResult
    {
        
    }
}
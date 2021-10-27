using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Util.LitJson;

namespace Gs2.Core.Net
{
    public interface IGs2SessionResult : IResult
    {
        int StatusCode { set; get; }
        JsonData Body { set; get; }
        Gs2Exception Error { set; get; }
        Gs2SessionTaskId Gs2SessionTaskId { set; get; }

        bool IsSuccess { get; }
    }
}
using Gs2.Core.Model;

namespace Gs2.Core.Net
{
    public interface IGs2SessionRequest
    {
        Gs2SessionTaskId TaskId { set; get; }
    }
}
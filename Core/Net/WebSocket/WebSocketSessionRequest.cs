namespace Gs2.Core.Net
{
    public class WebSocketSessionRequest : IGs2SessionRequest
    {
        public Gs2SessionTaskId TaskId { set; get; }
        public string Body { set; get; }
    }
}
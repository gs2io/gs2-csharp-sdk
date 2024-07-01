namespace Gs2.Core.Net
{
    public static class WebSocketSessionRequestFactory
    {
        public static T New<T>(string body) where T : WebSocketSessionRequest, new()
        {
            var request = new T {
                Body = body
            };
            return request;
        }
    }
}
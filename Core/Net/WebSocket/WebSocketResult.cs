using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Core.Net
{
    public class WebSocketResult : IGs2SessionResult
    {
        public int StatusCode { set; get; }
        public JsonData Body { set; get; }
        public Gs2Exception Error { set; get; }
        public Gs2SessionTaskId Gs2SessionTaskId { set; get; }
        public bool IsSuccess => StatusCode == 200;

#if UNITY_2017_1_OR_NEWER
        [Preserve]
#endif
        private class Gs2Message
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Type { private set; get; }
		    
            /** Gs2SessionTaskId */
            public string RequestId { private set; get; }

            /** HTTP ステータスコード */
            public int? Status { private set; get; }
	        
            /** メッセージ本体 */
            public JsonData Body { private set; get; }

            public static Gs2Message FromJson(JsonData data)
            {
                if (data == null)
                {
                    return new Gs2Message();
                }
                return new Gs2Message
                {
                    Type = data.Keys.Contains("type") ? (string)data["type"] : null,
                    RequestId = data.Keys.Contains("requestId") ? (string)data["requestId"] : null,
                    Status = data.Keys.Contains("status") ? (int?)data["status"] : null,
                    Body = data.Keys.Contains("body") ? data["body"] : null,
                };
            }
        }

        public WebSocketResult(string body)
        {
            var gs2Message = Gs2Message.FromJson(JsonMapper.ToObject(body));
            Body = gs2Message.Body;

            if (gs2Message.Status != 200 && gs2Message.Body != null)
            {
                var error = GeneralError.FromJson(gs2Message.Body);
                var errorMessage = error != null ? error.Message : body;
                Error = ExtractError(errorMessage, gs2Message.Status ?? 0);
            }

            Gs2SessionTaskId = new Gs2SessionTaskId(gs2Message.RequestId);
            StatusCode = gs2Message.Status ?? 0;
            Body = gs2Message.Body;
        }
        
        private static Gs2Exception ExtractError(string message, long statusCode)
        {
            switch (statusCode)
            {
                case 0:    // インターネット非接続のときに UnityWebRequest のステータスコードは 0 になる
                    return new NoInternetConnectionException(message);
                case 200:
                    return null;
                case 400:
                    return new BadRequestException(message);
                case 401:
                    return new UnauthorizedException(message);
                case 402:
                    return new QuotaLimitExceededException(message);
                case 404:
                    return new NotFoundException(message);
                case 409:
                    return new ConflictException(message);
                case 500:
                    return new InternalServerErrorException(message);
                case 502:
                    return new BadGatewayException(message);
                case 503:
                    return new ServiceUnavailableException(message);
                case 504:
                    return new RequestTimeoutException(message);
                default:
                    return new UnknownException(message);
            }
        }
    }
}
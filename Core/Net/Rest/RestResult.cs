using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Util.LitJson;

namespace Gs2.Core.Net
{
    public class RestResult : IGs2SessionResult
    {
        public int StatusCode { set; get; }
        public JsonData Body { get; set; }
        public Gs2Exception Error { set; get; }
        public Gs2SessionTaskId Gs2SessionTaskId { get; set; }
        public bool IsSuccess => StatusCode == 200;


        public RestResult(int statusCode, string body)
        {
            try {
                if (statusCode != 200) {
                    var error = GeneralError.FromJson(JsonMapper.ToObject(body));
                    var errorMessage = error != null ? error.Message : body;
                    Error = Gs2Exception.ExtractError(errorMessage, statusCode);
                    if (Error != null) {
                        Error.Metadata = error?.Metadata;
                    }
                }

                StatusCode = statusCode;
                Body = JsonMapper.ToObject(body);
            }
            catch (JsonException e) {
                throw new UnknownException(new [] {
                    new RequestError(
                        "client",
                        "core.network.result.error.parse.failed"
                    )
                }, e);
            }
        }
    }
}
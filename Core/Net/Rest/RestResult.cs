using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Networking;
#endif

namespace Gs2.Core.Net
{
    public class RestResult : IGs2SessionResult
    {
        public int StatusCode { set; get; }
        public JsonData Body { get; set; }
        public Gs2Exception Error { set; get; }
        public Gs2SessionTaskId Gs2SessionTaskId { get; set; }
        public bool IsSuccess => StatusCode == 200;


        public RestResult(int statusCode, string body, int requestResult = 0, string requestError = "")
        {
            try {
#if UNITY_2017_1_OR_NEWER
                if (statusCode != 200) {
                    if (statusCode == 0)
                    {
                        switch (requestResult)
                        {
                            case (int) UnityWebRequest.Result.ConnectionError:
                                Error = new ConnectionException(new [] {
                                    new RequestError("unityWebRequest", requestError),
                                });
                                break;
                            case (int) UnityWebRequest.Result.DataProcessingError:
                                Error = new DataProcessingException(new [] {
                                    new RequestError("unityWebRequest", requestError),
                                });
                                break;
                        }
                    }
                    else
                    {
                        var error = GeneralError.FromJson(JsonMapper.ToObject(body));
                        var errorMessage = error != null ? error.Message : body;
                        Error = Gs2Exception.ExtractError(errorMessage, statusCode);
                        if (Error != null) {
                            Error.Metadata = error?.Metadata;
                        }
                    }
                }
                else
                {
                    StatusCode = statusCode;
                    Body = JsonMapper.ToObject(body);
                }
#else
                if (statusCode != 200) {
                    if (statusCode == 0)
                    {
                        Error = new HttpRequestException(new[] {
                            new RequestError("httpClient", requestError),
                        });
                    }
                    else
                    {
                        var error = GeneralError.FromJson(JsonMapper.ToObject(body));
                        var errorMessage = error != null ? error.Message : body;
                        Error = Gs2Exception.ExtractError(errorMessage, statusCode);
                        if (Error != null)
                        {
                            Error.Metadata = error?.Metadata;
                        }
                    }
                }

                StatusCode = statusCode;
                Body = JsonMapper.ToObject(body);
#endif
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
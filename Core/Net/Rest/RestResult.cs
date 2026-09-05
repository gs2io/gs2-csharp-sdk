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
        public bool IsSuccess => StatusCode == 200 && Error == null;


        public RestResult(int statusCode, string body, int requestResult = 0, string requestError = "")
        {
            try {
                StatusCode = statusCode;
#if UNITY_2017_1_OR_NEWER
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
#endif
                if (Error == null)
                {
                    Body = statusCode == 0 || string.IsNullOrEmpty(body)
                        ? null
                        : JsonMapper.ToObject(body);

                    if (statusCode != 200)
                    {
                        if (statusCode == 0)
                        {
#if UNITY_2017_1_OR_NEWER
                            Error = new ConnectionException(new [] {
                                new RequestError("unityWebRequest", requestError),
                            });
#else
                            Error = new HttpRequestException(new[] {
                                new RequestError("httpClient", requestError),
                            });
#endif
                        }
                        else
                        {
                            var error = ParseError(Body);
                            var errorMessage = error != null ? error.Message : body;
                            Error = Gs2Exception.ExtractError(errorMessage, statusCode);
                            if (Error != null) {
                                Error.Metadata = error?.Metadata;
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) when (
                e is JsonException ||
                e is System.InvalidOperationException ||
                e is System.InvalidCastException
            ) {
                throw new UnknownException(new [] {
                    new RequestError(
                        "client",
                        "core.network.result.error.parse.failed"
                    )
                }, e);
            }
        }

        private static GeneralError ParseError(JsonData body)
        {
            if (body == null)
            {
                return null;
            }
            if (!body.IsObject)
            {
                throw new System.InvalidOperationException("Error response body was not a JSON object.");
            }
            if (!body.Keys.Contains("message") ||
                body["message"] == null ||
                !body["message"].IsString)
            {
                throw new System.InvalidOperationException("Error response message was not a string.");
            }
            if (body.Keys.Contains("metadata") &&
                body["metadata"] != null &&
                !body["metadata"].IsObject)
            {
                throw new System.InvalidOperationException("Error response metadata was not a JSON object.");
            }
            return GeneralError.FromJson(body);
        }
    }
}

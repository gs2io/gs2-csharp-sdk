using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Networking;
#endif

namespace Gs2.Core.Net
{
    /// <summary>
    /// 要求が transport の段階でどう失敗したか。Steady の基点宛の要求を 1 回だけ再送するかの判断
    /// （<see cref="Gs2RestSession.SendAsync"/>）に使う。
    /// </summary>
    public enum TransportFailure
    {
        /// <summary>transport の失敗ではない（HTTP 応答を受け取った）。</summary>
        None,
        /// <summary>接続段階の失敗（DNS / TCP 拒否 / TLS）。1 バイトも送っていないので同じ要求を再送してよい。</summary>
        ConnectFailed,
        /// <summary>タイムアウト。届いたかもしれないので冪等な GET / DELETE だけ再送してよい。</summary>
        Timeout,
        /// <summary>その他の transport の失敗（送信後の切断など）。再送しない。</summary>
        Other,
    }

    public class RestResult : IGs2SessionResult
    {
        public int StatusCode { set; get; }
        public JsonData Body { get; set; }
        public Gs2Exception Error { set; get; }
        public Gs2SessionTaskId Gs2SessionTaskId { get; set; }
        public bool IsSuccess => StatusCode == 200 && Error == null;

        /// <summary>
        /// transport の失敗の種類。HTTP 応答を受け取れなかった（StatusCode == 0）ときの既定は
        /// <see cref="TransportFailure.Other"/> で、transport の実装（DotNetRestSessionRequest /
        /// UnityRestSessionRequest）が分類できたときに後から上書きする。
        /// </summary>
        public TransportFailure TransportFailure { get; set; }


        public RestResult(int statusCode, string body, int requestResult = 0, string requestError = "")
        {
            try {
                StatusCode = statusCode;
                TransportFailure = statusCode == 0 ? TransportFailure.Other : TransportFailure.None;
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

#if UNITY_2017_1_OR_NEWER
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gs2.Core.Util;
using UnityEngine.Networking;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
using UnityEngine;
#endif

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Gs2.Core.Net
{
    public static class HttpMethodUnityExt
    {
        public static string TransformUnity(this HttpMethod self) {
            return self switch {
                HttpMethod.Get => UnityWebRequest.kHttpVerbGET,
                HttpMethod.Post => UnityWebRequest.kHttpVerbPOST,
                HttpMethod.Put => UnityWebRequest.kHttpVerbPUT,
                HttpMethod.Delete => UnityWebRequest.kHttpVerbDELETE,
                _ => UnityWebRequest.kHttpVerbGET
            };
        }
    }

    [Obsolete("This handler accepts every certificate and must not be used in production.")]
    public class DisabledCertificateHandler : CertificateHandler {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
    
    public class UnityRestSessionRequest : RestSessionRequest
    {
        private readonly CertificateHandler _certificateHandler;
        private readonly object _requestLock = new object();
        private UnityWebRequest _inflightRequest;

        public UnityRestSessionRequest()
        {
        }

        public UnityRestSessionRequest(bool checkCertificateRevocation = true)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            Gs2RestSession.ValidateCertificateRevocationConfiguration(
                checkCertificateRevocation,
                false
            );
#endif
        }

        public UnityRestSessionRequest(CertificateHandler certificateHandler = null)
        {
            this._certificateHandler = certificateHandler;
        }

        private static byte[] Compress(byte[] data)
        {
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        public override async Task<RestResult> Invoke()
        {
#if UNITY_2017_1_OR_NEWER
            var uri = QueryStrings.Count == 0 ?
                Url :
                Url + '?' + string.Join("&", QueryStrings.Select(
                    item => $"{item.Key}={UnityWebRequest.EscapeURL(item.Value)}").ToArray());
            using var request = new UnityWebRequest(
                uri,
                Method.TransformUnity()
            );
            lock (_requestLock)
            {
                _inflightRequest = request;
            }
            try
            {
            request.downloadHandler = new DownloadHandlerBuffer();
            foreach (var item in Headers.Where(item => (Method != HttpMethod.Post && Method != HttpMethod.Put) || item.Key.ToLower() != "content-type")) {
                request.SetRequestHeader(item.Key, item.Value);
            }

            request.SetRequestHeader("Content-Type", "application/json");

            if (EnableResponseDecompression)
            {
                request.SetRequestHeader("Accept-Encoding", "gzip");
            }

            if (Method == HttpMethod.Post || Method == HttpMethod.Put)
            {
                var bodyBytes = Encoding.UTF8.GetBytes(Body);
                if (EnableRequestCompression)
                {
                    bodyBytes = Compress(bodyBytes);
                    request.SetRequestHeader("Content-Encoding", "gzip");
                }
                request.uploadHandler = new UploadHandlerRaw(bodyBytes);
            }

            if (this._certificateHandler != null)
            {
                request.certificateHandler = this._certificateHandler;
                request.disposeCertificateHandlerOnDispose = false;
            }

            try {
                await request.SendWebRequest();
            }
#if GS2_ENABLE_UNITASK
            catch (UnityWebRequestException) {}
#endif
            finally {}

            RestResult result = null;
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                case UnityWebRequest.Result.ProtocolError:
                    result = new RestResult(
                        (int) request.responseCode,
                        request.downloadHandler?.text
                    );
                    break;

                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    result = new RestResult(
                        (int) request.responseCode,
                        null,
                        (int) request.result,
                        request.error
                    );
                    if (request.result == UnityWebRequest.Result.ConnectionError)
                    {
                        result.TransportFailure = ClassifyTransportFailure(
                            request.responseCode,
                            request.uploadedBytes,
                            request.error
                        );
                    }
                    break;
            }

            return result;
            }
            finally
            {
                lock (_requestLock)
                {
                    if (ReferenceEquals(_inflightRequest, request))
                    {
                        _inflightRequest = null;
                    }
                }
            }
#else
            throw new NotImplementedException();
#endif
        }

        public override void Abort()
        {
            lock (_requestLock)
            {
                _inflightRequest?.Abort();
            }
        }

        /// <summary>
        /// UnityWebRequest の ConnectionError を「1 バイトも送っていない」接続段階の失敗とそれ以外に分ける。
        /// ★UnityWebRequest には接続専用の失敗種別も接続専用のタイムアウトも無いので、
        /// 応答コード 0・送信 0 バイト・接続系の文言（"Cannot resolve destination host" /
        /// "Cannot connect to destination host" / SSL / certificate）で近似する。"Request timeout" は Timeout。
        /// </summary>
        internal static TransportFailure ClassifyTransportFailure(long responseCode, ulong uploadedBytes, string error)
        {
            if (error != null && error.IndexOf("Request timeout", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return TransportFailure.Timeout;
            }
            if (responseCode != 0 || uploadedBytes != 0 || error == null)
            {
                return TransportFailure.Other;
            }
            if (error.IndexOf("Cannot resolve destination host", StringComparison.OrdinalIgnoreCase) >= 0 ||
                error.IndexOf("Cannot connect to destination host", StringComparison.OrdinalIgnoreCase) >= 0 ||
                error.IndexOf("Unable to complete SSL connection", StringComparison.OrdinalIgnoreCase) >= 0 ||
                error.IndexOf("SSL", StringComparison.OrdinalIgnoreCase) >= 0 ||
                error.IndexOf("certificate", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return TransportFailure.ConnectFailed;
            }
            return TransportFailure.Other;
        }
    }
}

#endif

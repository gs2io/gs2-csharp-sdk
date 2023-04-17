#if UNITY_2017_1_OR_NEWER
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
using Gs2.Core.Exception;
#endif
using UnityEngine.Networking;

namespace Gs2.Core.Net
{
    public static class HttpMethodUnityExt
    {
        public static string TransformUnity(this HttpMethod self)
        {
            switch (self)
            {
                case HttpMethod.Get:
                    return UnityWebRequest.kHttpVerbGET;
                case HttpMethod.Post:
                    return UnityWebRequest.kHttpVerbPOST;
                case HttpMethod.Put:
                    return UnityWebRequest.kHttpVerbPUT;
                case HttpMethod.Delete:
                    return UnityWebRequest.kHttpVerbDELETE;
            }
            return UnityWebRequest.kHttpVerbGET;
        }
    }

    public class DisabledCertificateHandler : CertificateHandler {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
    
    public class UnityRestSessionRequest : RestSessionRequestFuture
    {
		CertificateHandler _certificateHandler = null;
        public bool _checkCertificateRevocation { get; } = true;

        public UnityRestSessionRequest()
        {
        }
        
        public UnityRestSessionRequest(bool checkCertificateRevocation = true)
        {
            _checkCertificateRevocation = checkCertificateRevocation;
        }
        
        public UnityRestSessionRequest(CertificateHandler certificateHandler = null)
        {
            _certificateHandler = certificateHandler;
        }
        
        public override async Task<RestResult> Invoke()
        {
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
            var uri = QueryStrings.Count == 0 ? 
                Url : 
                Url + '?' + string.Join("&", QueryStrings.Select(
                    item => $"{item.Key}={UnityWebRequest.EscapeURL(item.Value)}").ToArray());
            var contentType = Headers.Where(item => item.Key.ToLower() == "content-type").Select(item => item.Value).FirstOrDefault();
            using (var request = new UnityWebRequest(
                       uri,
                       Method.TransformUnity()
                   ))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                foreach (var item in Headers)
                {
                    if ((Method == HttpMethod.Post || Method == HttpMethod.Put) && item.Key.ToLower() == "content-type")
                    {
                        continue;
                    }

                    request.SetRequestHeader(item.Key, item.Value);
                }

                request.SetRequestHeader("Content-Type", "application/json");

                if (Method == HttpMethod.Post || Method == HttpMethod.Put)
                {
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(Body));
                }

                if (_certificateHandler != null)
                    request.certificateHandler = _certificateHandler;
                else if (!_checkCertificateRevocation)
                    request.certificateHandler = new DisabledCertificateHandler();

                await request.SendWebRequest();

                if (request.responseCode == 500)
                {
                    return await Invoke();
                }

                var result = new RestResult(
                    (int) request.responseCode,
                    request.downloadHandler.text
                );
                OnComplete(result);

                request.Dispose();
                
                return result;
            }
#else
            throw new NotImplementedException();
#endif
        }

        public override IEnumerator Action()
        {
            var uri = QueryStrings.Count == 0 ? 
                Url : 
                Url + '?' + string.Join("&", QueryStrings.Select(
                    item => $"{item.Key}={UnityWebRequest.EscapeURL(item.Value)}").ToArray());
            var contentType = Headers.Where(item => item.Key.ToLower() == "content-type").Select(item => item.Value).FirstOrDefault();
            using (var request = new UnityWebRequest(
                       uri,
                       Method.TransformUnity()
                   ))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                foreach (var item in Headers)
                {
                    if ((Method == HttpMethod.Post || Method == HttpMethod.Put) && item.Key.ToLower() == "content-type")
                    {
                        continue;
                    }

                    request.SetRequestHeader(item.Key, item.Value);
                }

                request.SetRequestHeader("Content-Type", "application/json");

                if (Method == HttpMethod.Post || Method == HttpMethod.Put)
                {
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(Body));
                }

                if (_certificateHandler != null)
                    request.certificateHandler = _certificateHandler;
                else if (!_checkCertificateRevocation)
                    request.certificateHandler = new DisabledCertificateHandler();

                yield return request.SendWebRequest();
                if (request.responseCode == 500)
                {
                    yield return Action();
                    yield break;
                }

                OnComplete(new RestResult(
                    (int) request.responseCode,
                    request.downloadHandler.text
                ));

                request.Dispose();
            }
        }
    }
}

#endif
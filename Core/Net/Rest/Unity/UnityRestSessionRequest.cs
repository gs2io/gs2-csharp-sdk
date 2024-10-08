#if UNITY_2017_1_OR_NEWER
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class DisabledCertificateHandler : CertificateHandler {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
    
    public class UnityRestSessionRequest : RestSessionRequestFuture
    {
        private readonly CertificateHandler _certificateHandler;
        private readonly bool _checkCertificateRevocation;

        public UnityRestSessionRequest()
        {
        }
        
        public UnityRestSessionRequest(bool checkCertificateRevocation = true)
        {
            this._checkCertificateRevocation = checkCertificateRevocation;
        }
        
        public UnityRestSessionRequest(CertificateHandler certificateHandler = null)
        {
            this._certificateHandler = certificateHandler;
            this._checkCertificateRevocation = true;
        }
        
        public override async Task<RestResult> Invoke()
        {
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
            var uri = QueryStrings.Count == 0 ? 
                Url : 
                Url + '?' + string.Join("&", QueryStrings.Select(
                    item => $"{item.Key}={UnityWebRequest.EscapeURL(item.Value)}").ToArray());
            using var request = new UnityWebRequest(
                uri,
                Method.TransformUnity()
            );
            request.downloadHandler = new DownloadHandlerBuffer();
            foreach (var item in Headers.Where(item => (Method != HttpMethod.Post && Method != HttpMethod.Put) || item.Key.ToLower() != "content-type")) {
                request.SetRequestHeader(item.Key, item.Value);
            }

            request.SetRequestHeader("Content-Type", "application/json");

            if (Method == HttpMethod.Post || Method == HttpMethod.Put)
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(Body));
            }

            if (this._certificateHandler != null)
                request.certificateHandler = this._certificateHandler;
            else if (!this._checkCertificateRevocation)
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

            // ReSharper disable once DisposeOnUsingVariable
            request.Dispose();
                
            return result;
#else
            throw new NotImplementedException();
#endif
        }

        public override IEnumerator Action() {
            
            var uri = QueryStrings.Count == 0 ? 
                Url : 
                Url + '?' + string.Join("&", QueryStrings.Select(
                    item => $"{item.Key}={UnityWebRequest.EscapeURL(item.Value)}").ToArray());
            using var request = new UnityWebRequest(
                uri,
                Method.TransformUnity()
            );
            request.downloadHandler = new DownloadHandlerBuffer();
            foreach (var item in Headers.Where(item => (Method != HttpMethod.Post && Method != HttpMethod.Put) || item.Key.ToLower() != "content-type")) {
                request.SetRequestHeader(item.Key, item.Value);
            }

            request.SetRequestHeader("Content-Type", "application/json");

            if (Method == HttpMethod.Post || Method == HttpMethod.Put)
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(Body));
            }

            if (this._certificateHandler != null)
                request.certificateHandler = this._certificateHandler;
            else if (!this._checkCertificateRevocation)
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

            // ReSharper disable once DisposeOnUsingVariable
            request.Dispose();
        }
    }
}

#endif
#if UNITY_2017_1_OR_NEWER
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class UnityRestSessionRequest : RestSessionRequestFuture
    {
        public override async Task<RestResult> Invoke()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator Action()
        {
            var uri = QueryStrings.Count == 0 ? 
                Url : 
                Url + '?' + string.Join("&", QueryStrings.Select(
                    item => $"{item.Key}={UnityWebRequest.EscapeURL(item.Value)}").ToArray());
            var contentType = Headers.Where(item => item.Key.ToLower() == "content-type").Select(item => item.Value).FirstOrDefault();
            var request = new UnityWebRequest(
                uri,
                Method.TransformUnity()
            );
            request.downloadHandler = new DownloadHandlerBuffer();
            foreach (var item in Headers)
            {
                if ((Method == HttpMethod.Post || Method == HttpMethod.Put) && item.Key.ToLower() == "content-type")
                {
                    continue;
                }

                request.SetRequestHeader(item.Key, item.Value);
            }

            if (Method == HttpMethod.Post || Method == HttpMethod.Put)
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(Body));
            }

            yield return request.SendWebRequest();

            OnComplete(new RestResult(
                (int) request.responseCode,
                request.downloadHandler.text
            ));
        }
    }
}

#endif
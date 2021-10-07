using System.Collections;
using UnityEngine.Networking;

namespace Gs2.Core.Net
{
    public abstract class Gs2RestTask
    {
        public UnityWebRequest UnityWebRequest { get; } = new UnityWebRequest();

        public Gs2RestTask()
        {
        }

        public IEnumerator Send()
        {
            UnityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return UnityWebRequest.SendWebRequest();

            Callback(
                new Gs2RestResponse(
#if UNITY_2020_1_OR_NEWER
                    UnityWebRequest.result != UnityWebRequest.Result.ConnectionError ? UnityWebRequest.downloadHandler.text : UnityWebRequest.error,
#else
                    !UnityWebRequest.isNetworkError || UnityWebRequest.isHttpError ? UnityWebRequest.downloadHandler.text : UnityWebRequest.error,
#endif
                    UnityWebRequest.responseCode
                )
            );
        }

        public abstract void Callback(Gs2RestResponse gs2RestResponse);
    }
}
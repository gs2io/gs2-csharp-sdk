using System.Collections;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.Result;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Events;
#endif
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System;
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Net
{
    public interface IGs2Session
    {
        IGs2Credential Credential { get; }
        Region Region { get; }

        bool IsDisconnected();
        bool IsCanceled();
        bool IsCompleted(IGs2SessionRequest request);
        IGs2SessionResult MarkRead(IGs2SessionRequest request);
        
#if UNITY_2017_1_OR_NEWER
        IEnumerator Open(UnityAction<AsyncResult<OpenResult>> callback);
        IEnumerator Close(UnityAction callback);
#else
        IEnumerator Open(Action<AsyncResult<OpenResult>> callback);
        IEnumerator Close(Action callback);
#endif
        Gs2Future<OpenResult> OpenFuture();
        Gs2Future CloseFuture();
        IEnumerator Send(IGs2SessionRequest request);

        bool Ping();

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        UniTask<OpenResult> OpenAsync();
        UniTask SendAsync(IGs2SessionRequest request);
        UniTask CloseAsync();
    #else
        Task<OpenResult> OpenAsync();
        Task SendAsync(IGs2SessionRequest request);
        Task CloseAsync();
    #endif
#endif
    }
}
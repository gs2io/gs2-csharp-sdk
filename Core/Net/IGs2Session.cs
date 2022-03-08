using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Model;
using Gs2.Core.Result;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Events;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
#else
using System;
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
        IEnumerator Send(IGs2SessionRequest request);

        bool Ping();

#if UNITY_2017_1_OR_NEWER
    #if GS2_ENABLE_UNITASK
        UniTask<OpenResult> OpenAsync();
        UniTask SendAsync(IGs2SessionRequest request);
        UniTask CloseAsync();
    #endif
#else
        Task<OpenResult> OpenAsync();
        Task SendAsync(IGs2SessionRequest request);
        Task CloseAsync();
#endif
    }
}
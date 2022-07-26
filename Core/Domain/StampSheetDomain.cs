using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Net;
using Gs2.Gs2Distributor;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
using Gs2.Util.LitJson;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public class StampSheetDomain
    {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly Gs2RestSession _session;
        private readonly string _stampSheet;
        private readonly string _stampSheetEncryptionKeyId;
        private readonly string _namespaceName;
        private readonly Action<CacheDatabase, string, string, string> _stampTaskEvent;
        private readonly Action<CacheDatabase, string, string, string> _stampSheetEvent;

        public StampSheetDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            Gs2RestSession session,
            string stampSheet,
            string stampSheetEncryptionKeyId,
            string namespaceName,
            Action<CacheDatabase, string, string, string> stampTaskEvent,
            Action<CacheDatabase, string, string, string> stampSheetEvent
        )
        {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._session = session;
            this._stampSheet = stampSheet;
            this._stampSheetEncryptionKeyId = stampSheetEncryptionKeyId;
            this._namespaceName = namespaceName;
            this._stampTaskEvent = stampTaskEvent;
            this._stampSheetEvent = stampSheetEvent;
        }
        
#if UNITY_2017_1_OR_NEWER
    #if GS2_ENABLE_UNITASK
        public async UniTask RunAsync()
    #else
        public Gs2Future Run()
    #endif
#else
        public async Task RunAsync()
#endif
        {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(Gs2Future self)
            {
#endif
                var client = new Gs2DistributorRestClient(
                    _session
                );
                var stampSheetJson = JsonMapper.ToObject(_stampSheet);
                var stampSheetPayload = stampSheetJson["body"].ToString();
                var stampSheetPayloadJson = JsonMapper.ToObject(stampSheetPayload);
                var stampTasks = stampSheetPayloadJson["tasks"];
                string contextStack = null;
                for (var i = 0; i < stampTasks.Count; i++)
                {
                    var stampTask = stampTasks[i].ToString();
                    var stampTaskJson = JsonMapper.ToObject(stampTasks[i].ToString());
                    var stampTaskPayload = stampTaskJson["body"].ToString();
                    var stampTaskPayloadJson = JsonMapper.ToObject(stampTaskPayload);
                	if (string.IsNullOrEmpty(_namespaceName))
                    {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                        var future = client.RunStampTaskWithoutNamespaceFuture(
                            new RunStampTaskWithoutNamespaceRequest()
                                .WithContextStack(contextStack)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        yield return future;
                        if (future.Error != null)
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;
#else
                    var result = await client.RunStampTaskWithoutNamespaceAsync(
                            new RunStampTaskWithoutNamespaceRequest()
                                    .WithContextStack(contextStack)
                                    .WithStampTask(stampTasks[i].ToString())
                                    .WithKeyId(_stampSheetEncryptionKeyId)
                    );
#endif
                            contextStack = result.ContextStack;
                        _stampTaskEvent.Invoke(
                            _cache,
                            stampTaskPayloadJson["action"].ToString(),
                            stampTaskPayloadJson["args"].ToString(),
                            result.Result
                        );
                    }
                    else
                    {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                        var future = client.RunStampTaskFuture(
                            new RunStampTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(_namespaceName)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        yield return future;
                        if (future.Error != null)
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;
#else
                        var result = await client.RunStampTaskAsync(
                            new RunStampTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(_namespaceName)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
#endif
                        contextStack = result.ContextStack;
                        _stampTaskEvent.Invoke(
                            _cache,
                            stampTaskPayloadJson["action"].ToString(),
                            stampTaskPayloadJson["args"].ToString(),
                            result.Result
                        );
                    }
                }

                string action = null;
                JsonData requestJson = null;
                JsonData resultJson = null;
                if (string.IsNullOrEmpty(_namespaceName))
                {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = client.RunStampSheetWithoutNamespaceFuture(
                        new RunStampSheetWithoutNamespaceRequest()
                            .WithContextStack(contextStack)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                    var result = future.Result;
#else
                    var result = await client.RunStampSheetWithoutNamespaceAsync(
                        new RunStampSheetWithoutNamespaceRequest()
                            .WithContextStack(contextStack)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
#endif
                    _stampSheetEvent.Invoke(
                        _cache,
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        result.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    requestJson = JsonMapper.ToObject(stampSheetPayloadJson["args"].ToString());
                    resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
                }
                else
                {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = client.RunStampSheetFuture(
                        new RunStampSheetRequest()
                            .WithContextStack(contextStack)
                            .WithNamespaceName(_namespaceName)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                    var result = future.Result;
#else
                    var result = await client.RunStampSheetAsync(
                        new RunStampSheetRequest()
                            .WithContextStack(contextStack)
                            .WithNamespaceName(_namespaceName)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
#endif
                    _stampSheetEvent.Invoke(
                        _cache,
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        result.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    requestJson = JsonMapper.ToObject(stampSheetPayloadJson["args"].ToString());
                    resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
                }

                if (resultJson.ContainsKey("stampSheet"))
                {
                    var newStampSheet = new StampSheetDomain(
                        _cache,
                        _jobQueueDomain,
                        _session,
                        resultJson["stampSheet"].ToString(),
                        resultJson["stampSheetEncryptionKeyId"].ToString(),
                        _namespaceName,
                        _stampTaskEvent,
                        _stampSheetEvent
                    );
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return newStampSheet.Run();
#else
                    await newStampSheet.RunAsync();
#endif
                }

                if (action == "Gs2JobQueue:PushByUserId")
                {
                    Gs2.PushJobQueue(
                        _jobQueueDomain,
                        requestJson["namespaceName"].ToString()
                    );
                }
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture(Impl);
#endif
        }
    }
}
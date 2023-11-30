using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
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
        private readonly Gs2 _gs2;
        private readonly string _stampSheet;
        private readonly string _stampSheetEncryptionKeyId;

        public StampSheetDomain(
            Gs2 gs2,
            string stampSheet,
            string stampSheetEncryptionKeyId
        )
        {
            this._gs2 = gs2;
            this._stampSheet = stampSheet;
            this._stampSheetEncryptionKeyId = stampSheetEncryptionKeyId;
        }
        
#if UNITY_2017_1_OR_NEWER
        public Gs2Future RunFuture()
        {
            IEnumerator Impl(Gs2Future self)
            {
                var client = new Gs2DistributorRestClient(
                    this._gs2.RestSession
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
                	if (string.IsNullOrEmpty(this._gs2.TransactionConfiguration?.NamespaceName))
                    {
                        var future = client.RunStampTaskWithoutNamespaceFuture(
                            new RunStampTaskWithoutNamespaceRequest()
                                .WithContextStack(contextStack)
                                .WithStampTask(stampTask)
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        yield return future;
                        if (future.Error != null)
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;
                        contextStack = result.ContextStack;
                        this._gs2.TransactionConfiguration?.StampTaskEventHandler?.Invoke(
                            this._gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                            stampTaskPayloadJson["action"].ToString(),
                            stampTaskPayloadJson["args"].ToString(),
                            result.Result
                        );
                    }
                    else
                    {
                        var future = client.RunStampTaskFuture(
                            new RunStampTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(this._gs2.TransactionConfiguration?.NamespaceName)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        yield return future;
                        if (future.Error != null)
                        {
                            if (future.Error is NotFoundException) {
                                if (this._gs2.TransactionConfiguration != null) {
                                    this._gs2.TransactionConfiguration.NamespaceName = null;
                                    yield return Impl(self);
                                    yield break;
                                }
                            }
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;
                        contextStack = result.ContextStack;
                        this._gs2.TransactionConfiguration?.StampTaskEventHandler?.Invoke(
                            this._gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                            stampTaskPayloadJson["action"].ToString(),
                            stampTaskPayloadJson["args"].ToString(),
                            result.Result
                        );
                    }
                }

                string action = null;
                JsonData requestJson = null;
                JsonData resultJson = null;
                if (string.IsNullOrEmpty(this._gs2.TransactionConfiguration?.NamespaceName))
                {
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
                    this._gs2.TransactionConfiguration?.StampSheetEventHandler?.Invoke(
                        this._gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString(),
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
                    var future = client.RunStampSheetFuture(
                        new RunStampSheetRequest()
                            .WithContextStack(contextStack)
                            .WithNamespaceName(this._gs2.TransactionConfiguration?.NamespaceName)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is NotFoundException) {
                            if (this._gs2.TransactionConfiguration != null) {
                                this._gs2.TransactionConfiguration.NamespaceName = null;
                                yield return Impl(self);
                                yield break;
                            }
                        }
                        self.OnError(future.Error);
                        yield break;
                    }
                    var result = future.Result;
                    this._gs2.TransactionConfiguration?.StampSheetEventHandler?.Invoke(
                        this._gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString(),
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        result.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    requestJson = JsonMapper.ToObject(stampSheetPayloadJson["args"].ToString());
                    resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
                }

                if (resultJson.ContainsKey("stampSheet") && resultJson.ContainsKey("stampSheetEncryptionKeyId") && resultJson.ContainsKey("autoRunStampSheet") && !bool.Parse(resultJson["autoRunStampSheet"].ToString()))
                {
                    var newStampSheet = new StampSheetDomain(
                        this._gs2,
                        resultJson["stampSheet"].ToString(),
                        resultJson["stampSheetEncryptionKeyId"].ToString()
                    );
                    yield return newStampSheet.RunFuture();
                }

                if (action == "Gs2JobQueue:PushByUserId")
                {
                    this._gs2.PushJobQueue(
                        requestJson["namespaceName"].ToString()
                    );
                }
            }
            return new Gs2InlineFuture(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask RunAsync()
    #else
        public async Task RunAsync()
    #endif
        {
            var client = new Gs2DistributorRestClient(
                this._gs2.RestSession
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
                if (string.IsNullOrEmpty(this._gs2.TransactionConfiguration?.NamespaceName))
                {
                    var result = await client.RunStampTaskWithoutNamespaceAsync(
                        new RunStampTaskWithoutNamespaceRequest()
                                .WithContextStack(contextStack)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    contextStack = result.ContextStack;
                    this._gs2.TransactionConfiguration?.StampTaskEventHandler?.Invoke(
                        this._gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                        stampTaskPayloadJson["action"].ToString(),
                        stampTaskPayloadJson["args"].ToString(),
                        result.Result
                    );
                }
                else
                {
                    try {
                        var result = await client.RunStampTaskAsync(
                            new RunStampTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(this._gs2.TransactionConfiguration?.NamespaceName)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        contextStack = result.ContextStack;
                        this._gs2.TransactionConfiguration?.StampTaskEventHandler?.Invoke(
                            this._gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                            stampTaskPayloadJson["action"].ToString(),
                            stampTaskPayloadJson["args"].ToString(),
                            result.Result
                        );
                    }
                    catch (NotFoundException) {
                        if (this._gs2.TransactionConfiguration != null) {
                            this._gs2.TransactionConfiguration.NamespaceName = null;
                            await RunAsync();
                            return;
                        }
                    }
                }
            }

            string action = null;
            JsonData requestJson = null;
            JsonData resultJson = null;
            if (string.IsNullOrEmpty(this._gs2.TransactionConfiguration?.NamespaceName))
            {
                var result = await client.RunStampSheetWithoutNamespaceAsync(
                    new RunStampSheetWithoutNamespaceRequest()
                        .WithContextStack(contextStack)
                        .WithStampSheet(_stampSheet)
                        .WithKeyId(_stampSheetEncryptionKeyId)
                );
                this._gs2.TransactionConfiguration?.StampSheetEventHandler?.Invoke(
                    this._gs2.Cache,
                    stampSheetPayloadJson["transactionId"].ToString(),
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
                try {
                    var result = await client.RunStampSheetAsync(
                        new RunStampSheetRequest()
                            .WithContextStack(contextStack)
                            .WithNamespaceName(this._gs2.TransactionConfiguration?.NamespaceName)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    this._gs2.TransactionConfiguration?.StampSheetEventHandler?.Invoke(
                        this._gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString(),
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        result.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    requestJson = JsonMapper.ToObject(stampSheetPayloadJson["args"].ToString());
                    resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
                }
                catch (NotFoundException) {
                    if (this._gs2.TransactionConfiguration != null) {
                        this._gs2.TransactionConfiguration.NamespaceName = null;
                        await RunAsync();
                        return;
                    }
                }
            }

            if (resultJson.ContainsKey("stampSheet") && resultJson.ContainsKey("stampSheetEncryptionKeyId") && resultJson.ContainsKey("autoRunStampSheet") && !bool.Parse(resultJson["autoRunStampSheet"].ToString()))
            {
                var newStampSheet = new StampSheetDomain(
                    this._gs2,
                    resultJson["stampSheet"].ToString(),
                    resultJson["stampSheetEncryptionKeyId"].ToString()
                );
                await newStampSheet.RunAsync();
            }

            if (action == "Gs2JobQueue:PushByUserId")
            {
                this._gs2.PushJobQueue(
                    requestJson["namespaceName"].ToString()
                );
            }
        }
#endif
    }
}
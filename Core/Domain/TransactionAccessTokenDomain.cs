/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable ArrangeThisQualifier
// ReSharper disable NotAccessedField.Local

using System;
using System.Collections;
using Gs2.Core.Exception;
using Gs2.Core.Net;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public partial class TransactionAccessTokenDomain
    {

        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly AccessToken _accessToken;
        public bool AutoRunStampSheet;
        public string TransactionId;
        public string StampSheet;
        public string StampSheetEncryptionKey;

        public TransactionAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            AccessToken accessToken,
            bool autoRunStampSheet,
            string transactionId,
            string stampSheet,
            string stampSheetEncryptionKey
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._accessToken = accessToken;
            this.AutoRunStampSheet = autoRunStampSheet;
            this.TransactionId = transactionId;
            this.StampSheet = stampSheet;
            this.StampSheetEncryptionKey = stampSheetEncryptionKey;
        }
        
#if UNITY_2017_1_OR_NEWER
        public IFuture<TransactionAccessTokenDomain> Wait(
            bool all = false
        ) {
            IEnumerator Impl(IFuture<TransactionAccessTokenDomain> self) {
#if GS2_ENABLE_UNITASK
                yield return WaitAsync().ToCoroutine(
                    v => { self.OnComplete(this); },
                    e => { self.OnError(e as Gs2Exception); }
                );
#else
                if (this.AutoRunStampSheet) {
                    while (true) {
                        var future = new Gs2Distributor.Domain.Gs2Distributor(
                            this._cache,
                            this._jobQueueDomain,
                            this._stampSheetConfiguration,
                            this._session
                        ).Namespace(
                            this._stampSheetConfiguration.NamespaceName
                        ).AccessToken(
                            this._accessToken
                        ).StampSheetResult(
                            this.TransactionId
                        ).Model();
                        
                        yield return future;
                        if (future.Error != null) {
                            self.OnError(future.Error);
                            yield break;
                        }

                        if (future.Result == null) {
                            yield return new WaitForSeconds(0.1f);
                            yield return Gs2Distributor.Domain.Gs2Distributor.Dispatch(
                                this._cache,
                                this._jobQueueDomain,
                                this._stampSheetConfiguration,
                                this._session,
                                this._accessToken
                            );
                            continue;
                        }
                        var result = future.Result;

                        if (result.SheetRequest.Action == "Gs2JobQueue:PushByUserId")
                        {
                            var result2 = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.SheetResult));
                            if (result2?.AutoRun != null && !result2.AutoRun.Value)
                            {
                                foreach (var job in result2.Items) {
                                    var future3 = new JobQueueJobAccessTokenDomain(
                                        this._cache,
                                        this._jobQueueDomain,
                                        this._stampSheetConfiguration,
                                        this._session,
                                        this._accessToken,
                                        true,
                                        job.JobId
                                    ).Wait();
                                    yield return future3;
                                    if (future3.Error != null) {
                                        self.OnError(future3.Error);
                                        yield break;
                                    }
                                }
                            }
                        }

                        var resultJson = JsonMapper.ToObject(result.SheetResult);
                        if (resultJson.ContainsKey("transactionId") && resultJson["transactionId"] != null) {
                            var next = new TransactionAccessTokenDomain(
                                this._cache,
                                this._jobQueueDomain,
                                this._stampSheetConfiguration,
                                this._session,
                                this._accessToken,
                                resultJson.ContainsKey("autoRunStampSheet") && (resultJson["autoRunStampSheet"] ?? false).ToString().ToLower() == "true",
                                !resultJson.ContainsKey("transactionId") ? null : resultJson["transactionId"]?.ToString(),
                                !resultJson.ContainsKey("stampSheet") ? null : resultJson["stampSheet"]?.ToString(),
                                !resultJson.ContainsKey("stampSheetEncryptionKeyId") ? null : resultJson["stampSheetEncryptionKeyId"]?.ToString()
                            );
                            if (all) {
                                while (next != null) {
                                    var future2 = next.Wait(all);
                                    yield return future2;
                                    if (future2.Error != null) {
                                        self.OnError(future2.Error);
                                        yield break;
                                    }
                                    next = future2.Result;
                                }
                            }
                        }

                        self.OnComplete(null);
                        break;
                    }
                } 
                else {
                    var stampSheet = new StampSheetDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._session,
                        this.StampSheet.ToString(),
                        this.StampSheetEncryptionKey.ToString(),
                        this._stampSheetConfiguration.NamespaceName,
                        this._stampSheetConfiguration.StampTaskEventHandler,
                        this._stampSheetConfiguration.StampSheetEventHandler
                    );
                    var future4 = stampSheet.Run();
                    yield return future4;
                    if (future4.Error != null) {
                        self.OnError(new TransactionException(stampSheet, future4.Error));
                        yield break;
                    }

                    self.OnComplete(this);
                }
#endif
            }
            return new Gs2InlineFuture<TransactionAccessTokenDomain>(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK

    #if UNITY_2017_1_OR_NEWER
        public async UniTask<TransactionAccessTokenDomain> WaitAsync(
    #else
        public async Task<TransactionAccessTokenDomain> WaitAsync(
    #endif
            bool all = false
        ) {
            if (this.AutoRunStampSheet) {
                while (true) {
                    var result = await new Gs2Distributor.Domain.Gs2Distributor(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session
                    ).Namespace(
                        this._stampSheetConfiguration.NamespaceName
                    ).AccessToken(
                        this._accessToken
                    ).StampSheetResult(
                        this.TransactionId
                    ).Model();
                    if (result == null) {
#if UNITY_2017_1_OR_NEWER
                        await UniTask.Delay(TimeSpan.FromMilliseconds(100));
#else
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
#endif
                        continue;
                    }

                    if (result.SheetRequest.Action == "Gs2JobQueue:PushByUserId")
                    {
                        var result2 = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.SheetResult));
                        if (result2?.AutoRun != null && !result2.AutoRun.Value)
                        {
                            foreach (var job in result2.Items) {
                                await new JobQueueJobAccessTokenDomain(
                                    this._cache,
                                    this._jobQueueDomain,
                                    this._stampSheetConfiguration,
                                    this._session,
                                    this._accessToken,
                                    true,
                                    job.JobId
                                ).WaitAsync();
                            }
                        }
                    }

                    var resultJson = JsonMapper.ToObject(result.SheetResult);
                    if (resultJson.ContainsKey("transactionId") && !string.IsNullOrEmpty(resultJson["transactionId"]?.ToString())) {
                        var next = new TransactionAccessTokenDomain(
                            this._cache,
                            this._jobQueueDomain,
                            this._stampSheetConfiguration,
                            this._session,
                            this._accessToken,
                            resultJson.ContainsKey("autoRunStampSheet") && (resultJson["autoRunStampSheet"] ?? false).ToString().ToLower() == "true",
                            !resultJson.ContainsKey("transactionId") ? null : resultJson["transactionId"]?.ToString(),
                            !resultJson.ContainsKey("stampSheet") ? null : resultJson["stampSheet"]?.ToString(),
                            !resultJson.ContainsKey("stampSheetEncryptionKeyId") ? null : resultJson["stampSheetEncryptionKeyId"]?.ToString()
                        );
                        if (all) {
                            while (next != null) {
                                next = await next.WaitAsync(all);
                            }
                        }
                    }

                    break;
                }
                return null;
            }
            else {
                var stampSheet = new StampSheetDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._session,
                    this.StampSheet.ToString(),
                    this.StampSheetEncryptionKey.ToString(),
                    this._stampSheetConfiguration.NamespaceName,
                    this._stampSheetConfiguration.StampTaskEventHandler,
                    this._stampSheetConfiguration.StampSheetEventHandler
                );
                try {
                    await stampSheet.RunAsync();
                } catch (Gs2Exception e) {
                    throw new TransactionException(stampSheet, e);
                }
                return this;
            }
        }
#endif
    }
}

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
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public partial class TransactionDomain
    {

        private readonly Gs2 _gs2;
        private readonly string _userId;
        public bool AutoRunStampSheet;
        public string TransactionId;
        public string StampSheet;
        public string StampSheetEncryptionKey;

        public TransactionDomain(
            Gs2 gs2,
            string userId,
            bool autoRunStampSheet,
            string transactionId,
            string stampSheet,
            string stampSheetEncryptionKey
        ) {
            this._gs2 = gs2;
            this._userId = userId;
            this.AutoRunStampSheet = autoRunStampSheet;
            this.TransactionId = transactionId;
            this.StampSheet = stampSheet;
            this.StampSheetEncryptionKey = stampSheetEncryptionKey;
        }
        
#if UNITY_2017_1_OR_NEWER
        public IFuture<TransactionDomain> WaitFuture(
            bool all = false
        ) {
            IEnumerator Impl(IFuture<TransactionDomain> self) {
                if (this.AutoRunStampSheet) {
                    while (true) {
                        var future = new Gs2Distributor.Domain.Gs2Distributor(
                            this._gs2
                        ).Namespace(
                            this._gs2.StampSheetConfiguration.NamespaceName
                        ).User(
                            this._userId
                        ).StampSheetResult(
                            this.TransactionId
                        ).ModelFuture();
                        
                        yield return future;
                        if (future.Error != null) {
                            self.OnError(future.Error);
                            yield break;
                        }

                        if (future.Result == null) {
                            yield return new WaitForSeconds(0.1f);
                                    
                            var dispatchFuture = this._gs2.DispatchByUserIdFuture(this._userId);
                            yield return dispatchFuture;
                            if (dispatchFuture.Error != null) {
                                self.OnError(dispatchFuture.Error);
                                yield break;
                            }
                            continue;
                        }
                        var result = future.Result;

                        if (result.SheetRequest.Action == "Gs2JobQueue:PushByUserId")
                        {
                            var result2 = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.SheetResult));
                            if (all)
                            {
                                foreach (var job in result2.Items) {
                                    var future3 = new JobQueueJobDomain(
                                        this._gs2,
                                        this._userId,
                                        true,
                                        job.JobId
                                    ).WaitFuture(all);
                                    yield return future3;
                                    if (future3.Error != null) {
                                        self.OnError(future3.Error);
                                        yield break;
                                    }
                                    
                                    var dispatchFuture = this._gs2.DispatchByUserIdFuture(this._userId);
                                    yield return dispatchFuture;
                                    if (dispatchFuture.Error != null) {
                                        self.OnError(dispatchFuture.Error);
                                        yield break;
                                    }
                                }
                            }
                        }

                        var resultJson = JsonMapper.ToObject(result.SheetResult);
                        if (resultJson["transactionId"] != null) {
                            var next = new TransactionDomain(
                                this._gs2,
                                this._userId,
                                resultJson.ContainsKey("autoRunStampSheet") && (resultJson["autoRunStampSheet"] ?? false).ToString().ToLower() == "true",
                                !resultJson.ContainsKey("transactionId") ? null : resultJson["transactionId"]?.ToString(),
                                !resultJson.ContainsKey("stampSheet") ? null : resultJson["stampSheet"]?.ToString(),
                                !resultJson.ContainsKey("stampSheetEncryptionKeyId") ? null : resultJson["stampSheetEncryptionKeyId"]?.ToString()
                            );
                            if (all || !next.AutoRunStampSheet) {
                                while (next != null) {
                                    var future2 = next.WaitFuture(all);
                                    yield return future2;
                                    if (future2.Error != null) {
                                        self.OnError(future2.Error);
                                        yield break;
                                    }
                                    next = future2.Result;
                                    
                                    var dispatchFuture = this._gs2.DispatchByUserIdFuture(this._userId);
                                    yield return dispatchFuture;
                                    if (dispatchFuture.Error != null) {
                                        self.OnError(dispatchFuture.Error);
                                        yield break;
                                    }
                                }
                            }
                        }

                        self.OnComplete(null);
                        break;
                    }
                } 
                else {
                    var stampSheet = new StampSheetDomain(
                        this._gs2,
                        this.StampSheet.ToString(),
                        this.StampSheetEncryptionKey.ToString()
                    );
                    var future4 = stampSheet.RunFuture();
                    yield return future4;
                    if (future4.Error != null) {
                        self.OnError(new TransactionException(stampSheet, future4.Error));
                        yield break;
                    }
                    
                    var dispatchFuture = Gs2Distributor.Domain.Gs2Distributor.DispatchByUserIdFuture(this._gs2, this._userId);
                    yield return dispatchFuture;
                    if (dispatchFuture.Error != null) {
                        self.OnError(dispatchFuture.Error);
                        yield break;
                    }

                    self.OnComplete(this);
                }
            }
            return new Gs2InlineFuture<TransactionDomain>(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK

    #if UNITY_2017_1_OR_NEWER
        public async UniTask<TransactionDomain> WaitAsync(
    #else
        public async Task<TransactionDomain> WaitAsync(
    #endif
            bool all = false
        ) {
            if (this.AutoRunStampSheet) {
                while (true) {
                    var result = await new Gs2Distributor.Domain.Gs2Distributor(
                        this._gs2
                    ).Namespace(
                        this._gs2.StampSheetConfiguration.NamespaceName
                    ).User(
                        this._userId
                    ).StampSheetResult(
                        this.TransactionId
                    ).ModelAsync();
                    if (result == null) {
#if UNITY_2017_1_OR_NEWER
                        await UniTask.Delay(TimeSpan.FromMilliseconds(100));
#else
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
#endif
                        await this._gs2.DispatchByUserIdAsync(this._userId);
                        continue;
                    }

                    if (result.SheetRequest.Action == "Gs2JobQueue:PushByUserId")
                    {
                        var result2 = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.SheetResult));
                        if (all)
                        {
                            foreach (var job in result2.Items) {
                                await new JobQueueJobDomain(
                                    this._gs2,
                                    this._userId,
                                    true,
                                    job.JobId
                                ).WaitAsync(all);
                                await this._gs2.DispatchByUserIdAsync(this._userId);
                            }
                        }
                    }

                    var resultJson = JsonMapper.ToObject(result.SheetResult);
                    if (resultJson.ContainsKey("transactionId") && !string.IsNullOrEmpty(resultJson["transactionId"]?.ToString())) {
                        var next = new TransactionDomain(
                            this._gs2,
                            this._userId,
                            resultJson.ContainsKey("autoRunStampSheet") && (resultJson["autoRunStampSheet"] ?? false).ToString().ToLower() == "true",
                            !resultJson.ContainsKey("transactionId") ? null : resultJson["transactionId"]?.ToString(),
                            !resultJson.ContainsKey("stampSheet") ? null : resultJson["stampSheet"]?.ToString(),
                            !resultJson.ContainsKey("stampSheetEncryptionKeyId") ? null : resultJson["stampSheetEncryptionKeyId"]?.ToString()
                        );
                        if (all || !next.AutoRunStampSheet) {
                            while (next != null) {
                                next = await next.WaitAsync(all);
                                await this._gs2.DispatchByUserIdAsync(this._userId);
                            }
                        }
                    }

                    break;
                }
                return null;
            }
            else {
                var stampSheet = new StampSheetDomain(
                    this._gs2,
                    this.StampSheet.ToString(),
                    this.StampSheetEncryptionKey.ToString()
                );
                try {
                    await stampSheet.RunAsync();
                    await Gs2Distributor.Domain.Gs2Distributor.DispatchByUserIdAsync(this._gs2, this._userId);
                } catch (Gs2Exception e) {
                    throw new TransactionException(stampSheet, e);
                }
                return this;
            }
        }
#endif
    }
}

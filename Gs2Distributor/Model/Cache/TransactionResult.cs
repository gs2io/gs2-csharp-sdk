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
 *
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using Gs2.Core.Domain;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Util.LitJson;
using System.Linq;
using System.Collections;
using Gs2.Core.Exception;
#if UNITY_2017_1_OR_NEWER
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Distributor.Model.Cache
{
    public static partial class TransactionResultExt
    {
        public static string CacheParentKey(
            this TransactionResult self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "distributor",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "TransactionResult"
            );
        }

        public static string CacheKey(
            this TransactionResult self,
            string transactionId
        ) {
            return string.Join(
                ":",
                transactionId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<TransactionResult> FetchFuture(
            this TransactionResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId,
            int? timeOffset,
            Func<IFuture<TransactionResult>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<TransactionResult> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is NotFoundException e)
                    {
                        (null as TransactionResult).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            transactionId,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "transactionResult") {
                            self.OnComplete(default);
                            yield break;
                        }
                    }
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    transactionId,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<TransactionResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<TransactionResult> FetchAsync(
    #else
        public static async Task<TransactionResult> FetchAsync(
    #endif
            this TransactionResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<TransactionResult>> fetchImpl
    #else
            Func<Task<TransactionResult>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    transactionId,
                    timeOffset
                );
                return item;
            }
            catch (NotFoundException e) {
                (null as TransactionResult).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    transactionId,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "transactionResult") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<TransactionResult, bool> GetCache(
            this TransactionResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<TransactionResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    transactionId
                )
            );
        }

        public static void PutCache(
            this TransactionResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<TransactionResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    transactionId
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    transactionId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Core.Domain.Gs2.DefaultCacheMinutes
            );

            foreach (var acquireAction in self?.AcquireResults ?? Array.Empty<AcquireActionResult>()) {
                var data = JsonMapper.ToObject(acquireAction.AcquireResult);
                if (data.ContainsKey("transactionResult")) {
                    JsonMapper.ToObject<TransactionResult>(data["transactionResult"].ToJson()).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        acquireAction.Action,
                        timeOffset
                    );
                }
            }
        }
        
        public static void PutCache(
            this Core.Model.TransactionResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            new TransactionResult {
                UserId = userId,
                TransactionId = self.TransactionId,
                VerifyResults = self.VerifyResults?.Select(v => new VerifyActionResult {
                    Action = v.Action,
                    VerifyRequest = v.VerifyRequest,
                    VerifyResult = v.VerifyResult,
                    StatusCode = v.StatusCode,
                }).ToArray(),
                ConsumeResults = self.ConsumeResults?.Select(v => new ConsumeActionResult {
                    Action = v.Action,
                    ConsumeRequest = v.ConsumeRequest,
                    ConsumeResult = v.ConsumeResult,
                    StatusCode = v.StatusCode,
                }).ToArray(),
                AcquireResults = self.AcquireResults?.Select(v => new AcquireActionResult {
                    Action = v.Action,
                    AcquireRequest = v.AcquireRequest,
                    AcquireResult = v.AcquireResult,
                    StatusCode = v.StatusCode,
                }).ToArray(),
            }.PutCache(
                cache,
                namespaceName,
                userId,
                transactionId,
                timeOffset
            );
        }
        
        public static void DeleteCache(
            this TransactionResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<TransactionResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    transactionId
                )
            );
        }

        public static void ListSubscribe(
            this TransactionResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<TransactionResult[]> callback
        ) {
            cache.ListSubscribe<TransactionResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this TransactionResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<TransactionResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}
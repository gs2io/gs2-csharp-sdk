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

#pragma warning disable 1998

using System;
using System.Collections;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inventory.Request;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain.SpeculativeExecutor
{
    public static class ConsumeActionSpeculativeExecutorIndex
    {
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumeAction consumeAction
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                if (VerifyInventoryCurrentMaxCapacityByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                    var future = VerifyInventoryCurrentMaxCapacityByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        VerifyInventoryCurrentMaxCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (ConsumeItemSetByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                    var future = ConsumeItemSetByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        ConsumeItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (VerifyItemSetByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                    var future = VerifyItemSetByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        VerifyItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (VerifyReferenceOfByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                    var future = VerifyReferenceOfByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        VerifyReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (ConsumeSimpleItemsByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                    var future = ConsumeSimpleItemsByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        ConsumeSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (VerifySimpleItemByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                    var future = VerifySimpleItemByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        VerifySimpleItemByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (ConsumeBigItemByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                    var future = ConsumeBigItemByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        ConsumeBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (VerifyBigItemByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                    var future = VerifyBigItemByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        VerifyBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                result.OnComplete(null);
                yield return null;
            }

            return new Gs2InlineFuture<Func<object>>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Func<object>> ExecuteAsync(
    #else
        public static async Task<Func<object>> ExecuteAsync(
    #endif
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumeAction consumeAction
        ) {
            if (VerifyInventoryCurrentMaxCapacityByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                return await VerifyInventoryCurrentMaxCapacityByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    VerifyInventoryCurrentMaxCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                );
            }
            if (ConsumeItemSetByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                return await ConsumeItemSetByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    ConsumeItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                );
            }
            if (VerifyItemSetByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                return await VerifyItemSetByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    VerifyItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                );
            }
            if (VerifyReferenceOfByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                return await VerifyReferenceOfByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    VerifyReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                );
            }
            if (ConsumeSimpleItemsByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                return await ConsumeSimpleItemsByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    ConsumeSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                );
            }
            if (VerifySimpleItemByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                return await VerifySimpleItemByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    VerifySimpleItemByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                );
            }
            if (ConsumeBigItemByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                return await ConsumeBigItemByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    ConsumeBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                );
            }
            if (VerifyBigItemByUserIdSpeculativeExecutor.Action() == consumeAction.Action) {
                return await VerifyBigItemByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    VerifyBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(consumeAction.Request))
                );
            }
            return () => { return null; };
        }
#endif
    }
}
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
    public static class AcquireActionSpeculativeExecutorIndex
    {
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireAction acquireAction
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                if (AddCapacityByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var future = AddCapacityByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (SetCapacityByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var future = SetCapacityByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (AcquireItemSetByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var future = AcquireItemSetByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (AddReferenceOfByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var future = AddReferenceOfByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (DeleteReferenceOfByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var future = DeleteReferenceOfByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (AcquireSimpleItemsByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var future = AcquireSimpleItemsByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (AcquireBigItemByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var future = AcquireBigItemByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
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
            AcquireAction acquireAction
        ) {
            if (AddCapacityByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                return await AddCapacityByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                );
            }
            if (SetCapacityByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                return await SetCapacityByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                );
            }
            if (AcquireItemSetByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                return await AcquireItemSetByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                );
            }
            if (AddReferenceOfByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                return await AddReferenceOfByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                );
            }
            if (DeleteReferenceOfByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                return await DeleteReferenceOfByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                );
            }
            if (AcquireSimpleItemsByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                return await AcquireSimpleItemsByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                );
            }
            if (AcquireBigItemByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                return await AcquireBigItemByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request))
                );
            }
            return () => { return null; };
        }
#endif
    }
}
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
using System.Numerics;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inventory.Model.Transaction;
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
            AcquireAction acquireAction,
            BigInteger rate
        ) {
            acquireAction.Action = acquireAction.Action.Replace("{region}", domain.RestSession.Region.DisplayName());
            acquireAction.Action = acquireAction.Action.Replace("{ownerId}", domain.RestSession.OwnerId);
            acquireAction.Action = acquireAction.Action.Replace("{userId}", accessToken.UserId);
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                if (AddCapacityByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var request = AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = AddCapacityByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
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
                    var request = SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = SetCapacityByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
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
                    var request = AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = AcquireItemSetByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (AcquireItemSetWithGradeByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var request = AcquireItemSetWithGradeByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = AcquireItemSetWithGradeByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
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
                    var request = AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = AddReferenceOfByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
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
                    var request = DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = DeleteReferenceOfByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
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
                    var request = AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = AcquireSimpleItemsByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (SetSimpleItemsByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var request = SetSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = SetSimpleItemsByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
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
                    var request = AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = AcquireBigItemByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (SetBigItemByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var request = SetBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = SetBigItemByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
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
            AcquireAction acquireAction,
            BigInteger rate
        ) {
            acquireAction.Action = acquireAction.Action.Replace("{region}", domain.RestSession.Region.DisplayName());
            acquireAction.Action = acquireAction.Action.Replace("{ownerId}", domain.RestSession.OwnerId);
            acquireAction.Action = acquireAction.Action.Replace("{userId}", accessToken.UserId);
            if (AddCapacityByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await AddCapacityByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (SetCapacityByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await SetCapacityByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (AcquireItemSetByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await AcquireItemSetByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (AcquireItemSetWithGradeByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = AcquireItemSetWithGradeByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await AcquireItemSetWithGradeByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (AddReferenceOfByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await AddReferenceOfByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (DeleteReferenceOfByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await DeleteReferenceOfByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (AcquireSimpleItemsByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await AcquireSimpleItemsByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (SetSimpleItemsByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = SetSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await SetSimpleItemsByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (AcquireBigItemByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await AcquireBigItemByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (SetBigItemByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = SetBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await SetBigItemByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            return null;
        }
#endif
    }
}
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
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq; /* diff +++ */
using Gs2.Core.Domain;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Result;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Model.Cache
{
    public static partial class ReferenceOfExt
    {
        public static void PutCache(
            this VerifyReferenceOfByUserIdResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            VerifyReferenceOfByUserIdRequest request
        ) {
            self.ItemSet?.PutCache(
                cache,
                request.NamespaceName,
                request.UserId,
                request.InventoryName,
                request.ItemName,
                request.ItemSetName,
                timeOffset
            );
            self.ItemModel?.PutCache(
                cache,
                request.NamespaceName,
                request.InventoryName,
                request.ItemName,
                timeOffset
            );
            self.Inventory?.PutCache(
                cache,
                request.NamespaceName,
                request.UserId,
                request.InventoryName,
/* diff +++ start */
                timeOffset
            );
            // 成功しても not_entry / empty なら参照元は無い。検証した時点のアイテムセットの一覧で載せるか消すかを決める
            if (self.ItemSet == null) {
                return;
            }
            var referenceOf = new ReferenceOf().WithName(request.ReferenceOf);
            if (!(self.ItemSet.ReferenceOf ?? Array.Empty<string>()).Contains(request.ReferenceOf)) {
                referenceOf.DeleteCache(
                    cache,
                    request.NamespaceName,
                    request.UserId,
                    request.InventoryName,
                    request.ItemName,
                    request.ItemSetName,
                    referenceOf.Name,
                    timeOffset
                );
                return;
            }
            referenceOf.PutCache(
                cache,
                request.NamespaceName,
                request.UserId,
                request.InventoryName,
                request.ItemName,
                request.ItemSetName,
                referenceOf.Name,
/* diff +++ end */
                timeOffset
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<VerifyReferenceOfByUserIdResult> InvokeFuture(
            this VerifyReferenceOfByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<VerifyReferenceOfByUserIdResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<VerifyReferenceOfByUserIdResult> self)
            {
                var future = invokeImpl();
                yield return future;
                if (future.Error != null) {
                    if (future.Error is Gs2.Core.Exception.Gs2Exception failed && failed.Result != null) {
                        VerifyReferenceOfByUserIdResult.FromJson(Gs2.Util.LitJson.JsonMapper.ToObject(failed.Result))?.PutCache(
                            cache,
                            userId,
                            timeOffset,
                            request
                        );
                    }
                    self.OnError(future.Error);
                    yield break;
                }

                future.Result.PutCache(
                    cache,
                    userId,
                    timeOffset,
                    request
                );

                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<VerifyReferenceOfByUserIdResult>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<VerifyReferenceOfByUserIdResult> InvokeAsync(
#else
        public static async Task<VerifyReferenceOfByUserIdResult> InvokeAsync(
#endif
            this VerifyReferenceOfByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<VerifyReferenceOfByUserIdResult>> invokeImpl
#elif UNITY_2017_1_OR_NEWER
            Func<TaskFuture<VerifyReferenceOfByUserIdResult>> invokeImpl
#else
            Func<Task<VerifyReferenceOfByUserIdResult>> invokeImpl
#endif
        )
        {
            VerifyReferenceOfByUserIdResult result;
            try {
                result = await invokeImpl();
            }
            catch (Gs2.Core.Exception.Gs2Exception e) when (e.Result != null) {
                VerifyReferenceOfByUserIdResult.FromJson(Gs2.Util.LitJson.JsonMapper.ToObject(e.Result))?.PutCache(
                    cache,
                    userId,
                    timeOffset,
                    request
                );
                throw;
            }
            result.PutCache(
                cache,
                userId,
                timeOffset,
                request
            );
            return result;
        }
    }
}
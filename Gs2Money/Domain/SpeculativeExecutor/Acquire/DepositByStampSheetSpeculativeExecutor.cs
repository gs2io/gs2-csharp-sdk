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
using System.Numerics;
using System.Collections;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Money.Model; /* diff +++ */
using Gs2.Gs2Money.Request;
using Gs2.Gs2Money.Model.Cache;
using Gs2.Gs2Money.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Money.Domain.SpeculativeExecutor
{
    public static class DepositByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Money:DepositByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DepositByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DepositByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Money.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Wallet(
                request.Slot
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var preparedRequest = request == null ? null :
                new DepositByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithUserId(request.UserId)
                    .WithSlot(request.Slot)
                    .WithPrice(request.Price)
                    .WithCount(request.Count)
                    .WithTimeOffsetToken(request.TimeOffsetToken);
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedRequest?.NamespaceName) ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                preparedRequest.UserId != preparedAccessToken.UserId ||
                preparedRequest.Slot == null ||
                preparedRequest.Price == null ||
                preparedRequest.Count == null) {
                return null;
            }
            var userId = preparedAccessToken.UserId;
            var timeOffset = preparedAccessToken.TimeOffset;
            bool IsExpected(Wallet wallet, int? slot) {
                var walletId = string.Join(
                    ":", "grn", "gs2",
                    domain.RestSession.Region.DisplayName(),
                    domain.RestSession.OwnerId,
                    "money", preparedRequest.NamespaceName,
                    "user", userId,
                    "wallet", slot
                );
                return wallet != null &&
                       wallet.WalletId == walletId &&
                       wallet.UserId == userId &&
                       wallet.Slot == slot;
            }
            var (item, found) = ((Wallet)null).GetCache(
                domain.Cache,
                preparedRequest.NamespaceName,
                userId,
                preparedRequest.Slot,
                timeOffset
            );
            if (!found || !IsExpected(item, preparedRequest.Slot)) {
                return null;
            }
            SynchronizeSharedFree(
                domain, item, preparedRequest.NamespaceName, userId,
                preparedRequest.Slot, timeOffset, out var preparedSharedWallet,
                out var sharedFreeUnavailable
            );
            if (sharedFreeUnavailable && !(preparedRequest.Price > 0) ||
                preparedSharedWallet != null &&
                !IsExpected(preparedSharedWallet, 0)) {
                return null;
            }
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
                var (cachedItem, find) = ((Wallet)null).GetCache( /* diff +++ */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    request.UserId,
                    request.Slot,
                    null
 diff --- end */
/* diff +++ start */
                    preparedRequest.NamespaceName,
                    userId,
                    preparedRequest.Slot,
                    timeOffset
                );
                if (!find || !IsExpected(cachedItem, preparedRequest.Slot)) {
                    return null;
                }
                var sourceItem = SynchronizeSharedFree(
                    domain,
                    cachedItem,
                    preparedRequest.NamespaceName,
                    userId,
                    preparedRequest.Slot,
                    timeOffset,
                    out var sharedWallet,
                    out sharedFreeUnavailable
                );
                if (sharedFreeUnavailable && !(preparedRequest.Price > 0)) {
                    return null;
                }
                if (sharedWallet != null && !IsExpected(sharedWallet, 0)) {
                    return null;
                }
                var previousFree = sourceItem?.Free;
                Wallet committedItem;
                try {
                    committedItem = sourceItem.SpeculativeExecution(
                        preparedRequest
                    );
                }
                catch (System.Exception) {
                    return null;
                }
                if (sharedWallet != null && previousFree != committedItem.Free) {
                    var committedSharedWallet = sharedWallet
                        .SpeculativeSyncFree(committedItem);
                    committedSharedWallet.Revision = 0;
                    committedSharedWallet.PutCache(
                        domain.Cache,
                        preparedRequest.NamespaceName,
                        userId,
                        0,
                        timeOffset
                    );
                }
                committedItem.PutCache(
                    domain.Cache,
                    preparedRequest.NamespaceName,
                    userId,
                    preparedRequest.Slot,
                    timeOffset
/* diff +++ end */
                );
                return null;
            };
        }
/* diff +++ start */

        internal static Wallet SynchronizeSharedFree(
            Gs2.Core.Domain.Gs2 domain,
            Wallet item,
            string namespaceName,
            string userId,
            int? slot,
            int? timeOffset,
            out Wallet sharedWallet,
            out bool sharedFreeUnavailable
        ) {
            sharedWallet = null;
            sharedFreeUnavailable = false;
            if (item?.ShareFree != true || slot is null or 0) {
                return item;
            }
            var (cachedSharedWallet, find) = ((Wallet)null).GetCache(
                domain.Cache,
                namespaceName,
                userId,
                0,
                timeOffset
            );
            if (!find || cachedSharedWallet == null) {
                sharedFreeUnavailable = true;
                return item;
            }
            sharedWallet = cachedSharedWallet;
            return item.SpeculativeSyncFree(cachedSharedWallet);
        }
/* diff +++ end */
    }
}

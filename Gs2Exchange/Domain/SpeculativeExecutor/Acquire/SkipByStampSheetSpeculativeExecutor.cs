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
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Exchange.Model; /* diff +++ */
using Gs2.Gs2Exchange.Request;
using Gs2.Gs2Exchange.Model.Cache;
/* diff --- start
using Gs2.Gs2Exchange.Model.Transaction;
 diff --- end */
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Exchange.Domain.SpeculativeExecutor
{
    public static class SkipByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Exchange:SkipByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SkipByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SkipByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Exchange.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Await(
                request.AwaitName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = SkipByUserIdRequest.FromJson(request?.ToJson());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId) {
                return null;
            }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var cachedAwait = ((Await)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.AwaitName,
                token.TimeOffset
            );
            var item = cachedAwait.Item1;
            var expectedAwaitId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:exchange:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:await:{prepared.AwaitName}";
            if (!cachedAwait.Item2 || item == null ||
                item.AwaitId != expectedAwaitId || item.UserId != token.UserId ||
                item.Name != prepared.AwaitName || item.RateName == null ||
                !item.SkipSeconds.HasValue || !item.ExchangedAt.HasValue) {
                return null;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */

/* diff --- start
            return () =>
            {
                item.PutCache(
 diff --- end */
/* diff +++ start */
            var cachedRate = ((RateModel)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                item.RateName,
                null
            );
            var rateModel = cachedRate.Item1;
            var expectedRateId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:exchange:{prepared.NamespaceName}:" +
                $"model:{item.RateName}";
            if (!cachedRate.Item2 || rateModel == null ||
                rateModel.RateModelId != expectedRateId ||
                rateModel.Name != item.RateName || !rateModel.LockTime.HasValue) {
                return null;
            }

            var totalLockSeconds = (long)rateModel.LockTime.Value * 60L;
            var normalizedLockSeconds = unchecked(rateModel.LockTime.Value * 60);
            var normalizedAcquirableAt = item.ExchangedAt.Value +
                ((long)normalizedLockSeconds - item.SkipSeconds.Value) * 1000L;
            long skipSeconds;
            switch (prepared.SkipType) {
                case "complete":
                    skipSeconds = totalLockSeconds;
                    break;
                case "minutes" when prepared.Minutes.HasValue:
                    skipSeconds = item.SkipSeconds.Value +
                                  (long)prepared.Minutes.Value * 60L;
                    break;
                case "totalRate" when prepared.Rate.HasValue:
                    skipSeconds = item.SkipSeconds.Value +
                                  (long)((float)totalLockSeconds *
                                         prepared.Rate.Value);
                    break;
                case "remainRate" when prepared.Rate.HasValue:
                    var remainMillis = (long)(
                        (float)(normalizedAcquirableAt - item.ExchangedAt.Value) *
                        prepared.Rate.Value
                    );
                    skipSeconds = item.SkipSeconds.Value +
                        unchecked((int)(remainMillis / 1000L));
                    break;
                default:
                    return null;
            }
            if (skipSeconds > totalLockSeconds) skipSeconds = totalLockSeconds;
            if (skipSeconds < 0) skipSeconds = 0;
            if (skipSeconds > int.MaxValue) skipSeconds = int.MaxValue;

            var changed = item.Clone() as Await;
            changed.SkipSeconds = (int)skipSeconds;
            changed.AcquirableAt = item.ExchangedAt.Value + skipSeconds * -1000L +
                                   (long)normalizedLockSeconds * 1000L;
            changed.Revision = 0;
            var awaitSnapshot = item.ToJson().ToJson();
            var rateSnapshot = rateModel.ToJson().ToJson();
            return () => {
                var liveAwait = ((Await)null).GetCache(
/* diff +++ end */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    request.UserId,
                    request.AwaitName,
 diff --- end */
/* diff +++ start */
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.AwaitName,
                    token.TimeOffset
                );
                var liveRate = ((RateModel)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    item.RateName,
/* diff +++ end */
                    null
/* diff +++ start */
                );
                if (!liveAwait.Item2 || liveAwait.Item1?.ToJson().ToJson() !=
                        awaitSnapshot || !liveRate.Item2 ||
                    liveRate.Item1?.ToJson().ToJson() != rateSnapshot) {
                    return null;
                }
                domain.Cache.Put(
                    ((Await)null).CacheParentKey(
                        prepared.NamespaceName,
                        token.UserId,
                        token.TimeOffset
                    ),
                    ((Await)null).CacheKey(prepared.AwaitName),
                    changed,
                    UnixTime.ToUnixTime(DateTime.Now) +
                    1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
/* diff +++ end */
                );
                return null;
            };
        }
    }
}

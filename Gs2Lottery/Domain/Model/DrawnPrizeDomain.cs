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
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Lottery.Domain.Iterator;
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Lottery.Domain.Model
{

    public partial class DrawnPrizeDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LotteryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _lotteryName;
        private readonly int _index;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string LotteryName => _lotteryName;
        public int Index => _index;

        public DrawnPrizeDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string lotteryName,
            int index
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LotteryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._lotteryName = lotteryName;
            this._index = index;
            this._parentKey = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.LotteryName,
                "DrawnPrize"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string prizeId,
            string childType
        )
        {
            return string.Join(
                ":",
                "lottery",
                namespaceName ?? "null",
                userId ?? "null",
                prizeId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            int index
        )
        {
            return string.Join(
                ":",
                index.ToString()
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Model.DrawnPrize> Model() {
            #else
        public IFuture<Gs2.Gs2Lottery.Model.DrawnPrize> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Lottery.Model.DrawnPrize> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.DrawnPrize> self)
            {
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2Lottery.Model.DrawnPrize>(
                       _parentKey,
                       Gs2.Gs2Lottery.Domain.Model.DrawnPrizeDomain.CreateCacheKey(
                            this.Index
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2Lottery.Model.DrawnPrize>(
                _parentKey,
                Gs2.Gs2Lottery.Domain.Model.DrawnPrizeDomain.CreateCacheKey(
                    this.Index
                )
            );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.DrawnPrize>(Impl);
        #endif
        }

        public ulong Subscribe(Action<Gs2.Gs2Lottery.Model.DrawnPrize> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Lottery.Domain.Model.DrawnPrizeDomain.CreateCacheKey(
                    this.Index
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Lottery.Model.DrawnPrize>(
                _parentKey,
                Gs2.Gs2Lottery.Domain.Model.DrawnPrizeDomain.CreateCacheKey(
                    this.Index
                ),
                callbackId
            );
        }

    }
}

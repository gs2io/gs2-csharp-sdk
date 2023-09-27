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
using Gs2.Gs2Showcase.Domain.Iterator;
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
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

namespace Gs2.Gs2Showcase.Domain.Model
{

    public partial class RandomShowcaseDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _showcaseName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string ShowcaseName => _showcaseName;

        public RandomShowcaseDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string showcaseName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ShowcaseRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._showcaseName = showcaseName;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "RandomShowcase"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Showcase.Model.RandomDisplayItem> RandomDisplayItems(
        )
        {
            return new DescribeRandomDisplayItemsByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.ShowcaseName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Showcase.Model.RandomDisplayItem> RandomDisplayItemsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Showcase.Model.RandomDisplayItem> RandomDisplayItems(
            #endif
        #else
        public DescribeRandomDisplayItemsByUserIdIterator RandomDisplayItems(
        #endif
        )
        {
            return new DescribeRandomDisplayItemsByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.ShowcaseName,
                this.UserId
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeRandomDisplayItems(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.ShowcaseName,
                    "RandomDisplayItem"
                ),
                callback
            );
        }

        public void UnsubscribeRandomDisplayItems(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.ShowcaseName,
                    "RandomDisplayItem"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain RandomDisplayItem(
            string displayItemName
        ) {
            return new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                this.ShowcaseName,
                displayItemName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string showcaseName,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                userId ?? "null",
                showcaseName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string showcaseName
        )
        {
            return string.Join(
                ":",
                showcaseName ?? "null"
            );
        }

    }

    public partial class RandomShowcaseDomain {

    }

    public partial class RandomShowcaseDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.RandomShowcase> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.RandomShowcase> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.RandomShowcase>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                        this.ShowcaseName?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.RandomShowcase>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Model.RandomShowcase> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.RandomShowcase>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                        this.ShowcaseName?.ToString()
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Model.RandomShowcase> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Showcase.Model.RandomShowcase> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.RandomShowcase> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Showcase.Model.RandomShowcase> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Showcase.Model.RandomShowcase> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                    this.ShowcaseName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Showcase.Model.RandomShowcase>(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                    this.ShowcaseName.ToString()
                ),
                callbackId
            );
        }

    }
}

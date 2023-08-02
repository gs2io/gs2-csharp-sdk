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

    public partial class RandomShowcaseAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _showcaseName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string ShowcaseName => _showcaseName;

        public RandomShowcaseAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
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
            this._accessToken = accessToken;
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
            return new DescribeRandomDisplayItemsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.ShowcaseName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Showcase.Model.RandomDisplayItem> RandomDisplayItemsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Showcase.Model.RandomDisplayItem> RandomDisplayItems(
            #endif
        #else
        public DescribeRandomDisplayItemsIterator RandomDisplayItems(
        #endif
        )
        {
            return new DescribeRandomDisplayItemsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.ShowcaseName,
                this.AccessToken
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

        public Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemAccessTokenDomain RandomDisplayItem(
            string displayItemName
        ) {
            return new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this._accessToken,
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Model.RandomShowcase> Model() {
            #else
        public IFuture<Gs2.Gs2Showcase.Model.RandomShowcase> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Showcase.Model.RandomShowcase> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.RandomShowcase> self)
            {
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2Showcase.Model.RandomShowcase>(
                       _parentKey,
                       Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                            this.ShowcaseName?.ToString()
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.RandomShowcase>(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                    this.ShowcaseName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.RandomShowcase>(Impl);
        #endif
        }

    }
}
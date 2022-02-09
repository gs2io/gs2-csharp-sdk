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
using Gs2.Gs2News.Domain.Iterator;
using Gs2.Gs2News.Request;
using Gs2.Gs2News.Result;
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

namespace Gs2.Gs2News.Domain.Model
{

    public partial class NewsAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2NewsRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;

        private readonly String _parentKey;
        public string BrowserUrl { get; set; }
        public string ZipUrl { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;

        public NewsAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2NewsRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2News.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                "News"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryAccessTokenDomain[]> WantGrantAsync(
            #else
        public IFuture<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryAccessTokenDomain[]> WantGrant(
            #endif
        #else
        public async Task<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryAccessTokenDomain[]> WantGrantAsync(
        #endif
            WantGrantRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryAccessTokenDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.WantGrantFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.WantGrantAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          {
                foreach (var item in resultModel.Items) {
                    var parentKey = Gs2.Gs2News.Domain.Model.UserDomain.CreateCacheParentKey(
                        requestModel.NamespaceName.ToString(),
                        this._accessToken?.UserId.ToString(),
                        "SetCookieRequestEntry"
                    );
                    var key = Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain.CreateCacheKey(
                        item.Key.ToString(),
                        item.Value.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            Gs2.Gs2News.Domain.Model.SetCookieRequestEntryAccessTokenDomain[] domain = new Gs2.Gs2News.Domain.Model.SetCookieRequestEntryAccessTokenDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2News.Domain.Model.SetCookieRequestEntryAccessTokenDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    this._accessToken,
                    result.Items[i]?.Key,
                    result.Items[i]?.Value
                );
            }
            this.BrowserUrl = result?.BrowserUrl;
            this.ZipUrl = result?.ZipUrl;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryAccessTokenDomain[]>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "news",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2News.Model.News> Model() {
            #else
        public IFuture<Gs2.Gs2News.Model.News> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2News.Model.News> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2News.Model.News> self)
            {
        #endif
            Gs2.Gs2News.Model.News value = _cache.Get<Gs2.Gs2News.Model.News>(
                _parentKey,
                Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                )
            );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Model.News>(Impl);
        #endif
        }

    }
}

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

namespace Gs2.Gs2News.Domain.Model
{

    public partial class NewsDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2NewsRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string BrowserUrl { get; set; }
        public string ZipUrl { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public NewsDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2NewsRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2News.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "News"
            );
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

    }

    public partial class NewsDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[]> WantGrantFuture(
            WantGrantByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.WantGrantByUserIdFuture(
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                WantGrantByUserIdResult result = null;
                    result = await this._client.WantGrantByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2News.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "SetCookieRequestEntry"
                        );
                        foreach (var item in resultModel.Items) {
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
                }
                var domain = new Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        request.NamespaceName,
                        request.UserId,
                        result.Items[i]?.Key,
                        result.Items[i]?.Value
                    );
                    var parentKey = Gs2.Gs2News.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SetCookieRequestEntry"
                    );
                    var key = Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain.CreateCacheKey(
                        result.Items[i].Key.ToString(),
                        result.Items[i].Value.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                this.BrowserUrl = result?.BrowserUrl;
                this.ZipUrl = result?.ZipUrl;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[]> WantGrantAsync(
            WantGrantByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var future = this._client.WantGrantByUserIdFuture(
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            WantGrantByUserIdResult result = null;
                result = await this._client.WantGrantByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2News.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SetCookieRequestEntry"
                    );
                    foreach (var item in resultModel.Items) {
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
            }
                var domain = new Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        request.NamespaceName,
                        request.UserId,
                        result.Items[i]?.Key,
                        result.Items[i]?.Value
                    );
                    var parentKey = Gs2.Gs2News.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SetCookieRequestEntry"
                    );
                    var key = Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain.CreateCacheKey(
                        result.Items[i].Key.ToString(),
                        result.Items[i].Value.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            this.BrowserUrl = result?.BrowserUrl;
            this.ZipUrl = result?.ZipUrl;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[]> WantGrantAsync(
            WantGrantByUserIdRequest request
        ) {
            var future = WantGrantFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to WantGrantFuture.")]
        public IFuture<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[]> WantGrant(
            WantGrantByUserIdRequest request
        ) {
            return WantGrantFuture(request);
        }
        #endif

    }

    public partial class NewsDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2News.Model.News> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2News.Model.News> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2News.Model.News>(
                    _parentKey,
                    Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Model.News>(Impl);
        }
        #else
        public async Task<Gs2.Gs2News.Model.News> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2News.Model.News>(
                    _parentKey,
                    Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2News.Model.News> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2News.Model.News> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2News.Model.News> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2News.Model.News> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2News.Model.News> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2News.Model.News>(
                _parentKey,
                Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                ),
                callbackId
            );
        }

    }
}

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
#pragma warning disable CS0169, CS0168

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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2NewsRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string BrowserUrl { get; set; }
        public string ZipUrl { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public NewsDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2NewsRestClient(
                gs2.RestSession
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

                var requestModel = request;
                var resultModel = result;
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
                            _gs2.Cache.Put(
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
                        this._gs2,
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
                    _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[]> WantGrantAsync(
            #else
        public async Task<Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain[]> WantGrantAsync(
            #endif
            WantGrantByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            WantGrantByUserIdResult result = null;
                result = await this._client.WantGrantByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                        _gs2.Cache.Put(
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
                        this._gs2,
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
                    _gs2.Cache.Put(
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
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2News.Model.News>(
                    _parentKey,
                    Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Model.News>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2News.Model.News> ModelAsync()
            #else
        public async Task<Gs2.Gs2News.Model.News> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2News.Model.News>(
                _parentKey,
                Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2News.Model.News>(
                    _parentKey,
                    Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                    )
                );
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
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
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2News.Model.News>(
                _parentKey,
                Gs2.Gs2News.Domain.Model.NewsDomain.CreateCacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2News.Model.News> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2News.Model.News> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2News.Model.News> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}

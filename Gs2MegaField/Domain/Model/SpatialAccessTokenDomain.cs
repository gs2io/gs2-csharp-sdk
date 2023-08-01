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
 *
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
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2MegaField.Domain.Iterator;
using Gs2.Gs2MegaField.Request;
using Gs2.Gs2MegaField.Result;
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

namespace Gs2.Gs2MegaField.Domain.Model
{

    public partial class SpatialAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MegaFieldRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _areaModelName;
        private readonly string _layerModelName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string AreaModelName => _areaModelName;
        public string LayerModelName => _layerModelName;

        public SpatialAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string areaModelName,
            string layerModelName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2MegaFieldRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._areaModelName = areaModelName;
            this._layerModelName = layerModelName;
            this._parentKey = Gs2.Gs2MegaField.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Spatial"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain> PutPositionAsync(
            #else
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain> PutPosition(
            #endif
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain> PutPositionAsync(
        #endif
            PutPositionRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithAreaModelName(this.AreaModelName)
                .WithLayerModelName(this.LayerModelName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.PutPositionFuture(
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
            var result = await this._client.PutPositionAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2MegaField.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Spatial"
                    );
                    var key = Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                        resultModel.Item.AreaModelName.ToString(),
                        resultModel.Item.LayerModelName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain[]> FetchPositionAsync(
            #else
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain[]> FetchPosition(
            #endif
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain[]> FetchPositionAsync(
        #endif
            FetchPositionRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithAreaModelName(this.AreaModelName)
                .WithLayerModelName(this.LayerModelName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.FetchPositionFuture(
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
            var result = await this._client.FetchPositionAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2MegaField.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Spatial"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                            item.AreaModelName.ToString(),
                            item.LayerModelName.ToString()
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
            var domain = new Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    this._accessToken,
                    result.Items[i]?.AreaModelName,
                    result.Items[i]?.LayerModelName
                );
                var parentKey = Gs2.Gs2MegaField.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Spatial"
            );
                var key = Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                    result.Items[i].AreaModelName.ToString(),
                    result.Items[i].LayerModelName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    result.Items[i],
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialAccessTokenDomain[]>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsAsync(
            #else
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIds(
            #endif
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsAsync(
        #endif
            NearUserIdsRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithAreaModelName(this.AreaModelName)
                .WithLayerModelName(this.LayerModelName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.NearUserIdsFuture(
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
            var result = await this._client.NearUserIdsAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
            var domain = new Gs2.Gs2MegaField.Domain.Model.SpatialDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2MegaField.Domain.Model.SpatialDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Items[i],
                    request.AreaModelName,
                    request.LayerModelName
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> ActionAsync(
            #else
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> Action(
            #endif
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> ActionAsync(
        #endif
            ActionRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithAreaModelName(this.AreaModelName)
                .WithLayerModelName(this.LayerModelName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ActionFuture(
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
            var result = await this._client.ActionAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2MegaField.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Spatial"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                            item.AreaModelName.ToString(),
                            item.LayerModelName.ToString()
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
            var domain = new Gs2.Gs2MegaField.Domain.Model.SpatialDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2MegaField.Domain.Model.SpatialDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result.Items[i]?.UserId,
                    result.Items[i]?.AreaModelName,
                    result.Items[i]?.LayerModelName
                );
                var parentKey = Gs2.Gs2MegaField.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Spatial"
            );
                var key = Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                    result.Items[i].AreaModelName.ToString(),
                    result.Items[i].LayerModelName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    result.Items[i],
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string areaModelName,
            string layerModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "megaField",
                namespaceName ?? "null",
                userId ?? "null",
                areaModelName ?? "null",
                layerModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string areaModelName,
            string layerModelName
        )
        {
            return string.Join(
                ":",
                areaModelName ?? "null",
                layerModelName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Model.Spatial> Model() {
            #else
        public IFuture<Gs2.Gs2MegaField.Model.Spatial> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2MegaField.Model.Spatial> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.Spatial> self)
            {
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2MegaField.Model.Spatial>(
                       _parentKey,
                       Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                            this.AreaModelName?.ToString(),
                            this.LayerModelName?.ToString()
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2MegaField.Model.Spatial>(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                    this.AreaModelName?.ToString(),
                    this.LayerModelName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.Spatial>(Impl);
        #endif
        }

    }
}

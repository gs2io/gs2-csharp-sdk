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

namespace Gs2.Gs2MegaField.Domain.Model
{

    public partial class UserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MegaFieldRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
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
            this._client = new Gs2MegaFieldRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        public Gs2.Gs2MegaField.Domain.Model.SpatialDomain Spatial(
            string areaModelName,
            string layerModelName
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.SpatialDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                areaModelName,
                layerModelName
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
                "megaField",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystemFuture(
            FetchPositionFromSystemRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.FetchPositionFromSystemFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                FetchPositionFromSystemResult result = null;
                    result = await this._client.FetchPositionFromSystemAsync(
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
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystemAsync(
            FetchPositionFromSystemRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.FetchPositionFromSystemFuture(
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
                .WithNamespaceName(this.NamespaceName);
            FetchPositionFromSystemResult result = null;
                result = await this._client.FetchPositionFromSystemAsync(
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
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystemAsync(
            FetchPositionFromSystemRequest request
        ) {
            var future = FetchPositionFromSystemFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to FetchPositionFromSystemFuture.")]
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystem(
            FetchPositionFromSystemRequest request
        ) {
            return FetchPositionFromSystemFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemFuture(
            NearUserIdsFromSystemRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.NearUserIdsFromSystemFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                NearUserIdsFromSystemResult result = null;
                    result = await this._client.NearUserIdsFromSystemAsync(
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
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemAsync(
            NearUserIdsFromSystemRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.NearUserIdsFromSystemFuture(
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
                .WithNamespaceName(this.NamespaceName);
            NearUserIdsFromSystemResult result = null;
                result = await this._client.NearUserIdsFromSystemAsync(
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
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemAsync(
            NearUserIdsFromSystemRequest request
        ) {
            var future = NearUserIdsFromSystemFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to NearUserIdsFromSystemFuture.")]
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystem(
            NearUserIdsFromSystemRequest request
        ) {
            return NearUserIdsFromSystemFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

    }
}

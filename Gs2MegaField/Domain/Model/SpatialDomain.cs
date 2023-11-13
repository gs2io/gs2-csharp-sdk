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

    public partial class SpatialDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MegaFieldRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _areaModelName;
        private readonly string _layerModelName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string AreaModelName => _areaModelName;
        public string LayerModelName => _layerModelName;

        public SpatialDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string areaModelName,
            string layerModelName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MegaFieldRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._areaModelName = areaModelName;
            this._layerModelName = layerModelName;
            this._parentKey = Gs2.Gs2MegaField.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Spatial"
            );
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

    }

    public partial class SpatialDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain> PutPositionFuture(
            PutPositionByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithAreaModelName(this.AreaModelName)
                    .WithLayerModelName(this.LayerModelName);
                var future = this._client.PutPositionByUserIdFuture(
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
                var cache = this._gs2.Cache;
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain> PutPositionAsync(
            #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain> PutPositionAsync(
            #endif
            PutPositionByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithAreaModelName(this.AreaModelName)
                .WithLayerModelName(this.LayerModelName);
            PutPositionByUserIdResult result = null;
                result = await this._client.PutPositionByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to PutPositionFuture.")]
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain> PutPosition(
            PutPositionByUserIdRequest request
        ) {
            return PutPositionFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> ActionFuture(
            ActionByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithAreaModelName(this.AreaModelName)
                    .WithLayerModelName(this.LayerModelName);
                var future = this._client.ActionByUserIdFuture(
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
                var cache = this._gs2.Cache;
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
                        this._gs2,
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
                    _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> ActionAsync(
            #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> ActionAsync(
            #endif
            ActionByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithAreaModelName(this.AreaModelName)
                .WithLayerModelName(this.LayerModelName);
            ActionByUserIdResult result = null;
                result = await this._client.ActionByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
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
                        this._gs2,
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
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to ActionFuture.")]
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> Action(
            ActionByUserIdRequest request
        ) {
            return ActionFuture(request);
        }
        #endif

    }

    public partial class SpatialDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Model.Spatial> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.Spatial> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2MegaField.Model.Spatial>(
                    _parentKey,
                    Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                        this.AreaModelName?.ToString(),
                        this.LayerModelName?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.Spatial>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Model.Spatial> ModelAsync()
            #else
        public async Task<Gs2.Gs2MegaField.Model.Spatial> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2MegaField.Model.Spatial>(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                    this.AreaModelName?.ToString(),
                    this.LayerModelName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2MegaField.Model.Spatial>(
                    _parentKey,
                    Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                        this.AreaModelName?.ToString(),
                        this.LayerModelName?.ToString()
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
        public async UniTask<Gs2.Gs2MegaField.Model.Spatial> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2MegaField.Model.Spatial> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2MegaField.Model.Spatial> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2MegaField.Model.Spatial> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                    this.AreaModelName.ToString(),
                    this.LayerModelName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2MegaField.Model.Spatial>(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.SpatialDomain.CreateCacheKey(
                    this.AreaModelName.ToString(),
                    this.LayerModelName.ToString()
                ),
                callbackId
            );
        }

    }
}

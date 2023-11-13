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

    public partial class AreaModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MegaFieldRestClient _client;
        private readonly string _namespaceName;
        private readonly string _areaModelName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string AreaModelName => _areaModelName;

        public AreaModelDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string areaModelName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MegaFieldRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._areaModelName = areaModelName;
            this._parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "AreaModel"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2MegaField.Model.LayerModel> LayerModels(
        )
        {
            return new DescribeLayerModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AreaModelName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2MegaField.Model.LayerModel> LayerModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2MegaField.Model.LayerModel> LayerModels(
            #endif
        #else
        public DescribeLayerModelsIterator LayerModelsAsync(
        #endif
        )
        {
            return new DescribeLayerModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AreaModelName
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

        public ulong SubscribeLayerModels(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2MegaField.Model.LayerModel>(
                Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.AreaModelName,
                    "LayerModel"
                ),
                callback
            );
        }

        public void UnsubscribeLayerModels(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2MegaField.Model.LayerModel>(
                Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.AreaModelName,
                    "LayerModel"
                ),
                callbackId
            );
        }

        public Gs2.Gs2MegaField.Domain.Model.LayerModelDomain LayerModel(
            string layerModelName
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.LayerModelDomain(
                this._gs2,
                this.NamespaceName,
                this.AreaModelName,
                layerModelName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string areaModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "megaField",
                namespaceName ?? "null",
                areaModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string areaModelName
        )
        {
            return string.Join(
                ":",
                areaModelName ?? "null"
            );
        }

    }

    public partial class AreaModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2MegaField.Model.AreaModel> GetFuture(
            GetAreaModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.AreaModel> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = this._client.GetAreaModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                            request.AreaModelName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2MegaField.Model.AreaModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "areaModel")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "AreaModel"
                        );
                        var key = Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.AreaModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2MegaField.Model.AreaModel> GetAsync(
            #else
        private async Task<Gs2.Gs2MegaField.Model.AreaModel> GetAsync(
            #endif
            GetAreaModelRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAreaModelName(this.AreaModelName);
            GetAreaModelResult result = null;
            try {
                result = await this._client.GetAreaModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                    request.AreaModelName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2MegaField.Model.AreaModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "areaModel")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "AreaModel"
                    );
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

    }

    public partial class AreaModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Model.AreaModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.AreaModel> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2MegaField.Model.AreaModel>(
                    _parentKey,
                    Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                        this.AreaModelName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetAreaModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                                    this.AreaModelName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2MegaField.Model.AreaModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "areaModel")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2MegaField.Model.AreaModel>(
                        _parentKey,
                        Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                            this.AreaModelName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.AreaModel>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Model.AreaModel> ModelAsync()
            #else
        public async Task<Gs2.Gs2MegaField.Model.AreaModel> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2MegaField.Model.AreaModel>(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                    this.AreaModelName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2MegaField.Model.AreaModel>(
                    _parentKey,
                    Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                        this.AreaModelName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetAreaModelRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                                    this.AreaModelName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2MegaField.Model.AreaModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "areaModel")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2MegaField.Model.AreaModel>(
                        _parentKey,
                        Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                            this.AreaModelName?.ToString()
                        )
                    );
                }
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2MegaField.Model.AreaModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2MegaField.Model.AreaModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2MegaField.Model.AreaModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2MegaField.Model.AreaModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                    this.AreaModelName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2MegaField.Model.AreaModel>(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.AreaModelDomain.CreateCacheKey(
                    this.AreaModelName.ToString()
                ),
                callbackId
            );
        }

    }
}

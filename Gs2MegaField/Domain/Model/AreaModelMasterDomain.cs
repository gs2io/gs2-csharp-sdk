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
using Gs2.Gs2MegaField.Model.Cache;
using Gs2.Gs2MegaField.Request;
using Gs2.Gs2MegaField.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2MegaField.Domain.Model
{

    public partial class AreaModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MegaFieldRestClient _client;
        public string NamespaceName { get; } = null!;
        public string AreaModelName { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public AreaModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string areaModelName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MegaFieldRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AreaModelName = areaModelName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2MegaField.Model.LayerModelMaster> LayerModelMasters(
        )
        {
            return new DescribeLayerModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AreaModelName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2MegaField.Model.LayerModelMaster> LayerModelMastersAsync(
            #else
        public DescribeLayerModelMastersIterator LayerModelMastersAsync(
            #endif
        )
        {
            return new DescribeLayerModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AreaModelName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeLayerModelMasters(
            Action<Gs2.Gs2MegaField.Model.LayerModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2MegaField.Model.LayerModelMaster>(
                (null as Gs2.Gs2MegaField.Model.LayerModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.AreaModelName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeLayerModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2MegaField.Model.LayerModelMaster[]> callback
        )
        {
            var items = await LayerModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeLayerModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeLayerModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2MegaField.Model.LayerModelMaster>(
                (null as Gs2.Gs2MegaField.Model.LayerModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.AreaModelName
                ),
                callbackId
            );
        }

        public Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain LayerModelMaster(
            string layerModelName
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                this.AreaModelName,
                layerModelName
            );
        }

    }

    public partial class AreaModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> GetFuture(
            GetAreaModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetAreaModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.AreaModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2MegaField.Model.AreaModelMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2MegaField.Model.AreaModelMaster> GetAsync(
            #endif
            GetAreaModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAreaModelName(this.AreaModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetAreaModelMasterAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> UpdateFuture(
            UpdateAreaModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateAreaModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> UpdateAsync(
            #endif
            UpdateAreaModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAreaModelName(this.AreaModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateAreaModelMasterAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> DeleteFuture(
            DeleteAreaModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteAreaModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> DeleteAsync(
            #endif
            DeleteAreaModelMasterRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteAreaModelMasterAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> CreateLayerModelMasterFuture(
            CreateLayerModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateLayerModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AreaModelName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> CreateLayerModelMasterAsync(
            #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> CreateLayerModelMasterAsync(
            #endif
            CreateLayerModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAreaModelName(this.AreaModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateLayerModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                this.AreaModelName,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

    }

    public partial class AreaModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> self)
            {
                var (value, find) = (null as Gs2.Gs2MegaField.Model.AreaModelMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.AreaModelName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2MegaField.Model.AreaModelMaster).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.AreaModelName,
                    () => this.GetFuture(
                        new GetAreaModelMasterRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.AreaModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Model.AreaModelMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2MegaField.Model.AreaModelMaster> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2MegaField.Model.AreaModelMaster).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.AreaModelName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2MegaField.Model.AreaModelMaster).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.AreaModelName,
                () => this.GetAsync(
                    new GetAreaModelMasterRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2MegaField.Model.AreaModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2MegaField.Model.AreaModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2MegaField.Model.AreaModelMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.AreaModelName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2MegaField.Model.AreaModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2MegaField.Model.AreaModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2MegaField.Model.AreaModelMaster).CacheKey(
                    this.AreaModelName
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                (null as Gs2.Gs2MegaField.Model.AreaModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2MegaField.Model.AreaModelMaster).CacheKey(
                    this.AreaModelName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2MegaField.Model.AreaModelMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2MegaField.Model.AreaModelMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2MegaField.Model.AreaModelMaster> callback)
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

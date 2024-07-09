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
#pragma warning disable CS0169, CS0168

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Inventory.Domain.Iterator;
using Gs2.Gs2Inventory.Model.Cache;
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Result;
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

namespace Gs2.Gs2Inventory.Domain.Model
{

    public partial class ReferenceOfAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string InventoryName { get; } = null!;
        public string ItemName { get; } = null!;
        public string ItemSetName { get; } = null!;
        public string ReferenceOf { get; } = null!;

        public ReferenceOfAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.InventoryName = inventoryName;
            this.ItemName = itemName;
            this.ItemSetName = itemSetName;
            this.ReferenceOf = referenceOf;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<string> GetFuture(
            GetReferenceOfRequest request
        ) {
            IEnumerator Impl(IFuture<string> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetReferenceOfFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<string>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<string> GetAsync(
            #else
        private async Task<string> GetAsync(
            #endif
            GetReferenceOfRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetReferenceOfAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> VerifyFuture(
            VerifyReferenceOfRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.VerifyReferenceOfFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    result?.Item
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> VerifyAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> VerifyAsync(
            #endif
            VerifyReferenceOfRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.VerifyReferenceOfAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                result?.Item
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> DeleteFuture(
            DeleteReferenceOfRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteReferenceOfFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    result?.Item
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> DeleteAsync(
            #endif
            DeleteReferenceOfRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteReferenceOfAsync(request)
                );
            }
            catch (NotFoundException e) {}
            return this;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<string> ModelFuture()
        {
            IEnumerator Impl(IFuture<string> self)
            {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.ReferenceOf).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    this.ReferenceOf
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Inventory.Model.ReferenceOf).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    this.ReferenceOf,
                    () => this.GetFuture(
                        new GetReferenceOfRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<string>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<string> ModelAsync()
            #else
        public async Task<string> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Inventory.Model.ReferenceOf).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                this.ReferenceOf
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Inventory.Model.ReferenceOf).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                this.ReferenceOf,
                () => this.GetAsync(
                    new GetReferenceOfRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<string> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<string> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<string> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inventory.Model.ReferenceOf).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                this.ReferenceOf
            );
        }

        public ulong Subscribe(Action<string> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName
                ),
                (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheKey(
                    this.ReferenceOf
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
            this._gs2.Cache.Unsubscribe<string>(
                (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName
                ),
                (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheKey(
                    this.ReferenceOf
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<string> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<string> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<string> callback)
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

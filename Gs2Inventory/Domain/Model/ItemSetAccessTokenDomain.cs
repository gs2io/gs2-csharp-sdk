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

    public partial class ItemSetAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; }
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string InventoryName { get; }
        public string ItemName { get; }
        public string ItemSetName { get; }
        public string Body { get; set; }
        public string Signature { get; set; }
        public long? OverflowCount { get; set; }

        public ItemSetAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string inventoryName,
            string itemName,
            string itemSetName
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
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> GetFuture(
            GetItemSetRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetItemSetFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Items);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.ItemSet[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
            #else
        private async Task<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
            #endif
            GetItemSetRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetItemSetAsync(request)
            );
            return result?.Items;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignatureFuture(
            GetItemWithSignatureRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetItemWithSignatureFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                this.Body = result?.Body;
                this.Signature = result?.Signature;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignatureAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignatureAsync(
            #endif
            GetItemWithSignatureRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetItemWithSignatureAsync(request)
            );
            var domain = this;
            this.Body = result?.Body;
            this.Signature = result?.Signature;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> ConsumeFuture(
            ConsumeItemSetRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ConsumeItemSetFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> ConsumeAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> ConsumeAsync(
            #endif
            ConsumeItemSetRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ConsumeItemSetAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> VerifyFuture(
            VerifyItemSetRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.VerifyItemSetFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> VerifyAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> VerifyAsync(
            #endif
            VerifyItemSetRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.VerifyItemSetAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> AddReferenceOfFuture(
            AddReferenceOfRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.AddReferenceOfFuture(request)
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
                    request.ReferenceOf
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> AddReferenceOfAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain> AddReferenceOfAsync(
            #endif
            AddReferenceOfRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.AddReferenceOfAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                request.ReferenceOf
            );

            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<string> ReferenceOves(
        )
        {
            return new DescribeReferenceOfIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.AccessToken,
                this.ItemName,
                this.ItemSetName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<string> ReferenceOvesAsync(
            #else
        public DescribeReferenceOfIterator ReferenceOvesAsync(
            #endif
        )
        {
            return new DescribeReferenceOfIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.AccessToken,
                this.ItemName,
                this.ItemSetName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain ReferenceOf(
            string referenceOf
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                referenceOf
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> self)
            {
                if (this.ItemSetName == null) {
                    var (value, find) = (null as Gs2.Gs2Inventory.Model.ItemSet[]).GetCache(
                        this._gs2.Cache,
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        this.ItemName
                    );
                    if (find) {
                        self.OnComplete(value ?? Array.Empty<Gs2.Gs2Inventory.Model.ItemSet>());
                        yield break;
                    }
                    var future = (null as Gs2.Gs2Inventory.Model.ItemSet[]).FetchFuture(
                        this._gs2.Cache,
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        this.ItemName,
                        () => this.GetFuture(
                            new GetItemSetRequest()
                        )
                    );
                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                    self.OnComplete(future.Result ?? Array.Empty<Gs2.Gs2Inventory.Model.ItemSet>());
                }
                else {
                    var (value, find) = (null as Gs2.Gs2Inventory.Model.ItemSet).GetCache(
                        this._gs2.Cache,
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        this.ItemName,
                        this.ItemSetName
                    );
                    if (find) {
                        self.OnComplete(value != null ? new []{ value } : Array.Empty<Gs2.Gs2Inventory.Model.ItemSet>());
                        yield break;
                    }
                    var future = (null as Gs2.Gs2Inventory.Model.ItemSet).FetchFuture(
                        this._gs2.Cache,
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        this.ItemName,
                        this.ItemSetName,
                        () => this.GetFuture(
                            new GetItemSetRequest()
                        )
                    );
                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                    self.OnComplete(future.Result ?? Array.Empty<Gs2.Gs2Inventory.Model.ItemSet>());
                }
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.ItemSet[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> ModelAsync()
            #else
        public async Task<Gs2.Gs2Inventory.Model.ItemSet[]> ModelAsync()
            #endif
        {
            if (this.ItemSetName == null) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.ItemSet[]).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName
                );
                if (find) {
                    return value ?? Array.Empty<Gs2.Gs2Inventory.Model.ItemSet>();
                }
                return await (null as Gs2.Gs2Inventory.Model.ItemSet[]).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    () => this.GetAsync(
                        new GetItemSetRequest()
                    )
                );
            }
            else {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.ItemSet).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName
                );
                if (find) {
                    return value != null ? new []{ value } : Array.Empty<Gs2.Gs2Inventory.Model.ItemSet>();
                }
                return await (null as Gs2.Gs2Inventory.Model.ItemSet).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    () => this.GetAsync(
                        new GetItemSetRequest()
                    )
                );
            }
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.ItemSet[]> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            if (this.ItemSetName == null) {
                (null as Gs2.Gs2Inventory.Model.ItemSet).DeleteCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName
                );
            }
            else {
                (null as Gs2.Gs2Inventory.Model.ItemSet[]).DeleteCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName
                );
            }
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback)
        {
            if (this.ItemSetName == null) {
                return this._gs2.Cache.Subscribe(
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheKey(
                        this.ItemName
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
            else {
                return this._gs2.Cache.Subscribe(
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheKey(
                        this.ItemName,
                        this.ItemSetName
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
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.ItemSet> callback) {
            if (this.ItemSetName == null) {
                throw new InvalidProgramException();
            }
            else {
                return this._gs2.Cache.Subscribe(
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheKey(
                        this.ItemName,
                        this.ItemSetName
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
        }

        public void Unsubscribe(ulong callbackId)
        {
            if (this.ItemSetName == null) {
                this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheKey(
                        this.ItemName
                    ),
                    callbackId
                );
            }
            else {
                this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheKey(
                        this.ItemName,
                        this.ItemSetName
                    ),
                    callbackId
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback)
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

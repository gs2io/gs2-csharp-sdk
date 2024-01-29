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
using Gs2.Gs2Inventory.Domain.Iterator;
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Result;
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

    public partial class SimpleInventoryAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _inventoryName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string InventoryName => _inventoryName;

        public SimpleInventoryAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string inventoryName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._inventoryName = inventoryName;
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "SimpleInventory"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain[]> ConsumeSimpleItemsFuture(
            ConsumeSimpleItemsRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain[]> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.ConsumeSimpleItemsFuture(
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
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "SimpleItem"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                item.ItemName.ToString()
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
                var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain(
                        this._gs2,
                        request.NamespaceName,
                        this._accessToken,
                        request.InventoryName,
                        result.Items[i]?.ItemName
                    );
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        result.Items[i].ItemName.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain[]> ConsumeSimpleItemsAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain[]> ConsumeSimpleItemsAsync(
            #endif
            ConsumeSimpleItemsRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName);
            ConsumeSimpleItemsResult result = null;
                result = await this._client.ConsumeSimpleItemsAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                            item.ItemName.ToString()
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
                var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain(
                        this._gs2,
                        request.NamespaceName,
                        this._accessToken,
                        request.InventoryName,
                        result.Items[i]?.ItemName
                    );
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        result.Items[i].ItemName.ToString()
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
        [Obsolete("The name has been changed to ConsumeSimpleItemsFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain[]> ConsumeSimpleItems(
            ConsumeSimpleItemsRequest request
        ) {
            return ConsumeSimpleItemsFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleItem> SimpleItems(
        )
        {
            return new DescribeSimpleItemsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.SimpleItem> SimpleItemsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleItem> SimpleItems(
            #endif
        #else
        public DescribeSimpleItemsIterator SimpleItemsAsync(
        #endif
        )
        {
            return new DescribeSimpleItemsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.AccessToken
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

        public ulong SubscribeSimpleItems(
            Action<Gs2.Gs2Inventory.Model.SimpleItem[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.SimpleItem>(
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    "SimpleItem"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSimpleItemsWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.SimpleItem[]> callback
        )
        {
            var items = await SimpleItemsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSimpleItems(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSimpleItems(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.SimpleItem>(
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    "SimpleItem"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain SimpleItem(
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                this.InventoryName,
                itemName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string inventoryName,
            string childType
        )
        {
            return string.Join(
                ":",
                "inventory",
                namespaceName ?? "null",
                userId ?? "null",
                inventoryName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string inventoryName
        )
        {
            return string.Join(
                ":",
                inventoryName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.SimpleInventory> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.SimpleInventory> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.SimpleInventory>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.SimpleInventory>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Model.SimpleInventory> ModelAsync()
            #else
        public async Task<Gs2.Gs2Inventory.Model.SimpleInventory> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.SimpleInventory>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                    this.InventoryName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.SimpleInventory>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
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
        public async UniTask<Gs2.Gs2Inventory.Model.SimpleInventory> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.SimpleInventory> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.SimpleInventory> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Inventory.Model.SimpleInventory>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.SimpleInventory> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.SimpleInventory>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.SimpleInventory> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.SimpleInventory> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.SimpleInventory> callback)
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

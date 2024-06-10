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

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string NextPageToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.Inventory> Inventories(
        )
        {
            return new DescribeInventoriesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.Inventory> InventoriesAsync(
            #else
        public DescribeInventoriesIterator InventoriesAsync(
            #endif
        )
        {
            return new DescribeInventoriesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeInventories(
            Action<Gs2.Gs2Inventory.Model.Inventory[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.Inventory>(
                (null as Gs2.Gs2Inventory.Model.Inventory).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeInventoriesWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.Inventory[]> callback
        )
        {
            var items = await InventoriesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeInventories(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeInventories(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.Inventory>(
                (null as Gs2.Gs2Inventory.Model.Inventory).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.InventoryAccessTokenDomain Inventory(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.InventoryAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                inventoryName
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.SimpleInventoryAccessTokenDomain SimpleInventory(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.SimpleInventoryAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                inventoryName
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.BigInventoryAccessTokenDomain BigInventory(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.BigInventoryAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                inventoryName
            );
        }

    }
}


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

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
    #else
using System.Collections;
using UnityEngine.Events;
using Gs2.Core;
using Gs2.Core.Exception;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain.Iterator
{

    #if UNITY_2017_1_OR_NEWER
        #if GS2_ENABLE_UNITASK
    public class DescribeReferenceOfIterator {
        #else
    public class DescribeReferenceOfIterator : Gs2Iterator<string> {
        #endif
    #else
    public class DescribeReferenceOfIterator : IAsyncEnumerable<string> {
    #endif
        private readonly CacheDatabase _cache;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _inventoryName;
        private readonly AccessToken _accessToken;
        private readonly string _itemName;
        private readonly string _itemSetName;
        private bool _last;
        private string[] _result;

        int? fetchSize;

        public DescribeReferenceOfIterator(
            CacheDatabase cache,
            Gs2InventoryRestClient client,
            string namespaceName,
            string inventoryName,
            AccessToken accessToken,
            string itemName,
            string itemSetName
        ) {
            this._cache = cache;
            this._client = client;
            this._namespaceName = namespaceName;
            this._inventoryName = inventoryName;
            this._accessToken = accessToken;
            this._itemName = itemName;
            this._itemSetName = itemSetName;
            this._last = false;
            this._result = new string[]{};

            this.fetchSize = null;
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask _load() {
            #else
        private IEnumerator _load() {
            #endif
        #else
        private async Task _load() {
        #endif
            string parentKey = "inventory:string";
            string listParentKey = parentKey;
            if (this._cache.IsListCached<string>
            (
                    listParentKey
            )) {
                this._result = this._cache.List<string>
                (
                        parentKey
                )
                    .ToArray();
                this._last = true;
            } else {

                #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = this._client.DescribeReferenceOfFuture(
                #else
                var r = await this._client.DescribeReferenceOfAsync(
                #endif
                    new Gs2.Gs2Inventory.Request.DescribeReferenceOfRequest()
                        .WithNamespaceName(this._namespaceName)
                        .WithInventoryName(this._inventoryName)
                        .WithAccessToken(this._accessToken != null ? this._accessToken.Token : null)
                        .WithItemName(this._itemName)
                        .WithItemSetName(this._itemSetName)
                );
                #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                yield return future;
                if (future.Error != null)
                {
                    Error = future.Error;
                    yield break;
                }
                var r = future.Result;
                #endif
                this._result = r.Items;
                this._last = true;
                foreach (var item in this._result) {
                    this._cache.Put(
                            parentKey,
                            item,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }

                if (this._last) {
                    this._cache.ListCached<string>(
                            listParentKey
                    );
                }
            }
        }

        private bool _hasNext()
        {
            return this._result.Length != 0 || !this._last;
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<string> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
            #else

        public override bool HasNext()
        {
            if (Error != null) return false;
            return _hasNext();
        }

        protected override IEnumerator Next(
            Action<string> callback
            #endif
        #else
        public async IAsyncEnumerator<string> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        #endif
        )
        {
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            return UniTaskAsyncEnumerable.Create<string>(async (writer, token) =>
            {
            #endif
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            while(this._hasNext()) {
        #endif
                if (this._result.Length == 0 && !this._last) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return this._load();
        #else
                    await this._load();
        #endif
                }
                if (this._result.Length == 0) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield break;
        #else
                    break;
        #endif
                }
                string ret = this._result[0];
                this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
                if (this._result.Length == 0 && !this._last) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return this._load();
        #else
                    await this._load();
        #endif
                }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
                await writer.YieldAsync(ret);
            #else
                Current = ret;
            #endif
        #else
                yield return ret;
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            }
            });
            #endif
        #else
            }
        #endif
        }
    }
}
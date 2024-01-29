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

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string Status { get; set; }
        public string Url { get; set; }
        public string UploadToken { get; set; }
        public string UploadUrl { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._parentKey = "inventory:Namespace";
        }

        public Gs2.Gs2Inventory.Domain.Model.CurrentItemModelMasterDomain CurrentItemModelMaster(
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.CurrentItemModelMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.InventoryModel> InventoryModels(
        )
        {
            return new DescribeInventoryModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.InventoryModel> InventoryModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.InventoryModel> InventoryModels(
            #endif
        #else
        public DescribeInventoryModelsIterator InventoryModelsAsync(
        #endif
        )
        {
            return new DescribeInventoryModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeInventoryModels(
            Action<Gs2.Gs2Inventory.Model.InventoryModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.InventoryModel>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "InventoryModel"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeInventoryModelsWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.InventoryModel[]> callback
        )
        {
            var items = await InventoryModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeInventoryModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeInventoryModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.InventoryModel>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "InventoryModel"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain InventoryModel(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.UserDomain(
                this._gs2,
                this.NamespaceName,
                userId
            );
        }

        public UserAccessTokenDomain AccessToken(
            AccessToken accessToken
        ) {
            return new UserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster> SimpleInventoryModelMasters(
        )
        {
            return new DescribeSimpleInventoryModelMastersIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster> SimpleInventoryModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster> SimpleInventoryModelMasters(
            #endif
        #else
        public DescribeSimpleInventoryModelMastersIterator SimpleInventoryModelMastersAsync(
        #endif
        )
        {
            return new DescribeSimpleInventoryModelMastersIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeSimpleInventoryModelMasters(
            Action<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "SimpleInventoryModelMaster"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSimpleInventoryModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster[]> callback
        )
        {
            var items = await SimpleInventoryModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSimpleInventoryModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSimpleInventoryModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "SimpleInventoryModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain SimpleInventoryModelMaster(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleInventoryModel> SimpleInventoryModels(
        )
        {
            return new DescribeSimpleInventoryModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.SimpleInventoryModel> SimpleInventoryModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleInventoryModel> SimpleInventoryModels(
            #endif
        #else
        public DescribeSimpleInventoryModelsIterator SimpleInventoryModelsAsync(
        #endif
        )
        {
            return new DescribeSimpleInventoryModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeSimpleInventoryModels(
            Action<Gs2.Gs2Inventory.Model.SimpleInventoryModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.SimpleInventoryModel>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "SimpleInventoryModel"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSimpleInventoryModelsWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.SimpleInventoryModel[]> callback
        )
        {
            var items = await SimpleInventoryModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSimpleInventoryModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSimpleInventoryModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.SimpleInventoryModel>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "SimpleInventoryModel"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelDomain SimpleInventoryModel(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> BigInventoryModelMasters(
        )
        {
            return new DescribeBigInventoryModelMastersIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> BigInventoryModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> BigInventoryModelMasters(
            #endif
        #else
        public DescribeBigInventoryModelMastersIterator BigInventoryModelMastersAsync(
        #endif
        )
        {
            return new DescribeBigInventoryModelMastersIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeBigInventoryModelMasters(
            Action<Gs2.Gs2Inventory.Model.BigInventoryModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "BigInventoryModelMaster"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeBigInventoryModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.BigInventoryModelMaster[]> callback
        )
        {
            var items = await BigInventoryModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeBigInventoryModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeBigInventoryModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "BigInventoryModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain BigInventoryModelMaster(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigInventoryModel> BigInventoryModels(
        )
        {
            return new DescribeBigInventoryModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.BigInventoryModel> BigInventoryModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigInventoryModel> BigInventoryModels(
            #endif
        #else
        public DescribeBigInventoryModelsIterator BigInventoryModelsAsync(
        #endif
        )
        {
            return new DescribeBigInventoryModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeBigInventoryModels(
            Action<Gs2.Gs2Inventory.Model.BigInventoryModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.BigInventoryModel>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "BigInventoryModel"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeBigInventoryModelsWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.BigInventoryModel[]> callback
        )
        {
            var items = await BigInventoryModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeBigInventoryModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeBigInventoryModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.BigInventoryModel>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "BigInventoryModel"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.BigInventoryModelDomain BigInventoryModel(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.BigInventoryModelDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.InventoryModelMaster> InventoryModelMasters(
        )
        {
            return new DescribeInventoryModelMastersIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.InventoryModelMaster> InventoryModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.InventoryModelMaster> InventoryModelMasters(
            #endif
        #else
        public DescribeInventoryModelMastersIterator InventoryModelMastersAsync(
        #endif
        )
        {
            return new DescribeInventoryModelMastersIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeInventoryModelMasters(
            Action<Gs2.Gs2Inventory.Model.InventoryModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "InventoryModelMaster"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeInventoryModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.InventoryModelMaster[]> callback
        )
        {
            var items = await InventoryModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeInventoryModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeInventoryModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "InventoryModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain InventoryModelMaster(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "inventory",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string namespaceName
        )
        {
            return string.Join(
                ":",
                namespaceName ?? "null"
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceStatusFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> GetStatusAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> GetStatusAsync(
            #endif
            GetNamespaceStatusRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceStatusResult result = null;
            try {
                result = await this._client.GetNamespaceStatusAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to GetStatusFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> GetStatus(
            GetNamespaceStatusRequest request
        ) {
            return GetStatusFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.Namespace> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "inventory",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Inventory.Model.Namespace> GetAsync(
            #else
        private async Task<Gs2.Gs2Inventory.Model.Namespace> GetAsync(
            #endif
            GetNamespaceRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceResult result = null;
            try {
                result = await this._client.GetNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "inventory",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateNamespaceFuture(
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
                        var parentKey = string.Join(
                            ":",
                            "inventory",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> UpdateAsync(
            #endif
            UpdateNamespaceRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            UpdateNamespaceResult result = null;
                result = await this._client.UpdateNamespaceAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "inventory",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> Update(
            UpdateNamespaceRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DeleteNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "inventory",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Inventory.Model.Namespace>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> DeleteAsync(
            #endif
            DeleteNamespaceRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            DeleteNamespaceResult result = null;
            try {
                result = await this._client.DeleteNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "inventory",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Inventory.Model.Namespace>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> Delete(
            DeleteNamespaceRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain> CreateSimpleInventoryModelMasterFuture(
            CreateSimpleInventoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateSimpleInventoryModelMasterFuture(
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
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "SimpleInventoryModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain> CreateSimpleInventoryModelMasterAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain> CreateSimpleInventoryModelMasterAsync(
            #endif
            CreateSimpleInventoryModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            CreateSimpleInventoryModelMasterResult result = null;
                result = await this._client.CreateSimpleInventoryModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "SimpleInventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateSimpleInventoryModelMasterFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain> CreateSimpleInventoryModelMaster(
            CreateSimpleInventoryModelMasterRequest request
        ) {
            return CreateSimpleInventoryModelMasterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> CreateBigInventoryModelMasterFuture(
            CreateBigInventoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateBigInventoryModelMasterFuture(
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
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BigInventoryModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> CreateBigInventoryModelMasterAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> CreateBigInventoryModelMasterAsync(
            #endif
            CreateBigInventoryModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            CreateBigInventoryModelMasterResult result = null;
                result = await this._client.CreateBigInventoryModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BigInventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateBigInventoryModelMasterFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> CreateBigInventoryModelMaster(
            CreateBigInventoryModelMasterRequest request
        ) {
            return CreateBigInventoryModelMasterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> CreateInventoryModelMasterFuture(
            CreateInventoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateInventoryModelMasterFuture(
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
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "InventoryModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> CreateInventoryModelMasterAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> CreateInventoryModelMasterAsync(
            #endif
            CreateInventoryModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            CreateInventoryModelMasterResult result = null;
                result = await this._client.CreateInventoryModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "InventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateInventoryModelMasterFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> CreateInventoryModelMaster(
            CreateInventoryModelMasterRequest request
        ) {
            return CreateInventoryModelMasterFuture(request);
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.Namespace> self)
            {
                var parentKey = string.Join(
                    ":",
                    "inventory",
                    "Namespace"
                );
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetNamespaceRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Namespace>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "namespace")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.Namespace>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Model.Namespace> ModelAsync()
            #else
        public async Task<Gs2.Gs2Inventory.Model.Namespace> ModelAsync()
            #endif
        {
            var parentKey = string.Join(
                ":",
                "inventory",
                "Namespace"
            );
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetNamespaceRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Namespace>(
                            parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "namespace")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
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
        public async UniTask<Gs2.Gs2Inventory.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Inventory.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.Namespace> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.Namespace> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.Namespace> callback)
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

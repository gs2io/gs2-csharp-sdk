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
using Gs2.Gs2Money2.Domain.Iterator;
using Gs2.Gs2Money2.Model.Cache;
using Gs2.Gs2Money2.Request;
using Gs2.Gs2Money2.Result;
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

namespace Gs2.Gs2Money2.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Money2RestClient _client;
        public string NamespaceName { get; } = null!;
        public string Status { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Money2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Money2.Model.DailyTransactionHistory> DailyTransactionHistoriesByCurrency(
            string currency,
            int? year,
            int? month = null
        )
        {
            return new DescribeDailyTransactionHistoriesByCurrencyIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                currency,
                year,
                month
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Money2.Model.DailyTransactionHistory> DailyTransactionHistoriesByCurrencyAsync(
            #else
        public DescribeDailyTransactionHistoriesByCurrencyIterator DailyTransactionHistoriesByCurrencyAsync(
            #endif
            string currency,
            int? year,
            int? month = null
        )
        {
            return new DescribeDailyTransactionHistoriesByCurrencyIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                currency,
                year,
                month
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeDailyTransactionHistoriesByCurrency(
            Action<Gs2.Gs2Money2.Model.DailyTransactionHistory[]> callback,
            string currency,
            int? year,
            int? month = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money2.Model.DailyTransactionHistory>(
                (null as Gs2.Gs2Money2.Model.DailyTransactionHistory).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDailyTransactionHistoriesByCurrencyWithInitialCallAsync(
            Action<Gs2.Gs2Money2.Model.DailyTransactionHistory[]> callback,
            string currency,
            int? year,
            int? month = null
        )
        {
            var items = await DailyTransactionHistoriesByCurrencyAsync(
                currency,
                year,
                month
            ).ToArrayAsync();
            var callbackId = SubscribeDailyTransactionHistoriesByCurrency(
                callback,
                currency,
                year,
                month
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDailyTransactionHistoriesByCurrency(
            ulong callbackId,
            string currency,
            int? year,
            int? month = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money2.Model.DailyTransactionHistory>(
                (null as Gs2.Gs2Money2.Model.DailyTransactionHistory).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Money2.Model.DailyTransactionHistory> DailyTransactionHistories(
            int? year,
            int? month = null,
            int? day = null
        )
        {
            return new DescribeDailyTransactionHistoriesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                year,
                month,
                day
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Money2.Model.DailyTransactionHistory> DailyTransactionHistoriesAsync(
            #else
        public DescribeDailyTransactionHistoriesIterator DailyTransactionHistoriesAsync(
            #endif
            int? year,
            int? month = null,
            int? day = null
        )
        {
            return new DescribeDailyTransactionHistoriesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                year,
                month,
                day
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeDailyTransactionHistories(
            Action<Gs2.Gs2Money2.Model.DailyTransactionHistory[]> callback,
            int? year,
            int? month = null,
            int? day = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money2.Model.DailyTransactionHistory>(
                (null as Gs2.Gs2Money2.Model.DailyTransactionHistory).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDailyTransactionHistoriesWithInitialCallAsync(
            Action<Gs2.Gs2Money2.Model.DailyTransactionHistory[]> callback,
            int? year,
            int? month = null,
            int? day = null
        )
        {
            var items = await DailyTransactionHistoriesAsync(
                year,
                month,
                day
            ).ToArrayAsync();
            var callbackId = SubscribeDailyTransactionHistories(
                callback,
                year,
                month,
                day
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDailyTransactionHistories(
            ulong callbackId,
            int? year,
            int? month = null,
            int? day = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money2.Model.DailyTransactionHistory>(
                (null as Gs2.Gs2Money2.Model.DailyTransactionHistory).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Money2.Domain.Model.DailyTransactionHistoryDomain DailyTransactionHistory(
            int? year,
            int? month,
            int? day,
            string currency
        ) {
            return new Gs2.Gs2Money2.Domain.Model.DailyTransactionHistoryDomain(
                this._gs2,
                this.NamespaceName,
                year,
                month,
                day,
                currency
            );
        }

        public Gs2.Gs2Money2.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Money2.Domain.Model.UserDomain(
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

        public Gs2.Gs2Money2.Domain.Model.CurrentModelMasterDomain CurrentModelMaster(
        ) {
            return new Gs2.Gs2Money2.Domain.Model.CurrentModelMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Money2.Model.StoreContentModel> StoreContentModels(
        )
        {
            return new DescribeStoreContentModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Money2.Model.StoreContentModel> StoreContentModelsAsync(
            #else
        public DescribeStoreContentModelsIterator StoreContentModelsAsync(
            #endif
        )
        {
            return new DescribeStoreContentModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeStoreContentModels(
            Action<Gs2.Gs2Money2.Model.StoreContentModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money2.Model.StoreContentModel>(
                (null as Gs2.Gs2Money2.Model.StoreContentModel).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeStoreContentModelsWithInitialCallAsync(
            Action<Gs2.Gs2Money2.Model.StoreContentModel[]> callback
        )
        {
            var items = await StoreContentModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeStoreContentModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeStoreContentModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money2.Model.StoreContentModel>(
                (null as Gs2.Gs2Money2.Model.StoreContentModel).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Money2.Domain.Model.StoreContentModelDomain StoreContentModel(
            string contentName
        ) {
            return new Gs2.Gs2Money2.Domain.Model.StoreContentModelDomain(
                this._gs2,
                this.NamespaceName,
                contentName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Money2.Model.UnusedBalance> UnusedBalances(
        )
        {
            return new DescribeUnusedBalancesIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Money2.Model.UnusedBalance> UnusedBalancesAsync(
            #else
        public DescribeUnusedBalancesIterator UnusedBalancesAsync(
            #endif
        )
        {
            return new DescribeUnusedBalancesIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeUnusedBalances(
            Action<Gs2.Gs2Money2.Model.UnusedBalance[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money2.Model.UnusedBalance>(
                (null as Gs2.Gs2Money2.Model.UnusedBalance).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeUnusedBalancesWithInitialCallAsync(
            Action<Gs2.Gs2Money2.Model.UnusedBalance[]> callback
        )
        {
            var items = await UnusedBalancesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeUnusedBalances(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeUnusedBalances(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money2.Model.UnusedBalance>(
                (null as Gs2.Gs2Money2.Model.UnusedBalance).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Money2.Domain.Model.UnusedBalanceDomain UnusedBalance(
            string currency
        ) {
            return new Gs2.Gs2Money2.Domain.Model.UnusedBalanceDomain(
                this._gs2,
                this.NamespaceName,
                currency
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Money2.Model.StoreContentModelMaster> StoreContentModelMasters(
        )
        {
            return new DescribeStoreContentModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Money2.Model.StoreContentModelMaster> StoreContentModelMastersAsync(
            #else
        public DescribeStoreContentModelMastersIterator StoreContentModelMastersAsync(
            #endif
        )
        {
            return new DescribeStoreContentModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeStoreContentModelMasters(
            Action<Gs2.Gs2Money2.Model.StoreContentModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money2.Model.StoreContentModelMaster>(
                (null as Gs2.Gs2Money2.Model.StoreContentModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeStoreContentModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2Money2.Model.StoreContentModelMaster[]> callback
        )
        {
            var items = await StoreContentModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeStoreContentModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeStoreContentModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money2.Model.StoreContentModelMaster>(
                (null as Gs2.Gs2Money2.Model.StoreContentModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain StoreContentModelMaster(
            string contentName
        ) {
            return new Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                contentName
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetNamespaceStatusFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> GetStatusAsync(
            #else
        public async Task<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> GetStatusAsync(
            #endif
            GetNamespaceStatusRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetNamespaceStatusAsync(request)
            );
            var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Money2.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Model.Namespace> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetNamespaceFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money2.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Money2.Model.Namespace> GetAsync(
            #else
        private async Task<Gs2.Gs2Money2.Model.Namespace> GetAsync(
            #endif
            GetNamespaceRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetNamespaceAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateNamespaceFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> UpdateAsync(
            #endif
            UpdateNamespaceRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateNamespaceAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteNamespaceFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> DeleteAsync(
            #endif
            DeleteNamespaceRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteNamespaceAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain> CreateStoreContentModelMasterFuture(
            CreateStoreContentModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateStoreContentModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain> CreateStoreContentModelMasterAsync(
            #else
        public async Task<Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain> CreateStoreContentModelMasterAsync(
            #endif
            CreateStoreContentModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateStoreContentModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Money2.Domain.Model.StoreContentModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Model.Namespace> self)
            {
                var (value, find) = (null as Gs2.Gs2Money2.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Money2.Model.Namespace).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    () => this.GetFuture(
                        new GetNamespaceRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money2.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Model.Namespace> ModelAsync()
            #else
        public async Task<Gs2.Gs2Money2.Model.Namespace> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Money2.Model.Namespace).GetCache(
                this._gs2.Cache,
                this.NamespaceName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Money2.Model.Namespace).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                () => this.GetAsync(
                    new GetNamespaceRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Money2.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Money2.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Money2.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Money2.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Money2.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Money2.Model.Namespace).CacheParentKey(
                ),
                (null as Gs2.Gs2Money2.Model.Namespace).CacheKey(
                    this.NamespaceName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Money2.Model.Namespace>(
                (null as Gs2.Gs2Money2.Model.Namespace).CacheParentKey(
                ),
                (null as Gs2.Gs2Money2.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Money2.Model.Namespace> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Money2.Model.Namespace> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Money2.Model.Namespace> callback)
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

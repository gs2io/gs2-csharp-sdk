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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Distributor.Domain.Iterator;
using Gs2.Gs2Distributor.Model.Cache;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Distributor.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DistributorRestClient _client;
        public string NamespaceName { get; } = null!;
        public string Status { get; set; } = null!;
        public string Result { get; set; } = null!;
        public string ContextStack { get; set; } = null!;
        public int? StatusCode { get; set; } = null!;
        public int[] VerifyTaskResultCodes { get; set; } = null!;
        public string[] VerifyTaskResults { get; set; } = null!;
        public int[] TaskResultCodes { get; set; } = null!;
        public string[] TaskResults { get; set; } = null!;
        public int? SheetResultCode { get; set; } = null!;
        public string SheetResult { get; set; } = null!;
        public Gs2.Gs2Distributor.Model.BatchResultPayload[] Results { get; set; } = null!;
/* diff --- start
        public bool? ExpressionResult { get; set; } = null!;
 diff --- end */
        public string NextPageToken { get; set; } = null!;
        public string NewContextStack { get; set; } = null!; /* diff +++ */

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DistributorRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

        public Gs2.Gs2Distributor.Domain.Model.CurrentDistributorMasterDomain CurrentDistributorMaster(
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.CurrentDistributorMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Distributor.Model.DistributorModel> DistributorModels(
        )
        {
            return new DescribeDistributorModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Distributor.Model.DistributorModel> DistributorModelsAsync(
        #else
        public DescribeDistributorModelsIterator DistributorModelsAsync(
        #endif
        )
        {
            return new DescribeDistributorModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeDistributorModels(
            Action<Gs2.Gs2Distributor.Model.DistributorModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Distributor.Model.DistributorModel>(
                (null as Gs2.Gs2Distributor.Model.DistributorModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callback,
                () =>
                {
        #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        #else
                    async Task Impl() {
        #endif
                        try {
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await DistributorModelsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDistributorModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeDistributorModelsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Distributor.Model.DistributorModel[]> callback
        )
        {
            var items = await DistributorModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeDistributorModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeDistributorModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Distributor.Model.DistributorModel>(
                (null as Gs2.Gs2Distributor.Model.DistributorModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateDistributorModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Distributor.Model.DistributorModel>(
                (null as Gs2.Gs2Distributor.Model.DistributorModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Distributor.Domain.Model.DistributorModelDomain DistributorModel(
            string distributorName
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.DistributorModelDomain(
                this._gs2,
                this.NamespaceName,
                distributorName
            );
        }

        public Gs2.Gs2Distributor.Domain.Model.DistributeDomain Distribute(
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.DistributeDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Distributor.Model.DistributorModelMaster> DistributorModelMasters(
            string namePrefix = null
        )
        {
            return new DescribeDistributorModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Distributor.Model.DistributorModelMaster> DistributorModelMastersAsync(
        #else
        public DescribeDistributorModelMastersIterator DistributorModelMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeDistributorModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeDistributorModelMasters(
            Action<Gs2.Gs2Distributor.Model.DistributorModelMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                (null as Gs2.Gs2Distributor.Model.DistributorModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callback,
                () =>
                {
        #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        #else
                    async Task Impl() {
        #endif
                        try {
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await DistributorModelMastersAsync(
                                namePrefix
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDistributorModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeDistributorModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Distributor.Model.DistributorModelMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await DistributorModelMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeDistributorModelMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeDistributorModelMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                (null as Gs2.Gs2Distributor.Model.DistributorModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateDistributorModelMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                (null as Gs2.Gs2Distributor.Model.DistributorModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain DistributorModelMaster(
            string distributorName
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                distributorName
            );
        }

        public Gs2.Gs2Distributor.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.UserDomain(
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

        public Gs2.Gs2Distributor.Domain.Model.ExpressionDomain Expression(
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.ExpressionDomain(
                this._gs2,
                this.NamespaceName
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) => GetStatusAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> GetStatusAsync(
        #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> GetStatusAsync(
        #endif
            GetNamespaceStatusRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetNamespaceStatusAsync(request)
            );
            var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Distributor.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Distributor.Model.Namespace> GetAsync(
        #else
        private async Task<Gs2.Gs2Distributor.Model.Namespace> GetAsync(
        #endif
            GetNamespaceRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetNamespaceAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> UpdateAsync(
        #endif
            UpdateNamespaceRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateNamespaceAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> DeleteAsync(
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
                    null,
                    () => this._client.DeleteNamespaceAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> SetTransactionDefaultConfigFuture(
/* diff --- start
            SetTransactionDefaultConfigByUserIdRequest request
 diff --- end */
            SetTransactionDefaultConfigRequest request /* diff +++ */
        ) => SetTransactionDefaultConfigAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> SetTransactionDefaultConfigAsync(
        #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> SetTransactionDefaultConfigAsync(
        #endif
/* diff --- start
            SetTransactionDefaultConfigByUserIdRequest request
 diff --- end */
            SetTransactionDefaultConfigRequest request /* diff +++ */
        ) {
            request = request
/* diff --- start
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserId(this.UserId);
 diff --- end */
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack); /* diff +++ */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
/* diff --- start
                () => this._client.SetTransactionDefaultConfigByUserIdAsync(request)
 diff --- end */
                () => this._client.SetTransactionDefaultConfigAsync(request) /* diff +++ */
            );
            var domain = this;
            domain.NewContextStack = result.NewContextStack; /* diff +++ */
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> BatchExecuteApiFuture(
            BatchExecuteApiRequest request
        ) => BatchExecuteApiAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> BatchExecuteApiAsync(
        #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> BatchExecuteApiAsync(
        #endif
            BatchExecuteApiRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.BatchExecuteApiAsync(request)
            );
            var domain = this;
            this.Results = domain.Results = result?.Results;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> CreateDistributorModelMasterFuture(
            CreateDistributorModelMasterRequest request
        ) => CreateDistributorModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> CreateDistributorModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> CreateDistributorModelMasterAsync(
        #endif
            CreateDistributorModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateDistributorModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Model.Namespace> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Distributor.Model.Namespace> ModelAsync()
        #else
        public async Task<Gs2.Gs2Distributor.Model.Namespace> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Distributor.Model.Namespace>(
                        (null as Gs2.Gs2Distributor.Model.Namespace).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Distributor.Model.Namespace).CacheKey(
                            this.NamespaceName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Distributor.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Distributor.Model.Namespace).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null,
                    () => this.GetAsync(
                        new GetNamespaceRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Distributor.Model.Namespace> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Distributor.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Distributor.Model.Namespace> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Distributor.Model.Namespace> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Distributor.Model.Namespace> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Distributor.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Distributor.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Distributor.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Distributor.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Distributor.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callback,
                () =>
                {
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
                    Impl().Forget();
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Distributor.Model.Namespace>(
                (null as Gs2.Gs2Distributor.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Distributor.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Distributor.Model.Namespace> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Distributor.Model.Namespace> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Distributor.Model.Namespace> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Distributor.Model.Namespace> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

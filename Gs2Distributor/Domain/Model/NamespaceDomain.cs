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
        public string NextPageToken { get; set; } = null!;
        public string NewContextStack { get; set; } = null!;

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

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
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
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

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
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await DistributorModelsAsync().ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDistributorModelsWithInitialCallAsync(
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
        #endif

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
        )
        {
            return new DescribeDistributorModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Distributor.Model.DistributorModelMaster> DistributorModelMastersAsync(
            #else
        public DescribeDistributorModelMastersIterator DistributorModelMastersAsync(
            #endif
        )
        {
            return new DescribeDistributorModelMastersIterator(
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

        public ulong SubscribeDistributorModelMasters(
            Action<Gs2.Gs2Distributor.Model.DistributorModelMaster[]> callback
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
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await DistributorModelMastersAsync().ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDistributorModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2Distributor.Model.DistributorModelMaster[]> callback
        )
        {
            var items = await DistributorModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeDistributorModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDistributorModelMasters(
            ulong callbackId
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
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Distributor.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Model.Namespace> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> SetTransactionDefaultConfigFuture(
            SetTransactionDefaultConfigRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.SetTransactionDefaultConfigFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.NewContextStack = result.NewContextStack;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> SetTransactionDefaultConfigAsync(
            #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> SetTransactionDefaultConfigAsync(
            #endif
            SetTransactionDefaultConfigRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.SetTransactionDefaultConfigAsync(request)
            );
            var domain = this;
            domain.NewContextStack = result.NewContextStack;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> BatchExecuteApiFuture(
            BatchExecuteApiRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.BatchExecuteApiFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> CreateDistributorModelMasterFuture(
            CreateDistributorModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.CreateDistributorModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Model.Namespace> self)
            {
                var (value, find) = (null as Gs2.Gs2Distributor.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Distributor.Model.Namespace).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null,
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Distributor.Model.Namespace> ModelAsync()
            #else
        public async Task<Gs2.Gs2Distributor.Model.Namespace> ModelAsync()
            #endif
        {
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
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Distributor.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Distributor.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Distributor.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
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
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
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
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Distributor.Model.Namespace> callback)
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
        #endif

    }
}

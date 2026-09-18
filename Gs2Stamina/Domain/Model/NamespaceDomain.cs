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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Stamina.Domain.Iterator;
using Gs2.Gs2Stamina.Model.Cache;
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Result;
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

namespace Gs2.Gs2Stamina.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2StaminaRestClient _client;
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
            this._client = new Gs2StaminaRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

        public Gs2.Gs2Stamina.Domain.Model.CurrentStaminaMasterDomain CurrentStaminaMaster(
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.CurrentStaminaMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> MaxStaminaTableMasters(
        )
        {
            return new DescribeMaxStaminaTableMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> MaxStaminaTableMastersAsync(
        #else
        public DescribeMaxStaminaTableMastersIterator MaxStaminaTableMastersAsync(
        #endif
        )
        {
            return new DescribeMaxStaminaTableMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeMaxStaminaTableMasters(
            Action<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                (null as Gs2.Gs2Stamina.Model.MaxStaminaTableMaster).CacheParentKey(
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
                            callback.Invoke(await MaxStaminaTableMastersAsync(
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
        public async UniTask<ulong> SubscribeMaxStaminaTableMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeMaxStaminaTableMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster[]> callback
        )
        {
            var items = await MaxStaminaTableMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeMaxStaminaTableMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeMaxStaminaTableMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                (null as Gs2.Gs2Stamina.Model.MaxStaminaTableMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateMaxStaminaTableMasters(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                (null as Gs2.Gs2Stamina.Model.MaxStaminaTableMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain MaxStaminaTableMaster(
            string maxStaminaTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain(
                this._gs2,
                this.NamespaceName,
                maxStaminaTableName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Stamina.Model.StaminaModel> StaminaModels(
        )
        {
            return new DescribeStaminaModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.StaminaModel> StaminaModelsAsync(
        #else
        public DescribeStaminaModelsIterator StaminaModelsAsync(
        #endif
        )
        {
            return new DescribeStaminaModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeStaminaModels(
            Action<Gs2.Gs2Stamina.Model.StaminaModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Stamina.Model.StaminaModel>(
                (null as Gs2.Gs2Stamina.Model.StaminaModel).CacheParentKey(
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
                            callback.Invoke(await StaminaModelsAsync(
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
        public async UniTask<ulong> SubscribeStaminaModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeStaminaModelsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Stamina.Model.StaminaModel[]> callback
        )
        {
            var items = await StaminaModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeStaminaModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeStaminaModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Stamina.Model.StaminaModel>(
                (null as Gs2.Gs2Stamina.Model.StaminaModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateStaminaModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Stamina.Model.StaminaModel>(
                (null as Gs2.Gs2Stamina.Model.StaminaModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain StaminaModel(
            string staminaName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain(
                this._gs2,
                this.NamespaceName,
                staminaName
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.UserDomain(
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
        public Gs2Iterator<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> RecoverIntervalTableMasters(
            string namePrefix = null
        )
        {
            return new DescribeRecoverIntervalTableMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> RecoverIntervalTableMastersAsync(
        #else
        public DescribeRecoverIntervalTableMastersIterator RecoverIntervalTableMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeRecoverIntervalTableMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeRecoverIntervalTableMasters(
            Action<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                (null as Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => namePrefix == null || item.Name.StartsWith(namePrefix))
                    .ToArray()),
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
                            callback.Invoke(await RecoverIntervalTableMastersAsync(
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
        public async UniTask<ulong> SubscribeRecoverIntervalTableMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeRecoverIntervalTableMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await RecoverIntervalTableMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeRecoverIntervalTableMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeRecoverIntervalTableMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                (null as Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateRecoverIntervalTableMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                (null as Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain RecoverIntervalTableMaster(
            string recoverIntervalTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain(
                this._gs2,
                this.NamespaceName,
                recoverIntervalTableName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> RecoverValueTableMasters(
            string namePrefix = null
        )
        {
            return new DescribeRecoverValueTableMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> RecoverValueTableMastersAsync(
        #else
        public DescribeRecoverValueTableMastersIterator RecoverValueTableMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeRecoverValueTableMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeRecoverValueTableMasters(
            Action<Gs2.Gs2Stamina.Model.RecoverValueTableMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                (null as Gs2.Gs2Stamina.Model.RecoverValueTableMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => namePrefix == null || item.Name.StartsWith(namePrefix))
                    .ToArray()),
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
                            callback.Invoke(await RecoverValueTableMastersAsync(
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
        public async UniTask<ulong> SubscribeRecoverValueTableMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeRecoverValueTableMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Stamina.Model.RecoverValueTableMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await RecoverValueTableMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeRecoverValueTableMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeRecoverValueTableMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                (null as Gs2.Gs2Stamina.Model.RecoverValueTableMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateRecoverValueTableMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                (null as Gs2.Gs2Stamina.Model.RecoverValueTableMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain RecoverValueTableMaster(
            string recoverValueTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain(
                this._gs2,
                this.NamespaceName,
                recoverValueTableName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Stamina.Model.StaminaModelMaster> StaminaModelMasters(
            string namePrefix = null
        )
        {
            return new DescribeStaminaModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.StaminaModelMaster> StaminaModelMastersAsync(
        #else
        public DescribeStaminaModelMastersIterator StaminaModelMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeStaminaModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeStaminaModelMasters(
            Action<Gs2.Gs2Stamina.Model.StaminaModelMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Stamina.Model.StaminaModelMaster>(
                (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => namePrefix == null || item.Name.StartsWith(namePrefix))
                    .ToArray()),
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
                            callback.Invoke(await StaminaModelMastersAsync(
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
        public async UniTask<ulong> SubscribeStaminaModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeStaminaModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Stamina.Model.StaminaModelMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await StaminaModelMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeStaminaModelMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeStaminaModelMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Stamina.Model.StaminaModelMaster>(
                (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateStaminaModelMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Stamina.Model.StaminaModelMaster>(
                (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain StaminaModelMaster(
            string staminaName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                staminaName
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) => GetStatusAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> GetStatusAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> GetStatusAsync(
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
        private IFuture<Gs2.Gs2Stamina.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Stamina.Model.Namespace> GetAsync(
        #else
        private async Task<Gs2.Gs2Stamina.Model.Namespace> GetAsync(
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> UpdateAsync(
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> DeleteAsync(
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> CreateMaxStaminaTableMasterFuture(
            CreateMaxStaminaTableMasterRequest request
        ) => CreateMaxStaminaTableMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> CreateMaxStaminaTableMasterAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> CreateMaxStaminaTableMasterAsync(
        #endif
            CreateMaxStaminaTableMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateMaxStaminaTableMasterAsync(request)
            );
            var domain = new Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> CreateRecoverIntervalTableMasterFuture(
            CreateRecoverIntervalTableMasterRequest request
        ) => CreateRecoverIntervalTableMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> CreateRecoverIntervalTableMasterAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> CreateRecoverIntervalTableMasterAsync(
        #endif
            CreateRecoverIntervalTableMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateRecoverIntervalTableMasterAsync(request)
            );
            var domain = new Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> CreateRecoverValueTableMasterFuture(
            CreateRecoverValueTableMasterRequest request
        ) => CreateRecoverValueTableMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> CreateRecoverValueTableMasterAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> CreateRecoverValueTableMasterAsync(
        #endif
            CreateRecoverValueTableMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateRecoverValueTableMasterAsync(request)
            );
            var domain = new Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> CreateStaminaModelMasterFuture(
            CreateStaminaModelMasterRequest request
        ) => CreateStaminaModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> CreateStaminaModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> CreateStaminaModelMasterAsync(
        #endif
            CreateStaminaModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateStaminaModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Model.Namespace> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Model.Namespace> ModelAsync()
        #else
        public async Task<Gs2.Gs2Stamina.Model.Namespace> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Stamina.Model.Namespace>(
                        (null as Gs2.Gs2Stamina.Model.Namespace).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Stamina.Model.Namespace).CacheKey(
                            this.NamespaceName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Stamina.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Stamina.Model.Namespace).FetchAsync(
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
        public UniTask<Gs2.Gs2Stamina.Model.Namespace> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Stamina.Model.Namespace> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Stamina.Model.Namespace> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Stamina.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Stamina.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Stamina.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Stamina.Model.Namespace).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Stamina.Model.Namespace>(
                (null as Gs2.Gs2Stamina.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Stamina.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Stamina.Model.Namespace> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Stamina.Model.Namespace> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Stamina.Model.Namespace> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

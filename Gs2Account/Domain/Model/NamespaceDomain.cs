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
using Gs2.Gs2Account.Domain.Iterator;
using Gs2.Gs2Account.Model.Cache;
using Gs2.Gs2Account.Request;
using Gs2.Gs2Account.Result;
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

namespace Gs2.Gs2Account.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2AccountRestClient _client;
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
            this._client = new Gs2AccountRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Account.Model.Account> Accounts(
        )
        {
            return new DescribeAccountsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Account.Model.Account> AccountsAsync(
            #else
        public DescribeAccountsIterator AccountsAsync(
            #endif
        )
        {
            return new DescribeAccountsIterator(
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

        public ulong SubscribeAccounts(
            Action<Gs2.Gs2Account.Model.Account[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.Account>(
                (null as Gs2.Gs2Account.Model.Account).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeAccountsWithInitialCallAsync(
            Action<Gs2.Gs2Account.Model.Account[]> callback
        )
        {
            var items = await AccountsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeAccounts(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeAccounts(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.Account>(
                (null as Gs2.Gs2Account.Model.Account).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Account.Domain.Model.AccountDomain Account(
            string userId
        ) {
            return new Gs2.Gs2Account.Domain.Model.AccountDomain(
                this._gs2,
                this.NamespaceName,
                userId
            );
        }

        public AccountAccessTokenDomain AccessToken(
            AccessToken accessToken
        ) {
            return new AccountAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                accessToken
            );
        }

        public Gs2.Gs2Account.Domain.Model.CurrentModelMasterDomain CurrentModelMaster(
        ) {
            return new Gs2.Gs2Account.Domain.Model.CurrentModelMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Account.Model.TakeOverTypeModel> TakeOverTypeModels(
        )
        {
            return new DescribeTakeOverTypeModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Account.Model.TakeOverTypeModel> TakeOverTypeModelsAsync(
            #else
        public DescribeTakeOverTypeModelsIterator TakeOverTypeModelsAsync(
            #endif
        )
        {
            return new DescribeTakeOverTypeModelsIterator(
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

        public ulong SubscribeTakeOverTypeModels(
            Action<Gs2.Gs2Account.Model.TakeOverTypeModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.TakeOverTypeModel>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModel).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeTakeOverTypeModelsWithInitialCallAsync(
            Action<Gs2.Gs2Account.Model.TakeOverTypeModel[]> callback
        )
        {
            var items = await TakeOverTypeModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeTakeOverTypeModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeTakeOverTypeModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.TakeOverTypeModel>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModel).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Account.Domain.Model.TakeOverTypeModelDomain TakeOverTypeModel(
            int? type
        ) {
            return new Gs2.Gs2Account.Domain.Model.TakeOverTypeModelDomain(
                this._gs2,
                this.NamespaceName,
                type
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Account.Model.TakeOverTypeModelMaster> TakeOverTypeModelMasters(
        )
        {
            return new DescribeTakeOverTypeModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Account.Model.TakeOverTypeModelMaster> TakeOverTypeModelMastersAsync(
            #else
        public DescribeTakeOverTypeModelMastersIterator TakeOverTypeModelMastersAsync(
            #endif
        )
        {
            return new DescribeTakeOverTypeModelMastersIterator(
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

        public ulong SubscribeTakeOverTypeModelMasters(
            Action<Gs2.Gs2Account.Model.TakeOverTypeModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.TakeOverTypeModelMaster>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeTakeOverTypeModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2Account.Model.TakeOverTypeModelMaster[]> callback
        )
        {
            var items = await TakeOverTypeModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeTakeOverTypeModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeTakeOverTypeModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.TakeOverTypeModelMaster>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Account.Domain.Model.TakeOverTypeModelMasterDomain TakeOverTypeModelMaster(
            int? type
        ) {
            return new Gs2.Gs2Account.Domain.Model.TakeOverTypeModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                type
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> self)
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetStatusAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetStatusAsync(
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
        private IFuture<Gs2.Gs2Account.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Namespace> self)
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Account.Model.Namespace> GetAsync(
            #else
        private async Task<Gs2.Gs2Account.Model.Namespace> GetAsync(
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
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> self)
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.NamespaceDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.NamespaceDomain> UpdateAsync(
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
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> self)
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.NamespaceDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.NamespaceDomain> DeleteAsync(
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
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> CreateAccountFuture(
            CreateAccountRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateAccountFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> CreateAccountAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> CreateAccountAsync(
            #endif
            CreateAccountRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateAccountAsync(request)
            );
            var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverFuture(
            DoTakeOverRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DoTakeOverFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverAsync(
            #endif
            DoTakeOverRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.DoTakeOverAsync(request)
            );
            var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverOpenIdConnectFuture(
            DoTakeOverOpenIdConnectRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DoTakeOverOpenIdConnectFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverOpenIdConnectAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverOpenIdConnectAsync(
            #endif
            DoTakeOverOpenIdConnectRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.DoTakeOverOpenIdConnectAsync(request)
            );
            var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.PlatformIdDomain> DeletePlatformIdByUserIdentifierFuture(
            DeletePlatformIdByUserIdentifierRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.PlatformIdDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeletePlatformIdByUserIdentifierFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Account.Domain.Model.PlatformIdDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type,
                    result?.Item?.UserIdentifier
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.PlatformIdDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.PlatformIdDomain> DeletePlatformIdByUserIdentifierAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.PlatformIdDomain> DeletePlatformIdByUserIdentifierAsync(
            #endif
            DeletePlatformIdByUserIdentifierRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeletePlatformIdByUserIdentifierAsync(request)
                );
                var domain = new Gs2.Gs2Account.Domain.Model.PlatformIdDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type,
                    result?.Item?.UserIdentifier
                );
                return domain;
            }
            catch (NotFoundException e) {
                return null;
            }
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Namespace> self)
            {
                var (value, find) = (null as Gs2.Gs2Account.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Account.Model.Namespace).FetchFuture(
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Model.Namespace> ModelAsync()
            #else
        public async Task<Gs2.Gs2Account.Model.Namespace> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Account.Model.Namespace).GetCache(
                this._gs2.Cache,
                this.NamespaceName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Account.Model.Namespace).FetchAsync(
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
        public async UniTask<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Account.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Account.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Account.Model.Namespace).CacheParentKey(
                ),
                (null as Gs2.Gs2Account.Model.Namespace).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Account.Model.Namespace>(
                (null as Gs2.Gs2Account.Model.Namespace).CacheParentKey(
                ),
                (null as Gs2.Gs2Account.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Account.Model.Namespace> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Account.Model.Namespace> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Account.Model.Namespace> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverByUserIdentifierFuture(
            DeleteTakeOverByUserIdentifierRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteTakeOverByUserIdentifierFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Account.Domain.Model.TakeOverDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverByUserIdentifierAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverByUserIdentifierAsync(
            #endif
            DeleteTakeOverByUserIdentifierRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteTakeOverByUserIdentifierAsync(request)
                );
                var domain = new Gs2.Gs2Account.Domain.Model.TakeOverDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type
                );
                return domain;
            }
            catch (NotFoundException e) {
                return null;
            }
        }
        #endif
    }
}

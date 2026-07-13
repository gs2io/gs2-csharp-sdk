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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
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
        public string AuthorizationUrl { get; set; } = null!; /* diff +++ */
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
            );
        }

        public ulong SubscribeAccounts(
            Action<Gs2.Gs2Account.Model.Account[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.Account>(
                (null as Gs2.Gs2Account.Model.Account).CacheParentKey(
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
                            callback.Invoke(await AccountsAsync(
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
        public async UniTask<ulong> SubscribeAccountsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeAccountsWithInitialCallAsync(
        #endif
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

        public void UnsubscribeAccounts(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.Account>(
                (null as Gs2.Gs2Account.Model.Account).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateAccounts(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Account.Model.Account>(
                (null as Gs2.Gs2Account.Model.Account).CacheParentKey(
                    this.NamespaceName,
                    null
                )
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
            );
        }

        public ulong SubscribeTakeOverTypeModels(
            Action<Gs2.Gs2Account.Model.TakeOverTypeModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.TakeOverTypeModel>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModel).CacheParentKey(
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
                            callback.Invoke(await TakeOverTypeModelsAsync(
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
        public async UniTask<ulong> SubscribeTakeOverTypeModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeTakeOverTypeModelsWithInitialCallAsync(
        #endif
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

        public void UnsubscribeTakeOverTypeModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.TakeOverTypeModel>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateTakeOverTypeModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Account.Model.TakeOverTypeModel>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
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
            );
        }

        public ulong SubscribeTakeOverTypeModelMasters(
            Action<Gs2.Gs2Account.Model.TakeOverTypeModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.TakeOverTypeModelMaster>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModelMaster).CacheParentKey(
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
                            callback.Invoke(await TakeOverTypeModelMastersAsync(
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
        public async UniTask<ulong> SubscribeTakeOverTypeModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeTakeOverTypeModelMastersWithInitialCallAsync(
        #endif
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

        public void UnsubscribeTakeOverTypeModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.TakeOverTypeModelMaster>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateTakeOverTypeModelMasters(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Account.Model.TakeOverTypeModelMaster>(
                (null as Gs2.Gs2Account.Model.TakeOverTypeModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
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
        ) => GetStatusAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                null,
                () => this._client.GetNamespaceStatusAsync(request)
            );
            var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Account.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                null,
                () => this._client.GetNamespaceAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                null,
                () => this._client.UpdateNamespaceAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                    null,
                    () => this._client.DeleteNamespaceAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> CreateAccountFuture(
            CreateAccountRequest request
        ) => CreateAccountAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverFuture(
            DoTakeOverRequest request
        ) => DoTakeOverAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverOpenIdConnectFuture(
            DoTakeOverOpenIdConnectRequest request
        ) => DoTakeOverOpenIdConnectAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

/* diff +++ start */
#if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetAuthorizationUrlFuture(
            GetAuthorizationUrlRequest request
        ) => GetAuthorizationUrlAsync(request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetAuthorizationUrlAsync(
#else
        public async Task<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetAuthorizationUrlAsync(
#endif
            GetAuthorizationUrlRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetAuthorizationUrlAsync(request)
            );
            var domain = this;
            this.AuthorizationUrl = domain.AuthorizationUrl = result?.AuthorizationUrl;
            return domain;
        }
        
/* diff +++ end */
        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.PlatformIdDomain> DeletePlatformIdByUserIdentifierFuture(
            DeletePlatformIdByUserIdentifierRequest request
        ) => DeletePlatformIdByUserIdentifierAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.PlatformIdDomain> DeletePlatformIdByUserIdentifierAsync(
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.PlatformIdDomain> DeletePlatformIdByUserIdentifierAsync(
        #endif
            DeletePlatformIdByUserIdentifierRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
/* diff --- start
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
 diff --- end */
                    .WithNamespaceName(this.NamespaceName); /* diff +++ */
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeletePlatformIdByUserIdentifierAsync(request)
                );
/* diff +++ start */
                var domain = new Gs2.Gs2Account.Domain.Model.PlatformIdDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type
                );
                return domain;
/* diff +++ end */
            }
/* diff --- start
            catch (NotFoundException e) {}
            var domain = new Gs2.Gs2Account.Domain.Model.PlatformIdDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.Type,
                result?.Item?.UserIdentifier
            );
            return domain;
 diff --- end */
/* diff +++ start */
            catch (NotFoundException e) {
                return null;
            }
/* diff +++ end */
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Model.Namespace> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Model.Namespace> ModelAsync()
        #else
        public async Task<Gs2.Gs2Account.Model.Namespace> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Account.Model.Namespace>(
                        (null as Gs2.Gs2Account.Model.Namespace).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Account.Model.Namespace).CacheKey(
                            this.NamespaceName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Account.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Account.Model.Namespace).FetchAsync(
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
        public UniTask<Gs2.Gs2Account.Model.Namespace> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Account.Model.Namespace> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Account.Model.Namespace> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Account.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Account.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Account.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Account.Model.Namespace).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Account.Model.Namespace>(
                (null as Gs2.Gs2Account.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Account.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Account.Model.Namespace> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Account.Model.Namespace> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
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

/* diff +++ start */
        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverByUserIdentifierFuture(
            DeleteTakeOverByUserIdentifierRequest request
        ) => DeleteTakeOverByUserIdentifierAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
/* diff +++ end */
    }
}

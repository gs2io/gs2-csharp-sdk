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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Idle.Domain.Iterator;
using Gs2.Gs2Idle.Model.Cache;
using Gs2.Gs2Idle.Request;
using Gs2.Gs2Idle.Result;
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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Idle.Domain.Model
{

    public partial class StatusAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdleRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string CategoryName { get; } = null!;

        public StatusAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string categoryName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdleRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.CategoryName = categoryName;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Idle.Model.Status> GetFuture(
            GetStatusRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Idle.Model.Status> GetAsync(
        #else
        private async Task<Gs2.Gs2Idle.Model.Status> GetAsync(
        #endif
            GetStatusRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithCategoryName(this.CategoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetStatusAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Model.AcquireAction[]> PredictionFuture(
            PredictionRequest request
        ) => PredictionAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Model.AcquireAction[]> PredictionAsync(
        #else
        public async Task<Gs2.Core.Model.AcquireAction[]> PredictionAsync(
        #endif
            PredictionRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithCategoryName(this.CategoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.PredictionAsync(request)
            );
            return result?.Items;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> ReceiveFuture(
            ReceiveRequest request,
            bool speculativeExecute = true
        ) => ReceiveAsync(request, speculativeExecute).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> ReceiveAsync(
        #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> ReceiveAsync(
        #endif
            ReceiveRequest request,
            bool speculativeExecute = true
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithCategoryName(this.CategoryName);

            if (speculativeExecute) {
                var commit = await Gs2.Gs2Idle.Domain.Transaction.SpeculativeExecutor.ReceiveByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    ReceiveByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.ReceiveAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult,
                result.Metadata
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Idle.Domain.Model.StatusAccessTokenDomain> DecreaseMaximumIdleMinutesFuture(
            DecreaseMaximumIdleMinutesRequest request
        ) => DecreaseMaximumIdleMinutesAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Idle.Domain.Model.StatusAccessTokenDomain> DecreaseMaximumIdleMinutesAsync(
        #else
        public async Task<Gs2.Gs2Idle.Domain.Model.StatusAccessTokenDomain> DecreaseMaximumIdleMinutesAsync(
        #endif
            DecreaseMaximumIdleMinutesRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithCategoryName(this.CategoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.DecreaseMaximumIdleMinutesAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Idle.Model.Status> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Idle.Model.Status> ModelAsync()
        #else
        public async Task<Gs2.Gs2Idle.Model.Status> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Idle.Model.Status>(
                        (null as Gs2.Gs2Idle.Model.Status).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Idle.Model.Status).CacheKey(
                            this.CategoryName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Idle.Model.Status).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Idle.Model.Status).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetStatusRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Idle.Model.Status> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Idle.Model.Status> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Idle.Model.Status> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Idle.Model.Status).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.CategoryName,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Idle.Model.Status> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Idle.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Idle.Model.Status).CacheKey(
                    this.CategoryName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Idle.Model.Status>(
                (null as Gs2.Gs2Idle.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Idle.Model.Status).CacheKey(
                    this.CategoryName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Idle.Model.Status> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Idle.Model.Status> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Idle.Model.Status> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

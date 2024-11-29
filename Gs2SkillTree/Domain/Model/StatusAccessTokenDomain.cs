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
using Gs2.Gs2SkillTree.Domain.Iterator;
using Gs2.Gs2SkillTree.Model.Cache;
using Gs2.Gs2SkillTree.Request;
using Gs2.Gs2SkillTree.Result;
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

namespace Gs2.Gs2SkillTree.Domain.Model
{

    public partial class StatusAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SkillTreeRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string PropertyId { get; } = null!;

        public StatusAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string propertyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SkillTreeRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            propertyId = propertyId?.Replace("{region}", gs2.RestSession.Region.DisplayName()).Replace("{ownerId}", gs2.RestSession.OwnerId ?? "").Replace("{userId}", UserId);
            this.PropertyId = propertyId;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> ReleaseFuture(
            ReleaseRequest request,
            bool speculativeExecute = true
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithPropertyId(this.PropertyId);

                if (speculativeExecute) {
                    var speculativeExecuteFuture = Gs2.Gs2SkillTree.Domain.Transaction.SpeculativeExecutor.ReleaseByUserIdSpeculativeExecutor.ExecuteFuture(
                        this._gs2,
                        AccessToken,
                        ReleaseByUserIdRequest.FromJson(request.ToJson())
                    );
                    yield return speculativeExecuteFuture;
                    if (speculativeExecuteFuture.Error != null)
                    {
                        self.OnError(speculativeExecuteFuture.Error);
                        yield break;
                    }
                    var commit = speculativeExecuteFuture.Result;
                    commit?.Invoke();
                }
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ReleaseFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.AccessToken,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId,
                    result.AtomicCommit,
                    result.TransactionResult
                );
                if (result.StampSheet != null) {
                    var future2 = transaction.WaitFuture(true);
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }
                self.OnComplete(transaction);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> ReleaseAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> ReleaseAsync(
            #endif
            ReleaseRequest request,
            bool speculativeExecute = true
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithPropertyId(this.PropertyId);

            if (speculativeExecute) {
                var commit = await Gs2.Gs2SkillTree.Domain.Transaction.SpeculativeExecutor.ReleaseByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    ReleaseByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ReleaseAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SkillTree.Domain.Model.StatusAccessTokenDomain> MarkRestrainFuture(
            MarkRestrainRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SkillTree.Domain.Model.StatusAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.MarkRestrainFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2SkillTree.Domain.Model.StatusAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SkillTree.Domain.Model.StatusAccessTokenDomain> MarkRestrainAsync(
            #else
        public async Task<Gs2.Gs2SkillTree.Domain.Model.StatusAccessTokenDomain> MarkRestrainAsync(
            #endif
            MarkRestrainRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.MarkRestrainAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> RestrainFuture(
            RestrainRequest request,
            bool speculativeExecute = true
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithPropertyId(this.PropertyId);

                if (speculativeExecute) {
                    var speculativeExecuteFuture = Gs2.Gs2SkillTree.Domain.Transaction.SpeculativeExecutor.RestrainByUserIdSpeculativeExecutor.ExecuteFuture(
                        this._gs2,
                        AccessToken,
                        RestrainByUserIdRequest.FromJson(request.ToJson())
                    );
                    yield return speculativeExecuteFuture;
                    if (speculativeExecuteFuture.Error != null)
                    {
                        self.OnError(speculativeExecuteFuture.Error);
                        yield break;
                    }
                    var commit = speculativeExecuteFuture.Result;
                    commit?.Invoke();
                }
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.RestrainFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.AccessToken,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId,
                    result.AtomicCommit,
                    result.TransactionResult
                );
                if (result.StampSheet != null) {
                    var future2 = transaction.WaitFuture(true);
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }
                self.OnComplete(transaction);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> RestrainAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> RestrainAsync(
            #endif
            RestrainRequest request,
            bool speculativeExecute = true
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithPropertyId(this.PropertyId);

            if (speculativeExecute) {
                var commit = await Gs2.Gs2SkillTree.Domain.Transaction.SpeculativeExecutor.RestrainByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    RestrainByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.RestrainAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2SkillTree.Model.Status> GetFuture(
            GetStatusRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SkillTree.Model.Status> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetStatusFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2SkillTree.Model.Status>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2SkillTree.Model.Status> GetAsync(
            #else
        private async Task<Gs2.Gs2SkillTree.Model.Status> GetAsync(
            #endif
            GetStatusRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetStatusAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> ResetFuture(
            ResetRequest request,
            bool speculativeExecute = true
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithPropertyId(this.PropertyId);

                if (speculativeExecute) {
                    var speculativeExecuteFuture = Gs2.Gs2SkillTree.Domain.Transaction.SpeculativeExecutor.ResetByUserIdSpeculativeExecutor.ExecuteFuture(
                        this._gs2,
                        AccessToken,
                        ResetByUserIdRequest.FromJson(request.ToJson())
                    );
                    yield return speculativeExecuteFuture;
                    if (speculativeExecuteFuture.Error != null)
                    {
                        self.OnError(speculativeExecuteFuture.Error);
                        yield break;
                    }
                    var commit = speculativeExecuteFuture.Result;
                    commit?.Invoke();
                }
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ResetFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.AccessToken,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId,
                    result.AtomicCommit,
                    result.TransactionResult
                );
                if (result.StampSheet != null) {
                    var future2 = transaction.WaitFuture(true);
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }
                self.OnComplete(transaction);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> ResetAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> ResetAsync(
            #endif
            ResetRequest request,
            bool speculativeExecute = true
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithPropertyId(this.PropertyId);

            if (speculativeExecute) {
                var commit = await Gs2.Gs2SkillTree.Domain.Transaction.SpeculativeExecutor.ResetByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    ResetByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ResetAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SkillTree.Model.Status> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2SkillTree.Model.Status> self)
            {
                var (value, find) = (null as Gs2.Gs2SkillTree.Model.Status).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.PropertyId
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2SkillTree.Model.Status).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.PropertyId,
                    () => this.GetFuture(
                        new GetStatusRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2SkillTree.Model.Status>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SkillTree.Model.Status> ModelAsync()
            #else
        public async Task<Gs2.Gs2SkillTree.Model.Status> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2SkillTree.Model.Status).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.PropertyId
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2SkillTree.Model.Status).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.PropertyId,
                () => this.GetAsync(
                    new GetStatusRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2SkillTree.Model.Status> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2SkillTree.Model.Status> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2SkillTree.Model.Status> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2SkillTree.Model.Status).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.PropertyId
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2SkillTree.Model.Status> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2SkillTree.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2SkillTree.Model.Status).CacheKey(
                    this.PropertyId
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2SkillTree.Model.Status>(
                (null as Gs2.Gs2SkillTree.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2SkillTree.Model.Status).CacheKey(
                    this.PropertyId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2SkillTree.Model.Status> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SkillTree.Model.Status> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SkillTree.Model.Status> callback)
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

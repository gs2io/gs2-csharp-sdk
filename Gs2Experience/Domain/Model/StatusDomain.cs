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
using Gs2.Gs2Experience.Domain.Iterator;
using Gs2.Gs2Experience.Model.Cache;
using Gs2.Gs2Experience.Request;
using Gs2.Gs2Experience.Result;
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

namespace Gs2.Gs2Experience.Domain.Model
{

    public partial class StatusDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ExperienceRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string ExperienceName { get; } = null!;
        public string PropertyId { get; } = null!;
        public string Body { get; set; } = null!;
        public string Signature { get; set; } = null!;

        public StatusDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string experienceName,
            string propertyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ExperienceRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.ExperienceName = experienceName;
            propertyId = propertyId?.Replace("{region}", gs2.RestSession.Region.DisplayName()).Replace("{ownerId}", gs2.RestSession.OwnerId ?? "").Replace("{userId}", UserId);
            this.PropertyId = propertyId;
        }

    }

    public partial class StatusDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Experience.Model.Status> GetFuture(
            GetStatusByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Experience.Model.Status> GetAsync(
        #else
        private async Task<Gs2.Gs2Experience.Model.Status> GetAsync(
        #endif
            GetStatusByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetStatusByUserIdAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> GetWithSignatureFuture(
            GetStatusWithSignatureByUserIdRequest request
        ) => GetWithSignatureAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> GetWithSignatureAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> GetWithSignatureAsync(
        #endif
            GetStatusWithSignatureByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetStatusWithSignatureByUserIdAsync(request)
            );
            var domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddExperienceFuture(
            AddExperienceByUserIdRequest request
        ) => AddExperienceAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddExperienceAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddExperienceAsync(
        #endif
            AddExperienceByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.AddExperienceByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> SubExperienceFuture(
            SubExperienceByUserIdRequest request
        ) => SubExperienceAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> SubExperienceAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> SubExperienceAsync(
        #endif
            SubExperienceByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SubExperienceByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetExperienceFuture(
            SetExperienceByUserIdRequest request
        ) => SetExperienceAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetExperienceAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetExperienceAsync(
        #endif
            SetExperienceByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SetExperienceByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddRankCapFuture(
            AddRankCapByUserIdRequest request
        ) => AddRankCapAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddRankCapAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddRankCapAsync(
        #endif
            AddRankCapByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.AddRankCapByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> SubRankCapFuture(
            SubRankCapByUserIdRequest request
        ) => SubRankCapAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> SubRankCapAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> SubRankCapAsync(
        #endif
            SubRankCapByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SubRankCapByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetRankCapFuture(
            SetRankCapByUserIdRequest request
        ) => SetRankCapAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetRankCapAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetRankCapAsync(
        #endif
            SetRankCapByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SetRankCapByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> DeleteFuture(
            DeleteStatusByUserIdRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> DeleteAsync(
        #endif
            DeleteStatusByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithExperienceName(this.ExperienceName)
                    .WithPropertyId(this.PropertyId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteStatusByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> VerifyRankFuture(
            VerifyRankByUserIdRequest request
        ) => VerifyRankAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> VerifyRankAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> VerifyRankAsync(
        #endif
            VerifyRankByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyRankByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> VerifyRankCapFuture(
            VerifyRankCapByUserIdRequest request
        ) => VerifyRankCapAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> VerifyRankCapAsync(
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> VerifyRankCapAsync(
        #endif
            VerifyRankCapByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyRankCapByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> MultiplyAcquireActionsFuture(
            MultiplyAcquireActionsByUserIdRequest request
        ) => MultiplyAcquireActionsAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionDomain> MultiplyAcquireActionsAsync(
        #else
        public async Task<Gs2.Core.Domain.TransactionDomain> MultiplyAcquireActionsAsync(
        #endif
            MultiplyAcquireActionsByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.MultiplyAcquireActionsByUserIdAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.UserId,
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

    }

    public partial class StatusDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Model.Status> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Model.Status> ModelAsync()
        #else
        public async Task<Gs2.Gs2Experience.Model.Status> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Experience.Model.Status>(
                        (null as Gs2.Gs2Experience.Model.Status).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            null
                        ),
                        (null as Gs2.Gs2Experience.Model.Status).CacheKey(
                            this.ExperienceName,
                            this.PropertyId
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Experience.Model.Status).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.ExperienceName,
                    this.PropertyId,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Experience.Model.Status).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.ExperienceName,
                    this.PropertyId,
                    null,
                    () => this.GetAsync(
                        new GetStatusByUserIdRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Experience.Model.Status> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Experience.Model.Status> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Experience.Model.Status> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Experience.Model.Status).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.ExperienceName,
                this.PropertyId,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Experience.Model.Status> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Experience.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Experience.Model.Status).CacheKey(
                    this.ExperienceName,
                    this.PropertyId
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Experience.Model.Status>(
                (null as Gs2.Gs2Experience.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Experience.Model.Status).CacheKey(
                    this.ExperienceName,
                    this.PropertyId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Experience.Model.Status> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Experience.Model.Status> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Experience.Model.Status> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

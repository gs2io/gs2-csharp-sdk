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
using Gs2.Gs2Grade.Domain.Iterator;
using Gs2.Gs2Grade.Model.Cache;
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Grade.Result;
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

namespace Gs2.Gs2Grade.Domain.Model
{

    public partial class StatusDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GradeRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string GradeName { get; } = null!;
        public string PropertyId { get; } = null!;
        public string ExperienceNamespaceName { get; set; } = null!;

        public StatusDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string gradeName,
            string propertyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GradeRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.GradeName = gradeName;
            propertyId = propertyId?.Replace("{region}", gs2.RestSession.Region.DisplayName()).Replace("{ownerId}", gs2.RestSession.OwnerId ?? "").Replace("{userId}", UserId);
            this.PropertyId = propertyId;
        }

    }

    public partial class StatusDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Grade.Model.Status> GetFuture(
            GetStatusByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Model.Status> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetStatusByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Model.Status>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Grade.Model.Status> GetAsync(
            #else
        private async Task<Gs2.Gs2Grade.Model.Status> GetAsync(
            #endif
            GetStatusByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetStatusByUserIdAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> AddGradeFuture(
            AddGradeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.AddGradeByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.ExperienceNamespaceName = result?.ExperienceNamespaceName;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> AddGradeAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> AddGradeAsync(
            #endif
            AddGradeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.AddGradeByUserIdAsync(request)
            );
            var domain = this;
            domain.ExperienceNamespaceName = result?.ExperienceNamespaceName;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> SubGradeFuture(
            SubGradeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.SubGradeByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.ExperienceNamespaceName = result?.ExperienceNamespaceName;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> SubGradeAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> SubGradeAsync(
            #endif
            SubGradeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.SubGradeByUserIdAsync(request)
            );
            var domain = this;
            domain.ExperienceNamespaceName = result?.ExperienceNamespaceName;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> SetGradeFuture(
            SetGradeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.SetGradeByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.ExperienceNamespaceName = result?.ExperienceNamespaceName;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> SetGradeAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> SetGradeAsync(
            #endif
            SetGradeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.SetGradeByUserIdAsync(request)
            );
            var domain = this;
            domain.ExperienceNamespaceName = result?.ExperienceNamespaceName;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> ApplyRankCapFuture(
            ApplyRankCapByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ApplyRankCapByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.ExperienceNamespaceName = result?.ExperienceNamespaceName;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> ApplyRankCapAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> ApplyRankCapAsync(
            #endif
            ApplyRankCapByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ApplyRankCapByUserIdAsync(request)
            );
            var domain = this;
            domain.ExperienceNamespaceName = result?.ExperienceNamespaceName;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> DeleteFuture(
            DeleteStatusByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteStatusByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> DeleteAsync(
            #endif
            DeleteStatusByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteStatusByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeFuture(
            VerifyGradeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.VerifyGradeByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeAsync(
            #endif
            VerifyGradeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.VerifyGradeByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeUpMaterialFuture(
            VerifyGradeUpMaterialByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.VerifyGradeUpMaterialByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeUpMaterialAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeUpMaterialAsync(
            #endif
            VerifyGradeUpMaterialByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.VerifyGradeUpMaterialByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> MultiplyAcquireActionsFuture(
            MultiplyAcquireActionsByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.MultiplyAcquireActionsByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.UserId,
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
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
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
                result.TransactionResult
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

    }

    public partial class StatusDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Model.Status> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Model.Status> self)
            {
                var (value, find) = (null as Gs2.Gs2Grade.Model.Status).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.GradeName,
                    this.PropertyId
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Grade.Model.Status).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.GradeName,
                    this.PropertyId,
                    () => this.GetFuture(
                        new GetStatusByUserIdRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Model.Status>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Model.Status> ModelAsync()
            #else
        public async Task<Gs2.Gs2Grade.Model.Status> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Grade.Model.Status).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.GradeName,
                this.PropertyId
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Grade.Model.Status).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.GradeName,
                this.PropertyId,
                () => this.GetAsync(
                    new GetStatusByUserIdRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Grade.Model.Status> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Grade.Model.Status> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Grade.Model.Status> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Grade.Model.Status).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.GradeName,
                this.PropertyId
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Grade.Model.Status> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Grade.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Grade.Model.Status).CacheKey(
                    this.GradeName,
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Grade.Model.Status>(
                (null as Gs2.Gs2Grade.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Grade.Model.Status).CacheKey(
                    this.GradeName,
                    this.PropertyId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Grade.Model.Status> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Grade.Model.Status> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Grade.Model.Status> callback)
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

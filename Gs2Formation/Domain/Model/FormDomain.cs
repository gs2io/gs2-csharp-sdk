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
using Gs2.Gs2Formation.Domain.Iterator;
using Gs2.Gs2Formation.Model.Cache;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
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

namespace Gs2.Gs2Formation.Domain.Model
{

    public partial class FormDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FormationRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string MoldModelName { get; } = null!;
        public int? Index { get; } = null!;
        public string Body { get; set; } = null!;
        public string Signature { get; set; } = null!;

        public FormDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string moldModelName,
            int? index
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FormationRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.MoldModelName = moldModelName;
            this.Index = index;
        }

    }

    public partial class FormDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.Form> GetFuture(
            GetFormByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Form> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetFormByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.Form>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Formation.Model.Form> GetAsync(
            #else
        private async Task<Gs2.Gs2Formation.Model.Form> GetAsync(
            #endif
            GetFormByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetFormByUserIdAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> GetWithSignatureFuture(
            GetFormWithSignatureByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetFormWithSignatureByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.Body = result?.Body;
                domain.Signature = result?.Signature;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormDomain> GetWithSignatureAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormDomain> GetWithSignatureAsync(
            #endif
            GetFormWithSignatureByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetFormWithSignatureByUserIdAsync(request)
            );
            var domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> SetFuture(
            SetFormByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.SetFormByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormDomain> SetAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormDomain> SetAsync(
            #endif
            SetFormByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.SetFormByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesFuture(
            AcquireActionsToFormPropertiesRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.AcquireActionsToFormPropertiesFuture(request)
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
        public async UniTask<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesAsync(
            #endif
            AcquireActionsToFormPropertiesRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.AcquireActionsToFormPropertiesAsync(request)
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> DeleteFuture(
            DeleteFormByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteFormByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormDomain> DeleteAsync(
            #endif
            DeleteFormByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteFormByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

    }

    public partial class FormDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.Form> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Form> self)
            {
                var (value, find) = (null as Gs2.Gs2Formation.Model.Form).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    this.Index ?? default
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Formation.Model.Form).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    this.Index ?? default,
                    () => this.GetFuture(
                        new GetFormByUserIdRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.Form>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Model.Form> ModelAsync()
            #else
        public async Task<Gs2.Gs2Formation.Model.Form> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Formation.Model.Form).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.MoldModelName,
                this.Index ?? default
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Formation.Model.Form).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.MoldModelName,
                this.Index ?? default,
                () => this.GetAsync(
                    new GetFormByUserIdRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Formation.Model.Form> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Formation.Model.Form> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Formation.Model.Form> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Formation.Model.Form).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.MoldModelName,
                this.Index ?? default
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Formation.Model.Form> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Formation.Model.Form).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName
                ),
                (null as Gs2.Gs2Formation.Model.Form).CacheKey(
                    this.Index ?? default
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Formation.Model.Form>(
                (null as Gs2.Gs2Formation.Model.Form).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName
                ),
                (null as Gs2.Gs2Formation.Model.Form).CacheKey(
                    this.Index ?? default
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Formation.Model.Form> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Formation.Model.Form> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Formation.Model.Form> callback)
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

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

    public partial class StaminaDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2StaminaRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string StaminaName { get; } = null!;
        public int? OverflowValue { get; set; } = null!;

        public StaminaDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string staminaName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2StaminaRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.StaminaName = staminaName;
        }

    }

    public partial class StaminaDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Stamina.Model.Stamina> GetFuture(
            GetStaminaByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Stamina.Model.Stamina> GetAsync(
        #else
        private async Task<Gs2.Gs2Stamina.Model.Stamina> GetAsync(
        #endif
            GetStaminaByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetStaminaByUserIdAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> UpdateFuture(
            UpdateStaminaByUserIdRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> UpdateAsync(
        #endif
            UpdateStaminaByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.UpdateStaminaByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ConsumeFuture(
            ConsumeStaminaByUserIdRequest request
        ) => ConsumeAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ConsumeAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ConsumeAsync(
        #endif
            ConsumeStaminaByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.ConsumeStaminaByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ApplyFuture(
            ApplyStaminaByUserIdRequest request
        ) => ApplyAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ApplyAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ApplyAsync(
        #endif
            ApplyStaminaByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.ApplyStaminaByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RecoverFuture(
            RecoverStaminaByUserIdRequest request
        ) => RecoverAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RecoverAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RecoverAsync(
        #endif
            RecoverStaminaByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.RecoverStaminaByUserIdAsync(request)
            );
            var domain = this;
            domain.OverflowValue = result?.OverflowValue;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValueFuture(
            RaiseMaxValueByUserIdRequest request
        ) => RaiseMaxValueAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValueAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValueAsync(
        #endif
            RaiseMaxValueByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.RaiseMaxValueByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DecreaseMaxValueFuture(
            DecreaseMaxValueByUserIdRequest request
        ) => DecreaseMaxValueAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DecreaseMaxValueAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DecreaseMaxValueAsync(
        #endif
            DecreaseMaxValueByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.DecreaseMaxValueByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValueFuture(
            SetMaxValueByUserIdRequest request
        ) => SetMaxValueAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValueAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValueAsync(
        #endif
            SetMaxValueByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SetMaxValueByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverIntervalFuture(
            SetRecoverIntervalByUserIdRequest request
        ) => SetRecoverIntervalAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverIntervalAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverIntervalAsync(
        #endif
            SetRecoverIntervalByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SetRecoverIntervalByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValueFuture(
            SetRecoverValueByUserIdRequest request
        ) => SetRecoverValueAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValueAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValueAsync(
        #endif
            SetRecoverValueByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SetRecoverValueByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DeleteFuture(
            DeleteStaminaByUserIdRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DeleteAsync(
        #endif
            DeleteStaminaByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithStaminaName(this.StaminaName)
                    .WithUserId(this.UserId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteStaminaByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyValueFuture(
            VerifyStaminaValueByUserIdRequest request
        ) => VerifyValueAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyValueAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyValueAsync(
        #endif
            VerifyStaminaValueByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyStaminaValueByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyMaxValueFuture(
            VerifyStaminaMaxValueByUserIdRequest request
        ) => VerifyMaxValueAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyMaxValueAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyMaxValueAsync(
        #endif
            VerifyStaminaMaxValueByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyStaminaMaxValueByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyRecoverIntervalMinutesFuture(
            VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        ) => VerifyRecoverIntervalMinutesAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyRecoverIntervalMinutesAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyRecoverIntervalMinutesAsync(
        #endif
            VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyStaminaRecoverIntervalMinutesByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyRecoverValueFuture(
            VerifyStaminaRecoverValueByUserIdRequest request
        ) => VerifyRecoverValueAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyRecoverValueAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyRecoverValueAsync(
        #endif
            VerifyStaminaRecoverValueByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyStaminaRecoverValueByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyOverflowValueFuture(
            VerifyStaminaOverflowValueByUserIdRequest request
        ) => VerifyOverflowValueAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyOverflowValueAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> VerifyOverflowValueAsync(
        #endif
            VerifyStaminaOverflowValueByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyStaminaOverflowValueByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

    }

    public partial class StaminaDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Model.Stamina> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Model.Stamina> ModelAsync()
        #else
        public async Task<Gs2.Gs2Stamina.Model.Stamina> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Stamina.Model.Stamina>(
                        (null as Gs2.Gs2Stamina.Model.Stamina).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            null
                        ),
                        (null as Gs2.Gs2Stamina.Model.Stamina).CacheKey(
                            this.StaminaName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Stamina.Model.Stamina).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.StaminaName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Stamina.Model.Stamina).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.StaminaName,
                    null,
                    () => this.GetAsync(
                        new GetStaminaByUserIdRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Stamina.Model.Stamina> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Stamina.Model.Stamina> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Stamina.Model.Stamina> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Stamina.Model.Stamina).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.StaminaName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Stamina.Model.Stamina> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Stamina.Model.Stamina).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Stamina.Model.Stamina).CacheKey(
                    this.StaminaName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Stamina.Model.Stamina>(
                (null as Gs2.Gs2Stamina.Model.Stamina).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Stamina.Model.Stamina).CacheKey(
                    this.StaminaName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Stamina.Model.Stamina> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Stamina.Model.Stamina> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Stamina.Model.Stamina> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

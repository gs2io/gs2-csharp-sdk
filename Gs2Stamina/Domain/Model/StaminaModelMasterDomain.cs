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

    public partial class StaminaModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2StaminaRestClient _client;
        public string NamespaceName { get; } = null!;
        public string StaminaName { get; } = null!;

        public StaminaModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string staminaName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2StaminaRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.StaminaName = staminaName;
        }

    }

    public partial class StaminaModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Stamina.Model.StaminaModelMaster> GetFuture(
            GetStaminaModelMasterRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Stamina.Model.StaminaModelMaster> GetAsync(
        #else
        private async Task<Gs2.Gs2Stamina.Model.StaminaModelMaster> GetAsync(
        #endif
            GetStaminaModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetStaminaModelMasterAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> UpdateFuture(
            UpdateStaminaModelMasterRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> UpdateAsync(
        #endif
            UpdateStaminaModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithStaminaName(this.StaminaName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateStaminaModelMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> DeleteFuture(
            DeleteStaminaModelMasterRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> DeleteAsync(
        #endif
            DeleteStaminaModelMasterRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithStaminaName(this.StaminaName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeleteStaminaModelMasterAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

    }

    public partial class StaminaModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Model.StaminaModelMaster> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Model.StaminaModelMaster> ModelAsync()
        #else
        public async Task<Gs2.Gs2Stamina.Model.StaminaModelMaster> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Stamina.Model.StaminaModelMaster>(
                        (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheKey(
                            this.StaminaName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.StaminaName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.StaminaName,
                    null,
                    () => this.GetAsync(
                        new GetStaminaModelMasterRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Stamina.Model.StaminaModelMaster> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Stamina.Model.StaminaModelMaster> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Stamina.Model.StaminaModelMaster> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.StaminaName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Stamina.Model.StaminaModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Stamina.Model.StaminaModelMaster>(
                (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Stamina.Model.StaminaModelMaster).CacheKey(
                    this.StaminaName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Stamina.Model.StaminaModelMaster> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Stamina.Model.StaminaModelMaster> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Stamina.Model.StaminaModelMaster> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

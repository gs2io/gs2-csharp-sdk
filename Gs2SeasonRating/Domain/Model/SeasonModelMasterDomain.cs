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
using Gs2.Gs2SeasonRating.Domain.Iterator;
using Gs2.Gs2SeasonRating.Model.Cache;
using Gs2.Gs2SeasonRating.Request;
using Gs2.Gs2SeasonRating.Result;
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

namespace Gs2.Gs2SeasonRating.Domain.Model
{

    public partial class SeasonModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SeasonRatingRestClient _client;
        public string NamespaceName { get; } = null!;
        public string SeasonName { get; } = null!;

        public SeasonModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string seasonName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SeasonRatingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.SeasonName = seasonName;
        }

    }

    public partial class SeasonModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> GetFuture(
            GetSeasonModelMasterRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> GetAsync(
        #else
        private async Task<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> GetAsync(
        #endif
            GetSeasonModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithSeasonName(this.SeasonName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetSeasonModelMasterAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> UpdateFuture(
            UpdateSeasonModelMasterRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> UpdateAsync(
        #endif
            UpdateSeasonModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithSeasonName(this.SeasonName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateSeasonModelMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> DeleteFuture(
            DeleteSeasonModelMasterRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> DeleteAsync(
        #endif
            DeleteSeasonModelMasterRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithSeasonName(this.SeasonName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeleteSeasonModelMasterAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

    }

    public partial class SeasonModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> ModelAsync()
        #else
        public async Task<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2SeasonRating.Model.SeasonModelMaster>(
                        (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).CacheKey(
                            this.SeasonName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.SeasonName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.SeasonName,
                    null,
                    () => this.GetAsync(
                        new GetSeasonModelMasterRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.SeasonName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).CacheKey(
                    this.SeasonName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2SeasonRating.Model.SeasonModelMaster>(
                (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).CacheKey(
                    this.SeasonName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Grade.Domain.Model
{

    public partial class GradeModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GradeRestClient _client;
        public string NamespaceName { get; } = null!;
        public string GradeName { get; } = null!;

        public GradeModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string gradeName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GradeRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.GradeName = gradeName;
        }

    }

    public partial class GradeModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Grade.Model.GradeModelMaster> GetFuture(
            GetGradeModelMasterRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Grade.Model.GradeModelMaster> GetAsync(
        #else
        private async Task<Gs2.Gs2Grade.Model.GradeModelMaster> GetAsync(
        #endif
            GetGradeModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGradeName(this.GradeName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetGradeModelMasterAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.GradeModelMasterDomain> UpdateFuture(
            UpdateGradeModelMasterRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Grade.Domain.Model.GradeModelMasterDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Grade.Domain.Model.GradeModelMasterDomain> UpdateAsync(
        #endif
            UpdateGradeModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGradeName(this.GradeName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateGradeModelMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.GradeModelMasterDomain> DeleteFuture(
            DeleteGradeModelMasterRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Grade.Domain.Model.GradeModelMasterDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Grade.Domain.Model.GradeModelMasterDomain> DeleteAsync(
        #endif
            DeleteGradeModelMasterRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGradeName(this.GradeName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeleteGradeModelMasterAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

    }

    public partial class GradeModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Model.GradeModelMaster> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Grade.Model.GradeModelMaster> ModelAsync()
        #else
        public async Task<Gs2.Gs2Grade.Model.GradeModelMaster> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Grade.Model.GradeModelMaster>(
                        (null as Gs2.Gs2Grade.Model.GradeModelMaster).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Grade.Model.GradeModelMaster).CacheKey(
                            this.GradeName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Grade.Model.GradeModelMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.GradeName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Grade.Model.GradeModelMaster).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.GradeName,
                    null,
                    () => this.GetAsync(
                        new GetGradeModelMasterRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Grade.Model.GradeModelMaster> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Grade.Model.GradeModelMaster> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Grade.Model.GradeModelMaster> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Grade.Model.GradeModelMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.GradeName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Grade.Model.GradeModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Grade.Model.GradeModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Grade.Model.GradeModelMaster).CacheKey(
                    this.GradeName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Grade.Model.GradeModelMaster>(
                (null as Gs2.Gs2Grade.Model.GradeModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Grade.Model.GradeModelMaster).CacheKey(
                    this.GradeName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Grade.Model.GradeModelMaster> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Grade.Model.GradeModelMaster> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Grade.Model.GradeModelMaster> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

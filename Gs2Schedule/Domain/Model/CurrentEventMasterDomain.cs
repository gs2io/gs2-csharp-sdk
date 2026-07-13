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
using Gs2.Gs2Schedule.Domain.Iterator;
using Gs2.Gs2Schedule.Model.Cache;
using Gs2.Gs2Schedule.Request;
using Gs2.Gs2Schedule.Result;
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

namespace Gs2.Gs2Schedule.Domain.Model
{

    public partial class CurrentEventMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ScheduleRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public CurrentEventMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ScheduleRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

    }

    public partial class CurrentEventMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> ExportMasterFuture(
            ExportMasterRequest request
        ) => ExportMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> ExportMasterAsync(
        #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> ExportMasterAsync(
        #endif
            ExportMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.ExportMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Schedule.Model.CurrentEventMaster> GetFuture(
            GetCurrentEventMasterRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Schedule.Model.CurrentEventMaster> GetAsync(
        #else
        private async Task<Gs2.Gs2Schedule.Model.CurrentEventMaster> GetAsync(
        #endif
            GetCurrentEventMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetCurrentEventMasterAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> PreUpdateFuture(
            PreUpdateCurrentEventMasterRequest request
        ) => PreUpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> PreUpdateAsync(
        #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> PreUpdateAsync(
        #endif
            PreUpdateCurrentEventMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.PreUpdateCurrentEventMasterAsync(request)
            );
            var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> UpdateFuture(
            UpdateCurrentEventMasterRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> UpdateAsync(
        #endif
            UpdateCurrentEventMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateCurrentEventMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> UpdateFromGitHubFuture(
            UpdateCurrentEventMasterFromGitHubRequest request
        ) => UpdateFromGitHubAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> UpdateFromGitHubAsync(
        #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.CurrentEventMasterDomain> UpdateFromGitHubAsync(
        #endif
            UpdateCurrentEventMasterFromGitHubRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateCurrentEventMasterFromGitHubAsync(request)
            );
            var domain = this;

            return domain;
        }

    }

    public partial class CurrentEventMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Model.CurrentEventMaster> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Schedule.Model.CurrentEventMaster> ModelAsync()
        #else
        public async Task<Gs2.Gs2Schedule.Model.CurrentEventMaster> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Schedule.Model.CurrentEventMaster>(
                        (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).CacheKey(
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null,
                    () => this.GetAsync(
                        new GetCurrentEventMasterRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Schedule.Model.CurrentEventMaster> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Schedule.Model.CurrentEventMaster> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Schedule.Model.CurrentEventMaster> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Schedule.Model.CurrentEventMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Schedule.Model.CurrentEventMaster>(
                (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Schedule.Model.CurrentEventMaster).CacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Schedule.Model.CurrentEventMaster> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Schedule.Model.CurrentEventMaster> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Schedule.Model.CurrentEventMaster> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

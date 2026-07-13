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
using Gs2.Gs2Showcase.Domain.Iterator;
using Gs2.Gs2Showcase.Model.Cache;
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
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

namespace Gs2.Gs2Showcase.Domain.Model
{

    public partial class CurrentShowcaseMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ShowcaseRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public CurrentShowcaseMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ShowcaseRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

    }

    public partial class CurrentShowcaseMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> ExportMasterFuture(
            ExportMasterRequest request
        ) => ExportMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> ExportMasterAsync(
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> ExportMasterAsync(
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
        private IFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> GetFuture(
            GetCurrentShowcaseMasterRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> GetAsync(
        #else
        private async Task<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> GetAsync(
        #endif
            GetCurrentShowcaseMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetCurrentShowcaseMasterAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> PreUpdateFuture(
            PreUpdateCurrentShowcaseMasterRequest request
        ) => PreUpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> PreUpdateAsync(
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> PreUpdateAsync(
        #endif
            PreUpdateCurrentShowcaseMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.PreUpdateCurrentShowcaseMasterAsync(request)
            );
            var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFuture(
            UpdateCurrentShowcaseMasterRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateAsync(
        #endif
            UpdateCurrentShowcaseMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateCurrentShowcaseMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFromGitHubFuture(
            UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) => UpdateFromGitHubAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFromGitHubAsync(
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFromGitHubAsync(
        #endif
            UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateCurrentShowcaseMasterFromGitHubAsync(request)
            );
            var domain = this;

            return domain;
        }

    }

    public partial class CurrentShowcaseMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> ModelAsync()
        #else
        public async Task<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                        (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).CacheKey(
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null,
                    () => this.GetAsync(
                        new GetCurrentShowcaseMasterRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Showcase.Model.CurrentShowcaseMaster).CacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

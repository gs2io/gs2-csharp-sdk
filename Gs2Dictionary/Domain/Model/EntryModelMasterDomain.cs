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
 * deny overwrite
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
/* diff --- start
using System.Collections;
 diff --- end */
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Dictionary.Domain.Iterator;
using Gs2.Gs2Dictionary.Model.Cache;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections; /* diff +++ */
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Dictionary.Domain.Model
{

    public partial class EntryModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DictionaryRestClient _client;
        public string NamespaceName { get; } = null!;
/* diff --- start
        public string EntryModelName { get; } = null!;
 diff --- end */
        public string EntryName { get; } = null!; /* diff +++ */

        public EntryModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
/* diff --- start
            string entryModelName
 diff --- end */
            string entryName /* diff +++ */
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DictionaryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
/* diff --- start
            this.EntryModelName = entryModelName;
 diff --- end */
            this.EntryName = entryName; /* diff +++ */
        }

    }

    public partial class EntryModelMasterDomain {
/* diff +++ start */

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> GetFuture(
            GetEntryModelMasterRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Dictionary.Model.EntryModelMaster> GetAsync(
        #else
        private async Task<Gs2.Gs2Dictionary.Model.EntryModelMaster> GetAsync(
        #endif
            GetEntryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithEntryName(this.EntryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetEntryModelMasterAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> UpdateFuture(
            UpdateEntryModelMasterRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> UpdateAsync(
        #endif
            UpdateEntryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithEntryName(this.EntryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateEntryModelMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> DeleteFuture(
            DeleteEntryModelMasterRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> DeleteAsync(
        #endif
            DeleteEntryModelMasterRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithEntryName(this.EntryName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeleteEntryModelMasterAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
/* diff +++ end */

    }

    public partial class EntryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Model.EntryModelMaster> ModelAsync()
        #else
        public async Task<Gs2.Gs2Dictionary.Model.EntryModelMaster> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                        (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).CacheKey(
                            this.EntryModelName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.EntryModelName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.EntryModelName,
                    null,
                    () => this.GetAsync(
                        new GetEntryModelMasterRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.EntryName,
                null
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.EntryName,
                null,
                () => this.GetAsync(
                    new GetEntryModelMasterRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
/* diff --- start
                this.EntryModelName,
 diff --- end */
                this.EntryName, /* diff +++ */
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Dictionary.Model.EntryModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).CacheKey(
/* diff --- start
                    this.EntryModelName
 diff --- end */
                    this.EntryName /* diff +++ */
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK // diff +++
            #if GS2_ENABLE_UNITASK
/* diff --- start
                    async UniTask Impl() {
 diff --- end */
                    ModelAsync().Forget(); /* diff +++ */
            #else
/* diff --- start
                    async Task Impl() {
 diff --- end */
                    ModelAsync(); /* diff +++ */
            #endif
/* diff --- start
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
 diff --- end */
        #endif // diff +++
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Dictionary.Model.EntryModelMaster).CacheKey(
/* diff --- start
                    this.EntryModelName
 diff --- end */
                    this.EntryName /* diff +++ */
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Dictionary.Model.EntryModelMaster> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Dictionary.Model.EntryModelMaster> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Dictionary.Model.EntryModelMaster> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Dictionary.Model.EntryModelMaster> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

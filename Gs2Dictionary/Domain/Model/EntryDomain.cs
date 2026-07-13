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
using System.Collections;
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

    public partial class EntryDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DictionaryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string EntryModelName { get; } = null!;
        public string Body { get; set; } = null!;
        public string Signature { get; set; } = null!;

        public EntryDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string entryModelName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DictionaryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.EntryModelName = entryModelName;
        }

    }

    public partial class EntryDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Dictionary.Model.Entry> GetFuture(
            GetEntryByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Dictionary.Model.Entry> GetAsync(
        #else
        private async Task<Gs2.Gs2Dictionary.Model.Entry> GetAsync(
        #endif
            GetEntryByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
/* diff --- start
                .WithEntryModelName(this.EntryModelName)
                .WithEntryModelName(this.EntryName);
 diff --- end */
                .WithEntryModelName(this.EntryModelName); /* diff +++ */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetEntryByUserIdAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain> GetWithSignatureFuture(
            GetEntryWithSignatureByUserIdRequest request
        ) => GetWithSignatureAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryDomain> GetWithSignatureAsync(
        #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryDomain> GetWithSignatureAsync(
        #endif
            GetEntryWithSignatureByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
/* diff --- start
                .WithEntryModelName(this.EntryModelName)
                .WithEntryModelName(this.EntryName);
 diff --- end */
                .WithEntryModelName(this.EntryModelName); /* diff +++ */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetEntryWithSignatureByUserIdAsync(request)
            );
            var domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain> VerifyFuture(
            VerifyEntryByUserIdRequest request
        ) => VerifyAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryDomain> VerifyAsync(
        #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryDomain> VerifyAsync(
        #endif
            VerifyEntryByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithEntryModelName(this.EntryModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyEntryByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

    }

    public partial class EntryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Model.Entry> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Model.Entry> ModelAsync()
        #else
        public async Task<Gs2.Gs2Dictionary.Model.Entry> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Dictionary.Model.Entry>(
                        (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            null
                        ),
                        (null as Gs2.Gs2Dictionary.Model.Entry).CacheKey(
                            this.EntryModelName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Dictionary.Model.Entry).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.EntryModelName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Dictionary.Model.Entry).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.EntryModelName,
                    null,
                    () => this.GetAsync(
                        new GetEntryByUserIdRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Dictionary.Model.Entry).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.EntryModelName,
                null
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Dictionary.Model.Entry).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.EntryModelName,
                null,
                () => this.GetAsync(
                    new GetEntryByUserIdRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Dictionary.Model.Entry> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Dictionary.Model.Entry> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Dictionary.Model.Entry> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Dictionary.Model.Entry> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Dictionary.Model.Entry> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Dictionary.Model.Entry> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Dictionary.Model.Entry).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.EntryModelName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Dictionary.Model.Entry> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheKey(
                    this.EntryModelName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Dictionary.Model.Entry>(
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheKey(
                    this.EntryModelName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Dictionary.Model.Entry> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Dictionary.Model.Entry> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Dictionary.Model.Entry> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Dictionary.Model.Entry> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

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
using Gs2.Gs2Key.Domain.Iterator;
using Gs2.Gs2Key.Model.Cache;
using Gs2.Gs2Key.Request;
using Gs2.Gs2Key.Result;
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

namespace Gs2.Gs2Key.Domain.Model
{

    public partial class KeyDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2KeyRestClient _client;
        public string NamespaceName { get; } = null!;
        public string KeyName { get; } = null!;
        public string Data { get; set; } = null!;

        public KeyDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string keyName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2KeyRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.KeyName = keyName;
        }

    }

    public partial class KeyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> UpdateFuture(
            UpdateKeyRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateKeyFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> UpdateAsync(
            #endif
            UpdateKeyRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateKeyAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Key.Model.Key> GetFuture(
            GetKeyRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.Key> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetKeyFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.Key>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Key.Model.Key> GetAsync(
            #else
        private async Task<Gs2.Gs2Key.Model.Key> GetAsync(
            #endif
            GetKeyRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetKeyAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> DeleteFuture(
            DeleteKeyRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteKeyFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> DeleteAsync(
            #endif
            DeleteKeyRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteKeyAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> EncryptFuture(
            EncryptRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.EncryptFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                this.Data = domain.Data = result?.Data;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> EncryptAsync(
            #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> EncryptAsync(
            #endif
            EncryptRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.EncryptAsync(request)
            );
            var domain = this;
            this.Data = domain.Data = result?.Data;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> DecryptFuture(
            DecryptRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DecryptFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                this.Data = domain.Data = result?.Data;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> DecryptAsync(
            #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> DecryptAsync(
            #endif
            DecryptRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.DecryptAsync(request)
            );
            var domain = this;
            this.Data = domain.Data = result?.Data;
            return domain;
        }
        #endif

    }

    public partial class KeyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Model.Key> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.Key> self)
            {
                var (value, find) = (null as Gs2.Gs2Key.Model.Key).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.KeyName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Key.Model.Key).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.KeyName,
                    () => this.GetFuture(
                        new GetKeyRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.Key>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Key.Model.Key> ModelAsync()
            #else
        public async Task<Gs2.Gs2Key.Model.Key> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Key.Model.Key).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.KeyName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Key.Model.Key).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.KeyName,
                () => this.GetAsync(
                    new GetKeyRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Key.Model.Key> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Key.Model.Key> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Key.Model.Key> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Key.Model.Key).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.KeyName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Key.Model.Key> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Key.Model.Key).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Key.Model.Key).CacheKey(
                    this.KeyName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Key.Model.Key>(
                (null as Gs2.Gs2Key.Model.Key).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Key.Model.Key).CacheKey(
                    this.KeyName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Key.Model.Key> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Key.Model.Key> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Key.Model.Key> callback)
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

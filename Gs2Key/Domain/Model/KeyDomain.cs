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

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Key.Domain.Iterator;
using Gs2.Gs2Key.Request;
using Gs2.Gs2Key.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
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
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2KeyRestClient _client;
        private readonly string _namespaceName;
        private readonly string _keyName;

        private readonly String _parentKey;
        public string Data { get; set; }
        public string NamespaceName => _namespaceName;
        public string KeyName => _keyName;

        public KeyDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string keyName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2KeyRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._keyName = keyName;
            this._parentKey = Gs2.Gs2Key.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Key"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> UpdateAsync(
        #endif
            UpdateKeyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateKeyFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.UpdateKeyAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = Gs2.Gs2Key.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Key"
                    );
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            Gs2.Gs2Key.Domain.Model.KeyDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Key.Model.Key> GetAsync(
            #else
        private IFuture<Gs2.Gs2Key.Model.Key> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Key.Model.Key> GetAsync(
        #endif
            GetKeyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.Key> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetKeyFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.GetKeyAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = Gs2.Gs2Key.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Key"
                    );
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.Key>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> DeleteAsync(
        #endif
            DeleteKeyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteKeyFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            DeleteKeyResult result = null;
            try {
                result = await this._client.DeleteKeyAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "key")
                {
                    var parentKey = Gs2.Gs2Key.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Key"
                );
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        request.KeyName.ToString()
                    );
                    _cache.Delete<Gs2.Gs2Key.Model.Key>(parentKey, key);
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = Gs2.Gs2Key.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Key"
                    );
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Key.Model.Key>(parentKey, key);
                }
            }
            Gs2.Gs2Key.Domain.Model.KeyDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> EncryptAsync(
            #else
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> Encrypt(
            #endif
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> EncryptAsync(
        #endif
            EncryptRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.EncryptFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.EncryptAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
            Gs2.Gs2Key.Domain.Model.KeyDomain domain = this;
            this.Data = domain.Data = result?.Data;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> DecryptAsync(
            #else
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> Decrypt(
            #endif
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> DecryptAsync(
        #endif
            DecryptRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DecryptFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.DecryptAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
            Gs2.Gs2Key.Domain.Model.KeyDomain domain = this;
            this.Data = domain.Data = result?.Data;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string keyName,
            string childType
        )
        {
            return string.Join(
                ":",
                "key",
                namespaceName ?? "null",
                keyName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string keyName
        )
        {
            return string.Join(
                ":",
                keyName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Model.Key> Model() {
            #else
        public IFuture<Gs2.Gs2Key.Model.Key> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Key.Model.Key> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.Key> self)
            {
        #endif
            Gs2.Gs2Key.Model.Key value = _cache.Get<Gs2.Gs2Key.Model.Key>(
                _parentKey,
                Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                    this.KeyName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetKeyRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "key")
                            {
                                _cache.Delete<Gs2.Gs2Key.Model.Key>(
                                    _parentKey,
                                    Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                                        this.KeyName?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "key")
                    {
                        _cache.Delete<Gs2.Gs2Key.Model.Key>(
                            _parentKey,
                            Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                                this.KeyName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Key.Model.Key>(
                    _parentKey,
                    Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        this.KeyName?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.Key>(Impl);
        #endif
        }

    }
}

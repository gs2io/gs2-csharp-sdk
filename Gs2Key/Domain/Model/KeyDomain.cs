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
using UnityEngine;
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

    }

    public partial class KeyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> UpdateFuture(
            UpdateKeyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                UpdateKeyResult result = null;
                    result = await this._client.UpdateKeyAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> UpdateAsync(
            UpdateKeyRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            UpdateKeyResult result = null;
                result = await this._client.UpdateKeyAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> UpdateAsync(
            UpdateKeyRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> Update(
            UpdateKeyRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Key.Model.Key> GetFuture(
            GetKeyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.Key> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                var future = this._client.GetKeyFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                            request.KeyName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Key.Model.Key>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "key")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                GetKeyResult result = null;
                try {
                    result = await this._client.GetKeyAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        request.KeyName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Key.Model.Key>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "key")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
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
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.Key>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Key.Model.Key> GetAsync(
            GetKeyRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            var future = this._client.GetKeyFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        request.KeyName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Key.Model.Key>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "key")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            GetKeyResult result = null;
            try {
                result = await this._client.GetKeyAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                    request.KeyName.ToString()
                    );
                _cache.Put<Gs2.Gs2Key.Model.Key>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "key")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
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
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> DeleteFuture(
            DeleteKeyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                var future = this._client.DeleteKeyFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                            request.KeyName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Key.Model.Key>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "key")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                DeleteKeyResult result = null;
                try {
                    result = await this._client.DeleteKeyAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        request.KeyName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Key.Model.Key>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "key")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> DeleteAsync(
            DeleteKeyRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            var future = this._client.DeleteKeyFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        request.KeyName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Key.Model.Key>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "key")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            DeleteKeyResult result = null;
            try {
                result = await this._client.DeleteKeyAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                    request.KeyName.ToString()
                    );
                _cache.Put<Gs2.Gs2Key.Model.Key>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "key")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> DeleteAsync(
            DeleteKeyRequest request
        ) {
            var future = DeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> Delete(
            DeleteKeyRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> EncryptFuture(
            EncryptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                EncryptResult result = null;
                    result = await this._client.EncryptAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Data = domain.Data = result?.Data;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> EncryptAsync(
            EncryptRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            EncryptResult result = null;
                result = await this._client.EncryptAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Data = domain.Data = result?.Data;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> EncryptAsync(
            EncryptRequest request
        ) {
            var future = EncryptFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to EncryptFuture.")]
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> Encrypt(
            EncryptRequest request
        ) {
            return EncryptFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> DecryptFuture(
            DecryptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithKeyName(this.KeyName);
                DecryptResult result = null;
                    result = await this._client.DecryptAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Data = domain.Data = result?.Data;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.KeyDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.KeyDomain> DecryptAsync(
            DecryptRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithKeyName(this.KeyName);
            DecryptResult result = null;
                result = await this._client.DecryptAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Data = domain.Data = result?.Data;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.KeyDomain> DecryptAsync(
            DecryptRequest request
        ) {
            var future = DecryptFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DecryptFuture.")]
        public IFuture<Gs2.Gs2Key.Domain.Model.KeyDomain> Decrypt(
            DecryptRequest request
        ) {
            return DecryptFuture(request);
        }
        #endif

    }

    public partial class KeyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Model.Key> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.Key> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Key.Model.Key>(
                    _parentKey,
                    Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        this.KeyName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetKeyRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                                    this.KeyName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Key.Model.Key>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "key")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _cache.Get<Gs2.Gs2Key.Model.Key>(
                        _parentKey,
                        Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                            this.KeyName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.Key>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Key.Model.Key> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Key.Model.Key>(
                    _parentKey,
                    Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                        this.KeyName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetKeyRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                                    this.KeyName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Key.Model.Key>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "key")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Key.Model.Key>(
                        _parentKey,
                        Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                            this.KeyName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Model.Key> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

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


        public ulong Subscribe(Action<Gs2.Gs2Key.Model.Key> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                    this.KeyName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Key.Model.Key>(
                _parentKey,
                Gs2.Gs2Key.Domain.Model.KeyDomain.CreateCacheKey(
                    this.KeyName.ToString()
                ),
                callbackId
            );
        }

    }
}

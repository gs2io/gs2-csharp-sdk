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

    public partial class GitHubApiKeyDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2KeyRestClient _client;
        private readonly string _namespaceName;
        private readonly string _apiKeyName;

        private readonly String _parentKey;
        public string ApiKey { get; set; }
        public string NamespaceName => _namespaceName;
        public string ApiKeyName => _apiKeyName;

        public GitHubApiKeyDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string apiKeyName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2KeyRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._apiKeyName = apiKeyName;
            this._parentKey = Gs2.Gs2Key.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "GitHubApiKey"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string apiKeyName,
            string childType
        )
        {
            return string.Join(
                ":",
                "key",
                namespaceName ?? "null",
                apiKeyName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string apiKeyName
        )
        {
            return string.Join(
                ":",
                apiKeyName ?? "null"
            );
        }

    }

    public partial class GitHubApiKeyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> UpdateFuture(
            UpdateGitHubApiKeyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithApiKeyName(this.ApiKeyName);
                var future = this._client.UpdateGitHubApiKeyFuture(
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
                    .WithApiKeyName(this.ApiKeyName);
                UpdateGitHubApiKeyResult result = null;
                    result = await this._client.UpdateGitHubApiKeyAsync(
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
                            "GitHubApiKey"
                        );
                        var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> UpdateAsync(
            UpdateGitHubApiKeyRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithApiKeyName(this.ApiKeyName);
            var future = this._client.UpdateGitHubApiKeyFuture(
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
                .WithApiKeyName(this.ApiKeyName);
            UpdateGitHubApiKeyResult result = null;
                result = await this._client.UpdateGitHubApiKeyAsync(
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
                        "GitHubApiKey"
                    );
                    var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> UpdateAsync(
            UpdateGitHubApiKeyRequest request
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
        public IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> Update(
            UpdateGitHubApiKeyRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Key.Model.GitHubApiKey> GetFuture(
            GetGitHubApiKeyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.GitHubApiKey> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithApiKeyName(this.ApiKeyName);
                var future = this._client.GetGitHubApiKeyFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                            request.ApiKeyName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "gitHubApiKey")
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
                    .WithApiKeyName(this.ApiKeyName);
                GetGitHubApiKeyResult result = null;
                try {
                    result = await this._client.GetGitHubApiKeyAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        request.ApiKeyName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "gitHubApiKey")
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
                            "GitHubApiKey"
                        );
                        var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.GitHubApiKey>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Key.Model.GitHubApiKey> GetAsync(
            GetGitHubApiKeyRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithApiKeyName(this.ApiKeyName);
            var future = this._client.GetGitHubApiKeyFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        request.ApiKeyName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "gitHubApiKey")
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
                .WithApiKeyName(this.ApiKeyName);
            GetGitHubApiKeyResult result = null;
            try {
                result = await this._client.GetGitHubApiKeyAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                    request.ApiKeyName.ToString()
                    );
                _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "gitHubApiKey")
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
                        "GitHubApiKey"
                    );
                    var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> DeleteFuture(
            DeleteGitHubApiKeyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithApiKeyName(this.ApiKeyName);
                var future = this._client.DeleteGitHubApiKeyFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                            request.ApiKeyName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "gitHubApiKey")
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
                    .WithApiKeyName(this.ApiKeyName);
                DeleteGitHubApiKeyResult result = null;
                try {
                    result = await this._client.DeleteGitHubApiKeyAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        request.ApiKeyName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "gitHubApiKey")
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
                            "GitHubApiKey"
                        );
                        var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Key.Model.GitHubApiKey>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> DeleteAsync(
            DeleteGitHubApiKeyRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithApiKeyName(this.ApiKeyName);
            var future = this._client.DeleteGitHubApiKeyFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        request.ApiKeyName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "gitHubApiKey")
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
                .WithApiKeyName(this.ApiKeyName);
            DeleteGitHubApiKeyResult result = null;
            try {
                result = await this._client.DeleteGitHubApiKeyAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                    request.ApiKeyName.ToString()
                    );
                _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "gitHubApiKey")
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
                        "GitHubApiKey"
                    );
                    var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Key.Model.GitHubApiKey>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> DeleteAsync(
            DeleteGitHubApiKeyRequest request
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
        public IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> Delete(
            DeleteGitHubApiKeyRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class GitHubApiKeyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Key.Model.GitHubApiKey> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.GitHubApiKey> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Key.Model.GitHubApiKey>(
                    _parentKey,
                    Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        this.ApiKeyName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetGitHubApiKeyRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                                    this.ApiKeyName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "gitHubApiKey")
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
                    (value, _) = _cache.Get<Gs2.Gs2Key.Model.GitHubApiKey>(
                        _parentKey,
                        Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                            this.ApiKeyName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.GitHubApiKey>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Key.Model.GitHubApiKey> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Key.Model.GitHubApiKey>(
                    _parentKey,
                    Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        this.ApiKeyName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetGitHubApiKeyRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                                    this.ApiKeyName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Key.Model.GitHubApiKey>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "gitHubApiKey")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Key.Model.GitHubApiKey>(
                        _parentKey,
                        Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                            this.ApiKeyName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Model.GitHubApiKey> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Key.Model.GitHubApiKey> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Key.Model.GitHubApiKey> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Key.Model.GitHubApiKey> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Key.Model.GitHubApiKey> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                    this.ApiKeyName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Key.Model.GitHubApiKey>(
                _parentKey,
                Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                    this.ApiKeyName.ToString()
                ),
                callbackId
            );
        }

    }
}

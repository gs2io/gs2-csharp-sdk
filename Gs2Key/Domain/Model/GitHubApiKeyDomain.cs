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
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "GitHubApiKey"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> UpdateAsync(
        #endif
            UpdateGitHubApiKeyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithApiKeyName(this._apiKeyName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.UpdateGitHubApiKeyAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        request.ApiKeyName != null ? request.ApiKeyName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Key.Model.GitHubApiKey> GetAsync(
            #else
        private IFuture<Gs2.Gs2Key.Model.GitHubApiKey> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Key.Model.GitHubApiKey> GetAsync(
        #endif
            GetGitHubApiKeyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.GitHubApiKey> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithApiKeyName(this._apiKeyName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetGitHubApiKeyFuture(
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
            var result = await this._client.GetGitHubApiKeyAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                        request.ApiKeyName != null ? request.ApiKeyName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.GitHubApiKey>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> DeleteAsync(
        #endif
            DeleteGitHubApiKeyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithApiKeyName(this._apiKeyName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteGitHubApiKeyFuture(
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
            DeleteGitHubApiKeyResult result = null;
            try {
                result = await this._client.DeleteGitHubApiKeyAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            _cache.Delete<Gs2.Gs2Key.Model.GitHubApiKey>(
                _parentKey,
                Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                    request.ApiKeyName != null ? request.ApiKeyName.ToString() : null
                )
            );
            Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain>(Impl);
        #endif
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Key.Model.GitHubApiKey> Model() {
            #else
        public IFuture<Gs2.Gs2Key.Model.GitHubApiKey> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Key.Model.GitHubApiKey> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Key.Model.GitHubApiKey> self)
            {
        #endif
            Gs2.Gs2Key.Model.GitHubApiKey value = _cache.Get<Gs2.Gs2Key.Model.GitHubApiKey>(
                _parentKey,
                Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                    this.ApiKeyName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetGitHubApiKeyRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Key.Model.GitHubApiKey>(
                            _parentKey,
                            Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                                this.ApiKeyName?.ToString()
                            )
                        );
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException) {
                    _cache.Delete<Gs2.Gs2Key.Model.GitHubApiKey>(
                        _parentKey,
                        Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                            this.ApiKeyName?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Key.Model.GitHubApiKey>(
                _parentKey,
                Gs2.Gs2Key.Domain.Model.GitHubApiKeyDomain.CreateCacheKey(
                    this.ApiKeyName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Key.Model.GitHubApiKey>(Impl);
        #endif
        }

    }
}

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
using Gs2.Gs2News.Domain.Iterator;
using Gs2.Gs2News.Request;
using Gs2.Gs2News.Result;
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

namespace Gs2.Gs2News.Domain.Model
{

    public partial class CurrentNewsMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2NewsRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string UploadToken { get; set; }
        public string TemplateUploadUrl { get; set; }
        public string NamespaceName => _namespaceName;

        public CurrentNewsMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2NewsRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._parentKey = Gs2.Gs2News.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "CurrentNewsMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "news",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

    }

    public partial class CurrentNewsMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> PrepareUpdateFuture(
            PrepareUpdateCurrentNewsMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.PrepareUpdateCurrentNewsMasterFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                PrepareUpdateCurrentNewsMasterResult result = null;
                    result = await this._client.PrepareUpdateCurrentNewsMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.UploadToken = domain.UploadToken = result?.UploadToken;
                this.TemplateUploadUrl = domain.TemplateUploadUrl = result?.TemplateUploadUrl;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> PrepareUpdateAsync(
            PrepareUpdateCurrentNewsMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.PrepareUpdateCurrentNewsMasterFuture(
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
                .WithNamespaceName(this.NamespaceName);
            PrepareUpdateCurrentNewsMasterResult result = null;
                result = await this._client.PrepareUpdateCurrentNewsMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.TemplateUploadUrl = domain.TemplateUploadUrl = result?.TemplateUploadUrl;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> PrepareUpdateAsync(
            PrepareUpdateCurrentNewsMasterRequest request
        ) {
            var future = PrepareUpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PrepareUpdateFuture.")]
        public IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> PrepareUpdate(
            PrepareUpdateCurrentNewsMasterRequest request
        ) {
            return PrepareUpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> UpdateFuture(
            UpdateCurrentNewsMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateCurrentNewsMasterFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                UpdateCurrentNewsMasterResult result = null;
                    result = await this._client.UpdateCurrentNewsMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> UpdateAsync(
            UpdateCurrentNewsMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.UpdateCurrentNewsMasterFuture(
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
                .WithNamespaceName(this.NamespaceName);
            UpdateCurrentNewsMasterResult result = null;
                result = await this._client.UpdateCurrentNewsMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> UpdateAsync(
            UpdateCurrentNewsMasterRequest request
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
        public IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> Update(
            UpdateCurrentNewsMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> PrepareUpdateFromGitHubFuture(
            PrepareUpdateCurrentNewsMasterFromGitHubRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.PrepareUpdateCurrentNewsMasterFromGitHubFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                PrepareUpdateCurrentNewsMasterFromGitHubResult result = null;
                    result = await this._client.PrepareUpdateCurrentNewsMasterFromGitHubAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.UploadToken = domain.UploadToken = result?.UploadToken;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> PrepareUpdateFromGitHubAsync(
            PrepareUpdateCurrentNewsMasterFromGitHubRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.PrepareUpdateCurrentNewsMasterFromGitHubFuture(
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
                .WithNamespaceName(this.NamespaceName);
            PrepareUpdateCurrentNewsMasterFromGitHubResult result = null;
                result = await this._client.PrepareUpdateCurrentNewsMasterFromGitHubAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> PrepareUpdateFromGitHubAsync(
            PrepareUpdateCurrentNewsMasterFromGitHubRequest request
        ) {
            var future = PrepareUpdateFromGitHubFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PrepareUpdateFromGitHubFuture.")]
        public IFuture<Gs2.Gs2News.Domain.Model.CurrentNewsMasterDomain> PrepareUpdateFromGitHub(
            PrepareUpdateCurrentNewsMasterFromGitHubRequest request
        ) {
            return PrepareUpdateFromGitHubFuture(request);
        }
        #endif

    }

    public partial class CurrentNewsMasterDomain {

    }
}

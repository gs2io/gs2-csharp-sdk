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
using Gs2.Gs2Quest.Domain.Iterator;
using Gs2.Gs2Quest.Request;
using Gs2.Gs2Quest.Result;
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

namespace Gs2.Gs2Quest.Domain.Model
{

    public partial class CurrentQuestMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2QuestRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;

        public CurrentQuestMasterDomain(
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
            this._client = new Gs2QuestRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "CurrentQuestMaster"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> ExportMasterAsync(
            #else
        public IFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> ExportMaster(
            #endif
        #else
        public async Task<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> ExportMasterAsync(
        #endif
            ExportMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ExportMasterFuture(
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
            var result = await this._client.ExportMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Quest.Model.CurrentQuestMaster> GetAsync(
            #else
        private IFuture<Gs2.Gs2Quest.Model.CurrentQuestMaster> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Quest.Model.CurrentQuestMaster> GetAsync(
        #endif
            GetCurrentQuestMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.CurrentQuestMaster> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetCurrentQuestMasterFuture(
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
            var result = await this._client.GetCurrentQuestMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.CurrentQuestMaster>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> UpdateAsync(
        #endif
            UpdateCurrentQuestMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateCurrentQuestMasterFuture(
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
            var result = await this._client.UpdateCurrentQuestMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> UpdateFromGitHubAsync(
            #else
        public IFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> UpdateFromGitHub(
            #endif
        #else
        public async Task<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> UpdateFromGitHubAsync(
        #endif
            UpdateCurrentQuestMasterFromGitHubRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateCurrentQuestMasterFromGitHubFuture(
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
            var result = await this._client.UpdateCurrentQuestMasterFromGitHubAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "quest",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string namespaceName
        )
        {
            return string.Join(
                ":",
                namespaceName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Model.CurrentQuestMaster> Model() {
            #else
        public IFuture<Gs2.Gs2Quest.Model.CurrentQuestMaster> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Quest.Model.CurrentQuestMaster> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.CurrentQuestMaster> self)
            {
        #endif
            Gs2.Gs2Quest.Model.CurrentQuestMaster value = _cache.Get<Gs2.Gs2Quest.Model.CurrentQuestMaster>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetCurrentQuestMasterRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "currentQuestMaster")
                            {
                                _cache.Delete<Gs2.Gs2Quest.Model.CurrentQuestMaster>(
                                    _parentKey,
                                    Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain.CreateCacheKey(
                                        this.NamespaceName?.ToString()
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
                    if (e.errors[0].component == "currentQuestMaster")
                    {
                    _cache.Delete<Gs2.Gs2Quest.Model.CurrentQuestMaster>(
                            _parentKey,
                            Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain.CreateCacheKey(
                                this.NamespaceName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Quest.Model.CurrentQuestMaster>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.CurrentQuestMasterDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.CurrentQuestMaster>(Impl);
        #endif
        }

    }
}

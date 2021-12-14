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
using Gs2.Gs2Experience.Domain.Iterator;
using Gs2.Gs2Experience.Request;
using Gs2.Gs2Experience.Result;
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

namespace Gs2.Gs2Experience.Domain.Model
{

    public partial class ExperienceModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ExperienceRestClient _client;
        private readonly string _namespaceName;
        private readonly string _experienceName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string ExperienceName => _experienceName;

        public ExperienceModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string experienceName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ExperienceRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._experienceName = experienceName;
            this._parentKey = Gs2.Gs2Experience.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "ExperienceModelMaster"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Experience.Model.ExperienceModelMaster> GetAsync(
            #else
        private IFuture<Gs2.Gs2Experience.Model.ExperienceModelMaster> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Experience.Model.ExperienceModelMaster> GetAsync(
        #endif
            GetExperienceModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
          IEnumerator Impl(IFuture<Gs2.Gs2Experience.Model.ExperienceModelMaster> self)
          {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithExperienceName(this._experienceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetExperienceModelMasterFuture(
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
            var result = await this._client.GetExperienceModelMasterAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain.CreateCacheKey(
                        request.ExperienceName != null ? request.ExperienceName.ToString() : null
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
            return new Gs2InlineFuture<Gs2.Gs2Experience.Model.ExperienceModelMaster>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain> UpdateAsync(
        #endif
            UpdateExperienceModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
          IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain> self)
          {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithExperienceName(this._experienceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateExperienceModelMasterFuture(
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
            var result = await this._client.UpdateExperienceModelMasterAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain.CreateCacheKey(
                        request.ExperienceName != null ? request.ExperienceName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain> DeleteAsync(
        #endif
            DeleteExperienceModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
          IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain> self)
          {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithExperienceName(this._experienceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteExperienceModelMasterFuture(
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
            DeleteExperienceModelMasterResult result = null;
            try {
                result = await this._client.DeleteExperienceModelMasterAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            _cache.Delete<Gs2.Gs2Experience.Model.ExperienceModelMaster>(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain.CreateCacheKey(
                    request.ExperienceName != null ? request.ExperienceName.ToString() : null
                )
            );
            Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string experienceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "experience",
                namespaceName ?? "null",
                experienceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string experienceName
        )
        {
            return string.Join(
                ":",
                experienceName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Model.ExperienceModelMaster> Model() {
            #else
        public IFuture<Gs2.Gs2Experience.Model.ExperienceModelMaster> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Model.ExperienceModelMaster> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Model.ExperienceModelMaster> self)
            {
        #endif
            Gs2.Gs2Experience.Model.ExperienceModelMaster value = _cache.Get<Gs2.Gs2Experience.Model.ExperienceModelMaster>(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain.CreateCacheKey(
                    this.ExperienceName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetExperienceModelMasterRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Experience.Model.ExperienceModelMaster>(
                            _parentKey,
                            Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain.CreateCacheKey(
                                this.ExperienceName?.ToString()
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
                    _cache.Delete<Gs2.Gs2Experience.Model.ExperienceModelMaster>(
                        _parentKey,
                        Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain.CreateCacheKey(
                            this.ExperienceName?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Experience.Model.ExperienceModelMaster>(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.ExperienceModelMasterDomain.CreateCacheKey(
                    this.ExperienceName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Experience.Model.ExperienceModelMaster>(Impl);
        #endif
        }

    }
}

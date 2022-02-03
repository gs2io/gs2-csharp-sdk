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

    public partial class StatusDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ExperienceRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _experienceName;
        private readonly string _propertyId;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string ExperienceName => _experienceName;
        public string PropertyId => _propertyId;

        public StatusDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string experienceName,
            string propertyId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ExperienceRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._experienceName = experienceName;
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._userId != null ? this._userId.ToString() : null,
                "Status"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Experience.Model.Status> GetAsync(
            #else
        private IFuture<Gs2.Gs2Experience.Model.Status> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Experience.Model.Status> GetAsync(
        #endif
            GetStatusByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Model.Status> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithExperienceName(this._experienceName)
                .WithPropertyId(this._propertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetStatusByUserIdFuture(
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
            var result = await this._client.GetStatusByUserIdAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        request.ExperienceName != null ? request.ExperienceName.ToString() : null,
                        request.PropertyId != null ? request.PropertyId.ToString() : null
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
            return new Gs2InlineFuture<Gs2.Gs2Experience.Model.Status>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> GetWithSignatureAsync(
            #else
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> GetWithSignature(
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> GetWithSignatureAsync(
        #endif
            GetStatusWithSignatureByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithExperienceName(this._experienceName)
                .WithPropertyId(this._propertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetStatusWithSignatureByUserIdFuture(
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
            var result = await this._client.GetStatusWithSignatureByUserIdAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        request.ExperienceName != null ? request.ExperienceName.ToString() : null,
                        request.PropertyId != null ? request.PropertyId.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Experience.Domain.Model.StatusDomain domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddExperienceAsync(
            #else
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddExperience(
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddExperienceAsync(
        #endif
            AddExperienceByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithExperienceName(this._experienceName)
                .WithPropertyId(this._propertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.AddExperienceByUserIdFuture(
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
            var result = await this._client.AddExperienceByUserIdAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        request.ExperienceName != null ? request.ExperienceName.ToString() : null,
                        request.PropertyId != null ? request.PropertyId.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Experience.Domain.Model.StatusDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetExperienceAsync(
            #else
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetExperience(
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetExperienceAsync(
        #endif
            SetExperienceByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithExperienceName(this._experienceName)
                .WithPropertyId(this._propertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SetExperienceByUserIdFuture(
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
            var result = await this._client.SetExperienceByUserIdAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        request.ExperienceName != null ? request.ExperienceName.ToString() : null,
                        request.PropertyId != null ? request.PropertyId.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Experience.Domain.Model.StatusDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddRankCapAsync(
            #else
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddRankCap(
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> AddRankCapAsync(
        #endif
            AddRankCapByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithExperienceName(this._experienceName)
                .WithPropertyId(this._propertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.AddRankCapByUserIdFuture(
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
            var result = await this._client.AddRankCapByUserIdAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        request.ExperienceName != null ? request.ExperienceName.ToString() : null,
                        request.PropertyId != null ? request.PropertyId.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Experience.Domain.Model.StatusDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetRankCapAsync(
            #else
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetRankCap(
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> SetRankCapAsync(
        #endif
            SetRankCapByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithExperienceName(this._experienceName)
                .WithPropertyId(this._propertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SetRankCapByUserIdFuture(
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
            var result = await this._client.SetRankCapByUserIdAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        request.ExperienceName != null ? request.ExperienceName.ToString() : null,
                        request.PropertyId != null ? request.PropertyId.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Experience.Domain.Model.StatusDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusDomain> DeleteAsync(
        #endif
            DeleteStatusByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithExperienceName(this._experienceName)
                .WithPropertyId(this._propertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteStatusByUserIdFuture(
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
            DeleteStatusByUserIdResult result = null;
            try {
                result = await this._client.DeleteStatusByUserIdAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            _cache.Delete<Gs2.Gs2Experience.Model.Status>(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    request.ExperienceName != null ? request.ExperienceName.ToString() : null,
                    request.PropertyId != null ? request.PropertyId.ToString() : null
                )
            );
            Gs2.Gs2Experience.Domain.Model.StatusDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string experienceName,
            string propertyId,
            string childType
        )
        {
            return string.Join(
                ":",
                "experience",
                namespaceName ?? "null",
                userId ?? "null",
                experienceName ?? "null",
                propertyId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string experienceName,
            string propertyId
        )
        {
            return string.Join(
                ":",
                experienceName ?? "null",
                propertyId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Experience.Model.Status> Model() {
            #else
        public IFuture<Gs2.Gs2Experience.Model.Status> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Experience.Model.Status> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Model.Status> self)
            {
        #endif
            Gs2.Gs2Experience.Model.Status value = _cache.Get<Gs2.Gs2Experience.Model.Status>(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    this.ExperienceName?.ToString(),
                    this.PropertyId?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetStatusByUserIdRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Experience.Model.Status>(
                            _parentKey,
                            Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                                this.ExperienceName?.ToString(),
                                this.PropertyId?.ToString()
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
                    _cache.Delete<Gs2.Gs2Experience.Model.Status>(
                        _parentKey,
                        Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                            this.ExperienceName?.ToString(),
                            this.PropertyId?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Experience.Model.Status>(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    this.ExperienceName?.ToString(),
                    this.PropertyId?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Experience.Model.Status>(Impl);
        #endif
        }

    }
}
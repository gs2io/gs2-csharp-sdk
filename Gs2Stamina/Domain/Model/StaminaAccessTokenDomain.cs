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
using Gs2.Gs2Stamina.Domain.Iterator;
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Result;
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

namespace Gs2.Gs2Stamina.Domain.Model
{

    public partial class StaminaAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2StaminaRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;
        private readonly string _staminaName;

        private readonly String _parentKey;
        public long? OverflowValue { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public string StaminaName => _staminaName;

        public StaminaAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string staminaName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2StaminaRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._staminaName = staminaName;
            this._parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                "Stamina"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Stamina.Model.Stamina> GetAsync(
            #else
        private IFuture<Gs2.Gs2Stamina.Model.Stamina> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Stamina.Model.Stamina> GetAsync(
        #endif
            GetStaminaRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.Stamina> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithStaminaName(this._staminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetStaminaFuture(
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
            var result = await this._client.GetStaminaAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.StaminaModel != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.StaminaModel,
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
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.Stamina>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> ConsumeAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> Consume(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> ConsumeAsync(
        #endif
            ConsumeStaminaRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithStaminaName(this._staminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ConsumeStaminaFuture(
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
            var result = await this._client.ConsumeStaminaAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.StaminaModel != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.StaminaModel,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetMaxValueByStatusAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetMaxValueByStatus(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetMaxValueByStatusAsync(
        #endif
            SetMaxValueByStatusRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithStaminaName(this._staminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SetMaxValueByStatusFuture(
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
            var result = await this._client.SetMaxValueByStatusAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.StaminaModel != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.StaminaModel,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetRecoverIntervalByStatusAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetRecoverIntervalByStatus(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetRecoverIntervalByStatusAsync(
        #endif
            SetRecoverIntervalByStatusRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithStaminaName(this._staminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SetRecoverIntervalByStatusFuture(
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
            var result = await this._client.SetRecoverIntervalByStatusAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.StaminaModel != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.StaminaModel,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetRecoverValueByStatusAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetRecoverValueByStatus(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> SetRecoverValueByStatusAsync(
        #endif
            SetRecoverValueByStatusRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithStaminaName(this._staminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SetRecoverValueByStatusFuture(
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
            var result = await this._client.SetRecoverValueByStatusAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.StaminaModel != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        request.StaminaName != null ? request.StaminaName.ToString() : null
                    ),
                    result.StaminaModel,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaAccessTokenDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string staminaName,
            string childType
        )
        {
            return string.Join(
                ":",
                "stamina",
                namespaceName ?? "null",
                userId ?? "null",
                staminaName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string staminaName
        )
        {
            return string.Join(
                ":",
                staminaName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Model.Stamina> Model() {
            #else
        public IFuture<Gs2.Gs2Stamina.Model.Stamina> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Model.Stamina> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.Stamina> self)
            {
        #endif
            Gs2.Gs2Stamina.Model.Stamina value = _cache.Get<Gs2.Gs2Stamina.Model.Stamina>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                    this.StaminaName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetStaminaRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Stamina.Model.Stamina>(
                            _parentKey,
                            Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                this.StaminaName?.ToString()
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
                    _cache.Delete<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            this.StaminaName?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Stamina.Model.Stamina>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                    this.StaminaName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.Stamina>(Impl);
        #endif
        }

    }
}
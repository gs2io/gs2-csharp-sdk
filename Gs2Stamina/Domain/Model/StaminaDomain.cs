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

namespace Gs2.Gs2Stamina.Domain.Model
{

    public partial class StaminaDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2StaminaRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _staminaName;

        private readonly String _parentKey;
        public int? OverflowValue { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string StaminaName => _staminaName;

        public StaminaDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
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
            this._userId = userId;
            this._staminaName = staminaName;
            this._parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Stamina"
            );
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

    }

    public partial class StaminaDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Stamina.Model.Stamina> GetFuture(
            GetStaminaByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.Stamina> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.GetStaminaByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            request.StaminaName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "stamina")
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                GetStaminaByUserIdResult result = null;
                try {
                    result = await this._client.GetStaminaByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "stamina")
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
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.Stamina>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Stamina.Model.Stamina> GetAsync(
            GetStaminaByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.GetStaminaByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "stamina")
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            GetStaminaByUserIdResult result = null;
            try {
                result = await this._client.GetStaminaByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                    request.StaminaName.ToString()
                    );
                _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "stamina")
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
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> UpdateFuture(
            UpdateStaminaByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.UpdateStaminaByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                UpdateStaminaByUserIdResult result = null;
                    result = await this._client.UpdateStaminaByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> UpdateAsync(
            UpdateStaminaByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.UpdateStaminaByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            UpdateStaminaByUserIdResult result = null;
                result = await this._client.UpdateStaminaByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> UpdateAsync(
            UpdateStaminaByUserIdRequest request
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> Update(
            UpdateStaminaByUserIdRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ConsumeFuture(
            ConsumeStaminaByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.ConsumeStaminaByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                ConsumeStaminaByUserIdResult result = null;
                    result = await this._client.ConsumeStaminaByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ConsumeAsync(
            ConsumeStaminaByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.ConsumeStaminaByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            ConsumeStaminaByUserIdResult result = null;
                result = await this._client.ConsumeStaminaByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ConsumeAsync(
            ConsumeStaminaByUserIdRequest request
        ) {
            var future = ConsumeFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ConsumeFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> Consume(
            ConsumeStaminaByUserIdRequest request
        ) {
            return ConsumeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RecoverFuture(
            RecoverStaminaByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.RecoverStaminaByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                RecoverStaminaByUserIdResult result = null;
                    result = await this._client.RecoverStaminaByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;
                domain.OverflowValue = result?.OverflowValue;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RecoverAsync(
            RecoverStaminaByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.RecoverStaminaByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            RecoverStaminaByUserIdResult result = null;
                result = await this._client.RecoverStaminaByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;
            domain.OverflowValue = result?.OverflowValue;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RecoverAsync(
            RecoverStaminaByUserIdRequest request
        ) {
            var future = RecoverFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to RecoverFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> Recover(
            RecoverStaminaByUserIdRequest request
        ) {
            return RecoverFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValueFuture(
            RaiseMaxValueByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.RaiseMaxValueByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                RaiseMaxValueByUserIdResult result = null;
                    result = await this._client.RaiseMaxValueByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValueAsync(
            RaiseMaxValueByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.RaiseMaxValueByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            RaiseMaxValueByUserIdResult result = null;
                result = await this._client.RaiseMaxValueByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValueAsync(
            RaiseMaxValueByUserIdRequest request
        ) {
            var future = RaiseMaxValueFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to RaiseMaxValueFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValue(
            RaiseMaxValueByUserIdRequest request
        ) {
            return RaiseMaxValueFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DecreaseMaxValueFuture(
            DecreaseMaxValueByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.DecreaseMaxValueByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                DecreaseMaxValueByUserIdResult result = null;
                    result = await this._client.DecreaseMaxValueByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DecreaseMaxValueAsync(
            DecreaseMaxValueByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.DecreaseMaxValueByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            DecreaseMaxValueByUserIdResult result = null;
                result = await this._client.DecreaseMaxValueByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DecreaseMaxValueAsync(
            DecreaseMaxValueByUserIdRequest request
        ) {
            var future = DecreaseMaxValueFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DecreaseMaxValueFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DecreaseMaxValue(
            DecreaseMaxValueByUserIdRequest request
        ) {
            return DecreaseMaxValueFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValueFuture(
            SetMaxValueByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.SetMaxValueByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                SetMaxValueByUserIdResult result = null;
                    result = await this._client.SetMaxValueByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValueAsync(
            SetMaxValueByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.SetMaxValueByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            SetMaxValueByUserIdResult result = null;
                result = await this._client.SetMaxValueByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValueAsync(
            SetMaxValueByUserIdRequest request
        ) {
            var future = SetMaxValueFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to SetMaxValueFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValue(
            SetMaxValueByUserIdRequest request
        ) {
            return SetMaxValueFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverIntervalFuture(
            SetRecoverIntervalByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.SetRecoverIntervalByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                SetRecoverIntervalByUserIdResult result = null;
                    result = await this._client.SetRecoverIntervalByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverIntervalAsync(
            SetRecoverIntervalByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.SetRecoverIntervalByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            SetRecoverIntervalByUserIdResult result = null;
                result = await this._client.SetRecoverIntervalByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverIntervalAsync(
            SetRecoverIntervalByUserIdRequest request
        ) {
            var future = SetRecoverIntervalFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to SetRecoverIntervalFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverInterval(
            SetRecoverIntervalByUserIdRequest request
        ) {
            return SetRecoverIntervalFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValueFuture(
            SetRecoverValueByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.SetRecoverValueByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                SetRecoverValueByUserIdResult result = null;
                    result = await this._client.SetRecoverValueByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValueAsync(
            SetRecoverValueByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.SetRecoverValueByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            SetRecoverValueByUserIdResult result = null;
                result = await this._client.SetRecoverValueByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.StaminaModel != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "StaminaModel"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                        resultModel.StaminaModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.StaminaModel,
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValueAsync(
            SetRecoverValueByUserIdRequest request
        ) {
            var future = SetRecoverValueFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to SetRecoverValueFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValue(
            SetRecoverValueByUserIdRequest request
        ) {
            return SetRecoverValueFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DeleteFuture(
            DeleteStaminaByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                var future = this._client.DeleteStaminaByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            request.StaminaName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "stamina")
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
                    .WithUserId(this.UserId)
                    .WithStaminaName(this.StaminaName);
                DeleteStaminaByUserIdResult result = null;
                try {
                    result = await this._client.DeleteStaminaByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "stamina")
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
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Delete<Gs2.Gs2Stamina.Model.Stamina>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DeleteAsync(
            DeleteStaminaByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            var future = this._client.DeleteStaminaByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "stamina")
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
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            DeleteStaminaByUserIdResult result = null;
            try {
                result = await this._client.DeleteStaminaByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                    request.StaminaName.ToString()
                    );
                _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "stamina")
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
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Stamina"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        resultModel.Item.StaminaName.ToString()
                    );
                    cache.Delete<Gs2.Gs2Stamina.Model.Stamina>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DeleteAsync(
            DeleteStaminaByUserIdRequest request
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> Delete(
            DeleteStaminaByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class StaminaDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Model.Stamina> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.Stamina> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Stamina.Model.Stamina>(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        this.StaminaName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetStaminaByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                    this.StaminaName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "stamina")
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
                    (value, _) = _cache.Get<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            this.StaminaName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.Stamina>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Model.Stamina> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Stamina.Model.Stamina>(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        this.StaminaName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetStaminaByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                    this.StaminaName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "stamina")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            this.StaminaName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Model.Stamina> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Stamina.Model.Stamina> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Stamina.Model.Stamina> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Stamina.Model.Stamina> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

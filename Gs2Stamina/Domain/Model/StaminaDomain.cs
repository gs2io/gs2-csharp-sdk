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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Stamina.Model.Stamina> GetAsync(
            #else
        private IFuture<Gs2.Gs2Stamina.Model.Stamina> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Stamina.Model.Stamina> GetAsync(
        #endif
            GetStaminaByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.Stamina> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetStaminaByUserIdFuture(
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
            var result = await this._client.GetStaminaByUserIdAsync(
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> UpdateAsync(
        #endif
            UpdateStaminaByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.UpdateStaminaByUserIdAsync(
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
            Gs2.Gs2Stamina.Domain.Model.StaminaDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ConsumeAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> Consume(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> ConsumeAsync(
        #endif
            ConsumeStaminaByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.ConsumeStaminaByUserIdAsync(
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
            Gs2.Gs2Stamina.Domain.Model.StaminaDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RecoverAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> Recover(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RecoverAsync(
        #endif
            RecoverStaminaByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.RecoverStaminaByUserIdAsync(
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
            Gs2.Gs2Stamina.Domain.Model.StaminaDomain domain = this;
            domain.OverflowValue = result?.OverflowValue;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValueAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValue(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> RaiseMaxValueAsync(
        #endif
            RaiseMaxValueByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.RaiseMaxValueByUserIdAsync(
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
            Gs2.Gs2Stamina.Domain.Model.StaminaDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValueAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValue(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetMaxValueAsync(
        #endif
            SetMaxValueByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.SetMaxValueByUserIdAsync(
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
            Gs2.Gs2Stamina.Domain.Model.StaminaDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverIntervalAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverInterval(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverIntervalAsync(
        #endif
            SetRecoverIntervalByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.SetRecoverIntervalByUserIdAsync(
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
            Gs2.Gs2Stamina.Domain.Model.StaminaDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValueAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValue(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> SetRecoverValueAsync(
        #endif
            SetRecoverValueByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.SetRecoverValueByUserIdAsync(
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
            Gs2.Gs2Stamina.Domain.Model.StaminaDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> DeleteAsync(
        #endif
            DeleteStaminaByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithStaminaName(this.StaminaName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteStaminaByUserIdFuture(
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
            DeleteStaminaByUserIdResult result = null;
            try {
                result = await this._client.DeleteStaminaByUserIdAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "stamina")
                {
                    var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                        request.StaminaName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
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
            Gs2.Gs2Stamina.Domain.Model.StaminaDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaDomain>(Impl);
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
            var (value, find) = _cache.Get<Gs2.Gs2Stamina.Model.Stamina>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                    this.StaminaName?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetStaminaByUserIdRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "stamina")
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
                    if (e.errors[0].component == "stamina")
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
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Stamina.Model.Stamina>(
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

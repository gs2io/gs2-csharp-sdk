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
#pragma warning disable CS0169, CS0168

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Enchant.Domain.Iterator;
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Result;
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

namespace Gs2.Gs2Enchant.Domain.Model
{

    public partial class RarityParameterStatusDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnchantRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _parameterName;
        private readonly string _propertyId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string ParameterName => _parameterName;
        public string PropertyId => _propertyId;

        public RarityParameterStatusDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnchantRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parameterName = parameterName;
            propertyId = propertyId?.Replace("{region}", gs2.RestSession.Region.DisplayName()).Replace("{ownerId}", gs2.RestSession.OwnerId ?? "").Replace("{userId}", UserId);
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "RarityParameterStatus"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId,
            string childType
        )
        {
            return string.Join(
                ":",
                "enchant",
                namespaceName ?? "null",
                userId ?? "null",
                parameterName ?? "null",
                propertyId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string parameterName,
            string propertyId
        )
        {
            return string.Join(
                ":",
                parameterName ?? "null",
                propertyId ?? "null"
            );
        }

    }

    public partial class RarityParameterStatusDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Enchant.Model.RarityParameterStatus> GetFuture(
            GetRarityParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.RarityParameterStatus> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetRarityParameterStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            request.ParameterName.ToString(),
                            request.PropertyId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "rarityParameterStatus")
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

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "RarityParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            resultModel.Item.ParameterName.ToString(),
                            resultModel.Item.PropertyId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.RarityParameterStatus>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Enchant.Model.RarityParameterStatus> GetAsync(
            #else
        private async Task<Gs2.Gs2Enchant.Model.RarityParameterStatus> GetAsync(
            #endif
            GetRarityParameterStatusByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            GetRarityParameterStatusByUserIdResult result = null;
            try {
                result = await this._client.GetRarityParameterStatusByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                    request.ParameterName.ToString(),
                    request.PropertyId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "rarityParameterStatus")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "RarityParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
                        resultModel.Item.PropertyId.ToString()
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
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> DeleteFuture(
            DeleteRarityParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.DeleteRarityParameterStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            request.ParameterName.ToString(),
                            request.PropertyId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "rarityParameterStatus")
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

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "RarityParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            resultModel.Item.ParameterName.ToString(),
                            resultModel.Item.PropertyId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Enchant.Model.RarityParameterStatus>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> DeleteAsync(
            #endif
            DeleteRarityParameterStatusByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            DeleteRarityParameterStatusByUserIdResult result = null;
            try {
                result = await this._client.DeleteRarityParameterStatusByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                    request.ParameterName.ToString(),
                    request.PropertyId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "rarityParameterStatus")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "RarityParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
                        resultModel.Item.PropertyId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Enchant.Model.RarityParameterStatus>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> Delete(
            DeleteRarityParameterStatusByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> ReDrawFuture(
            ReDrawRarityParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.ReDrawRarityParameterStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "RarityParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            resultModel.Item.ParameterName.ToString(),
                            resultModel.Item.PropertyId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> ReDrawAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> ReDrawAsync(
            #endif
            ReDrawRarityParameterStatusByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            ReDrawRarityParameterStatusByUserIdResult result = null;
                result = await this._client.ReDrawRarityParameterStatusByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "RarityParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
                        resultModel.Item.PropertyId.ToString()
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
        [Obsolete("The name has been changed to ReDrawFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> ReDraw(
            ReDrawRarityParameterStatusByUserIdRequest request
        ) {
            return ReDrawFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> AddFuture(
            AddRarityParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.AddRarityParameterStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "RarityParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            resultModel.Item.ParameterName.ToString(),
                            resultModel.Item.PropertyId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> AddAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> AddAsync(
            #endif
            AddRarityParameterStatusByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            AddRarityParameterStatusByUserIdResult result = null;
                result = await this._client.AddRarityParameterStatusByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "RarityParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
                        resultModel.Item.PropertyId.ToString()
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
        [Obsolete("The name has been changed to AddFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> Add(
            AddRarityParameterStatusByUserIdRequest request
        ) {
            return AddFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> VerifyFuture(
            VerifyRarityParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.VerifyRarityParameterStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "RarityParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            resultModel.Item.ParameterName.ToString(),
                            resultModel.Item.PropertyId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> VerifyAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> VerifyAsync(
            #endif
            VerifyRarityParameterStatusByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            VerifyRarityParameterStatusByUserIdResult result = null;
                result = await this._client.VerifyRarityParameterStatusByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "RarityParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
                        resultModel.Item.PropertyId.ToString()
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
        [Obsolete("The name has been changed to VerifyFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> Verify(
            VerifyRarityParameterStatusByUserIdRequest request
        ) {
            return VerifyFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> SetFuture(
            SetRarityParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.SetRarityParameterStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "RarityParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            resultModel.Item.ParameterName.ToString(),
                            resultModel.Item.PropertyId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> SetAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> SetAsync(
            #endif
            SetRarityParameterStatusByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            SetRarityParameterStatusByUserIdResult result = null;
                result = await this._client.SetRarityParameterStatusByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "RarityParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
                        resultModel.Item.PropertyId.ToString()
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
        [Obsolete("The name has been changed to SetFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain> Set(
            SetRarityParameterStatusByUserIdRequest request
        ) {
            return SetFuture(request);
        }
        #endif

    }

    public partial class RarityParameterStatusDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Model.RarityParameterStatus> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.RarityParameterStatus> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                    _parentKey,
                    Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                        this.ParameterName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetRarityParameterStatusByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                                    this.ParameterName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "rarityParameterStatus")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                        _parentKey,
                        Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            this.ParameterName?.ToString(),
                            this.PropertyId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.RarityParameterStatus>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Model.RarityParameterStatus> ModelAsync()
            #else
        public async Task<Gs2.Gs2Enchant.Model.RarityParameterStatus> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                    _parentKey,
                    Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                        this.ParameterName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetRarityParameterStatusByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                                    this.ParameterName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors.Length == 0 || e.errors[0].component != "rarityParameterStatus")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                        _parentKey,
                        Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                            this.ParameterName?.ToString(),
                            this.PropertyId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Enchant.Model.RarityParameterStatus> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Enchant.Model.RarityParameterStatus> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Enchant.Model.RarityParameterStatus> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Enchant.Model.RarityParameterStatus> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                    this.ParameterName.ToString(),
                    this.PropertyId.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain.CreateCacheKey(
                    this.ParameterName.ToString(),
                    this.PropertyId.ToString()
                ),
                callbackId
            );
        }

    }
}

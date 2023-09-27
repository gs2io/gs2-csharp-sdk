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

    public partial class BalanceParameterStatusDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
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

        public BalanceParameterStatusDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2EnchantRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parameterName = parameterName;
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "BalanceParameterStatus"
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

    public partial class BalanceParameterStatusDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> GetFuture(
            GetBalanceParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetBalanceParameterStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                            request.ParameterName.ToString(),
                            request.PropertyId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "balanceParameterStatus")
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
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                GetBalanceParameterStatusByUserIdResult result = null;
                try {
                    result = await this._client.GetBalanceParameterStatusByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        request.ParameterName.ToString(),
                        request.PropertyId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "balanceParameterStatus")
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
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "BalanceParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Enchant.Model.BalanceParameterStatus> GetAsync(
            GetBalanceParameterStatusByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.GetBalanceParameterStatusByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        request.ParameterName.ToString(),
                        request.PropertyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "balanceParameterStatus")
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
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            GetBalanceParameterStatusByUserIdResult result = null;
            try {
                result = await this._client.GetBalanceParameterStatusByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                    request.ParameterName.ToString(),
                    request.PropertyId.ToString()
                    );
                _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "balanceParameterStatus")
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
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BalanceParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> DeleteFuture(
            DeleteBalanceParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.DeleteBalanceParameterStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                            request.ParameterName.ToString(),
                            request.PropertyId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "balanceParameterStatus")
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
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                DeleteBalanceParameterStatusByUserIdResult result = null;
                try {
                    result = await this._client.DeleteBalanceParameterStatusByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        request.ParameterName.ToString(),
                        request.PropertyId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "balanceParameterStatus")
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
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "BalanceParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                            resultModel.Item.ParameterName.ToString(),
                            resultModel.Item.PropertyId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> DeleteAsync(
            DeleteBalanceParameterStatusByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.DeleteBalanceParameterStatusByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        request.ParameterName.ToString(),
                        request.PropertyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "balanceParameterStatus")
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
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            DeleteBalanceParameterStatusByUserIdResult result = null;
            try {
                result = await this._client.DeleteBalanceParameterStatusByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                    request.ParameterName.ToString(),
                    request.PropertyId.ToString()
                    );
                _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "balanceParameterStatus")
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
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BalanceParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
                        resultModel.Item.PropertyId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> DeleteAsync(
            DeleteBalanceParameterStatusByUserIdRequest request
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
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> Delete(
            DeleteBalanceParameterStatusByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> ReDrawFuture(
            ReDrawBalanceParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.ReDrawBalanceParameterStatusByUserIdFuture(
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
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                ReDrawBalanceParameterStatusByUserIdResult result = null;
                    result = await this._client.ReDrawBalanceParameterStatusByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "BalanceParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> ReDrawAsync(
            ReDrawBalanceParameterStatusByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.ReDrawBalanceParameterStatusByUserIdFuture(
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
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            ReDrawBalanceParameterStatusByUserIdResult result = null;
                result = await this._client.ReDrawBalanceParameterStatusByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BalanceParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> ReDrawAsync(
            ReDrawBalanceParameterStatusByUserIdRequest request
        ) {
            var future = ReDrawFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ReDrawFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> ReDraw(
            ReDrawBalanceParameterStatusByUserIdRequest request
        ) {
            return ReDrawFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> SetFuture(
            SetBalanceParameterStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.SetBalanceParameterStatusByUserIdFuture(
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
                    .WithParameterName(this.ParameterName)
                    .WithPropertyId(this.PropertyId);
                SetBalanceParameterStatusByUserIdResult result = null;
                    result = await this._client.SetBalanceParameterStatusByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "BalanceParameterStatus"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> SetAsync(
            SetBalanceParameterStatusByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.SetBalanceParameterStatusByUserIdFuture(
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
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            SetBalanceParameterStatusByUserIdResult result = null;
                result = await this._client.SetBalanceParameterStatusByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BalanceParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> SetAsync(
            SetBalanceParameterStatusByUserIdRequest request
        ) {
            var future = SetFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to SetFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> Set(
            SetBalanceParameterStatusByUserIdRequest request
        ) {
            return SetFuture(request);
        }
        #endif

    }

    public partial class BalanceParameterStatusDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                    _parentKey,
                    Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        this.ParameterName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetBalanceParameterStatusByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                                    this.ParameterName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "balanceParameterStatus")
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
                    (value, _) = _cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                        _parentKey,
                        Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                            this.ParameterName?.ToString(),
                            this.PropertyId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Enchant.Model.BalanceParameterStatus> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                    _parentKey,
                    Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        this.ParameterName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetBalanceParameterStatusByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                                    this.ParameterName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "balanceParameterStatus")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                        _parentKey,
                        Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Enchant.Model.BalanceParameterStatus> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Enchant.Model.BalanceParameterStatus> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Enchant.Model.BalanceParameterStatus> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Enchant.Model.BalanceParameterStatus> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                    this.ParameterName.ToString(),
                    this.PropertyId.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                    this.ParameterName.ToString(),
                    this.PropertyId.ToString()
                ),
                callbackId
            );
        }

    }
}

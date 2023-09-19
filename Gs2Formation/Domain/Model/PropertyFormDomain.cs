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
using Gs2.Gs2Formation.Domain.Iterator;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
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

namespace Gs2.Gs2Formation.Domain.Model
{

    public partial class PropertyFormDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _propertyFormModelName;
        private readonly string _propertyId;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string PropertyFormModelName => _propertyFormModelName;
        public string PropertyId => _propertyId;

        public PropertyFormDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string propertyFormModelName,
            string propertyId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FormationRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._propertyFormModelName = propertyFormModelName;
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "PropertyForm"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string propertyFormModelName,
            string propertyId,
            string childType
        )
        {
            return string.Join(
                ":",
                "formation",
                namespaceName ?? "null",
                userId ?? "null",
                propertyFormModelName ?? "null",
                propertyId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string propertyFormModelName,
            string propertyId
        )
        {
            return string.Join(
                ":",
                propertyFormModelName ?? "null",
                propertyId ?? "null"
            );
        }

    }

    public partial class PropertyFormDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.PropertyForm> GetFuture(
            GetPropertyFormByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.PropertyForm> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetPropertyFormByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            request.PropertyFormModelName.ToString(),
                            request.PropertyId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "propertyForm")
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
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                GetPropertyFormByUserIdResult result = null;
                try {
                    result = await this._client.GetPropertyFormByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString(),
                        request.PropertyId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "propertyForm")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.PropertyFormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                            resultModel.PropertyFormModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.PropertyFormModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.PropertyForm>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Formation.Model.PropertyForm> GetAsync(
            GetPropertyFormByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.GetPropertyFormByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString(),
                        request.PropertyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "propertyForm")
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
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            GetPropertyFormByUserIdResult result = null;
            try {
                result = await this._client.GetPropertyFormByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                    request.PropertyFormModelName.ToString(),
                    request.PropertyId.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "propertyForm")
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
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "PropertyForm"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString(),
                        requestModel.PropertyId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.PropertyFormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                        resultModel.PropertyFormModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.PropertyFormModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> GetWithSignatureFuture(
            GetPropertyFormWithSignatureByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetPropertyFormWithSignatureByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            request.PropertyFormModelName.ToString(),
                            request.PropertyId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "propertyForm")
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
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                GetPropertyFormWithSignatureByUserIdResult result = null;
                try {
                    result = await this._client.GetPropertyFormWithSignatureByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString(),
                        request.PropertyId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "propertyForm")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.PropertyFormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                            resultModel.PropertyFormModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.PropertyFormModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;
                domain.Body = result?.Body;
                domain.Signature = result?.Signature;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> GetWithSignatureAsync(
            GetPropertyFormWithSignatureByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.GetPropertyFormWithSignatureByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString(),
                        request.PropertyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "propertyForm")
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
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            GetPropertyFormWithSignatureByUserIdResult result = null;
            try {
                result = await this._client.GetPropertyFormWithSignatureByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                    request.PropertyFormModelName.ToString(),
                    request.PropertyId.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "propertyForm")
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
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "PropertyForm"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString(),
                        requestModel.PropertyId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.PropertyFormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                        resultModel.PropertyFormModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.PropertyFormModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> GetWithSignatureAsync(
            GetPropertyFormWithSignatureByUserIdRequest request
        ) {
            var future = GetWithSignatureFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to GetWithSignatureFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> GetWithSignature(
            GetPropertyFormWithSignatureByUserIdRequest request
        ) {
            return GetWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> SetFuture(
            SetPropertyFormByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.SetPropertyFormByUserIdFuture(
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
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                SetPropertyFormByUserIdResult result = null;
                    result = await this._client.SetPropertyFormByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.PropertyFormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                            resultModel.PropertyFormModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.PropertyFormModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> SetAsync(
            SetPropertyFormByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.SetPropertyFormByUserIdFuture(
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
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            SetPropertyFormByUserIdResult result = null;
                result = await this._client.SetPropertyFormByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "PropertyForm"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString(),
                        requestModel.PropertyId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.PropertyFormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                        resultModel.PropertyFormModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.PropertyFormModel,
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> SetAsync(
            SetPropertyFormByUserIdRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> Set(
            SetPropertyFormByUserIdRequest request
        ) {
            return SetFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesFuture(
            AcquireActionsToPropertyFormPropertiesRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.AcquireActionsToPropertyFormPropertiesFuture(
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
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                AcquireActionsToPropertyFormPropertiesResult result = null;
                    result = await this._client.AcquireActionsToPropertyFormPropertiesAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var stampSheet = new Gs2.Core.Domain.TransactionDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    this.UserId,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId

                );
                if (result?.StampSheet != null)
                {
                    var future2 = stampSheet.Wait();
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }

            self.OnComplete(stampSheet);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        }
        #else
        public async Task<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesAsync(
            AcquireActionsToPropertyFormPropertiesRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.AcquireActionsToPropertyFormPropertiesFuture(
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
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            AcquireActionsToPropertyFormPropertiesResult result = null;
                result = await this._client.AcquireActionsToPropertyFormPropertiesAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "PropertyForm"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString(),
                        requestModel.PropertyId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            var stampSheet = new Gs2.Core.Domain.TransactionDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.UserId,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId

            );
            if (result?.StampSheet != null)
            {
                await stampSheet.WaitAsync();
            }

            return stampSheet;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesAsync(
            AcquireActionsToPropertyFormPropertiesRequest request
        ) {
            var future = AcquireActionsToPropertiesFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to AcquireActionsToPropertiesFuture.")]
        public IFuture<Gs2.Core.Domain.TransactionDomain> AcquireActionsToProperties(
            AcquireActionsToPropertyFormPropertiesRequest request
        ) {
            return AcquireActionsToPropertiesFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> DeleteFuture(
            DeletePropertyFormByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.DeletePropertyFormByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            request.PropertyFormModelName.ToString(),
                            request.PropertyId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "propertyForm")
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
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                DeletePropertyFormByUserIdResult result = null;
                try {
                    result = await this._client.DeletePropertyFormByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString(),
                        request.PropertyId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "propertyForm")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.PropertyForm>(parentKey, key);
                    }
                    if (resultModel.PropertyFormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                            resultModel.PropertyFormModel.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.PropertyFormModel>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> DeleteAsync(
            DeletePropertyFormByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.DeletePropertyFormByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString(),
                        request.PropertyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "propertyForm")
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
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            DeletePropertyFormByUserIdResult result = null;
            try {
                result = await this._client.DeletePropertyFormByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                    request.PropertyFormModelName.ToString(),
                    request.PropertyId.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "propertyForm")
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
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "PropertyForm"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString(),
                        requestModel.PropertyId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.PropertyForm>(parentKey, key);
                }
                if (resultModel.PropertyFormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                        resultModel.PropertyFormModel.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.PropertyFormModel>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> DeleteAsync(
            DeletePropertyFormByUserIdRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> Delete(
            DeletePropertyFormByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class PropertyFormDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.PropertyForm> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.PropertyForm> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.PropertyForm>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        this.PropertyFormModelName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetPropertyFormByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                                    this.PropertyFormModelName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "propertyForm")
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
                    (value, _) = _cache.Get<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            this.PropertyFormModelName?.ToString(),
                            this.PropertyId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.PropertyForm>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Model.PropertyForm> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.PropertyForm>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                        this.PropertyFormModelName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetPropertyFormByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                                    this.PropertyFormModelName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "propertyForm")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            this.PropertyFormModelName?.ToString(),
                            this.PropertyId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Model.PropertyForm> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Formation.Model.PropertyForm> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Formation.Model.PropertyForm> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Formation.Model.PropertyForm> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

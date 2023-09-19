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

    public partial class PropertyFormAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _propertyFormModelName;
        private readonly string _propertyId;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string PropertyFormModelName => _propertyFormModelName;
        public string PropertyId => _propertyId;

        public PropertyFormAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
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
            this._accessToken = accessToken;
            this._propertyFormModelName = propertyFormModelName;
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "PropertyForm"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.PropertyForm> GetFuture(
            GetPropertyFormRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.PropertyForm> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetPropertyFormFuture(
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                GetPropertyFormResult result = null;
                try {
                    result = await this._client.GetPropertyFormAsync(
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
            GetPropertyFormRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.GetPropertyFormFuture(
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
                .WithAccessToken(this._accessToken?.Token)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            GetPropertyFormResult result = null;
            try {
                result = await this._client.GetPropertyFormAsync(
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> GetWithSignatureFuture(
            GetPropertyFormWithSignatureRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetPropertyFormWithSignatureFuture(
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                GetPropertyFormWithSignatureResult result = null;
                try {
                    result = await this._client.GetPropertyFormWithSignatureAsync(
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> GetWithSignatureAsync(
            GetPropertyFormWithSignatureRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.GetPropertyFormWithSignatureFuture(
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
                .WithAccessToken(this._accessToken?.Token)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            GetPropertyFormWithSignatureResult result = null;
            try {
                result = await this._client.GetPropertyFormWithSignatureAsync(
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> GetWithSignatureAsync(
            GetPropertyFormWithSignatureRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> GetWithSignature(
            GetPropertyFormWithSignatureRequest request
        ) {
            return GetWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> SetWithSignatureFuture(
            SetPropertyFormWithSignatureRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.SetPropertyFormWithSignatureFuture(
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                SetPropertyFormWithSignatureResult result = null;
                    result = await this._client.SetPropertyFormWithSignatureAsync(
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
                    if (resultModel.ProeprtyFormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                            resultModel.ProeprtyFormModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.ProeprtyFormModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> SetWithSignatureAsync(
            SetPropertyFormWithSignatureRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.SetPropertyFormWithSignatureFuture(
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
                .WithAccessToken(this._accessToken?.Token)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            SetPropertyFormWithSignatureResult result = null;
                result = await this._client.SetPropertyFormWithSignatureAsync(
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
                if (resultModel.ProeprtyFormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                        resultModel.ProeprtyFormModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.ProeprtyFormModel,
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> SetWithSignatureAsync(
            SetPropertyFormWithSignatureRequest request
        ) {
            var future = SetWithSignatureFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to SetWithSignatureFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> SetWithSignature(
            SetPropertyFormWithSignatureRequest request
        ) {
            return SetWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> DeleteFuture(
            DeletePropertyFormRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.DeletePropertyFormFuture(
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPropertyFormModelName(this.PropertyFormModelName)
                    .WithPropertyId(this.PropertyId);
                DeletePropertyFormResult result = null;
                try {
                    result = await this._client.DeletePropertyFormAsync(
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> DeleteAsync(
            DeletePropertyFormRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            var future = this._client.DeletePropertyFormFuture(
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
                .WithAccessToken(this._accessToken?.Token)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            DeletePropertyFormResult result = null;
            try {
                result = await this._client.DeletePropertyFormAsync(
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> DeleteAsync(
            DeletePropertyFormRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain> Delete(
            DeletePropertyFormRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

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
                        new GetPropertyFormRequest()
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
                        new GetPropertyFormRequest()
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

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

    public partial class FormAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _moldModelName;
        private readonly int? _index;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string MoldModelName => _moldModelName;
        public int? Index => _index;

        public FormAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string moldModelName,
            int? index
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
            this._moldModelName = moldModelName;
            this._index = index;
            this._parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.MoldModelName,
                "Form"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.Form> GetFuture(
            GetFormRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Form> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.GetFormFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            request.Index.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.Form>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "form")
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
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                GetFormResult result = null;
                try {
                    result = await this._client.GetFormAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        request.Index.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "form")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.MoldModelName,
                            "Form"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            resultModel.Item.Index.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Mold != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Mold.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Mold,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.MoldModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MoldModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                            resultModel.MoldModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.MoldModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.FormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.MoldModelName,
                            "FormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                            resultModel.FormModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.FormModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.Form>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Formation.Model.Form> GetAsync(
            GetFormRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            var future = this._client.GetFormFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        request.Index.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "form")
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
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            GetFormResult result = null;
            try {
                result = await this._client.GetFormAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    request.Index.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "form")
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
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.MoldModelName,
                        "Form"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        resultModel.Item.Index.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.Mold != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mold"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                        resultModel.Mold.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Mold,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.MoldModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MoldModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                        resultModel.MoldModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.MoldModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.FormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.MoldModelName,
                        "FormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                        resultModel.FormModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.FormModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> GetWithSignatureFuture(
            GetFormWithSignatureRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.GetFormWithSignatureFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            request.Index.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.Form>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "form")
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
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                GetFormWithSignatureResult result = null;
                try {
                    result = await this._client.GetFormWithSignatureAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        request.Index.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "form")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.MoldModelName,
                            "Form"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            resultModel.Item.Index.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Mold != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Mold.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Mold,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.MoldModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MoldModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                            resultModel.MoldModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.MoldModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.FormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.MoldModelName,
                            "FormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                            resultModel.FormModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.FormModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;
                domain.Body = result?.Body;
                domain.Signature = result?.Signature;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> GetWithSignatureAsync(
            GetFormWithSignatureRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            var future = this._client.GetFormWithSignatureFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        request.Index.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "form")
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
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            GetFormWithSignatureResult result = null;
            try {
                result = await this._client.GetFormWithSignatureAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    request.Index.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "form")
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
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.MoldModelName,
                        "Form"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        resultModel.Item.Index.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.Mold != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mold"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                        resultModel.Mold.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Mold,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.MoldModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MoldModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                        resultModel.MoldModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.MoldModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.FormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.MoldModelName,
                        "FormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                        resultModel.FormModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.FormModel,
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> GetWithSignatureAsync(
            GetFormWithSignatureRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> GetWithSignature(
            GetFormWithSignatureRequest request
        ) {
            return GetWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> SetWithSignatureFuture(
            SetFormWithSignatureRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.SetFormWithSignatureFuture(
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
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                SetFormWithSignatureResult result = null;
                    result = await this._client.SetFormWithSignatureAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.MoldModelName,
                            "Form"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            resultModel.Item.Index.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Mold != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Mold.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Mold,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.MoldModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MoldModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                            resultModel.MoldModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.MoldModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.FormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.MoldModelName,
                            "FormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                            resultModel.FormModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.FormModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> SetWithSignatureAsync(
            SetFormWithSignatureRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            var future = this._client.SetFormWithSignatureFuture(
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
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            SetFormWithSignatureResult result = null;
                result = await this._client.SetFormWithSignatureAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.MoldModelName,
                        "Form"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        resultModel.Item.Index.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.Mold != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mold"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                        resultModel.Mold.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Mold,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.MoldModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MoldModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                        resultModel.MoldModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.MoldModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.FormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.MoldModelName,
                        "FormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                        resultModel.FormModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.FormModel,
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> SetWithSignatureAsync(
            SetFormWithSignatureRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> SetWithSignature(
            SetFormWithSignatureRequest request
        ) {
            return SetWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> DeleteFuture(
            DeleteFormRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.DeleteFormFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            request.Index.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.Form>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "form")
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
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                DeleteFormResult result = null;
                try {
                    result = await this._client.DeleteFormAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        request.Index.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "form")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.MoldModelName,
                            "Form"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            resultModel.Item.Index.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.Form>(parentKey, key);
                    }
                    if (resultModel.Mold != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Mold.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.Mold>(parentKey, key);
                    }
                    if (resultModel.MoldModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MoldModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                            resultModel.MoldModel.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.MoldModel>(parentKey, key);
                    }
                    if (resultModel.FormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.MoldModelName,
                            "FormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                            resultModel.FormModel.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.FormModel>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> DeleteAsync(
            DeleteFormRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            var future = this._client.DeleteFormFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        request.Index.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "form")
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
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            DeleteFormResult result = null;
            try {
                result = await this._client.DeleteFormAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    request.Index.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "form")
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
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.MoldModelName,
                        "Form"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        resultModel.Item.Index.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.Form>(parentKey, key);
                }
                if (resultModel.Mold != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mold"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                        resultModel.Mold.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.Mold>(parentKey, key);
                }
                if (resultModel.MoldModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MoldModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                        resultModel.MoldModel.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.MoldModel>(parentKey, key);
                }
                if (resultModel.FormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.MoldModelName,
                        "FormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                        resultModel.FormModel.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.FormModel>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> DeleteAsync(
            DeleteFormRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain> Delete(
            DeleteFormRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string moldModelName,
            string index,
            string childType
        )
        {
            return string.Join(
                ":",
                "formation",
                namespaceName ?? "null",
                userId ?? "null",
                moldModelName ?? "null",
                index ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string index
        )
        {
            return string.Join(
                ":",
                index ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.Form> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Form> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        this.Index?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetFormRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                                    this.Index?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Formation.Model.Form>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "form")
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
                    (value, _) = _cache.Get<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            this.Index?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.Form>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Model.Form> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        this.Index?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetFormRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                                    this.Index?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "form")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            this.Index?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Model.Form> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Formation.Model.Form> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Formation.Model.Form> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Formation.Model.Form> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Formation.Model.Form> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    this.Index.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Formation.Model.Form>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    this.Index.ToString()
                ),
                callbackId
            );
        }

    }
}

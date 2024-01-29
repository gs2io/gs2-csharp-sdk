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
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Formation.Domain.Model
{

    public partial class FormDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _moldModelName;
        private readonly int? _index;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string MoldModelName => _moldModelName;
        public int? Index => _index;

        public FormDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string moldModelName,
            int? index
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FormationRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._moldModelName = moldModelName;
            this._index = index;
            this._parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.MoldModelName,
                "Form"
            );
        }

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

    }

    public partial class FormDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.Form> GetFuture(
            GetFormByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Form> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.GetFormByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            request.Index.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Form>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "form")
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Formation.Model.Form> GetAsync(
            #else
        private async Task<Gs2.Gs2Formation.Model.Form> GetAsync(
            #endif
            GetFormByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            GetFormByUserIdResult result = null;
            try {
                result = await this._client.GetFormByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    request.Index.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "form")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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
                    );
                    _gs2.Cache.Put(
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> GetWithSignatureFuture(
            GetFormWithSignatureByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.GetFormWithSignatureByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            request.Index.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Form>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "form")
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        );
                        _gs2.Cache.Put(
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormDomain> GetWithSignatureAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormDomain> GetWithSignatureAsync(
            #endif
            GetFormWithSignatureByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            GetFormWithSignatureByUserIdResult result = null;
            try {
                result = await this._client.GetFormWithSignatureByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    request.Index.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "form")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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
                    );
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to GetWithSignatureFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> GetWithSignature(
            GetFormWithSignatureByUserIdRequest request
        ) {
            return GetWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> SetFuture(
            SetFormByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.SetFormByUserIdFuture(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        );
                        _gs2.Cache.Put(
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormDomain> SetAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormDomain> SetAsync(
            #endif
            SetFormByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            SetFormByUserIdResult result = null;
                result = await this._client.SetFormByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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
                    );
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to SetFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> Set(
            SetFormByUserIdRequest request
        ) {
            return SetFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesFuture(
            AcquireActionsToFormPropertiesRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.AcquireActionsToFormPropertiesFuture(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Mold,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.UserId,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId
                );
                if (result.StampSheet != null) {
                    var future2 = transaction.WaitFuture(true);
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }
                self.OnComplete(transaction);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionDomain> AcquireActionsToPropertiesAsync(
            #endif
            AcquireActionsToFormPropertiesRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            AcquireActionsToFormPropertiesResult result = null;
                result = await this._client.AcquireActionsToFormPropertiesAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Mold,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.UserId,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to AcquireActionsToPropertiesFuture.")]
        public IFuture<Gs2.Core.Domain.TransactionDomain> AcquireActionsToProperties(
            AcquireActionsToFormPropertiesRequest request
        ) {
            return AcquireActionsToPropertiesFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> DeleteFuture(
            DeleteFormByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMoldModelName(this.MoldModelName)
                    .WithIndex(this.Index);
                var future = this._client.DeleteFormByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            request.Index.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Form>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "form")
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
                        _gs2.Cache.Delete<Gs2.Gs2Formation.Model.Form>(parentKey, key);
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
                        _gs2.Cache.Delete<Gs2.Gs2Formation.Model.Mold>(parentKey, key);
                    }
                    if (resultModel.MoldModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MoldModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                            resultModel.MoldModel.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Formation.Model.MoldModel>(parentKey, key);
                    }
                    if (resultModel.FormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.MoldModelName,
                            "FormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Formation.Model.FormModel>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormDomain> DeleteAsync(
            #endif
            DeleteFormByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMoldModelName(this.MoldModelName)
                .WithIndex(this.Index);
            DeleteFormByUserIdResult result = null;
            try {
                result = await this._client.DeleteFormByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    request.Index.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "form")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Delete<Gs2.Gs2Formation.Model.Form>(parentKey, key);
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
                    _gs2.Cache.Delete<Gs2.Gs2Formation.Model.Mold>(parentKey, key);
                }
                if (resultModel.MoldModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MoldModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                        resultModel.MoldModel.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Formation.Model.MoldModel>(parentKey, key);
                }
                if (resultModel.FormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.MoldModelName,
                        "FormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormModelDomain.CreateCacheKey(
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Formation.Model.FormModel>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormDomain> Delete(
            DeleteFormByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class FormDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.Form> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Form> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        this.Index?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetFormByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                                    this.Index?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Form>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "form")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.Form>(
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Model.Form> ModelAsync()
            #else
        public async Task<Gs2.Gs2Formation.Model.Form> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Formation.Model.Form>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    this.Index?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.Form>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                        this.Index?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetFormByUserIdRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                                    this.Index?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Form>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "form")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.Form>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            this.Index?.ToString()
                        )
                    );
                }
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
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


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Formation.Model.Form>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    this.Index.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Formation.Model.Form> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    this.Index.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Formation.Model.Form>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                    this.Index.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Formation.Model.Form> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Formation.Model.Form> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Formation.Model.Form> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}

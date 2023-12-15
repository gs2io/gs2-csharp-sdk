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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
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
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string propertyFormModelName,
            string propertyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FormationRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._propertyFormModelName = propertyFormModelName;
            propertyId = propertyId?.Replace("{region}", gs2.RestSession.Region.DisplayName()).Replace("{ownerId}", gs2.RestSession.OwnerId ?? "").Replace("{userId}", UserId);
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
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "propertyForm")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Formation.Model.PropertyForm> GetAsync(
            #else
        private async Task<Gs2.Gs2Formation.Model.PropertyForm> GetAsync(
            #endif
            GetPropertyFormByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "propertyForm")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "propertyForm")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> GetWithSignatureAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> GetWithSignatureAsync(
            #endif
            GetPropertyFormWithSignatureByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "propertyForm")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> SetAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> SetAsync(
            #endif
            SetPropertyFormByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            SetPropertyFormByUserIdResult result = null;
                result = await this._client.SetPropertyFormByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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
                    _gs2.Cache.Put(
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
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
            AcquireActionsToPropertyFormPropertiesRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyFormModelName(this.PropertyFormModelName)
                .WithPropertyId(this.PropertyId);
            AcquireActionsToPropertyFormPropertiesResult result = null;
                result = await this._client.AcquireActionsToPropertyFormPropertiesAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
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
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "propertyForm")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Formation.Model.PropertyForm>(parentKey, key);
                    }
                    if (resultModel.PropertyFormModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                            resultModel.PropertyFormModel.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Formation.Model.PropertyFormModel>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormDomain> DeleteAsync(
            #endif
            DeletePropertyFormByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "propertyForm")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Delete<Gs2.Gs2Formation.Model.PropertyForm>(parentKey, key);
                }
                if (resultModel.PropertyFormModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain.CreateCacheKey(
                        resultModel.PropertyFormModel.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Formation.Model.PropertyFormModel>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.PropertyForm>(
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
                            this._gs2.Cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "propertyForm")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.PropertyForm>(
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Model.PropertyForm> ModelAsync()
            #else
        public async Task<Gs2.Gs2Formation.Model.PropertyForm> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Formation.Model.PropertyForm>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                    this.PropertyFormModelName?.ToString(),
                    this.PropertyId?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.PropertyForm>(
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
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.PropertyForm>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "propertyForm")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.PropertyForm>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            this.PropertyFormModelName?.ToString(),
                            this.PropertyId?.ToString()
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


        public ulong Subscribe(Action<Gs2.Gs2Formation.Model.PropertyForm> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                    this.PropertyFormModelName.ToString(),
                    this.PropertyId.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Formation.Model.PropertyForm>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                    this.PropertyFormModelName.ToString(),
                    this.PropertyId.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Formation.Model.PropertyForm> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Formation.Model.PropertyForm> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Formation.Model.PropertyForm> callback)
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

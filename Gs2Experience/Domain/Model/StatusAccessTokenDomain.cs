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
using Gs2.Gs2Experience.Domain.Iterator;
using Gs2.Gs2Experience.Request;
using Gs2.Gs2Experience.Result;
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

namespace Gs2.Gs2Experience.Domain.Model
{

    public partial class StatusAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ExperienceRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _experienceName;
        private readonly string _propertyId;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string ExperienceName => _experienceName;
        public string PropertyId => _propertyId;

        public StatusAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string experienceName,
            string propertyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ExperienceRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._experienceName = experienceName;
            propertyId = propertyId?.Replace("{region}", gs2.RestSession.Region.DisplayName()).Replace("{ownerId}", gs2.RestSession.OwnerId ?? "").Replace("{userId}", UserId);
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Status"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Experience.Model.Status> GetFuture(
            GetStatusRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Model.Status> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithExperienceName(this.ExperienceName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetStatusFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                            request.ExperienceName.ToString(),
                            request.PropertyId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Experience.Model.Status>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "status")
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
                        var parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Item.ExperienceName.ToString(),
                            resultModel.Item.PropertyId.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Model.Status>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Experience.Model.Status> GetAsync(
            #else
        private async Task<Gs2.Gs2Experience.Model.Status> GetAsync(
            #endif
            GetStatusRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            GetStatusResult result = null;
            try {
                result = await this._client.GetStatusAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    request.ExperienceName.ToString(),
                    request.PropertyId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Experience.Model.Status>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "status")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Item.ExperienceName.ToString(),
                        resultModel.Item.PropertyId.ToString()
                    );
                    _gs2.Cache.Put(
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
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> GetWithSignatureFuture(
            GetStatusWithSignatureRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithExperienceName(this.ExperienceName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetStatusWithSignatureFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                            request.ExperienceName.ToString(),
                            request.PropertyId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Experience.Model.Status>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "status")
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
                        var parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Item.ExperienceName.ToString(),
                            resultModel.Item.PropertyId.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;
                domain.Body = result?.Body;
                domain.Signature = result?.Signature;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> GetWithSignatureAsync(
            #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> GetWithSignatureAsync(
            #endif
            GetStatusWithSignatureRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            GetStatusWithSignatureResult result = null;
            try {
                result = await this._client.GetStatusWithSignatureAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    request.ExperienceName.ToString(),
                    request.PropertyId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Experience.Model.Status>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "status")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Item.ExperienceName.ToString(),
                        resultModel.Item.PropertyId.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
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
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> GetWithSignature(
            GetStatusWithSignatureRequest request
        ) {
            return GetWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> VerifyRankFuture(
            VerifyRankRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithExperienceName(this.ExperienceName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.VerifyRankFuture(
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
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> VerifyRankAsync(
            #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> VerifyRankAsync(
            #endif
            VerifyRankRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            VerifyRankResult result = null;
                result = await this._client.VerifyRankAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to VerifyRankFuture.")]
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> VerifyRank(
            VerifyRankRequest request
        ) {
            return VerifyRankFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> VerifyRankCapFuture(
            VerifyRankCapRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithExperienceName(this.ExperienceName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.VerifyRankCapFuture(
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
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> VerifyRankCapAsync(
            #else
        public async Task<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> VerifyRankCapAsync(
            #endif
            VerifyRankCapRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithExperienceName(this.ExperienceName)
                .WithPropertyId(this.PropertyId);
            VerifyRankCapResult result = null;
                result = await this._client.VerifyRankCapAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to VerifyRankCapFuture.")]
        public IFuture<Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain> VerifyRankCap(
            VerifyRankCapRequest request
        ) {
            return VerifyRankCapFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string experienceName,
            string propertyId,
            string childType
        )
        {
            return string.Join(
                ":",
                "experience",
                namespaceName ?? "null",
                userId ?? "null",
                experienceName ?? "null",
                propertyId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string experienceName,
            string propertyId
        )
        {
            return string.Join(
                ":",
                experienceName ?? "null",
                propertyId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Model.Status> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Model.Status> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Experience.Model.Status>(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        this.ExperienceName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetStatusRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                                    this.ExperienceName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Experience.Model.Status>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "status")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Experience.Model.Status>(
                        _parentKey,
                        Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                            this.ExperienceName?.ToString(),
                            this.PropertyId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Model.Status>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Experience.Model.Status> ModelAsync()
            #else
        public async Task<Gs2.Gs2Experience.Model.Status> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Experience.Model.Status>(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    this.ExperienceName?.ToString(),
                    this.PropertyId?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Experience.Model.Status>(
                    _parentKey,
                    Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        this.ExperienceName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetStatusRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                                    this.ExperienceName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Experience.Model.Status>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "status")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Experience.Model.Status>(
                        _parentKey,
                        Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                            this.ExperienceName?.ToString(),
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
        public async UniTask<Gs2.Gs2Experience.Model.Status> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Experience.Model.Status> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Experience.Model.Status> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Experience.Model.Status> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    this.ExperienceName.ToString(),
                    this.PropertyId.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Experience.Model.Status>(
                _parentKey,
                Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    this.ExperienceName.ToString(),
                    this.PropertyId.ToString()
                ),
                callbackId
            );
        }

    }
}

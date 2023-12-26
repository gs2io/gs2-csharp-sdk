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
 *
 * deny overwrite
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
using Gs2.Gs2Grade.Domain.Iterator;
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Grade.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Experience.Model;
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

namespace Gs2.Gs2Grade.Domain.Model
{

    public partial class StatusDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GradeRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _gradeName;
        private readonly string _propertyId;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string GradeName => _gradeName;
        public string PropertyId => _propertyId;

        public StatusDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string gradeName,
            string propertyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GradeRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._gradeName = gradeName;
            propertyId = propertyId?.Replace("{region}", gs2.RestSession.Region.DisplayName()).Replace("{ownerId}", gs2.RestSession.OwnerId ?? "").Replace("{userId}", UserId);
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Status"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string gradeName,
            string propertyId,
            string childType
        )
        {
            return string.Join(
                ":",
                "grade",
                namespaceName ?? "null",
                userId ?? "null",
                gradeName ?? "null",
                propertyId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string gradeName,
            string propertyId
        )
        {
            return string.Join(
                ":",
                gradeName ?? "null",
                propertyId ?? "null"
            );
        }

    }

    public partial class StatusDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Grade.Model.Status> GetFuture(
            GetStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Model.Status> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            request.GradeName.ToString(),
                            request.PropertyId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Grade.Model.Status>(
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Item.GradeName.ToString(),
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Model.Status>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Grade.Model.Status> GetAsync(
            #else
        private async Task<Gs2.Gs2Grade.Model.Status> GetAsync(
            #endif
            GetStatusByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            GetStatusByUserIdResult result = null;
            try {
                result = await this._client.GetStatusByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                    request.GradeName.ToString(),
                    request.PropertyId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Grade.Model.Status>(
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
                    var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Item.GradeName.ToString(),
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
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> AddGradeFuture(
            AddGradeByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.AddGradeByUserIdFuture(
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Item.GradeName.ToString(),
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

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> AddGradeAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> AddGradeAsync(
            #endif
            AddGradeByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            AddGradeByUserIdResult result = null;
                result = await this._client.AddGradeByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Item.GradeName.ToString(),
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

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to AddGradeFuture.")]
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> AddGrade(
            AddGradeByUserIdRequest request
        ) {
            return AddGradeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> SubGradeFuture(
            SubGradeByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.SubGradeByUserIdFuture(
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Item.GradeName.ToString(),
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

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> SubGradeAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> SubGradeAsync(
            #endif
            SubGradeByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            SubGradeByUserIdResult result = null;
                result = await this._client.SubGradeByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Item.GradeName.ToString(),
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

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to SubGradeFuture.")]
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> SubGrade(
            SubGradeByUserIdRequest request
        ) {
            return SubGradeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> SetGradeFuture(
            SetGradeByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.SetGradeByUserIdFuture(
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Item.GradeName.ToString(),
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

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> SetGradeAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> SetGradeAsync(
            #endif
            SetGradeByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            SetGradeByUserIdResult result = null;
                result = await this._client.SetGradeByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Item.GradeName.ToString(),
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

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to SetGradeFuture.")]
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> SetGrade(
            SetGradeByUserIdRequest request
        ) {
            return SetGradeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> ApplyRankCapFuture(
            ApplyRankCapByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.ApplyRankCapByUserIdFuture(
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Item.GradeName.ToString(),
                            resultModel.Item.PropertyId.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    {
                        var future2 = this._gs2.Grade.Namespace(
                            this.NamespaceName
                        ).GradeModel(
                            this.GradeName
                        ).ModelFuture();
                        yield return future2;
                        if (future2.Error != null)
                        {
                            self.OnError(future2.Error);
                            yield break;
                        }
                        var grade = future2.Result;
                    
                        var parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                            ExperienceModel.GetNamespaceNameFromGrn(grade.ExperienceModelId),
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                            ExperienceModel.GetExperienceNameFromGrn(grade.ExperienceModelId),
                            resultModel.Item.PropertyId.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Experience.Model.Status>(
                            parentKey,
                            key
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> ApplyRankCapAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> ApplyRankCapAsync(
            #endif
            ApplyRankCapByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            ApplyRankCapByUserIdResult result = null;
                result = await this._client.ApplyRankCapByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Item.GradeName.ToString(),
                        resultModel.Item.PropertyId.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                {
                    var grade = await this._gs2.Grade.Namespace(
                        this.NamespaceName
                    ).GradeModel(
                        this.GradeName
                    ).ModelAsync();
                    
                    var parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                        ExperienceModel.GetNamespaceNameFromGrn(grade.ExperienceModelId),
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                        ExperienceModel.GetExperienceNameFromGrn(grade.ExperienceModelId),
                        resultModel.Item.PropertyId.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Experience.Model.Status>(
                        parentKey,
                        key
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to ApplyRankCapFuture.")]
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> ApplyRankCap(
            ApplyRankCapByUserIdRequest request
        ) {
            return ApplyRankCapFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> DeleteFuture(
            DeleteStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.DeleteStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            request.GradeName.ToString(),
                            request.PropertyId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Grade.Model.Status>(
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Item.GradeName.ToString(),
                            resultModel.Item.PropertyId.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Grade.Model.Status>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> DeleteAsync(
            #endif
            DeleteStatusByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            DeleteStatusByUserIdResult result = null;
            try {
                result = await this._client.DeleteStatusByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                    request.GradeName.ToString(),
                    request.PropertyId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Grade.Model.Status>(
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
                    var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Item.GradeName.ToString(),
                        resultModel.Item.PropertyId.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Grade.Model.Status>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> Delete(
            DeleteStatusByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeFuture(
            VerifyGradeByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.VerifyGradeByUserIdFuture(
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeAsync(
            #endif
            VerifyGradeByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            VerifyGradeByUserIdResult result = null;
                result = await this._client.VerifyGradeByUserIdAsync(
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
        [Obsolete("The name has been changed to VerifyGradeFuture.")]
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGrade(
            VerifyGradeByUserIdRequest request
        ) {
            return VerifyGradeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeUpMaterialFuture(
            VerifyGradeUpMaterialByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.VerifyGradeUpMaterialByUserIdFuture(
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeUpMaterialAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeUpMaterialAsync(
            #endif
            VerifyGradeUpMaterialByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            VerifyGradeUpMaterialByUserIdResult result = null;
                result = await this._client.VerifyGradeUpMaterialByUserIdAsync(
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
        [Obsolete("The name has been changed to VerifyGradeUpMaterialFuture.")]
        public IFuture<Gs2.Gs2Grade.Domain.Model.StatusDomain> VerifyGradeUpMaterial(
            VerifyGradeUpMaterialByUserIdRequest request
        ) {
            return VerifyGradeUpMaterialFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> MultiplyAcquireActionsFuture(
            MultiplyAcquireActionsByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGradeName(this.GradeName)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.MultiplyAcquireActionsByUserIdFuture(
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
        public async UniTask<Gs2.Core.Domain.TransactionDomain> MultiplyAcquireActionsAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionDomain> MultiplyAcquireActionsAsync(
            #endif
            MultiplyAcquireActionsByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGradeName(this.GradeName)
                .WithPropertyId(this.PropertyId);
            MultiplyAcquireActionsByUserIdResult result = null;
                result = await this._client.MultiplyAcquireActionsByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
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
        [Obsolete("The name has been changed to MultiplyAcquireActionsFuture.")]
        public IFuture<Gs2.Core.Domain.TransactionDomain> MultiplyAcquireActions(
            MultiplyAcquireActionsByUserIdRequest request
        ) {
            return MultiplyAcquireActionsFuture(request);
        }
        #endif

    }

    public partial class StatusDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Model.Status> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Model.Status> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Grade.Model.Status>(
                    _parentKey,
                    Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        this.GradeName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetStatusByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                                    this.GradeName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Grade.Model.Status>(
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Grade.Model.Status>(
                        _parentKey,
                        Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            this.GradeName?.ToString(),
                            this.PropertyId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Model.Status>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Model.Status> ModelAsync()
            #else
        public async Task<Gs2.Gs2Grade.Model.Status> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Grade.Model.Status>(
                _parentKey,
                Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                    this.GradeName?.ToString(),
                    this.PropertyId?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Grade.Model.Status>(
                    _parentKey,
                    Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        this.GradeName?.ToString(),
                        this.PropertyId?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetStatusByUserIdRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                                    this.GradeName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Grade.Model.Status>(
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Grade.Model.Status>(
                        _parentKey,
                        Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            this.GradeName?.ToString(),
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
        public async UniTask<Gs2.Gs2Grade.Model.Status> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Grade.Model.Status> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Grade.Model.Status> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Grade.Model.Status>(
                _parentKey,
                Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                    this.GradeName.ToString(),
                    this.PropertyId.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Grade.Model.Status> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                    this.GradeName.ToString(),
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Grade.Model.Status>(
                _parentKey,
                Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                    this.GradeName.ToString(),
                    this.PropertyId.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Grade.Model.Status> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Grade.Model.Status> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Grade.Model.Status> callback)
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

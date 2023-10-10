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

    public partial class NamespaceDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string Status { get; set; }
        public string Url { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;

        public NamespaceDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FormationRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._parentKey = "formation:Namespace";
        }

        public Gs2.Gs2Formation.Domain.Model.CurrentFormMasterDomain CurrentFormMaster(
        ) {
            return new Gs2.Gs2Formation.Domain.Model.CurrentFormMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.MoldModel> MoldModels(
        )
        {
            return new DescribeMoldModelsIterator(
                this._cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.MoldModel> MoldModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.MoldModel> MoldModels(
            #endif
        #else
        public DescribeMoldModelsIterator MoldModels(
        #endif
        )
        {
            return new DescribeMoldModelsIterator(
                this._cache,
                this._client,
                this.NamespaceName
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeMoldModels(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Formation.Model.MoldModel>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "MoldModel"
                ),
                callback
            );
        }

        public void UnsubscribeMoldModels(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Formation.Model.MoldModel>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "MoldModel"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.MoldModelDomain MoldModel(
            string moldModelName
        ) {
            return new Gs2.Gs2Formation.Domain.Model.MoldModelDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                moldModelName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.PropertyFormModel> PropertyFormModels(
        )
        {
            return new DescribePropertyFormModelsIterator(
                this._cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.PropertyFormModel> PropertyFormModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.PropertyFormModel> PropertyFormModels(
            #endif
        #else
        public DescribePropertyFormModelsIterator PropertyFormModels(
        #endif
        )
        {
            return new DescribePropertyFormModelsIterator(
                this._cache,
                this._client,
                this.NamespaceName
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribePropertyFormModels(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Formation.Model.PropertyFormModel>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "PropertyFormModel"
                ),
                callback
            );
        }

        public void UnsubscribePropertyFormModels(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Formation.Model.PropertyFormModel>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "PropertyFormModel"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain PropertyFormModel(
            string propertyFormModelName
        ) {
            return new Gs2.Gs2Formation.Domain.Model.PropertyFormModelDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                propertyFormModelName
            );
        }

        public Gs2.Gs2Formation.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Formation.Domain.Model.UserDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                userId
            );
        }

        public UserAccessTokenDomain AccessToken(
            AccessToken accessToken
        ) {
            return new UserAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.PropertyFormModelMaster> PropertyFormModelMasters(
        )
        {
            return new DescribePropertyFormModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.PropertyFormModelMaster> PropertyFormModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.PropertyFormModelMaster> PropertyFormModelMasters(
            #endif
        #else
        public DescribePropertyFormModelMastersIterator PropertyFormModelMasters(
        #endif
        )
        {
            return new DescribePropertyFormModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribePropertyFormModelMasters(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "PropertyFormModelMaster"
                ),
                callback
            );
        }

        public void UnsubscribePropertyFormModelMasters(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "PropertyFormModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain PropertyFormModelMaster(
            string propertyFormModelName
        ) {
            return new Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                propertyFormModelName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.FormModelMaster> FormModelMasters(
        )
        {
            return new DescribeFormModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.FormModelMaster> FormModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.FormModelMaster> FormModelMasters(
            #endif
        #else
        public DescribeFormModelMastersIterator FormModelMasters(
        #endif
        )
        {
            return new DescribeFormModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeFormModelMasters(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Formation.Model.FormModelMaster>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "FormModelMaster"
                ),
                callback
            );
        }

        public void UnsubscribeFormModelMasters(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Formation.Model.FormModelMaster>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "FormModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain FormModelMaster(
            string formModelName
        ) {
            return new Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                formModelName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.MoldModelMaster> MoldModelMasters(
        )
        {
            return new DescribeMoldModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.MoldModelMaster> MoldModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.MoldModelMaster> MoldModelMasters(
            #endif
        #else
        public DescribeMoldModelMastersIterator MoldModelMasters(
        #endif
        )
        {
            return new DescribeMoldModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeMoldModelMasters(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Formation.Model.MoldModelMaster>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "MoldModelMaster"
                ),
                callback
            );
        }

        public void UnsubscribeMoldModelMasters(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Formation.Model.MoldModelMaster>(
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "MoldModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain MoldModelMaster(
            string moldModelName
        ) {
            return new Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                moldModelName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "formation",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string namespaceName
        )
        {
            return string.Join(
                ":",
                namespaceName ?? "null"
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceStatusFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                GetNamespaceStatusResult result = null;
                try {
                    result = await this._client.GetNamespaceStatusAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> GetStatusAsync(
            GetNamespaceStatusRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetNamespaceStatusFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceStatusResult result = null;
            try {
                result = await this._client.GetNamespaceStatusAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> GetStatusAsync(
            GetNamespaceStatusRequest request
        ) {
            var future = GetStatusFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to GetStatusFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> GetStatus(
            GetNamespaceStatusRequest request
        ) {
            return GetStatusFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Namespace> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                GetNamespaceResult result = null;
                try {
                    result = await this._client.GetNamespaceAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "formation",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.Namespace>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Formation.Model.Namespace> GetAsync(
            GetNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceResult result = null;
            try {
                result = await this._client.GetNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "formation",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateNamespaceFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                UpdateNamespaceResult result = null;
                    result = await this._client.UpdateNamespaceAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "formation",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> UpdateAsync(
            UpdateNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.UpdateNamespaceFuture(
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
                .WithNamespaceName(this.NamespaceName);
            UpdateNamespaceResult result = null;
                result = await this._client.UpdateNamespaceAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "formation",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> UpdateAsync(
            UpdateNamespaceRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> Update(
            UpdateNamespaceRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DeleteNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                DeleteNamespaceResult result = null;
                try {
                    result = await this._client.DeleteNamespaceAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "formation",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.Namespace>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> DeleteAsync(
            DeleteNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.DeleteNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            DeleteNamespaceResult result = null;
            try {
                result = await this._client.DeleteNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "formation",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.Namespace>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> DeleteAsync(
            DeleteNamespaceRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> Delete(
            DeleteNamespaceRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> CreatePropertyFormModelMasterFuture(
            CreatePropertyFormModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreatePropertyFormModelMasterFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                CreatePropertyFormModelMasterResult result = null;
                    result = await this._client.CreatePropertyFormModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> CreatePropertyFormModelMasterAsync(
            CreatePropertyFormModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.CreatePropertyFormModelMasterFuture(
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
                .WithNamespaceName(this.NamespaceName);
            CreatePropertyFormModelMasterResult result = null;
                result = await this._client.CreatePropertyFormModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> CreatePropertyFormModelMasterAsync(
            CreatePropertyFormModelMasterRequest request
        ) {
            var future = CreatePropertyFormModelMasterFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreatePropertyFormModelMasterFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> CreatePropertyFormModelMaster(
            CreatePropertyFormModelMasterRequest request
        ) {
            return CreatePropertyFormModelMasterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain> CreateFormModelMasterFuture(
            CreateFormModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateFormModelMasterFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                CreateFormModelMasterResult result = null;
                    result = await this._client.CreateFormModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "FormModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain> CreateFormModelMasterAsync(
            CreateFormModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.CreateFormModelMasterFuture(
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
                .WithNamespaceName(this.NamespaceName);
            CreateFormModelMasterResult result = null;
                result = await this._client.CreateFormModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "FormModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain> CreateFormModelMasterAsync(
            CreateFormModelMasterRequest request
        ) {
            var future = CreateFormModelMasterFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateFormModelMasterFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.FormModelMasterDomain> CreateFormModelMaster(
            CreateFormModelMasterRequest request
        ) {
            return CreateFormModelMasterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> CreateMoldModelMasterFuture(
            CreateMoldModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateMoldModelMasterFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                CreateMoldModelMasterResult result = null;
                    result = await this._client.CreateMoldModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MoldModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> CreateMoldModelMasterAsync(
            CreateMoldModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.CreateMoldModelMasterFuture(
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
                .WithNamespaceName(this.NamespaceName);
            CreateMoldModelMasterResult result = null;
                result = await this._client.CreateMoldModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MoldModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> CreateMoldModelMasterAsync(
            CreateMoldModelMasterRequest request
        ) {
            var future = CreateMoldModelMasterFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateMoldModelMasterFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> CreateMoldModelMaster(
            CreateMoldModelMasterRequest request
        ) {
            return CreateMoldModelMasterFuture(request);
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Namespace> self)
            {
                var parentKey = string.Join(
                    ":",
                    "formation",
                    "Namespace"
                );
                var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetNamespaceRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "namespace")
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
                    (value, _) = _cache.Get<Gs2.Gs2Formation.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.Namespace>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Model.Namespace> ModelAsync()
        {
            var parentKey = string.Join(
                ":",
                "formation",
                "Namespace"
            );
            var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetNamespaceRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Formation.Model.Namespace>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "namespace")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Formation.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Model.Namespace> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Formation.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Formation.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Formation.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Formation.Model.Namespace> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Formation.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callbackId
            );
        }

    }
}

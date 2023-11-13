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

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnchantRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string Status { get; set; }
        public string Url { get; set; }
        public string UploadToken { get; set; }
        public string UploadUrl { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnchantRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._parentKey = "enchant:Namespace";
        }

        public Gs2.Gs2Enchant.Domain.Model.CurrentParameterMasterDomain CurrentParameterMaster(
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.CurrentParameterMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Enchant.Model.BalanceParameterModel> BalanceParameterModels(
        )
        {
            return new DescribeBalanceParameterModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Enchant.Model.BalanceParameterModel> BalanceParameterModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Enchant.Model.BalanceParameterModel> BalanceParameterModels(
            #endif
        #else
        public DescribeBalanceParameterModelsIterator BalanceParameterModelsAsync(
        #endif
        )
        {
            return new DescribeBalanceParameterModelsIterator(
                this._gs2.Cache,
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

        public ulong SubscribeBalanceParameterModels(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "BalanceParameterModel"
                ),
                callback
            );
        }

        public void UnsubscribeBalanceParameterModels(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "BalanceParameterModel"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain BalanceParameterModel(
            string parameterName
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain(
                this._gs2,
                this.NamespaceName,
                parameterName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Enchant.Model.BalanceParameterModelMaster> BalanceParameterModelMasters(
        )
        {
            return new DescribeBalanceParameterModelMastersIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Enchant.Model.BalanceParameterModelMaster> BalanceParameterModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Enchant.Model.BalanceParameterModelMaster> BalanceParameterModelMasters(
            #endif
        #else
        public DescribeBalanceParameterModelMastersIterator BalanceParameterModelMastersAsync(
        #endif
        )
        {
            return new DescribeBalanceParameterModelMastersIterator(
                this._gs2.Cache,
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

        public ulong SubscribeBalanceParameterModelMasters(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enchant.Model.BalanceParameterModelMaster>(
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "BalanceParameterModelMaster"
                ),
                callback
            );
        }

        public void UnsubscribeBalanceParameterModelMasters(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enchant.Model.BalanceParameterModelMaster>(
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "BalanceParameterModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain BalanceParameterModelMaster(
            string parameterName
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                parameterName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Enchant.Model.RarityParameterModel> RarityParameterModels(
        )
        {
            return new DescribeRarityParameterModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Enchant.Model.RarityParameterModel> RarityParameterModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Enchant.Model.RarityParameterModel> RarityParameterModels(
            #endif
        #else
        public DescribeRarityParameterModelsIterator RarityParameterModelsAsync(
        #endif
        )
        {
            return new DescribeRarityParameterModelsIterator(
                this._gs2.Cache,
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

        public ulong SubscribeRarityParameterModels(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enchant.Model.RarityParameterModel>(
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "RarityParameterModel"
                ),
                callback
            );
        }

        public void UnsubscribeRarityParameterModels(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enchant.Model.RarityParameterModel>(
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "RarityParameterModel"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Enchant.Domain.Model.RarityParameterModelDomain RarityParameterModel(
            string parameterName
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.RarityParameterModelDomain(
                this._gs2,
                this.NamespaceName,
                parameterName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Enchant.Model.RarityParameterModelMaster> RarityParameterModelMasters(
        )
        {
            return new DescribeRarityParameterModelMastersIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Enchant.Model.RarityParameterModelMaster> RarityParameterModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Enchant.Model.RarityParameterModelMaster> RarityParameterModelMasters(
            #endif
        #else
        public DescribeRarityParameterModelMastersIterator RarityParameterModelMastersAsync(
        #endif
        )
        {
            return new DescribeRarityParameterModelMastersIterator(
                this._gs2.Cache,
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

        public ulong SubscribeRarityParameterModelMasters(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enchant.Model.RarityParameterModelMaster>(
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "RarityParameterModelMaster"
                ),
                callback
            );
        }

        public void UnsubscribeRarityParameterModelMasters(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enchant.Model.RarityParameterModelMaster>(
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "RarityParameterModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain RarityParameterModelMaster(
            string parameterName
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                parameterName
            );
        }

        public Gs2.Gs2Enchant.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.UserDomain(
                this._gs2,
                this.NamespaceName,
                userId
            );
        }

        public UserAccessTokenDomain AccessToken(
            AccessToken accessToken
        ) {
            return new UserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                accessToken
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "enchant",
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
        public IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceStatusFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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
                    
                }
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> GetStatusAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> GetStatusAsync(
            #endif
            GetNamespaceStatusRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceStatusResult result = null;
            try {
                result = await this._client.GetNamespaceStatusAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to GetStatusFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> GetStatus(
            GetNamespaceStatusRequest request
        ) {
            return GetStatusFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Enchant.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.Namespace> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "enchant",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Enchant.Model.Namespace> GetAsync(
            #else
        private async Task<Gs2.Gs2Enchant.Model.Namespace> GetAsync(
            #endif
            GetNamespaceRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceResult result = null;
            try {
                result = await this._client.GetNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "enchant",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> self)
            {
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

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "enchant",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> UpdateAsync(
            #endif
            UpdateNamespaceRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            UpdateNamespaceResult result = null;
                result = await this._client.UpdateNamespaceAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "enchant",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
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
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> Update(
            UpdateNamespaceRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DeleteNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "enchant",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Enchant.Model.Namespace>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> DeleteAsync(
            #endif
            DeleteNamespaceRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            DeleteNamespaceResult result = null;
            try {
                result = await this._client.DeleteNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "enchant",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Enchant.Model.Namespace>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> Delete(
            DeleteNamespaceRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain> CreateBalanceParameterModelMasterFuture(
            CreateBalanceParameterModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateBalanceParameterModelMasterFuture(
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
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BalanceParameterModelMaster"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain> CreateBalanceParameterModelMasterAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain> CreateBalanceParameterModelMasterAsync(
            #endif
            CreateBalanceParameterModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            CreateBalanceParameterModelMasterResult result = null;
                result = await this._client.CreateBalanceParameterModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BalanceParameterModelMaster"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateBalanceParameterModelMasterFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelMasterDomain> CreateBalanceParameterModelMaster(
            CreateBalanceParameterModelMasterRequest request
        ) {
            return CreateBalanceParameterModelMasterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain> CreateRarityParameterModelMasterFuture(
            CreateRarityParameterModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateRarityParameterModelMasterFuture(
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
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RarityParameterModelMaster"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain> CreateRarityParameterModelMasterAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain> CreateRarityParameterModelMasterAsync(
            #endif
            CreateRarityParameterModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            CreateRarityParameterModelMasterResult result = null;
                result = await this._client.CreateRarityParameterModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RarityParameterModelMaster"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateRarityParameterModelMasterFuture.")]
        public IFuture<Gs2.Gs2Enchant.Domain.Model.RarityParameterModelMasterDomain> CreateRarityParameterModelMaster(
            CreateRarityParameterModelMasterRequest request
        ) {
            return CreateRarityParameterModelMasterFuture(request);
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.Namespace> self)
            {
                var parentKey = string.Join(
                    ":",
                    "enchant",
                    "Namespace"
                );
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                            var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.Namespace>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "namespace")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.Namespace>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Model.Namespace> ModelAsync()
            #else
        public async Task<Gs2.Gs2Enchant.Model.Namespace> ModelAsync()
            #endif
        {
            var parentKey = string.Join(
                ":",
                "enchant",
                "Namespace"
            );
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Enchant.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetNamespaceRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.Namespace>(
                            parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "namespace")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
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
        public async UniTask<Gs2.Gs2Enchant.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Enchant.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Enchant.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Enchant.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Enchant.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callbackId
            );
        }

    }
}

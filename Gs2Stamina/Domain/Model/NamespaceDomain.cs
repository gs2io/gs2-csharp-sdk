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
using Gs2.Gs2Stamina.Domain.Iterator;
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
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

namespace Gs2.Gs2Stamina.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2StaminaRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string Status { get; set; }
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
            this._client = new Gs2StaminaRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._parentKey = "stamina:Gs2.Gs2Stamina.Model.Namespace";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> GetStatusAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> GetStatus(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> GetStatusAsync(
        #endif
            GetNamespaceStatusRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetNamespaceStatusFuture(
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
            var result = await this._client.GetNamespaceStatusAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Stamina.Domain.Model.NamespaceDomain domain = this;
            this.Status = domain.Status = result?.Status;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Stamina.Model.Namespace> GetAsync(
            #else
        private IFuture<Gs2.Gs2Stamina.Model.Namespace> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Stamina.Model.Namespace> GetAsync(
        #endif
            GetNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.Namespace> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetNamespaceFuture(
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
            var result = await this._client.GetNamespaceAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.Namespace>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> UpdateAsync(
        #endif
            UpdateNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.UpdateNamespaceAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Stamina.Domain.Model.NamespaceDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> DeleteAsync(
        #endif
            DeleteNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteNamespaceFuture(
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
            DeleteNamespaceResult result = null;
            try {
                result = await this._client.DeleteNamespaceAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Stamina.Domain.Model.NamespaceDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> CreateRecoverIntervalTableMasterAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> CreateRecoverIntervalTableMaster(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> CreateRecoverIntervalTableMasterAsync(
        #endif
            CreateRecoverIntervalTableMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateRecoverIntervalTableMasterFuture(
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
            var result = await this._client.CreateRecoverIntervalTableMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "RecoverIntervalTableMaster"
                );
                var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain domain = new Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.Name
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> CreateMaxStaminaTableMasterAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> CreateMaxStaminaTableMaster(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> CreateMaxStaminaTableMasterAsync(
        #endif
            CreateMaxStaminaTableMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateMaxStaminaTableMasterFuture(
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
            var result = await this._client.CreateMaxStaminaTableMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "MaxStaminaTableMaster"
                );
                var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain domain = new Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.Name
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> CreateRecoverValueTableMasterAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> CreateRecoverValueTableMaster(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> CreateRecoverValueTableMasterAsync(
        #endif
            CreateRecoverValueTableMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateRecoverValueTableMasterFuture(
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
            var result = await this._client.CreateRecoverValueTableMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "RecoverValueTableMaster"
                );
                var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain domain = new Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.Name
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> CreateStaminaModelMasterAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> CreateStaminaModelMaster(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> CreateStaminaModelMasterAsync(
        #endif
            CreateStaminaModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateStaminaModelMasterFuture(
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
            var result = await this._client.CreateStaminaModelMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "StaminaModelMaster"
                );
                var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain domain = new Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.Name
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain>(Impl);
        #endif
        }

        public Gs2.Gs2Stamina.Domain.Model.CurrentStaminaMasterDomain CurrentStaminaMaster(
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.CurrentStaminaMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableDomain MaxStaminaTable(
            string maxStaminaTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                maxStaminaTableName
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableDomain RecoverIntervalTable(
            string recoverIntervalTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                recoverIntervalTableName
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.RecoverValueTableDomain RecoverValueTable(
            string recoverValueTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.RecoverValueTableDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                recoverValueTableName
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Stamina.Model.StaminaModel> StaminaModels(
        )
        {
            return new DescribeStaminaModelsIterator(
                this._cache,
                this._client,
                this._namespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.StaminaModel> StaminaModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Stamina.Model.StaminaModel> StaminaModels(
            #endif
        #else
        public DescribeStaminaModelsIterator StaminaModels(
        #endif
        )
        {
            return new DescribeStaminaModelsIterator(
                this._cache,
                this._client,
                this._namespaceName
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

        public Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain StaminaModel(
            string staminaName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                staminaName
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.UserDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
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
                this._namespaceName,
                accessToken
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> RecoverIntervalTableMasters(
        )
        {
            return new DescribeRecoverIntervalTableMastersIterator(
                this._cache,
                this._client,
                this._namespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> RecoverIntervalTableMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> RecoverIntervalTableMasters(
            #endif
        #else
        public DescribeRecoverIntervalTableMastersIterator RecoverIntervalTableMasters(
        #endif
        )
        {
            return new DescribeRecoverIntervalTableMastersIterator(
                this._cache,
                this._client,
                this._namespaceName
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

        public Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain RecoverIntervalTableMaster(
            string recoverIntervalTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                recoverIntervalTableName
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> MaxStaminaTableMasters(
        )
        {
            return new DescribeMaxStaminaTableMastersIterator(
                this._cache,
                this._client,
                this._namespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> MaxStaminaTableMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> MaxStaminaTableMasters(
            #endif
        #else
        public DescribeMaxStaminaTableMastersIterator MaxStaminaTableMasters(
        #endif
        )
        {
            return new DescribeMaxStaminaTableMastersIterator(
                this._cache,
                this._client,
                this._namespaceName
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

        public Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain MaxStaminaTableMaster(
            string maxStaminaTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                maxStaminaTableName
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> RecoverValueTableMasters(
        )
        {
            return new DescribeRecoverValueTableMastersIterator(
                this._cache,
                this._client,
                this._namespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> RecoverValueTableMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> RecoverValueTableMasters(
            #endif
        #else
        public DescribeRecoverValueTableMastersIterator RecoverValueTableMasters(
        #endif
        )
        {
            return new DescribeRecoverValueTableMastersIterator(
                this._cache,
                this._client,
                this._namespaceName
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

        public Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain RecoverValueTableMaster(
            string recoverValueTableName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                recoverValueTableName
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Stamina.Model.StaminaModelMaster> StaminaModelMasters(
        )
        {
            return new DescribeStaminaModelMastersIterator(
                this._cache,
                this._client,
                this._namespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.StaminaModelMaster> StaminaModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Stamina.Model.StaminaModelMaster> StaminaModelMasters(
            #endif
        #else
        public DescribeStaminaModelMastersIterator StaminaModelMasters(
        #endif
        )
        {
            return new DescribeStaminaModelMastersIterator(
                this._cache,
                this._client,
                this._namespaceName
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

        public Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain StaminaModelMaster(
            string staminaName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.StaminaModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                staminaName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "stamina",
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Model.Namespace> Model() {
            #else
        public IFuture<Gs2.Gs2Stamina.Model.Namespace> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Model.Namespace> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.Namespace> self)
            {
        #endif
            Gs2.Gs2Stamina.Model.Namespace value = _cache.Get<Gs2.Gs2Stamina.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetNamespaceRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "namespace")
                            {
                                _cache.Delete<Gs2.Gs2Stamina.Model.Namespace>(
                                    _parentKey,
                                    Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheKey(
                                        this.NamespaceName?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "namespace")
                    {
                    _cache.Delete<Gs2.Gs2Stamina.Model.Namespace>(
                            _parentKey,
                            Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheKey(
                                this.NamespaceName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Stamina.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
                )
            );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.Namespace>(Impl);
        #endif
        }

    }
}

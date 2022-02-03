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
using Gs2.Gs2Datastore.Domain.Iterator;
using Gs2.Gs2Datastore.Request;
using Gs2.Gs2Datastore.Result;
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

namespace Gs2.Gs2Datastore.Domain.Model
{

    public partial class DataObjectAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2DatastoreRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;
        private readonly string _dataObjectName;

        private readonly String _parentKey;
        public string UploadUrl { get; set; }
        public string FileUrl { get; set; }
        public long? ContentLength { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public string DataObjectName => _dataObjectName;

        public DataObjectAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string dataObjectName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2DatastoreRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._dataObjectName = dataObjectName;
            this._parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                "DataObject"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> UpdateAsync(
        #endif
            UpdateDataObjectRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this._dataObjectName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateDataObjectFuture(
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
            var result = await this._client.UpdateDataObjectAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        request.DataObjectName != null ? request.DataObjectName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareReUploadAsync(
            #else
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareReUpload(
            #endif
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareReUploadAsync(
        #endif
            PrepareReUploadRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this._dataObjectName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.PrepareReUploadFuture(
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
            var result = await this._client.PrepareReUploadAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        request.DataObjectName != null ? request.DataObjectName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain domain = this;
            domain.UploadUrl = result?.UploadUrl;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DoneUploadAsync(
            #else
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DoneUpload(
            #endif
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DoneUploadAsync(
        #endif
            DoneUploadRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this._dataObjectName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DoneUploadFuture(
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
            var result = await this._client.DoneUploadAsync(
                request
            );
            #endif
            _cache.ListCacheClear<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheParentKey(
                    this.NamespaceName?.ToString(),
                    this.UserId?.ToString(),
                    this.DataObjectName?.ToString(),
                    "DataObjectHistory"
                )
            );
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        request.DataObjectName != null ? request.DataObjectName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DeleteAsync(
        #endif
            DeleteDataObjectRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this._dataObjectName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteDataObjectFuture(
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
            var result = await this._client.DeleteDataObjectAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        request.DataObjectName != null ? request.DataObjectName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataAsync(
            #else
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnData(
            #endif
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataAsync(
        #endif
            PrepareDownloadOwnDataRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this._dataObjectName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.PrepareDownloadOwnDataFuture(
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
            var result = await this._client.PrepareDownloadOwnDataAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        request.DataObjectName != null ? request.DataObjectName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain domain = this;
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataByGenerationAsync(
            #else
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataByGeneration(
            #endif
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataByGenerationAsync(
        #endif
            PrepareDownloadOwnDataByGenerationRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this._dataObjectName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.PrepareDownloadOwnDataByGenerationFuture(
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
            var result = await this._client.PrepareDownloadOwnDataByGenerationAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        request.DataObjectName != null ? request.DataObjectName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain domain = this;
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistories(
            #else
        public Gs2Iterator<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistories(
            #endif
        #else
        public DescribeDataObjectHistoriesIterator DataObjectHistories(
        #endif
        )
        {
            return new DescribeDataObjectHistoriesIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken,
                this._dataObjectName
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

        public Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryAccessTokenDomain DataObjectHistory(
            string generation
        ) {
            return new Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._accessToken,
                this._dataObjectName,
                generation
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string dataObjectName,
            string childType
        )
        {
            return string.Join(
                ":",
                "datastore",
                namespaceName ?? "null",
                userId ?? "null",
                dataObjectName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string dataObjectName
        )
        {
            return string.Join(
                ":",
                dataObjectName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Model.DataObject> Model() {
            #else
        public IFuture<Gs2.Gs2Datastore.Model.DataObject> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Datastore.Model.DataObject> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Model.DataObject> self)
            {
        #endif
            Gs2.Gs2Datastore.Model.DataObject value = _cache.Get<Gs2.Gs2Datastore.Model.DataObject>(
                _parentKey,
                Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                    this.DataObjectName?.ToString()
                )
            );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Model.DataObject>(Impl);
        #endif
        }

    }
}
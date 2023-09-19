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

namespace Gs2.Gs2Datastore.Domain.Model
{

    public partial class DataObjectDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2DatastoreRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _dataObjectName;

        private readonly String _parentKey;
        public string UploadUrl { get; set; }
        public string FileUrl { get; set; }
        public long? ContentLength { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string DataObjectName => _dataObjectName;

        public DataObjectDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
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
            this._userId = userId;
            this._dataObjectName = dataObjectName;
            this._parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "DataObject"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistories(
        )
        {
            return new DescribeDataObjectHistoriesByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.DataObjectName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistoriesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistories(
            #endif
        #else
        public DescribeDataObjectHistoriesByUserIdIterator DataObjectHistories(
        #endif
        )
        {
            return new DescribeDataObjectHistoriesByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.DataObjectName
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

        public Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain DataObjectHistory(
            string generation
        ) {
            return new Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                this.DataObjectName,
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

    }

    public partial class DataObjectDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> UpdateFuture(
            UpdateDataObjectByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = this._client.UpdateDataObjectByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                UpdateDataObjectByUserIdResult result = null;
                    result = await this._client.UpdateDataObjectByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> UpdateAsync(
            UpdateDataObjectByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            var future = this._client.UpdateDataObjectByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            UpdateDataObjectByUserIdResult result = null;
                result = await this._client.UpdateDataObjectByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DataObject"
                    );
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> UpdateAsync(
            UpdateDataObjectByUserIdRequest request
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
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> Update(
            UpdateDataObjectByUserIdRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareReUploadFuture(
            PrepareReUploadByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = this._client.PrepareReUploadByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                PrepareReUploadByUserIdResult result = null;
                    result = await this._client.PrepareReUploadByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
                domain.UploadUrl = result?.UploadUrl;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareReUploadAsync(
            PrepareReUploadByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            var future = this._client.PrepareReUploadByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            PrepareReUploadByUserIdResult result = null;
                result = await this._client.PrepareReUploadByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DataObject"
                    );
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
            domain.UploadUrl = result?.UploadUrl;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareReUploadAsync(
            PrepareReUploadByUserIdRequest request
        ) {
            var future = PrepareReUploadFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PrepareReUploadFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareReUpload(
            PrepareReUploadByUserIdRequest request
        ) {
            return PrepareReUploadFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DoneUploadFuture(
            DoneUploadByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = this._client.DoneUploadByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                DoneUploadByUserIdResult result = null;
                    result = await this._client.DoneUploadByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                        cache.ClearListCache<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                            Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheParentKey(
                                this.NamespaceName?.ToString(),
                                this.UserId?.ToString(),
                                this.DataObjectName?.ToString(),
                                "DataObjectHistory"
                            )
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DoneUploadAsync(
            DoneUploadByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            var future = this._client.DoneUploadByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            DoneUploadByUserIdResult result = null;
                result = await this._client.DoneUploadByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DataObject"
                    );
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    cache.ClearListCache<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                        Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            this.DataObjectName?.ToString(),
                            "DataObjectHistory"
                        )
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DoneUploadAsync(
            DoneUploadByUserIdRequest request
        ) {
            var future = DoneUploadFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DoneUploadFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DoneUpload(
            DoneUploadByUserIdRequest request
        ) {
            return DoneUploadFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DeleteFuture(
            DeleteDataObjectByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = this._client.DeleteDataObjectByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                DeleteDataObjectByUserIdResult result = null;
                    result = await this._client.DeleteDataObjectByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DeleteAsync(
            DeleteDataObjectByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            var future = this._client.DeleteDataObjectByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            DeleteDataObjectByUserIdResult result = null;
                result = await this._client.DeleteDataObjectByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DataObject"
                    );
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DeleteAsync(
            DeleteDataObjectByUserIdRequest request
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
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> Delete(
            DeleteDataObjectByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndNameFuture(
            PrepareDownloadByUserIdAndDataObjectNameRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = this._client.PrepareDownloadByUserIdAndDataObjectNameFuture(
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
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                PrepareDownloadByUserIdAndDataObjectNameResult result = null;
                    result = await this._client.PrepareDownloadByUserIdAndDataObjectNameAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
                domain.FileUrl = result?.FileUrl;
                domain.ContentLength = result?.ContentLength;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndNameAsync(
            PrepareDownloadByUserIdAndDataObjectNameRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            var future = this._client.PrepareDownloadByUserIdAndDataObjectNameFuture(
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
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            PrepareDownloadByUserIdAndDataObjectNameResult result = null;
                result = await this._client.PrepareDownloadByUserIdAndDataObjectNameAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DataObject"
                    );
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndNameAsync(
            PrepareDownloadByUserIdAndDataObjectNameRequest request
        ) {
            var future = PrepareDownloadByUserIdAndNameFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PrepareDownloadByUserIdAndNameFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndName(
            PrepareDownloadByUserIdAndDataObjectNameRequest request
        ) {
            return PrepareDownloadByUserIdAndNameFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndNameAndGenerationFuture(
            PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = this._client.PrepareDownloadByUserIdAndDataObjectNameAndGenerationFuture(
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
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                PrepareDownloadByUserIdAndDataObjectNameAndGenerationResult result = null;
                    result = await this._client.PrepareDownloadByUserIdAndDataObjectNameAndGenerationAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
                domain.FileUrl = result?.FileUrl;
                domain.ContentLength = result?.ContentLength;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndNameAndGenerationAsync(
            PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            var future = this._client.PrepareDownloadByUserIdAndDataObjectNameAndGenerationFuture(
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
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            PrepareDownloadByUserIdAndDataObjectNameAndGenerationResult result = null;
                result = await this._client.PrepareDownloadByUserIdAndDataObjectNameAndGenerationAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DataObject"
                    );
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
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
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndNameAndGenerationAsync(
            PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request
        ) {
            var future = PrepareDownloadByUserIdAndNameAndGenerationFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PrepareDownloadByUserIdAndNameAndGenerationFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndNameAndGeneration(
            PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request
        ) {
            return PrepareDownloadByUserIdAndNameAndGenerationFuture(request);
        }
        #endif

    }

    public partial class DataObjectDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Model.DataObject> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Model.DataObject> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Datastore.Model.DataObject>(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        this.DataObjectName?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Model.DataObject>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Model.DataObject> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Datastore.Model.DataObject>(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                        this.DataObjectName?.ToString()
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Model.DataObject> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Datastore.Model.DataObject> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Datastore.Model.DataObject> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Datastore.Model.DataObject> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

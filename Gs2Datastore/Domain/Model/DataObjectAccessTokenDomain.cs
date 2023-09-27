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

    public partial class DataObjectAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2DatastoreRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _dataObjectName;

        private readonly String _parentKey;
        public string UploadUrl { get; set; }
        public string FileUrl { get; set; }
        public long? ContentLength { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
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
                this.NamespaceName,
                this.UserId,
                "DataObject"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> UpdateFuture(
            UpdateDataObjectRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
                UpdateDataObjectResult result = null;
                    result = await this._client.UpdateDataObjectAsync(
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> UpdateAsync(
            UpdateDataObjectRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
            UpdateDataObjectResult result = null;
                result = await this._client.UpdateDataObjectAsync(
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
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> UpdateAsync(
            UpdateDataObjectRequest request
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
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> Update(
            UpdateDataObjectRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareReUploadFuture(
            PrepareReUploadRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
                PrepareReUploadResult result = null;
                    result = await this._client.PrepareReUploadAsync(
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareReUploadAsync(
            PrepareReUploadRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
            PrepareReUploadResult result = null;
                result = await this._client.PrepareReUploadAsync(
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
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareReUploadAsync(
            PrepareReUploadRequest request
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
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareReUpload(
            PrepareReUploadRequest request
        ) {
            return PrepareReUploadFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DoneUploadFuture(
            DoneUploadRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
                DoneUploadResult result = null;
                    result = await this._client.DoneUploadAsync(
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DoneUploadAsync(
            DoneUploadRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
            DoneUploadResult result = null;
                result = await this._client.DoneUploadAsync(
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
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DoneUploadAsync(
            DoneUploadRequest request
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
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DoneUpload(
            DoneUploadRequest request
        ) {
            return DoneUploadFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DeleteFuture(
            DeleteDataObjectRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
                DeleteDataObjectResult result = null;
                    result = await this._client.DeleteDataObjectAsync(
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DeleteAsync(
            DeleteDataObjectRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
            DeleteDataObjectResult result = null;
                result = await this._client.DeleteDataObjectAsync(
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
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> DeleteAsync(
            DeleteDataObjectRequest request
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
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> Delete(
            DeleteDataObjectRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataFuture(
            PrepareDownloadOwnDataRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
                PrepareDownloadOwnDataResult result = null;
                    result = await this._client.PrepareDownloadOwnDataAsync(
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataAsync(
            PrepareDownloadOwnDataRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
            PrepareDownloadOwnDataResult result = null;
                result = await this._client.PrepareDownloadOwnDataAsync(
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
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataAsync(
            PrepareDownloadOwnDataRequest request
        ) {
            var future = PrepareDownloadOwnDataFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PrepareDownloadOwnDataFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnData(
            PrepareDownloadOwnDataRequest request
        ) {
            return PrepareDownloadOwnDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataByGenerationFuture(
            PrepareDownloadOwnDataByGenerationRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName);
                PrepareDownloadOwnDataByGenerationResult result = null;
                    result = await this._client.PrepareDownloadOwnDataByGenerationAsync(
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataByGenerationAsync(
            PrepareDownloadOwnDataByGenerationRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName);
            PrepareDownloadOwnDataByGenerationResult result = null;
                result = await this._client.PrepareDownloadOwnDataByGenerationAsync(
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
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataByGenerationAsync(
            PrepareDownloadOwnDataByGenerationRequest request
        ) {
            var future = PrepareDownloadOwnDataByGenerationFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PrepareDownloadOwnDataByGenerationFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadOwnDataByGeneration(
            PrepareDownloadOwnDataByGenerationRequest request
        ) {
            return PrepareDownloadOwnDataByGenerationFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistories(
        )
        {
            return new DescribeDataObjectHistoriesIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.DataObjectName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistoriesAsync(
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
                this.NamespaceName,
                this.AccessToken,
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

        public ulong SubscribeDataObjectHistories(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.DataObjectName,
                    "DataObjectHistory"
                ),
                callback
            );
        }

        public void UnsubscribeDataObjectHistories(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.DataObjectName,
                    "DataObjectHistory"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryAccessTokenDomain DataObjectHistory(
            string generation
        ) {
            return new Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this._accessToken,
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


        public ulong Subscribe(Action<Gs2.Gs2Datastore.Model.DataObject> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                    this.DataObjectName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Datastore.Model.DataObject>(
                _parentKey,
                Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                    this.DataObjectName.ToString()
                ),
                callbackId
            );
        }

    }
}

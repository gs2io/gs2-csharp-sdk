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
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Datastore.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DatastoreRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string UploadUrl { get; set; }
        public string FileUrl { get; set; }
        public long? ContentLength { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DatastoreRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Datastore.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareUploadFuture(
            PrepareUploadRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token);
                var future = this._client.PrepareUploadFuture(
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
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.Name
                );
                domain.UploadUrl = result?.UploadUrl;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareUploadAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareUploadAsync(
            #endif
            PrepareUploadRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            PrepareUploadResult result = null;
                result = await this._client.PrepareUploadAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.Name
                );
            domain.UploadUrl = result?.UploadUrl;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to PrepareUploadFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareUpload(
            PrepareUploadRequest request
        ) {
            return PrepareUploadFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadFuture(
            PrepareDownloadRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token);
                var future = this._client.PrepareDownloadFuture(
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
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.Name
                );
                domain.FileUrl = result?.FileUrl;
                domain.ContentLength = result?.ContentLength;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadAsync(
            #endif
            PrepareDownloadRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            PrepareDownloadResult result = null;
                result = await this._client.PrepareDownloadAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.Name
                );
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to PrepareDownloadFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownload(
            PrepareDownloadRequest request
        ) {
            return PrepareDownloadFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadByGenerationFuture(
            PrepareDownloadByGenerationRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token);
                var future = this._client.PrepareDownloadByGenerationFuture(
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
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataObject"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.Name
                );
                domain.FileUrl = result?.FileUrl;
                domain.ContentLength = result?.ContentLength;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadByGenerationAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadByGenerationAsync(
            #endif
            PrepareDownloadByGenerationRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            PrepareDownloadByGenerationResult result = null;
                result = await this._client.PrepareDownloadByGenerationAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.Name
                );
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to PrepareDownloadByGenerationFuture.")]
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadByGeneration(
            PrepareDownloadByGenerationRequest request
        ) {
            return PrepareDownloadByGenerationFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Datastore.Model.DataObject> DataObjects(
            string status
        )
        {
            return new DescribeDataObjectsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                status
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Datastore.Model.DataObject> DataObjectsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Datastore.Model.DataObject> DataObjects(
            #endif
        #else
        public DescribeDataObjectsIterator DataObjectsAsync(
        #endif
            string status
        )
        {
            return new DescribeDataObjectsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                status
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

        public ulong SubscribeDataObjects(
            Action<Gs2.Gs2Datastore.Model.DataObject[]> callback,
            string status
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Datastore.Model.DataObject>(
                Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "DataObject"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDataObjectsWithInitialCallAsync(
            Action<Gs2.Gs2Datastore.Model.DataObject[]> callback,
            string status
        )
        {
            var items = await DataObjectsAsync(
                status
            ).ToArrayAsync();
            var callbackId = SubscribeDataObjects(
                callback,
                status
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDataObjects(
            ulong callbackId,
            string status
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Datastore.Model.DataObject>(
                Gs2.Gs2Datastore.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "DataObject"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain DataObject(
            string dataObjectName
        ) {
            return new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                dataObjectName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "datastore",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
            );
        }

    }
}

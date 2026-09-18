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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Datastore.Domain.Iterator;
using Gs2.Gs2Datastore.Model.Cache;
using Gs2.Gs2Datastore.Request;
using Gs2.Gs2Datastore.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Datastore.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DatastoreRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string UploadUrl { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public long? ContentLength { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DatastoreRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareUploadFuture(
            PrepareUploadRequest request
        ) => PrepareUploadAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareUploadAsync(
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareUploadAsync(
        #endif
            PrepareUploadRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.PrepareUploadAsync(request)
            );
            var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.Name
            );
            domain.UploadUrl = result?.UploadUrl;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadFuture(
            PrepareDownloadRequest request
        ) => PrepareDownloadAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadAsync(
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadAsync(
        #endif
            PrepareDownloadRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.PrepareDownloadAsync(request)
            );
            var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.Name
            );
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadByGenerationFuture(
            PrepareDownloadByGenerationRequest request
        ) => PrepareDownloadByGenerationAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadByGenerationAsync(
        #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain> PrepareDownloadByGenerationAsync(
        #endif
            PrepareDownloadByGenerationRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.PrepareDownloadByGenerationAsync(request)
            );
            var domain = new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.Name
            );
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

            return domain;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Datastore.Model.DataObject> DataObjects(
            string status = null
        )
        {
            return new DescribeDataObjectsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                status
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Datastore.Model.DataObject> DataObjectsAsync(
        #else
        public DescribeDataObjectsIterator DataObjectsAsync(
        #endif
            string status = null
        )
        {
            return new DescribeDataObjectsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                status
            );
        }

        public ulong SubscribeDataObjects(
            Action<Gs2.Gs2Datastore.Model.DataObject[]> callback,
            string status = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Datastore.Model.DataObject>(
                (null as Gs2.Gs2Datastore.Model.DataObject).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                items => callback.Invoke(items
                    .Where(item => status == null || item.Status == status)
                    .ToArray()),
                () =>
                {
        #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        #else
                    async Task Impl() {
        #endif
                        try {
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await DataObjectsAsync(
                                status
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDataObjectsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeDataObjectsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Datastore.Model.DataObject[]> callback,
            string status = null
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

        public void UnsubscribeDataObjects(
            ulong callbackId,
            string status = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Datastore.Model.DataObject>(
                (null as Gs2.Gs2Datastore.Model.DataObject).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateDataObjects(
            string status = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Datastore.Model.DataObject>(
                (null as Gs2.Gs2Datastore.Model.DataObject).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain DataObject(
            string dataObjectName
        ) {
            return new Gs2.Gs2Datastore.Domain.Model.DataObjectAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                dataObjectName
            );
        }

    }
}

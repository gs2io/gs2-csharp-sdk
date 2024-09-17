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

    public partial class DataObjectDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DatastoreRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string DataObjectName { get; } = null!;
        public string UploadUrl { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public long? ContentLength { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public DataObjectDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string dataObjectName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DatastoreRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.DataObjectName = dataObjectName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistories(
            string timeOffsetToken = null
        )
        {
            return new DescribeDataObjectHistoriesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.DataObjectName,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Datastore.Model.DataObjectHistory> DataObjectHistoriesAsync(
            #else
        public DescribeDataObjectHistoriesByUserIdIterator DataObjectHistoriesAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeDataObjectHistoriesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.DataObjectName,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeDataObjectHistories(
            Action<Gs2.Gs2Datastore.Model.DataObjectHistory[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                (null as Gs2.Gs2Datastore.Model.DataObjectHistory).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.DataObjectName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDataObjectHistoriesWithInitialCallAsync(
            Action<Gs2.Gs2Datastore.Model.DataObjectHistory[]> callback
        )
        {
            var items = await DataObjectHistoriesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeDataObjectHistories(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDataObjectHistories(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                (null as Gs2.Gs2Datastore.Model.DataObjectHistory).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.DataObjectName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain DataObjectHistory(
            string generation
        ) {
            return new Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.DataObjectName,
                generation
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
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithDataObjectName(this.DataObjectName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.UpdateDataObjectByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> UpdateAsync(
            #endif
            UpdateDataObjectByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithDataObjectName(this.DataObjectName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.UpdateDataObjectByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareReUploadFuture(
            PrepareReUploadByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithDataObjectName(this.DataObjectName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PrepareReUploadByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.UploadUrl = result?.UploadUrl;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareReUploadAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareReUploadAsync(
            #endif
            PrepareReUploadByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithDataObjectName(this.DataObjectName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PrepareReUploadByUserIdAsync(request)
            );
            var domain = this;
            domain.UploadUrl = result?.UploadUrl;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DoneUploadFuture(
            DoneUploadByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithDataObjectName(this.DataObjectName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DoneUploadByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DoneUploadAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DoneUploadAsync(
            #endif
            DoneUploadByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithDataObjectName(this.DataObjectName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.DoneUploadByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DeleteFuture(
            DeleteDataObjectByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteDataObjectByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> DeleteAsync(
            #endif
            DeleteDataObjectByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteDataObjectByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndDataObjectNameFuture(
            PrepareDownloadByUserIdAndDataObjectNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PrepareDownloadByUserIdAndDataObjectNameFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.FileUrl = result?.FileUrl;
                domain.ContentLength = result?.ContentLength;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndDataObjectNameAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndDataObjectNameAsync(
            #endif
            PrepareDownloadByUserIdAndDataObjectNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PrepareDownloadByUserIdAndDataObjectNameAsync(request)
            );
            var domain = this;
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndDataObjectNameAndGenerationFuture(
            PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDataObjectName(this.DataObjectName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PrepareDownloadByUserIdAndDataObjectNameAndGenerationFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                domain.FileUrl = result?.FileUrl;
                domain.ContentLength = result?.ContentLength;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndDataObjectNameAndGenerationAsync(
            #else
        public async Task<Gs2.Gs2Datastore.Domain.Model.DataObjectDomain> PrepareDownloadByUserIdAndDataObjectNameAndGenerationAsync(
            #endif
            PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDataObjectName(this.DataObjectName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PrepareDownloadByUserIdAndDataObjectNameAndGenerationAsync(request)
            );
            var domain = this;
            domain.FileUrl = result?.FileUrl;
            domain.ContentLength = result?.ContentLength;

            return domain;
        }
        #endif

    }

    public partial class DataObjectDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Model.DataObject> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Model.DataObject> self)
            {
                var (value, find) = (null as Gs2.Gs2Datastore.Model.DataObject).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.DataObjectName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                self.OnComplete(null);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Model.DataObject>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Datastore.Model.DataObject> ModelAsync()
            #else
        public async Task<Gs2.Gs2Datastore.Model.DataObject> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Datastore.Model.DataObject).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.DataObjectName
            );
            if (find) {
                return value;
            }
            return null;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
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


        public void Invalidate()
        {
            (null as Gs2.Gs2Datastore.Model.DataObject).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.DataObjectName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Datastore.Model.DataObject> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Datastore.Model.DataObject).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Datastore.Model.DataObject).CacheKey(
                    this.DataObjectName
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Datastore.Model.DataObject>(
                (null as Gs2.Gs2Datastore.Model.DataObject).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Datastore.Model.DataObject).CacheKey(
                    this.DataObjectName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Datastore.Model.DataObject> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Datastore.Model.DataObject> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Datastore.Model.DataObject> callback)
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

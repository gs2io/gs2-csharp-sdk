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
#pragma warning disable CS0414 // Field is assigned but its value is never used

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2SeasonRating.Domain.Iterator;
using Gs2.Gs2SeasonRating.Model.Cache;
using Gs2.Gs2SeasonRating.Domain.Model;
using Gs2.Gs2SeasonRating.Request;
using Gs2.Gs2SeasonRating.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2SeasonRating.Model;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2SeasonRating.Domain
{

    public class Gs2SeasonRating {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SeasonRatingRestClient _client;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2SeasonRating(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SeasonRatingRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) => CreateNamespaceAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2SeasonRating> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) => DumpUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2SeasonRating> DumpUserDataAsync(
        #else
        public async Task<Gs2SeasonRating> DumpUserDataAsync(
        #endif
            DumpUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.DumpUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2SeasonRating> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) => CheckDumpUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2SeasonRating> CheckDumpUserDataAsync(
        #else
        public async Task<Gs2SeasonRating> CheckDumpUserDataAsync(
        #endif
            CheckDumpUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CheckDumpUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2SeasonRating> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) => CleanUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2SeasonRating> CleanUserDataAsync(
        #else
        public async Task<Gs2SeasonRating> CleanUserDataAsync(
        #endif
            CleanUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CleanUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2SeasonRating> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) => CheckCleanUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2SeasonRating> CheckCleanUserDataAsync(
        #else
        public async Task<Gs2SeasonRating> CheckCleanUserDataAsync(
        #endif
            CheckCleanUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CheckCleanUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2SeasonRating> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) => PrepareImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2SeasonRating> PrepareImportUserDataAsync(
        #else
        public async Task<Gs2SeasonRating> PrepareImportUserDataAsync(
        #endif
            PrepareImportUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.PrepareImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2SeasonRating> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) => ImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2SeasonRating> ImportUserDataAsync(
        #else
        public async Task<Gs2SeasonRating> ImportUserDataAsync(
        #endif
            ImportUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.ImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2SeasonRating> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) => CheckImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2SeasonRating> CheckImportUserDataAsync(
        #else
        public async Task<Gs2SeasonRating> CheckImportUserDataAsync(
        #endif
            CheckImportUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CheckImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2SeasonRating.Model.Namespace> Namespaces(
            string namePrefix = null
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2SeasonRating.Model.Namespace> NamespacesAsync(
        #else
        public DescribeNamespacesIterator NamespacesAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client,
                namePrefix
            );
        }

        public ulong SubscribeNamespaces(
            Action<Gs2.Gs2SeasonRating.Model.Namespace[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2SeasonRating.Model.Namespace>(
                (null as Gs2.Gs2SeasonRating.Model.Namespace).CacheParentKey(
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => namePrefix == null || item.Name.StartsWith(namePrefix))
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
                            callback.Invoke(await NamespacesAsync(
                                namePrefix
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
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeNamespacesWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2SeasonRating.Model.Namespace[]> callback,
            string namePrefix = null
        )
        {
            var items = await NamespacesAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeNamespaces(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeNamespaces(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2SeasonRating.Model.Namespace>(
                (null as Gs2.Gs2SeasonRating.Model.Namespace).CacheParentKey(
                    null
                ),
                callbackId
            );
        }

        public void InvalidateNamespaces(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2SeasonRating.Model.Namespace>(
                (null as Gs2.Gs2SeasonRating.Model.Namespace).CacheParentKey(
                    null
                )
            );
        }

        public Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

        public void UpdateCacheFromStampSheet(
                string transactionId,
                int? timeOffset,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromStampTask(
                string taskId,
                int? timeOffset,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromJobResult(
                string method,
                int? timeOffset,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
        }

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
        }
    }
}

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

    public partial class DataObjectHistoryAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2DatastoreRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _dataObjectName;
        private readonly string _generation;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string DataObjectName => _dataObjectName;
        public string Generation => _generation;

        public DataObjectHistoryAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string dataObjectName,
            string generation
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
            this._generation = generation;
            this._parentKey = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.DataObjectName,
                "DataObjectHistory"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Datastore.Model.DataObjectHistory> GetFuture(
            GetDataObjectHistoryRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Model.DataObjectHistory> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName)
                    .WithGeneration(this.Generation);
                var future = this._client.GetDataObjectHistoryFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                            request.Generation.ToString()
                        );
                        _cache.Put<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "dataObjectHistory")
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
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithDataObjectName(this.DataObjectName)
                    .WithGeneration(this.Generation);
                GetDataObjectHistoryResult result = null;
                try {
                    result = await this._client.GetDataObjectHistoryAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                        request.Generation.ToString()
                        );
                    _cache.Put<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "dataObjectHistory")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.DataObjectName,
                            "DataObjectHistory"
                        );
                        var key = Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                            resultModel.Item.Generation.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Model.DataObjectHistory>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Datastore.Model.DataObjectHistory> GetAsync(
            GetDataObjectHistoryRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName)
                .WithGeneration(this.Generation);
            var future = this._client.GetDataObjectHistoryFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                        request.Generation.ToString()
                    );
                    _cache.Put<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "dataObjectHistory")
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
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithDataObjectName(this.DataObjectName)
                .WithGeneration(this.Generation);
            GetDataObjectHistoryResult result = null;
            try {
                result = await this._client.GetDataObjectHistoryAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                    request.Generation.ToString()
                    );
                _cache.Put<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "dataObjectHistory")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Datastore.Domain.Model.DataObjectDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.DataObjectName,
                        "DataObjectHistory"
                    );
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                        resultModel.Item.Generation.ToString()
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

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string dataObjectName,
            string generation,
            string childType
        )
        {
            return string.Join(
                ":",
                "datastore",
                namespaceName ?? "null",
                userId ?? "null",
                dataObjectName ?? "null",
                generation ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string generation
        )
        {
            return string.Join(
                ":",
                generation ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Datastore.Model.DataObjectHistory> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Datastore.Model.DataObjectHistory> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                        this.Generation?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetDataObjectHistoryRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                                    this.Generation?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "dataObjectHistory")
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
                    (value, _) = _cache.Get<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                        _parentKey,
                        Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                            this.Generation?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Datastore.Model.DataObjectHistory>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Datastore.Model.DataObjectHistory> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                    _parentKey,
                    Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                        this.Generation?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetDataObjectHistoryRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                                    this.Generation?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "dataObjectHistory")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                        _parentKey,
                        Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                            this.Generation?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Datastore.Model.DataObjectHistory> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Datastore.Model.DataObjectHistory> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Datastore.Model.DataObjectHistory> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Datastore.Model.DataObjectHistory> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Datastore.Model.DataObjectHistory> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                    this.Generation.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Datastore.Model.DataObjectHistory>(
                _parentKey,
                Gs2.Gs2Datastore.Domain.Model.DataObjectHistoryDomain.CreateCacheKey(
                    this.Generation.ToString()
                ),
                callbackId
            );
        }

    }
}

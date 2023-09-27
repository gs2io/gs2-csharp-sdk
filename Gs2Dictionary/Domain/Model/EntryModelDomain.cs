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
using Gs2.Gs2Dictionary.Domain.Iterator;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Result;
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

namespace Gs2.Gs2Dictionary.Domain.Model
{

    public partial class EntryModelDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2DictionaryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _entryName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string EntryName => _entryName;

        public EntryModelDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string entryName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2DictionaryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._entryName = entryName;
            this._parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "EntryModel"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string entryName,
            string childType
        )
        {
            return string.Join(
                ":",
                "dictionary",
                namespaceName ?? "null",
                entryName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string entryName
        )
        {
            return string.Join(
                ":",
                entryName ?? "null"
            );
        }

    }

    public partial class EntryModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Dictionary.Model.EntryModel> GetFuture(
            GetEntryModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Model.EntryModel> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEntryName(this.EntryName);
                var future = this._client.GetEntryModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                            request.EntryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Dictionary.Model.EntryModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "entryModel")
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
                    .WithEntryName(this.EntryName);
                GetEntryModelResult result = null;
                try {
                    result = await this._client.GetEntryModelAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                        request.EntryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Dictionary.Model.EntryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "entryModel")
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
                        var parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "EntryModel"
                        );
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Model.EntryModel>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Dictionary.Model.EntryModel> GetAsync(
            GetEntryModelRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEntryName(this.EntryName);
            var future = this._client.GetEntryModelFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                        request.EntryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Dictionary.Model.EntryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "entryModel")
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
                .WithEntryName(this.EntryName);
            GetEntryModelResult result = null;
            try {
                result = await this._client.GetEntryModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                    request.EntryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Dictionary.Model.EntryModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "entryModel")
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
                    var parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "EntryModel"
                    );
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
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

    }

    public partial class EntryModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Model.EntryModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Model.EntryModel> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Dictionary.Model.EntryModel>(
                    _parentKey,
                    Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                        this.EntryName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetEntryModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                                    this.EntryName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Dictionary.Model.EntryModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "entryModel")
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
                    (value, _) = _cache.Get<Gs2.Gs2Dictionary.Model.EntryModel>(
                        _parentKey,
                        Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                            this.EntryName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Model.EntryModel>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Dictionary.Model.EntryModel> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Dictionary.Model.EntryModel>(
                    _parentKey,
                    Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                        this.EntryName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetEntryModelRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                                    this.EntryName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Dictionary.Model.EntryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "entryModel")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Dictionary.Model.EntryModel>(
                        _parentKey,
                        Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                            this.EntryName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Model.EntryModel> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Dictionary.Model.EntryModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Dictionary.Model.EntryModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Dictionary.Model.EntryModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Dictionary.Model.EntryModel> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                    this.EntryName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Dictionary.Model.EntryModel>(
                _parentKey,
                Gs2.Gs2Dictionary.Domain.Model.EntryModelDomain.CreateCacheKey(
                    this.EntryName.ToString()
                ),
                callbackId
            );
        }

    }
}

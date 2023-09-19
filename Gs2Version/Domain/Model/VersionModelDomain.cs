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
using Gs2.Gs2Version.Domain.Iterator;
using Gs2.Gs2Version.Request;
using Gs2.Gs2Version.Result;
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

namespace Gs2.Gs2Version.Domain.Model
{

    public partial class VersionModelDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2VersionRestClient _client;
        private readonly string _namespaceName;
        private readonly string _versionName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string VersionName => _versionName;

        public VersionModelDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string versionName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2VersionRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._versionName = versionName;
            this._parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "VersionModel"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string versionName,
            string childType
        )
        {
            return string.Join(
                ":",
                "version",
                namespaceName ?? "null",
                versionName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string versionName
        )
        {
            return string.Join(
                ":",
                versionName ?? "null"
            );
        }

    }

    public partial class VersionModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Version.Model.VersionModel> GetFuture(
            GetVersionModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Version.Model.VersionModel> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithVersionName(this.VersionName);
                var future = this._client.GetVersionModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                            request.VersionName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Version.Model.VersionModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "versionModel")
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
                    .WithVersionName(this.VersionName);
                GetVersionModelResult result = null;
                try {
                    result = await this._client.GetVersionModelAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                        request.VersionName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Version.Model.VersionModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "versionModel")
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
                        var parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "VersionModel"
                        );
                        var key = Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Version.Model.VersionModel>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Version.Model.VersionModel> GetAsync(
            GetVersionModelRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithVersionName(this.VersionName);
            var future = this._client.GetVersionModelFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                        request.VersionName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Version.Model.VersionModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "versionModel")
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
                .WithVersionName(this.VersionName);
            GetVersionModelResult result = null;
            try {
                result = await this._client.GetVersionModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                    request.VersionName.ToString()
                    );
                _cache.Put<Gs2.Gs2Version.Model.VersionModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "versionModel")
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
                    var parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "VersionModel"
                    );
                    var key = Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
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

    public partial class VersionModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Version.Model.VersionModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Version.Model.VersionModel> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Version.Model.VersionModel>(
                    _parentKey,
                    Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                        this.VersionName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetVersionModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                                    this.VersionName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Version.Model.VersionModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "versionModel")
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
                    (value, _) = _cache.Get<Gs2.Gs2Version.Model.VersionModel>(
                        _parentKey,
                        Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                            this.VersionName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Version.Model.VersionModel>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Version.Model.VersionModel> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Version.Model.VersionModel>(
                    _parentKey,
                    Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                        this.VersionName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetVersionModelRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                                    this.VersionName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Version.Model.VersionModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "versionModel")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Version.Model.VersionModel>(
                        _parentKey,
                        Gs2.Gs2Version.Domain.Model.VersionModelDomain.CreateCacheKey(
                            this.VersionName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Version.Model.VersionModel> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Version.Model.VersionModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Version.Model.VersionModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Version.Model.VersionModel> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

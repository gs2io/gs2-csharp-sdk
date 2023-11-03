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

    public partial class VersionModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2VersionRestClient _client;
        private readonly string _namespaceName;
        private readonly string _versionName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string VersionName => _versionName;

        public VersionModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string versionName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2VersionRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._versionName = versionName;
            this._parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "VersionModelMaster"
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

    public partial class VersionModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Version.Model.VersionModelMaster> GetFuture(
            GetVersionModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Version.Model.VersionModelMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithVersionName(this.VersionName);
                var future = this._client.GetVersionModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                            request.VersionName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Version.Model.VersionModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "versionModelMaster")
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

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "VersionModelMaster"
                        );
                        var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Version.Model.VersionModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Version.Model.VersionModelMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Version.Model.VersionModelMaster> GetAsync(
            #endif
            GetVersionModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithVersionName(this.VersionName);
            GetVersionModelMasterResult result = null;
            try {
                result = await this._client.GetVersionModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                    request.VersionName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Version.Model.VersionModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "versionModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "VersionModelMaster"
                    );
                    var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> UpdateFuture(
            UpdateVersionModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithVersionName(this.VersionName);
                var future = this._client.UpdateVersionModelMasterFuture(
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "VersionModelMaster"
                        );
                        var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> UpdateAsync(
            #endif
            UpdateVersionModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithVersionName(this.VersionName);
            UpdateVersionModelMasterResult result = null;
                result = await this._client.UpdateVersionModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "VersionModelMaster"
                    );
                    var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
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
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> Update(
            UpdateVersionModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> DeleteFuture(
            DeleteVersionModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithVersionName(this.VersionName);
                var future = this._client.DeleteVersionModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                            request.VersionName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Version.Model.VersionModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "versionModelMaster")
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

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "VersionModelMaster"
                        );
                        var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Version.Model.VersionModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> DeleteAsync(
            #endif
            DeleteVersionModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithVersionName(this.VersionName);
            DeleteVersionModelMasterResult result = null;
            try {
                result = await this._client.DeleteVersionModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                    request.VersionName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Version.Model.VersionModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "versionModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Version.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "VersionModelMaster"
                    );
                    var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Version.Model.VersionModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain> Delete(
            DeleteVersionModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class VersionModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Version.Model.VersionModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Version.Model.VersionModelMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Version.Model.VersionModelMaster>(
                    _parentKey,
                    Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                        this.VersionName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetVersionModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                                    this.VersionName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Version.Model.VersionModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "versionModelMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Version.Model.VersionModelMaster>(
                        _parentKey,
                        Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                            this.VersionName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Version.Model.VersionModelMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Version.Model.VersionModelMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Version.Model.VersionModelMaster> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Version.Model.VersionModelMaster>(
                    _parentKey,
                    Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                        this.VersionName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetVersionModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                                    this.VersionName?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Version.Model.VersionModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "versionModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Version.Model.VersionModelMaster>(
                        _parentKey,
                        Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                            this.VersionName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Version.Model.VersionModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Version.Model.VersionModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Version.Model.VersionModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Version.Model.VersionModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                    this.VersionName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Version.Model.VersionModelMaster>(
                _parentKey,
                Gs2.Gs2Version.Domain.Model.VersionModelMasterDomain.CreateCacheKey(
                    this.VersionName.ToString()
                ),
                callbackId
            );
        }

    }
}

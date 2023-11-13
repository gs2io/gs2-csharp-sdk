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

    public partial class AcceptVersionAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2VersionRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _versionName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string VersionName => _versionName;

        public AcceptVersionAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string versionName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2VersionRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._versionName = versionName;
            this._parentKey = Gs2.Gs2Version.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "AcceptVersion"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> AcceptFuture(
            AcceptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithVersionName(this.VersionName);
                var future = this._client.AcceptFuture(
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
                        var parentKey = Gs2.Gs2Version.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "AcceptVersion"
                        );
                        var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                            resultModel.Item.VersionName.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> AcceptAsync(
            #else
        public async Task<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> AcceptAsync(
            #endif
            AcceptRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithVersionName(this.VersionName);
            AcceptResult result = null;
                result = await this._client.AcceptAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Version.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "AcceptVersion"
                    );
                    var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                        resultModel.Item.VersionName.ToString()
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
        [Obsolete("The name has been changed to AcceptFuture.")]
        public IFuture<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> Accept(
            AcceptRequest request
        ) {
            return AcceptFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Version.Model.AcceptVersion> GetFuture(
            GetAcceptVersionRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Version.Model.AcceptVersion> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithVersionName(this.VersionName);
                var future = this._client.GetAcceptVersionFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                            request.VersionName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Version.Model.AcceptVersion>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "acceptVersion")
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
                        var parentKey = Gs2.Gs2Version.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "AcceptVersion"
                        );
                        var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                            resultModel.Item.VersionName.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Version.Model.AcceptVersion>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Version.Model.AcceptVersion> GetAsync(
            #else
        private async Task<Gs2.Gs2Version.Model.AcceptVersion> GetAsync(
            #endif
            GetAcceptVersionRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithVersionName(this.VersionName);
            GetAcceptVersionResult result = null;
            try {
                result = await this._client.GetAcceptVersionAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                    request.VersionName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Version.Model.AcceptVersion>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "acceptVersion")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Version.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "AcceptVersion"
                    );
                    var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                        resultModel.Item.VersionName.ToString()
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
        public IFuture<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> DeleteFuture(
            DeleteAcceptVersionRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithVersionName(this.VersionName);
                var future = this._client.DeleteAcceptVersionFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                            request.VersionName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Version.Model.AcceptVersion>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "acceptVersion")
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
                        var parentKey = Gs2.Gs2Version.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "AcceptVersion"
                        );
                        var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                            resultModel.Item.VersionName.ToString()
                        );
                        cache.Delete<Gs2.Gs2Version.Model.AcceptVersion>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> DeleteAsync(
            #endif
            DeleteAcceptVersionRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithVersionName(this.VersionName);
            DeleteAcceptVersionResult result = null;
            try {
                result = await this._client.DeleteAcceptVersionAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                    request.VersionName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Version.Model.AcceptVersion>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "acceptVersion")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Version.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "AcceptVersion"
                    );
                    var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                        resultModel.Item.VersionName.ToString()
                    );
                    cache.Delete<Gs2.Gs2Version.Model.AcceptVersion>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Version.Domain.Model.AcceptVersionAccessTokenDomain> Delete(
            DeleteAcceptVersionRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string versionName,
            string childType
        )
        {
            return string.Join(
                ":",
                "version",
                namespaceName ?? "null",
                userId ?? "null",
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Version.Model.AcceptVersion> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Version.Model.AcceptVersion> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Version.Model.AcceptVersion>(
                    _parentKey,
                    Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                        this.VersionName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetAcceptVersionRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                                    this.VersionName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Version.Model.AcceptVersion>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "acceptVersion")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Version.Model.AcceptVersion>(
                        _parentKey,
                        Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                            this.VersionName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Version.Model.AcceptVersion>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Version.Model.AcceptVersion> ModelAsync()
            #else
        public async Task<Gs2.Gs2Version.Model.AcceptVersion> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Version.Model.AcceptVersion>(
                _parentKey,
                Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                    this.VersionName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Version.Model.AcceptVersion>(
                    _parentKey,
                    Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                        this.VersionName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetAcceptVersionRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                                    this.VersionName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Version.Model.AcceptVersion>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "acceptVersion")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Version.Model.AcceptVersion>(
                        _parentKey,
                        Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                            this.VersionName?.ToString()
                        )
                    );
                }
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Version.Model.AcceptVersion> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Version.Model.AcceptVersion> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Version.Model.AcceptVersion> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Version.Model.AcceptVersion> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                    this.VersionName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Version.Model.AcceptVersion>(
                _parentKey,
                Gs2.Gs2Version.Domain.Model.AcceptVersionDomain.CreateCacheKey(
                    this.VersionName.ToString()
                ),
                callbackId
            );
        }

    }
}

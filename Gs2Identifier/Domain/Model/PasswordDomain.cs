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
using Gs2.Gs2Identifier.Domain.Iterator;
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
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

namespace Gs2.Gs2Identifier.Domain.Model
{

    public partial class PasswordDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2IdentifierRestClient _client;
        private readonly string _userName;

        private readonly String _parentKey;
        public string UserName => _userName;

        public PasswordDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string userName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2IdentifierRestClient(
                session
            );
            this._userName = userName;
            this._parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                this.UserName,
                "Password"
            );
        }

        public static string CreateCacheParentKey(
            string userName,
            string childType
        )
        {
            return string.Join(
                ":",
                "identifier",
                userName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

    }

    public partial class PasswordDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> CreateFuture(
            CreatePasswordRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithUserName(this.UserName);
                var future = this._client.CreatePasswordFuture(
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
                    .WithUserName(this.UserName);
                CreatePasswordResult result = null;
                    result = await this._client.CreatePasswordAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.UserName,
                            "Password"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.PasswordDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> CreateAsync(
            CreatePasswordRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithUserName(this.UserName);
            var future = this._client.CreatePasswordFuture(
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
                .WithUserName(this.UserName);
            CreatePasswordResult result = null;
                result = await this._client.CreatePasswordAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.UserName,
                        "Password"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> CreateAsync(
            CreatePasswordRequest request
        ) {
            var future = CreateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> Create(
            CreatePasswordRequest request
        ) {
            return CreateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Identifier.Model.Password> GetFuture(
            GetPasswordRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.Password> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithUserName(this.UserName);
                var future = this._client.GetPasswordFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                        );
                        _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "password")
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
                    .WithUserName(this.UserName);
                GetPasswordResult result = null;
                try {
                    result = await this._client.GetPasswordAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                        );
                    _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "password")
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
                        var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.UserName,
                            "Password"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.Password>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Identifier.Model.Password> GetAsync(
            GetPasswordRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithUserName(this.UserName);
            var future = this._client.GetPasswordFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                    );
                    _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "password")
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
                .WithUserName(this.UserName);
            GetPasswordResult result = null;
            try {
                result = await this._client.GetPasswordAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                    );
                _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "password")
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
                    var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.UserName,
                        "Password"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> DeleteFuture(
            DeletePasswordRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithUserName(this.UserName);
                var future = this._client.DeletePasswordFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                        );
                        _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "password")
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
                    .WithUserName(this.UserName);
                DeletePasswordResult result = null;
                try {
                    result = await this._client.DeletePasswordAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                        );
                    _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "password")
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
                        var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.UserName,
                            "Password"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                        );
                        cache.Delete<Gs2.Gs2Identifier.Model.Password>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.PasswordDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> DeleteAsync(
            DeletePasswordRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithUserName(this.UserName);
            var future = this._client.DeletePasswordFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                    );
                    _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "password")
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
                .WithUserName(this.UserName);
            DeletePasswordResult result = null;
            try {
                result = await this._client.DeletePasswordAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                    );
                _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "password")
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
                    var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.UserName,
                        "Password"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                    );
                    cache.Delete<Gs2.Gs2Identifier.Model.Password>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> DeleteAsync(
            DeletePasswordRequest request
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
        public IFuture<Gs2.Gs2Identifier.Domain.Model.PasswordDomain> Delete(
            DeletePasswordRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class PasswordDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.Password> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.Password> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Identifier.Model.Password>(
                    _parentKey,
                    Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetPasswordRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                                );
                            _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "password")
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
                    (value, _) = _cache.Get<Gs2.Gs2Identifier.Model.Password>(
                        _parentKey,
                        Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.Password>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Model.Password> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Identifier.Model.Password>(
                    _parentKey,
                    Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetPasswordRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                                );
                    _cache.Put<Gs2.Gs2Identifier.Model.Password>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "password")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Identifier.Model.Password>(
                        _parentKey,
                        Gs2.Gs2Identifier.Domain.Model.PasswordDomain.CreateCacheKey(
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Model.Password> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Identifier.Model.Password> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Identifier.Model.Password> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Identifier.Model.Password> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

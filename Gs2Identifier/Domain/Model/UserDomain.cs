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

    public partial class UserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2IdentifierRestClient _client;
        private readonly string _userName;

        private readonly String _parentKey;
        public string ClientSecret { get; set; }
        public string NextPageToken { get; set; }
        public string UserName => _userName;

        public UserDomain(
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
            this._parentKey = "identifier:User";
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Identifier.Model.Identifier> Identifiers(
        )
        {
            return new DescribeIdentifiersIterator(
                this._cache,
                this._client,
                this.UserName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.Identifier> IdentifiersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.Identifier> Identifiers(
            #endif
        #else
        public DescribeIdentifiersIterator Identifiers(
        #endif
        )
        {
            return new DescribeIdentifiersIterator(
                this._cache,
                this._client,
                this.UserName
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

        public ulong SubscribeIdentifiers(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Identifier.Model.Identifier>(
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.UserName,
                    "Identifier"
                ),
                callback
            );
        }

        public void UnsubscribeIdentifiers(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.Identifier>(
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.UserName,
                    "Identifier"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.IdentifierDomain Identifier(
            string clientId
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.UserName,
                clientId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Identifier.Model.Password> Passwords(
        )
        {
            return new DescribePasswordsIterator(
                this._cache,
                this._client,
                this.UserName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.Password> PasswordsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.Password> Passwords(
            #endif
        #else
        public DescribePasswordsIterator Passwords(
        #endif
        )
        {
            return new DescribePasswordsIterator(
                this._cache,
                this._client,
                this.UserName
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

        public ulong SubscribePasswords(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Identifier.Model.Password>(
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.UserName,
                    "Password"
                ),
                callback
            );
        }

        public void UnsubscribePasswords(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.Password>(
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.UserName,
                    "Password"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.PasswordDomain Password(
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.PasswordDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.UserName
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain AttachSecurityPolicy(
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.UserName
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
            string userName
        )
        {
            return string.Join(
                ":",
                userName ?? "null"
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> UpdateFuture(
            UpdateUserRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithUserName(this.UserName);
                var future = this._client.UpdateUserFuture(
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
                UpdateUserResult result = null;
                    result = await this._client.UpdateUserAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "User"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.UserDomain> UpdateAsync(
            UpdateUserRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithUserName(this.UserName);
            var future = this._client.UpdateUserFuture(
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
            UpdateUserResult result = null;
                result = await this._client.UpdateUserAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "User"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.UserDomain> UpdateAsync(
            UpdateUserRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> Update(
            UpdateUserRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Identifier.Model.User> GetFuture(
            GetUserRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.User> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithUserName(this.UserName);
                var future = this._client.GetUserFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                            request.UserName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Identifier.Model.User>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "user")
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
                GetUserResult result = null;
                try {
                    result = await this._client.GetUserAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        request.UserName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Identifier.Model.User>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "user")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "User"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.User>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Identifier.Model.User> GetAsync(
            GetUserRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithUserName(this.UserName);
            var future = this._client.GetUserFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        request.UserName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Identifier.Model.User>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "user")
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
            GetUserResult result = null;
            try {
                result = await this._client.GetUserAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                    request.UserName.ToString()
                    );
                _cache.Put<Gs2.Gs2Identifier.Model.User>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "user")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "User"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> DeleteFuture(
            DeleteUserRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithUserName(this.UserName);
                var future = this._client.DeleteUserFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                            request.UserName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Identifier.Model.User>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "user")
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
                DeleteUserResult result = null;
                try {
                    result = await this._client.DeleteUserAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        request.UserName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Identifier.Model.User>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "user")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "User"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Identifier.Model.User>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.UserDomain> DeleteAsync(
            DeleteUserRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithUserName(this.UserName);
            var future = this._client.DeleteUserFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        request.UserName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Identifier.Model.User>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "user")
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
            DeleteUserResult result = null;
            try {
                result = await this._client.DeleteUserAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                    request.UserName.ToString()
                    );
                _cache.Put<Gs2.Gs2Identifier.Model.User>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "user")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "User"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Identifier.Model.User>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.UserDomain> DeleteAsync(
            DeleteUserRequest request
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
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> Delete(
            DeleteUserRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifierFuture(
            CreateIdentifierRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithUserName(this.UserName);
                var future = this._client.CreateIdentifierFuture(
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
                CreateIdentifierResult result = null;
                    result = await this._client.CreateIdentifierAsync(
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
                            "Identifier"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                            resultModel.Item.ClientId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.UserName,
                    result?.Item?.ClientId
                );
                domain.ClientSecret = result?.ClientSecret;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifierAsync(
            CreateIdentifierRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithUserName(this.UserName);
            var future = this._client.CreateIdentifierFuture(
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
            CreateIdentifierResult result = null;
                result = await this._client.CreateIdentifierAsync(
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
                        "Identifier"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                        resultModel.Item.ClientId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.UserName,
                    result?.Item?.ClientId
                );
            domain.ClientSecret = result?.ClientSecret;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifierAsync(
            CreateIdentifierRequest request
        ) {
            var future = CreateIdentifierFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateIdentifierFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifier(
            CreateIdentifierRequest request
        ) {
            return CreateIdentifierFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.User> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.User> self)
            {
                var parentKey = string.Join(
                    ":",
                    "identifier",
                    "User"
                );
                var (value, find) = _cache.Get<Gs2.Gs2Identifier.Model.User>(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        this.UserName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetUserRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                                    this.UserName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Identifier.Model.User>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "user")
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
                    (value, _) = _cache.Get<Gs2.Gs2Identifier.Model.User>(
                        parentKey,
                        Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                            this.UserName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.User>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Model.User> ModelAsync()
        {
            var parentKey = string.Join(
                ":",
                "identifier",
                "User"
            );
            var (value, find) = _cache.Get<Gs2.Gs2Identifier.Model.User>(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        this.UserName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetUserRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                                    this.UserName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Identifier.Model.User>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "user")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Identifier.Model.User>(
                        parentKey,
                        Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                            this.UserName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Model.User> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Identifier.Model.User> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Identifier.Model.User> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Identifier.Model.User> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Identifier.Model.User> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                    this.UserName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Identifier.Model.User>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                    this.UserName.ToString()
                ),
                callbackId
            );
        }

    }
}

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
 *
 * deny overwrite
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
using Gs2.Gs2Account.Domain.Iterator;
using Gs2.Gs2Account.Request;
using Gs2.Gs2Account.Result;
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

namespace Gs2.Gs2Account.Domain.Model
{

    public partial class AccountDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2AccountRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public AccountDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2AccountRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Account"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Account.Model.TakeOver> TakeOvers(
        )
        {
            return new DescribeTakeOversByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Account.Model.TakeOver> TakeOversAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Account.Model.TakeOver> TakeOvers(
            #endif
        #else
        public DescribeTakeOversByUserIdIterator TakeOvers(
        #endif
        )
        {
            return new DescribeTakeOversByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.UserId
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

        public Gs2.Gs2Account.Domain.Model.TakeOverDomain TakeOver(
            int? type
        ) {
            return new Gs2.Gs2Account.Domain.Model.TakeOverDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                type
            );
        }

        public Gs2.Gs2Account.Domain.Model.DataOwnerDomain DataOwner(
        ) {
            return new Gs2.Gs2Account.Domain.Model.DataOwnerDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "account",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
            );
        }

    }

    public partial class AccountDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffsetFuture(
            UpdateTimeOffsetRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.UpdateTimeOffsetFuture(
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
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                UpdateTimeOffsetResult result = null;
                    result = await this._client.UpdateTimeOffsetAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffsetAsync(
            UpdateTimeOffsetRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var future = this._client.UpdateTimeOffsetFuture(
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
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            UpdateTimeOffsetResult result = null;
                result = await this._client.UpdateTimeOffsetAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
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
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffsetAsync(
            UpdateTimeOffsetRequest request
        ) {
            var future = UpdateTimeOffsetFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateTimeOffsetFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffset(
            UpdateTimeOffsetRequest request
        ) {
            return UpdateTimeOffsetFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateBannedFuture(
            UpdateBannedRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.UpdateBannedFuture(
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
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                UpdateBannedResult result = null;
                    result = await this._client.UpdateBannedAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateBannedAsync(
            UpdateBannedRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var future = this._client.UpdateBannedFuture(
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
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            UpdateBannedResult result = null;
                result = await this._client.UpdateBannedAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        this.UserId
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
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateBannedAsync(
            UpdateBannedRequest request
        ) {
            var future = UpdateBannedFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateBannedFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateBanned(
            UpdateBannedRequest request
        ) {
            return UpdateBannedFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Account.Model.Account> GetFuture(
            GetAccountRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Account> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.GetAccountFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            request.UserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Account.Model.Account>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "account")
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
                    .WithUserId(this.UserId);
                GetAccountResult result = null;
                try {
                    result = await this._client.GetAccountAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        request.UserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Account.Model.Account>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "account")
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
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.Account>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Account.Model.Account> GetAsync(
            GetAccountRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var future = this._client.GetAccountFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        request.UserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Account.Model.Account>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "account")
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
                .WithUserId(this.UserId);
            GetAccountResult result = null;
            try {
                result = await this._client.GetAccountAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                    this.UserId
                    );
                _cache.Put<Gs2.Gs2Account.Model.Account>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "account")
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
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        this.UserId
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
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> DeleteFuture(
            DeleteAccountRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.DeleteAccountFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            request.UserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Account.Model.Account>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "account")
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
                    .WithUserId(this.UserId);
                DeleteAccountResult result = null;
                try {
                    result = await this._client.DeleteAccountAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        request.UserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Account.Model.Account>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "account")
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
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Account.Model.Account>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> DeleteAsync(
            DeleteAccountRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var future = this._client.DeleteAccountFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        request.UserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Account.Model.Account>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "account")
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
                .WithUserId(this.UserId);
            DeleteAccountResult result = null;
            try {
                result = await this._client.DeleteAccountAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                    request.UserId.ToString()
                    );
                _cache.Put<Gs2.Gs2Account.Model.Account>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "account")
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
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Account.Model.Account>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> DeleteAsync(
            DeleteAccountRequest request
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
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> Delete(
            DeleteAccountRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> AuthenticationFuture(
            AuthenticationRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.AuthenticationFuture(
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
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                AuthenticationResult result = null;
                    result = await this._client.AuthenticationAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId
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
                domain.Body = result?.Body;
                domain.Signature = result?.Signature;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> AuthenticationAsync(
            AuthenticationRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var future = this._client.AuthenticationFuture(
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
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            AuthenticationResult result = null;
                result = await this._client.AuthenticationAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        this.UserId
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
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> AuthenticationAsync(
            AuthenticationRequest request
        ) {
            var future = AuthenticationFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to AuthenticationFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> Authentication(
            AuthenticationRequest request
        ) {
            return AuthenticationFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> DeleteDataOwnerFuture(
            DeleteDataOwnerByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.DeleteDataOwnerByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Account.Domain.Model.DataOwnerDomain.CreateCacheKey(
                        );
                        _cache.Put<Gs2.Gs2Account.Model.DataOwner>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "dataOwner")
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
                    .WithUserId(this.UserId);
                DeleteDataOwnerByUserIdResult result = null;
                try {
                    result = await this._client.DeleteDataOwnerByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.DataOwnerDomain.CreateCacheKey(
                        );
                    _cache.Put<Gs2.Gs2Account.Model.DataOwner>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "dataOwner")
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
                        var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataOwner"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.DataOwnerDomain.CreateCacheKey(
                        );
                        cache.Delete<Gs2.Gs2Account.Model.DataOwner>(parentKey, key);
                    }
                }
                var domain = new Gs2.Gs2Account.Domain.Model.DataOwnerDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.DataOwnerDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> DeleteDataOwnerAsync(
            DeleteDataOwnerByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var future = this._client.DeleteDataOwnerByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Account.Domain.Model.DataOwnerDomain.CreateCacheKey(
                    );
                    _cache.Put<Gs2.Gs2Account.Model.DataOwner>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "dataOwner")
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
                .WithUserId(this.UserId);
            DeleteDataOwnerByUserIdResult result = null;
            try {
                result = await this._client.DeleteDataOwnerByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Account.Domain.Model.DataOwnerDomain.CreateCacheKey(
                    );
                _cache.Put<Gs2.Gs2Account.Model.DataOwner>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "dataOwner")
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
                    var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DataOwner"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.DataOwnerDomain.CreateCacheKey(
                    );
                    cache.Delete<Gs2.Gs2Account.Model.DataOwner>(parentKey, key);
                }
            }
                var domain = new Gs2.Gs2Account.Domain.Model.DataOwnerDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> DeleteDataOwnerAsync(
            DeleteDataOwnerByUserIdRequest request
        ) {
            var future = DeleteDataOwnerFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteDataOwnerFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> DeleteDataOwner(
            DeleteDataOwnerByUserIdRequest request
        ) {
            return DeleteDataOwnerFuture(request);
        }
        #endif

    }

    public partial class AccountDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Model.Account> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Account> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Account.Model.Account>(
                    _parentKey,
                    Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        this.UserId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetAccountRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                                    this.UserId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Account.Model.Account>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "account")
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
                    (value, _) = _cache.Get<Gs2.Gs2Account.Model.Account>(
                        _parentKey,
                        Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.Account>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Model.Account> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Account.Model.Account>(
                    _parentKey,
                    Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        this.UserId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetAccountRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                                    this.UserId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Account.Model.Account>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "account")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Account.Model.Account>(
                        _parentKey,
                        Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Model.Account> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Account.Model.Account> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Account.Model.Account> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Account.Model.Account> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

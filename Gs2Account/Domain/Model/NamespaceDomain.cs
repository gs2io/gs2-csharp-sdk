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

    public partial class NamespaceDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2AccountRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string Status { get; set; }
        public string Url { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;

        public NamespaceDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2AccountRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._parentKey = "account:Namespace";
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Account.Model.Account> Accounts(
        )
        {
            return new DescribeAccountsIterator(
                this._cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Account.Model.Account> AccountsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Account.Model.Account> Accounts(
            #endif
        #else
        public DescribeAccountsIterator Accounts(
        #endif
        )
        {
            return new DescribeAccountsIterator(
                this._cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeAccounts(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Account.Model.Account>(
                Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Account"
                ),
                callback
            );
        }

        public void UnsubscribeAccounts(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Account.Model.Account>(
                Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Account"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Account.Domain.Model.AccountDomain Account(
            string userId
        ) {
            return new Gs2.Gs2Account.Domain.Model.AccountDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                userId
            );
        }

        public AccountAccessTokenDomain AccessToken(
            AccessToken accessToken
        ) {
            return new AccountAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                accessToken
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "account",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string namespaceName
        )
        {
            return string.Join(
                ":",
                namespaceName ?? "null"
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceStatusFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                GetNamespaceStatusResult result = null;
                try {
                    result = await this._client.GetNamespaceStatusAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetStatusAsync(
            GetNamespaceStatusRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetNamespaceStatusFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceStatusResult result = null;
            try {
                result = await this._client.GetNamespaceStatusAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetStatusAsync(
            GetNamespaceStatusRequest request
        ) {
            var future = GetStatusFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to GetStatusFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> GetStatus(
            GetNamespaceStatusRequest request
        ) {
            return GetStatusFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Account.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Namespace> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                GetNamespaceResult result = null;
                try {
                    result = await this._client.GetNamespaceAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
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
                            "account",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.Namespace>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Account.Model.Namespace> GetAsync(
            GetNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceResult result = null;
            try {
                result = await this._client.GetNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
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
                        "account",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateNamespaceFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                UpdateNamespaceResult result = null;
                    result = await this._client.UpdateNamespaceAsync(
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
                            "account",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.NamespaceDomain> UpdateAsync(
            UpdateNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.UpdateNamespaceFuture(
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
                .WithNamespaceName(this.NamespaceName);
            UpdateNamespaceResult result = null;
                result = await this._client.UpdateNamespaceAsync(
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
                        "account",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Account.Domain.Model.NamespaceDomain> UpdateAsync(
            UpdateNamespaceRequest request
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
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> Update(
            UpdateNamespaceRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DeleteNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                DeleteNamespaceResult result = null;
                try {
                    result = await this._client.DeleteNamespaceAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
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
                            "account",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Account.Model.Namespace>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.NamespaceDomain> DeleteAsync(
            DeleteNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.DeleteNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            DeleteNamespaceResult result = null;
            try {
                result = await this._client.DeleteNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
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
                        "account",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Account.Model.Namespace>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.NamespaceDomain> DeleteAsync(
            DeleteNamespaceRequest request
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
        public IFuture<Gs2.Gs2Account.Domain.Model.NamespaceDomain> Delete(
            DeleteNamespaceRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> CreateAccountFuture(
            CreateAccountRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateAccountFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                CreateAccountResult result = null;
                    result = await this._client.CreateAccountAsync(
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
                var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> CreateAccountAsync(
            CreateAccountRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.CreateAccountFuture(
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
                .WithNamespaceName(this.NamespaceName);
            CreateAccountResult result = null;
                result = await this._client.CreateAccountAsync(
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
                var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
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
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> CreateAccountAsync(
            CreateAccountRequest request
        ) {
            var future = CreateAccountFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateAccountFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> CreateAccount(
            CreateAccountRequest request
        ) {
            return CreateAccountFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverByUserIdentifierFuture(
            DeleteTakeOverByUserIdentifierRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DeleteTakeOverByUserIdentifierFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName);
                DeleteTakeOverByUserIdentifierResult result = null;
                try {
                    result = await this._client.DeleteTakeOverByUserIdentifierAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            resultModel.Item.UserId,
                            "TakeOver"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                            resultModel.Item.Type.ToString()
                        );
                        cache.Delete<Gs2.Gs2Account.Model.TakeOver>(parentKey, key);
                    }
                }
                var domain = new Gs2.Gs2Account.Domain.Model.TakeOverDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverByUserIdentifierAsync(
            DeleteTakeOverByUserIdentifierRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.DeleteTakeOverByUserIdentifierFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName);
            DeleteTakeOverByUserIdentifierResult result = null;
            try {
                result = await this._client.DeleteTakeOverByUserIdentifierAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        resultModel.Item.UserId,
                        "TakeOver"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                        resultModel.Item.Type.ToString()
                    );
                    cache.Delete<Gs2.Gs2Account.Model.TakeOver>(parentKey, key);
                }
            }
                var domain = new Gs2.Gs2Account.Domain.Model.TakeOverDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverByUserIdentifierAsync(
            DeleteTakeOverByUserIdentifierRequest request
        ) {
            var future = DeleteTakeOverByUserIdentifierFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteTakeOverByUserIdentifierFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverByUserIdentifier(
            DeleteTakeOverByUserIdentifierRequest request
        ) {
            return DeleteTakeOverByUserIdentifierFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverFuture(
            DoTakeOverRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DoTakeOverFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                DoTakeOverResult result = null;
                    result = await this._client.DoTakeOverAsync(
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
                var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverAsync(
            DoTakeOverRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.DoTakeOverFuture(
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
                .WithNamespaceName(this.NamespaceName);
            DoTakeOverResult result = null;
                result = await this._client.DoTakeOverAsync(
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
                var domain = new Gs2.Gs2Account.Domain.Model.AccountDomain(
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
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOverAsync(
            DoTakeOverRequest request
        ) {
            var future = DoTakeOverFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DoTakeOverFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> DoTakeOver(
            DoTakeOverRequest request
        ) {
            return DoTakeOverFuture(request);
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Namespace> self)
            {
                var parentKey = string.Join(
                    ":",
                    "account",
                    "Namespace"
                );
                var (value, find) = _cache.Get<Gs2.Gs2Account.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetNamespaceRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "namespace")
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
                    (value, _) = _cache.Get<Gs2.Gs2Account.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.Namespace>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Model.Namespace> ModelAsync()
        {
            var parentKey = string.Join(
                ":",
                "account",
                "Namespace"
            );
            var (value, find) = _cache.Get<Gs2.Gs2Account.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetNamespaceRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Account.Model.Namespace>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "namespace")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Account.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Model.Namespace> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Account.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Account.Model.Namespace> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Account.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callbackId
            );
        }

    }
}

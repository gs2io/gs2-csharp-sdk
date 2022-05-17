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
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "Account"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffsetAsync(
            #else
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffset(
            #endif
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffsetAsync(
        #endif
            UpdateTimeOffsetRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #else
            var result = await this._client.UpdateTimeOffsetAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #endif
            Gs2.Gs2Account.Domain.Model.AccountDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Account.Model.Account> GetAsync(
            #else
        private IFuture<Gs2.Gs2Account.Model.Account> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Account.Model.Account> GetAsync(
        #endif
            GetAccountRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Account> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetAccountFuture(
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
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #else
            var result = await this._client.GetAccountAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.Account>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> DeleteAsync(
        #endif
            DeleteAccountRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteAccountFuture(
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
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "Account"
                );
                var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                    resultModel.Item.UserId.ToString()
                );
                cache.Delete<Gs2.Gs2Account.Model.Account>(parentKey, key);
            }
            #else
            DeleteAccountResult result = null;
            try {
                result = await this._client.DeleteAccountAsync(
                    request
                );
                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
              
                {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        _namespaceName.ToString(),
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Account.Model.Account>(parentKey, key);
                }
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            Gs2.Gs2Account.Domain.Model.AccountDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> AuthenticationAsync(
            #else
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> Authentication(
            #endif
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> AuthenticationAsync(
        #endif
            AuthenticationRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #else
            var result = await this._client.AuthenticationAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #endif
            Gs2.Gs2Account.Domain.Model.AccountDomain domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Account.Model.TakeOver> TakeOvers(
        )
        {
            return new DescribeTakeOversByUserIdIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._userId
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
                this._namespaceName,
                this._userId
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
                this._namespaceName,
                this._userId,
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
                this._namespaceName,
                this._userId
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Model.Account> Model() {
            #else
        public IFuture<Gs2.Gs2Account.Model.Account> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Account.Model.Account> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Account> self)
            {
        #endif
            Gs2.Gs2Account.Model.Account value = _cache.Get<Gs2.Gs2Account.Model.Account>(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                    this.UserId?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetAccountRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "account")
                            {
                                _cache.Delete<Gs2.Gs2Account.Model.Account>(
                                    _parentKey,
                                    Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                                        this.UserId?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "account")
                    {
                    _cache.Delete<Gs2.Gs2Account.Model.Account>(
                            _parentKey,
                            Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                                this.UserId?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Account.Model.Account>(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                    this.UserId?.ToString()
                )
            );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.Account>(Impl);
        #endif
        }

    }
}

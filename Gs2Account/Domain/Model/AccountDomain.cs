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
#pragma warning disable CS0169, CS0168

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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2AccountRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public Gs2.Gs2Account.Model.BanStatus[] BanStatuses { get; set; }
        public string Body { get; set; }
        public string Signature { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public AccountDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2AccountRestClient(
                gs2.RestSession
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
                this._gs2.Cache,
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
        public DescribeTakeOversByUserIdIterator TakeOversAsync(
        #endif
        )
        {
            return new DescribeTakeOversByUserIdIterator(
                this._gs2.Cache,
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

        public ulong SubscribeTakeOvers(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.TakeOver>(
                Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "TakeOver"
                ),
                callback
            );
        }

        public void UnsubscribeTakeOvers(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.TakeOver>(
                Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "TakeOver"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Account.Domain.Model.TakeOverDomain TakeOver(
            int? type
        ) {
            return new Gs2.Gs2Account.Domain.Model.TakeOverDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                type
            );
        }

        public Gs2.Gs2Account.Domain.Model.DataOwnerDomain DataOwner(
        ) {
            return new Gs2.Gs2Account.Domain.Model.DataOwnerDomain(
                this._gs2,
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffsetAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateTimeOffsetAsync(
            #endif
            UpdateTimeOffsetRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            UpdateTimeOffsetResult result = null;
                result = await this._client.UpdateTimeOffsetAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    _gs2.Cache.Put(
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateBannedAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateBannedAsync(
            #endif
            UpdateBannedRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            UpdateBannedResult result = null;
                result = await this._client.UpdateBannedAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to UpdateBannedFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> UpdateBanned(
            UpdateBannedRequest request
        ) {
            return UpdateBannedFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> AddBanFuture(
            AddBanRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.AddBanFuture(
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
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> AddBanAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> AddBanAsync(
            #endif
            AddBanRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            AddBanResult result = null;
                result = await this._client.AddBanAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to AddBanFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> AddBan(
            AddBanRequest request
        ) {
            return AddBanFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> RemoveBanFuture(
            RemoveBanRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.RemoveBanFuture(
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
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> RemoveBanAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> RemoveBanAsync(
            #endif
            RemoveBanRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            RemoveBanResult result = null;
                result = await this._client.RemoveBanAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to RemoveBanFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> RemoveBan(
            RemoveBanRequest request
        ) {
            return RemoveBanFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Account.Model.Account> GetFuture(
            GetAccountRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Account> self)
            {
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
                        this._gs2.Cache.Put<Gs2.Gs2Account.Model.Account>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "account")
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
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Account.Model.Account> GetAsync(
            #else
        private async Task<Gs2.Gs2Account.Model.Account> GetAsync(
            #endif
            GetAccountRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Account.Model.Account>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "account")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        this.UserId
                    );
                    _gs2.Cache.Put(
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
                        this._gs2.Cache.Put<Gs2.Gs2Account.Model.Account>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "account")
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
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Account.Model.Account>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> DeleteAsync(
            #endif
            DeleteAccountRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Account.Model.Account>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "account")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Account.Model.Account>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Account"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId
                        );
                        _gs2.Cache.Put(
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
                domain.BanStatuses = result?.BanStatuses;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountDomain> AuthenticationAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountDomain> AuthenticationAsync(
            #endif
            AuthenticationRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            AuthenticationResult result = null;
                result = await this._client.AuthenticationAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Account"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                        this.UserId
                    );
                    _gs2.Cache.Put(
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
            domain.BanStatuses = result?.BanStatuses;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to AuthenticationFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountDomain> Authentication(
            AuthenticationRequest request
        ) {
            return AuthenticationFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverFuture(
            DeleteTakeOverByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.DeleteTakeOverByUserIdFuture(
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "TakeOver"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                            resultModel.Item.Type.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Account.Model.TakeOver>(parentKey, key);
                    }
                }
                var domain = new Gs2.Gs2Account.Domain.Model.TakeOverDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOverAsync(
            #endif
            DeleteTakeOverByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            DeleteTakeOverByUserIdResult result = null;
            try {
                result = await this._client.DeleteTakeOverByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "TakeOver"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                        resultModel.Item.Type.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Account.Model.TakeOver>(parentKey, key);
                }
            }
                var domain = new Gs2.Gs2Account.Domain.Model.TakeOverDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Type
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteTakeOverFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> DeleteTakeOver(
            DeleteTakeOverByUserIdRequest request
        ) {
            return DeleteTakeOverFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> DeleteDataOwnerFuture(
            DeleteDataOwnerByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> self)
            {
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
                        this._gs2.Cache.Put<Gs2.Gs2Account.Model.DataOwner>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "dataOwner")
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
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DataOwner"
                        );
                        var key = Gs2.Gs2Account.Domain.Model.DataOwnerDomain.CreateCacheKey(
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Account.Model.DataOwner>(parentKey, key);
                    }
                }
                var domain = new Gs2.Gs2Account.Domain.Model.DataOwnerDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.DataOwnerDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> DeleteDataOwnerAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.DataOwnerDomain> DeleteDataOwnerAsync(
            #endif
            DeleteDataOwnerByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Account.Model.DataOwner>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "dataOwner")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DataOwner"
                    );
                    var key = Gs2.Gs2Account.Domain.Model.DataOwnerDomain.CreateCacheKey(
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Account.Model.DataOwner>(parentKey, key);
                }
            }
                var domain = new Gs2.Gs2Account.Domain.Model.DataOwnerDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Account.Model.Account>(
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
                            this._gs2.Cache.Put<Gs2.Gs2Account.Model.Account>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "account")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Account.Model.Account>(
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Model.Account> ModelAsync()
            #else
        public async Task<Gs2.Gs2Account.Model.Account> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Account.Model.Account>(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                    this.UserId?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Account.Model.Account>(
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
                        this._gs2.Cache.Put<Gs2.Gs2Account.Model.Account>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "account")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Account.Model.Account>(
                        _parentKey,
                        Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                            this.UserId?.ToString()
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


        public ulong Subscribe(Action<Gs2.Gs2Account.Model.Account> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                    this.UserId.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Account.Model.Account>(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheKey(
                    this.UserId.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Account.Model.Account> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Account.Model.Account> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Account.Model.Account> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}

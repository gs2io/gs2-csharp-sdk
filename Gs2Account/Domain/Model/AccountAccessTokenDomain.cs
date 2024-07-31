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
using Gs2.Gs2Account.Model.Cache;
using Gs2.Gs2Account.Request;
using Gs2.Gs2Account.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Account.Domain.Model
{

    public partial class AccountAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2AccountRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public Gs2.Gs2Account.Model.BanStatus[] BanStatuses { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string Signature { get; set; } = null!;
        public string AuthorizationUrl { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public AccountAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2AccountRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.AccountAccessTokenDomain> GetAuthorizationUrlFuture(
            GetAuthorizationUrlRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.AccountAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetAuthorizationUrlFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                this.AuthorizationUrl = domain.AuthorizationUrl = result?.AuthorizationUrl;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.AccountAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Account.Domain.Model.AccountAccessTokenDomain> GetAuthorizationUrlAsync(
            #else
        public async Task<Gs2.Gs2Account.Domain.Model.AccountAccessTokenDomain> GetAuthorizationUrlAsync(
            #endif
            GetAuthorizationUrlRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetAuthorizationUrlAsync(request)
            );
            var domain = this;
            this.AuthorizationUrl = domain.AuthorizationUrl = result?.AuthorizationUrl;
            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Account.Model.TakeOver> TakeOvers(
        )
        {
            return new DescribeTakeOversIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Account.Model.TakeOver> TakeOversAsync(
            #else
        public DescribeTakeOversIterator TakeOversAsync(
            #endif
        )
        {
            return new DescribeTakeOversIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeTakeOvers(
            Action<Gs2.Gs2Account.Model.TakeOver[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.TakeOver>(
                (null as Gs2.Gs2Account.Model.TakeOver).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeTakeOversWithInitialCallAsync(
            Action<Gs2.Gs2Account.Model.TakeOver[]> callback
        )
        {
            var items = await TakeOversAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeTakeOvers(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeTakeOvers(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.TakeOver>(
                (null as Gs2.Gs2Account.Model.TakeOver).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain TakeOver(
            int? type
        ) {
            return new Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                type
            );
        }

        public Gs2.Gs2Account.Domain.Model.DataOwnerAccessTokenDomain DataOwner(
        ) {
            return new Gs2.Gs2Account.Domain.Model.DataOwnerAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Account.Model.PlatformId> PlatformIds(
        )
        {
            return new DescribePlatformIdsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Account.Model.PlatformId> PlatformIdsAsync(
            #else
        public DescribePlatformIdsIterator PlatformIdsAsync(
            #endif
        )
        {
            return new DescribePlatformIdsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribePlatformIds(
            Action<Gs2.Gs2Account.Model.PlatformId[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Account.Model.PlatformId>(
                (null as Gs2.Gs2Account.Model.PlatformId).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribePlatformIdsWithInitialCallAsync(
            Action<Gs2.Gs2Account.Model.PlatformId[]> callback
        )
        {
            var items = await PlatformIdsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribePlatformIds(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribePlatformIds(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Account.Model.PlatformId>(
                (null as Gs2.Gs2Account.Model.PlatformId).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Account.Domain.Model.PlatformIdAccessTokenDomain PlatformId(
            int? type,
            string userIdentifier
        ) {
            return new Gs2.Gs2Account.Domain.Model.PlatformIdAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                type,
                userIdentifier
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Model.Account> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.Account> self)
            {
                var (value, find) = (null as Gs2.Gs2Account.Model.Account).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                self.OnComplete(null);
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
            var (value, find) = (null as Gs2.Gs2Account.Model.Account).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId
            );
            if (find) {
                return value;
            }
            return null;
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


        public void Invalidate()
        {
            (null as Gs2.Gs2Account.Model.Account).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Account.Model.Account> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Account.Model.Account).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Account.Model.Account).CacheKey(
                    this.UserId
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Account.Model.Account>(
                (null as Gs2.Gs2Account.Model.Account).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Account.Model.Account).CacheKey(
                    this.UserId
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

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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Friend.Domain.Iterator;
using Gs2.Gs2Friend.Model.Cache;
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
using Gs2.Gs2Friend.Model; /* diff +++ */
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Friend.Domain.Model
{

    public partial class SendFriendRequestAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string TargetUserId { get; } = null!;

        public SendFriendRequestAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string targetUserId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.TargetUserId = targetUserId;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        private IFuture<Gs2.Gs2Friend.Model.FriendRequest> GetFuture(
 diff --- end */
        private IFuture<Gs2.Gs2Friend.Model.SendFriendRequest> GetFuture( /* diff +++ */
            GetSendRequestRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        private async UniTask<Gs2.Gs2Friend.Model.FriendRequest> GetAsync(
 diff --- end */
        private async UniTask<Gs2.Gs2Friend.Model.SendFriendRequest> GetAsync( /* diff +++ */
        #else
/* diff --- start
        private async Task<Gs2.Gs2Friend.Model.FriendRequest> GetAsync(
 diff --- end */
        private async Task<Gs2.Gs2Friend.Model.SendFriendRequest> GetAsync( /* diff +++ */
        #endif
            GetSendRequestRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithTargetUserId(this.TargetUserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetSendRequestAsync(request)
            );
/* diff --- start
            return result?.Item;
 diff --- end */
/* diff +++ start */
            return result?.Item == null ? null : new SendFriendRequest {
                UserId = result.Item.UserId,
                TargetUserId = result.Item.TargetUserId,
            };
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> DeleteFuture(
 diff --- end */
        public IFuture<Gs2.Gs2Friend.Domain.Model.SendFriendRequestAccessTokenDomain> DeleteFuture( /* diff +++ */
            DeleteRequestRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> DeleteAsync(
 diff --- end */
        public async UniTask<Gs2.Gs2Friend.Domain.Model.SendFriendRequestAccessTokenDomain> DeleteAsync( /* diff +++ */
        #else
/* diff --- start
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> DeleteAsync(
 diff --- end */
        public async Task<Gs2.Gs2Friend.Domain.Model.SendFriendRequestAccessTokenDomain> DeleteAsync( /* diff +++ */
        #endif
            DeleteRequestRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithTargetUserId(this.TargetUserId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteRequestAsync(request)
                );
            }
            catch (NotFoundException e) {}
/* diff --- start
            var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain(
 diff --- end */
/* diff +++ start */
            _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.SendFriendRequest>(
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
            _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.SendFriendRequest>(
                (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.TargetUserId,
                    this.AccessToken?.TimeOffset
                )
            );
            var domain = new Gs2.Gs2Friend.Domain.Model.SendFriendRequestAccessTokenDomain(
/* diff +++ end */
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
/* diff --- start
                result?.Item?.TargetUserId
 diff --- end */
                request?.TargetUserId /* diff +++ */
            );
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public IFuture<Gs2.Gs2Friend.Model.FriendRequest> ModelFuture() => ModelAsync().ToGs2Future();
 diff --- end */
        public IFuture<Gs2.Gs2Friend.Model.SendFriendRequest> ModelFuture() => ModelAsync().ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public async UniTask<Gs2.Gs2Friend.Model.FriendRequest> ModelAsync()
 diff --- end */
        public async UniTask<Gs2.Gs2Friend.Model.SendFriendRequest> ModelAsync() /* diff +++ */
        #else
/* diff --- start
        public async Task<Gs2.Gs2Friend.Model.FriendRequest> ModelAsync()
 diff --- end */
        public async Task<Gs2.Gs2Friend.Model.SendFriendRequest> ModelAsync() /* diff +++ */
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.FriendRequest>(
 diff --- end */
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.SendFriendRequest>( /* diff +++ */
                        (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                            this.TargetUserId
                        )
                    ).LockAsync()) {
                if (this.UserId == null) {
                    throw new NullReferenceException();
                }
/* diff --- start
                var (value, find) = this._gs2.Cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
 diff --- end */
                var (value, find) = this._gs2.Cache.Get<Gs2.Gs2Friend.Model.SendFriendRequest>( /* diff +++ */
                    (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.AccessToken?.TimeOffset
                    ),
                    (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                        this.TargetUserId
                    )
                );
                if (find) {
                    return value;
                }
/* diff --- start
                return await (null as Gs2.Gs2Friend.Model.FriendRequest).FetchAsync(
 diff --- end */
                return await (null as Gs2.Gs2Friend.Model.SendFriendRequest).FetchAsync( /* diff +++ */
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.TargetUserId,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetSendRequestRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Friend.Model.FriendRequest> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Friend.Model.SendFriendRequest> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Friend.Model.FriendRequest> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Friend.Model.SendFriendRequest> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Friend.Model.FriendRequest> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Friend.Model.SendFriendRequest> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
/* diff --- start
            (null as Gs2.Gs2Friend.Model.FriendRequest).DeleteCache(
 diff --- end */
            (null as Gs2.Gs2Friend.Model.SendFriendRequest).DeleteCache( /* diff +++ */
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.TargetUserId,
                this.AccessToken?.TimeOffset
            );
        }

/* diff --- start
        public ulong Subscribe(Action<Gs2.Gs2Friend.Model.FriendRequest> callback)
 diff --- end */
        public ulong Subscribe(Action<Gs2.Gs2Friend.Model.SendFriendRequest> callback) /* diff +++ */
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                    this.TargetUserId
                ),
                callback,
                () =>
                {
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
            #if GS2_ENABLE_UNITASK // diff +++
                    Impl().Forget();
/* diff +++ start */
            #else
                    Impl();
            #endif
/* diff +++ end */
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
/* diff --- start
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
 diff --- end */
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Friend.Model.SendFriendRequest>( /* diff +++ */
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                    this.TargetUserId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Friend.Model.FriendRequest> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Friend.Model.SendFriendRequest> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.FriendRequest> callback)
 diff --- end */
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.SendFriendRequest> callback) /* diff +++ */
        #else
/* diff --- start
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.FriendRequest> callback)
 diff --- end */
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.SendFriendRequest> callback) /* diff +++ */
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

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

    public partial class FriendAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public bool? WithProfile { get; } = null!;

        public FriendAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            bool? withProfile
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.WithProfile = withProfile;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> AddFriendFuture(
            AddFriendRequest request
        ) => AddFriendAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> AddFriendAsync(
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> AddFriendAsync(
        #endif
            AddFriendRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.AddFriendAsync(request)
            );
            var domain = new Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.WithProfile,
                request.TargetUserId
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> DeleteFriendFuture(
            DeleteFriendRequest request
        ) => DeleteFriendAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> DeleteFriendAsync(
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> DeleteFriendAsync(
        #endif
            DeleteFriendRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteFriendAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = new Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.WithProfile,
                request.TargetUserId
            );
            return domain;
        }

        public Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain FriendUser(
            string targetUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.WithProfile,
                targetUserId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Model.Friend> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Model.Friend> ModelAsync()
        #else
        public async Task<Gs2.Gs2Friend.Model.Friend> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.Friend>(
                        (null as Gs2.Gs2Friend.Model.Friend).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Friend.Model.Friend).CacheKey(
                            this.WithProfile
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Friend.Model.Friend).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.WithProfile,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return null;
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Friend.Model.Friend> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Friend.Model.Friend> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Friend.Model.Friend> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Friend.Model.Friend).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.WithProfile,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Friend.Model.Friend> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Friend.Model.Friend).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Friend.Model.Friend).CacheKey(
                    this.WithProfile
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
                    Impl().Forget();
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Friend.Model.Friend>(
                (null as Gs2.Gs2Friend.Model.Friend).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Friend.Model.Friend).CacheKey(
                    this.WithProfile
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Friend.Model.Friend> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.Friend> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.Friend> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

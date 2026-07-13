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

    public partial class FriendUserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public bool? WithProfile { get; } = null!;
        public string TargetUserId { get; } = null!;

        public FriendUserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            bool? withProfile,
            string targetUserId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.WithProfile = withProfile;
            this.TargetUserId = targetUserId;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Friend.Model.FriendUser> GetFuture(
            GetFriendRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Friend.Model.FriendUser> GetAsync(
        #else
        private async Task<Gs2.Gs2Friend.Model.FriendUser> GetAsync(
        #endif
            GetFriendRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithTargetUserId(this.TargetUserId)
                .WithWithProfile(this.WithProfile);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetFriendAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff +++ start */
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> DeleteFuture(
            DeleteFriendRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendUserAccessTokenDomain> DeleteAsync(
        #endif
            DeleteFriendRequest request
        ) {
            try {
                request = request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithTargetUserId(this.TargetUserId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteFriendAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff +++ end */
        public IFuture<Gs2.Gs2Friend.Model.FriendUser> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Model.FriendUser> ModelAsync()
        #else
        public async Task<Gs2.Gs2Friend.Model.FriendUser> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.FriendUser>(
                        (null as Gs2.Gs2Friend.Model.FriendUser).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.WithProfile,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Friend.Model.FriendUser).CacheKey(
                            this.TargetUserId
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Friend.Model.FriendUser).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
/* diff --- start
                    this.WithProfile,
 diff --- end */
                    this.WithProfile ?? default, /* diff +++ */
                    this.TargetUserId,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Friend.Model.FriendUser).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
/* diff --- start
                    this.WithProfile,
 diff --- end */
                    this.WithProfile ?? default, /* diff +++ */
                    this.TargetUserId,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetFriendRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Friend.Model.FriendUser> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Friend.Model.FriendUser> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Friend.Model.FriendUser> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Friend.Model.FriendUser> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Friend.Model.FriendUser> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Friend.Model.FriendUser> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Friend.Model.FriendUser).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                this.WithProfile,
 diff --- end */
                this.WithProfile ?? default, /* diff +++ */
                this.TargetUserId,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Friend.Model.FriendUser> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
/* diff --- start
                    this.WithProfile,
 diff --- end */
                    this.WithProfile ?? default, /* diff +++ */
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheKey(
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
                    Impl().Forget();
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Friend.Model.FriendUser>(
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
/* diff --- start
                    this.WithProfile,
 diff --- end */
                    this.WithProfile ?? default, /* diff +++ */
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheKey(
                    this.TargetUserId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Friend.Model.FriendUser> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Friend.Model.FriendUser> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.FriendUser> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.FriendUser> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

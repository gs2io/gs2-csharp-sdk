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

namespace Gs2.Gs2Friend.Domain.Model
{

    public partial class FollowAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public bool? WithProfile { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public FollowAccessTokenDomain(
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
        public IFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> FollowFuture(
            FollowRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.FollowFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    this.WithProfile,
                    request.TargetUserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> FollowAsync(
            #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> FollowAsync(
            #endif
            FollowRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.FollowAsync(request)
            );
            var domain = new Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.WithProfile,
                request.TargetUserId
            );

            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Friend.Model.FollowUser> Follows(
        )
        {
            return new DescribeFollowsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.WithProfile
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FollowUser> FollowsAsync(
            #else
        public DescribeFollowsIterator FollowsAsync(
            #endif
        )
        {
            return new DescribeFollowsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.WithProfile
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeFollows(
            Action<Gs2.Gs2Friend.Model.FollowUser[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FollowUser>(
                (null as Gs2.Gs2Friend.Model.FollowUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.WithProfile,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await FollowsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeFollowsWithInitialCallAsync(
            Action<Gs2.Gs2Friend.Model.FollowUser[]> callback
        )
        {
            var items = await FollowsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeFollows(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeFollows(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FollowUser>(
                (null as Gs2.Gs2Friend.Model.FollowUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.WithProfile,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateFollows(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FollowUser>(
                (null as Gs2.Gs2Friend.Model.FollowUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.WithProfile,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain FollowUser(
            string targetUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.WithProfile,
                targetUserId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Model.Follow> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.Follow> self)
            {
                var (value, find) = (null as Gs2.Gs2Friend.Model.Follow).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.WithProfile,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                self.OnComplete(null);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.Follow>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Model.Follow> ModelAsync()
            #else
        public async Task<Gs2.Gs2Friend.Model.Follow> ModelAsync()
            #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.Follow>(
                        (null as Gs2.Gs2Friend.Model.Follow).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Friend.Model.Follow).CacheKey(
                            this.WithProfile
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Friend.Model.Follow).GetCache(
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
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Friend.Model.Follow> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Friend.Model.Follow> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Friend.Model.Follow> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Friend.Model.Follow).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.WithProfile,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Friend.Model.Follow> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Friend.Model.Follow).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Friend.Model.Follow).CacheKey(
                    this.WithProfile
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Friend.Model.Follow>(
                (null as Gs2.Gs2Friend.Model.Follow).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Friend.Model.Follow).CacheKey(
                    this.WithProfile
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Friend.Model.Follow> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.Follow> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.Follow> callback)
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

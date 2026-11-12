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
/* diff --- start
using System.Collections;
 diff --- end */
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
using System.Collections; /* diff +++ */
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

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }

        public Gs2.Gs2Friend.Domain.Model.ProfileDomain Profile(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.ProfileDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId
            );
        }

        public Gs2.Gs2Friend.Domain.Model.PublicProfileDomain PublicProfile(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.PublicProfileDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<string> BlackListUsers(
            string timeOffsetToken = null
        )
        {
            return new DescribeBlackListByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<string> BlackListUsersAsync(
        #else
        public DescribeBlackListByUserIdIterator BlackListUsersAsync(
        #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeBlackListByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }

        public Gs2.Gs2Friend.Domain.Model.BlackListDomain BlackList(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.BlackListDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId
            );
        }

        public Gs2.Gs2Friend.Domain.Model.FollowDomain Follow(
            bool? withProfile
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FollowDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                withProfile
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendUser> Friends(
            bool? withProfile = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeFriendsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                withProfile,
                timeOffsetToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendUser> FriendsAsync(
        #else
        public DescribeFriendsByUserIdIterator FriendsAsync(
        #endif
            bool? withProfile = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeFriendsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                withProfile,
                timeOffsetToken
            );
        }

        public ulong SubscribeFriends(
            Action<Gs2.Gs2Friend.Model.FriendUser[]> callback,
            bool? withProfile = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendUser>(
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    withProfile,
                    null
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
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await FriendsAsync(
                                withProfile
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeFriendsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeFriendsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Friend.Model.FriendUser[]> callback,
            bool? withProfile = null
        )
        {
            var items = await FriendsAsync(
                withProfile
            ).ToArrayAsync();
            var callbackId = SubscribeFriends(
                callback,
                withProfile
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeFriends(
            ulong callbackId,
            bool? withProfile = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendUser>(
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    withProfile,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateFriends(
            bool? withProfile = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    withProfile,
                    null
                )
            );
        }

        public Gs2.Gs2Friend.Domain.Model.FriendDomain Friend(
            bool? withProfile
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FriendDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                withProfile
            );
        }
        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> SendRequests(
            bool? withProfile = null,
            string timeOffsetToken = null
 diff --- end */
/* diff +++ start */
        public Gs2Iterator<Gs2.Gs2Friend.Model.SendFriendRequest> SendRequests(
            string timeOffsetToken = null,
            bool? withProfile = null
/* diff +++ end */
        )
        {
            return new DescribeSendRequestsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                withProfile,
                timeOffsetToken
 diff --- end */
/* diff +++ start */
                timeOffsetToken,
                withProfile
/* diff +++ end */
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> SendRequestsAsync(
 diff --- end */
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.SendFriendRequest> SendRequestsAsync( /* diff +++ */
        #else
        public DescribeSendRequestsByUserIdIterator SendRequestsAsync(
        #endif
/* diff --- start
            bool? withProfile = null,
            string timeOffsetToken = null
 diff --- end */
/* diff +++ start */
            string timeOffsetToken = null,
            bool? withProfile = null
/* diff +++ end */
        )
        {
            return new DescribeSendRequestsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                withProfile,
                timeOffsetToken
 diff --- end */
/* diff +++ start */
                timeOffsetToken,
                withProfile
/* diff +++ end */
            );
        }

        public ulong SubscribeSendRequests(
/* diff --- start
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback,
 diff --- end */
            Action<Gs2.Gs2Friend.Model.SendFriendRequest[]> callback, /* diff +++ */
            bool? withProfile = null
        )
        {
/* diff --- start
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
 diff --- end */
/* diff +++ start */
            void ReFetch()
            {
        #if GS2_ENABLE_UNITASK
                async UniTask Impl() {
        #else
                async Task Impl() {
        #endif
                    try {
        #if GS2_ENABLE_UNITASK
                        await UniTask.SwitchToMainThread();
        #endif
                        callback.Invoke(await SendRequestsAsync(
                            withProfile: withProfile
                        ).ToArrayAsync());
                    }
                    catch (System.Exception) {
                        // ignored
                    }
                }
                Impl().Forget();
            }
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.SendFriendRequest>(
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
/* diff +++ end */
                    this.NamespaceName,
                    this.UserId,
                    withProfile, /* diff +++ */
                    null
                ),
                callback,
/* diff --- start
                () =>
                {
        //#if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        //#else
                    async Task Impl() {
        //#endif
                        try {
        //#if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        //#endif
                            callback.Invoke(await SendRequestsAsync(
                                withProfile
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
 diff --- end */
                ReFetch /* diff +++ */
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSendRequestsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeSendRequestsWithInitialCallAsync(
        #endif
/* diff --- start
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback,
 diff --- end */
            Action<Gs2.Gs2Friend.Model.SendFriendRequest[]> callback, /* diff +++ */
            bool? withProfile = null
        )
        {
            var items = await SendRequestsAsync(
/* diff --- start
                withProfile
 diff --- end */
                withProfile: withProfile /* diff +++ */
            ).ToArrayAsync();
            var callbackId = SubscribeSendRequests(
                callback,
                withProfile
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeSendRequests(
            ulong callbackId,
            bool? withProfile = null
        )
        {
/* diff --- start
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
 diff --- end */
/* diff +++ start */
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.SendFriendRequest>(
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
/* diff +++ end */
                    this.NamespaceName,
                    this.UserId,
                    withProfile, /* diff +++ */
                    null
                ),
                callbackId
            );
        }

        public void InvalidateSendRequests(
            bool? withProfile = null
        )
        {
/* diff --- start
            this._gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
 diff --- end */
/* diff +++ start */
            this._gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.SendFriendRequest>(
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
/* diff +++ end */
                    this.NamespaceName,
                    this.UserId,
                    withProfile, /* diff +++ */
                    null
                )
            );
        }

        public Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain SendFriendRequest(
/* diff --- start
            string targetUserId
 diff --- end */
/* diff +++ start */
            string targetUserId,
            bool? withProfile = null
/* diff +++ end */
        ) {
            return new Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                targetUserId
 diff --- end */
/* diff +++ start */
                targetUserId,
                withProfile
/* diff +++ end */
            );
        }
        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequests(
            bool? withProfile = null,
            string timeOffsetToken = null
 diff --- end */
/* diff +++ start */
        public Gs2Iterator<Gs2.Gs2Friend.Model.ReceiveFriendRequest> ReceiveRequests(
            string timeOffsetToken = null,
            bool? withProfile = null
/* diff +++ end */
        )
        {
            return new DescribeReceiveRequestsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                withProfile,
                timeOffsetToken
 diff --- end */
/* diff +++ start */
                timeOffsetToken,
                withProfile
/* diff +++ end */
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequestsAsync(
 diff --- end */
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.ReceiveFriendRequest> ReceiveRequestsAsync( /* diff +++ */
        #else
        public DescribeReceiveRequestsByUserIdIterator ReceiveRequestsAsync(
        #endif
/* diff --- start
            bool? withProfile = null,
            string timeOffsetToken = null
 diff --- end */
/* diff +++ start */
            string timeOffsetToken = null,
            bool? withProfile = null
/* diff +++ end */
        )
        {
            return new DescribeReceiveRequestsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                withProfile,
                timeOffsetToken
 diff --- end */
/* diff +++ start */
                timeOffsetToken,
                withProfile
/* diff +++ end */
            );
        }

        public ulong SubscribeReceiveRequests(
/* diff --- start
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback,
 diff --- end */
            Action<Gs2.Gs2Friend.Model.ReceiveFriendRequest[]> callback, /* diff +++ */
            bool? withProfile = null
        )
        {
/* diff --- start
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
 diff --- end */
/* diff +++ start */
            void ReFetch()
            {
        #if GS2_ENABLE_UNITASK
                async UniTask Impl() {
        #else
                async Task Impl() {
        #endif
                    try {
        #if GS2_ENABLE_UNITASK
                        await UniTask.SwitchToMainThread();
        #endif
                        callback.Invoke(await ReceiveRequestsAsync(
                            withProfile: withProfile
                        ).ToArrayAsync());
                    }
                    catch (System.Exception) {
                        // ignored
                    }
                }
                Impl().Forget();
            }
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
/* diff +++ end */
                    this.NamespaceName,
                    this.UserId,
                    withProfile, /* diff +++ */
                    null
                ),
                callback,
/* diff --- start
                () =>
                {
        //#if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        //#else
                    async Task Impl() {
        //#endif
                        try {
        //#if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        //#endif
                            callback.Invoke(await ReceiveRequestsAsync(
                                withProfile
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
 diff --- end */
                ReFetch /* diff +++ */
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeReceiveRequestsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeReceiveRequestsWithInitialCallAsync(
        #endif
/* diff --- start
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback,
 diff --- end */
            Action<Gs2.Gs2Friend.Model.ReceiveFriendRequest[]> callback, /* diff +++ */
            bool? withProfile = null
        )
        {
            var items = await ReceiveRequestsAsync(
/* diff --- start
                withProfile
 diff --- end */
                withProfile: withProfile /* diff +++ */
            ).ToArrayAsync();
            var callbackId = SubscribeReceiveRequests(
                callback,
                withProfile
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeReceiveRequests(
            ulong callbackId,
            bool? withProfile = null
        )
        {
/* diff --- start
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
 diff --- end */
/* diff +++ start */
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
/* diff +++ end */
                    this.NamespaceName,
                    this.UserId,
                    withProfile, /* diff +++ */
                    null
                ),
                callbackId
            );
        }

        public void InvalidateReceiveRequests(
            bool? withProfile = null
        )
        {
/* diff --- start
            this._gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
 diff --- end */
/* diff +++ start */
            this._gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
/* diff +++ end */
                    this.NamespaceName,
                    this.UserId,
                    withProfile, /* diff +++ */
                    null
                )
            );
        }

        public Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain ReceiveFriendRequest(
/* diff --- start
            string fromUserId
 diff --- end */
/* diff +++ start */
            string fromUserId,
            bool? withProfile = null
/* diff +++ end */
        ) {
            return new Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                fromUserId
 diff --- end */
/* diff +++ start */
                fromUserId,
                withProfile
/* diff +++ end */
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestFuture(
 diff --- end */
        public IFuture<Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain> SendRequestFuture( /* diff +++ */
            SendRequestByUserIdRequest request
        ) => SendRequestAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestAsync(
 diff --- end */
        public async UniTask<Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain> SendRequestAsync( /* diff +++ */
        #else
/* diff --- start
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestAsync(
 diff --- end */
        public async Task<Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain> SendRequestAsync( /* diff +++ */
        #endif
            SendRequestByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SendRequestByUserIdAsync(request)
            );
/* diff --- start
            var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
 diff --- end */
            var domain = new Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain( /* diff +++ */
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
/* diff --- start
                result?.Item?.TargetUserId
 diff --- end */
/* diff +++ start */
                result?.Item?.TargetUserId,
                request.WithProfile
/* diff +++ end */
            );

            return domain;
        }

    }

    public partial class UserDomain {

    }
}

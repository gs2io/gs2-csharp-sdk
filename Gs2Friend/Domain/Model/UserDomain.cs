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

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
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
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

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

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
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
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeFriends(
            Action<Gs2.Gs2Friend.Model.FriendUser[]> callback,
            bool? withProfile = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendUser>(
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    withProfile
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeFriendsWithInitialCallAsync(
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
        #endif

        public void UnsubscribeFriends(
            ulong callbackId,
            bool? withProfile = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendUser>(
                (null as Gs2.Gs2Friend.Model.FriendUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    withProfile
                ),
                callbackId
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
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> SendRequests(
            string timeOffsetToken = null
        )
        {
            return new DescribeSendRequestsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> SendRequestsAsync(
            #else
        public DescribeSendRequestsByUserIdIterator SendRequestsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeSendRequestsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSendRequests(
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSendRequestsWithInitialCallAsync(
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback
        )
        {
            var items = await SendRequestsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSendRequests(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSendRequests(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain SendFriendRequest(
            string targetUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                targetUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequests(
            string timeOffsetToken = null
        )
        {
            return new DescribeReceiveRequestsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequestsAsync(
            #else
        public DescribeReceiveRequestsByUserIdIterator ReceiveRequestsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeReceiveRequestsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeReceiveRequests(
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeReceiveRequestsWithInitialCallAsync(
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback
        )
        {
            var items = await ReceiveRequestsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeReceiveRequests(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeReceiveRequests(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain ReceiveFriendRequest(
            string fromUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                fromUserId
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestFuture(
            SendRequestByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.SendRequestByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TargetUserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestAsync(
            #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestAsync(
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
                () => this._client.SendRequestByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.TargetUserId
            );

            return domain;
        }
        #endif

    }

    public partial class UserDomain {

    }
}

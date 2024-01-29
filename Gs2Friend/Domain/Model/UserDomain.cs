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
using Gs2.Gs2Friend.Domain.Iterator;
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Result;
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
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Friend.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
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
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<string> BlackLists(
        )
        {
            return new DescribeBlackListByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<string> BlackListsAsync(
            #else
        public Gs2Iterator<string> BlackLists(
            #endif
        #else
        public DescribeBlackListByUserIdIterator BlackListsAsync(
        #endif
        )
        {
            return new DescribeBlackListByUserIdIterator(
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

        public ulong SubscribeBlackLists(
            Action<string[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<string>(
                "friend:UserId",
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeBlackListsWithInitialCallAsync(
            Action<string[]> callback
        )
        {
            var items = await BlackListsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeBlackLists(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeBlackLists(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<string>(
                "friend:UserId",
                callbackId
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
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Friend.Model.FollowUser> Follows(
            bool? withProfile
        )
        {
            return new DescribeFollowsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                withProfile
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FollowUser> FollowsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FollowUser> Follows(
            #endif
        #else
        public DescribeFollowsByUserIdIterator FollowsAsync(
        #endif
            bool? withProfile
        )
        {
            return new DescribeFollowsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                withProfile
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

        public ulong SubscribeFollows(
            bool? withProfile,
            Action<Gs2.Gs2Friend.Model.FollowUser[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FollowUser>(
                Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "FollowUser:" + (withProfile == true)
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeFollowsWithInitialCallAsync(
            bool? withProfile,
            Action<Gs2.Gs2Friend.Model.FollowUser[]> callback
        )
        {
            var items = await FollowsAsync(
                withProfile
            ).ToArrayAsync();
            var callbackId = SubscribeFollows(
                withProfile,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeFollows(
            bool? withProfile,
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FollowUser>(
                Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "FollowUser:" + (withProfile == true)
                ),
                callbackId
            );
        }

        public Gs2.Gs2Friend.Domain.Model.FollowUserDomain FollowUser(
            string targetUserId,
            bool? withProfile
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FollowUserDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                targetUserId,
                withProfile
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendUser> Friends(
            bool? withProfile
        )
        {
            return new DescribeFriendsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                withProfile
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendUser> FriendsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendUser> Friends(
            #endif
        #else
        public DescribeFriendsByUserIdIterator FriendsAsync(
        #endif
            bool? withProfile
        )
        {
            return new DescribeFriendsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                withProfile
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

        public ulong SubscribeFriends(
            bool? withProfile,
            Action<Gs2.Gs2Friend.Model.FriendUser[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendUser>(
                Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    withProfile?.ToString() ?? "False",
                    "FriendUser"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeFriendsWithInitialCallAsync(
            bool? withProfile,
            Action<Gs2.Gs2Friend.Model.FriendUser[]> callback
        )
        {
            var items = await FriendsAsync(
                withProfile
            ).ToArrayAsync();
            var callbackId = SubscribeFriends(
                withProfile,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeFriends(
            bool? withProfile,
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendUser>(
                Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    withProfile?.ToString() ?? "False",
                    "FriendUser"
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
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> SendRequests(
        )
        {
            return new DescribeSendRequestsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> SendRequestsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> SendRequests(
            #endif
        #else
        public DescribeSendRequestsByUserIdIterator SendRequestsAsync(
        #endif
        )
        {
            return new DescribeSendRequestsByUserIdIterator(
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

        public ulong SubscribeSendRequests(
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "FriendRequest"
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
                Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "FriendRequest"
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
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequests(
        )
        {
            return new DescribeReceiveRequestsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequestsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequests(
            #endif
        #else
        public DescribeReceiveRequestsByUserIdIterator ReceiveRequestsAsync(
        #endif
        )
        {
            return new DescribeReceiveRequestsByUserIdIterator(
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

        public ulong SubscribeReceiveRequests(
            Action<Gs2.Gs2Friend.Model.FriendRequest[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "FriendRequest"
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
                Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "FriendRequest"
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

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "friend",
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

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestFuture(
            SendRequestByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.SendRequestByUserIdFuture(
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
                        var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "SendFriendRequest"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                            resultModel.Item.TargetUserId.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TargetUserId,
                    "SendFriendRequest"
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            SendRequestByUserIdResult result = null;
                result = await this._client.SendRequestByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SendFriendRequest"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                        resultModel.Item.TargetUserId.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TargetUserId,
                    "SendFriendRequest"
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to SendRequestFuture.")]
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequest(
            SendRequestByUserIdRequest request
        ) {
            return SendRequestFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

    }
}

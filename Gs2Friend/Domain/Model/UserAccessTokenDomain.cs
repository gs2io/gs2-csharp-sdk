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
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Friend.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Friend.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> SendRequestFuture(
            SendRequestRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token);
                var future = this._client.SendRequestFuture(
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
                var cache = this._gs2.Cache;
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
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.TargetUserId,
                    "SendFriendRequest"
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> SendRequestAsync(
            #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> SendRequestAsync(
            #endif
            SendRequestRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            SendRequestResult result = null;
                result = await this._client.SendRequestAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
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
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.TargetUserId,
                    "SendFriendRequest"
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to SendRequestFuture.")]
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> SendRequest(
            SendRequestRequest request
        ) {
            return SendRequestFuture(request);
        }
        #endif

        public Gs2.Gs2Friend.Domain.Model.ProfileAccessTokenDomain Profile(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.ProfileAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken
            );
        }

        public Gs2.Gs2Friend.Domain.Model.PublicProfileAccessTokenDomain PublicProfile(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.PublicProfileAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<string> BlackLists(
        )
        {
            return new DescribeBlackListIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<string> BlackListsAsync(
            #else
        public Gs2Iterator<string> BlackLists(
            #endif
        #else
        public DescribeBlackListIterator BlackListsAsync(
        #endif
        )
        {
            return new DescribeBlackListIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
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

        public ulong SubscribeBlackLists(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<string>(
                "friend:UserId",
                callback
            );
        }

        public void UnsubscribeBlackLists(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<string>(
                "friend:UserId",
                callbackId
            );
        }

        public Gs2.Gs2Friend.Domain.Model.BlackListAccessTokenDomain BlackList(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.BlackListAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Friend.Model.FollowUser> Follows(
            bool? withProfile
        )
        {
            return new DescribeFollowsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                withProfile
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FollowUser> FollowsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FollowUser> Follows(
            #endif
        #else
        public DescribeFollowsIterator FollowsAsync(
        #endif
            bool? withProfile
        )
        {
            return new DescribeFollowsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
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
            Action callback,
            bool? withProfile
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

        public void UnsubscribeFollows(
            ulong callbackId,
            bool? withProfile
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

        public Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain FollowUser(
            string targetUserId,
            bool? withProfile
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
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
            return new DescribeFriendsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                withProfile
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendUser> FriendsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendUser> Friends(
            #endif
        #else
        public DescribeFriendsIterator FriendsAsync(
        #endif
            bool? withProfile
        )
        {
            return new DescribeFriendsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
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
            Action callback,
            bool? withProfile
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

        public void UnsubscribeFriends(
            ulong callbackId,
            bool? withProfile
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

        public Gs2.Gs2Friend.Domain.Model.FriendAccessTokenDomain Friend(
            bool? withProfile
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FriendAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                withProfile
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> SendRequests(
        )
        {
            return new DescribeSendRequestsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> SendRequestsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> SendRequests(
            #endif
        #else
        public DescribeSendRequestsIterator SendRequestsAsync(
        #endif
        )
        {
            return new DescribeSendRequestsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
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

        public ulong SubscribeSendRequests(Action callback)
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

        public void UnsubscribeSendRequests(ulong callbackId)
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

        public Gs2.Gs2Friend.Domain.Model.SendFriendRequestAccessTokenDomain SendFriendRequest(
            string targetUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.SendFriendRequestAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                targetUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequests(
        )
        {
            return new DescribeReceiveRequestsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequestsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequests(
            #endif
        #else
        public DescribeReceiveRequestsIterator ReceiveRequestsAsync(
        #endif
        )
        {
            return new DescribeReceiveRequestsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
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

        public ulong SubscribeReceiveRequests(Action callback)
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

        public void UnsubscribeReceiveRequests(ulong callbackId)
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

        public Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestAccessTokenDomain ReceiveFriendRequest(
            string fromUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
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
}

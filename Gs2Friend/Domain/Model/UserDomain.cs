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

    public partial class UserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FriendRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FriendRestClient(
                session
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
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId
            );
        }

        public Gs2.Gs2Friend.Domain.Model.PublicProfileDomain PublicProfile(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.PublicProfileDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
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
                this._cache,
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
        public DescribeBlackListByUserIdIterator BlackLists(
        #endif
        )
        {
            return new DescribeBlackListByUserIdIterator(
                this._cache,
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

        public ulong SubscribeBlackLists(Action callback)
        {
            return this._cache.ListSubscribe<string>(
                "friend:UserId",
                callback
            );
        }

        public void UnsubscribeBlackLists(ulong callbackId)
        {
            this._cache.ListUnsubscribe<string>(
                "friend:UserId",
                callbackId
            );
        }

        public Gs2.Gs2Friend.Domain.Model.BlackListDomain BlackList(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.BlackListDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
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
                this._cache,
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
        public DescribeFollowsByUserIdIterator Follows(
        #endif
            bool? withProfile
        )
        {
            return new DescribeFollowsByUserIdIterator(
                this._cache,
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
            Action callback,
            bool? withProfile
        )
        {
            return this._cache.ListSubscribe<Gs2.Gs2Friend.Model.FollowUser>(
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
            this._cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FollowUser>(
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
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
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
                this._cache,
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
        public DescribeFriendsByUserIdIterator Friends(
        #endif
            bool? withProfile
        )
        {
            return new DescribeFriendsByUserIdIterator(
                this._cache,
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
            Action callback,
            bool? withProfile
        )
        {
            return this._cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendUser>(
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
            this._cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendUser>(
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
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
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
                this._cache,
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
        public DescribeSendRequestsByUserIdIterator SendRequests(
        #endif
        )
        {
            return new DescribeSendRequestsByUserIdIterator(
                this._cache,
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

        public ulong SubscribeSendRequests(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
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
            this._cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
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
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
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
                this._cache,
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
        public DescribeReceiveRequestsByUserIdIterator ReceiveRequests(
        #endif
        )
        {
            return new DescribeReceiveRequestsByUserIdIterator(
                this._cache,
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

        public ulong SubscribeReceiveRequests(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
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
            this._cache.ListUnsubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
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
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
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
                #if UNITY_2017_1_OR_NEWER
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                SendRequestByUserIdResult result = null;
                    result = await this._client.SendRequestByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
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
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TargetUserId,
                    "SendFriendRequest"
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestAsync(
            SendRequestByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            SendRequestByUserIdResult result = null;
                result = await this._client.SendRequestByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
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
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TargetUserId,
                    "SendFriendRequest"
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> SendRequestAsync(
            SendRequestByUserIdRequest request
        ) {
            var future = SendRequestFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
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

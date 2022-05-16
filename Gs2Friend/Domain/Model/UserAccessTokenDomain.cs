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
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FriendRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;

        public UserAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FriendRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Friend.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> SendRequestAsync(
            #else
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> SendRequest(
            #endif
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> SendRequestAsync(
        #endif
            SendRequestRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    this._accessToken?.UserId.ToString(),
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
            #else
            var result = await this._client.SendRequestAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    this._accessToken?.UserId.ToString(),
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
            #endif
            Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                this._accessToken,
                result?.Item?.TargetUserId
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestAccessTokenDomain>(Impl);
        #endif
        }

        public Gs2.Gs2Friend.Domain.Model.ProfileAccessTokenDomain Profile(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.ProfileAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._accessToken
            );
        }

        public Gs2.Gs2Friend.Domain.Model.PublicProfileAccessTokenDomain PublicProfile(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.PublicProfileAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<string> BlackLists(
        )
        {
            return new DescribeBlackListIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken
            );
        }

        public IUniTaskAsyncEnumerable<string> BlackListsAsync(
            #else
        public Gs2Iterator<string> BlackLists(
            #endif
        #else
        public DescribeBlackListIterator BlackLists(
        #endif
        )
        {
            return new DescribeBlackListIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken
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

        public Gs2.Gs2Friend.Domain.Model.BlackListAccessTokenDomain BlackList(
        ) {
            return new Gs2.Gs2Friend.Domain.Model.BlackListAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
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
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken,
                withProfile
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FollowUser> FollowsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FollowUser> Follows(
            #endif
        #else
        public DescribeFollowsIterator Follows(
        #endif
            bool? withProfile
        )
        {
            return new DescribeFollowsIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken,
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

        public Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain FollowUser(
            string targetUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._accessToken,
                targetUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendUser> Friends(
            bool? withProfile
        )
        {
            return new DescribeFriendsIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken,
                withProfile
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendUser> FriendsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendUser> Friends(
            #endif
        #else
        public DescribeFriendsIterator Friends(
        #endif
            bool? withProfile
        )
        {
            return new DescribeFriendsIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken,
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

        public Gs2.Gs2Friend.Domain.Model.FriendAccessTokenDomain Friend(
            bool? withProfile
        ) {
            return new Gs2.Gs2Friend.Domain.Model.FriendAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
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
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> SendRequestsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> SendRequests(
            #endif
        #else
        public DescribeSendRequestsIterator SendRequests(
        #endif
        )
        {
            return new DescribeSendRequestsIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken
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

        public Gs2.Gs2Friend.Domain.Model.SendFriendRequestAccessTokenDomain SendFriendRequest(
            string targetUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.SendFriendRequestAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
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
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequestsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest> ReceiveRequests(
            #endif
        #else
        public DescribeReceiveRequestsIterator ReceiveRequests(
        #endif
        )
        {
            return new DescribeReceiveRequestsIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._accessToken
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

        public Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestAccessTokenDomain ReceiveFriendRequest(
            string fromUserId
        ) {
            return new Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
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

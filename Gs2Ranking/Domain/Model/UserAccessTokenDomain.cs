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
using Gs2.Gs2Ranking.Domain.Iterator;
using Gs2.Gs2Ranking.Request;
using Gs2.Gs2Ranking.Result;
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

namespace Gs2.Gs2Ranking.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public bool? Processing { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2RankingRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain> SubscribeFuture(
            SubscribeRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token);
                var future = this._client.SubscribeFuture(
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
                        var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "SubscribeUser"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                            resultModel.Item.CategoryName.ToString(),
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
                var domain = new Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.CategoryName,
                    result?.Item?.TargetUserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain> SubscribeAsync(
            #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain> SubscribeAsync(
            #endif
            SubscribeRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            SubscribeResult result = null;
                result = await this._client.SubscribeAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SubscribeUser"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        resultModel.Item.CategoryName.ToString(),
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
                var domain = new Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.CategoryName,
                    result?.Item?.TargetUserId
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to SubscribeFuture.")]
        public IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain> Subscribe(
            SubscribeRequest request
        ) {
            return SubscribeFuture(request);
        }
        #endif

        public Gs2.Gs2Ranking.Domain.Model.SubscribeAccessTokenDomain Subscribe(
            string categoryName
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.SubscribeAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                categoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribeUsers(
            string categoryName
        )
        {
            return new DescribeSubscribesByCategoryNameIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribeUsersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribeUsers(
            #endif
        #else
        public DescribeSubscribesByCategoryNameIterator SubscribeUsersAsync(
        #endif
            string categoryName
        )
        {
            return new DescribeSubscribesByCategoryNameIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                categoryName,
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

        public ulong SubscribeSubscribeUsers(
            Action<Gs2.Gs2Ranking.Model.SubscribeUser[]> callback,
            string categoryName
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking.Model.SubscribeUser>(
                Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "SubscribeUser:" + categoryName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSubscribeUsersWithInitialCallAsync(
            Action<Gs2.Gs2Ranking.Model.SubscribeUser[]> callback,
            string categoryName
        )
        {
            var items = await SubscribeUsersAsync(
                categoryName
            ).ToArrayAsync();
            var callbackId = SubscribeSubscribeUsers(
                callback,
                categoryName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSubscribeUsers(
            ulong callbackId,
            string categoryName
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking.Model.SubscribeUser>(
                Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "SubscribeUser:" + categoryName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain SubscribeUser(
            string categoryName,
            string targetUserId
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.SubscribeUserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                categoryName,
                targetUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> Rankings(
            string categoryName,
            string additionalScopeName = null
        )
        {
            return new DescribeRankingsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.AccessToken,
                additionalScopeName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> RankingsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> Rankings(
            #endif
        #else
        public DescribeRankingsIterator RankingsAsync(
        #endif
            string categoryName,
            string additionalScopeName = null
        )
        {
            return new DescribeRankingsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.AccessToken,
                additionalScopeName
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

        public ulong SubscribeRankings(
            Action<Gs2.Gs2Ranking.Model.Ranking[]> callback,
            string categoryName,
            string additionalScopeName = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking.Model.Ranking>(
                Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Ranking"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeRankingsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking.Model.Ranking[]> callback,
            string categoryName,
            string additionalScopeName = null
        )
        {
            var items = await RankingsAsync(
                categoryName,
                additionalScopeName
            ).ToArrayAsync();
            var callbackId = SubscribeRankings(
                callback,
                categoryName,
                additionalScopeName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeRankings(
            ulong callbackId,
            string categoryName,
            string additionalScopeName = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking.Model.Ranking>(
                Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Ranking"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking.Domain.Model.RankingAccessTokenDomain Ranking(
            string categoryName
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.RankingAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                categoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Score> Scores(
            string categoryName,
            string scorerUserId
        )
        {
            return new DescribeScoresIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.AccessToken,
                scorerUserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Score> ScoresAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Score> Scores(
            #endif
        #else
        public DescribeScoresIterator ScoresAsync(
        #endif
            string categoryName,
            string scorerUserId
        )
        {
            return new DescribeScoresIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.AccessToken,
                scorerUserId
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

        public ulong SubscribeScores(
            Action<Gs2.Gs2Ranking.Model.Score[]> callback,
            string categoryName,
            string scorerUserId
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking.Model.Score>(
                Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Score:" + categoryName + ":" + scorerUserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeScoresWithInitialCallAsync(
            Action<Gs2.Gs2Ranking.Model.Score[]> callback,
            string categoryName,
            string scorerUserId
        )
        {
            var items = await ScoresAsync(
                categoryName,
                scorerUserId
            ).ToArrayAsync();
            var callbackId = SubscribeScores(
                callback,
                categoryName,
                scorerUserId
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeScores(
            ulong callbackId,
            string categoryName,
            string scorerUserId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking.Model.Score>(
                Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Score:" + categoryName + ":" + scorerUserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain Score(
            string categoryName,
            string scorerUserId,
            string uniqueId
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                categoryName,
                scorerUserId,
                uniqueId
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
                "ranking",
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

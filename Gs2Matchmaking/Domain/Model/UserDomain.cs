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
using Gs2.Gs2Matchmaking.Domain.Iterator;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
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

namespace Gs2.Gs2Matchmaking.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string MatchmakingContextToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> Gatherings(
        )
        {
            return new DescribeGatheringsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> GatheringsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> Gatherings(
            #endif
        #else
        public DescribeGatheringsIterator GatheringsAsync(
        #endif
        )
        {
            return new DescribeGatheringsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeGatherings(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Singleton",
                    "Gathering"
                ),
                callback
            );
        }

        public void UnsubscribeGatherings(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Singleton",
                    "Gathering"
                ),
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingByPlayer(
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByPlayerIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                player
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingByPlayerAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingByPlayer(
            #endif
        #else
        public DoMatchmakingByPlayerIterator DoMatchmakingByPlayerAsync(
        #endif
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByPlayerIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                player
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

        public ulong SubscribeDoMatchmakingByPlayer(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                "matchmaking",
                callback
            );
        }

        public void UnsubscribeDoMatchmakingByPlayer(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                "matchmaking",
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmaking(
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                player
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmaking(
            #endif
        #else
        public DoMatchmakingByUserIdIterator DoMatchmakingAsync(
        #endif
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                player
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

        public ulong SubscribeDoMatchmaking(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                "matchmaking",
                callback
            );
        }

        public void UnsubscribeDoMatchmaking(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                "matchmaking",
                callbackId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain Gathering(
            string gatheringName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                gatheringName
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.BallotDomain Ballot(
            string ratingName,
            string gatheringName,
            int? numberOfPlayer,
            string keyId
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.BallotDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                ratingName,
                gatheringName,
                numberOfPlayer,
                keyId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Rating> Ratings(
        )
        {
            return new DescribeRatingsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Rating> RatingsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Rating> Ratings(
            #endif
        #else
        public DescribeRatingsByUserIdIterator RatingsAsync(
        #endif
        )
        {
            return new DescribeRatingsByUserIdIterator(
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

        public ulong SubscribeRatings(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Rating>(
                Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Rating"
                ),
                callback
            );
        }

        public void UnsubscribeRatings(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Rating>(
                Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Rating"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.RatingDomain Rating(
            string ratingName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                ratingName
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
                "matchmaking",
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
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGatheringFuture(
            CreateGatheringByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.CreateGatheringByUserIdFuture(
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
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            "Gathering"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                    this._gs2,
                    request.NamespaceName,
                    request.UserId,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGatheringAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGatheringAsync(
            #endif
            CreateGatheringByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            CreateGatheringByUserIdResult result = null;
                result = await this._client.CreateGatheringByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        "Gathering"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                    this._gs2,
                    request.NamespaceName,
                    request.UserId,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateGatheringFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGathering(
            CreateGatheringByUserIdRequest request
        ) {
            return CreateGatheringFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGatheringFuture(
            DeleteGatheringRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DeleteGatheringFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            request.GatheringName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "gathering")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            "Gathering"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Matchmaking.Model.Gathering>(parentKey, key);
                    }
                }
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                    this._gs2,
                    request.NamespaceName,
                    this.UserId,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGatheringAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGatheringAsync(
            #endif
            DeleteGatheringRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            DeleteGatheringResult result = null;
            try {
                result = await this._client.DeleteGatheringAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                    request.GatheringName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "gathering")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        "Gathering"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Matchmaking.Model.Gathering>(parentKey, key);
                }
            }
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                    this._gs2,
                    request.NamespaceName,
                    this.UserId,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteGatheringFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGathering(
            DeleteGatheringRequest request
        ) {
            return DeleteGatheringFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResultFuture(
            PutResultRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.PutResultFuture(
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
                    {
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Rating"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                                item.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain(
                        this._gs2,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        result.Items[i]?.Name
                    );
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Rating"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResultAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResultAsync(
            #endif
            PutResultRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            PutResultResult result = null;
                result = await this._client.PutResultAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Rating"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                            item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain(
                        this._gs2,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        result.Items[i]?.Name
                    );
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Rating"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to PutResultFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResult(
            PutResultRequest request
        ) {
            return PutResultFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

    }
}

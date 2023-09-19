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

    public partial class BallotAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _ratingName;
        private readonly string _gatheringName;
        private readonly int? _numberOfPlayer;
        private readonly string _keyId;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string RatingName => _ratingName;
        public string GatheringName => _gatheringName;
        public int? NumberOfPlayer => _numberOfPlayer;
        public string KeyId => _keyId;

        public BallotAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string ratingName,
            string gatheringName,
            int? numberOfPlayer,
            string keyId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2MatchmakingRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._ratingName = ratingName;
            this._gatheringName = gatheringName;
            this._numberOfPlayer = numberOfPlayer;
            this._keyId = keyId;
            this._parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Ballot"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain> GetFuture(
            GetBallotRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRatingName(this.RatingName)
                    .WithGatheringName(this.GatheringName)
                    .WithNumberOfPlayer(this.NumberOfPlayer)
                    .WithKeyId(this.KeyId);
                var future = this._client.GetBallotFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                            request.RatingName.ToString(),
                            request.GatheringName.ToString(),
                            request.NumberOfPlayer.ToString(),
                            request.KeyId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "ballot")
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRatingName(this.RatingName)
                    .WithGatheringName(this.GatheringName)
                    .WithNumberOfPlayer(this.NumberOfPlayer)
                    .WithKeyId(this.KeyId);
                GetBallotResult result = null;
                try {
                    result = await this._client.GetBallotAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                        request.RatingName.ToString(),
                        request.GatheringName.ToString(),
                        request.NumberOfPlayer.ToString(),
                        request.KeyId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "ballot")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Ballot"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                            resultModel.Item.RatingName.ToString(),
                            resultModel.Item.GatheringName.ToString(),
                            resultModel.Item.NumberOfPlayer.ToString(),
                            requestModel.KeyId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;
                domain.Body = result?.Body;
                domain.Signature = result?.Signature;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain> GetAsync(
            GetBallotRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRatingName(this.RatingName)
                .WithGatheringName(this.GatheringName)
                .WithNumberOfPlayer(this.NumberOfPlayer)
                .WithKeyId(this.KeyId);
            var future = this._client.GetBallotFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                        request.RatingName.ToString(),
                        request.GatheringName.ToString(),
                        request.NumberOfPlayer.ToString(),
                        request.KeyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "ballot")
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRatingName(this.RatingName)
                .WithGatheringName(this.GatheringName)
                .WithNumberOfPlayer(this.NumberOfPlayer)
                .WithKeyId(this.KeyId);
            GetBallotResult result = null;
            try {
                result = await this._client.GetBallotAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                    request.RatingName.ToString(),
                    request.GatheringName.ToString(),
                    request.NumberOfPlayer.ToString(),
                    request.KeyId.ToString()
                    );
                _cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "ballot")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Ballot"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                        resultModel.Item.RatingName.ToString(),
                        resultModel.Item.GatheringName.ToString(),
                        resultModel.Item.NumberOfPlayer.ToString(),
                        requestModel.KeyId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

            return domain;
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string ratingName,
            string gatheringName,
            string numberOfPlayer,
            string keyId,
            string childType
        )
        {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName ?? "null",
                userId ?? "null",
                ratingName ?? "null",
                gatheringName ?? "null",
                numberOfPlayer ?? "null",
                keyId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string ratingName,
            string gatheringName,
            string numberOfPlayer,
            string keyId
        )
        {
            return string.Join(
                ":",
                ratingName ?? "null",
                gatheringName ?? "null",
                numberOfPlayer ?? "null",
                keyId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Model.Ballot> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.Ballot> self)
            {
                Gs2.Gs2Matchmaking.Model.Ballot value = null;
                var find = false;
                if (!find) {
                    var future = this.GetFuture(
                        new GetBallotRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                                    this.RatingName?.ToString(),
                                    this.GatheringName?.ToString(),
                                    this.NumberOfPlayer?.ToString(),
                                    this.KeyId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "ballot")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _cache.Get<Gs2.Gs2Matchmaking.Model.Ballot>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                            this.RatingName?.ToString(),
                            this.GatheringName?.ToString(),
                            this.NumberOfPlayer?.ToString(),
                            this.KeyId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Model.Ballot>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Matchmaking.Model.Ballot> ModelAsync()
        {
            Gs2.Gs2Matchmaking.Model.Ballot value = null;
            var find = false;
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetBallotRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                                    this.RatingName?.ToString(),
                                    this.GatheringName?.ToString(),
                                    this.NumberOfPlayer?.ToString(),
                                    this.KeyId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "ballot")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Matchmaking.Model.Ballot>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                            this.RatingName?.ToString(),
                            this.GatheringName?.ToString(),
                            this.NumberOfPlayer?.ToString(),
                            this.KeyId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Model.Ballot> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Matchmaking.Model.Ballot> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Model.Ballot> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Matchmaking.Model.Ballot> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

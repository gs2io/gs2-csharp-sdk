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
using Gs2.Gs2Matchmaking.Domain.Iterator;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
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

namespace Gs2.Gs2Matchmaking.Domain.Model
{

    public partial class BallotAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;
        private readonly string _ratingName;
        private readonly string _gatheringName;
        private readonly int? _numberOfPlayer;
        private readonly string _keyId;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
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
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                "Ballot"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain> GetAsync(
            #else
        private IFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain> GetAsync(
        #endif
            GetBallotRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRatingName(this._ratingName)
                .WithGatheringName(this._gatheringName)
                .WithNumberOfPlayer(this._numberOfPlayer)
                .WithKeyId(this._keyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetBallotFuture(
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
            var result = await this._client.GetBallotAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                        request.RatingName != null ? request.RatingName.ToString() : null,
                        request.GatheringName != null ? request.GatheringName.ToString() : null,
                        request.NumberOfPlayer != null ? request.NumberOfPlayer.ToString() : null,
                        request.KeyId != null ? request.KeyId.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain>(Impl);
        #endif
        }

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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Model.Ballot> Model() {
            #else
        public IFuture<Gs2.Gs2Matchmaking.Model.Ballot> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Matchmaking.Model.Ballot> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.Ballot> self)
            {
        #endif
            Gs2.Gs2Matchmaking.Model.Ballot value = _cache.Get<Gs2.Gs2Matchmaking.Model.Ballot>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                    this.RatingName?.ToString(),
                    this.GatheringName?.ToString(),
                    this.NumberOfPlayer?.ToString(),
                    this.KeyId?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetBallotRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Matchmaking.Model.Ballot>(
                            _parentKey,
                            Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                                this.RatingName?.ToString(),
                                this.GatheringName?.ToString(),
                                this.NumberOfPlayer?.ToString(),
                                this.KeyId?.ToString()
                            )
                        );
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException) {
                    _cache.Delete<Gs2.Gs2Matchmaking.Model.Ballot>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                            this.RatingName?.ToString(),
                            this.GatheringName?.ToString(),
                            this.NumberOfPlayer?.ToString(),
                            this.KeyId?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Matchmaking.Model.Ballot>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                    this.RatingName?.ToString(),
                    this.GatheringName?.ToString(),
                    this.NumberOfPlayer?.ToString(),
                    this.KeyId?.ToString()
                )
            );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Model.Ballot>(Impl);
        #endif
        }

    }
}

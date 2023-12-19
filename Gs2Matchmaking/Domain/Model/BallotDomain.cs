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

    public partial class BallotDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _ratingName;
        private readonly string _gatheringName;
        private readonly int? _numberOfPlayer;
        private readonly string _keyId;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string RatingName => _ratingName;
        public string GatheringName => _gatheringName;
        public int? NumberOfPlayer => _numberOfPlayer;
        public string KeyId => _keyId;

        public BallotDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string ratingName,
            string gatheringName,
            int? numberOfPlayer,
            string keyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
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

    }

    public partial class BallotDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotDomain> GetFuture(
            GetBallotByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithRatingName(this.RatingName)
                    .WithGatheringName(this.GatheringName)
                    .WithNumberOfPlayer(this.NumberOfPlayer)
                    .WithKeyId(this.KeyId);
                var future = this._client.GetBallotByUserIdFuture(
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
                        this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "ballot")
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
                            this.UserId,
                            "Ballot"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                            resultModel.Item.RatingName.ToString(),
                            resultModel.Item.GatheringName.ToString(),
                            resultModel.Item.NumberOfPlayer.ToString(),
                            requestModel.KeyId.ToString()
                        );
                        _gs2.Cache.Put(
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
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.BallotDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Matchmaking.Domain.Model.BallotDomain> GetAsync(
            #else
        private async Task<Gs2.Gs2Matchmaking.Domain.Model.BallotDomain> GetAsync(
            #endif
            GetBallotByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithRatingName(this.RatingName)
                .WithGatheringName(this.GatheringName)
                .WithNumberOfPlayer(this.NumberOfPlayer)
                .WithKeyId(this.KeyId);
            GetBallotByUserIdResult result = null;
            try {
                result = await this._client.GetBallotByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                    request.RatingName.ToString(),
                    request.GatheringName.ToString(),
                    request.NumberOfPlayer.ToString(),
                    request.KeyId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "ballot")
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
                        this.UserId,
                        "Ballot"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                        resultModel.Item.RatingName.ToString(),
                        resultModel.Item.GatheringName.ToString(),
                        resultModel.Item.NumberOfPlayer.ToString(),
                        requestModel.KeyId.ToString()
                    );
                    _gs2.Cache.Put(
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

    }

    public partial class BallotDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Model.Ballot> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.Ballot> self)
            {
                Gs2.Gs2Matchmaking.Model.Ballot value = null;
                var find = false;
                if (!find) {
                    var future = this.GetFuture(
                        new GetBallotByUserIdRequest()
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
                            this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "ballot")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.Ballot>(
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Model.Ballot> ModelAsync()
            #else
        public async Task<Gs2.Gs2Matchmaking.Model.Ballot> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Matchmaking.Model.Ballot>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                    this.RatingName?.ToString(),
                    this.GatheringName?.ToString(),
                    this.NumberOfPlayer?.ToString(),
                    this.KeyId?.ToString()
                )).LockAsync())
            {
        # endif
                Gs2.Gs2Matchmaking.Model.Ballot value = null;
                var find = false;
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetBallotByUserIdRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                                    this.RatingName?.ToString(),
                                    this.GatheringName?.ToString(),
                                    this.NumberOfPlayer?.ToString(),
                                    this.KeyId?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Ballot>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "ballot")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.Ballot>(
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
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
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


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Matchmaking.Model.Ballot>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                    this.RatingName.ToString(),
                    this.GatheringName.ToString(),
                    this.NumberOfPlayer.ToString(),
                    this.KeyId.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Matchmaking.Model.Ballot> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                    this.RatingName.ToString(),
                    this.GatheringName.ToString(),
                    this.NumberOfPlayer.ToString(),
                    this.KeyId.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Matchmaking.Model.Ballot>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.BallotDomain.CreateCacheKey(
                    this.RatingName.ToString(),
                    this.GatheringName.ToString(),
                    this.NumberOfPlayer.ToString(),
                    this.KeyId.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Matchmaking.Model.Ballot> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Matchmaking.Model.Ballot> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Matchmaking.Model.Ballot> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}

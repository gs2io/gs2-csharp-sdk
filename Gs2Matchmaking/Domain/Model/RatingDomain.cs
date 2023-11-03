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

    public partial class RatingDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _ratingName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string RatingName => _ratingName;

        public RatingDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string ratingName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._ratingName = ratingName;
            this._parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Rating"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string ratingName,
            string childType
        )
        {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName ?? "null",
                userId ?? "null",
                ratingName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string ratingName
        )
        {
            return string.Join(
                ":",
                ratingName ?? "null"
            );
        }

    }

    public partial class RatingDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Matchmaking.Model.Rating> GetFuture(
            GetRatingByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.Rating> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithRatingName(this.RatingName);
                var future = this._client.GetRatingByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                            request.RatingName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Rating>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "rating")
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Rating"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Model.Rating>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Matchmaking.Model.Rating> GetAsync(
            #else
        private async Task<Gs2.Gs2Matchmaking.Model.Rating> GetAsync(
            #endif
            GetRatingByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithRatingName(this.RatingName);
            GetRatingByUserIdResult result = null;
            try {
                result = await this._client.GetRatingByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                    request.RatingName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Rating>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "rating")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Rating"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain> DeleteFuture(
            DeleteRatingRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithRatingName(this.RatingName);
                var future = this._client.DeleteRatingFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                            request.RatingName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Rating>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "rating")
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Rating"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Matchmaking.Model.Rating>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain> DeleteAsync(
            #endif
            DeleteRatingRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithRatingName(this.RatingName);
            DeleteRatingResult result = null;
            try {
                result = await this._client.DeleteRatingAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                    request.RatingName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Rating>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "rating")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Rating"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Matchmaking.Model.Rating>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain> Delete(
            DeleteRatingRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class RatingDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Model.Rating> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.Rating> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.Rating>(
                    _parentKey,
                    Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                        this.RatingName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetRatingByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                                    this.RatingName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Rating>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "rating")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.Rating>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                            this.RatingName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Model.Rating>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Model.Rating> ModelAsync()
            #else
        public async Task<Gs2.Gs2Matchmaking.Model.Rating> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.Rating>(
                    _parentKey,
                    Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                        this.RatingName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetRatingByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                                    this.RatingName?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.Rating>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "rating")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.Rating>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                            this.RatingName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Matchmaking.Model.Rating> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Model.Rating> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Matchmaking.Model.Rating> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Matchmaking.Model.Rating> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                    this.RatingName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Matchmaking.Model.Rating>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                    this.RatingName.ToString()
                ),
                callbackId
            );
        }

    }
}

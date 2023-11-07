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
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Ranking.Domain.Model
{

    public partial class ScoreDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _categoryName;
        private readonly string _scorerUserId;
        private readonly string _uniqueId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string CategoryName => _categoryName;
        public string ScorerUserId => _scorerUserId;
        public string UniqueId => _uniqueId;

        public ScoreDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string categoryName,
            string scorerUserId,
            string uniqueId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2RankingRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._categoryName = categoryName;
            this._scorerUserId = scorerUserId;
            this._uniqueId = uniqueId;
            this._parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Score"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string categoryName,
            string scorerUserId,
            string uniqueId,
            string childType
        )
        {
            return string.Join(
                ":",
                "ranking",
                namespaceName ?? "null",
                userId ?? "null",
                categoryName ?? "null",
                scorerUserId ?? "null",
                uniqueId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string categoryName,
            string scorerUserId,
            string uniqueId
        )
        {
            return string.Join(
                ":",
                categoryName ?? "null",
                scorerUserId ?? "null",
                uniqueId ?? "null"
            );
        }

    }

    public partial class ScoreDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Ranking.Model.Score> GetFuture(
            GetScoreByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.Score> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithCategoryName(this.CategoryName)
                    .WithScorerUserId(this.ScorerUserId)
                    .WithUniqueId(this.UniqueId);
                var future = this._client.GetScoreByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                            request.CategoryName.ToString(),
                            request.ScorerUserId.ToString(),
                            request.UniqueId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.Score>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "score")
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
                        var parentKey = string.Join(
                            ":",
                            this.NamespaceName,
                            this.UserId,
                            this.CategoryName,
                            resultModel.Item.ScorerUserId,
                            "Score"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                            resultModel.Item.CategoryName.ToString(),
                            resultModel.Item.ScorerUserId.ToString(),
                            resultModel.Item.UniqueId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.Score>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Ranking.Model.Score> GetAsync(
            #else
        private async Task<Gs2.Gs2Ranking.Model.Score> GetAsync(
            #endif
            GetScoreByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithCategoryName(this.CategoryName)
                .WithScorerUserId(this.ScorerUserId)
                .WithUniqueId(this.UniqueId);
            GetScoreByUserIdResult result = null;
            try {
                result = await this._client.GetScoreByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                    request.CategoryName.ToString(),
                    request.ScorerUserId.ToString(),
                    request.UniqueId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.Score>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "score")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = string.Join(
                        ":",
                        this.NamespaceName,
                        this.UserId,
                        this.CategoryName,
                        resultModel.Item.ScorerUserId,
                        "Score"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                        resultModel.Item.CategoryName.ToString(),
                        resultModel.Item.ScorerUserId.ToString(),
                        resultModel.Item.UniqueId.ToString()
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

    }

    public partial class ScoreDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Model.Score> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.Score> self)
            {
                var parentKey = string.Join(
                    ":",
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.ScorerUserId,
                    "Score"
                );
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Ranking.Model.Score>(
                    parentKey,
                    Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                        this.CategoryName?.ToString(),
                        this.ScorerUserId?.ToString(),
                        this.UniqueId?.ToString() ?? "0"
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetScoreByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                                    this.CategoryName?.ToString(),
                                    this.ScorerUserId?.ToString(),
                                    this.UniqueId?.ToString() ?? "0"
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.Score>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "score")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Ranking.Model.Score>(
                        parentKey,
                        Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                            this.CategoryName?.ToString(),
                            this.ScorerUserId?.ToString(),
                            this.UniqueId?.ToString() ?? "0"
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.Score>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Model.Score> ModelAsync()
            #else
        public async Task<Gs2.Gs2Ranking.Model.Score> ModelAsync()
            #endif
        {
            var parentKey = string.Join(
                ":",
                this.NamespaceName,
                this.UserId,
                this.CategoryName,
                this.ScorerUserId,
                "Score"
            );
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Ranking.Model.Score>(
                    parentKey,
                    Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                        this.CategoryName?.ToString(),
                        this.ScorerUserId?.ToString(),
                        this.UniqueId?.ToString() ?? "0"
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetScoreByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                                    this.CategoryName?.ToString(),
                                    this.ScorerUserId?.ToString(),
                                    this.UniqueId?.ToString() ?? "0"
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.Score>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors.Length == 0 || e.errors[0].component != "score")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Ranking.Model.Score>(
                        parentKey,
                        Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                            this.CategoryName?.ToString(),
                            this.ScorerUserId?.ToString(),
                            this.UniqueId?.ToString() ?? "0"
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Ranking.Model.Score> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Ranking.Model.Score> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Ranking.Model.Score> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Ranking.Model.Score> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                    this.CategoryName.ToString(),
                    this.ScorerUserId.ToString(),
                    this.UniqueId.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Ranking.Model.Score>(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                    this.CategoryName.ToString(),
                    this.ScorerUserId.ToString(),
                    this.UniqueId.ToString()
                ),
                callbackId
            );
        }

    }
}

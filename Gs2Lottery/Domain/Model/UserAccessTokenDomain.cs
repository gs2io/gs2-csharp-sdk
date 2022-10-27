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
using Gs2.Gs2Lottery.Domain.Iterator;
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
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

namespace Gs2.Gs2Lottery.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LotteryRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
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
            this._client = new Gs2LotteryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Lottery.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Model.BoxItems> GetBoxAsync(
            #else
        public IFuture<Gs2.Gs2Lottery.Model.BoxItems> GetBox(
            #endif
        #else
        public async Task<Gs2.Gs2Lottery.Model.BoxItems> GetBoxAsync(
        #endif
            GetBoxRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.BoxItems> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetBoxFuture(
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
            var result = await this._client.GetBoxAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BoxItems"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.BoxItems>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Domain.Model.UserAccessTokenDomain> ResetBoxAsync(
            #else
        public IFuture<Gs2.Gs2Lottery.Domain.Model.UserAccessTokenDomain> ResetBox(
            #endif
        #else
        public async Task<Gs2.Gs2Lottery.Domain.Model.UserAccessTokenDomain> ResetBoxAsync(
        #endif
            ResetBoxRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Domain.Model.UserAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ResetBoxFuture(
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
            ResetBoxResult result = null;
            try {
                result = await this._client.ResetBoxAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "box")
                {
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId.ToString(),
                        "BoxItems"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                    );
                    this._cache.Delete<Gs2.Gs2Lottery.Model.BoxItems>(
                        parentKey,
                        key
                    );
                    this._cache.ListCacheClear<Gs2.Gs2Lottery.Model.BoxItems>(
                        parentKey
                    );
                }
                {
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId.ToString(),
                        "Box"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxDomain.CreateCacheKey(
                        request.PrizeTableName
                    );
                    this._cache.Delete<Gs2.Gs2Lottery.Model.Box>(
                        parentKey,
                        key
                    );
                    this._cache.ListCacheClear<Gs2.Gs2Lottery.Model.Box>(
                        parentKey
                    );
                }
            }
            Gs2.Gs2Lottery.Domain.Model.UserAccessTokenDomain domain = this;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Domain.Model.UserAccessTokenDomain>(Impl);
        #endif
        }

        public Gs2.Gs2Lottery.Domain.Model.LotteryAccessTokenDomain Lottery(
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.LotteryAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this._accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Lottery.Model.Probability> Probabilities(
            string lotteryName
        )
        {
            return new DescribeProbabilitiesIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                lotteryName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Lottery.Model.Probability> ProbabilitiesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Lottery.Model.Probability> Probabilities(
            #endif
        #else
        public DescribeProbabilitiesIterator Probabilities(
        #endif
            string lotteryName
        )
        {
            return new DescribeProbabilitiesIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                lotteryName,
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

        public Gs2.Gs2Lottery.Domain.Model.ProbabilityAccessTokenDomain Probability(
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.ProbabilityAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this._accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Lottery.Model.Box> Boxes(
        )
        {
            return new DescribeBoxesIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Lottery.Model.Box> BoxesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Lottery.Model.Box> Boxes(
            #endif
        #else
        public DescribeBoxesIterator Boxes(
        #endif
        )
        {
            return new DescribeBoxesIterator(
                this._cache,
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

        public Gs2.Gs2Lottery.Domain.Model.BoxAccessTokenDomain Box(
            string prizeTableName
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.BoxAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this._accessToken,
                prizeTableName
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
                "lottery",
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

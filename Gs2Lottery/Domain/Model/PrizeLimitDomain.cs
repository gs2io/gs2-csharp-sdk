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

    public partial class PrizeLimitDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LotteryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _prizeTableName;
        private readonly string _prizeId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string PrizeTableName => _prizeTableName;
        public string PrizeId => _prizeId;

        public PrizeLimitDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string prizeTableName,
            string prizeId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LotteryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._prizeTableName = prizeTableName;
            this._prizeId = prizeId;
            this._parentKey = Gs2.Gs2Lottery.Domain.Model.PrizeTableDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.PrizeTableName,
                "PrizeLimit"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Lottery.Model.PrizeLimit> GetAsync(
            #else
        private IFuture<Gs2.Gs2Lottery.Model.PrizeLimit> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Lottery.Model.PrizeLimit> GetAsync(
        #endif
            GetPrizeLimitRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.PrizeLimit> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithPrizeTableName(this.PrizeTableName)
                .WithPrizeId(this.PrizeId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetPrizeLimitFuture(
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
            var result = await this._client.GetPrizeLimitAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.PrizeTableDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.PrizeTableName,
                        "PrizeLimit"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain.CreateCacheKey(
                        resultModel.Item.PrizeId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.PrizeLimit>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain> ResetAsync(
            #else
        public IFuture<Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain> Reset(
            #endif
        #else
        public async Task<Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain> ResetAsync(
        #endif
            ResetPrizeLimitRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithPrizeTableName(this.PrizeTableName)
                .WithPrizeId(this.PrizeId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ResetPrizeLimitFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain.CreateCacheKey(
                        request.PrizeId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Lottery.Model.PrizeLimit>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            ResetPrizeLimitResult result = null;
            try {
                result = await this._client.ResetPrizeLimitAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "prizeLimit")
                {
                    var key = Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain.CreateCacheKey(
                        request.PrizeId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Lottery.Model.PrizeLimit>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
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
                
            }
            Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain domain = this;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string prizeTableName,
            string prizeId,
            string childType
        )
        {
            return string.Join(
                ":",
                "lottery",
                namespaceName ?? "null",
                prizeTableName ?? "null",
                prizeId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string prizeId
        )
        {
            return string.Join(
                ":",
                prizeId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Model.PrizeLimit> Model() {
            #else
        public IFuture<Gs2.Gs2Lottery.Model.PrizeLimit> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Lottery.Model.PrizeLimit> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.PrizeLimit> self)
            {
        #endif
            var (value, find) = _cache.Get<Gs2.Gs2Lottery.Model.PrizeLimit>(
                _parentKey,
                Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain.CreateCacheKey(
                    this.PrizeId?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetPrizeLimitRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain.CreateCacheKey(
                                    this.PrizeId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Lottery.Model.PrizeLimit>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "prizeLimit")
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain.CreateCacheKey(
                            this.PrizeId?.ToString()
                        );
                    _cache.Put<Gs2.Gs2Lottery.Model.PrizeLimit>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "prizeLimit")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Lottery.Model.PrizeLimit>(
                    _parentKey,
                    Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain.CreateCacheKey(
                        this.PrizeId?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.PrizeLimit>(Impl);
        #endif
        }

    }
}

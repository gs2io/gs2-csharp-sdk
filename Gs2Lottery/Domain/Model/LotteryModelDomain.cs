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

namespace Gs2.Gs2Lottery.Domain.Model
{

    public partial class LotteryModelDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LotteryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _lotteryName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string LotteryName => _lotteryName;

        public LotteryModelDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string lotteryName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LotteryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._lotteryName = lotteryName;
            this._parentKey = Gs2.Gs2Lottery.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "LotteryModel"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string lotteryName,
            string childType
        )
        {
            return string.Join(
                ":",
                "lottery",
                namespaceName ?? "null",
                lotteryName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string lotteryName
        )
        {
            return string.Join(
                ":",
                lotteryName ?? "null"
            );
        }

    }

    public partial class LotteryModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Lottery.Model.LotteryModel> GetFuture(
            GetLotteryModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.LotteryModel> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithLotteryName(this.LotteryName);
                var future = this._client.GetLotteryModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                            request.LotteryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Lottery.Model.LotteryModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "lotteryModel")
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
                    .WithLotteryName(this.LotteryName);
                GetLotteryModelResult result = null;
                try {
                    result = await this._client.GetLotteryModelAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                        request.LotteryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Lottery.Model.LotteryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "lotteryModel")
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
                        var parentKey = Gs2.Gs2Lottery.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "LotteryModel"
                        );
                        var key = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.LotteryModel>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Lottery.Model.LotteryModel> GetAsync(
            GetLotteryModelRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithLotteryName(this.LotteryName);
            var future = this._client.GetLotteryModelFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                        request.LotteryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Lottery.Model.LotteryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "lotteryModel")
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
                .WithLotteryName(this.LotteryName);
            GetLotteryModelResult result = null;
            try {
                result = await this._client.GetLotteryModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                    request.LotteryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Lottery.Model.LotteryModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "lotteryModel")
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
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "LotteryModel"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
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

    }

    public partial class LotteryModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lottery.Model.LotteryModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.LotteryModel> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Lottery.Model.LotteryModel>(
                    _parentKey,
                    Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                        this.LotteryName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetLotteryModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                                    this.LotteryName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Lottery.Model.LotteryModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "lotteryModel")
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
                    (value, _) = _cache.Get<Gs2.Gs2Lottery.Model.LotteryModel>(
                        _parentKey,
                        Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                            this.LotteryName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.LotteryModel>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Lottery.Model.LotteryModel> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Lottery.Model.LotteryModel>(
                    _parentKey,
                    Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                        this.LotteryName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetLotteryModelRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                                    this.LotteryName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Lottery.Model.LotteryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "lotteryModel")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Lottery.Model.LotteryModel>(
                        _parentKey,
                        Gs2.Gs2Lottery.Domain.Model.LotteryModelDomain.CreateCacheKey(
                            this.LotteryName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Model.LotteryModel> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Lottery.Model.LotteryModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Lottery.Model.LotteryModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Lottery.Model.LotteryModel> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

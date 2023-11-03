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

    public partial class BoxItemsDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LotteryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _prizeTableName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string PrizeTableName => _prizeTableName;

        public BoxItemsDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string prizeTableName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LotteryRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._prizeTableName = prizeTableName;
            this._parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "BoxItems"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string prizeTableName,
            string childType
        )
        {
            return string.Join(
                ":",
                "lottery",
                namespaceName ?? "null",
                userId ?? "null",
                prizeTableName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string prizeTableName
        )
        {
            return string.Join(
                ":",
                prizeTableName ?? "null"
            );
        }

    }

    public partial class BoxItemsDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Lottery.Model.BoxItems> GetFuture(
            GetBoxByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.BoxItems> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPrizeTableName(this.PrizeTableName);
                var future = this._client.GetBoxByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            request.PrizeTableName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "boxItems")
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
                        var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "BoxItems"
                        );
                        var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            resultModel.Item.PrizeTableName.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.BoxItems>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Lottery.Model.BoxItems> GetAsync(
            #else
        private async Task<Gs2.Gs2Lottery.Model.BoxItems> GetAsync(
            #endif
            GetBoxByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPrizeTableName(this.PrizeTableName);
            GetBoxByUserIdResult result = null;
            try {
                result = await this._client.GetBoxByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                    request.PrizeTableName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "boxItems")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BoxItems"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        resultModel.Item.PrizeTableName.ToString()
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
        public IFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain> ResetBoxFuture(
            ResetBoxByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPrizeTableName(this.PrizeTableName);
                var future = this._client.ResetBoxByUserIdFuture(
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId.ToString(),
                            "BoxItems"
                        );
                        var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            requestModel.PrizeTableName
                        );
                        cache.Delete<Gs2.Gs2Lottery.Model.BoxItems>(
                            parentKey,
                            key
                        );
                        cache.ClearListCache<Gs2.Gs2Lottery.Model.BoxItems>(
                            parentKey
                        );
                    }
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain> ResetBoxAsync(
            #else
        public async Task<Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain> ResetBoxAsync(
            #endif
            ResetBoxByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPrizeTableName(this.PrizeTableName);
            ResetBoxByUserIdResult result = null;
                result = await this._client.ResetBoxByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                {
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId.ToString(),
                        "BoxItems"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        requestModel.PrizeTableName
                    );
                    cache.Delete<Gs2.Gs2Lottery.Model.BoxItems>(
                        parentKey,
                        key
                    );
                    cache.ClearListCache<Gs2.Gs2Lottery.Model.BoxItems>(
                        parentKey
                    );
                }
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to ResetBoxFuture.")]
        public IFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain> ResetBox(
            ResetBoxByUserIdRequest request
        ) {
            return ResetBoxFuture(request);
        }
        #endif

    }

    public partial class BoxItemsDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lottery.Model.BoxItems> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.BoxItems> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Lottery.Model.BoxItems>(
                    _parentKey,
                    Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        this.PrizeTableName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetBoxByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                                    this.PrizeTableName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "boxItems")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Lottery.Model.BoxItems>(
                        _parentKey,
                        Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            this.PrizeTableName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.BoxItems>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Lottery.Model.BoxItems> ModelAsync()
            #else
        public async Task<Gs2.Gs2Lottery.Model.BoxItems> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Lottery.Model.BoxItems>(
                    _parentKey,
                    Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        this.PrizeTableName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetBoxByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                                    this.PrizeTableName?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "boxItems")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Lottery.Model.BoxItems>(
                        _parentKey,
                        Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            this.PrizeTableName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Lottery.Model.BoxItems> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Lottery.Model.BoxItems> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Lottery.Model.BoxItems> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Lottery.Model.BoxItems> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                    this.PrizeTableName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Lottery.Model.BoxItems>(
                _parentKey,
                Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                    this.PrizeTableName.ToString()
                ),
                callbackId
            );
        }

    }
}

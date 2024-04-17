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
using Gs2.Gs2Lottery.Model.Cache;
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Lottery.Domain.Model
{

    public partial class BoxItemsAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LotteryRestClient _client;
        public string NamespaceName { get; }
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string PrizeTableName { get; }

        public BoxItemsAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string prizeTableName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LotteryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.PrizeTableName = prizeTableName;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Lottery.Model.BoxItems> GetFuture(
            GetBoxRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.BoxItems> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithPrizeTableName(this.PrizeTableName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetBoxFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            GetBoxRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithPrizeTableName(this.PrizeTableName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetBoxAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> ResetBoxFuture(
            ResetBoxRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithPrizeTableName(this.PrizeTableName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ResetBoxFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                _gs2.Cache.ClearListCache<Gs2.Gs2Lottery.Model.BoxItems>(
                    (null as Gs2.Gs2Lottery.Model.BoxItems).CacheParentKey(
                        this.NamespaceName,
                        this.UserId
                    )
                );
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> ResetBoxAsync(
            #else
        public async Task<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> ResetBoxAsync(
            #endif
            ResetBoxRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithPrizeTableName(this.PrizeTableName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ResetBoxAsync(request)
            );
            _gs2.Cache.ClearListCache<Gs2.Gs2Lottery.Model.BoxItems>(
                (null as Gs2.Gs2Lottery.Model.BoxItems).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                )
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lottery.Model.BoxItems> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.BoxItems> self)
            {
                var (value, find) = (null as Gs2.Gs2Lottery.Model.BoxItems).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.PrizeTableName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Lottery.Model.BoxItems).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.PrizeTableName,
                    () => this.GetFuture(
                        new GetBoxRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
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
            var (value, find) = (null as Gs2.Gs2Lottery.Model.BoxItems).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.PrizeTableName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Lottery.Model.BoxItems).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.PrizeTableName,
                () => this.GetAsync(
                    new GetBoxRequest()
                )
            );
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


        public void Invalidate()
        {
            (null as Gs2.Gs2Lottery.Model.BoxItems).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.PrizeTableName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Lottery.Model.BoxItems> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Lottery.Model.BoxItems).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Lottery.Model.BoxItems).CacheKey(
                    this.PrizeTableName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Lottery.Model.BoxItems>(
                (null as Gs2.Gs2Lottery.Model.BoxItems).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Lottery.Model.BoxItems).CacheKey(
                    this.PrizeTableName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Lottery.Model.BoxItems> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Lottery.Model.BoxItems> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Lottery.Model.BoxItems> callback)
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

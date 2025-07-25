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

    public partial class PrizeTableDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LotteryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string PrizeTableName { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public PrizeTableDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string prizeTableName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LotteryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.PrizeTableName = prizeTableName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Lottery.Model.PrizeLimit> PrizeLimits(
        )
        {
            return new DescribePrizeLimitsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.PrizeTableName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Lottery.Model.PrizeLimit> PrizeLimitsAsync(
            #else
        public DescribePrizeLimitsIterator PrizeLimitsAsync(
            #endif
        )
        {
            return new DescribePrizeLimitsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.PrizeTableName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribePrizeLimits(
            Action<Gs2.Gs2Lottery.Model.PrizeLimit[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Lottery.Model.PrizeLimit>(
                (null as Gs2.Gs2Lottery.Model.PrizeLimit).CacheParentKey(
                    this.NamespaceName,
                    this.PrizeTableName,
                    null
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await PrizeLimitsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribePrizeLimitsWithInitialCallAsync(
            Action<Gs2.Gs2Lottery.Model.PrizeLimit[]> callback
        )
        {
            var items = await PrizeLimitsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribePrizeLimits(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribePrizeLimits(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Lottery.Model.PrizeLimit>(
                (null as Gs2.Gs2Lottery.Model.PrizeLimit).CacheParentKey(
                    this.NamespaceName,
                    this.PrizeTableName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidatePrizeLimits(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Lottery.Model.PrizeLimit>(
                (null as Gs2.Gs2Lottery.Model.PrizeLimit).CacheParentKey(
                    this.NamespaceName,
                    this.PrizeTableName,
                    null
                )
            );
        }

        public Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain PrizeLimit(
            string prizeId
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.PrizeLimitDomain(
                this._gs2,
                this.NamespaceName,
                this.PrizeTableName,
                prizeId
            );
        }

    }

    public partial class PrizeTableDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Lottery.Model.PrizeTable> GetFuture(
            GetPrizeTableRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.PrizeTable> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithPrizeTableName(this.PrizeTableName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.GetPrizeTableFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.PrizeTable>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Lottery.Model.PrizeTable> GetAsync(
            #else
        private async Task<Gs2.Gs2Lottery.Model.PrizeTable> GetAsync(
            #endif
            GetPrizeTableRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithPrizeTableName(this.PrizeTableName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetPrizeTableAsync(request)
            );
            return result?.Item;
        }
        #endif

    }

    public partial class PrizeTableDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lottery.Model.PrizeTable> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.PrizeTable> self)
            {
                var (value, find) = (null as Gs2.Gs2Lottery.Model.PrizeTable).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.PrizeTableName,
                    null
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Lottery.Model.PrizeTable).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.PrizeTableName,
                    null,
                    () => this.GetFuture(
                        new GetPrizeTableRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.PrizeTable>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Lottery.Model.PrizeTable> ModelAsync()
            #else
        public async Task<Gs2.Gs2Lottery.Model.PrizeTable> ModelAsync()
            #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Lottery.Model.PrizeTable>(
                        (null as Gs2.Gs2Lottery.Model.PrizeTable).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Lottery.Model.PrizeTable).CacheKey(
                            this.PrizeTableName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Lottery.Model.PrizeTable).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.PrizeTableName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Lottery.Model.PrizeTable).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.PrizeTableName,
                    null,
                    () => this.GetAsync(
                        new GetPrizeTableRequest()
                    )
                );
            }
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Lottery.Model.PrizeTable> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Lottery.Model.PrizeTable> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Lottery.Model.PrizeTable> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Lottery.Model.PrizeTable).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.PrizeTableName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Lottery.Model.PrizeTable> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Lottery.Model.PrizeTable).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Lottery.Model.PrizeTable).CacheKey(
                    this.PrizeTableName
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Lottery.Model.PrizeTable>(
                (null as Gs2.Gs2Lottery.Model.PrizeTable).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Lottery.Model.PrizeTable).CacheKey(
                    this.PrizeTableName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Lottery.Model.PrizeTable> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Lottery.Model.PrizeTable> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Lottery.Model.PrizeTable> callback)
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

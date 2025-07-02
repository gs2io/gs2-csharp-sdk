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
using Gs2.Gs2Freeze.Domain.Iterator;
using Gs2.Gs2Freeze.Model.Cache;
using Gs2.Gs2Freeze.Request;
using Gs2.Gs2Freeze.Result;
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

namespace Gs2.Gs2Freeze.Domain.Model
{

    public partial class StageDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FreezeRestClient _client;
        public string StageName { get; } = null!;
        public Gs2.Gs2Freeze.Model.Microservice[] Source { get; set; } = null!;
        public Gs2.Gs2Freeze.Model.Microservice[] Current { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public StageDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string stageName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FreezeRestClient(
                gs2.RestSession
            );
            this.StageName = stageName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Freeze.Model.Output> Outputs(
        )
        {
            return new DescribeOutputsIterator(
                this._gs2,
                this._client,
                this.StageName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Freeze.Model.Output> OutputsAsync(
            #else
        public DescribeOutputsIterator OutputsAsync(
            #endif
        )
        {
            return new DescribeOutputsIterator(
                this._gs2,
                this._client,
                this.StageName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeOutputs(
            Action<Gs2.Gs2Freeze.Model.Output[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Freeze.Model.Output>(
                (null as Gs2.Gs2Freeze.Model.Output).CacheParentKey(
                    this.StageName,
                    null
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await OutputsAsync(
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
        public async UniTask<ulong> SubscribeOutputsWithInitialCallAsync(
            Action<Gs2.Gs2Freeze.Model.Output[]> callback
        )
        {
            var items = await OutputsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeOutputs(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeOutputs(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Freeze.Model.Output>(
                (null as Gs2.Gs2Freeze.Model.Output).CacheParentKey(
                    this.StageName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateOutputs(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Freeze.Model.Output>(
                (null as Gs2.Gs2Freeze.Model.Output).CacheParentKey(
                    this.StageName,
                    null
                )
            );
        }

        public Gs2.Gs2Freeze.Domain.Model.OutputDomain Output(
            string outputName
        ) {
            return new Gs2.Gs2Freeze.Domain.Model.OutputDomain(
                this._gs2,
                this.StageName,
                outputName
            );
        }

    }

    public partial class StageDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Freeze.Model.Stage> GetFuture(
            GetStageRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Freeze.Model.Stage> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStageName(this.StageName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.GetStageFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Freeze.Model.Stage>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Freeze.Model.Stage> GetAsync(
            #else
        private async Task<Gs2.Gs2Freeze.Model.Stage> GetAsync(
            #endif
            GetStageRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStageName(this.StageName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetStageAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Freeze.Domain.Model.StageDomain> PromoteFuture(
            PromoteStageRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Freeze.Domain.Model.StageDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStageName(this.StageName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.PromoteStageFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Freeze.Domain.Model.StageDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Freeze.Domain.Model.StageDomain> PromoteAsync(
            #else
        public async Task<Gs2.Gs2Freeze.Domain.Model.StageDomain> PromoteAsync(
            #endif
            PromoteStageRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStageName(this.StageName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.PromoteStageAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Freeze.Domain.Model.StageDomain> RollbackFuture(
            RollbackStageRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Freeze.Domain.Model.StageDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStageName(this.StageName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.RollbackStageFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Freeze.Domain.Model.StageDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Freeze.Domain.Model.StageDomain> RollbackAsync(
            #else
        public async Task<Gs2.Gs2Freeze.Domain.Model.StageDomain> RollbackAsync(
            #endif
            RollbackStageRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStageName(this.StageName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.RollbackStageAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

    }

    public partial class StageDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Freeze.Model.Stage> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Freeze.Model.Stage> self)
            {
                var (value, find) = (null as Gs2.Gs2Freeze.Model.Stage).GetCache(
                    this._gs2.Cache,
                    this.StageName,
                    null
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Freeze.Model.Stage).FetchFuture(
                    this._gs2.Cache,
                    this.StageName,
                    null,
                    () => this.GetFuture(
                        new GetStageRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Freeze.Model.Stage>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Freeze.Model.Stage> ModelAsync()
            #else
        public async Task<Gs2.Gs2Freeze.Model.Stage> ModelAsync()
            #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Freeze.Model.Stage>(
                        (null as Gs2.Gs2Freeze.Model.Stage).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Freeze.Model.Stage).CacheKey(
                            this.StageName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Freeze.Model.Stage).GetCache(
                    this._gs2.Cache,
                    this.StageName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Freeze.Model.Stage).FetchAsync(
                    this._gs2.Cache,
                    this.StageName,
                    null,
                    () => this.GetAsync(
                        new GetStageRequest()
                    )
                );
            }
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Freeze.Model.Stage> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Freeze.Model.Stage> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Freeze.Model.Stage> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Freeze.Model.Stage).DeleteCache(
                this._gs2.Cache,
                this.StageName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Freeze.Model.Stage> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Freeze.Model.Stage).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Freeze.Model.Stage).CacheKey(
                    this.StageName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Freeze.Model.Stage>(
                (null as Gs2.Gs2Freeze.Model.Stage).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Freeze.Model.Stage).CacheKey(
                    this.StageName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Freeze.Model.Stage> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Freeze.Model.Stage> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Freeze.Model.Stage> callback)
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

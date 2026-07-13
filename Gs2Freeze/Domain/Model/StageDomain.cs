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
using System.Collections;
using System.Collections.Generic;
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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
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
            );
        }

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
        #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        #else
                    async Task Impl() {
        #endif
                        try {
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await OutputsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeOutputsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeOutputsWithInitialCallAsync(
        #endif
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
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Freeze.Domain.Model.StageDomain> PromoteFuture(
            PromoteStageRequest request
        ) => PromoteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Freeze.Domain.Model.StageDomain> RollbackFuture(
            RollbackStageRequest request
        ) => RollbackAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

    }

    public partial class StageDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Freeze.Model.Stage> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Freeze.Model.Stage> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Freeze.Model.Stage> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Freeze.Model.Stage> Model() => ModelAsync();
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
                    Impl().Forget();
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
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Freeze.Model.Stage> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

    }
}

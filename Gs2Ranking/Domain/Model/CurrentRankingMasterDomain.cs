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
using Gs2.Gs2Ranking.Model.Cache;
using Gs2.Gs2Ranking.Request;
using Gs2.Gs2Ranking.Result;
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

namespace Gs2.Gs2Ranking.Domain.Model
{

    public partial class CurrentRankingMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2RankingRestClient _client;
        public string NamespaceName { get; }

        public CurrentRankingMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2RankingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

    }

    public partial class CurrentRankingMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> ExportMasterFuture(
            ExportMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.ExportMasterFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> ExportMasterAsync(
            #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> ExportMasterAsync(
            #endif
            ExportMasterRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.ExportMasterAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Ranking.Model.CurrentRankingMaster> GetFuture(
            GetCurrentRankingMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.CurrentRankingMaster> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetCurrentRankingMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.CurrentRankingMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Ranking.Model.CurrentRankingMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Ranking.Model.CurrentRankingMaster> GetAsync(
            #endif
            GetCurrentRankingMasterRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetCurrentRankingMasterAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> UpdateFuture(
            UpdateCurrentRankingMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateCurrentRankingMasterFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> UpdateAsync(
            #endif
            UpdateCurrentRankingMasterRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateCurrentRankingMasterAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> UpdateFromGitHubFuture(
            UpdateCurrentRankingMasterFromGitHubRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateCurrentRankingMasterFromGitHubFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> UpdateFromGitHubAsync(
            #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.CurrentRankingMasterDomain> UpdateFromGitHubAsync(
            #endif
            UpdateCurrentRankingMasterFromGitHubRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateCurrentRankingMasterFromGitHubAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

    }

    public partial class CurrentRankingMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Model.CurrentRankingMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.CurrentRankingMaster> self)
            {
                var (value, find) = (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    () => this.GetFuture(
                        new GetCurrentRankingMasterRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.CurrentRankingMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Model.CurrentRankingMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Ranking.Model.CurrentRankingMaster> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).GetCache(
                this._gs2.Cache,
                this.NamespaceName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                () => this.GetAsync(
                    new GetCurrentRankingMasterRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Ranking.Model.CurrentRankingMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Ranking.Model.CurrentRankingMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Ranking.Model.CurrentRankingMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Ranking.Model.CurrentRankingMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Ranking.Model.CurrentRankingMaster>(
                (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Ranking.Model.CurrentRankingMaster).CacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Ranking.Model.CurrentRankingMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Ranking.Model.CurrentRankingMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Ranking.Model.CurrentRankingMaster> callback)
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

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
using Gs2.Gs2SeasonRating.Domain.Iterator;
using Gs2.Gs2SeasonRating.Model.Cache;
using Gs2.Gs2SeasonRating.Request;
using Gs2.Gs2SeasonRating.Result;
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

namespace Gs2.Gs2SeasonRating.Domain.Model
{

    public partial class VoteDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SeasonRatingRestClient _client;
        public string NamespaceName { get; } = null!;
        public string SeasonName { get; } = null!;
        public string SessionName { get; } = null!;

        public VoteDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string seasonName,
            string sessionName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SeasonRatingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.SeasonName = seasonName;
            this.SessionName = sessionName;
        }

    }

    public partial class VoteDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.VoteDomain> CommitFuture(
            CommitVoteRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Domain.Model.VoteDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithSeasonName(this.SeasonName)
                    .WithSessionName(this.SessionName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CommitVoteFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Domain.Model.VoteDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.VoteDomain> CommitAsync(
            #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.VoteDomain> CommitAsync(
            #endif
            CommitVoteRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithSeasonName(this.SeasonName)
                .WithSessionName(this.SessionName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CommitVoteAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

    }

    public partial class VoteDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Model.Vote> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Model.Vote> self)
            {
                var (value, find) = (null as Gs2.Gs2SeasonRating.Model.Vote).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.SeasonName,
                    this.SessionName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                self.OnComplete(null);
            }
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Model.Vote>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Model.Vote> ModelAsync()
            #else
        public async Task<Gs2.Gs2SeasonRating.Model.Vote> ModelAsync()
            #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2SeasonRating.Model.Vote>(
                        (null as Gs2.Gs2SeasonRating.Model.Vote).CacheParentKey(
                            this.NamespaceName
                        ),
                        (null as Gs2.Gs2SeasonRating.Model.Vote).CacheKey(
                            this.SeasonName,
                            this.SessionName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2SeasonRating.Model.Vote).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.SeasonName,
                    this.SessionName
                );
                if (find) {
                    return value;
                }
                return null;
            }
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2SeasonRating.Model.Vote> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2SeasonRating.Model.Vote> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2SeasonRating.Model.Vote> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2SeasonRating.Model.Vote).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.SeasonName,
                this.SessionName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2SeasonRating.Model.Vote> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2SeasonRating.Model.Vote).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2SeasonRating.Model.Vote).CacheKey(
                    this.SeasonName,
                    this.SessionName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2SeasonRating.Model.Vote>(
                (null as Gs2.Gs2SeasonRating.Model.Vote).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2SeasonRating.Model.Vote).CacheKey(
                    this.SeasonName,
                    this.SessionName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2SeasonRating.Model.Vote> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SeasonRating.Model.Vote> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SeasonRating.Model.Vote> callback)
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

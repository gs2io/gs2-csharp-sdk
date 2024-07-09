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
 *
 * deny overwrite
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
using Gs2.Gs2JobQueue.Domain.Iterator;
using Gs2.Gs2JobQueue.Model.Cache;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
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

namespace Gs2.Gs2JobQueue.Domain.Model
{

    public partial class JobResultAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2JobQueueRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string JobName { get; } = null!;
        public int? TryNumber { get; } = null!;

        public JobResultAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string jobName,
            int? tryNumber
        ) {
            this._gs2 = gs2;
            this._client = new Gs2JobQueueRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.JobName = jobName;
            this.TryNumber = tryNumber;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2JobQueue.Model.JobResult> GetFuture(
            GetJobResultRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Model.JobResult> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithJobName(this.JobName)
                    .WithTryNumber(this.TryNumber);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetJobResultFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Model.JobResult>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2JobQueue.Model.JobResult> GetAsync(
            #else
        private async Task<Gs2.Gs2JobQueue.Model.JobResult> GetAsync(
            #endif
            GetJobResultRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithJobName(this.JobName)
                .WithTryNumber(this.TryNumber);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetJobResultAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Model.JobResult> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Model.JobResult> self)
            {
                var (value, find) = (null as Gs2.Gs2JobQueue.Model.JobResult).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.JobName,
                    this.TryNumber ?? default
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2JobQueue.Model.JobResult).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.JobName,
                    this.TryNumber ?? default,
                    () => this.GetFuture(
                        new GetJobResultRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Model.JobResult>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2JobQueue.Model.JobResult> ModelAsync()
            #else
        public async Task<Gs2.Gs2JobQueue.Model.JobResult> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2JobQueue.Model.JobResult).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.JobName,
                this.TryNumber ?? default
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2JobQueue.Model.JobResult).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.JobName,
                this.TryNumber ?? default,
                () => this.GetAsync(
                    new GetJobResultRequest()
                )
            );
        }
        #endif
        
        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Model.JobResult> ModelNoCacheFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Model.JobResult> self)
            {
                var future = (null as Gs2.Gs2JobQueue.Model.JobResult).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.JobName,
                    this.TryNumber ?? default,
                    () => this.GetFuture(
                        new GetJobResultRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Model.JobResult>(Impl);
        }
        #endif
        
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2JobQueue.Model.JobResult> ModelNoCacheAsync()
            #else
        public async Task<Gs2.Gs2JobQueue.Model.JobResult> ModelNoCacheAsync()
            #endif
        {
            return await (null as Gs2.Gs2JobQueue.Model.JobResult).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.JobName,
                this.TryNumber ?? default,
                () => this.GetAsync(
                    new GetJobResultRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2JobQueue.Model.JobResult> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2JobQueue.Model.JobResult> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2JobQueue.Model.JobResult> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2JobQueue.Model.JobResult).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.JobName,
                this.TryNumber ?? default
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2JobQueue.Model.JobResult> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2JobQueue.Model.JobResult).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.JobName
                ),
                (null as Gs2.Gs2JobQueue.Model.JobResult).CacheKey(
                    this.TryNumber ?? default
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2JobQueue.Model.JobResult>(
                (null as Gs2.Gs2JobQueue.Model.JobResult).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.JobName
                ),
                (null as Gs2.Gs2JobQueue.Model.JobResult).CacheKey(
                    this.TryNumber ?? default
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2JobQueue.Model.JobResult> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2JobQueue.Model.JobResult> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2JobQueue.Model.JobResult> callback)
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

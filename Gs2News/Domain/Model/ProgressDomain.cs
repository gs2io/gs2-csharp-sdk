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
using Gs2.Gs2News.Domain.Iterator;
using Gs2.Gs2News.Model.Cache;
using Gs2.Gs2News.Request;
using Gs2.Gs2News.Result;
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

namespace Gs2.Gs2News.Domain.Model
{

    public partial class ProgressDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2NewsRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UploadToken { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public ProgressDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string uploadToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2NewsRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UploadToken = uploadToken;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2News.Model.Output> Outputs(
        )
        {
            return new DescribeOutputsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UploadToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2News.Model.Output> OutputsAsync(
            #else
        public DescribeOutputsIterator OutputsAsync(
            #endif
        )
        {
            return new DescribeOutputsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UploadToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeOutputs(
            Action<Gs2.Gs2News.Model.Output[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2News.Model.Output>(
                (null as Gs2.Gs2News.Model.Output).CacheParentKey(
                    this.NamespaceName,
                    this.UploadToken
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeOutputsWithInitialCallAsync(
            Action<Gs2.Gs2News.Model.Output[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2News.Model.Output>(
                (null as Gs2.Gs2News.Model.Output).CacheParentKey(
                    this.NamespaceName,
                    this.UploadToken
                ),
                callbackId
            );
        }

        public Gs2.Gs2News.Domain.Model.OutputDomain Output(
            string outputName
        ) {
            return new Gs2.Gs2News.Domain.Model.OutputDomain(
                this._gs2,
                this.NamespaceName,
                this.UploadToken,
                outputName
            );
        }

    }

    public partial class ProgressDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2News.Model.Progress> GetFuture(
            GetProgressRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2News.Model.Progress> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUploadToken(this.UploadToken);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetProgressFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Model.Progress>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2News.Model.Progress> GetAsync(
            #else
        private async Task<Gs2.Gs2News.Model.Progress> GetAsync(
            #endif
            GetProgressRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUploadToken(this.UploadToken);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetProgressAsync(request)
            );
            return result?.Item;
        }
        #endif

    }

    public partial class ProgressDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2News.Model.Progress> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2News.Model.Progress> self)
            {
                var (value, find) = (null as Gs2.Gs2News.Model.Progress).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UploadToken
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2News.Model.Progress).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UploadToken,
                    () => this.GetFuture(
                        new GetProgressRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Model.Progress>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2News.Model.Progress> ModelAsync()
            #else
        public async Task<Gs2.Gs2News.Model.Progress> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2News.Model.Progress).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UploadToken
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2News.Model.Progress).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UploadToken,
                () => this.GetAsync(
                    new GetProgressRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2News.Model.Progress> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2News.Model.Progress> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2News.Model.Progress> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2News.Model.Progress).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UploadToken
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2News.Model.Progress> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2News.Model.Progress).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2News.Model.Progress).CacheKey(
                    this.UploadToken
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2News.Model.Progress>(
                (null as Gs2.Gs2News.Model.Progress).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2News.Model.Progress).CacheKey(
                    this.UploadToken
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2News.Model.Progress> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2News.Model.Progress> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2News.Model.Progress> callback)
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

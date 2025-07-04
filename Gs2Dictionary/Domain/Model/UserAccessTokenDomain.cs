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
using Gs2.Gs2Dictionary.Domain.Iterator;
using Gs2.Gs2Dictionary.Model.Cache;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Result;
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

namespace Gs2.Gs2Dictionary.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DictionaryRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string NextPageToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DictionaryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain[]> DeleteEntriesFuture(
            DeleteEntriesRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain[]> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteEntriesFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain[]> DeleteEntriesAsync(
            #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain[]> DeleteEntriesAsync(
            #endif
            DeleteEntriesRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteEntriesAsync(request)
                );
                var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain>();
                return domain;
            }
            catch (NotFoundException e) {}
            return Array.Empty<EntryAccessTokenDomain>();
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]> AddLikesFuture(
            AddLikesRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.AddLikesFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]> AddLikesAsync(
            #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]> AddLikesAsync(
            #endif
            AddLikesRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.AddLikesAsync(request)
            );
            var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                v?.Name
            )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain>();
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.UserAccessTokenDomain> ResetLikesFuture(
            ResetLikesRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.UserAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.ResetLikesFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.UserAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.UserAccessTokenDomain> ResetLikesAsync(
            #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.UserAccessTokenDomain> ResetLikesAsync(
            #endif
            ResetLikesRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.ResetLikesAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]> DeleteLikesFuture(
            DeleteLikesRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteLikesFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]> DeleteLikesAsync(
            #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain[]> DeleteLikesAsync(
            #endif
            DeleteLikesRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteLikesAsync(request)
                );
                var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain>();
                return domain;
            }
            catch (NotFoundException e) {}
            return Array.Empty<LikeAccessTokenDomain>();
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Dictionary.Model.Entry> Entries(
        )
        {
            return new DescribeEntriesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Dictionary.Model.Entry> EntriesAsync(
            #else
        public DescribeEntriesIterator EntriesAsync(
            #endif
        )
        {
            return new DescribeEntriesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeEntries(
            Action<Gs2.Gs2Dictionary.Model.Entry[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Dictionary.Model.Entry>(
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await EntriesAsync().ToArrayAsync());
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
        public async UniTask<ulong> SubscribeEntriesWithInitialCallAsync(
            Action<Gs2.Gs2Dictionary.Model.Entry[]> callback
        )
        {
            var items = await EntriesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeEntries(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeEntries(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Dictionary.Model.Entry>(
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateEntries(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Dictionary.Model.Entry>(
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain Entry(
            string entryModelName
        ) {
            return new Gs2.Gs2Dictionary.Domain.Model.EntryAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                entryModelName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Dictionary.Model.Like> Likes(
        )
        {
            return new DescribeLikesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Dictionary.Model.Like> LikesAsync(
            #else
        public DescribeLikesIterator LikesAsync(
            #endif
        )
        {
            return new DescribeLikesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeLikes(
            Action<Gs2.Gs2Dictionary.Model.Like[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Dictionary.Model.Like>(
                (null as Gs2.Gs2Dictionary.Model.Like).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await LikesAsync().ToArrayAsync());
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
        public async UniTask<ulong> SubscribeLikesWithInitialCallAsync(
            Action<Gs2.Gs2Dictionary.Model.Like[]> callback
        )
        {
            var items = await LikesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeLikes(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeLikes(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Dictionary.Model.Like>(
                (null as Gs2.Gs2Dictionary.Model.Like).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateLikes(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Dictionary.Model.Like>(
                (null as Gs2.Gs2Dictionary.Model.Like).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain Like(
            string entryModelName
        ) {
            return new Gs2.Gs2Dictionary.Domain.Model.LikeAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                entryModelName
            );
        }

    }
}

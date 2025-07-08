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
using Gs2.Gs2Chat.Domain.Iterator;
using Gs2.Gs2Chat.Model.Cache;
using Gs2.Gs2Chat.Request;
using Gs2.Gs2Chat.Result;
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

namespace Gs2.Gs2Chat.Domain.Model
{

    public partial class RoomAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ChatRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string RoomName { get; } = null!;
        public string Password { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public RoomAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string roomName,
            string password
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ChatRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.RoomName = roomName;
            this.Password = password;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> UpdateFuture(
            UpdateRoomRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName)
                    .WithPassword(this.Password)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.UpdateRoomFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> UpdateAsync(
            #endif
            UpdateRoomRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName)
                .WithPassword(this.Password)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.UpdateRoomAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> DeleteFuture(
            DeleteRoomRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteRoomFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> DeleteAsync(
            #endif
            DeleteRoomRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName)
                    .WithAccessToken(this.AccessToken?.Token);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteRoomAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain> PostFuture(
            PostRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithPassword(this.Password);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.PostFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    result?.Item?.RoomName,
                    request.Password,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain> PostAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain> PostAsync(
            #endif
            PostRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithPassword(this.Password);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.PostAsync(request)
            );
            var domain = new Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.RoomName,
                request.Password,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Chat.Model.Room> GetFuture(
            GetRoomRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Room> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.GetRoomFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Model.Room>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Chat.Model.Room> GetAsync(
            #else
        private async Task<Gs2.Gs2Chat.Model.Room> GetAsync(
            #endif
            GetRoomRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetRoomAsync(request)
            );
            return result?.Item;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Chat.Model.Message> Messages(
            int? category = null
        )
        {
            return new DescribeMessagesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.RoomName,
                this.Password,
                category
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Chat.Model.Message> MessagesAsync(
            #else
        public DescribeMessagesIterator MessagesAsync(
            #endif
            int? category = null
        )
        {
            return new DescribeMessagesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.RoomName,
                this.Password,
                category
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeMessages(
            Action<Gs2.Gs2Chat.Model.Message[]> callback,
            int? category = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Chat.Model.Message>(
                (null as Gs2.Gs2Chat.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await MessagesAsync(
                                category
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
        public async UniTask<ulong> SubscribeMessagesWithInitialCallAsync(
            Action<Gs2.Gs2Chat.Model.Message[]> callback,
            int? category = null
        )
        {
            var items = await MessagesAsync(
                category
            ).ToArrayAsync();
            var callbackId = SubscribeMessages(
                callback,
                category
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeMessages(
            ulong callbackId,
            int? category = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Chat.Model.Message>(
                (null as Gs2.Gs2Chat.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateMessages(
            int? category = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Chat.Model.Message>(
                (null as Gs2.Gs2Chat.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset
                )
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Chat.Model.Message> LatestMessages(
            int? category = null
        )
        {
            return new DescribeLatestMessagesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.RoomName,
                this.Password,
                category
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Chat.Model.Message> LatestMessagesAsync(
            #else
        public DescribeLatestMessagesIterator LatestMessagesAsync(
            #endif
            int? category = null
        )
        {
            return new DescribeLatestMessagesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.RoomName,
                this.Password,
                category
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeLatestMessages(
            Action<Gs2.Gs2Chat.Model.Message[]> callback,
            int? category = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Chat.Model.Message>(
                (null as Gs2.Gs2Chat.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await LatestMessagesAsync(
                                category
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
        public async UniTask<ulong> SubscribeLatestMessagesWithInitialCallAsync(
            Action<Gs2.Gs2Chat.Model.Message[]> callback,
            int? category = null
        )
        {
            var items = await LatestMessagesAsync(
                category
            ).ToArrayAsync();
            var callbackId = SubscribeLatestMessages(
                callback,
                category
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeLatestMessages(
            ulong callbackId,
            int? category = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Chat.Model.Message>(
                (null as Gs2.Gs2Chat.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateLatestMessages(
            int? category = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Chat.Model.Message>(
                (null as Gs2.Gs2Chat.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain Message(
            string messageName
        ) {
            return new Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.RoomName,
                this.Password,
                messageName
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Model.Room> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Room> self)
            {
                var (value, find) = (null as Gs2.Gs2Chat.Model.Room).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Chat.Model.Room).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset,
                    () => this.GetFuture(
                        new GetRoomRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Model.Room>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Model.Room> ModelAsync()
            #else
        public async Task<Gs2.Gs2Chat.Model.Room> ModelAsync()
            #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Chat.Model.Room>(
                        (null as Gs2.Gs2Chat.Model.Room).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Chat.Model.Room).CacheKey(
                            this.RoomName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Chat.Model.Room).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Chat.Model.Room).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetRoomRequest()
                    )
                );
            }
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Chat.Model.Room> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Chat.Model.Room> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Chat.Model.Room> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Chat.Model.Room).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.RoomName,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Chat.Model.Room> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Chat.Model.Room).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Chat.Model.Room).CacheKey(
                    this.RoomName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Chat.Model.Room>(
                (null as Gs2.Gs2Chat.Model.Room).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Chat.Model.Room).CacheKey(
                    this.RoomName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Chat.Model.Room> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Chat.Model.Room> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Chat.Model.Room> callback)
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

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

    public partial class SubscribeDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ChatRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string RoomName { get; } = null!;

        public SubscribeDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string roomName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ChatRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.RoomName = roomName;
        }

    }

    public partial class SubscribeDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> SubscribeFuture(
            SubscribeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.SubscribeByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> SubscribeAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> SubscribeAsync(
            #endif
            SubscribeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SubscribeByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Chat.Model.Subscribe> GetFuture(
            GetSubscribeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Subscribe> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.GetSubscribeByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Model.Subscribe>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Chat.Model.Subscribe> GetAsync(
            #else
        private async Task<Gs2.Gs2Chat.Model.Subscribe> GetAsync(
            #endif
            GetSubscribeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetSubscribeByUserIdAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> UpdateNotificationTypeFuture(
            UpdateNotificationTypeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.UpdateNotificationTypeByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> UpdateNotificationTypeAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> UpdateNotificationTypeAsync(
            #endif
            UpdateNotificationTypeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.UpdateNotificationTypeByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> UnsubscribeFuture(
            UnsubscribeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.UnsubscribeByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.SubscribeDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> UnsubscribeAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.SubscribeDomain> UnsubscribeAsync(
            #endif
            UnsubscribeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.UnsubscribeByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

    }

    public partial class SubscribeDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Model.Subscribe> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Subscribe> self)
            {
                var (value, find) = (null as Gs2.Gs2Chat.Model.Subscribe).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    null
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Chat.Model.Subscribe).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    null,
                    () => this.GetFuture(
                        new GetSubscribeByUserIdRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Model.Subscribe>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Model.Subscribe> ModelAsync()
            #else
        public async Task<Gs2.Gs2Chat.Model.Subscribe> ModelAsync()
            #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Chat.Model.Subscribe>(
                        (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            null
                        ),
                        (null as Gs2.Gs2Chat.Model.Subscribe).CacheKey(
                            this.RoomName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Chat.Model.Subscribe).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Chat.Model.Subscribe).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RoomName,
                    null,
                    () => this.GetAsync(
                        new GetSubscribeByUserIdRequest()
                    )
                );
            }
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Chat.Model.Subscribe> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Chat.Model.Subscribe> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Chat.Model.Subscribe> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Chat.Model.Subscribe).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.RoomName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Chat.Model.Subscribe> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Chat.Model.Subscribe>(
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheKey(
                    this.RoomName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Chat.Model.Subscribe> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Chat.Model.Subscribe> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Chat.Model.Subscribe> callback)
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

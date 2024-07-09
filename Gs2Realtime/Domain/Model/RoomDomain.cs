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
using Gs2.Gs2Realtime.Domain.Iterator;
using Gs2.Gs2Realtime.Model.Cache;
using Gs2.Gs2Realtime.Request;
using Gs2.Gs2Realtime.Result;
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

namespace Gs2.Gs2Realtime.Domain.Model
{

    public partial class RoomDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2RealtimeRestClient _client;
        public string NamespaceName { get; } = null!;
        public string RoomName { get; } = null!;

        public RoomDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string roomName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2RealtimeRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.RoomName = roomName;
        }

    }

    public partial class RoomDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Realtime.Model.Room> GetFuture(
            GetRoomRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Realtime.Model.Room> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
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
            return new Gs2InlineFuture<Gs2.Gs2Realtime.Model.Room>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Realtime.Model.Room> GetAsync(
            #else
        private async Task<Gs2.Gs2Realtime.Model.Room> GetAsync(
            #endif
            GetRoomRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetRoomAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Realtime.Domain.Model.RoomDomain> DeleteFuture(
            DeleteRoomRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Realtime.Domain.Model.RoomDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
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
            return new Gs2InlineFuture<Gs2.Gs2Realtime.Domain.Model.RoomDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Realtime.Domain.Model.RoomDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Realtime.Domain.Model.RoomDomain> DeleteAsync(
            #endif
            DeleteRoomRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteRoomAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

    }

    public partial class RoomDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Realtime.Model.Room> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Realtime.Model.Room> self)
            {
                var (value, find) = (null as Gs2.Gs2Realtime.Model.Room).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.RoomName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Realtime.Model.Room).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.RoomName,
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
            return new Gs2InlineFuture<Gs2.Gs2Realtime.Model.Room>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Realtime.Model.Room> ModelAsync()
            #else
        public async Task<Gs2.Gs2Realtime.Model.Room> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Realtime.Model.Room).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.RoomName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Realtime.Model.Room).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.RoomName,
                () => this.GetAsync(
                    new GetRoomRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Realtime.Model.Room> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Realtime.Model.Room> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Realtime.Model.Room> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Realtime.Model.Room).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.RoomName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Realtime.Model.Room> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Realtime.Model.Room).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Realtime.Model.Room).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Realtime.Model.Room>(
                (null as Gs2.Gs2Realtime.Model.Room).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Realtime.Model.Room).CacheKey(
                    this.RoomName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Realtime.Model.Room> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Realtime.Model.Room> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Realtime.Model.Room> callback)
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

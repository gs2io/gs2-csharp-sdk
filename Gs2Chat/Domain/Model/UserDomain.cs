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

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ChatRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ChatRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Chat.Model.Room> Rooms(
            string namePrefix = null
        )
        {
            return new DescribeRoomsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Chat.Model.Room> RoomsAsync(
            #else
        public DescribeRoomsIterator RoomsAsync(
            #endif
            string namePrefix = null
        )
        {
            return new DescribeRoomsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeRooms(
            Action<Gs2.Gs2Chat.Model.Room[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Chat.Model.Room>(
                (null as Gs2.Gs2Chat.Model.Room).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await RoomsAsync(
                                namePrefix
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
        public async UniTask<ulong> SubscribeRoomsWithInitialCallAsync(
            Action<Gs2.Gs2Chat.Model.Room[]> callback,
            string namePrefix = null
        )
        {
            var items = await RoomsAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeRooms(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeRooms(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Chat.Model.Room>(
                (null as Gs2.Gs2Chat.Model.Room).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateRooms(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Chat.Model.Room>(
                (null as Gs2.Gs2Chat.Model.Room).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                )
            );
        }

        public Gs2.Gs2Chat.Domain.Model.RoomDomain Room(
            string roomName,
            string password = null
        ) {
            return new Gs2.Gs2Chat.Domain.Model.RoomDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                roomName,
                password
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Chat.Model.Subscribe> Subscribes(
            string roomNamePrefix = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeSubscribesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                roomNamePrefix,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Chat.Model.Subscribe> SubscribesAsync(
            #else
        public DescribeSubscribesByUserIdIterator SubscribesAsync(
            #endif
            string roomNamePrefix = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeSubscribesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                roomNamePrefix,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSubscribes(
            Action<Gs2.Gs2Chat.Model.Subscribe[]> callback,
            string roomNamePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Chat.Model.Subscribe>(
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await SubscribesAsync(
                                roomNamePrefix
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
        public async UniTask<ulong> SubscribeSubscribesWithInitialCallAsync(
            Action<Gs2.Gs2Chat.Model.Subscribe[]> callback,
            string roomNamePrefix = null
        )
        {
            var items = await SubscribesAsync(
                roomNamePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeSubscribes(
                callback,
                roomNamePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSubscribes(
            ulong callbackId,
            string roomNamePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Chat.Model.Subscribe>(
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateSubscribes(
            string roomNamePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Chat.Model.Subscribe>(
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                )
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Chat.Model.Subscribe> SubscribesByRoomName(
            string roomName
        )
        {
            return new DescribeSubscribesByRoomNameIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                roomName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Chat.Model.Subscribe> SubscribesByRoomNameAsync(
            #else
        public DescribeSubscribesByRoomNameIterator SubscribesByRoomNameAsync(
            #endif
            string roomName
        )
        {
            return new DescribeSubscribesByRoomNameIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                roomName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSubscribesByRoomName(
            Action<Gs2.Gs2Chat.Model.Subscribe[]> callback,
            string roomName
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Chat.Model.Subscribe>(
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await SubscribesByRoomNameAsync(
                                roomName
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
        public async UniTask<ulong> SubscribeSubscribesByRoomNameWithInitialCallAsync(
            Action<Gs2.Gs2Chat.Model.Subscribe[]> callback,
            string roomName
        )
        {
            var items = await SubscribesByRoomNameAsync(
                roomName
            ).ToArrayAsync();
            var callbackId = SubscribeSubscribesByRoomName(
                callback,
                roomName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSubscribesByRoomName(
            ulong callbackId,
            string roomName
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Chat.Model.Subscribe>(
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateSubscribesByRoomName(
            string roomName
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Chat.Model.Subscribe>(
                (null as Gs2.Gs2Chat.Model.Subscribe).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                )
            );
        }

        public Gs2.Gs2Chat.Domain.Model.SubscribeDomain Subscribe(
            string roomName
        ) {
            return new Gs2.Gs2Chat.Domain.Model.SubscribeDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                roomName
            );
        }

    }

    public partial class UserDomain {

    }

    public partial class UserDomain {

    }
}

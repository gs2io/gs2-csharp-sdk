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
using Gs2.Gs2Chat.Request;
using Gs2.Gs2Chat.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Chat.Domain.Model
{

    public partial class RoomDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ChatRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _roomName;
        private readonly string _password;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string RoomName => _roomName;
        public string Password => _password;

        public RoomDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string roomName,
            string password
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ChatRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._roomName = roomName;
            this._password = password;
            this._parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Singleton",
                "Room"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Chat.Model.Message> Messages(
        )
        {
            return new DescribeMessagesByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.RoomName,
                this.Password,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Chat.Model.Message> MessagesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Chat.Model.Message> Messages(
            #endif
        #else
        public DescribeMessagesByUserIdIterator MessagesAsync(
        #endif
        )
        {
            return new DescribeMessagesByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.RoomName,
                this.Password,
                this.UserId
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeMessages(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Chat.Model.Message>(
                Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Singleton",
                    this.RoomName,
                    "Message"
                ),
                callback
            );
        }

        public void UnsubscribeMessages(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Chat.Model.Message>(
                Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Singleton",
                    this.RoomName,
                    "Message"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Chat.Domain.Model.MessageDomain Message(
            string messageName
        ) {
            return new Gs2.Gs2Chat.Domain.Model.MessageDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.RoomName,
                this.Password,
                messageName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string roomName,
            string childType
        )
        {
            return string.Join(
                ":",
                "chat",
                namespaceName ?? "null",
                userId ?? "null",
                roomName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string roomName
        )
        {
            return string.Join(
                ":",
                roomName ?? "null"
            );
        }

    }

    public partial class RoomDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Chat.Model.Room> GetFuture(
            GetRoomRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Room> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRoomName(this.RoomName);
                var future = this._client.GetRoomFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                            request.RoomName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Chat.Model.Room>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "room")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            "Room"
                        );
                        var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName);
            GetRoomResult result = null;
            try {
                result = await this._client.GetRoomAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                    request.RoomName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Chat.Model.Room>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "room")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        "Room"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.RoomDomain> UpdateFromBackendFuture(
            UpdateRoomFromBackendRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.RoomDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithRoomName(this.RoomName)
                    .WithPassword(this.Password);
                var future = this._client.UpdateRoomFromBackendFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            "Room"
                        );
                        var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.RoomDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.RoomDomain> UpdateFromBackendAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.RoomDomain> UpdateFromBackendAsync(
            #endif
            UpdateRoomFromBackendRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithRoomName(this.RoomName)
                .WithPassword(this.Password);
            UpdateRoomFromBackendResult result = null;
                result = await this._client.UpdateRoomFromBackendAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        "Room"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to UpdateFromBackendFuture.")]
        public IFuture<Gs2.Gs2Chat.Domain.Model.RoomDomain> UpdateFromBackend(
            UpdateRoomFromBackendRequest request
        ) {
            return UpdateFromBackendFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.RoomDomain> DeleteFromBackendFuture(
            DeleteRoomFromBackendRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.RoomDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithRoomName(this.RoomName);
                var future = this._client.DeleteRoomFromBackendFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                            request.RoomName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Chat.Model.Room>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "room")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            "Room"
                        );
                        var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Chat.Model.Room>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.RoomDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.RoomDomain> DeleteFromBackendAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.RoomDomain> DeleteFromBackendAsync(
            #endif
            DeleteRoomFromBackendRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithRoomName(this.RoomName);
            DeleteRoomFromBackendResult result = null;
            try {
                result = await this._client.DeleteRoomFromBackendAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                    request.RoomName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Chat.Model.Room>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "room")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        "Room"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Chat.Model.Room>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFromBackendFuture.")]
        public IFuture<Gs2.Gs2Chat.Domain.Model.RoomDomain> DeleteFromBackend(
            DeleteRoomFromBackendRequest request
        ) {
            return DeleteFromBackendFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.MessageDomain> PostFuture(
            PostByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.MessageDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithRoomName(this.RoomName)
                    .WithPassword(this.Password);
                var future = this._client.PostByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            this.RoomName,
                            "Message"
                        );
                        var key = Gs2.Gs2Chat.Domain.Model.MessageDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Chat.Domain.Model.MessageDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.RoomName,
                    request.Password,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.MessageDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Chat.Domain.Model.MessageDomain> PostAsync(
            #else
        public async Task<Gs2.Gs2Chat.Domain.Model.MessageDomain> PostAsync(
            #endif
            PostByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithRoomName(this.RoomName)
                .WithPassword(this.Password);
            PostByUserIdResult result = null;
                result = await this._client.PostByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        this.RoomName,
                        "Message"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.MessageDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Chat.Domain.Model.MessageDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.RoomName,
                    request.Password,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to PostFuture.")]
        public IFuture<Gs2.Gs2Chat.Domain.Model.MessageDomain> Post(
            PostByUserIdRequest request
        ) {
            return PostFuture(request);
        }
        #endif

    }

    public partial class RoomDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Model.Room> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Room> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Chat.Model.Room>(
                    _parentKey,
                    Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                        this.RoomName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetRoomRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                                    this.RoomName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Chat.Model.Room>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "room")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Chat.Model.Room>(
                        _parentKey,
                        Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                            this.RoomName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
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
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Chat.Model.Room>(
                _parentKey,
                Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                    this.RoomName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Chat.Model.Room>(
                    _parentKey,
                    Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                        this.RoomName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetRoomRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                                    this.RoomName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Chat.Model.Room>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "room")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Chat.Model.Room>(
                        _parentKey,
                        Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                            this.RoomName?.ToString()
                        )
                    );
                }
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
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


        public ulong Subscribe(Action<Gs2.Gs2Chat.Model.Room> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                    this.RoomName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Chat.Model.Room>(
                _parentKey,
                Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                    this.RoomName.ToString()
                ),
                callbackId
            );
        }

    }
}

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

    public partial class SubscribeAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ChatRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _roomName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string RoomName => _roomName;

        public SubscribeAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string roomName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ChatRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._roomName = roomName;
            this._parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Subscribe"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> SubscribeFuture(
            SubscribeRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRoomName(this.RoomName);
                var future = this._client.SubscribeFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRoomName(this.RoomName);
                SubscribeResult result = null;
                    result = await this._client.SubscribeAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Subscribe"
                        );
                        var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                            resultModel.Item.RoomName.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> SubscribeAsync(
            SubscribeRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this.RoomName);
            var future = this._client.SubscribeFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this.RoomName);
            SubscribeResult result = null;
                result = await this._client.SubscribeAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Subscribe"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        resultModel.Item.RoomName.ToString()
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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> SubscribeAsync(
            SubscribeRequest request
        ) {
            var future = SubscribeFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to SubscribeFuture.")]
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> Subscribe(
            SubscribeRequest request
        ) {
            return SubscribeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Chat.Model.Subscribe> GetFuture(
            GetSubscribeRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Subscribe> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRoomName(this.RoomName);
                var future = this._client.GetSubscribeFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                            request.RoomName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "subscribe")
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRoomName(this.RoomName);
                GetSubscribeResult result = null;
                try {
                    result = await this._client.GetSubscribeAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        request.RoomName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "subscribe")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Subscribe"
                        );
                        var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                            resultModel.Item.RoomName.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Chat.Model.Subscribe>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Chat.Model.Subscribe> GetAsync(
            GetSubscribeRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this.RoomName);
            var future = this._client.GetSubscribeFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        request.RoomName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "subscribe")
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this.RoomName);
            GetSubscribeResult result = null;
            try {
                result = await this._client.GetSubscribeAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                    request.RoomName.ToString()
                    );
                _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "subscribe")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Subscribe"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        resultModel.Item.RoomName.ToString()
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
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> UpdateNotificationTypeFuture(
            UpdateNotificationTypeRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRoomName(this.RoomName);
                var future = this._client.UpdateNotificationTypeFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRoomName(this.RoomName);
                UpdateNotificationTypeResult result = null;
                    result = await this._client.UpdateNotificationTypeAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Subscribe"
                        );
                        var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                            resultModel.Item.RoomName.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> UpdateNotificationTypeAsync(
            UpdateNotificationTypeRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this.RoomName);
            var future = this._client.UpdateNotificationTypeFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this.RoomName);
            UpdateNotificationTypeResult result = null;
                result = await this._client.UpdateNotificationTypeAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Subscribe"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        resultModel.Item.RoomName.ToString()
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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> UpdateNotificationTypeAsync(
            UpdateNotificationTypeRequest request
        ) {
            var future = UpdateNotificationTypeFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateNotificationTypeFuture.")]
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> UpdateNotificationType(
            UpdateNotificationTypeRequest request
        ) {
            return UpdateNotificationTypeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> UnsubscribeFuture(
            UnsubscribeRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRoomName(this.RoomName);
                var future = this._client.UnsubscribeFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                            request.RoomName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "subscribe")
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithRoomName(this.RoomName);
                UnsubscribeResult result = null;
                try {
                    result = await this._client.UnsubscribeAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        request.RoomName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "subscribe")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Subscribe"
                        );
                        var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                            resultModel.Item.RoomName.ToString()
                        );
                        cache.Delete<Gs2.Gs2Chat.Model.Subscribe>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> UnsubscribeAsync(
            UnsubscribeRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this.RoomName);
            var future = this._client.UnsubscribeFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        request.RoomName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "subscribe")
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this.RoomName);
            UnsubscribeResult result = null;
            try {
                result = await this._client.UnsubscribeAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                    request.RoomName.ToString()
                    );
                _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "subscribe")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Subscribe"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        resultModel.Item.RoomName.ToString()
                    );
                    cache.Delete<Gs2.Gs2Chat.Model.Subscribe>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> UnsubscribeAsync(
            UnsubscribeRequest request
        ) {
            var future = UnsubscribeFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UnsubscribeFuture.")]
        public IFuture<Gs2.Gs2Chat.Domain.Model.SubscribeAccessTokenDomain> Unsubscribe(
            UnsubscribeRequest request
        ) {
            return UnsubscribeFuture(request);
        }
        #endif

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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Chat.Model.Subscribe> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Subscribe> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Chat.Model.Subscribe>(
                    _parentKey,
                    Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        this.RoomName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetSubscribeRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                                    this.RoomName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "subscribe")
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
                    (value, _) = _cache.Get<Gs2.Gs2Chat.Model.Subscribe>(
                        _parentKey,
                        Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                            this.RoomName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Model.Subscribe>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Chat.Model.Subscribe> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Chat.Model.Subscribe>(
                    _parentKey,
                    Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                        this.RoomName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetSubscribeRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                                    this.RoomName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Chat.Model.Subscribe>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "subscribe")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Chat.Model.Subscribe>(
                        _parentKey,
                        Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                            this.RoomName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Chat.Model.Subscribe> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

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


        public ulong Subscribe(Action<Gs2.Gs2Chat.Model.Subscribe> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                    this.RoomName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Chat.Model.Subscribe>(
                _parentKey,
                Gs2.Gs2Chat.Domain.Model.SubscribeDomain.CreateCacheKey(
                    this.RoomName.ToString()
                ),
                callbackId
            );
        }

    }
}

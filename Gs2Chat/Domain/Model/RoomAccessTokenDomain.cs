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

    public partial class RoomAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ChatRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _roomName;
        private readonly string _password;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public string RoomName => _roomName;
        public string Password => _password;

        public RoomAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string roomName,
            string password
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
            this._password = password;
            this._parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "Singleton",
                "Room"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> UpdateAsync(
        #endif
            UpdateRoomRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this._roomName)
                .WithPassword(this._password);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateRoomFuture(
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
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #else
            var result = await this._client.UpdateRoomAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #endif
            Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> DeleteAsync(
        #endif
            DeleteRoomRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this._roomName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteRoomFuture(
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
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "Singleton",
                        "Room"
                );
                var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Delete<Gs2.Gs2Chat.Model.Room>(parentKey, key);
            }
            #else
            DeleteRoomResult result = null;
            try {
                result = await this._client.DeleteRoomAsync(
                    request
                );
                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
              
                {
                    var parentKey = Gs2.Gs2Chat.Domain.Model.UserDomain.CreateCacheParentKey(
                        _namespaceName.ToString(),
                        "Singleton",
                            "Room"
                    );
                    var key = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Chat.Model.Room>(parentKey, key);
                }
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.RoomAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain> PostAsync(
            #else
        public IFuture<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain> Post(
            #endif
        #else
        public async Task<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain> PostAsync(
        #endif
            PostRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithRoomName(this._roomName)
                .WithPassword(this._password);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.PostFuture(
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
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "Singleton",
                    resultModel.Item.RoomName.ToString(),
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
            #else
            var result = await this._client.PostAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "Singleton",
                    resultModel.Item.RoomName.ToString(),
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
            #endif
            Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain domain = new Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                this._accessToken,
                result?.Item?.RoomName,
                request.Password,
                result?.Item?.Name
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Chat.Model.Message> Messages(
        )
        {
            return new DescribeMessagesIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._roomName,
                this._password,
                this._accessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Chat.Model.Message> MessagesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Chat.Model.Message> Messages(
            #endif
        #else
        public DescribeMessagesIterator Messages(
        #endif
        )
        {
            return new DescribeMessagesIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._roomName,
                this._password,
                this._accessToken
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

        public Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain Message(
            string messageName
        ) {
            return new Gs2.Gs2Chat.Domain.Model.MessageAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._accessToken,
                this._roomName,
                this._password,
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Chat.Model.Room> Model() {
            #else
        public IFuture<Gs2.Gs2Chat.Model.Room> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Chat.Model.Room> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Chat.Model.Room> self)
            {
        #endif
            Gs2.Gs2Chat.Model.Room value = _cache.Get<Gs2.Gs2Chat.Model.Room>(
                _parentKey,
                Gs2.Gs2Chat.Domain.Model.RoomDomain.CreateCacheKey(
                    this.RoomName?.ToString()
                )
            );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Chat.Model.Room>(Impl);
        #endif
        }

    }
}

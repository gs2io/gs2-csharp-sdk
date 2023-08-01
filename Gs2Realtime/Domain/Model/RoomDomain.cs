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
using Gs2.Gs2Realtime.Domain.Iterator;
using Gs2.Gs2Realtime.Request;
using Gs2.Gs2Realtime.Result;
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

namespace Gs2.Gs2Realtime.Domain.Model
{

    public partial class RoomDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2RealtimeRestClient _client;
        private readonly string _namespaceName;
        private readonly string _roomName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string RoomName => _roomName;

        public RoomDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string roomName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2RealtimeRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._roomName = roomName;
            this._parentKey = Gs2.Gs2Realtime.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Room"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Realtime.Model.Room> GetAsync(
            #else
        private IFuture<Gs2.Gs2Realtime.Model.Room> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Realtime.Model.Room> GetAsync(
        #endif
            GetRoomRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Realtime.Model.Room> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetRoomFuture(
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
            var result = await this._client.GetRoomAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Realtime.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Room"
                    );
                    var key = Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Realtime.Model.Room>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Realtime.Domain.Model.RoomDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Realtime.Domain.Model.RoomDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Realtime.Domain.Model.RoomDomain> DeleteAsync(
        #endif
            DeleteRoomRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Realtime.Domain.Model.RoomDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteRoomFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
                        request.RoomName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Realtime.Model.Room>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            DeleteRoomResult result = null;
            try {
                result = await this._client.DeleteRoomAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "room")
                {
                    var key = Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
                        request.RoomName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Realtime.Model.Room>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Realtime.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Room"
                    );
                    var key = Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Realtime.Model.Room>(parentKey, key);
                }
            }
            Gs2.Gs2Realtime.Domain.Model.RoomDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Realtime.Domain.Model.RoomDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string roomName,
            string childType
        )
        {
            return string.Join(
                ":",
                "realtime",
                namespaceName ?? "null",
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
        public async UniTask<Gs2.Gs2Realtime.Model.Room> Model() {
            #else
        public IFuture<Gs2.Gs2Realtime.Model.Room> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Realtime.Model.Room> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Realtime.Model.Room> self)
            {
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2Realtime.Model.Room>(
                       _parentKey,
                       Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
                            this.RoomName?.ToString()
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2Realtime.Model.Room>(
                _parentKey,
                Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
                    this.RoomName?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetRoomRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
                                    this.RoomName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Realtime.Model.Room>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "room")
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
                            this.RoomName?.ToString()
                        );
                    _cache.Put<Gs2.Gs2Realtime.Model.Room>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "room")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Realtime.Model.Room>(
                    _parentKey,
                    Gs2.Gs2Realtime.Domain.Model.RoomDomain.CreateCacheKey(
                        this.RoomName?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Realtime.Model.Room>(Impl);
        #endif
        }

    }
}

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
using Gs2.Gs2Gateway.Domain.Iterator;
using Gs2.Gs2Gateway.Request;
using Gs2.Gs2Gateway.Result;
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

namespace Gs2.Gs2Gateway.Domain.Model
{

    public partial class WebSocketSessionDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2GatewayRestClient _client;
        private readonly Gs2WebSocketSession _wssession;
        private readonly Gs2GatewayWebSocketClient _wsclient;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _connectionId;

        private readonly String _parentKey;
        public string Protocol { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public WebSocketSessionDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            Gs2WebSocketSession wssession,
            string namespaceName,
            string userId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2GatewayRestClient(
                session
            );
            this._wssession = wssession;
            this._wsclient = new Gs2GatewayWebSocketClient(
                wssession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._userId != null ? this._userId.ToString() : null,
                "WebSocketSession"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain> SetUserIdAsync(
            #else
        public IFuture<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain> SetUserId(
            #endif
        #else
        public async Task<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain> SetUserIdAsync(
        #endif
            SetUserIdByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain> self)
            {
        #endif
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future2 = Model();
            yield return future2;
            if (future2.Error != null)
            {
                self.OnError(future2.Error);
                yield break;
            }
            var model = future2.Result;
            #else
            var model = await Model();
            #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SetUserIdByUserIdFuture(
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
                var parentKey = Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                    resultModel.Item.NamespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "WebSocketSession"
                );
                var key = Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain.CreateCacheKey(
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.SetUserIdByUserIdAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                    resultModel.Item.NamespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "WebSocketSession"
                );
                var key = Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain.CreateCacheKey(
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #endif
            Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "gateway",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Gateway.Model.WebSocketSession> Model() {
            #else
        public IFuture<Gs2.Gs2Gateway.Model.WebSocketSession> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Gateway.Model.WebSocketSession> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Gateway.Model.WebSocketSession> self)
            {
        #endif
            Gs2.Gs2Gateway.Model.WebSocketSession value = _cache.Get<Gs2.Gs2Gateway.Model.WebSocketSession>(
                _parentKey,
                Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Gateway.Model.WebSocketSession>(Impl);
        #endif
        }

    }
}

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
using Gs2.Gs2Gateway.Domain.Iterator;
using Gs2.Gs2Gateway.Request;
using Gs2.Gs2Gateway.Result;
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
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Gateway.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GatewayRestClient _client;
        private readonly Gs2GatewayWebSocketClient _wsclient;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string Protocol { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GatewayRestClient(
                gs2.RestSession
            );
            this._wsclient = new Gs2GatewayWebSocketClient(
                gs2.WebSocketSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Gateway.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Gateway.Model.WebSocketSession> WebSocketSessions(
        )
        {
            return new DescribeWebSocketSessionsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Gateway.Model.WebSocketSession> WebSocketSessionsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Gateway.Model.WebSocketSession> WebSocketSessions(
            #endif
        #else
        public DescribeWebSocketSessionsIterator WebSocketSessionsAsync(
        #endif
        )
        {
            return new DescribeWebSocketSessionsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
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

        public ulong SubscribeWebSocketSessions(
            Action<Gs2.Gs2Gateway.Model.WebSocketSession[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Gateway.Model.WebSocketSession>(
                Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "WebSocketSession"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWebSocketSessionsWithInitialCallAsync(
            Action<Gs2.Gs2Gateway.Model.WebSocketSession[]> callback
        )
        {
            var items = await WebSocketSessionsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeWebSocketSessions(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeWebSocketSessions(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Gateway.Model.WebSocketSession>(
                Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "WebSocketSession"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Gateway.Domain.Model.WebSocketSessionAccessTokenDomain WebSocketSession(
        ) {
            return new Gs2.Gs2Gateway.Domain.Model.WebSocketSessionAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken
            );
        }

        public Gs2.Gs2Gateway.Domain.Model.FirebaseTokenAccessTokenDomain FirebaseToken(
        ) {
            return new Gs2.Gs2Gateway.Domain.Model.FirebaseTokenAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken
            );
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
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
            );
        }

    }
}

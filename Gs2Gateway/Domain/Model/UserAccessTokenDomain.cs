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
using Gs2.Gs2Gateway.Model.Cache;
using Gs2.Gs2Gateway.Request;
using Gs2.Gs2Gateway.Result;
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

namespace Gs2.Gs2Gateway.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GatewayRestClient _client;
        private readonly Gs2GatewayWebSocketClient _wsclient;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string Protocol { get; set; } = null!;
        public string[] SendConnectionIds { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

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
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Gateway.Model.WebSocketSession> WebSocketSessions(
        )
        {
            return new DescribeWebSocketSessionsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Gateway.Model.WebSocketSession> WebSocketSessionsAsync(
            #else
        public DescribeWebSocketSessionsIterator WebSocketSessionsAsync(
            #endif
        )
        {
            return new DescribeWebSocketSessionsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeWebSocketSessions(
            Action<Gs2.Gs2Gateway.Model.WebSocketSession[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Gateway.Model.WebSocketSession>(
                (null as Gs2.Gs2Gateway.Model.WebSocketSession).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
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
                (null as Gs2.Gs2Gateway.Model.WebSocketSession).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Gateway.Domain.Model.WebSocketSessionAccessTokenDomain WebSocketSession(
        ) {
            return new Gs2.Gs2Gateway.Domain.Model.WebSocketSessionAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public Gs2.Gs2Gateway.Domain.Model.FirebaseTokenAccessTokenDomain FirebaseToken(
        ) {
            return new Gs2.Gs2Gateway.Domain.Model.FirebaseTokenAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken
            );
        }

    }
}

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
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Gateway.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GatewayRestClient _client;
        private readonly Gs2GatewayWebSocketClient _wsclient;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string Protocol { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GatewayRestClient(
                gs2.RestSession
            );
            this._wsclient = new Gs2GatewayWebSocketClient(
                gs2.WebSocketSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
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
            return new DescribeWebSocketSessionsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Gateway.Model.WebSocketSession> WebSocketSessionsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Gateway.Model.WebSocketSession> WebSocketSessions(
            #endif
        #else
        public DescribeWebSocketSessionsByUserIdIterator WebSocketSessionsAsync(
        #endif
        )
        {
            return new DescribeWebSocketSessionsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeWebSocketSessions(Action callback)
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

        public void UnsubscribeWebSocketSessions(ulong callbackId)
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

        public Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain WebSocketSession(
        ) {
            return new Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId
            );
        }

        public Gs2.Gs2Gateway.Domain.Model.FirebaseTokenDomain FirebaseToken(
        ) {
            return new Gs2.Gs2Gateway.Domain.Model.FirebaseTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId
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

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Gateway.Domain.Model.UserDomain> SendNotificationFuture(
            SendNotificationRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Gateway.Domain.Model.UserDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.SendNotificationFuture(
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
                    
                }
                var domain = this;
                this.Protocol = domain.Protocol = result?.Protocol;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Gateway.Domain.Model.UserDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Gateway.Domain.Model.UserDomain> SendNotificationAsync(
            #else
        public async Task<Gs2.Gs2Gateway.Domain.Model.UserDomain> SendNotificationAsync(
            #endif
            SendNotificationRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            SendNotificationResult result = null;
                result = await this._client.SendNotificationAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Protocol = domain.Protocol = result?.Protocol;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to SendNotificationFuture.")]
        public IFuture<Gs2.Gs2Gateway.Domain.Model.UserDomain> SendNotification(
            SendNotificationRequest request
        ) {
            return SendNotificationFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain[]> DisconnectFuture(
            DisconnectByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain[]> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.DisconnectByUserIdFuture(
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
                    {
                        var parentKey = Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "WebSocketSession"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain.CreateCacheKey(
                            );
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain(
                        this._gs2,
                        result.Items[i]?.NamespaceName,
                        result.Items[i]?.UserId
                    );
                    var parentKey = Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "WebSocketSession"
                    );
                    var key = Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain.CreateCacheKey(
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain[]> DisconnectAsync(
            #else
        public async Task<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain[]> DisconnectAsync(
            #endif
            DisconnectByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            DisconnectByUserIdResult result = null;
                result = await this._client.DisconnectByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "WebSocketSession"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain.CreateCacheKey(
                        );
                        cache.Put(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain(
                        this._gs2,
                        result.Items[i]?.NamespaceName,
                        result.Items[i]?.UserId
                    );
                    var parentKey = Gs2.Gs2Gateway.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "WebSocketSession"
                    );
                    var key = Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain.CreateCacheKey(
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DisconnectFuture.")]
        public IFuture<Gs2.Gs2Gateway.Domain.Model.WebSocketSessionDomain[]> Disconnect(
            DisconnectByUserIdRequest request
        ) {
            return DisconnectFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Gateway.Domain.Model.UserDomain> DisconnectAllFuture(
            DisconnectAllRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Gateway.Domain.Model.UserDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DisconnectAllFuture(
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
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Gateway.Domain.Model.UserDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Gateway.Domain.Model.UserDomain> DisconnectAllAsync(
            #else
        public async Task<Gs2.Gs2Gateway.Domain.Model.UserDomain> DisconnectAllAsync(
            #endif
            DisconnectAllRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            DisconnectAllResult result = null;
                result = await this._client.DisconnectAllAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DisconnectAllFuture.")]
        public IFuture<Gs2.Gs2Gateway.Domain.Model.UserDomain> DisconnectAll(
            DisconnectAllRequest request
        ) {
            return DisconnectAllFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

    }
}

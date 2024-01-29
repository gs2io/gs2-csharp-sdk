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
using Gs2.Gs2Inbox.Domain.Iterator;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Result;
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

namespace Gs2.Gs2Inbox.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InboxRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InboxRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> ReceiveGlobalMessageFuture(
            ReceiveGlobalMessageRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token);
                var future = this._client.ReceiveGlobalMessageFuture(
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
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Message"
                        );
                        foreach (var item in resultModel.Item) {
                            var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                                item.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                item,
                                item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                    _gs2.Cache.Delete<Gs2.Gs2Inbox.Model.Received>(
                        Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "Received"
                        ),
                        Gs2.Gs2Inbox.Domain.Model.ReceivedDomain.CreateCacheKey(
                        )
                    );
                }
                var domain = new Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[result?.Item.Length ?? 0];
                for (int i=0; i<result?.Item.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain(
                        this._gs2,
                        request.NamespaceName,
                        this._accessToken,
                        result.Item[i]?.Name
                    );
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Message"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        result.Item[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Item[i],
                        result.Item[i].ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> ReceiveGlobalMessageAsync(
            #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> ReceiveGlobalMessageAsync(
            #endif
            ReceiveGlobalMessageRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            ReceiveGlobalMessageResult result = null;
                result = await this._client.ReceiveGlobalMessageAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Message"
                    );
                    foreach (var item in resultModel.Item) {
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            item,
                            item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                _gs2.Cache.Delete<Gs2.Gs2Inbox.Model.Received>(
                    Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "Received"
                    ),
                    Gs2.Gs2Inbox.Domain.Model.ReceivedDomain.CreateCacheKey(
                    )
                );
            }
                var domain = new Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[result?.Item.Length ?? 0];
                for (int i=0; i<result?.Item.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain(
                        this._gs2,
                        request.NamespaceName,
                        this._accessToken,
                        result.Item[i]?.Name
                    );
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Message"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        result.Item[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Item[i],
                        result.Item[i].ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to ReceiveGlobalMessageFuture.")]
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> ReceiveGlobalMessage(
            ReceiveGlobalMessageRequest request
        ) {
            return ReceiveGlobalMessageFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inbox.Model.Message> Messages(
            bool? isRead
        )
        {
            return new DescribeMessagesIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                isRead
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inbox.Model.Message> MessagesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inbox.Model.Message> Messages(
            #endif
        #else
        public DescribeMessagesIterator MessagesAsync(
        #endif
            bool? isRead
        )
        {
            return new DescribeMessagesIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                isRead
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

        public ulong SubscribeMessages(
            Action<Gs2.Gs2Inbox.Model.Message[]> callback,
            bool? isRead
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inbox.Model.Message>(
                Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Message"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeMessagesWithInitialCallAsync(
            Action<Gs2.Gs2Inbox.Model.Message[]> callback,
            bool? isRead
        )
        {
            var items = await MessagesAsync(
                isRead
            ).ToArrayAsync();
            var callbackId = SubscribeMessages(
                callback,
                isRead
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeMessages(
            ulong callbackId,
            bool? isRead
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inbox.Model.Message>(
                Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Message"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain Message(
            string messageName
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                messageName
            );
        }

        public Gs2.Gs2Inbox.Domain.Model.ReceivedAccessTokenDomain Received(
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.ReceivedAccessTokenDomain(
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
                "inbox",
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

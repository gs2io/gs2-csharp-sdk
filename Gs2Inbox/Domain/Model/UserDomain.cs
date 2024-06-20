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
using Gs2.Gs2Inbox.Model.Cache;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Result;
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

namespace Gs2.Gs2Inbox.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InboxRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InboxRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inbox.Model.Message> Messages(
            bool? isRead = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeMessagesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                isRead,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inbox.Model.Message> MessagesAsync(
            #else
        public DescribeMessagesByUserIdIterator MessagesAsync(
            #endif
            bool? isRead = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeMessagesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                isRead,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeMessages(
            Action<Gs2.Gs2Inbox.Model.Message[]> callback,
            bool? isRead = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inbox.Model.Message>(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeMessagesWithInitialCallAsync(
            Action<Gs2.Gs2Inbox.Model.Message[]> callback,
            bool? isRead = null
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
            bool? isRead = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inbox.Model.Message>(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inbox.Domain.Model.MessageDomain Message(
            string messageName
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.MessageDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                messageName
            );
        }

        public Gs2.Gs2Inbox.Domain.Model.ReceivedDomain Received(
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.ReceivedDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain> SendMessageFuture(
            SendMessageByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.SendMessageByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Inbox.Domain.Model.MessageDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageDomain> SendMessageAsync(
            #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageDomain> SendMessageAsync(
            #endif
            SendMessageByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.SendMessageByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Inbox.Domain.Model.MessageDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]> ReceiveGlobalMessageFuture(
            ReceiveGlobalMessageByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ReceiveGlobalMessageByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                _gs2.Cache.ClearListCache<Gs2.Gs2Inbox.Model.Message>(
                    (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                        this.NamespaceName,
                        this.UserId
                    )
                );
                _gs2.Cache.Delete<Gs2.Gs2Inbox.Model.Received>(
                    (null as Gs2.Gs2Inbox.Model.Received).CacheParentKey(
                        this.NamespaceName,
                        this.UserId
                    ),
                    (null as Gs2.Gs2Inbox.Model.Received).CacheKey(
                    )
                );
                var domain = result?.Item?.Select(v => new Gs2.Gs2Inbox.Domain.Model.MessageDomain(
                    this._gs2,
                    this.NamespaceName,
                    v?.UserId,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Inbox.Domain.Model.MessageDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]> ReceiveGlobalMessageAsync(
            #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]> ReceiveGlobalMessageAsync(
            #endif
            ReceiveGlobalMessageByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ReceiveGlobalMessageByUserIdAsync(request)
            );
            _gs2.Cache.ClearListCache<Gs2.Gs2Inbox.Model.Message>(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                )
            );
            _gs2.Cache.Delete<Gs2.Gs2Inbox.Model.Received>(
                (null as Gs2.Gs2Inbox.Model.Received).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Inbox.Model.Received).CacheKey(
                )
            );
            var domain = result?.Item?.Select(v => new Gs2.Gs2Inbox.Domain.Model.MessageDomain(
                this._gs2,
                this.NamespaceName,
                v?.UserId,
                v?.Name
            )).ToArray() ?? Array.Empty<Gs2.Gs2Inbox.Domain.Model.MessageDomain>();
            return domain;
        }
        #endif

    }

    public partial class UserDomain {

    }
}

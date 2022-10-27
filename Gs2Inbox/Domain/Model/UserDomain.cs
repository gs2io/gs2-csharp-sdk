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
using Gs2.Gs2Inbox.Domain.Iterator;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Result;
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

namespace Gs2.Gs2Inbox.Domain.Model
{

    public partial class UserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InboxRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2InboxRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this._namespaceName?.ToString() ?? null,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageDomain> SendMessageAsync(
            #else
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain> SendMessage(
            #endif
        #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageDomain> SendMessageAsync(
        #endif
            SendMessageByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SendMessageByUserIdFuture(
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
            var result = await this._client.SendMessageByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;

            {
                var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                    this._namespaceName?.ToString() ?? null,
                    this._userId?.ToString() ?? null,
                    "Message"
                );
                var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            var domain = new Gs2.Gs2Inbox.Domain.Model.MessageDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.UserId,
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
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]> ReceiveGlobalMessageAsync(
            #else
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]> ReceiveGlobalMessage(
            #endif
        #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]> ReceiveGlobalMessageAsync(
        #endif
            ReceiveGlobalMessageByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ReceiveGlobalMessageByUserIdFuture(
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
            var result = await this._client.ReceiveGlobalMessageByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            {
                foreach (var item in resultModel.Item) {
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this._namespaceName?.ToString() ?? null,
                        this._userId?.ToString() ?? null,
                        "Message"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        item,
                        item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            cache.Delete<Gs2.Gs2Inbox.Model.Received>(
                Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName?.ToString(),
                    this.UserId?.ToString(),
                    "Received"
                ),
                Gs2.Gs2Inbox.Domain.Model.ReceivedDomain.CreateCacheKey(
                )
            );
            var domain = new Gs2.Gs2Inbox.Domain.Model.MessageDomain[result?.Item.Length ?? 0];
            for (int i=0; i<result?.Item.Length; i++)
            {
                domain[i] = new Gs2.Gs2Inbox.Domain.Model.MessageDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result.Item[i]?.UserId,
                    result.Item[i]?.Name
                );
                var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName?.ToString() ?? null,
                this._userId?.ToString() ?? null,
                "Message"
            );
                var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                    result.Item[i].Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    result.Item[i],
                    result.Item[i].ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageDomain[]>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inbox.Model.Message> Messages(
        )
        {
            return new DescribeMessagesByUserIdIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._userId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inbox.Model.Message> MessagesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inbox.Model.Message> Messages(
            #endif
        #else
        public DescribeMessagesByUserIdIterator Messages(
        #endif
        )
        {
            return new DescribeMessagesByUserIdIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._userId
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

        public Gs2.Gs2Inbox.Domain.Model.MessageDomain Message(
            string messageName
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.MessageDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._userId,
                messageName
            );
        }

        public Gs2.Gs2Inbox.Domain.Model.ReceivedDomain Received(
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.ReceivedDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._userId
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

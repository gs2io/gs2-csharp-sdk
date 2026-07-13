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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Guild.Domain.Iterator;
using Gs2.Gs2Guild.Model.Cache;
using Gs2.Gs2Guild.Request;
using Gs2.Gs2Guild.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Guild.Domain.Model
{

    public partial class ReceiveMemberRequestDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GuildRestClient _client;
        public string NamespaceName { get; } = null!;
        public string GuildModelName { get; } = null!;
        public string GuildName { get; } = null!;
        public string FromUserId { get; } = null!;

        public ReceiveMemberRequestDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string guildModelName,
            string guildName,
            string fromUserId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GuildRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.GuildModelName = guildModelName;
            this.GuildName = guildName;
            this.FromUserId = fromUserId;
        }

    }

    public partial class ReceiveMemberRequestDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> GetReceiveRequestFuture(
            GetReceiveRequestByGuildNameRequest request
        ) => GetReceiveRequestAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> GetReceiveRequestAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> GetReceiveRequestAsync(
        #endif
            GetReceiveRequestByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName)
                .WithFromUserId(this.FromUserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.GetReceiveRequestByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> AcceptFuture(
            AcceptRequestByGuildNameRequest request
        ) => AcceptAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> AcceptAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> AcceptAsync(
        #endif
            AcceptRequestByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName)
                .WithFromUserId(this.FromUserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.AcceptRequestByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> RejectFuture(
            RejectRequestByGuildNameRequest request
        ) => RejectAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> RejectAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain> RejectAsync(
        #endif
            RejectRequestByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName)
                .WithFromUserId(this.FromUserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.RejectRequestByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

    }

    public partial class ReceiveMemberRequestDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Model.ReceiveMemberRequest> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Model.ReceiveMemberRequest> ModelAsync()
        #else
        public async Task<Gs2.Gs2Guild.Model.ReceiveMemberRequest> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                        (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                            this.NamespaceName,
                            this.GuildModelName,
                            this.GuildName,
                            null
                        ),
                        (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheKey(
                            this.FromUserId
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.FromUserId,
                    null
                );
                if (find) {
                    return value;
                }
                return null;
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Guild.Model.ReceiveMemberRequest> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Guild.Model.ReceiveMemberRequest> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Guild.Model.ReceiveMemberRequest> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                this.FromUserId,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    null
                ),
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheKey(
                    this.FromUserId
                ),
                callback,
                () =>
                {
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    null
                ),
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheKey(
                    this.FromUserId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

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
 *
 * deny overwrite
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

namespace Gs2.Gs2Guild.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GuildRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GuildRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> CreateGuildFuture(
            CreateGuildRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.CreateGuildFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Guild.Domain.Model.GuildDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.GuildModelName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> CreateGuildAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> CreateGuildAsync(
            #endif
            CreateGuildRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.CreateGuildAsync(request)
            );
            var domain = new Gs2.Gs2Guild.Domain.Model.GuildDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.GuildModelName,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> AssumeFuture(
            AssumeRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.AssumeFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this._gs2.Auth.AccessToken();
                domain.Token = result?.Token;
                domain.UserId = result?.UserId;
                domain.Expire = result?.Expire;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> AssumeAsync(
            #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> AssumeAsync(
            #endif
            AssumeRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.AssumeAsync(request)
            );
            var domain = this._gs2.Auth.AccessToken();
            domain.Token = result?.Token;
            domain.UserId = result?.UserId;
            domain.Expire = result?.Expire;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> SendRequestFuture(
            SendRequestRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.SendRequestFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Guild.Domain.Model.GuildDomain(
                    this._gs2,
                    this.NamespaceName,
                    request.GuildModelName,
                    request.TargetGuildName
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> SendRequestAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> SendRequestAsync(
            #endif
            SendRequestRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.SendRequestAsync(request)
            );
            var domain = new Gs2.Gs2Guild.Domain.Model.GuildDomain(
                this._gs2,
                this.NamespaceName,
                request.GuildModelName,
                request.TargetGuildName
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain> DeleteFuture(
            DeleteRequestRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteRequestFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    request.GuildModelName,
                    request.TargetGuildName
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain> DeleteAsync(
            #endif
            DeleteRequestRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteRequestAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = new Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                request.GuildModelName,
                request.TargetGuildName
            );
            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Guild.Model.SendMemberRequest> SendRequests(
            string guildModelName
        )
        {
            return new DescribeSendRequestsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                guildModelName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Guild.Model.SendMemberRequest> SendRequestsAsync(
            #else
        public DescribeSendRequestsIterator SendRequestsAsync(
            #endif
            string guildModelName
        )
        {
            return new DescribeSendRequestsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                guildModelName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSendRequests(
            Action<Gs2.Gs2Guild.Model.SendMemberRequest[]> callback,
            string guildModelName
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.SendMemberRequest>(
                (null as Gs2.Gs2Guild.Model.SendMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    guildModelName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSendRequestsWithInitialCallAsync(
            Action<Gs2.Gs2Guild.Model.SendMemberRequest[]> callback,
            string guildModelName
        )
        {
            var items = await SendRequestsAsync(
                guildModelName
            ).ToArrayAsync();
            var callbackId = SubscribeSendRequests(
                callback,
                guildModelName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSendRequests(
            ulong callbackId,
            string guildModelName
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.SendMemberRequest>(
                (null as Gs2.Gs2Guild.Model.SendMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    guildModelName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain SendMemberRequest(
            string guildModelName,
            string guildName
        ) {
            return new Gs2.Gs2Guild.Domain.Model.SendMemberRequestAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                guildModelName,
                guildName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Guild.Model.Guild> SearchGuilds(
            string guildModelName,
            string displayName = null,
            int[] attributes1 = null,
            int[] attributes2 = null,
            int[] attributes3 = null,
            int[] attributes4 = null,
            int[] attributes5 = null,
            string[] joinPolicies = null,
            bool? includeFullMembersGuild = null
        )
        {
            return new SearchGuildsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                guildModelName,
                this.AccessToken,
                displayName,
                attributes1,
                attributes2,
                attributes3,
                attributes4,
                attributes5,
                joinPolicies,
                includeFullMembersGuild
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Guild.Model.Guild> SearchGuildsAsync(
            #else
        public SearchGuildsIterator SearchGuildsAsync(
            #endif
            string guildModelName,
            string displayName = null,
            int[] attributes1 = null,
            int[] attributes2 = null,
            int[] attributes3 = null,
            int[] attributes4 = null,
            int[] attributes5 = null,
            string[] joinPolicies = null,
            bool? includeFullMembersGuild = null
        )
        {
            return new SearchGuildsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                guildModelName,
                this.AccessToken,
                displayName,
                attributes1,
                attributes2,
                attributes3,
                attributes4,
                attributes5,
                joinPolicies,
                includeFullMembersGuild
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif
        
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Guild.Model.JoinedGuild> JoinedGuilds(
            string guildModelName = null
        )
        {
            return new DescribeJoinedGuildsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                guildModelName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Guild.Model.JoinedGuild> JoinedGuildsAsync(
            #else
        public DescribeJoinedGuildsIterator JoinedGuildsAsync(
            #endif
            string guildModelName = null
        )
        {
            return new DescribeJoinedGuildsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                guildModelName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeJoinedGuilds(
            Action<Gs2.Gs2Guild.Model.JoinedGuild[]> callback,
            string guildModelName = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.JoinedGuild>(
                (null as Gs2.Gs2Guild.Model.JoinedGuild).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeJoinedGuildsWithInitialCallAsync(
            Action<Gs2.Gs2Guild.Model.JoinedGuild[]> callback,
            string guildModelName = null
        )
        {
            var items = await JoinedGuildsAsync(
                guildModelName
            ).ToArrayAsync();
            var callbackId = SubscribeJoinedGuilds(
                callback,
                guildModelName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeJoinedGuilds(
            ulong callbackId,
            string guildModelName = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.JoinedGuild>(
                (null as Gs2.Gs2Guild.Model.JoinedGuild).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Guild.Domain.Model.JoinedGuildAccessTokenDomain JoinedGuild(
            string guildModelName,
            string guildName
        ) {
            return new Gs2.Gs2Guild.Domain.Model.JoinedGuildAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                guildModelName,
                guildName
            );
        }

    }
}

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

    public partial class GuildAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GuildRestClient _client;
        public string NamespaceName { get; } = null!;
        public string GuildModelName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string GuildName => this.AccessToken.UserId;

        public GuildAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string guildModelName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GuildRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.GuildModelName = guildModelName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Guild.Model.Guild> GetFuture(
            GetGuildRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Model.Guild> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.GetGuildFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Model.Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Guild.Model.Guild> GetAsync(
            #else
        private async Task<Gs2.Gs2Guild.Model.Guild> GetAsync(
            #endif
            GetGuildRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                () => this._client.GetGuildAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> UpdateFuture(
            UpdateGuildRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithGuildModelName(this.GuildModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.UpdateGuildFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> UpdateAsync(
            #endif
            UpdateGuildRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithGuildModelName(this.GuildModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                () => this._client.UpdateGuildAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> DeleteMemberFuture(
            DeleteMemberRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.DeleteMemberFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> DeleteMemberAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> DeleteMemberAsync(
            #endif
            DeleteMemberRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithAccessToken(this.AccessToken?.Token);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.DeleteMemberAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> UpdateMemberRoleFuture(
            UpdateMemberRoleRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.UpdateMemberRoleFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> UpdateMemberRoleAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> UpdateMemberRoleAsync(
            #endif
            UpdateMemberRoleRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                () => this._client.UpdateMemberRoleAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> DeleteFuture(
            DeleteGuildRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.DeleteGuildFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> DeleteAsync(
            #endif
            DeleteGuildRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithAccessToken(this.AccessToken?.Token);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.DeleteGuildAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> VerifyIncludeMemberFuture(
            VerifyIncludeMemberRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.VerifyIncludeMemberFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> VerifyIncludeMemberAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> VerifyIncludeMemberAsync(
            #endif
            VerifyIncludeMemberRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                () => this._client.VerifyIncludeMemberAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain> AddIgnoreUserFuture(
            AddIgnoreUserRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.AddIgnoreUserFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    request.GuildModelName,
                    this.AccessToken
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain> AddIgnoreUserAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain> AddIgnoreUserAsync(
            #endif
            AddIgnoreUserRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                () => this._client.AddIgnoreUserAsync(request)
            );
            var domain = new Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain(
                this._gs2,
                request.NamespaceName,
                request.GuildModelName,
                this.AccessToken
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain> PromoteSeniorMemberFuture(
            PromoteSeniorMemberRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.PromoteSeniorMemberFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.GuildModelName,
                    this.AccessToken
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain> PromoteSeniorMemberAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain> PromoteSeniorMemberAsync(
            #endif
            PromoteSeniorMemberRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                () => this._client.PromoteSeniorMemberAsync(request)
            );
            var domain = new Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.GuildModelName,
                this.AccessToken
            );

            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Guild.Model.ReceiveMemberRequest> ReceiveRequests(
        )
        {
            return new DescribeReceiveRequestsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.GuildModelName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Guild.Model.ReceiveMemberRequest> ReceiveRequestsAsync(
            #else
        public DescribeReceiveRequestsIterator ReceiveRequestsAsync(
            #endif
        )
        {
            return new DescribeReceiveRequestsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.GuildModelName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeReceiveRequests(
            Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeReceiveRequestsWithInitialCallAsync(
            Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest[]> callback
        )
        {
            var items = await ReceiveRequestsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeReceiveRequests(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeReceiveRequests(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestAccessTokenDomain ReceiveMemberRequest(
            string fromUserId
        ) {
            return new Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.GuildModelName,
                this.AccessToken,
                fromUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Guild.Model.IgnoreUser> IgnoreUsers(
        )
        {
            return new DescribeIgnoreUsersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.GuildModelName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Guild.Model.IgnoreUser> IgnoreUsersAsync(
            #else
        public DescribeIgnoreUsersIterator IgnoreUsersAsync(
            #endif
        )
        {
            return new DescribeIgnoreUsersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.GuildModelName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeIgnoreUsers(
            Action<Gs2.Gs2Guild.Model.IgnoreUser[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.IgnoreUser>(
                (null as Gs2.Gs2Guild.Model.IgnoreUser).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeIgnoreUsersWithInitialCallAsync(
            Action<Gs2.Gs2Guild.Model.IgnoreUser[]> callback
        )
        {
            var items = await IgnoreUsersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeIgnoreUsers(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeIgnoreUsers(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.IgnoreUser>(
                (null as Gs2.Gs2Guild.Model.IgnoreUser).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain IgnoreUser(
        ) {
            return new Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.GuildModelName,
                this.AccessToken
            );
        }

        public Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain LastGuildMasterActivity(
        ) {
            return new Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.GuildModelName,
                this.AccessToken
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Model.Guild> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Model.Guild> self)
            {
                var (value, find) = (null as Gs2.Gs2Guild.Model.Guild).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Guild.Model.Guild).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    () => this.GetFuture(
                        new GetGuildRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Model.Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Model.Guild> ModelAsync()
            #else
        public async Task<Gs2.Gs2Guild.Model.Guild> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Guild.Model.Guild).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Guild.Model.Guild).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                () => this.GetAsync(
                    new GetGuildRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Guild.Model.Guild> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Guild.Model.Guild> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Guild.Model.Guild> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Guild.Model.Guild).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Guild.Model.Guild> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Guild.Model.Guild).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Guild.Model.Guild).CacheKey(
                    this.GuildModelName,
                    this.GuildName
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
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
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Guild.Model.Guild>(
                (null as Gs2.Gs2Guild.Model.Guild).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Guild.Model.Guild).CacheKey(
                    this.GuildModelName,
                    this.GuildName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Guild.Model.Guild> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Guild.Model.Guild> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Guild.Model.Guild> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}

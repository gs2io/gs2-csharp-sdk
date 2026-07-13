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
using System.Collections;
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

    public partial class GuildAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GuildRestClient _client;
        public string NamespaceName { get; } = null!;
        public string GuildModelName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string GuildName => this.AccessToken.UserId;
/* diff --- start
        public string UserId { get; } = null!;
 diff --- end */

        public GuildAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string guildModelName,
/* diff --- start
            AccessToken accessToken,
            string userId
 diff --- end */
            AccessToken accessToken /* diff +++ */
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GuildRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.GuildModelName = guildModelName;
            this.AccessToken = accessToken;
/* diff --- start
            this.UserId = userId;
 diff --- end */
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Guild.Model.Guild> GetFuture(
            GetGuildRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                this.AccessToken?.TimeOffset,
                () => this._client.GetGuildAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> UpdateFuture(
            UpdateGuildRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                this.AccessToken?.TimeOffset,
                () => this._client.UpdateGuildAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> DeleteMemberFuture(
            DeleteMemberRequest request
        ) => DeleteMemberAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteMemberAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> UpdateMemberRoleFuture(
            UpdateMemberRoleRequest request
        ) => UpdateMemberRoleAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                this.AccessToken?.TimeOffset,
                () => this._client.UpdateMemberRoleAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff +++ start */
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> BatchUpdateMemberRoleFuture(
            BatchUpdateMemberRoleRequest request
        ) => BatchUpdateMemberRoleAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> BatchUpdateMemberRoleAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> BatchUpdateMemberRoleAsync(
        #endif
            BatchUpdateMemberRoleRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                this.AccessToken?.TimeOffset,
                () => this._client.BatchUpdateMemberRoleAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff +++ end */
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> DeleteFuture(
            DeleteGuildRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteGuildAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildAccessTokenDomain> VerifyIncludeMemberFuture(
            VerifyIncludeMemberRequest request
        ) => VerifyIncludeMemberAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                this.AccessToken?.TimeOffset,
                () => this._client.VerifyIncludeMemberAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.IgnoreUserAccessTokenDomain> AddIgnoreUserFuture(
            AddIgnoreUserRequest request
        ) => AddIgnoreUserAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
/* diff --- start
                .WithAccessToken(this.AccessToken?.Token)
                .WithUserId(this.UserId);
 diff --- end */
                .WithAccessToken(this.AccessToken?.Token); /* diff +++ */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                this.AccessToken?.TimeOffset,
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityAccessTokenDomain> PromoteSeniorMemberFuture(
            PromoteSeniorMemberRequest request
        ) => PromoteSeniorMemberAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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
                this.AccessToken?.TimeOffset,
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
            );
        }

        public ulong SubscribeReceiveRequests(
            Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.AccessToken?.TimeOffset
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
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
/* diff --- start
                            callback.Invoke(await ReceiveRequestsAsync(
                            ).ToArrayAsync());
 diff --- end */
                            callback.Invoke(await ReceiveRequestsAsync().ToArrayAsync()); /* diff +++ */
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeReceiveRequestsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeReceiveRequestsWithInitialCallAsync(
        #endif
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

        public void UnsubscribeReceiveRequests(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateReceiveRequests(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.AccessToken?.TimeOffset
                )
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
            );
        }

        public ulong SubscribeIgnoreUsers(
            Action<Gs2.Gs2Guild.Model.IgnoreUser[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.IgnoreUser>(
                (null as Gs2.Gs2Guild.Model.IgnoreUser).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.AccessToken?.TimeOffset
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
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
/* diff --- start
                            callback.Invoke(await IgnoreUsersAsync(
                            ).ToArrayAsync());
 diff --- end */
                            callback.Invoke(await IgnoreUsersAsync().ToArrayAsync()); /* diff +++ */
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeIgnoreUsersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeIgnoreUsersWithInitialCallAsync(
        #endif
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

        public void UnsubscribeIgnoreUsers(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.IgnoreUser>(
                (null as Gs2.Gs2Guild.Model.IgnoreUser).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateIgnoreUsers(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Guild.Model.IgnoreUser>(
                (null as Gs2.Gs2Guild.Model.IgnoreUser).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.AccessToken?.TimeOffset
                )
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
        public IFuture<Gs2.Gs2Guild.Model.Guild> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Model.Guild> ModelAsync()
        #else
        public async Task<Gs2.Gs2Guild.Model.Guild> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Guild.Model.Guild>(
                        (null as Gs2.Gs2Guild.Model.Guild).CacheParentKey(
                            this.NamespaceName,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Guild.Model.Guild).CacheKey(
                            this.GuildModelName,
                            this.GuildName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Guild.Model.Guild).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Guild.Model.Guild).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetGuildRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Guild.Model.Guild).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                this.AccessToken?.TimeOffset
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Guild.Model.Guild).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                this.AccessToken?.TimeOffset,
                () => this.GetAsync(
                    new GetGuildRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Guild.Model.Guild> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Guild.Model.Guild> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Guild.Model.Guild> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Guild.Model.Guild> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Guild.Model.Guild> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Guild.Model.Guild> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Guild.Model.Guild).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Guild.Model.Guild> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Guild.Model.Guild).CacheParentKey(
                    this.NamespaceName,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Guild.Model.Guild).CacheKey(
                    this.GuildModelName,
                    this.GuildName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Guild.Model.Guild>(
                (null as Gs2.Gs2Guild.Model.Guild).CacheParentKey(
                    this.NamespaceName,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Guild.Model.Guild).CacheKey(
                    this.GuildModelName,
                    this.GuildName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Guild.Model.Guild> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Guild.Model.Guild> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
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

    }
}

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

    public partial class GuildDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GuildRestClient _client;
        public string NamespaceName { get; } = null!;
        public string GuildModelName { get; } = null!;
        public string GuildName { get; } = null!;

        public GuildDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string guildModelName,
            string guildName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GuildRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.GuildModelName = guildModelName;
            this.GuildName = guildName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Guild.Model.ReceiveMemberRequest> ReceiveRequestsByGuildName(
        )
        {
            return new DescribeReceiveRequestsByGuildNameIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Guild.Model.ReceiveMemberRequest> ReceiveRequestsByGuildNameAsync(
        #else
        public DescribeReceiveRequestsByGuildNameIterator ReceiveRequestsByGuildNameAsync(
        #endif
        )
        {
            return new DescribeReceiveRequestsByGuildNameIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName
            );
        }

        public ulong SubscribeReceiveRequestsByGuildName(
            Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    null
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
                            callback.Invoke(await ReceiveRequestsByGuildNameAsync().ToArrayAsync());
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
        public async UniTask<ulong> SubscribeReceiveRequestsByGuildNameWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeReceiveRequestsByGuildNameWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Guild.Model.ReceiveMemberRequest[]> callback
        )
        {
            var items = await ReceiveRequestsByGuildNameAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeReceiveRequestsByGuildName(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeReceiveRequestsByGuildName(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateReceiveRequestsByGuildName(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Guild.Model.ReceiveMemberRequest>(
                (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    null
                )
            );
        }

        public Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain ReceiveMemberRequest(
            string fromUserId
        ) {
            return new Gs2.Gs2Guild.Domain.Model.ReceiveMemberRequestDomain(
                this._gs2,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                fromUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Guild.Model.IgnoreUser> IgnoreUsersByGuildName(
        )
        {
            return new DescribeIgnoreUsersByGuildNameIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Guild.Model.IgnoreUser> IgnoreUsersByGuildNameAsync(
        #else
        public DescribeIgnoreUsersByGuildNameIterator IgnoreUsersByGuildNameAsync(
        #endif
        )
        {
            return new DescribeIgnoreUsersByGuildNameIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName
            );
        }

        public ulong SubscribeIgnoreUsersByGuildName(
            Action<Gs2.Gs2Guild.Model.IgnoreUser[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.IgnoreUser>(
                (null as Gs2.Gs2Guild.Model.IgnoreUser).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    null
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
                            callback.Invoke(await IgnoreUsersByGuildNameAsync().ToArrayAsync());
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
        public async UniTask<ulong> SubscribeIgnoreUsersByGuildNameWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeIgnoreUsersByGuildNameWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Guild.Model.IgnoreUser[]> callback
        )
        {
            var items = await IgnoreUsersByGuildNameAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeIgnoreUsersByGuildName(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeIgnoreUsersByGuildName(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.IgnoreUser>(
                (null as Gs2.Gs2Guild.Model.IgnoreUser).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateIgnoreUsersByGuildName(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Guild.Model.IgnoreUser>(
                (null as Gs2.Gs2Guild.Model.IgnoreUser).CacheParentKey(
                    this.NamespaceName,
                    this.GuildModelName,
                    this.GuildName,
                    null
                )
            );
        }

        public Gs2.Gs2Guild.Domain.Model.IgnoreUserDomain IgnoreUser(
        ) {
            return new Gs2.Gs2Guild.Domain.Model.IgnoreUserDomain(
                this._gs2,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName
            );
        }

        public Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityDomain LastGuildMasterActivity(
        ) {
            return new Gs2.Gs2Guild.Domain.Model.LastGuildMasterActivityDomain(
                this._gs2,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName
            );
        }

    }

    public partial class GuildDomain {

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
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.GetGuildAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> UpdateFuture(
            UpdateGuildByGuildNameRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> UpdateAsync(
        #endif
            UpdateGuildByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildName(this.GuildName)
                .WithGuildModelName(this.GuildModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.UpdateGuildByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> DeleteMemberFuture(
            DeleteMemberByGuildNameRequest request
        ) => DeleteMemberAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> DeleteMemberAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> DeleteMemberAsync(
        #endif
            DeleteMemberByGuildNameRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.GuildName,
                    null,
                    () => this._client.DeleteMemberByGuildNameAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> UpdateMemberRoleFuture(
            UpdateMemberRoleByGuildNameRequest request
        ) => UpdateMemberRoleAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> UpdateMemberRoleAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> UpdateMemberRoleAsync(
        #endif
            UpdateMemberRoleByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.UpdateMemberRoleByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> BatchUpdateMemberRoleFuture(
            BatchUpdateMemberRoleByGuildNameRequest request
        ) => BatchUpdateMemberRoleAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> BatchUpdateMemberRoleAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> BatchUpdateMemberRoleAsync(
        #endif
            BatchUpdateMemberRoleByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.BatchUpdateMemberRoleByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> DeleteFuture(
            DeleteGuildByGuildNameRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> DeleteAsync(
        #endif
            DeleteGuildByGuildNameRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.GuildName,
                    null,
                    () => this._client.DeleteGuildByGuildNameAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> IncreaseMaximumCurrentMaximumMemberCountFuture(
            IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) => IncreaseMaximumCurrentMaximumMemberCountAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> IncreaseMaximumCurrentMaximumMemberCountAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> IncreaseMaximumCurrentMaximumMemberCountAsync(
        #endif
            IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.IncreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> DecreaseMaximumCurrentMaximumMemberCountFuture(
            DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) => DecreaseMaximumCurrentMaximumMemberCountAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> DecreaseMaximumCurrentMaximumMemberCountAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> DecreaseMaximumCurrentMaximumMemberCountAsync(
        #endif
            DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.DecreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> VerifyCurrentMaximumMemberCountFuture(
            VerifyCurrentMaximumMemberCountByGuildNameRequest request
        ) => VerifyCurrentMaximumMemberCountAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> VerifyCurrentMaximumMemberCountAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> VerifyCurrentMaximumMemberCountAsync(
        #endif
            VerifyCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.VerifyCurrentMaximumMemberCountByGuildNameAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> VerifyIncludeMemberFuture(
            VerifyIncludeMemberByUserIdRequest request
        ) => VerifyIncludeMemberAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> VerifyIncludeMemberAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> VerifyIncludeMemberAsync(
        #endif
            VerifyIncludeMemberByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildModelName(this.GuildModelName)
                .WithGuildName(this.GuildName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.VerifyIncludeMemberByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> SetMaximumCurrentMaximumMemberCountFuture(
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) => SetMaximumCurrentMaximumMemberCountAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Domain.Model.GuildDomain> SetMaximumCurrentMaximumMemberCountAsync(
        #else
        public async Task<Gs2.Gs2Guild.Domain.Model.GuildDomain> SetMaximumCurrentMaximumMemberCountAsync(
        #endif
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithGuildName(this.GuildName)
                .WithGuildModelName(this.GuildModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.GuildName,
                null,
                () => this._client.SetMaximumCurrentMaximumMemberCountByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }

    }

    public partial class GuildDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Model.Guild> ModelFuture(
            AccessToken accessToken
        ) => ModelAsync(accessToken).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Guild.Model.Guild> ModelAsync(
        #else
        public async Task<Gs2.Gs2Guild.Model.Guild> ModelAsync(
        #endif
            AccessToken accessToken
        ) {
            var (value, find) = (null as Gs2.Gs2Guild.Model.Guild).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                null
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Guild.Model.Guild).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                null,
                () => this.GetAsync(
                    new GetGuildRequest()
                        .WithAccessToken(accessToken.Token)
                )
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Guild.Model.Guild> Model(
            AccessToken accessToken
        )
        {
            return await ModelAsync(accessToken);
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Guild.Model.Guild> Model(
            AccessToken accessToken
        )
        {
            return ModelFuture(accessToken);
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Guild.Model.Guild> Model(
            AccessToken accessToken
        )
        {
            return await ModelAsync(accessToken);
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Guild.Model.Guild).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.GuildModelName,
                this.GuildName,
                null
            );
        }

        public ulong Subscribe(
            AccessToken accessToken,
            Action<Gs2.Gs2Guild.Model.Guild> callback
        )
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Guild.Model.Guild).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Guild.Model.Guild).CacheKey(
                    this.GuildModelName,
                    this.GuildName
                ),
                callback,
                () =>
                {
                    ModelAsync(accessToken).Forget();
                }
            );
        }

        public void Unsubscribe(
            ulong callbackId
        )
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Guild.Model.Guild>(
                (null as Gs2.Gs2Guild.Model.Guild).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Guild.Model.Guild).CacheKey(
                    this.GuildModelName,
                    this.GuildName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(
            AccessToken accessToken,
            Action<Gs2.Gs2Guild.Model.Guild> callback
        ) => SubscribeWithInitialCallAsync(accessToken, callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(
            AccessToken accessToken,
            Action<Gs2.Gs2Guild.Model.Guild> callback
        )
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(
            AccessToken accessToken,
            Action<Gs2.Gs2Guild.Model.Guild> callback
        )
        #endif
        {
            var item = await ModelAsync(accessToken);
            var callbackId = Subscribe(accessToken, callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

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

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
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
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeReceiveRequestsByGuildName(
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
        public async UniTask<ulong> SubscribeReceiveRequestsByGuildNameWithInitialCallAsync(
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
        #endif

        public void UnsubscribeReceiveRequestsByGuildName(
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

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
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
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeIgnoreUsersByGuildName(
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
        public async UniTask<ulong> SubscribeIgnoreUsersByGuildNameWithInitialCallAsync(
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
        #endif

        public void UnsubscribeIgnoreUsersByGuildName(
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
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Model.Guild> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
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
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> UpdateFuture(
            UpdateGuildByGuildNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildName(this.GuildName)
                    .WithGuildModelName(this.GuildModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.UpdateGuildByGuildNameFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                () => this._client.UpdateGuildByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> DeleteMemberFuture(
            DeleteMemberByGuildNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.DeleteMemberByGuildNameFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                    () => this._client.DeleteMemberByGuildNameAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> UpdateMemberRoleFuture(
            UpdateMemberRoleByGuildNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.UpdateMemberRoleByGuildNameFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                () => this._client.UpdateMemberRoleByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> DeleteFuture(
            DeleteGuildByGuildNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.DeleteGuildByGuildNameFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                    () => this._client.DeleteGuildByGuildNameAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> IncreaseMaximumCurrentMaximumMemberCountFuture(
            IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.IncreaseMaximumCurrentMaximumMemberCountByGuildNameFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                () => this._client.IncreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> DecreaseMaximumCurrentMaximumMemberCountFuture(
            DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.DecreaseMaximumCurrentMaximumMemberCountByGuildNameFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                () => this._client.DecreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> VerifyCurrentMaximumMemberCountFuture(
            VerifyCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.VerifyCurrentMaximumMemberCountByGuildNameFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                () => this._client.VerifyCurrentMaximumMemberCountByGuildNameAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> VerifyIncludeMemberFuture(
            VerifyIncludeMemberByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildModelName(this.GuildModelName)
                    .WithGuildName(this.GuildName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.VerifyIncludeMemberByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                () => this._client.VerifyIncludeMemberByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> SetMaximumCurrentMaximumMemberCountFuture(
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithGuildName(this.GuildName)
                    .WithGuildModelName(this.GuildModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.GuildName,
                    () => this._client.SetMaximumCurrentMaximumMemberCountByGuildNameFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.GuildDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
                () => this._client.SetMaximumCurrentMaximumMemberCountByGuildNameAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

    }

    public partial class GuildDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Model.Guild> ModelFuture(
            AccessToken accessToken
        )
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
                            .WithAccessToken(accessToken.Token)
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
                        .WithAccessToken(accessToken.Token)
                )
            );
        }
        #endif

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
                this.GuildName
            );
        }

        public ulong Subscribe(
            AccessToken accessToken,
            Action<Gs2.Gs2Guild.Model.Guild> callback
        )
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
                    ModelAsync(accessToken).Forget();
            #else
                    ModelAsync(accessToken);
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(
            ulong callbackId
        )
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
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(
            AccessToken accessToken,
            Action<Gs2.Gs2Guild.Model.Guild> callback
        )
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture(accessToken);
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(accessToken, callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
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
        #endif

    }
}

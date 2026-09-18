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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Identifier.Domain.Iterator;
using Gs2.Gs2Identifier.Model.Cache;
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
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

namespace Gs2.Gs2Identifier.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdentifierRestClient _client;
        public string UserName { get; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string userName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdentifierRestClient(
                gs2.RestSession
            );
            this.UserName = userName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Identifier.Model.Identifier> Identifiers(
        )
        {
            return new DescribeIdentifiersIterator(
                this._gs2,
                this._client,
                this.UserName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.Identifier> IdentifiersAsync(
        #else
        public DescribeIdentifiersIterator IdentifiersAsync(
        #endif
        )
        {
            return new DescribeIdentifiersIterator(
                this._gs2,
                this._client,
                this.UserName
            );
        }

        public ulong SubscribeIdentifiers(
            Action<Gs2.Gs2Identifier.Model.Identifier[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Identifier.Model.Identifier>(
                (null as Gs2.Gs2Identifier.Model.Identifier).CacheParentKey(
                    this.UserName,
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
/* diff --- start
                            callback.Invoke(await IdentifiersAsync(
                            ).ToArrayAsync());
 diff --- end */
                            callback.Invoke(await IdentifiersAsync().ToArrayAsync()); /* diff +++ */
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
        public async UniTask<ulong> SubscribeIdentifiersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeIdentifiersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Identifier.Model.Identifier[]> callback
        )
        {
            var items = await IdentifiersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeIdentifiers(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeIdentifiers(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.Identifier>(
                (null as Gs2.Gs2Identifier.Model.Identifier).CacheParentKey(
                    this.UserName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateIdentifiers(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Identifier.Model.Identifier>(
                (null as Gs2.Gs2Identifier.Model.Identifier).CacheParentKey(
                    this.UserName,
                    null
                )
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<string> AttachedGuards(
            string clientId = null
        )
        {
            return new DescribeAttachedGuardsIterator(
                this._gs2,
                this._client,
                this.UserName,
                clientId
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<string> AttachedGuardsAsync(
        #else
        public DescribeAttachedGuardsIterator AttachedGuardsAsync(
        #endif
            string clientId = null
        )
        {
            return new DescribeAttachedGuardsIterator(
                this._gs2,
                this._client,
                this.UserName,
                clientId
/* diff --- start
            );
        }

        public ulong SubscribeAttachedGuards(
            Action<string[]> callback,
            string clientId = null
        )
        {
            return this._gs2.Cache.ListSubscribe<string>(
                (null as string).CacheParentKey(
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => clientId == null || item.ClientId == clientId)
                    .ToArray()),
                () =>
                {
        //#if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        //#else
                    async Task Impl() {
        //#endif
                        try {
        //#if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        //#endif
                            callback.Invoke(await AttachedGuardsAsync(
                                clientId
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        //#if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeAttachedGuardsWithInitialCallAsync(
        //#else
        public async Task<ulong> SubscribeAttachedGuardsWithInitialCallAsync(
        //#endif
            Action<string[]> callback,
            string clientId = null
        )
        {
            var items = await AttachedGuardsAsync(
                clientId
            ).ToArrayAsync();
            var callbackId = SubscribeAttachedGuards(
                callback,
                clientId
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeAttachedGuards(
            ulong callbackId,
            string clientId = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<string>(
                (null as string).CacheParentKey(
                    null
                ),
                callbackId
            );
        }

        public void InvalidateAttachedGuards(
            string clientId = null
        )
        {
            this._gs2.Cache.ClearListCache<string>(
                (null as string).CacheParentKey(
                    null
                )
 diff --- end */
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.IdentifierDomain Identifier(
            string clientId
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
                this._gs2,
                this.UserName,
                clientId
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.PasswordDomain Password(
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.PasswordDomain(
                this._gs2,
                this.UserName
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain AttachSecurityPolicy(
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain(
                this._gs2,
                this.UserName
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> UpdateFuture(
            UpdateUserRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.UserDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.UserDomain> UpdateAsync(
        #endif
            UpdateUserRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserName(this.UserName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateUserAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Identifier.Model.User> GetFuture(
            GetUserRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Identifier.Model.User> GetAsync(
        #else
        private async Task<Gs2.Gs2Identifier.Model.User> GetAsync(
        #endif
            GetUserRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserName(this.UserName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetUserAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> DeleteFuture(
            DeleteUserRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.UserDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.UserDomain> DeleteAsync(
        #endif
            DeleteUserRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithUserName(this.UserName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeleteUserAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifierFuture(
            CreateIdentifierRequest request
        ) => CreateIdentifierAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifierAsync(
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifierAsync(
        #endif
            CreateIdentifierRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserName(this.UserName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateIdentifierAsync(request)
            );
            var domain = new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
                this._gs2,
                result?.Item?.UserName,
                result?.Item?.ClientId
            );
            domain.ClientSecret = result?.ClientSecret;

            return domain;
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.User> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Model.User> ModelAsync()
        #else
        public async Task<Gs2.Gs2Identifier.Model.User> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Identifier.Model.User>(
                        (null as Gs2.Gs2Identifier.Model.User).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Identifier.Model.User).CacheKey(
                            this.UserName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Identifier.Model.User).GetCache(
                    this._gs2.Cache,
                    this.UserName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Identifier.Model.User).FetchAsync(
                    this._gs2.Cache,
                    this.UserName,
                    null,
                    () => this.GetAsync(
                        new GetUserRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Identifier.Model.User).GetCache(
                this._gs2.Cache,
                this.UserName,
                null
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Identifier.Model.User).FetchAsync(
                this._gs2.Cache,
                this.UserName,
                null,
                () => this.GetAsync(
                    new GetUserRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Identifier.Model.User> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Identifier.Model.User> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Identifier.Model.User> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Identifier.Model.User> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Identifier.Model.User> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Identifier.Model.User> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Identifier.Model.User).DeleteCache(
                this._gs2.Cache,
                this.UserName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Identifier.Model.User> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Identifier.Model.User).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Identifier.Model.User).CacheKey(
                    this.UserName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Identifier.Model.User>(
                (null as Gs2.Gs2Identifier.Model.User).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Identifier.Model.User).CacheKey(
                    this.UserName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Identifier.Model.User> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Identifier.Model.User> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Identifier.Model.User> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Identifier.Model.User> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

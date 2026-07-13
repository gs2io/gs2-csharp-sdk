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
using Gs2.Gs2Friend.Domain.Iterator;
using Gs2.Gs2Friend.Model.Cache;
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
using Gs2.Gs2Friend.Model;
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

namespace Gs2.Gs2Friend.Domain.Model
{

    public partial class SendFriendRequestDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string TargetUserId { get; } = null!;

        public SendFriendRequestDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string targetUserId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.TargetUserId = targetUserId;
        }

    }

    public partial class SendFriendRequestDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Friend.Model.SendFriendRequest> GetFuture(
            GetSendRequestByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Friend.Model.SendFriendRequest> GetAsync(
        #else
        private async Task<Gs2.Gs2Friend.Model.SendFriendRequest> GetAsync(
        #endif
            GetSendRequestByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithTargetUserId(this.TargetUserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetSendRequestByUserIdAsync(request)
            );
            return result?.Item == null ? null : new SendFriendRequest {
                UserId = result.Item.UserId,
                TargetUserId = result.Item.TargetUserId,
            };
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain> DeleteFuture(
            DeleteRequestByUserIdRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain> DeleteAsync(
        #endif
            DeleteRequestByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithTargetUserId(this.TargetUserId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteRequestByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = new Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain(
                this._gs2,
                this.NamespaceName,
                request.UserId,
                request?.TargetUserId
            );
            return domain;
        }

    }

    public partial class SendFriendRequestDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Model.SendFriendRequest> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Model.SendFriendRequest> ModelAsync()
        #else
        public async Task<Gs2.Gs2Friend.Model.SendFriendRequest> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.SendFriendRequest>(
                        (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            null
                        ),
                        (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                            this.TargetUserId
                        )
                    ).LockAsync()) {
                if (this.UserId == null) {
                    throw new NullReferenceException();
                }
                var (value, find) = this._gs2.Cache.Get<Gs2.Gs2Friend.Model.SendFriendRequest>(
                    (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        null
                    ),
                    (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                        this.TargetUserId
                    )
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Friend.Model.SendFriendRequest).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.TargetUserId,
                    null,
                    () => this.GetAsync(
                        new GetSendRequestByUserIdRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Friend.Model.SendFriendRequest> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Friend.Model.SendFriendRequest> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Friend.Model.SendFriendRequest> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Friend.Model.SendFriendRequest).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.TargetUserId,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Friend.Model.SendFriendRequest> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                    this.TargetUserId
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
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Friend.Model.SendFriendRequest>(
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                    this.TargetUserId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Friend.Model.SendFriendRequest> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.SendFriendRequest> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.SendFriendRequest> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}

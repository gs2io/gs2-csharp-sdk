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
using Gs2.Gs2Schedule.Domain.Iterator;
using Gs2.Gs2Schedule.Model.Cache;
using Gs2.Gs2Schedule.Request;
using Gs2.Gs2Schedule.Result;
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

namespace Gs2.Gs2Schedule.Domain.Model
{

    public partial class TriggerDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ScheduleRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string TriggerName { get; } = null!;

        public TriggerDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string triggerName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ScheduleRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.TriggerName = triggerName;
        }

    }

    public partial class TriggerDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Schedule.Model.Trigger> GetFuture(
            GetTriggerByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Model.Trigger> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithTriggerName(this.TriggerName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.GetTriggerByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Model.Trigger>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Schedule.Model.Trigger> GetAsync(
            #else
        private async Task<Gs2.Gs2Schedule.Model.Trigger> GetAsync(
            #endif
            GetTriggerByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithTriggerName(this.TriggerName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetTriggerByUserIdAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> TriggerFuture(
            TriggerByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithTriggerName(this.TriggerName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.TriggerByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> TriggerAsync(
            #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> TriggerAsync(
            #endif
            TriggerByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithTriggerName(this.TriggerName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.TriggerByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> ExtendFuture(
            ExtendTriggerByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithTriggerName(this.TriggerName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.ExtendTriggerByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> ExtendAsync(
            #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> ExtendAsync(
            #endif
            ExtendTriggerByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithTriggerName(this.TriggerName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.ExtendTriggerByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> DeleteFuture(
            DeleteTriggerByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithTriggerName(this.TriggerName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteTriggerByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> DeleteAsync(
            #endif
            DeleteTriggerByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithTriggerName(this.TriggerName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteTriggerByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> VerifyFuture(
            VerifyTriggerByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithTriggerName(this.TriggerName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.VerifyTriggerByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Domain.Model.TriggerDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> VerifyAsync(
            #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.TriggerDomain> VerifyAsync(
            #endif
            VerifyTriggerByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithTriggerName(this.TriggerName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyTriggerByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

    }

    public partial class TriggerDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Model.Trigger> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Model.Trigger> self)
            {
                var (value, find) = (null as Gs2.Gs2Schedule.Model.Trigger).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.TriggerName,
                    null
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Schedule.Model.Trigger).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.TriggerName,
                    null,
                    () => this.GetFuture(
                        new GetTriggerByUserIdRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Model.Trigger>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Schedule.Model.Trigger> ModelAsync()
            #else
        public async Task<Gs2.Gs2Schedule.Model.Trigger> ModelAsync()
            #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Schedule.Model.Trigger>(
                        (null as Gs2.Gs2Schedule.Model.Trigger).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            null
                        ),
                        (null as Gs2.Gs2Schedule.Model.Trigger).CacheKey(
                            this.TriggerName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Schedule.Model.Trigger).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.TriggerName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Schedule.Model.Trigger).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.TriggerName,
                    null,
                    () => this.GetAsync(
                        new GetTriggerByUserIdRequest()
                    )
                );
            }
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Schedule.Model.Trigger> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Schedule.Model.Trigger> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Schedule.Model.Trigger> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Schedule.Model.Trigger).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.TriggerName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Schedule.Model.Trigger> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Schedule.Model.Trigger).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Schedule.Model.Trigger).CacheKey(
                    this.TriggerName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Schedule.Model.Trigger>(
                (null as Gs2.Gs2Schedule.Model.Trigger).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Schedule.Model.Trigger).CacheKey(
                    this.TriggerName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Schedule.Model.Trigger> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Schedule.Model.Trigger> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Schedule.Model.Trigger> callback)
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

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
    public partial class EventAccessTokenDomain
    {
        public Gs2.Gs2Schedule.Domain.Model.RepeatScheduleAccessTokenDomain RepeatSchedule(
        ) {
            return new Gs2.Gs2Schedule.Domain.Model.RepeatScheduleAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.EventName,
                this.InSchedule
            );
        }
    }

    public partial class RepeatScheduleAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ScheduleRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string EventName { get; } = null!;
        public bool? InSchedule { get; set; } = null!;

        public RepeatScheduleAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string eventName,
            bool? inSchedule = null
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ScheduleRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.EventName = eventName;
            this.InSchedule = inSchedule;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Schedule.Model.RepeatSchedule> GetFuture(
            GetEventRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Model.RepeatSchedule> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithEventName(this.EventName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithIsInSchedule(this.InSchedule);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.GetEventFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.RepeatSchedule);
            }
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Model.RepeatSchedule>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Schedule.Model.RepeatSchedule> GetAsync(
            #else
        private async Task<Gs2.Gs2Schedule.Model.RepeatSchedule> GetAsync(
            #endif
            GetEventRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithEventName(this.EventName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithIsInSchedule(this.InSchedule);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetEventAsync(request)
            );
            return result?.RepeatSchedule;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Model.RepeatSchedule> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Model.RepeatSchedule> self)
            {
                var (value, find) = (null as Gs2.Gs2Schedule.Model.RepeatSchedule).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.EventName,
                    this.InSchedule ?? true
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Schedule.Model.RepeatSchedule).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.EventName,
                    this.InSchedule ?? true,
                    () => this.GetFuture(
                        new GetEventRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Model.RepeatSchedule>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Schedule.Model.RepeatSchedule> ModelAsync()
            #else
        public async Task<Gs2.Gs2Schedule.Model.RepeatSchedule> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Schedule.Model.RepeatSchedule).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.EventName,
                this.InSchedule ?? true
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Schedule.Model.RepeatSchedule).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.EventName,
                this.InSchedule ?? true,
                () => this.GetAsync(
                    new GetEventRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Schedule.Model.RepeatSchedule> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Schedule.Model.RepeatSchedule> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Schedule.Model.RepeatSchedule> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Schedule.Model.RepeatSchedule).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.EventName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Schedule.Model.RepeatSchedule> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Schedule.Model.RepeatSchedule).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InSchedule ?? true
                ),
                (null as Gs2.Gs2Schedule.Model.RepeatSchedule).CacheKey(
                    this.EventName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Schedule.Model.RepeatSchedule>(
                (null as Gs2.Gs2Schedule.Model.RepeatSchedule).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InSchedule ?? true
                ),
                (null as Gs2.Gs2Schedule.Model.RepeatSchedule).CacheKey(
                    this.EventName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Schedule.Model.RepeatSchedule> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Schedule.Model.RepeatSchedule> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Schedule.Model.RepeatSchedule> callback)
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

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

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ScheduleRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string NextPageToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ScheduleRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Schedule.Model.Trigger> Triggers(
        )
        {
            return new DescribeTriggersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Schedule.Model.Trigger> TriggersAsync(
            #else
        public DescribeTriggersIterator TriggersAsync(
            #endif
        )
        {
            return new DescribeTriggersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeTriggers(
            Action<Gs2.Gs2Schedule.Model.Trigger[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Schedule.Model.Trigger>(
                (null as Gs2.Gs2Schedule.Model.Trigger).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await TriggersAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeTriggersWithInitialCallAsync(
            Action<Gs2.Gs2Schedule.Model.Trigger[]> callback
        )
        {
            var items = await TriggersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeTriggers(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeTriggers(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Schedule.Model.Trigger>(
                (null as Gs2.Gs2Schedule.Model.Trigger).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateTriggers(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Schedule.Model.Trigger>(
                (null as Gs2.Gs2Schedule.Model.Trigger).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Schedule.Domain.Model.TriggerAccessTokenDomain Trigger(
            string triggerName
        ) {
            return new Gs2.Gs2Schedule.Domain.Model.TriggerAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                triggerName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Schedule.Model.Event> Events(
            bool isInSchedule = true
        )
        {
            return new DescribeEventsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Schedule.Model.Event> EventsAsync(
            #else
        public DescribeEventsIterator EventsAsync(
            #endif
            bool isInSchedule = true
        )
        {
            return new DescribeEventsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeEvents(
            Action<Gs2.Gs2Schedule.Model.Event[]> callback,
            bool isInSchedule = true
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Schedule.Model.Event>(
                (null as Gs2.Gs2Schedule.Model.Event).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    isInSchedule,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await EventsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeEventsWithInitialCallAsync(
            Action<Gs2.Gs2Schedule.Model.Event[]> callback,
            bool isInSchedule = true
        )
        {
            var items = await EventsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeEvents(
                callback,
                isInSchedule
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeEvents(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Schedule.Model.Event>(
                (null as Gs2.Gs2Schedule.Model.Event).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    true,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Schedule.Model.Event>(
                (null as Gs2.Gs2Schedule.Model.Event).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    false,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateEvents(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Schedule.Model.Event>(
                (null as Gs2.Gs2Schedule.Model.Event).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    true,
                    this.AccessToken?.TimeOffset
                )
            );
            this._gs2.Cache.ClearListCache<Gs2.Gs2Schedule.Model.Event>(
                (null as Gs2.Gs2Schedule.Model.Event).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    false,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Schedule.Domain.Model.EventAccessTokenDomain Event(
            string eventName,
            bool isInSchedule = true
        ) {
            return new Gs2.Gs2Schedule.Domain.Model.EventAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                eventName,
                isInSchedule
            );
        }

    }
}

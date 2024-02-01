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
using Gs2.Gs2StateMachine.Domain.Iterator;
using Gs2.Gs2StateMachine.Model.Cache;
using Gs2.Gs2StateMachine.Request;
using Gs2.Gs2StateMachine.Result;
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

namespace Gs2.Gs2StateMachine.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2StateMachineRestClient _client;
        public string NamespaceName { get; }
        public string UserId { get; }
        public string NextPageToken { get; set; }

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2StateMachineRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2StateMachine.Model.Status> Statuses(
            string status = null
        )
        {
            return new DescribeStatusesByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                status
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2StateMachine.Model.Status> StatusesAsync(
            #else
        public DescribeStatusesByUserIdIterator StatusesAsync(
            #endif
            string status = null
        )
        {
            return new DescribeStatusesByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                status
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeStatuses(
            Action<Gs2.Gs2StateMachine.Model.Status[]> callback,
            string status = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2StateMachine.Model.Status>(
                (null as Gs2.Gs2StateMachine.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeStatusesWithInitialCallAsync(
            Action<Gs2.Gs2StateMachine.Model.Status[]> callback,
            string status = null
        )
        {
            var items = await StatusesAsync(
                status
            ).ToArrayAsync();
            var callbackId = SubscribeStatuses(
                callback,
                status
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeStatuses(
            ulong callbackId,
            string status = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2StateMachine.Model.Status>(
                (null as Gs2.Gs2StateMachine.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2StateMachine.Domain.Model.StatusDomain Status(
            string statusName
        ) {
            return new Gs2.Gs2StateMachine.Domain.Model.StatusDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                statusName
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2StateMachine.Domain.Model.StatusDomain> StartStateMachineFuture(
            StartStateMachineByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2StateMachine.Domain.Model.StatusDomain> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.StartStateMachineByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2StateMachine.Domain.Model.StatusDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2StateMachine.Domain.Model.StatusDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2StateMachine.Domain.Model.StatusDomain> StartStateMachineAsync(
            #else
        public async Task<Gs2.Gs2StateMachine.Domain.Model.StatusDomain> StartStateMachineAsync(
            #endif
            StartStateMachineByUserIdRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.StartStateMachineByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2StateMachine.Domain.Model.StatusDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

    }

    public partial class UserDomain {

    }
}

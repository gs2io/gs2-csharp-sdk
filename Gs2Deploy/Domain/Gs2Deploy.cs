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
#pragma warning disable CS0414 // Field is assigned but its value is never used

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Deploy.Domain.Iterator;
using Gs2.Gs2Deploy.Model.Cache;
using Gs2.Gs2Deploy.Domain.Model;
using Gs2.Gs2Deploy.Request;
using Gs2.Gs2Deploy.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Deploy.Model;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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

namespace Gs2.Gs2Deploy.Domain
{

    public class Gs2Deploy {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DeployRestClient _client;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2Deploy(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DeployRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Deploy> PreCreateStackFuture(
            PreCreateStackRequest request
        ) => PreCreateStackAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Deploy> PreCreateStackAsync(
        #else
        public async Task<Gs2Deploy> PreCreateStackAsync(
        #endif
            PreCreateStackRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.PreCreateStackAsync(request)
            );
            var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFuture(
            CreateStackRequest request
        ) => CreateStackAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackAsync(
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackAsync(
        #endif
            CreateStackRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateStackAsync(request)
            );
            var domain = new Gs2.Gs2Deploy.Domain.Model.StackDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFromGitHubFuture(
            CreateStackFromGitHubRequest request
        ) => CreateStackFromGitHubAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFromGitHubAsync(
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFromGitHubAsync(
        #endif
            CreateStackFromGitHubRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateStackFromGitHubAsync(request)
            );
            var domain = new Gs2.Gs2Deploy.Domain.Model.StackDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Deploy> PreValidateFuture(
            PreValidateRequest request
        ) => PreValidateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Deploy> PreValidateAsync(
        #else
        public async Task<Gs2Deploy> PreValidateAsync(
        #endif
            PreValidateRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.PreValidateAsync(request)
            );
            var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Deploy> ValidateFuture(
            ValidateRequest request
        ) => ValidateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Deploy> ValidateAsync(
        #else
        public async Task<Gs2Deploy> ValidateAsync(
        #endif
            ValidateRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.ValidateAsync(request)
            );
            var domain = this;
            return domain;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Stack> Stacks(
            string namePrefix = null
        )
        {
            return new DescribeStacksIterator(
                this._gs2,
                this._client,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Stack> StacksAsync(
        #else
        public DescribeStacksIterator StacksAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeStacksIterator(
                this._gs2,
                this._client,
                namePrefix
            );
        }

        public ulong SubscribeStacks(
            Action<Gs2.Gs2Deploy.Model.Stack[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Deploy.Model.Stack>(
                (null as Gs2.Gs2Deploy.Model.Stack).CacheParentKey(
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => namePrefix == null || item.Name.StartsWith(namePrefix))
                    .ToArray()),
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
                            callback.Invoke(await StacksAsync(
                                namePrefix
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

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeStacksWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeStacksWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Deploy.Model.Stack[]> callback,
            string namePrefix = null
        )
        {
            var items = await StacksAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeStacks(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeStacks(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Deploy.Model.Stack>(
                (null as Gs2.Gs2Deploy.Model.Stack).CacheParentKey(
                    null
                ),
                callbackId
            );
        }

        public void InvalidateStacks(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Deploy.Model.Stack>(
                (null as Gs2.Gs2Deploy.Model.Stack).CacheParentKey(
                    null
                )
            );
        }

        public Gs2.Gs2Deploy.Domain.Model.StackDomain Stack(
            string stackName
        ) {
            return new Gs2.Gs2Deploy.Domain.Model.StackDomain(
                this._gs2,
                stackName
            );
        }

        public void UpdateCacheFromStampSheet(
                string transactionId,
                int? timeOffset,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromStampTask(
                string taskId,
                int? timeOffset,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromJobResult(
                string method,
                int? timeOffset,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
        }

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
        }
    }
}

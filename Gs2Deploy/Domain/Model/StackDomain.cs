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
using Gs2.Gs2Deploy.Domain.Iterator;
using Gs2.Gs2Deploy.Model.Cache;
using Gs2.Gs2Deploy.Request;
using Gs2.Gs2Deploy.Result;
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

namespace Gs2.Gs2Deploy.Domain.Model
{

    public partial class StackDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DeployRestClient _client;
        public string StackName { get; } = null!;
        public string Status { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public StackDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string stackName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DeployRestClient(
                gs2.RestSession
            );
            this.StackName = stackName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Resource> Resources(
        )
        {
            return new DescribeResourcesIterator(
                this._gs2,
                this._client,
                this.StackName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Resource> ResourcesAsync(
            #else
        public DescribeResourcesIterator ResourcesAsync(
            #endif
        )
        {
            return new DescribeResourcesIterator(
                this._gs2,
                this._client,
                this.StackName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeResources(
            Action<Gs2.Gs2Deploy.Model.Resource[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Deploy.Model.Resource>(
                (null as Gs2.Gs2Deploy.Model.Resource).CacheParentKey(
                    this.StackName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeResourcesWithInitialCallAsync(
            Action<Gs2.Gs2Deploy.Model.Resource[]> callback
        )
        {
            var items = await ResourcesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeResources(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeResources(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Deploy.Model.Resource>(
                (null as Gs2.Gs2Deploy.Model.Resource).CacheParentKey(
                    this.StackName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Deploy.Domain.Model.ResourceDomain Resource(
            string resourceName
        ) {
            return new Gs2.Gs2Deploy.Domain.Model.ResourceDomain(
                this._gs2,
                this.StackName,
                resourceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Event> Events(
        )
        {
            return new DescribeEventsIterator(
                this._gs2,
                this._client,
                this.StackName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Event> EventsAsync(
            #else
        public DescribeEventsIterator EventsAsync(
            #endif
        )
        {
            return new DescribeEventsIterator(
                this._gs2,
                this._client,
                this.StackName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeEvents(
            Action<Gs2.Gs2Deploy.Model.Event[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Deploy.Model.Event>(
                (null as Gs2.Gs2Deploy.Model.Event).CacheParentKey(
                    this.StackName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeEventsWithInitialCallAsync(
            Action<Gs2.Gs2Deploy.Model.Event[]> callback
        )
        {
            var items = await EventsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeEvents(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeEvents(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Deploy.Model.Event>(
                (null as Gs2.Gs2Deploy.Model.Event).CacheParentKey(
                    this.StackName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Deploy.Domain.Model.EventDomain Event(
            string eventName
        ) {
            return new Gs2.Gs2Deploy.Domain.Model.EventDomain(
                this._gs2,
                this.StackName,
                eventName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Output> Outputs(
        )
        {
            return new DescribeOutputsIterator(
                this._gs2,
                this._client,
                this.StackName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Output> OutputsAsync(
            #else
        public DescribeOutputsIterator OutputsAsync(
            #endif
        )
        {
            return new DescribeOutputsIterator(
                this._gs2,
                this._client,
                this.StackName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeOutputs(
            Action<Gs2.Gs2Deploy.Model.Output[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Deploy.Model.Output>(
                (null as Gs2.Gs2Deploy.Model.Output).CacheParentKey(
                    this.StackName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeOutputsWithInitialCallAsync(
            Action<Gs2.Gs2Deploy.Model.Output[]> callback
        )
        {
            var items = await OutputsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeOutputs(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeOutputs(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Deploy.Model.Output>(
                (null as Gs2.Gs2Deploy.Model.Output).CacheParentKey(
                    this.StackName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Deploy.Domain.Model.OutputDomain Output(
            string outputName
        ) {
            return new Gs2.Gs2Deploy.Domain.Model.OutputDomain(
                this._gs2,
                this.StackName,
                outputName
            );
        }

    }

    public partial class StackDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatusFuture(
            GetStackStatusRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetStackStatusFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatusAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatusAsync(
            #endif
            GetStackStatusRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStackName(this.StackName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetStackStatusAsync(request)
            );
            var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Deploy.Model.Stack> GetFuture(
            GetStackRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Model.Stack> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetStackFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Model.Stack>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Deploy.Model.Stack> GetAsync(
            #else
        private async Task<Gs2.Gs2Deploy.Model.Stack> GetAsync(
            #endif
            GetStackRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStackName(this.StackName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetStackAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFuture(
            UpdateStackRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateStackFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateAsync(
            #endif
            UpdateStackRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStackName(this.StackName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateStackAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Model.ChangeSet[]> ChangeSetFuture(
            ChangeSetRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Model.ChangeSet[]> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.ChangeSetFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = result?.Items;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Model.ChangeSet[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Model.ChangeSet[]> ChangeSetAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Model.ChangeSet[]> ChangeSetAsync(
            #endif
            ChangeSetRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStackName(this.StackName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.ChangeSetAsync(request)
            );
            var domain = result?.Items;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHubFuture(
            UpdateStackFromGitHubRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateStackFromGitHubFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHubAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHubAsync(
            #endif
            UpdateStackFromGitHubRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStackName(this.StackName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateStackFromGitHubAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteFuture(
            DeleteStackRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteStackFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteAsync(
            #endif
            DeleteStackRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteStackAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDeleteFuture(
            ForceDeleteStackRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.ForceDeleteStackFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDeleteAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDeleteAsync(
            #endif
            ForceDeleteStackRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithStackName(this.StackName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.ForceDeleteStackAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResourcesFuture(
            DeleteStackResourcesRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteStackResourcesFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResourcesAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResourcesAsync(
            #endif
            DeleteStackResourcesRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteStackResourcesAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntityFuture(
            DeleteStackEntityRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteStackEntityFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntityAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntityAsync(
            #endif
            DeleteStackEntityRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithStackName(this.StackName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteStackEntityAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

    }

    public partial class StackDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Model.Stack> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Model.Stack> self)
            {
                var (value, find) = (null as Gs2.Gs2Deploy.Model.Stack).GetCache(
                    this._gs2.Cache,
                    this.StackName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Deploy.Model.Stack).FetchFuture(
                    this._gs2.Cache,
                    this.StackName,
                    () => this.GetFuture(
                        new GetStackRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Model.Stack>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Model.Stack> ModelAsync()
            #else
        public async Task<Gs2.Gs2Deploy.Model.Stack> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Deploy.Model.Stack).GetCache(
                this._gs2.Cache,
                this.StackName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Deploy.Model.Stack).FetchAsync(
                this._gs2.Cache,
                this.StackName,
                () => this.GetAsync(
                    new GetStackRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Deploy.Model.Stack> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Deploy.Model.Stack> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Deploy.Model.Stack> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Deploy.Model.Stack).DeleteCache(
                this._gs2.Cache,
                this.StackName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Deploy.Model.Stack> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Deploy.Model.Stack).CacheParentKey(
                ),
                (null as Gs2.Gs2Deploy.Model.Stack).CacheKey(
                    this.StackName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Deploy.Model.Stack>(
                (null as Gs2.Gs2Deploy.Model.Stack).CacheParentKey(
                ),
                (null as Gs2.Gs2Deploy.Model.Stack).CacheKey(
                    this.StackName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Deploy.Model.Stack> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Deploy.Model.Stack> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Deploy.Model.Stack> callback)
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

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
using Gs2.Gs2SerialKey.Domain.Iterator;
using Gs2.Gs2SerialKey.Model.Cache;
using Gs2.Gs2SerialKey.Request;
using Gs2.Gs2SerialKey.Result;
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

namespace Gs2.Gs2SerialKey.Domain.Model
{

    public partial class CampaignModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SerialKeyRestClient _client;
        public string NamespaceName { get; } = null!;
        public string CampaignModelName { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public CampaignModelDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string campaignModelName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SerialKeyRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.CampaignModelName = campaignModelName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2SerialKey.Model.IssueJob> IssueJobs(
        )
        {
            return new DescribeIssueJobsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.CampaignModelName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2SerialKey.Model.IssueJob> IssueJobsAsync(
            #else
        public DescribeIssueJobsIterator IssueJobsAsync(
            #endif
        )
        {
            return new DescribeIssueJobsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.CampaignModelName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeIssueJobs(
            Action<Gs2.Gs2SerialKey.Model.IssueJob[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2SerialKey.Model.IssueJob>(
                (null as Gs2.Gs2SerialKey.Model.IssueJob).CacheParentKey(
                    this.NamespaceName,
                    this.CampaignModelName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeIssueJobsWithInitialCallAsync(
            Action<Gs2.Gs2SerialKey.Model.IssueJob[]> callback
        )
        {
            var items = await IssueJobsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeIssueJobs(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeIssueJobs(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2SerialKey.Model.IssueJob>(
                (null as Gs2.Gs2SerialKey.Model.IssueJob).CacheParentKey(
                    this.NamespaceName,
                    this.CampaignModelName
                ),
                callbackId
            );
        }

        public Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain IssueJob(
            string issueJobName
        ) {
            return new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                this._gs2,
                this.NamespaceName,
                this.CampaignModelName,
                issueJobName
            );
        }

    }

    public partial class CampaignModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> GetFuture(
            GetCampaignModelRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithCampaignModelName(this.CampaignModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetCampaignModelFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.CampaignModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2SerialKey.Model.CampaignModel> GetAsync(
            #else
        private async Task<Gs2.Gs2SerialKey.Model.CampaignModel> GetAsync(
            #endif
            GetCampaignModelRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithCampaignModelName(this.CampaignModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetCampaignModelAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> IssueFuture(
            IssueRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithCampaignModelName(this.CampaignModelName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.IssueFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.CampaignModelName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> IssueAsync(
            #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> IssueAsync(
            #endif
            IssueRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithCampaignModelName(this.CampaignModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.IssueAsync(request)
            );
            var domain = new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                this._gs2,
                this.NamespaceName,
                this.CampaignModelName,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

    }

    public partial class CampaignModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> self)
            {
                var (value, find) = (null as Gs2.Gs2SerialKey.Model.CampaignModel).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.CampaignModelName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2SerialKey.Model.CampaignModel).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.CampaignModelName,
                    () => this.GetFuture(
                        new GetCampaignModelRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.CampaignModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SerialKey.Model.CampaignModel> ModelAsync()
            #else
        public async Task<Gs2.Gs2SerialKey.Model.CampaignModel> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2SerialKey.Model.CampaignModel).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.CampaignModelName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2SerialKey.Model.CampaignModel).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.CampaignModelName,
                () => this.GetAsync(
                    new GetCampaignModelRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2SerialKey.Model.CampaignModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2SerialKey.Model.CampaignModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2SerialKey.Model.CampaignModel).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.CampaignModelName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2SerialKey.Model.CampaignModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2SerialKey.Model.CampaignModel).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2SerialKey.Model.CampaignModel).CacheKey(
                    this.CampaignModelName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2SerialKey.Model.CampaignModel>(
                (null as Gs2.Gs2SerialKey.Model.CampaignModel).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2SerialKey.Model.CampaignModel).CacheKey(
                    this.CampaignModelName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2SerialKey.Model.CampaignModel> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SerialKey.Model.CampaignModel> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SerialKey.Model.CampaignModel> callback)
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

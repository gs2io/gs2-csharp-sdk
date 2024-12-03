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
using Gs2.Gs2JobQueue.Domain.Iterator;
using Gs2.Gs2JobQueue.Model.Cache;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
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

namespace Gs2.Gs2JobQueue.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2JobQueueRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public bool? AutoRun { get; set; } = null!;
        public bool? IsLastJob { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2JobQueueRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2JobQueue.Model.Job> Jobs(
            string timeOffsetToken = null
        )
        {
            return new DescribeJobsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2JobQueue.Model.Job> JobsAsync(
            #else
        public DescribeJobsByUserIdIterator JobsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeJobsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeJobs(
            Action<Gs2.Gs2JobQueue.Model.Job[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2JobQueue.Model.Job>(
                (null as Gs2.Gs2JobQueue.Model.Job).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeJobsWithInitialCallAsync(
            Action<Gs2.Gs2JobQueue.Model.Job[]> callback
        )
        {
            var items = await JobsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeJobs(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeJobs(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2JobQueue.Model.Job>(
                (null as Gs2.Gs2JobQueue.Model.Job).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2JobQueue.Domain.Model.JobDomain Job(
            string jobName
        ) {
            return new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                jobName
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain[]> PushFuture(
            PushByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain[]> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PushByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = result?.Items?.Select(v => new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                    this._gs2,
                    this.NamespaceName,
                    v?.UserId,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2JobQueue.Domain.Model.JobDomain>();
                if (result.AutoRun != null && !result.AutoRun.Value)
                {
                    this._gs2.JobQueueDomain.Push(
                        this.NamespaceName
                    );
                }
                this.AutoRun = result?.AutoRun;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2JobQueue.Domain.Model.JobDomain[]> PushAsync(
            #else
        public async Task<Gs2.Gs2JobQueue.Domain.Model.JobDomain[]> PushAsync(
            #endif
            PushByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PushByUserIdAsync(request)
            );
            var domain = result?.Items?.Select(v => new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                this._gs2,
                this.NamespaceName,
                v?.UserId,
                v?.Name
            )).ToArray() ?? Array.Empty<Gs2.Gs2JobQueue.Domain.Model.JobDomain>();
            if (result.AutoRun != null && !result.AutoRun.Value)
            {
                this._gs2.JobQueueDomain.Push(
                    this.NamespaceName
                );
            }
            this.AutoRun = result?.AutoRun;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain> RunFuture(
            RunByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.RunByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                if (result?.Item != null) {
                    this._gs2.UpdateCacheFromJobResult(
                        result?.Item,
                        result?.Result
                    );
                }
                var domain = new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Name
                );
                domain.Result = result?.Result;
                domain.IsLastJob = result?.IsLastJob;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2JobQueue.Domain.Model.JobDomain> RunAsync(
            #else
        public async Task<Gs2.Gs2JobQueue.Domain.Model.JobDomain> RunAsync(
            #endif
            RunByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.RunByUserIdAsync(request)
            );
            if (result?.Item != null) {
                this._gs2.UpdateCacheFromJobResult(
                    result?.Item,
                    result?.Result
                );
            }
            var domain = new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.Name
            );
            domain.Result = result?.Result;
            domain.IsLastJob = result?.IsLastJob;

            return domain;
        }
        #endif

    }

    public partial class UserDomain {

    }
}

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
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
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
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public bool? AutoRun { get; set; }
        public bool? IsLastJob { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2JobQueueRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2JobQueue.Model.Job> Jobs(
        )
        {
            return new DescribeJobsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2JobQueue.Model.Job> JobsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2JobQueue.Model.Job> Jobs(
            #endif
        #else
        public DescribeJobsByUserIdIterator JobsAsync(
        #endif
        )
        {
            return new DescribeJobsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeJobs(
            Action<Gs2.Gs2JobQueue.Model.Job[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2JobQueue.Model.Job>(
                Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Job"
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
                Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Job"
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
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2JobQueue.Model.DeadLetterJob> DeadLetterJobs(
        )
        {
            return new DescribeDeadLetterJobsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2JobQueue.Model.DeadLetterJob> DeadLetterJobsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2JobQueue.Model.DeadLetterJob> DeadLetterJobs(
            #endif
        #else
        public DescribeDeadLetterJobsByUserIdIterator DeadLetterJobsAsync(
        #endif
        )
        {
            return new DescribeDeadLetterJobsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeDeadLetterJobs(
            Action<Gs2.Gs2JobQueue.Model.DeadLetterJob[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "DeadLetterJob"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDeadLetterJobsWithInitialCallAsync(
            Action<Gs2.Gs2JobQueue.Model.DeadLetterJob[]> callback
        )
        {
            var items = await DeadLetterJobsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeDeadLetterJobs(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDeadLetterJobs(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "DeadLetterJob"
                ),
                callbackId
            );
        }

        public Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain DeadLetterJob(
            string deadLetterJobName
        ) {
            return new Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                deadLetterJobName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "jobQueue",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.PushByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Job"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                                item.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2JobQueue.Domain.Model.JobDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                        this._gs2,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        result.Items[i]?.Name
                    );
                    var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Job"
                    );
                    var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            PushByUserIdResult result = null;
                result = await this._client.PushByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Job"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                            item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2JobQueue.Domain.Model.JobDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                        this._gs2,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        result.Items[i]?.Name
                    );
                    var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Job"
                    );
                    var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
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
        [Obsolete("The name has been changed to PushFuture.")]
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain[]> Push(
            PushByUserIdRequest request
        ) {
            return PushFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain> RunFuture(
            RunByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.RunByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Job"
                        );
                        var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2JobQueue.Model.Job>(parentKey, key);
                    }
                }
                if (result?.Item != null) {
                    this._gs2.UpdateCacheFromJobResult(
                        result?.Item,
                        result?.Result
                    );
                }
                var domain = new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Name
                );
                domain.IsLastJob = result?.IsLastJob;
                domain.Result = result?.Result;

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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            RunByUserIdResult result = null;
            try {
                result = await this._client.RunByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Job"
                    );
                    var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2JobQueue.Model.Job>(parentKey, key);
                }
            }
            if (result?.Item != null) {
                this._gs2.UpdateCacheFromJobResult(
                    result?.Item,
                    result?.Result
                );
            }
                var domain = new Gs2.Gs2JobQueue.Domain.Model.JobDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.Name
                );
            domain.IsLastJob = result?.IsLastJob;
            domain.Result = result?.Result;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to RunFuture.")]
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.JobDomain> Run(
            RunByUserIdRequest request
        ) {
            return RunFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

    }
}

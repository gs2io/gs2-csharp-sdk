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
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2JobQueue.Domain.Model
{

    public partial class JobResultDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2JobQueueRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _jobName;
        private readonly int? _tryNumber;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string JobName => _jobName;
        public int? TryNumber => _tryNumber;

        public JobResultDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string jobName,
            int? tryNumber
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2JobQueueRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._jobName = jobName;
            this._tryNumber = tryNumber;
            this._parentKey = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.JobName,
                "JobResult"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string jobName,
            string tryNumber,
            string childType
        )
        {
            return string.Join(
                ":",
                "jobQueue",
                namespaceName ?? "null",
                userId ?? "null",
                jobName ?? "null",
                tryNumber ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string tryNumber
        )
        {
            return string.Join(
                ":",
                tryNumber ?? "null"
            );
        }

    }

    public partial class JobResultDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2JobQueue.Model.JobResult> GetFuture(
            GetJobResultByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Model.JobResult> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithJobName(this.JobName);
                var future = this._client.GetJobResultByUserIdFuture(
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithJobName(this.JobName);
                GetJobResultByUserIdResult result = null;
                try {
                    result = await this._client.GetJobResultByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.JobName,
                            "JobResult"
                        );
                        var key = Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                            resultModel.Item.TryNumber.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Model.JobResult>(Impl);
        }
        #else
        private async Task<Gs2.Gs2JobQueue.Model.JobResult> GetAsync(
            GetJobResultByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithJobName(this.JobName);
            var future = this._client.GetJobResultByUserIdFuture(
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithJobName(this.JobName);
            GetJobResultByUserIdResult result = null;
            try {
                result = await this._client.GetJobResultByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.JobName,
                        "JobResult"
                    );
                    var key = Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                        resultModel.Item.TryNumber.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

    }

    public partial class JobResultDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Model.JobResult> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Model.JobResult> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2JobQueue.Model.JobResult>(
                    _parentKey,
                    Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                        this.TryNumber?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetJobResultByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                                    this.TryNumber?.ToString()
                                );
                            _cache.Put<Gs2.Gs2JobQueue.Model.JobResult>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "jobResult")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _cache.Get<Gs2.Gs2JobQueue.Model.JobResult>(
                        _parentKey,
                        Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                            this.TryNumber?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Model.JobResult>(Impl);
        }
        #else
        public async Task<Gs2.Gs2JobQueue.Model.JobResult> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2JobQueue.Model.JobResult>(
                    _parentKey,
                    Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                        this.TryNumber?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetJobResultByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                                    this.TryNumber?.ToString()
                                );
                    _cache.Put<Gs2.Gs2JobQueue.Model.JobResult>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "jobResult")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2JobQueue.Model.JobResult>(
                        _parentKey,
                        Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                            this.TryNumber?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2JobQueue.Model.JobResult> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2JobQueue.Model.JobResult> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2JobQueue.Model.JobResult> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2JobQueue.Model.JobResult> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2JobQueue.Model.JobResult> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                    this.TryNumber.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2JobQueue.Model.JobResult>(
                _parentKey,
                Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                    this.TryNumber.ToString()
                ),
                callbackId
            );
        }

    }
}

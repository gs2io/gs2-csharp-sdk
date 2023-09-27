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
using Gs2.Gs2SerialKey.Domain.Iterator;
using Gs2.Gs2SerialKey.Request;
using Gs2.Gs2SerialKey.Result;
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

namespace Gs2.Gs2SerialKey.Domain.Model
{

    public partial class CampaignModelDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2SerialKeyRestClient _client;
        private readonly string _namespaceName;
        private readonly string _campaignModelName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string CampaignModelName => _campaignModelName;

        public CampaignModelDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string campaignModelName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2SerialKeyRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._campaignModelName = campaignModelName;
            this._parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "CampaignModel"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2SerialKey.Model.IssueJob> IssueJobs(
        )
        {
            return new DescribeIssueJobsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.CampaignModelName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2SerialKey.Model.IssueJob> IssueJobsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2SerialKey.Model.IssueJob> IssueJobs(
            #endif
        #else
        public DescribeIssueJobsIterator IssueJobs(
        #endif
        )
        {
            return new DescribeIssueJobsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.CampaignModelName
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

        public ulong SubscribeIssueJobs(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2SerialKey.Model.IssueJob>(
                Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.CampaignModelName,
                    "IssueJob"
                ),
                callback
            );
        }

        public void UnsubscribeIssueJobs(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2SerialKey.Model.IssueJob>(
                Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.CampaignModelName,
                    "IssueJob"
                ),
                callbackId
            );
        }

        public Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain IssueJob(
            string issueJobName
        ) {
            return new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.CampaignModelName,
                issueJobName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string campaignModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "serialKey",
                namespaceName ?? "null",
                campaignModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string campaignModelName
        )
        {
            return string.Join(
                ":",
                campaignModelName ?? "null"
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
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithCampaignModelName(this.CampaignModelName);
                var future = this._client.GetCampaignModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            request.CampaignModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "campaignModel")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
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
                    .WithCampaignModelName(this.CampaignModelName);
                GetCampaignModelResult result = null;
                try {
                    result = await this._client.GetCampaignModelAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        request.CampaignModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "campaignModel")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CampaignModel"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.CampaignModel>(Impl);
        }
        #else
        private async Task<Gs2.Gs2SerialKey.Model.CampaignModel> GetAsync(
            GetCampaignModelRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCampaignModelName(this.CampaignModelName);
            var future = this._client.GetCampaignModelFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        request.CampaignModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "campaignModel")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
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
                .WithCampaignModelName(this.CampaignModelName);
            GetCampaignModelResult result = null;
            try {
                result = await this._client.GetCampaignModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                    request.CampaignModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "campaignModel")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CampaignModel"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> IssueFuture(
            IssueRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithCampaignModelName(this.CampaignModelName);
                var future = this._client.IssueFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithCampaignModelName(this.CampaignModelName);
                IssueResult result = null;
                    result = await this._client.IssueAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.CampaignModelName,
                            "IssueJob"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.CampaignModelName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> IssueAsync(
            IssueRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCampaignModelName(this.CampaignModelName);
            var future = this._client.IssueFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCampaignModelName(this.CampaignModelName);
            IssueResult result = null;
                result = await this._client.IssueAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.CampaignModelName,
                        "IssueJob"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.CampaignModelName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> IssueAsync(
            IssueRequest request
        ) {
            var future = IssueFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to IssueFuture.")]
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> Issue(
            IssueRequest request
        ) {
            return IssueFuture(request);
        }
        #endif

    }

    public partial class CampaignModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
                    _parentKey,
                    Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        this.CampaignModelName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetCampaignModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                                    this.CampaignModelName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "campaignModel")
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
                    (value, _) = _cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
                        _parentKey,
                        Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            this.CampaignModelName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.CampaignModel>(Impl);
        }
        #else
        public async Task<Gs2.Gs2SerialKey.Model.CampaignModel> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
                    _parentKey,
                    Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        this.CampaignModelName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetCampaignModelRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                                    this.CampaignModelName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "campaignModel")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
                        _parentKey,
                        Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            this.CampaignModelName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SerialKey.Model.CampaignModel> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

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


        public ulong Subscribe(Action<Gs2.Gs2SerialKey.Model.CampaignModel> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                    this.CampaignModelName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2SerialKey.Model.CampaignModel>(
                _parentKey,
                Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                    this.CampaignModelName.ToString()
                ),
                callbackId
            );
        }

    }
}

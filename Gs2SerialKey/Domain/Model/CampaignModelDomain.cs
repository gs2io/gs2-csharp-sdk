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
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "CampaignModel"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2SerialKey.Model.CampaignModel> GetAsync(
            #else
        private IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> Get(
            #endif
        #else
        private async Task<Gs2.Gs2SerialKey.Model.CampaignModel> GetAsync(
        #endif
            GetCampaignModelRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithCampaignModelName(this._campaignModelName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetCampaignModelFuture(
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
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #else
            var result = await this._client.GetCampaignModelAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
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
            #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.CampaignModel>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> IssueAsync(
            #else
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> Issue(
            #endif
        #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> IssueAsync(
        #endif
            IssueRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithCampaignModelName(this._campaignModelName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _campaignModelName.ToString(),
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
            #else
            var result = await this._client.IssueAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _campaignModelName.ToString(),
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
            #endif
            Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain domain = new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                request.CampaignModelName,
                result?.Item?.Name
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2SerialKey.Model.IssueJob> IssueJobs(
        )
        {
            return new DescribeIssueJobsIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._campaignModelName
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
                this._namespaceName,
                this._campaignModelName
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

        public Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain IssueJob(
            string issueJobName
        ) {
            return new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._campaignModelName,
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SerialKey.Model.CampaignModel> Model() {
            #else
        public IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2SerialKey.Model.CampaignModel> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Model.CampaignModel> self)
            {
        #endif
            Gs2.Gs2SerialKey.Model.CampaignModel value = _cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
                _parentKey,
                Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                    this.CampaignModelName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetCampaignModelRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "campaignModel")
                            {
                                _cache.Delete<Gs2.Gs2SerialKey.Model.CampaignModel>(
                                    _parentKey,
                                    Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                                        this.CampaignModelName?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "campaignModel")
                    {
                        _cache.Delete<Gs2.Gs2SerialKey.Model.CampaignModel>(
                            _parentKey,
                            Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                                this.CampaignModelName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
                    _parentKey,
                    Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        this.CampaignModelName?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.CampaignModel>(Impl);
        #endif
        }

    }
}
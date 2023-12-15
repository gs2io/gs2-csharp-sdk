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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SerialKeyRestClient _client;
        private readonly string _namespaceName;
        private readonly string _campaignModelName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string CampaignModelName => _campaignModelName;

        public CampaignModelDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string campaignModelName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SerialKeyRestClient(
                gs2.RestSession
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
                this._gs2.Cache,
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
        public DescribeIssueJobsIterator IssueJobsAsync(
        #endif
        )
        {
            return new DescribeIssueJobsIterator(
                this._gs2.Cache,
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
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2SerialKey.Model.IssueJob>(
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2SerialKey.Model.IssueJob>(
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
                this._gs2,
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
                        this._gs2.Cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "campaignModel")
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CampaignModel"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2SerialKey.Model.CampaignModel> GetAsync(
            #else
        private async Task<Gs2.Gs2SerialKey.Model.CampaignModel> GetAsync(
            #endif
            GetCampaignModelRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "campaignModel")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CampaignModel"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                    this._gs2,
                    request.NamespaceName,
                    request.CampaignModelName,
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCampaignModelName(this.CampaignModelName);
            IssueResult result = null;
                result = await this._client.IssueAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2SerialKey.Domain.Model.IssueJobDomain(
                    this._gs2,
                    request.NamespaceName,
                    request.CampaignModelName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
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
                            this._gs2.Cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "campaignModel")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SerialKey.Model.CampaignModel> ModelAsync()
            #else
        public async Task<Gs2.Gs2SerialKey.Model.CampaignModel> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2SerialKey.Model.CampaignModel>(
                _parentKey,
                Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                    this.CampaignModelName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
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
                        this._gs2.Cache.Put<Gs2.Gs2SerialKey.Model.CampaignModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "campaignModel")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2SerialKey.Model.CampaignModel>(
                        _parentKey,
                        Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            this.CampaignModelName?.ToString()
                        )
                    );
                }
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
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


        public ulong Subscribe(Action<Gs2.Gs2SerialKey.Model.CampaignModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                    this.CampaignModelName.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2SerialKey.Model.CampaignModel>(
                _parentKey,
                Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                    this.CampaignModelName.ToString()
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

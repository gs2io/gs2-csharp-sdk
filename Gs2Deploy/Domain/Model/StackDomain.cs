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
using Gs2.Gs2Deploy.Domain.Iterator;
using Gs2.Gs2Deploy.Request;
using Gs2.Gs2Deploy.Result;
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

namespace Gs2.Gs2Deploy.Domain.Model
{

    public partial class StackDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2DeployRestClient _client;
        private readonly string _stackName;

        private readonly String _parentKey;
        public string Status { get; set; }
        public string NextPageToken { get; set; }
        public string StackName => _stackName;

        public StackDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string stackName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2DeployRestClient(
                session
            );
            this._stackName = stackName;
            this._parentKey = "deploy:Stack";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatusAsync(
            #else
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatus(
            #endif
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatusAsync(
        #endif
            GetStackStatusRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
        #endif
            request
                .WithStackName(this.StackName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetStackStatusFuture(
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
            var result = await this._client.GetStackStatusAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
            Gs2.Gs2Deploy.Domain.Model.StackDomain domain = this;
            this.Status = domain.Status = result?.Status;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Deploy.Model.Stack> GetAsync(
            #else
        private IFuture<Gs2.Gs2Deploy.Model.Stack> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Deploy.Model.Stack> GetAsync(
        #endif
            GetStackRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Model.Stack> self)
            {
        #endif
            request
                .WithStackName(this.StackName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetStackFuture(
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
            var result = await this._client.GetStackAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "deploy",
                        "Stack"
                    );
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Model.Stack>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateAsync(
        #endif
            UpdateStackRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
        #endif
            request
                .WithStackName(this.StackName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateStackFuture(
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
            var result = await this._client.UpdateStackAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "deploy",
                        "Stack"
                    );
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
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
            Gs2.Gs2Deploy.Domain.Model.StackDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHubAsync(
            #else
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHub(
            #endif
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHubAsync(
        #endif
            UpdateStackFromGitHubRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
        #endif
            request
                .WithStackName(this.StackName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateStackFromGitHubFuture(
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
            var result = await this._client.UpdateStackFromGitHubAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "deploy",
                        "Stack"
                    );
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
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
            Gs2.Gs2Deploy.Domain.Model.StackDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteAsync(
        #endif
            DeleteStackRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
        #endif
            request
                .WithStackName(this.StackName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteStackFuture(
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
            DeleteStackResult result = null;
            try {
                result = await this._client.DeleteStackAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "stack")
                {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "deploy",
                        "Stack"
                    );
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Deploy.Model.Stack>(parentKey, key);
                }
            }
            Gs2.Gs2Deploy.Domain.Model.StackDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDeleteAsync(
            #else
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDelete(
            #endif
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDeleteAsync(
        #endif
            ForceDeleteStackRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
        #endif
            request
                .WithStackName(this.StackName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ForceDeleteStackFuture(
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
            var result = await this._client.ForceDeleteStackAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "deploy",
                        "Stack"
                    );
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
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
            Gs2.Gs2Deploy.Domain.Model.StackDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResourcesAsync(
            #else
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResources(
            #endif
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResourcesAsync(
        #endif
            DeleteStackResourcesRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
        #endif
            request
                .WithStackName(this.StackName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteStackResourcesFuture(
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
            DeleteStackResourcesResult result = null;
            try {
                result = await this._client.DeleteStackResourcesAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "stack")
                {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "deploy",
                        "Stack"
                    );
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Deploy.Model.Stack>(parentKey, key);
                }
            }
            Gs2.Gs2Deploy.Domain.Model.StackDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntityAsync(
            #else
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntity(
            #endif
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntityAsync(
        #endif
            DeleteStackEntityRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
        #endif
            request
                .WithStackName(this.StackName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteStackEntityFuture(
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
            DeleteStackEntityResult result = null;
            try {
                result = await this._client.DeleteStackEntityAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "stack")
                {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "deploy",
                        "Stack"
                    );
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Deploy.Model.Stack>(parentKey, key);
                }
            }
            Gs2.Gs2Deploy.Domain.Model.StackDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Resource> Resources(
        )
        {
            return new DescribeResourcesIterator(
                this._cache,
                this._client,
                this.StackName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Resource> ResourcesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Resource> Resources(
            #endif
        #else
        public DescribeResourcesIterator Resources(
        #endif
        )
        {
            return new DescribeResourcesIterator(
                this._cache,
                this._client,
                this.StackName
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

        public Gs2.Gs2Deploy.Domain.Model.ResourceDomain Resource(
            string resourceName
        ) {
            return new Gs2.Gs2Deploy.Domain.Model.ResourceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.StackName,
                resourceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Event> Events(
        )
        {
            return new DescribeEventsIterator(
                this._cache,
                this._client,
                this.StackName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Event> EventsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Event> Events(
            #endif
        #else
        public DescribeEventsIterator Events(
        #endif
        )
        {
            return new DescribeEventsIterator(
                this._cache,
                this._client,
                this.StackName
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

        public Gs2.Gs2Deploy.Domain.Model.EventDomain Event(
            string eventName
        ) {
            return new Gs2.Gs2Deploy.Domain.Model.EventDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.StackName,
                eventName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Output> Outputs(
        )
        {
            return new DescribeOutputsIterator(
                this._cache,
                this._client,
                this.StackName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Output> OutputsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Output> Outputs(
            #endif
        #else
        public DescribeOutputsIterator Outputs(
        #endif
        )
        {
            return new DescribeOutputsIterator(
                this._cache,
                this._client,
                this.StackName
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

        public Gs2.Gs2Deploy.Domain.Model.OutputDomain Output(
            string outputName
        ) {
            return new Gs2.Gs2Deploy.Domain.Model.OutputDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.StackName,
                outputName
            );
        }

        public static string CreateCacheParentKey(
            string stackName,
            string childType
        )
        {
            return string.Join(
                ":",
                "deploy",
                stackName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string stackName
        )
        {
            return string.Join(
                ":",
                stackName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Model.Stack> Model() {
            #else
        public IFuture<Gs2.Gs2Deploy.Model.Stack> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Deploy.Model.Stack> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Model.Stack> self)
            {
        #endif
            var parentKey = string.Join(
                ":",
                "deploy",
                "Stack"
            );
            var (value, find) = _cache.Get<Gs2.Gs2Deploy.Model.Stack>(
                parentKey,
                Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                    this.StackName?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetStackRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                                    this.StackName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "stack")
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
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                            this.StackName?.ToString()
                        );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "stack")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Deploy.Model.Stack>(
                    parentKey,
                    Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        this.StackName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Model.Stack>(Impl);
        #endif
        }

    }
}

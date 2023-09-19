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

    }

    public partial class StackDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatusFuture(
            GetStackStatusRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithStackName(this.StackName);
                var future = this._client.GetStackStatusFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                            request.StackName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "stack")
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
                    .WithStackName(this.StackName);
                GetStackStatusResult result = null;
                try {
                    result = await this._client.GetStackStatusAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "stack")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatusAsync(
            GetStackStatusRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithStackName(this.StackName);
            var future = this._client.GetStackStatusFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "stack")
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
                .WithStackName(this.StackName);
            GetStackStatusResult result = null;
            try {
                result = await this._client.GetStackStatusAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                    request.StackName.ToString()
                    );
                _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "stack")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatusAsync(
            GetStackStatusRequest request
        ) {
            var future = GetStatusFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to GetStatusFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> GetStatus(
            GetStackStatusRequest request
        ) {
            return GetStatusFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Deploy.Model.Stack> GetFuture(
            GetStackRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Model.Stack> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithStackName(this.StackName);
                var future = this._client.GetStackFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                            request.StackName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "stack")
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
                    .WithStackName(this.StackName);
                GetStackResult result = null;
                try {
                    result = await this._client.GetStackAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "stack")
                    {
                        throw;
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
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Model.Stack>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Deploy.Model.Stack> GetAsync(
            GetStackRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithStackName(this.StackName);
            var future = this._client.GetStackFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "stack")
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
                .WithStackName(this.StackName);
            GetStackResult result = null;
            try {
                result = await this._client.GetStackAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                    request.StackName.ToString()
                    );
                _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "stack")
                {
                    throw;
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
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFuture(
            UpdateStackRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithStackName(this.StackName);
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
                request
                    .WithStackName(this.StackName);
                UpdateStackResult result = null;
                    result = await this._client.UpdateStackAsync(
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateAsync(
            UpdateStackRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithStackName(this.StackName);
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
            request
                .WithStackName(this.StackName);
            UpdateStackResult result = null;
                result = await this._client.UpdateStackAsync(
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateAsync(
            UpdateStackRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> Update(
            UpdateStackRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Model.ChangeSet[]> ChangeSetFuture(
            ChangeSetRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Model.ChangeSet[]> self)
            {
                request
                    .WithStackName(this.StackName);
                var future = this._client.ChangeSetFuture(
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
                if (resultModel != null) {
                    
                }
                var domain = result?.Items;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Model.ChangeSet[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Model.ChangeSet[]> ChangeSetAsync(
            ChangeSetRequest request
        ) {
            request
                .WithStackName(this.StackName);
            ChangeSetResult result = null;
                result = await this._client.ChangeSetAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
            var domain = result?.Items;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Model.ChangeSet[]> ChangeSetAsync(
            ChangeSetRequest request
        ) {
            var future = ChangeSetFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ChangeSetFuture.")]
        public IFuture<Gs2.Gs2Deploy.Model.ChangeSet[]> ChangeSet(
            ChangeSetRequest request
        ) {
            return ChangeSetFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHubFuture(
            UpdateStackFromGitHubRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithStackName(this.StackName);
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
                request
                    .WithStackName(this.StackName);
                UpdateStackFromGitHubResult result = null;
                    result = await this._client.UpdateStackFromGitHubAsync(
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHubAsync(
            UpdateStackFromGitHubRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithStackName(this.StackName);
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
            request
                .WithStackName(this.StackName);
            UpdateStackFromGitHubResult result = null;
                result = await this._client.UpdateStackFromGitHubAsync(
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHubAsync(
            UpdateStackFromGitHubRequest request
        ) {
            var future = UpdateFromGitHubFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFromGitHubFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> UpdateFromGitHub(
            UpdateStackFromGitHubRequest request
        ) {
            return UpdateFromGitHubFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteFuture(
            DeleteStackRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithStackName(this.StackName);
                var future = this._client.DeleteStackFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                            request.StackName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "stack")
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
                    .WithStackName(this.StackName);
                DeleteStackResult result = null;
                try {
                    result = await this._client.DeleteStackAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "stack")
                    {
                        throw;
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteAsync(
            DeleteStackRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithStackName(this.StackName);
            var future = this._client.DeleteStackFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "stack")
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
                .WithStackName(this.StackName);
            DeleteStackResult result = null;
            try {
                result = await this._client.DeleteStackAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                    request.StackName.ToString()
                    );
                _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "stack")
                {
                    throw;
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteAsync(
            DeleteStackRequest request
        ) {
            var future = DeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> Delete(
            DeleteStackRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDeleteFuture(
            ForceDeleteStackRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithStackName(this.StackName);
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
                request
                    .WithStackName(this.StackName);
                ForceDeleteStackResult result = null;
                    result = await this._client.ForceDeleteStackAsync(
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDeleteAsync(
            ForceDeleteStackRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithStackName(this.StackName);
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
            request
                .WithStackName(this.StackName);
            ForceDeleteStackResult result = null;
                result = await this._client.ForceDeleteStackAsync(
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDeleteAsync(
            ForceDeleteStackRequest request
        ) {
            var future = ForceDeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ForceDeleteFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> ForceDelete(
            ForceDeleteStackRequest request
        ) {
            return ForceDeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResourcesFuture(
            DeleteStackResourcesRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithStackName(this.StackName);
                var future = this._client.DeleteStackResourcesFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                            request.StackName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "stack")
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
                    .WithStackName(this.StackName);
                DeleteStackResourcesResult result = null;
                try {
                    result = await this._client.DeleteStackResourcesAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "stack")
                    {
                        throw;
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResourcesAsync(
            DeleteStackResourcesRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithStackName(this.StackName);
            var future = this._client.DeleteStackResourcesFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "stack")
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
                .WithStackName(this.StackName);
            DeleteStackResourcesResult result = null;
            try {
                result = await this._client.DeleteStackResourcesAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                    request.StackName.ToString()
                    );
                _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "stack")
                {
                    throw;
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResourcesAsync(
            DeleteStackResourcesRequest request
        ) {
            var future = DeleteResourcesFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteResourcesFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteResources(
            DeleteStackResourcesRequest request
        ) {
            return DeleteResourcesFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntityFuture(
            DeleteStackEntityRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithStackName(this.StackName);
                var future = this._client.DeleteStackEntityFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                            request.StackName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "stack")
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
                    .WithStackName(this.StackName);
                DeleteStackEntityResult result = null;
                try {
                    result = await this._client.DeleteStackEntityAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "stack")
                    {
                        throw;
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntityAsync(
            DeleteStackEntityRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithStackName(this.StackName);
            var future = this._client.DeleteStackEntityFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                        request.StackName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "stack")
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
                .WithStackName(this.StackName);
            DeleteStackEntityResult result = null;
            try {
                result = await this._client.DeleteStackEntityAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                    request.StackName.ToString()
                    );
                _cache.Put<Gs2.Gs2Deploy.Model.Stack>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "stack")
                {
                    throw;
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntityAsync(
            DeleteStackEntityRequest request
        ) {
            var future = DeleteEntityFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteEntityFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> DeleteEntity(
            DeleteStackEntityRequest request
        ) {
            return DeleteEntityFuture(request);
        }
        #endif

    }

    public partial class StackDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Model.Stack> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Model.Stack> self)
            {
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
                    var future = this.GetFuture(
                        new GetStackRequest()
                    );
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
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _cache.Get<Gs2.Gs2Deploy.Model.Stack>(
                        parentKey,
                        Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                            this.StackName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Model.Stack>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Deploy.Model.Stack> ModelAsync()
        {
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
                try {
                    await this.GetAsync(
                        new GetStackRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
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
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Deploy.Model.Stack>(
                        parentKey,
                        Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
                            this.StackName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Deploy.Model.Stack> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

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

    }
}

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
#pragma warning disable CS0414 // Field is assigned but its value is never used

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Deploy.Domain.Iterator;
using Gs2.Gs2Deploy.Domain.Model;
using Gs2.Gs2Deploy.Request;
using Gs2.Gs2Deploy.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Deploy.Model;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Deploy.Domain
{

    public class Gs2Deploy {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DeployRestClient _client;

        private readonly String _parentKey;

        public Gs2Deploy(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DeployRestClient(
                gs2.RestSession
            );
            this._parentKey = "deploy";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFuture(
            CreateStackRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                var future = this._client.CreateStackFuture(
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
                        var parentKey = string.Join(
                            ":",
                            "deploy",
                            "Stack"
                        );
                        var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Deploy.Domain.Model.StackDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackAsync(
            #endif
            CreateStackRequest request
        ) {
            CreateStackResult result = null;
                result = await this._client.CreateStackAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Deploy.Domain.Model.StackDomain(
                    this._gs2,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateStackFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStack(
            CreateStackRequest request
        ) {
            return CreateStackFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFromGitHubFuture(
            CreateStackFromGitHubRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> self)
            {
                var future = this._client.CreateStackFromGitHubFuture(
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
                        var parentKey = string.Join(
                            ":",
                            "deploy",
                            "Stack"
                        );
                        var key = Gs2.Gs2Deploy.Domain.Model.StackDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Deploy.Domain.Model.StackDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFromGitHubAsync(
            #else
        public async Task<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFromGitHubAsync(
            #endif
            CreateStackFromGitHubRequest request
        ) {
            CreateStackFromGitHubResult result = null;
                result = await this._client.CreateStackFromGitHubAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Deploy.Domain.Model.StackDomain(
                    this._gs2,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateStackFromGitHubFuture.")]
        public IFuture<Gs2.Gs2Deploy.Domain.Model.StackDomain> CreateStackFromGitHub(
            CreateStackFromGitHubRequest request
        ) {
            return CreateStackFromGitHubFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Deploy> ValidateFuture(
            ValidateRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Deploy> self)
            {
                var future = this._client.ValidateFuture(
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
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Deploy>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Deploy> ValidateAsync(
            #else
        public async Task<Gs2Deploy> ValidateAsync(
            #endif
            ValidateRequest request
        ) {
            ValidateResult result = null;
                result = await this._client.ValidateAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to ValidateFuture.")]
        public IFuture<Gs2Deploy> Validate(
            ValidateRequest request
        ) {
            return ValidateFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Stack> Stacks(
        )
        {
            return new DescribeStacksIterator(
                this._gs2.Cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Stack> StacksAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Deploy.Model.Stack> Stacks(
            #endif
        #else
        public DescribeStacksIterator StacksAsync(
        #endif
        )
        {
            return new DescribeStacksIterator(
                this._gs2.Cache,
                this._client
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

        public ulong SubscribeStacks(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Deploy.Model.Stack>(
                "deploy:Stack",
                callback
            );
        }

        public void UnsubscribeStacks(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Deploy.Model.Stack>(
                "deploy:Stack",
                callbackId
            );
        }

        public Gs2.Gs2Deploy.Domain.Model.StackDomain Stack(
            string stackName
        ) {
            return new Gs2.Gs2Deploy.Domain.Model.StackDomain(
                this._gs2,
                stackName
            );
        }

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromJobResult(
                string method,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
        }

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
    #if UNITY_2017_1_OR_NEWER
    #endif
        }
    }
}

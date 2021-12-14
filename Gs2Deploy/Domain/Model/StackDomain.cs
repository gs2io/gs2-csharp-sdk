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

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Deploy.Domain.Cache;
using Gs2.Gs2Deploy.Domain.Iterator;
using Gs2.Gs2Deploy.Request;
using Gs2.Gs2Deploy.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
    #else
using System.Collections;
using Gs2.Core;
using Gs2.Core.Domain;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Deploy.Domain.Model
{

    public class StackDomain {
        Gs2RestSession session;
        Gs2DeployRestClient client;
        StackDomainCache stackCache;
        string stackName;
        ResourceDomainCache resourceCache;
        EventDomainCache eventCache;
        OutputDomainCache outputCache;

        public StackDomain(
            Gs2RestSession session,
            StackDomainCache stackCache,
            string stackName
        ) {
            this.session = session;
            this.client = new Gs2DeployRestClient(
                session
            );
            this.stackCache = stackCache;
            this.stackName = stackName;
            this.resourceCache = new ResourceDomainCache();
            this.eventCache = new EventDomainCache();
            this.outputCache = new OutputDomainCache();
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<GetStackStatusResult> GetStatus(
            #else

        public class GetStatusFuture : Gs2Future<GetStackStatusResult>
        {
            private StackDomain _domain;
            private GetStackStatusRequest _request;

            public GetStatusFuture(
                StackDomain domain,
                GetStackStatusRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.GetStackStatus(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            OnComplete(r.Result);
                        }
                        else
                        {
                            OnError(r.Error);
                        }
                    }
                );
            }
        }

        public GetStatusFuture GetStatus(
            #endif
        #else
        public async Task<GetStackStatusResult> GetStatus(
        #endif
            GetStackStatusRequest request
        ) {
            request.WithStackName(this.stackName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            GetStackStatusResult r = await this.client.GetStackStatusAsync(
                request
            );
            return r;
        #else
            return new GetStatusFuture(
                this,
                request
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<GetStackResult> Load(
            #else

        public class LoadFuture : Gs2Future<GetStackResult>
        {
            private StackDomain _domain;
            private GetStackRequest _request;

            public LoadFuture(
                StackDomain domain,
                GetStackRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.GetStack(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.stackCache.Update(r.Result.Item);
                            OnComplete(r.Result);
                        }
                        else
                        {
                            OnError(r.Error);
                        }
                    }
                );
            }
        }

        public LoadFuture Load(
            #endif
        #else
        public async Task<GetStackResult> Load(
        #endif
            GetStackRequest request
        ) {
            request.WithStackName(this.stackName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            GetStackResult r = await this.client.GetStackAsync(
                request
            );
            this.stackCache.Update(r.Item);
            return r;
        #else
            return new LoadFuture(
                this,
                request
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<UpdateStackResult> Update(
            #else

        public class UpdateFuture : Gs2Future<UpdateStackResult>
        {
            private StackDomain _domain;
            private UpdateStackRequest _request;

            public UpdateFuture(
                StackDomain domain,
                UpdateStackRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.UpdateStack(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.stackCache.Update(r.Result.Item);
                            OnComplete(r.Result);
                        }
                        else
                        {
                            OnError(r.Error);
                        }
                    }
                );
            }
        }

        public UpdateFuture Update(
            #endif
        #else
        public async Task<UpdateStackResult> Update(
        #endif
            UpdateStackRequest request
        ) {
            request.WithStackName(this.stackName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            UpdateStackResult r = await this.client.UpdateStackAsync(
                request
            );
            this.stackCache.Update(r.Item);
            return r;
        #else
            return new UpdateFuture(
                this,
                request
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<UpdateStackFromGitHubResult> UpdateFromGitHub(
            #else

        public class UpdateFromGitHubFuture : Gs2Future<UpdateStackFromGitHubResult>
        {
            private StackDomain _domain;
            private UpdateStackFromGitHubRequest _request;

            public UpdateFromGitHubFuture(
                StackDomain domain,
                UpdateStackFromGitHubRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.UpdateStackFromGitHub(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.stackCache.Update(r.Result.Item);
                            OnComplete(r.Result);
                        }
                        else
                        {
                            OnError(r.Error);
                        }
                    }
                );
            }
        }

        public UpdateFromGitHubFuture UpdateFromGitHub(
            #endif
        #else
        public async Task<UpdateStackFromGitHubResult> UpdateFromGitHub(
        #endif
            UpdateStackFromGitHubRequest request
        ) {
            request.WithStackName(this.stackName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            UpdateStackFromGitHubResult r = await this.client.UpdateStackFromGitHubAsync(
                request
            );
            this.stackCache.Update(r.Item);
            return r;
        #else
            return new UpdateFromGitHubFuture(
                this,
                request
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<DeleteStackResult> Delete(
            #else

        public class DeleteFuture : Gs2Future<DeleteStackResult>
        {
            private StackDomain _domain;
            private DeleteStackRequest _request;

            public DeleteFuture(
                StackDomain domain,
                DeleteStackRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.DeleteStack(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.stackCache.Delete(r.Result.Item);
                            OnComplete(r.Result);
                        }
                        else
                        {
                            OnError(r.Error);
                        }
                    }
                );
            }
        }

        public DeleteFuture Delete(
            #endif
        #else
        public async Task<DeleteStackResult> Delete(
        #endif
            DeleteStackRequest request
        ) {
            request.WithStackName(this.stackName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            DeleteStackResult r = await this.client.DeleteStackAsync(
                request
            );
            this.stackCache.Delete(r.Item);
            return r;
        #else
            return new DeleteFuture(
                this,
                request
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<ForceDeleteStackResult> ForceDelete(
            #else

        public class ForceDeleteFuture : Gs2Future<ForceDeleteStackResult>
        {
            private StackDomain _domain;
            private ForceDeleteStackRequest _request;

            public ForceDeleteFuture(
                StackDomain domain,
                ForceDeleteStackRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.ForceDeleteStack(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.stackCache.Update(r.Result.Item);
                            OnComplete(r.Result);
                        }
                        else
                        {
                            OnError(r.Error);
                        }
                    }
                );
            }
        }

        public ForceDeleteFuture ForceDelete(
            #endif
        #else
        public async Task<ForceDeleteStackResult> ForceDelete(
        #endif
            ForceDeleteStackRequest request
        ) {
            request.WithStackName(this.stackName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            ForceDeleteStackResult r = await this.client.ForceDeleteStackAsync(
                request
            );
            this.stackCache.Update(r.Item);
            return r;
        #else
            return new ForceDeleteFuture(
                this,
                request
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<DeleteStackResourcesResult> DeleteResources(
            #else

        public class DeleteResourcesFuture : Gs2Future<DeleteStackResourcesResult>
        {
            private StackDomain _domain;
            private DeleteStackResourcesRequest _request;

            public DeleteResourcesFuture(
                StackDomain domain,
                DeleteStackResourcesRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.DeleteStackResources(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.stackCache.Delete(r.Result.Item);
                            OnComplete(r.Result);
                        }
                        else
                        {
                            OnError(r.Error);
                        }
                    }
                );
            }
        }

        public DeleteResourcesFuture DeleteResources(
            #endif
        #else
        public async Task<DeleteStackResourcesResult> DeleteResources(
        #endif
            DeleteStackResourcesRequest request
        ) {
            request.WithStackName(this.stackName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            DeleteStackResourcesResult r = await this.client.DeleteStackResourcesAsync(
                request
            );
            this.stackCache.Delete(r.Item);
            return r;
        #else
            return new DeleteResourcesFuture(
                this,
                request
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<DeleteStackEntityResult> DeleteEntity(
            #else

        public class DeleteEntityFuture : Gs2Future<DeleteStackEntityResult>
        {
            private StackDomain _domain;
            private DeleteStackEntityRequest _request;

            public DeleteEntityFuture(
                StackDomain domain,
                DeleteStackEntityRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.DeleteStackEntity(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.stackCache.Delete(r.Result.Item);
                            OnComplete(r.Result);
                        }
                        else
                        {
                            OnError(r.Error);
                        }
                    }
                );
            }
        }

        public DeleteEntityFuture DeleteEntity(
            #endif
        #else
        public async Task<DeleteStackEntityResult> DeleteEntity(
        #endif
            DeleteStackEntityRequest request
        ) {
            request.WithStackName(this.stackName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            DeleteStackEntityResult r = await this.client.DeleteStackEntityAsync(
                request
            );
            this.stackCache.Delete(r.Item);
            return r;
        #else
            return new DeleteEntityFuture(
                this,
                request
            );
        #endif
        }

        public DescribeResourcesIterator Resources(
        ) {
            return new DescribeResourcesIterator(
                this.resourceCache,
                this.client,
                this.stackName
            );
        }

        public DescribeEventsIterator Events(
        ) {
            return new DescribeEventsIterator(
                this.eventCache,
                this.client,
                this.stackName
            );
        }

        public DescribeOutputsIterator Outputs(
        ) {
            return new DescribeOutputsIterator(
                this.outputCache,
                this.client,
                this.stackName
            );
        }

        public EventDomain Event(
            string eventName
        ) {
            return new EventDomain(
                this.session,
                this.eventCache,
                this.stackName,
                eventName
            );
        }

        public OutputDomain Output(
            string outputName
        ) {
            return new OutputDomain(
                this.session,
                this.outputCache,
                this.stackName,
                outputName
            );
        }

        public ResourceDomain Resource(
            string resourceName
        ) {
            return new ResourceDomain(
                this.session,
                this.resourceCache,
                this.stackName,
                resourceName
            );
        }

    }
}

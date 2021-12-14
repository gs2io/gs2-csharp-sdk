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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Deploy.Domain.Cache;
using Gs2.Gs2Deploy.Domain.Iterator;
using Gs2.Gs2Deploy.Domain.Model;
using Gs2.Gs2Deploy.Request;
using Gs2.Gs2Deploy.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #else
using Gs2.Core;
using Gs2.Core.Domain;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Deploy.Domain
{

    public class Gs2Deploy {
        Gs2RestSession session;
        Gs2DeployRestClient client;
        StackDomainCache stackCache;

        public Gs2Deploy(
            Gs2RestSession session
        ) {
            this.session = session;
            this.client = new Gs2DeployRestClient (
                session
            );
            this.stackCache = new StackDomainCache();
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<CreateStackResult> CreateStack(
            #else

        public class CreateStackFuture : Gs2Future<CreateStackResult>
        {
            private Gs2Deploy _domain;
            private CreateStackRequest _request;

            public CreateStackFuture(
                Gs2Deploy domain,
                CreateStackRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.CreateStack(
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

        public CreateStackFuture CreateStack(
            #endif
        #else
        public async Task<CreateStackResult> CreateStack(
        #endif
            CreateStackRequest request
        ) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            return new CreateStackFuture(
                this,
                request
            );
        #else
            var r = await this.client.CreateStackAsync(
                request
            );
            this.stackCache.Update(r.Item);
            return r;
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<CreateStackFromGitHubResult> CreateStackFromGitHub(
            #else

        public class CreateStackFromGitHubFuture : Gs2Future<CreateStackFromGitHubResult>
        {
            private Gs2Deploy _domain;
            private CreateStackFromGitHubRequest _request;

            public CreateStackFromGitHubFuture(
                Gs2Deploy domain,
                CreateStackFromGitHubRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.CreateStackFromGitHub(
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

        public CreateStackFromGitHubFuture CreateStackFromGitHub(
            #endif
        #else
        public async Task<CreateStackFromGitHubResult> CreateStackFromGitHub(
        #endif
            CreateStackFromGitHubRequest request
        ) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            return new CreateStackFromGitHubFuture(
                this,
                request
            );
        #else
            var r = await this.client.CreateStackFromGitHubAsync(
                request
            );
            this.stackCache.Update(r.Item);
            return r;
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<ValidateResult> Validate(
            #else

        public class ValidateFuture : Gs2Future<ValidateResult>
        {
            private Gs2Deploy _domain;
            private ValidateRequest _request;

            public ValidateFuture(
                Gs2Deploy domain,
                ValidateRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.Validate(
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

        public ValidateFuture Validate(
            #endif
        #else
        public async Task<ValidateResult> Validate(
        #endif
            ValidateRequest request
        ) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            return new ValidateFuture(
                this,
                request
            );
        #else
            var r = await this.client.ValidateAsync(
                request
            );
            return r;
        #endif
        }

        public DescribeStacksIterator Stacks(
        ) {
            return new DescribeStacksIterator(
                this.stackCache,
                this.client
            );
        }

        public StackDomain Stack(
            string stackName
        ) {
            return new StackDomain(
                this.session,
                this.stackCache,
                stackName
            );
        }
    }
}

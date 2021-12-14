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

    public class ResourceDomain {
        Gs2RestSession session;
        Gs2DeployRestClient client;
        ResourceDomainCache resourceCache;
        string stackName;
        string resourceName;

        public ResourceDomain(
            Gs2RestSession session,
            ResourceDomainCache resourceCache,
            string stackName,
            string resourceName
        ) {
            this.session = session;
            this.client = new Gs2DeployRestClient(
                session
            );
            this.resourceCache = resourceCache;
            this.stackName = stackName;
            this.resourceName = resourceName;
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<GetResourceResult> Load(
            #else

        public class LoadFuture : Gs2Future<GetResourceResult>
        {
            private ResourceDomain _domain;
            private GetResourceRequest _request;

            public LoadFuture(
                ResourceDomain domain,
                GetResourceRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.GetResource(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.resourceCache.Update(r.Result.Item);
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
        public async Task<GetResourceResult> Load(
        #endif
            GetResourceRequest request
        ) {
            request.WithStackName(this.stackName);
            request.WithResourceName(this.resourceName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            GetResourceResult r = await this.client.GetResourceAsync(
                request
            );
            this.resourceCache.Update(r.Item);
            return r;
        #else
            return new LoadFuture(
                this,
                request
            );
        #endif
        }

    }
}

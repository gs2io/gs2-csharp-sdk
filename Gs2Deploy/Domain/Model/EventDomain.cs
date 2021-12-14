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

    public class EventDomain {
        Gs2RestSession session;
        Gs2DeployRestClient client;
        EventDomainCache eventCache;
        string stackName;
        string eventName;

        public EventDomain(
            Gs2RestSession session,
            EventDomainCache eventCache,
            string stackName,
            string eventName
        ) {
            this.session = session;
            this.client = new Gs2DeployRestClient(
                session
            );
            this.eventCache = eventCache;
            this.stackName = stackName;
            this.eventName = eventName;
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<GetEventResult> Load(
            #else

        public class LoadFuture : Gs2Future<GetEventResult>
        {
            private EventDomain _domain;
            private GetEventRequest _request;

            public LoadFuture(
                EventDomain domain,
                GetEventRequest request
            )
            {
                _domain = domain;
                _request = request;
            }

            public override IEnumerator Action()
            {
                yield return _domain.client.GetEvent(
                    _request,
                    r =>
                    {
                        if (r.Error == null)
                        {
                            _domain.eventCache.Update(r.Result.Item);
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
        public async Task<GetEventResult> Load(
        #endif
            GetEventRequest request
        ) {
            request.WithStackName(this.stackName);
            request.WithEventName(this.eventName);
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            GetEventResult r = await this.client.GetEventAsync(
                request
            );
            this.eventCache.Update(r.Item);
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

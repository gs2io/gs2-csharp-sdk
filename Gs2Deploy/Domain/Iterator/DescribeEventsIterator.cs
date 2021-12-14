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
using Gs2.Gs2Deploy.Domain.Cache;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using System.Collections.Generic;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
    #else
using System.Collections;
using UnityEngine.Events;
using Gs2.Core;
using Gs2.Core.Exception;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Deploy.Domain.Iterator
{

    #if UNITY_2017_1_OR_NEWER
    public class DescribeEventsIterator {
    #else
    public class DescribeEventsIterator : IAsyncEnumerable<Gs2.Gs2Deploy.Model.Event> {
    #endif
        EventDomainCache eventCache;
        Gs2DeployRestClient client;
        string stackName;
        string? pageToken;
        bool last;
        Gs2.Gs2Deploy.Model.Event[] result;

        int? fetchSize;

        public DescribeEventsIterator(
            EventDomainCache eventCache,
            Gs2DeployRestClient client,
            string stackName
        ) {
            this.eventCache = eventCache;
            this.client = client;
            this.stackName = stackName;
            this.pageToken = null;
            this.last = false;
            this.result = new Gs2.Gs2Deploy.Model.Event[]{};

            this.fetchSize = null;
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            this.load();
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask load() {
            #else
        private IEnumerator load(UnityAction<AsyncResult<Result.DescribeEventsResult>> callback) {
            #endif
        #else
        private async Task load() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            void Callback(AsyncResult<Result.DescribeEventsResult> r)
            {
                if (r.Error == null)
                {
                    foreach (var item in r.Result.Items) {
                        this.eventCache.Update(item);
                    }
                    this.result = r.Result.Items;
                    this.pageToken = r.Result.NextPageToken;
                    this.last = this.pageToken == null;
                }
                callback(r);
            }
            yield return this.client.DescribeEvents(
                new Gs2.Gs2Deploy.Request.DescribeEventsRequest()
                    .WithStackName(this.stackName)
                    .WithPageToken(this.pageToken)
                    .WithLimit(this.fetchSize),
                Callback
            );
        #else
            var r = await this.client.DescribeEventsAsync(
                new Gs2.Gs2Deploy.Request.DescribeEventsRequest()
                    .WithStackName(this.stackName)
                    .WithPageToken(this.pageToken)
                    .WithLimit(this.fetchSize)
            );
            foreach (var item in r.Items) {
                this.eventCache.Update(item);
            }
            this.result = r.Items;
            this.pageToken = r.NextPageToken;
            this.last = this.pageToken == null;
        #endif
        }

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
        private bool hasNext()
        #else
        public bool HasNext()
        #endif
        {
            return this.result.Length != 0 || !this.last;
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK

        public IUniTaskAsyncEnumerable<Gs2.Gs2Deploy.Model.Event> GetAsyncEnumerator()
        {
            return UniTaskAsyncEnumerable.Create<Gs2.Gs2Deploy.Model.Event>(async (writer, token) =>
            {
                while(this.hasNext()) {
                    if (this.result.Length == 0 && !this.last) {
                        await this.load();
                    }
                    if (this.result.Length == 0) {
                        break;
                    }
                    Gs2.Gs2Deploy.Model.Event ret = this.result[0];
                    this.result = this.result.ToList().GetRange(1, this.result.Length - 1).ToArray();
                    if (this.result.Length == 0 && !this.last) {
                        await this.load();
                    }
                    await writer.YieldAsync(ret);
                }
            });
        }

            #else

        public IEnumerator Next(UnityAction<AsyncResult<Gs2.Gs2Deploy.Model.Event>> callback)
        {
            Gs2Exception exception = null;
            if (this.result.Length == 0 && !this.last) {
                void Callback(AsyncResult<Result.DescribeEventsResult> r)
                {
                    if (r.Error != null)
                    {
                        exception = r.Error;
                    }
                }
                yield return this.load(Callback);
                if (exception != null)
                {
                    callback.Invoke(new AsyncResult<Gs2.Gs2Deploy.Model.Event>(
                        null,
                        exception
                    ));
                    yield break;
                }
            }
            if (this.result.Length == 0) {
                callback.Invoke(new AsyncResult<Gs2.Gs2Deploy.Model.Event>(
                    null,
                    null
                ));
                yield break;
            }
            Gs2.Gs2Deploy.Model.Event ret = this.result[0];
            this.result = this.result.ToList().GetRange(1, this.result.Length - 1).ToArray();
            if (this.result.Length == 0 && !this.last) {
                void Callback(AsyncResult<Result.DescribeEventsResult> r)
                {
                    if (r.Error != null)
                    {
                        exception = r.Error;
                    }
                }
                yield return this.load(Callback);
                if (exception != null)
                {
                    callback.Invoke(new AsyncResult<Gs2.Gs2Deploy.Model.Event>(
                        null,
                        exception
                    ));
                    yield break;
                }
            }
            callback.Invoke(new AsyncResult<Gs2.Gs2Deploy.Model.Event>(
                ret,
                null
            ));
        }

            #endif
        #else

        public async IAsyncEnumerator<Gs2.Gs2Deploy.Model.Event> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        )
        {
            while(this.hasNext()) {
                if (this.result.Length == 0 && !this.last) {
                    await this.load();
                }
                if (this.result.Length == 0) {
                    break;
                }
                Gs2.Gs2Deploy.Model.Event ret = this.result[0];
                this.result = this.result.ToList().GetRange(1, this.result.Length - 1).ToArray();
                if (this.result.Length == 0 && !this.last) {
                    await this.load();
                }
                yield return ret;
            }
        }

        #endif
    }
}

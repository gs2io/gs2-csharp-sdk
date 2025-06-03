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
using Gs2.Gs2Freeze.Domain.Iterator;
using Gs2.Gs2Freeze.Model.Cache;
using Gs2.Gs2Freeze.Domain.Model;
using Gs2.Gs2Freeze.Request;
using Gs2.Gs2Freeze.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Freeze.Model;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Freeze.Domain
{

    public class Gs2Freeze {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FreezeRestClient _client;

        public Gs2Freeze(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FreezeRestClient(
                gs2.RestSession
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Freeze.Model.Stage> Stages(
        )
        {
            return new DescribeStagesIterator(
                this._gs2,
                this._client
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Freeze.Model.Stage> StagesAsync(
            #else
        public DescribeStagesIterator StagesAsync(
            #endif
        )
        {
            return new DescribeStagesIterator(
                this._gs2,
                this._client
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeStages(
            Action<Gs2.Gs2Freeze.Model.Stage[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Freeze.Model.Stage>(
                (null as Gs2.Gs2Freeze.Model.Stage).CacheParentKey(
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await StagesAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeStagesWithInitialCallAsync(
            Action<Gs2.Gs2Freeze.Model.Stage[]> callback
        )
        {
            var items = await StagesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeStages(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeStages(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Freeze.Model.Stage>(
                (null as Gs2.Gs2Freeze.Model.Stage).CacheParentKey(
                ),
                callbackId
            );
        }

        public void InvalidateStages(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Freeze.Model.Stage>(
                (null as Gs2.Gs2Freeze.Model.Stage).CacheParentKey(
                )
            );
        }

        public Gs2.Gs2Freeze.Domain.Model.StageDomain Stage(
            string stageName
        ) {
            return new Gs2.Gs2Freeze.Domain.Model.StageDomain(
                this._gs2,
                stageName
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
        }
    }
}

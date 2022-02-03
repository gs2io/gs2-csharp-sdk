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
using Gs2.Gs2JobQueue.Domain.Iterator;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
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

namespace Gs2.Gs2JobQueue.Domain.Model
{

    public partial class JobResultAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2JobQueueRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;
        private readonly string _jobName;
        private readonly string _tryNumber;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public string JobName => _jobName;
        public string TryNumber => _tryNumber;

        public JobResultAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string jobName,
            string tryNumber
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2JobQueueRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._jobName = jobName;
            this._tryNumber = tryNumber;
            this._parentKey = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                this._jobName != null ? this._jobName.ToString() : null,
                "JobResult"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string jobName,
            string tryNumber,
            string childType
        )
        {
            return string.Join(
                ":",
                "jobQueue",
                namespaceName ?? "null",
                userId ?? "null",
                jobName ?? "null",
                tryNumber ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string tryNumber
        )
        {
            return string.Join(
                ":",
                tryNumber ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2JobQueue.Model.JobResult> Model() {
            #else
        public IFuture<Gs2.Gs2JobQueue.Model.JobResult> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2JobQueue.Model.JobResult> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Model.JobResult> self)
            {
        #endif
            Gs2.Gs2JobQueue.Model.JobResult value = _cache.Get<Gs2.Gs2JobQueue.Model.JobResult>(
                _parentKey,
                Gs2.Gs2JobQueue.Domain.Model.JobResultDomain.CreateCacheKey(
                    this.TryNumber?.ToString()
                )
            );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Model.JobResult>(Impl);
        #endif
        }

    }
}
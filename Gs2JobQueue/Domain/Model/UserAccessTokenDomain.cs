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
#pragma warning disable CS0169, CS0168

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2JobQueue.Domain.Iterator;
using Gs2.Gs2JobQueue.Model.Cache;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2JobQueue.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2JobQueueRestClient _client;
        public string NamespaceName { get; }
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public bool? AutoRun { get; set; }
        public bool? IsLastJob { get; set; }
        public string NextPageToken { get; set; }

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2JobQueueRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain> RunFuture(
            RunRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.RunFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                if (result?.Item != null) {
                    this._gs2.UpdateCacheFromJobResult(
                        result?.Item,
                        result?.Result
                    );
                }
                var domain = new Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    result?.Item?.Name
                );
                domain.IsLastJob = result?.IsLastJob;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain> RunAsync(
            #else
        public async Task<Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain> RunAsync(
            #endif
            RunRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.RunAsync(request)
            );
            if (result?.Item != null) {
                this._gs2.UpdateCacheFromJobResult(
                    result?.Item,
                    result?.Result
                );
            }
            var domain = new Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.Name
            );
            domain.IsLastJob = result?.IsLastJob;

            return domain;
        }
        #endif

        public Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain Job(
            string jobName
        ) {
            return new Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                jobName
            );
        }

        public Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobAccessTokenDomain DeadLetterJob(
            string deadLetterJobName
        ) {
            return new Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                deadLetterJobName
            );
        }

    }
}

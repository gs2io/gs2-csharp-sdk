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
using Gs2.Gs2Identifier.Domain.Iterator;
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
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

namespace Gs2.Gs2Identifier.Domain.Model
{

    public partial class AttachSecurityPolicyDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2IdentifierRestClient _client;
        private readonly string _userName;

        private readonly String _parentKey;
        public string UserName => _userName;

        public AttachSecurityPolicyDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string userName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2IdentifierRestClient(
                session
            );
            this._userName = userName;
            this._parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                this._userName != null ? this._userName.ToString() : null,
                "AttachSecurityPolicy"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> GetHasSecurityPolicyAsync(
            #else
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> GetHasSecurityPolicy(
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> GetHasSecurityPolicyAsync(
        #endif
            GetHasSecurityPolicyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> self)
            {
        #endif
            request
                .WithUserName(this._userName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetHasSecurityPolicyFuture(
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
              
            #else
            var result = await this._client.GetHasSecurityPolicyAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            #endif
            Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[] domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result.Items[i]?.Name
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> AttachSecurityPolicyAsync(
            #else
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> AttachSecurityPolicy(
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> AttachSecurityPolicyAsync(
        #endif
            AttachSecurityPolicyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> self)
            {
        #endif
            request
                .WithUserName(this._userName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.AttachSecurityPolicyFuture(
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
              
            #else
            var result = await this._client.AttachSecurityPolicyAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            #endif
            Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[] domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result.Items[i]?.Name
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> DetachSecurityPolicyAsync(
            #else
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> DetachSecurityPolicy(
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> DetachSecurityPolicyAsync(
        #endif
            DetachSecurityPolicyRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> self)
            {
        #endif
            request
                .WithUserName(this._userName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DetachSecurityPolicyFuture(
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
              
            #else
            var result = await this._client.DetachSecurityPolicyAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            #endif
            Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[] domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result.Items[i]?.Name
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string userName,
            string childType
        )
        {
            return string.Join(
                ":",
                "identifier",
                userName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> Model() {
            #else
        public IFuture<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> self)
            {
        #endif
            Gs2.Gs2Identifier.Model.AttachSecurityPolicy value = _cache.Get<Gs2.Gs2Identifier.Model.AttachSecurityPolicy>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.AttachSecurityPolicy>(Impl);
        #endif
        }

    }
}

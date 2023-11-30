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
using Gs2.Gs2Version.Domain.Iterator;
using Gs2.Gs2Version.Request;
using Gs2.Gs2Version.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
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

namespace Gs2.Gs2Version.Domain.Model
{

    public partial class CheckerDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2VersionRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string ProjectToken { get; set; }
        public Gs2.Gs2Version.Model.Status[] Warnings { get; set; }
        public Gs2.Gs2Version.Model.Status[] Errors { get; set; }
        public string Body { get; set; }
        public string Signature { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public CheckerDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2VersionRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Version.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Checker"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "version",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

    }

    public partial class CheckerDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Version.Domain.Model.CheckerDomain> CheckVersionFuture(
            CheckVersionByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Version.Domain.Model.CheckerDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.CheckVersionByUserIdFuture(
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
                this.ProjectToken = domain.ProjectToken = result?.ProjectToken;
                this.Warnings = domain.Warnings = result?.Warnings;
                this.Errors = domain.Errors = result?.Errors;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Version.Domain.Model.CheckerDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Version.Domain.Model.CheckerDomain> CheckVersionAsync(
            #else
        public async Task<Gs2.Gs2Version.Domain.Model.CheckerDomain> CheckVersionAsync(
            #endif
            CheckVersionByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            CheckVersionByUserIdResult result = null;
                result = await this._client.CheckVersionByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.ProjectToken = domain.ProjectToken = result?.ProjectToken;
            this.Warnings = domain.Warnings = result?.Warnings;
            this.Errors = domain.Errors = result?.Errors;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CheckVersionFuture.")]
        public IFuture<Gs2.Gs2Version.Domain.Model.CheckerDomain> CheckVersion(
            CheckVersionByUserIdRequest request
        ) {
            return CheckVersionFuture(request);
        }
        #endif

    }

    public partial class CheckerDomain {

    }
}

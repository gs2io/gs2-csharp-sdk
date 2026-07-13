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
 * deny overwrite
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2MegaField.Domain.Iterator;
using Gs2.Gs2MegaField.Model.Cache;
using Gs2.Gs2MegaField.Request;
using Gs2.Gs2MegaField.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2MegaField.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MegaFieldRestClient _client;
/* diff --- start
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
 diff --- end */
/* diff +++ start */
        public string NamespaceName { get; }
        public string UserId { get; }
/* diff +++ end */

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MegaFieldRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }

        public Gs2.Gs2MegaField.Domain.Model.SpatialDomain Spatial(
            string areaModelName,
            string layerModelName
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.SpatialDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                areaModelName,
                layerModelName
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystemFuture(
            FetchPositionFromSystemRequest request
        ) => FetchPositionFromSystemAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystemAsync(
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystemAsync(
        #endif
            FetchPositionFromSystemRequest request
        ) {
            request = request
/* diff --- start
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
 diff --- end */
                .WithNamespaceName(this.NamespaceName); /* diff +++ */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.FetchPositionFromSystemAsync(request)
            );
            var domain = result?.Items?.Select(v => new Gs2.Gs2MegaField.Domain.Model.SpatialDomain(
                this._gs2,
                this.NamespaceName,
                v?.UserId,
                v?.AreaModelName,
                v?.LayerModelName
            )).ToArray() ?? Array.Empty<Gs2.Gs2MegaField.Domain.Model.SpatialDomain>();
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemFuture(
            NearUserIdsFromSystemRequest request
        ) => NearUserIdsFromSystemAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemAsync(
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemAsync(
        #endif
            NearUserIdsFromSystemRequest request
        ) {
            request = request
/* diff --- start
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
 diff --- end */
                .WithNamespaceName(this.NamespaceName); /* diff +++ */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.NearUserIdsFromSystemAsync(request)
            );
/* diff --- start
            var domain = result?.Items?.Select(v => new Gs2.Gs2MegaField.Domain.Model.SpatialDomain(
                this._gs2
            )).ToArray() ?? Array.Empty<Gs2.Gs2MegaField.Domain.Model.SpatialDomain>();
 diff --- end */
/* diff +++ start */
            var domain = new Gs2.Gs2MegaField.Domain.Model.SpatialDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2MegaField.Domain.Model.SpatialDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Items[i],
                    request.AreaModelName,
                    request.LayerModelName
                );
            }
/* diff +++ end */
            return domain;
        }

    }

    public partial class UserDomain {

    }
}

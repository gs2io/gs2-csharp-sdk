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
 *
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

namespace Gs2.Gs2MegaField.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MegaFieldRestClient _client;
        public string NamespaceName { get; }
        public string UserId { get; }

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
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.FetchPositionFromSystemFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = result?.Items?.Select(v => new Gs2.Gs2MegaField.Domain.Model.SpatialDomain(
                    this._gs2,
                    this.NamespaceName,
                    v?.UserId,
                    v?.AreaModelName,
                    v?.LayerModelName
                )).ToArray() ?? Array.Empty<Gs2.Gs2MegaField.Domain.Model.SpatialDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystemAsync(
            #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> FetchPositionFromSystemAsync(
            #endif
            FetchPositionFromSystemRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName);
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
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemFuture(
            NearUserIdsFromSystemRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.NearUserIdsFromSystemFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemAsync(
            #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.SpatialDomain[]> NearUserIdsFromSystemAsync(
            #endif
            NearUserIdsFromSystemRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.NearUserIdsFromSystemAsync(request)
            );
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
            return domain;
        }
        #endif

    }

    public partial class UserDomain {

    }
}

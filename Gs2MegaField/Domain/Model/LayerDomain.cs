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
using Gs2.Gs2MegaField.Domain.Iterator;
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
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2MegaField.Domain.Model
{

    public partial class LayerDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MegaFieldRestClient _client;
        private readonly string _namespaceName;
        private readonly string _areaModelName;
        private readonly string _layerModelName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string AreaModelName => _areaModelName;
        public string LayerModelName => _layerModelName;

        public LayerDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string areaModelName,
            string layerModelName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2MegaFieldRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._areaModelName = areaModelName;
            this._layerModelName = layerModelName;
            this._parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Layer"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string areaModelName,
            string layerModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "megaField",
                namespaceName ?? "null",
                areaModelName ?? "null",
                layerModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string areaModelName,
            string layerModelName
        )
        {
            return string.Join(
                ":",
                areaModelName ?? "null",
                layerModelName ?? "null"
            );
        }

    }

    public partial class LayerDomain {

    }

    public partial class LayerDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Model.Layer> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.Layer> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2MegaField.Model.Layer>(
                    _parentKey,
                    Gs2.Gs2MegaField.Domain.Model.LayerDomain.CreateCacheKey(
                        this.AreaModelName?.ToString(),
                        this.LayerModelName?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.Layer>(Impl);
        }
        #else
        public async Task<Gs2.Gs2MegaField.Model.Layer> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2MegaField.Model.Layer>(
                    _parentKey,
                    Gs2.Gs2MegaField.Domain.Model.LayerDomain.CreateCacheKey(
                        this.AreaModelName?.ToString(),
                        this.LayerModelName?.ToString()
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Model.Layer> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2MegaField.Model.Layer> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2MegaField.Model.Layer> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2MegaField.Model.Layer> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

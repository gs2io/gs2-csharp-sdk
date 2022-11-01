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
using Gs2.Gs2Log.Domain.Iterator;
using Gs2.Gs2Log.Request;
using Gs2.Gs2Log.Result;
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

namespace Gs2.Gs2Log.Domain.Model
{

    public partial class InsightDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LogRestClient _client;
        private readonly string _namespaceName;
        private readonly string _insightName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string InsightName => _insightName;

        public InsightDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string insightName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LogRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._insightName = insightName;
            this._parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Insight"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Log.Model.Insight> GetAsync(
            #else
        private IFuture<Gs2.Gs2Log.Model.Insight> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Log.Model.Insight> GetAsync(
        #endif
            GetInsightRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Insight> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInsightName(this.InsightName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetInsightFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.GetInsightAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Insight"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Model.Insight>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.InsightDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.InsightDomain> DeleteAsync(
        #endif
            DeleteInsightRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInsightName(this.InsightName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteInsightFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            DeleteInsightResult result = null;
            try {
                result = await this._client.DeleteInsightAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "insight")
                {
                    var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Insight"
                );
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        request.InsightName.ToString()
                    );
                    _cache.Delete<Gs2.Gs2Log.Model.Insight>(parentKey, key);
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Insight"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Log.Model.Insight>(parentKey, key);
                }
            }
            Gs2.Gs2Log.Domain.Model.InsightDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.InsightDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string insightName,
            string childType
        )
        {
            return string.Join(
                ":",
                "log",
                namespaceName ?? "null",
                insightName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string insightName
        )
        {
            return string.Join(
                ":",
                insightName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Model.Insight> Model() {
            #else
        public IFuture<Gs2.Gs2Log.Model.Insight> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Log.Model.Insight> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Insight> self)
            {
        #endif
            Gs2.Gs2Log.Model.Insight value = _cache.Get<Gs2.Gs2Log.Model.Insight>(
                _parentKey,
                Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                    this.InsightName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetInsightRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "insight")
                            {
                                _cache.Delete<Gs2.Gs2Log.Model.Insight>(
                                    _parentKey,
                                    Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                                        this.InsightName?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "insight")
                    {
                        _cache.Delete<Gs2.Gs2Log.Model.Insight>(
                            _parentKey,
                            Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                                this.InsightName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Log.Model.Insight>(
                    _parentKey,
                    Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        this.InsightName?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Model.Insight>(Impl);
        #endif
        }

    }
}

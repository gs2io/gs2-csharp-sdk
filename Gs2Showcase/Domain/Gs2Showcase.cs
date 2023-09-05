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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Showcase.Domain.Iterator;
using Gs2.Gs2Showcase.Domain.Model;
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Showcase.Model;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Showcase.Domain
{

    public class Gs2Showcase {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ShowcaseRestClient _client;

        private readonly String _parentKey;

        public Gs2Showcase(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ShowcaseRestClient(
                session
            );
            this._parentKey = "showcase";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public IFuture<Gs2.Gs2Showcase.Domain.Model.NamespaceDomain> CreateNamespace(
            #endif
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #endif
            CreateNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.NamespaceDomain> self)
            {
        #endif
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateNamespaceFuture(
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
            var result = await this._client.CreateNamespaceAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "showcase",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            var domain = new Gs2.Gs2Showcase.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                result?.Item?.Name
            );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Showcase.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Showcase.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Showcase.Model.Namespace> Namespaces(
            #endif
        #else
        public DescribeNamespacesIterator Namespaces(
        #endif
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public Gs2.Gs2Showcase.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Showcase.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DecrementPurchaseCountByUserIdRequest, DecrementPurchaseCountByUserIdResult> DecrementPurchaseCountByUserIdComplete = new UnityEvent<string, DecrementPurchaseCountByUserIdRequest, DecrementPurchaseCountByUserIdResult>();
    #else
        public static Action<string, DecrementPurchaseCountByUserIdRequest, DecrementPurchaseCountByUserIdResult> DecrementPurchaseCountByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ForceReDrawByUserIdRequest, ForceReDrawByUserIdResult> ForceReDrawByUserIdComplete = new UnityEvent<string, ForceReDrawByUserIdRequest, ForceReDrawByUserIdResult>();
    #else
        public static Action<string, ForceReDrawByUserIdRequest, ForceReDrawByUserIdResult> ForceReDrawByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampSheet(
                CacheDatabase cache,
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DecrementPurchaseCountByUserId": {
                        var requestModel = DecrementPurchaseCountByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DecrementPurchaseCountByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.ShowcaseName,
                                "RandomDisplayItem"
                            );
                            var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        DecrementPurchaseCountByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ForceReDrawByUserId": {
                        var requestModel = ForceReDrawByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ForceReDrawByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.ShowcaseName,
                                "RandomDisplayItem"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                                    item.Name.ToString()
                                );
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }

                        ForceReDrawByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, IncrementPurchaseCountByUserIdRequest, IncrementPurchaseCountByUserIdResult> IncrementPurchaseCountByUserIdComplete = new UnityEvent<string, IncrementPurchaseCountByUserIdRequest, IncrementPurchaseCountByUserIdResult>();
    #else
        public static Action<string, IncrementPurchaseCountByUserIdRequest, IncrementPurchaseCountByUserIdResult> IncrementPurchaseCountByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampTask(
                CacheDatabase cache,
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "IncrementPurchaseCountByUserId": {
                        var requestModel = IncrementPurchaseCountByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = IncrementPurchaseCountByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.ShowcaseName,
                                "RandomDisplayItem"
                            );
                            var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        IncrementPurchaseCountByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

        public static void UpdateCacheFromJobResult(
                CacheDatabase cache,
                string method,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
            switch (method) {
                case "decrement_purchase_count_by_user_id": {
                    var requestModel = DecrementPurchaseCountByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = DecrementPurchaseCountByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.ShowcaseName,
                            "RandomDisplayItem"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    DecrementPurchaseCountByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "force_re_draw_by_user_id": {
                    var requestModel = ForceReDrawByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = ForceReDrawByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    {
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.ShowcaseName,
                            "RandomDisplayItem"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                                item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }

                    ForceReDrawByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
            }
        }

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
    #if UNITY_2017_1_OR_NEWER
    #endif
        }
    }
}

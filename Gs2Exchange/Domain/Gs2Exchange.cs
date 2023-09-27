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
using Gs2.Gs2Exchange.Domain.Iterator;
using Gs2.Gs2Exchange.Domain.Model;
using Gs2.Gs2Exchange.Request;
using Gs2.Gs2Exchange.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Exchange.Model;
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

namespace Gs2.Gs2Exchange.Domain
{

    public class Gs2Exchange {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ExchangeRestClient _client;

        private readonly String _parentKey;

        public Gs2Exchange(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ExchangeRestClient(
                session
            );
            this._parentKey = "exchange";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                CreateNamespaceResult result = null;
                    result = await this._client.CreateNamespaceAsync(
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
                            "exchange",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Exchange.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Exchange.Domain.Model.NamespaceDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            CreateNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            CreateNamespaceResult result = null;
                result = await this._client.CreateNamespaceAsync(
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
                        "exchange",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Exchange.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Exchange.Domain.Model.NamespaceDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            CreateNamespaceRequest request
        ) {
            var future = CreateNamespaceFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateNamespaceFuture.")]
        public IFuture<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> CreateNamespace(
            CreateNamespaceRequest request
        ) {
            return CreateNamespaceFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Exchange.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Exchange.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Exchange.Model.Namespace> Namespaces(
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

        public ulong SubscribeNamespaces(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Exchange.Model.Namespace>(
                "exchange:Namespace",
                callback
            );
        }

        public void UnsubscribeNamespaces(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Exchange.Model.Namespace>(
                "exchange:Namespace",
                callbackId
            );
        }

        public Gs2.Gs2Exchange.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Exchange.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ExchangeByUserIdRequest, ExchangeByUserIdResult> ExchangeByUserIdComplete = new UnityEvent<string, ExchangeByUserIdRequest, ExchangeByUserIdResult>();
    #else
        public static Action<string, ExchangeByUserIdRequest, ExchangeByUserIdResult> ExchangeByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, IncrementalExchangeByUserIdRequest, IncrementalExchangeByUserIdResult> IncrementalExchangeByUserIdComplete = new UnityEvent<string, IncrementalExchangeByUserIdRequest, IncrementalExchangeByUserIdResult>();
    #else
        public static Action<string, IncrementalExchangeByUserIdRequest, IncrementalExchangeByUserIdResult> IncrementalExchangeByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, UnlockIncrementalExchangeByUserIdRequest, UnlockIncrementalExchangeByUserIdResult> UnlockIncrementalExchangeByUserIdComplete = new UnityEvent<string, UnlockIncrementalExchangeByUserIdRequest, UnlockIncrementalExchangeByUserIdResult>();
    #else
        public static Action<string, UnlockIncrementalExchangeByUserIdRequest, UnlockIncrementalExchangeByUserIdResult> UnlockIncrementalExchangeByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, CreateAwaitByUserIdRequest, CreateAwaitByUserIdResult> CreateAwaitByUserIdComplete = new UnityEvent<string, CreateAwaitByUserIdRequest, CreateAwaitByUserIdResult>();
    #else
        public static Action<string, CreateAwaitByUserIdRequest, CreateAwaitByUserIdResult> CreateAwaitByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampSheet(
                CacheDatabase cache,
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "ExchangeByUserId": {
                        var requestModel = ExchangeByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ExchangeByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Exchange.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "RateModel"
                            );
                            var key = Gs2.Gs2Exchange.Domain.Model.RateModelDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        ExchangeByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "IncrementalExchangeByUserId": {
                        var requestModel = IncrementalExchangeByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = IncrementalExchangeByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Exchange.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "IncrementalRateModel"
                            );
                            var key = Gs2.Gs2Exchange.Domain.Model.IncrementalRateModelDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        IncrementalExchangeByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "UnlockIncrementalExchangeByUserId": {
                        var requestModel = UnlockIncrementalExchangeByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = UnlockIncrementalExchangeByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Exchange.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "IncrementalRateModel"
                            );
                            var key = Gs2.Gs2Exchange.Domain.Model.IncrementalRateModelDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        UnlockIncrementalExchangeByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "CreateAwaitByUserId": {
                        var requestModel = CreateAwaitByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = CreateAwaitByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Await"
                            );
                            var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        CreateAwaitByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DeleteAwaitByUserIdRequest, DeleteAwaitByUserIdResult> DeleteAwaitByUserIdComplete = new UnityEvent<string, DeleteAwaitByUserIdRequest, DeleteAwaitByUserIdResult>();
    #else
        public static Action<string, DeleteAwaitByUserIdRequest, DeleteAwaitByUserIdResult> DeleteAwaitByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampTask(
                CacheDatabase cache,
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DeleteAwaitByUserId": {
                        var requestModel = DeleteAwaitByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DeleteAwaitByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Await"
                            );
                            var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Delete<Gs2.Gs2Exchange.Model.Await>(parentKey, key);
                        }

                        DeleteAwaitByUserIdComplete?.Invoke(
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
                case "exchange_by_user_id": {
                    var requestModel = ExchangeByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = ExchangeByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Exchange.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "RateModel"
                        );
                        var key = Gs2.Gs2Exchange.Domain.Model.RateModelDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    ExchangeByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "incremental_exchange_by_user_id": {
                    var requestModel = IncrementalExchangeByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = IncrementalExchangeByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Exchange.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "IncrementalRateModel"
                        );
                        var key = Gs2.Gs2Exchange.Domain.Model.IncrementalRateModelDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    IncrementalExchangeByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "unlock_incremental_exchange_by_user_id": {
                    var requestModel = UnlockIncrementalExchangeByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = UnlockIncrementalExchangeByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Exchange.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "IncrementalRateModel"
                        );
                        var key = Gs2.Gs2Exchange.Domain.Model.IncrementalRateModelDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    UnlockIncrementalExchangeByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "create_await_by_user_id": {
                    var requestModel = CreateAwaitByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = CreateAwaitByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Await"
                        );
                        var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    CreateAwaitByUserIdComplete?.Invoke(
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

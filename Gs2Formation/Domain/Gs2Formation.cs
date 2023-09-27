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
using Gs2.Gs2Formation.Domain.Iterator;
using Gs2.Gs2Formation.Domain.Model;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Formation.Model;
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

namespace Gs2.Gs2Formation.Domain
{

    public class Gs2Formation {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FormationRestClient _client;

        private readonly String _parentKey;

        public Gs2Formation(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FormationRestClient(
                session
            );
            this._parentKey = "formation";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> self)
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
                            "formation",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Formation.Domain.Model.NamespaceDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
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
                        "formation",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Formation.Domain.Model.NamespaceDomain(
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.NamespaceDomain> CreateNamespace(
            CreateNamespaceRequest request
        ) {
            return CreateNamespaceFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.Namespace> Namespaces(
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
            return this._cache.ListSubscribe<Gs2.Gs2Formation.Model.Namespace>(
                "formation:Namespace",
                callback
            );
        }

        public void UnsubscribeNamespaces(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Formation.Model.Namespace>(
                "formation:Namespace",
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Formation.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AddMoldCapacityByUserIdRequest, AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdComplete = new UnityEvent<string, AddMoldCapacityByUserIdRequest, AddMoldCapacityByUserIdResult>();
    #else
        public static Action<string, AddMoldCapacityByUserIdRequest, AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetMoldCapacityByUserIdRequest, SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdComplete = new UnityEvent<string, SetMoldCapacityByUserIdRequest, SetMoldCapacityByUserIdResult>();
    #else
        public static Action<string, SetMoldCapacityByUserIdRequest, SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AcquireActionsToFormPropertiesRequest, AcquireActionsToFormPropertiesResult> AcquireActionsToFormPropertiesComplete = new UnityEvent<string, AcquireActionsToFormPropertiesRequest, AcquireActionsToFormPropertiesResult>();
    #else
        public static Action<string, AcquireActionsToFormPropertiesRequest, AcquireActionsToFormPropertiesResult> AcquireActionsToFormPropertiesComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AcquireActionsToPropertyFormPropertiesRequest, AcquireActionsToPropertyFormPropertiesResult> AcquireActionsToPropertyFormPropertiesComplete = new UnityEvent<string, AcquireActionsToPropertyFormPropertiesRequest, AcquireActionsToPropertyFormPropertiesResult>();
    #else
        public static Action<string, AcquireActionsToPropertyFormPropertiesRequest, AcquireActionsToPropertyFormPropertiesResult> AcquireActionsToPropertyFormPropertiesComplete;
    #endif

        public static void UpdateCacheFromStampSheet(
                CacheDatabase cache,
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "AddMoldCapacityByUserId": {
                        var requestModel = AddMoldCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddMoldCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Mold"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.MoldModel != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "MoldModel"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                                resultModel.MoldModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.MoldModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        AddMoldCapacityByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetMoldCapacityByUserId": {
                        var requestModel = SetMoldCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetMoldCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Mold"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.MoldModel != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "MoldModel"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                                resultModel.MoldModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.MoldModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        SetMoldCapacityByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireActionsToFormProperties": {
                        var requestModel = AcquireActionsToFormPropertiesRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireActionsToFormPropertiesResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.MoldModelName,
                                "Form"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                                resultModel.Item.Index.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.Mold != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Mold"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                                resultModel.Mold.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Mold,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        AcquireActionsToFormPropertiesComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireActionsToPropertyFormProperties": {
                        var requestModel = AcquireActionsToPropertyFormPropertiesRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireActionsToPropertyFormPropertiesResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "PropertyForm"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString(),
                                requestModel.PropertyId.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        AcquireActionsToPropertyFormPropertiesComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SubMoldCapacityByUserIdRequest, SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdComplete = new UnityEvent<string, SubMoldCapacityByUserIdRequest, SubMoldCapacityByUserIdResult>();
    #else
        public static Action<string, SubMoldCapacityByUserIdRequest, SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampTask(
                CacheDatabase cache,
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "SubMoldCapacityByUserId": {
                        var requestModel = SubMoldCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SubMoldCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Mold"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.MoldModel != null) {
                            var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "MoldModel"
                            );
                            var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                                resultModel.MoldModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.MoldModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        SubMoldCapacityByUserIdComplete?.Invoke(
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
                case "add_mold_capacity_by_user_id": {
                    var requestModel = AddMoldCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddMoldCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.MoldModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "MoldModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                            resultModel.MoldModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.MoldModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    AddMoldCapacityByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_mold_capacity_by_user_id": {
                    var requestModel = SetMoldCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetMoldCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.MoldModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "MoldModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                            resultModel.MoldModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.MoldModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    SetMoldCapacityByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_actions_to_form_properties": {
                    var requestModel = AcquireActionsToFormPropertiesRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireActionsToFormPropertiesResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.MoldModelName,
                            "Form"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.FormDomain.CreateCacheKey(
                            resultModel.Item.Index.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Mold != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Mold.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Mold,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    AcquireActionsToFormPropertiesComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_actions_to_property_form_properties": {
                    var requestModel = AcquireActionsToPropertyFormPropertiesRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireActionsToPropertyFormPropertiesResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "PropertyForm"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString(),
                            requestModel.PropertyId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    AcquireActionsToPropertyFormPropertiesComplete?.Invoke(
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

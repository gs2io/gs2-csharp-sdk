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
using Gs2.Gs2Stamina.Domain.Iterator;
using Gs2.Gs2Stamina.Domain.Model;
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Stamina.Model;
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

namespace Gs2.Gs2Stamina.Domain
{

    public class Gs2Stamina {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2StaminaRestClient _client;

        private readonly String _parentKey;

        public Gs2Stamina(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2StaminaRestClient(
                session
            );
            this._parentKey = "stamina";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> CreateNamespace(
            #endif
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #endif
            CreateNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> self)
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
                        "stamina",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            var domain = new Gs2.Gs2Stamina.Domain.Model.NamespaceDomain(
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
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Stamina.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Stamina.Model.Namespace> Namespaces(
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

        public Gs2.Gs2Stamina.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, RecoverStaminaByUserIdRequest, RecoverStaminaByUserIdResult> RecoverStaminaByUserIdComplete = new UnityEvent<string, RecoverStaminaByUserIdRequest, RecoverStaminaByUserIdResult>();
    #else
        public static Action<string, RecoverStaminaByUserIdRequest, RecoverStaminaByUserIdResult> RecoverStaminaByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, RaiseMaxValueByUserIdRequest, RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdComplete = new UnityEvent<string, RaiseMaxValueByUserIdRequest, RaiseMaxValueByUserIdResult>();
    #else
        public static Action<string, RaiseMaxValueByUserIdRequest, RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetMaxValueByUserIdRequest, SetMaxValueByUserIdResult> SetMaxValueByUserIdComplete = new UnityEvent<string, SetMaxValueByUserIdRequest, SetMaxValueByUserIdResult>();
    #else
        public static Action<string, SetMaxValueByUserIdRequest, SetMaxValueByUserIdResult> SetMaxValueByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetRecoverIntervalByUserIdRequest, SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdComplete = new UnityEvent<string, SetRecoverIntervalByUserIdRequest, SetRecoverIntervalByUserIdResult>();
    #else
        public static Action<string, SetRecoverIntervalByUserIdRequest, SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetRecoverValueByUserIdRequest, SetRecoverValueByUserIdResult> SetRecoverValueByUserIdComplete = new UnityEvent<string, SetRecoverValueByUserIdRequest, SetRecoverValueByUserIdResult>();
    #else
        public static Action<string, SetRecoverValueByUserIdRequest, SetRecoverValueByUserIdResult> SetRecoverValueByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampSheet(
                CacheDatabase cache,
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "RecoverStaminaByUserId": {
                        var requestModel = RecoverStaminaByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = RecoverStaminaByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Stamina"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                resultModel.Item.StaminaName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.StaminaModel != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "StaminaModel"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                                resultModel.StaminaModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.StaminaModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        RecoverStaminaByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "RaiseMaxValueByUserId": {
                        var requestModel = RaiseMaxValueByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = RaiseMaxValueByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Stamina"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                resultModel.Item.StaminaName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.StaminaModel != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "StaminaModel"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                                resultModel.StaminaModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.StaminaModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        RaiseMaxValueByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetMaxValueByUserId": {
                        var requestModel = SetMaxValueByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetMaxValueByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Stamina"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                resultModel.Item.StaminaName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.StaminaModel != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "StaminaModel"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                                resultModel.StaminaModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.StaminaModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        SetMaxValueByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetRecoverIntervalByUserId": {
                        var requestModel = SetRecoverIntervalByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetRecoverIntervalByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Stamina"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                resultModel.Item.StaminaName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.StaminaModel != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "StaminaModel"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                                resultModel.StaminaModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.StaminaModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        SetRecoverIntervalByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetRecoverValueByUserId": {
                        var requestModel = SetRecoverValueByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetRecoverValueByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Stamina"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                resultModel.Item.StaminaName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.StaminaModel != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "StaminaModel"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                                resultModel.StaminaModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.StaminaModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        SetRecoverValueByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DecreaseMaxValueByUserIdRequest, DecreaseMaxValueByUserIdResult> DecreaseMaxValueByUserIdComplete = new UnityEvent<string, DecreaseMaxValueByUserIdRequest, DecreaseMaxValueByUserIdResult>();
    #else
        public static Action<string, DecreaseMaxValueByUserIdRequest, DecreaseMaxValueByUserIdResult> DecreaseMaxValueByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeStaminaByUserIdRequest, ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdComplete = new UnityEvent<string, ConsumeStaminaByUserIdRequest, ConsumeStaminaByUserIdResult>();
    #else
        public static Action<string, ConsumeStaminaByUserIdRequest, ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampTask(
                CacheDatabase cache,
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DecreaseMaxValueByUserId": {
                        var requestModel = DecreaseMaxValueByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DecreaseMaxValueByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Stamina"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                resultModel.Item.StaminaName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.StaminaModel != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "StaminaModel"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                                resultModel.StaminaModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.StaminaModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        DecreaseMaxValueByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ConsumeStaminaByUserId": {
                        var requestModel = ConsumeStaminaByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeStaminaByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Stamina"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                                resultModel.Item.StaminaName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.StaminaModel != null) {
                            var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "StaminaModel"
                            );
                            var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                                resultModel.StaminaModel.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.StaminaModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        ConsumeStaminaByUserIdComplete?.Invoke(
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
                case "recover_stamina_by_user_id": {
                    var requestModel = RecoverStaminaByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = RecoverStaminaByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    RecoverStaminaByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "raise_max_value_by_user_id": {
                    var requestModel = RaiseMaxValueByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = RaiseMaxValueByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    RaiseMaxValueByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_max_value_by_user_id": {
                    var requestModel = SetMaxValueByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetMaxValueByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    SetMaxValueByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_recover_interval_by_user_id": {
                    var requestModel = SetRecoverIntervalByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetRecoverIntervalByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    SetRecoverIntervalByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_recover_value_by_user_id": {
                    var requestModel = SetRecoverValueByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetRecoverValueByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Stamina"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                            resultModel.Item.StaminaName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.StaminaModel != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "StaminaModel"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.StaminaModelDomain.CreateCacheKey(
                            resultModel.StaminaModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.StaminaModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    SetRecoverValueByUserIdComplete?.Invoke(
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

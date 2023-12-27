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
#pragma warning disable CS0414 // Field is assigned but its value is never used

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Enhance.Domain.Iterator;
using Gs2.Gs2Enhance.Domain.Model;
using Gs2.Gs2Enhance.Request;
using Gs2.Gs2Enhance.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Enhance.Model;
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

namespace Gs2.Gs2Enhance.Domain
{

    public class Gs2Enhance {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnhanceRestClient _client;

        private readonly String _parentKey;
        public string Url { get; set; }
        public string UploadToken { get; set; }
        public string UploadUrl { get; set; }

        public Gs2Enhance(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnhanceRestClient(
                gs2.RestSession
            );
            this._parentKey = "enhance";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> self)
            {
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "enhance",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Enhance.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            CreateNamespaceResult result = null;
                result = await this._client.CreateNamespaceAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "enhance",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Enhance.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateNamespaceFuture.")]
        public IFuture<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> CreateNamespace(
            CreateNamespaceRequest request
        ) {
            return CreateNamespaceFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enhance> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Enhance> self)
            {
                var future = this._client.DumpUserDataByUserIdFuture(
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
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Enhance>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enhance> DumpUserDataAsync(
            #else
        public async Task<Gs2Enhance> DumpUserDataAsync(
            #endif
            DumpUserDataByUserIdRequest request
        ) {
            DumpUserDataByUserIdResult result = null;
                result = await this._client.DumpUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DumpUserDataFuture.")]
        public IFuture<Gs2Enhance> DumpUserData(
            DumpUserDataByUserIdRequest request
        ) {
            return DumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enhance> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Enhance> self)
            {
                var future = this._client.CheckDumpUserDataByUserIdFuture(
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
                this.Url = domain.Url = result?.Url;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Enhance>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enhance> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Enhance> CheckDumpUserDataAsync(
            #endif
            CheckDumpUserDataByUserIdRequest request
        ) {
            CheckDumpUserDataByUserIdResult result = null;
                result = await this._client.CheckDumpUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CheckDumpUserDataFuture.")]
        public IFuture<Gs2Enhance> CheckDumpUserData(
            CheckDumpUserDataByUserIdRequest request
        ) {
            return CheckDumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enhance> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Enhance> self)
            {
                var future = this._client.CleanUserDataByUserIdFuture(
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
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Enhance>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enhance> CleanUserDataAsync(
            #else
        public async Task<Gs2Enhance> CleanUserDataAsync(
            #endif
            CleanUserDataByUserIdRequest request
        ) {
            CleanUserDataByUserIdResult result = null;
                result = await this._client.CleanUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CleanUserDataFuture.")]
        public IFuture<Gs2Enhance> CleanUserData(
            CleanUserDataByUserIdRequest request
        ) {
            return CleanUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enhance> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Enhance> self)
            {
                var future = this._client.CheckCleanUserDataByUserIdFuture(
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
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Enhance>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enhance> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Enhance> CheckCleanUserDataAsync(
            #endif
            CheckCleanUserDataByUserIdRequest request
        ) {
            CheckCleanUserDataByUserIdResult result = null;
                result = await this._client.CheckCleanUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CheckCleanUserDataFuture.")]
        public IFuture<Gs2Enhance> CheckCleanUserData(
            CheckCleanUserDataByUserIdRequest request
        ) {
            return CheckCleanUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enhance> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Enhance> self)
            {
                var future = this._client.PrepareImportUserDataByUserIdFuture(
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
                this.UploadToken = domain.UploadToken = result?.UploadToken;
                this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Enhance>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enhance> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Enhance> PrepareImportUserDataAsync(
            #endif
            PrepareImportUserDataByUserIdRequest request
        ) {
            PrepareImportUserDataByUserIdResult result = null;
                result = await this._client.PrepareImportUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to PrepareImportUserDataFuture.")]
        public IFuture<Gs2Enhance> PrepareImportUserData(
            PrepareImportUserDataByUserIdRequest request
        ) {
            return PrepareImportUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enhance> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Enhance> self)
            {
                var future = this._client.ImportUserDataByUserIdFuture(
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
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Enhance>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enhance> ImportUserDataAsync(
            #else
        public async Task<Gs2Enhance> ImportUserDataAsync(
            #endif
            ImportUserDataByUserIdRequest request
        ) {
            ImportUserDataByUserIdResult result = null;
                result = await this._client.ImportUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to ImportUserDataFuture.")]
        public IFuture<Gs2Enhance> ImportUserData(
            ImportUserDataByUserIdRequest request
        ) {
            return ImportUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enhance> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Enhance> self)
            {
                var future = this._client.CheckImportUserDataByUserIdFuture(
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
                this.Url = domain.Url = result?.Url;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Enhance>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enhance> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Enhance> CheckImportUserDataAsync(
            #endif
            CheckImportUserDataByUserIdRequest request
        ) {
            CheckImportUserDataByUserIdResult result = null;
                result = await this._client.CheckImportUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CheckImportUserDataFuture.")]
        public IFuture<Gs2Enhance> CheckImportUserData(
            CheckImportUserDataByUserIdRequest request
        ) {
            return CheckImportUserDataFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Enhance.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2.Cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Enhance.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Enhance.Model.Namespace> Namespaces(
            #endif
        #else
        public DescribeNamespacesIterator NamespacesAsync(
        #endif
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2.Cache,
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
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enhance.Model.Namespace>(
                "enhance:Namespace",
                callback
            );
        }

        public void UnsubscribeNamespaces(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enhance.Model.Namespace>(
                "enhance:Namespace",
                callbackId
            );
        }

        public Gs2.Gs2Enhance.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Enhance.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DirectEnhanceByUserIdRequest, DirectEnhanceByUserIdResult> DirectEnhanceByUserIdComplete = new UnityEvent<string, DirectEnhanceByUserIdRequest, DirectEnhanceByUserIdResult>();
    #else
        public static Action<string, DirectEnhanceByUserIdRequest, DirectEnhanceByUserIdResult> DirectEnhanceByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, UnleashByUserIdRequest, UnleashByUserIdResult> UnleashByUserIdComplete = new UnityEvent<string, UnleashByUserIdRequest, UnleashByUserIdResult>();
    #else
        public static Action<string, UnleashByUserIdRequest, UnleashByUserIdResult> UnleashByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, CreateProgressByUserIdRequest, CreateProgressByUserIdResult> CreateProgressByUserIdComplete = new UnityEvent<string, CreateProgressByUserIdRequest, CreateProgressByUserIdResult>();
    #else
        public static Action<string, CreateProgressByUserIdRequest, CreateProgressByUserIdResult> CreateProgressByUserIdComplete;
    #endif

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DirectEnhanceByUserId": {
                        var requestModel = DirectEnhanceByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DirectEnhanceByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "RateModel"
                            );
                            var key = Gs2.Gs2Enhance.Domain.Model.RateModelDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        DirectEnhanceByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "UnleashByUserId": {
                        var requestModel = UnleashByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = UnleashByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                "UnleashRateModel"
                            );
                            var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        UnleashByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "CreateProgressByUserId": {
                        var requestModel = CreateProgressByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = CreateProgressByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Enhance.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Progress"
                            );
                            var key = Gs2.Gs2Enhance.Domain.Model.ProgressDomain.CreateCacheKey(
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        CreateProgressByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DeleteProgressByUserIdRequest, DeleteProgressByUserIdResult> DeleteProgressByUserIdComplete = new UnityEvent<string, DeleteProgressByUserIdRequest, DeleteProgressByUserIdResult>();
    #else
        public static Action<string, DeleteProgressByUserIdRequest, DeleteProgressByUserIdResult> DeleteProgressByUserIdComplete;
    #endif

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DeleteProgressByUserId": {
                        var requestModel = DeleteProgressByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DeleteProgressByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Enhance.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Progress"
                            );
                            var key = Gs2.Gs2Enhance.Domain.Model.ProgressDomain.CreateCacheKey(
                            );
                            _gs2.Cache.Delete<Gs2.Gs2Enhance.Model.Progress>(parentKey, key);
                        }

                        DeleteProgressByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

        public void UpdateCacheFromJobResult(
                string method,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
            switch (method) {
                case "direct_enhance_by_user_id": {
                    var requestModel = DirectEnhanceByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = DirectEnhanceByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "RateModel"
                        );
                        var key = Gs2.Gs2Enhance.Domain.Model.RateModelDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    DirectEnhanceByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "unleash_by_user_id": {
                    var requestModel = UnleashByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = UnleashByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            "UnleashRateModel"
                        );
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    UnleashByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "create_progress_by_user_id": {
                    var requestModel = CreateProgressByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = CreateProgressByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Enhance.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Progress"
                        );
                        var key = Gs2.Gs2Enhance.Domain.Model.ProgressDomain.CreateCacheKey(
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    CreateProgressByUserIdComplete?.Invoke(
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

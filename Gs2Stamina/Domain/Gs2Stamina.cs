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
using Cysharp.Threading.Tasks.Linq;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Stamina.Domain
{

    public class Gs2Stamina {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2StaminaRestClient _client;

        private readonly String _parentKey;
        public string Url { get; set; }
        public string UploadToken { get; set; }
        public string UploadUrl { get; set; }

        public Gs2Stamina(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2StaminaRestClient(
                gs2.RestSession
            );
            this._parentKey = "stamina";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> self)
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
                            "stamina",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Stamina.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
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
                        "stamina",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Stamina.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateNamespaceFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.NamespaceDomain> CreateNamespace(
            CreateNamespaceRequest request
        ) {
            return CreateNamespaceFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Stamina> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Stamina> self)
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
            return new Gs2InlineFuture<Gs2Stamina>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Stamina> DumpUserDataAsync(
            #else
        public async Task<Gs2Stamina> DumpUserDataAsync(
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
        public IFuture<Gs2Stamina> DumpUserData(
            DumpUserDataByUserIdRequest request
        ) {
            return DumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Stamina> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Stamina> self)
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
            return new Gs2InlineFuture<Gs2Stamina>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Stamina> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Stamina> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Stamina> CheckDumpUserData(
            CheckDumpUserDataByUserIdRequest request
        ) {
            return CheckDumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Stamina> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Stamina> self)
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
            return new Gs2InlineFuture<Gs2Stamina>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Stamina> CleanUserDataAsync(
            #else
        public async Task<Gs2Stamina> CleanUserDataAsync(
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
        public IFuture<Gs2Stamina> CleanUserData(
            CleanUserDataByUserIdRequest request
        ) {
            return CleanUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Stamina> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Stamina> self)
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
            return new Gs2InlineFuture<Gs2Stamina>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Stamina> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Stamina> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Stamina> CheckCleanUserData(
            CheckCleanUserDataByUserIdRequest request
        ) {
            return CheckCleanUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Stamina> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Stamina> self)
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
            return new Gs2InlineFuture<Gs2Stamina>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Stamina> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Stamina> PrepareImportUserDataAsync(
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
        public IFuture<Gs2Stamina> PrepareImportUserData(
            PrepareImportUserDataByUserIdRequest request
        ) {
            return PrepareImportUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Stamina> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Stamina> self)
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
            return new Gs2InlineFuture<Gs2Stamina>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Stamina> ImportUserDataAsync(
            #else
        public async Task<Gs2Stamina> ImportUserDataAsync(
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
        public IFuture<Gs2Stamina> ImportUserData(
            ImportUserDataByUserIdRequest request
        ) {
            return ImportUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Stamina> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Stamina> self)
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
            return new Gs2InlineFuture<Gs2Stamina>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Stamina> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Stamina> CheckImportUserDataAsync(
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
        public IFuture<Gs2Stamina> CheckImportUserData(
            CheckImportUserDataByUserIdRequest request
        ) {
            return CheckImportUserDataFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Stamina.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2.Cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Stamina.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Stamina.Model.Namespace> Namespaces(
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

        public ulong SubscribeNamespaces(
            Action<Gs2.Gs2Stamina.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Stamina.Model.Namespace>(
                "stamina:Namespace",
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2Stamina.Model.Namespace[]> callback
        )
        {
            var items = await NamespacesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeNamespaces(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeNamespaces(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Stamina.Model.Namespace>(
                "stamina:Namespace",
                callbackId
            );
        }

        public Gs2.Gs2Stamina.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Stamina.Domain.Model.NamespaceDomain(
                this._gs2,
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

        public void UpdateCacheFromStampSheet(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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

        public void UpdateCacheFromStampTask(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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
                            _gs2.Cache.Put(
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

        public void UpdateCacheFromJobResult(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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
                        _gs2.Cache.Put(
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

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
#pragma warning disable CS0414 // Field is assigned but its value is never used

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Inventory.Domain.Iterator;
using Gs2.Gs2Inventory.Domain.Model;
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Inventory.Model;
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

namespace Gs2.Gs2Inventory.Domain
{

    public class Gs2Inventory {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;

        private readonly String _parentKey;
        public string Url { get; set; }
        public string UploadToken { get; set; }
        public string UploadUrl { get; set; }

        public Gs2Inventory(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this._parentKey = "inventory";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> self)
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "inventory",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Inventory.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            CreateNamespaceResult result = null;
                result = await this._client.CreateNamespaceAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "inventory",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Inventory.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateNamespaceFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> CreateNamespace(
            CreateNamespaceRequest request
        ) {
            return CreateNamespaceFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inventory> self)
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Inventory>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Inventory> DumpUserDataAsync(
            #else
        public async Task<Gs2Inventory> DumpUserDataAsync(
            #endif
            DumpUserDataByUserIdRequest request
        ) {
            DumpUserDataByUserIdResult result = null;
                result = await this._client.DumpUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DumpUserDataFuture.")]
        public IFuture<Gs2Inventory> DumpUserData(
            DumpUserDataByUserIdRequest request
        ) {
            return DumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inventory> self)
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Url = domain.Url = result?.Url;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Inventory>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Inventory> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Inventory> CheckDumpUserDataAsync(
            #endif
            CheckDumpUserDataByUserIdRequest request
        ) {
            CheckDumpUserDataByUserIdResult result = null;
                result = await this._client.CheckDumpUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CheckDumpUserDataFuture.")]
        public IFuture<Gs2Inventory> CheckDumpUserData(
            CheckDumpUserDataByUserIdRequest request
        ) {
            return CheckDumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inventory> self)
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Inventory>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Inventory> CleanUserDataAsync(
            #else
        public async Task<Gs2Inventory> CleanUserDataAsync(
            #endif
            CleanUserDataByUserIdRequest request
        ) {
            CleanUserDataByUserIdResult result = null;
                result = await this._client.CleanUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CleanUserDataFuture.")]
        public IFuture<Gs2Inventory> CleanUserData(
            CleanUserDataByUserIdRequest request
        ) {
            return CleanUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inventory> self)
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Inventory>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Inventory> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Inventory> CheckCleanUserDataAsync(
            #endif
            CheckCleanUserDataByUserIdRequest request
        ) {
            CheckCleanUserDataByUserIdResult result = null;
                result = await this._client.CheckCleanUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CheckCleanUserDataFuture.")]
        public IFuture<Gs2Inventory> CheckCleanUserData(
            CheckCleanUserDataByUserIdRequest request
        ) {
            return CheckCleanUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inventory> self)
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.UploadToken = domain.UploadToken = result?.UploadToken;
                this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Inventory>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Inventory> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Inventory> PrepareImportUserDataAsync(
            #endif
            PrepareImportUserDataByUserIdRequest request
        ) {
            PrepareImportUserDataByUserIdResult result = null;
                result = await this._client.PrepareImportUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
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
        public IFuture<Gs2Inventory> PrepareImportUserData(
            PrepareImportUserDataByUserIdRequest request
        ) {
            return PrepareImportUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inventory> self)
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Inventory>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Inventory> ImportUserDataAsync(
            #else
        public async Task<Gs2Inventory> ImportUserDataAsync(
            #endif
            ImportUserDataByUserIdRequest request
        ) {
            ImportUserDataByUserIdResult result = null;
                result = await this._client.ImportUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to ImportUserDataFuture.")]
        public IFuture<Gs2Inventory> ImportUserData(
            ImportUserDataByUserIdRequest request
        ) {
            return ImportUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inventory> self)
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Url = domain.Url = result?.Url;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2Inventory>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Inventory> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Inventory> CheckImportUserDataAsync(
            #endif
            CheckImportUserDataByUserIdRequest request
        ) {
            CheckImportUserDataByUserIdResult result = null;
                result = await this._client.CheckImportUserDataByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CheckImportUserDataFuture.")]
        public IFuture<Gs2Inventory> CheckImportUserData(
            CheckImportUserDataByUserIdRequest request
        ) {
            return CheckImportUserDataFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2.Cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.Namespace> Namespaces(
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
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.Namespace>(
                "inventory:Namespace",
                callback
            );
        }

        public void UnsubscribeNamespaces(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.Namespace>(
                "inventory:Namespace",
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AddCapacityByUserIdRequest, AddCapacityByUserIdResult> AddCapacityByUserIdComplete = new UnityEvent<string, AddCapacityByUserIdRequest, AddCapacityByUserIdResult>();
    #else
        public static Action<string, AddCapacityByUserIdRequest, AddCapacityByUserIdResult> AddCapacityByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetCapacityByUserIdRequest, SetCapacityByUserIdResult> SetCapacityByUserIdComplete = new UnityEvent<string, SetCapacityByUserIdRequest, SetCapacityByUserIdResult>();
    #else
        public static Action<string, SetCapacityByUserIdRequest, SetCapacityByUserIdResult> SetCapacityByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AcquireItemSetByUserIdRequest, AcquireItemSetByUserIdResult> AcquireItemSetByUserIdComplete = new UnityEvent<string, AcquireItemSetByUserIdRequest, AcquireItemSetByUserIdResult>();
    #else
        public static Action<string, AcquireItemSetByUserIdRequest, AcquireItemSetByUserIdResult> AcquireItemSetByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AddReferenceOfByUserIdRequest, AddReferenceOfByUserIdResult> AddReferenceOfByUserIdComplete = new UnityEvent<string, AddReferenceOfByUserIdRequest, AddReferenceOfByUserIdResult>();
    #else
        public static Action<string, AddReferenceOfByUserIdRequest, AddReferenceOfByUserIdResult> AddReferenceOfByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DeleteReferenceOfByUserIdRequest, DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdComplete = new UnityEvent<string, DeleteReferenceOfByUserIdRequest, DeleteReferenceOfByUserIdResult>();
    #else
        public static Action<string, DeleteReferenceOfByUserIdRequest, DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AcquireSimpleItemsByUserIdRequest, AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdComplete = new UnityEvent<string, AcquireSimpleItemsByUserIdRequest, AcquireSimpleItemsByUserIdResult>();
    #else
        public static Action<string, AcquireSimpleItemsByUserIdRequest, AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AcquireBigItemByUserIdRequest, AcquireBigItemByUserIdResult> AcquireBigItemByUserIdComplete = new UnityEvent<string, AcquireBigItemByUserIdRequest, AcquireBigItemByUserIdResult>();
    #else
        public static Action<string, AcquireBigItemByUserIdRequest, AcquireBigItemByUserIdResult> AcquireBigItemByUserIdComplete;
    #endif

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "AddCapacityByUserId": {
                        var requestModel = AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Inventory"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                resultModel.Item.InventoryName.ToString()
                            );
                            var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                                parentKey,
                                key
                            );
                            if (item == null || item.Revision < resultModel.Item.Revision)
                            {
                                _gs2.Cache.Put(
                                    parentKey,
                                    key,
                                    resultModel.Item,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }

                        AddCapacityByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetCapacityByUserId": {
                        var requestModel = SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Inventory"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                resultModel.Item.InventoryName.ToString()
                            );
                            var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                                parentKey,
                                key
                            );
                            if (item == null || item.Revision < resultModel.Item.Revision)
                            {
                                _gs2.Cache.Put(
                                    parentKey,
                                    key,
                                    resultModel.Item,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }

                        SetCapacityByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireItemSetByUserId": {
                        var requestModel = AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireItemSetByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet"
                            );
                            var nullParentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet:Null"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    item.ItemName.ToString(),
                                    item.Name.ToString()
                                );
                                if (item.Count == 0) {
                                    _gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                        parentKey,
                                        key,
                                        null,
                                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                    );
                                }
                                else
                                {
                                    _gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                        parentKey,
                                        key,
                                        item,
                                        item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                    );
                                }
                            }
                            if (resultModel.Items.Length == 0) {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    requestModel.ItemName,
                                    null
                                );
                                _gs2.Cache.Put(
                                    nullParentKey,
                                    key,
                                    resultModel.Items,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                            var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                            {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    group.Key,
                                    null
                                );
                                var items = group.ToArray();
                                _gs2.Cache.Put(
                                    nullParentKey,
                                    key,
                                    items,
                                    items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                                );
                                return items;
                            }).ToArray();
                        }
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
                                "ItemModel"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                                resultModel.ItemModel.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.ItemModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.Inventory != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Inventory"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                resultModel.Inventory.InventoryName.ToString()
                            );
                            var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                                parentKey,
                                key
                            );
                            if (item == null || item.Revision < resultModel.Inventory.Revision)
                            {
                                _gs2.Cache.Put(
                                    parentKey,
                                    key,
                                    resultModel.Inventory,
                                    resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                                );
                            }
                        }

                        AcquireItemSetByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AddReferenceOfByUserId": {
                        var requestModel = AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.ItemSet != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                resultModel.ItemSet.ItemName.ToString(),
                                resultModel.ItemSet.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.ItemSet,
                                resultModel.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
                                "ItemModel"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                                resultModel.ItemModel.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.ItemModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        AddReferenceOfByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "DeleteReferenceOfByUserId": {
                        var requestModel = DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DeleteReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.ItemSet != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                resultModel.ItemSet.ItemName.ToString(),
                                resultModel.ItemSet.Name.ToString()
                            );
                            _gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
                                "ItemModel"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                                resultModel.ItemModel.Name.ToString()
                            );
                            _gs2.Cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
                        }
                        if (resultModel.Inventory != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Inventory"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                resultModel.Inventory.InventoryName.ToString()
                            );
                            _gs2.Cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                        }

                        DeleteReferenceOfByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireSimpleItemsByUserId": {
                        var requestModel = AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "SimpleItem"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                    item.ItemName.ToString()
                                );
                                _gs2.Cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }

                        AcquireSimpleItemsByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireBigItemByUserId": {
                        var requestModel = AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "BigItem"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                                resultModel.Item.ItemName.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        AcquireBigItemByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, VerifyInventoryCurrentMaxCapacityByUserIdRequest, VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdComplete = new UnityEvent<string, VerifyInventoryCurrentMaxCapacityByUserIdRequest, VerifyInventoryCurrentMaxCapacityByUserIdResult>();
    #else
        public static Action<string, VerifyInventoryCurrentMaxCapacityByUserIdRequest, VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdComplete = new UnityEvent<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult>();
    #else
        public static Action<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, VerifyItemSetByUserIdRequest, VerifyItemSetByUserIdResult> VerifyItemSetByUserIdComplete = new UnityEvent<string, VerifyItemSetByUserIdRequest, VerifyItemSetByUserIdResult>();
    #else
        public static Action<string, VerifyItemSetByUserIdRequest, VerifyItemSetByUserIdResult> VerifyItemSetByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, VerifyReferenceOfByUserIdRequest, VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdComplete = new UnityEvent<string, VerifyReferenceOfByUserIdRequest, VerifyReferenceOfByUserIdResult>();
    #else
        public static Action<string, VerifyReferenceOfByUserIdRequest, VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdComplete = new UnityEvent<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult>();
    #else
        public static Action<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, VerifySimpleItemByUserIdRequest, VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdComplete = new UnityEvent<string, VerifySimpleItemByUserIdRequest, VerifySimpleItemByUserIdResult>();
    #else
        public static Action<string, VerifySimpleItemByUserIdRequest, VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeBigItemByUserIdRequest, ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdComplete = new UnityEvent<string, ConsumeBigItemByUserIdRequest, ConsumeBigItemByUserIdResult>();
    #else
        public static Action<string, ConsumeBigItemByUserIdRequest, ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, VerifyBigItemByUserIdRequest, VerifyBigItemByUserIdResult> VerifyBigItemByUserIdComplete = new UnityEvent<string, VerifyBigItemByUserIdRequest, VerifyBigItemByUserIdResult>();
    #else
        public static Action<string, VerifyBigItemByUserIdRequest, VerifyBigItemByUserIdResult> VerifyBigItemByUserIdComplete;
    #endif

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "VerifyInventoryCurrentMaxCapacityByUserId": {
                        var requestModel = VerifyInventoryCurrentMaxCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = VerifyInventoryCurrentMaxCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        

                        VerifyInventoryCurrentMaxCapacityByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ConsumeItemSetByUserId": {
                        var requestModel = ConsumeItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeItemSetByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet"
                            );
                            var nullParentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet:Null"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    item.ItemName.ToString(),
                                    item.Name.ToString()
                                );
                                if (item.Count == 0) {
                                    _gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                        parentKey,
                                        key,
                                        null,
                                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                    );
                                }
                                else
                                {
                                    _gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                        parentKey,
                                        key,
                                        item,
                                        item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                    );
                                }
                            }
                            if (resultModel.Items.Length == 0) {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    requestModel.ItemName,
                                    null
                                );
                                _gs2.Cache.Put(
                                    nullParentKey,
                                    key,
                                    resultModel.Items,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                            var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                            {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    group.Key,
                                    null
                                );
                                var items = group.ToArray();
                                _gs2.Cache.Put(
                                    nullParentKey,
                                    key,
                                    items,
                                    items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                                );
                                return items;
                            }).ToArray();
                        }
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
                                "ItemModel"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                                resultModel.ItemModel.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.ItemModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.Inventory != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Inventory"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                resultModel.Inventory.InventoryName.ToString()
                            );
                            var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                                parentKey,
                                key
                            );
                            if (item == null || item.Revision < resultModel.Inventory.Revision)
                            {
                                _gs2.Cache.Put(
                                    parentKey,
                                    key,
                                    resultModel.Inventory,
                                    resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                                );
                            }
                        }

                        ConsumeItemSetByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "VerifyItemSetByUserId": {
                        var requestModel = VerifyItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = VerifyItemSetByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        

                        VerifyItemSetByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "VerifyReferenceOfByUserId": {
                        var requestModel = VerifyReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = VerifyReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.ItemSet != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                resultModel.ItemSet.ItemName.ToString(),
                                resultModel.ItemSet.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.ItemSet,
                                resultModel.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
                                "ItemModel"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                                resultModel.ItemModel.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.ItemModel,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        VerifyReferenceOfByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ConsumeSimpleItemsByUserId": {
                        var requestModel = ConsumeSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "SimpleItem"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                    item.ItemName.ToString()
                                );
                                _gs2.Cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }

                        ConsumeSimpleItemsByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "VerifySimpleItemByUserId": {
                        var requestModel = VerifySimpleItemByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = VerifySimpleItemByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        

                        VerifySimpleItemByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ConsumeBigItemByUserId": {
                        var requestModel = ConsumeBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "BigItem"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                                resultModel.Item.ItemName.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        ConsumeBigItemByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "VerifyBigItemByUserId": {
                        var requestModel = VerifyBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = VerifyBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        

                        VerifyBigItemByUserIdComplete?.Invoke(
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
                case "add_capacity_by_user_id": {
                    var requestModel = AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Item.InventoryName.ToString()
                        );
                        var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Item.Revision)
                        {
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }

                    AddCapacityByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_capacity_by_user_id": {
                    var requestModel = SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Item.InventoryName.ToString()
                        );
                        var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Item.Revision)
                        {
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }

                    SetCapacityByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_item_set_by_user_id": {
                    var requestModel = AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireItemSetByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "ItemSet"
                        );
                        var nullParentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "ItemSet:Null"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                item.ItemName.ToString(),
                                item.Name.ToString()
                            );
                            if (item.Count == 0) {
                                _gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key,
                                    null,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                            else
                            {
                                _gs2.Cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }
                        if (resultModel.Items.Length == 0) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                requestModel.ItemName,
                                null
                            );
                            _gs2.Cache.Put(
                                nullParentKey,
                                key,
                                resultModel.Items,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            _gs2.Cache.Put(
                                nullParentKey,
                                key,
                                items,
                                items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                            return items;
                        }).ToArray();
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Inventory.Revision)
                        {
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Inventory,
                                resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                        }
                    }

                    AcquireItemSetByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "add_reference_of_by_user_id": {
                    var requestModel = AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.ItemSet != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "ItemSet"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            resultModel.ItemSet.ItemName.ToString(),
                            resultModel.ItemSet.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemSet,
                            resultModel.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    AddReferenceOfByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "delete_reference_of_by_user_id": {
                    var requestModel = DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = DeleteReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.ItemSet != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "ItemSet"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            resultModel.ItemSet.ItemName.ToString(),
                            resultModel.ItemSet.Name.ToString()
                        );
                        _gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
                    }
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                    }

                    DeleteReferenceOfByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_simple_items_by_user_id": {
                    var requestModel = AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "SimpleItem"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                item.ItemName.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }

                    AcquireSimpleItemsByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_big_item_by_user_id": {
                    var requestModel = AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "BigItem"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                            resultModel.Item.ItemName.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    AcquireBigItemByUserIdComplete?.Invoke(
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

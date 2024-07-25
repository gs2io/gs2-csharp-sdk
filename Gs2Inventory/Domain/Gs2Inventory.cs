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
using Gs2.Gs2Inventory.Domain.Iterator;
using Gs2.Gs2Inventory.Model.Cache;
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
using Cysharp.Threading.Tasks.Linq;
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
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2Inventory(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateNamespaceFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Inventory> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DumpUserDataByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.DumpUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Inventory> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CheckDumpUserDataByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CheckDumpUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Inventory> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CleanUserDataByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CleanUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Inventory> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CheckCleanUserDataByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CheckCleanUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Inventory> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.PrepareImportUserDataByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.PrepareImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Inventory> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.ImportUserDataByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.ImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inventory> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Inventory> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CheckImportUserDataByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CheckImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.Namespace> NamespacesAsync(
            #else
        public DescribeNamespacesIterator NamespacesAsync(
            #endif
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeNamespaces(
            Action<Gs2.Gs2Inventory.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.Namespace>(
                (null as Gs2.Gs2Inventory.Model.Namespace).CacheParentKey(
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.Namespace[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.Namespace>(
                (null as Gs2.Gs2Inventory.Model.Namespace).CacheParentKey(
                ),
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
        public static UnityEvent<string, AcquireItemSetWithGradeByUserIdRequest, AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdComplete = new UnityEvent<string, AcquireItemSetWithGradeByUserIdRequest, AcquireItemSetWithGradeByUserIdResult>();
    #else
        public static Action<string, AcquireItemSetWithGradeByUserIdRequest, AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdComplete;
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
        public static UnityEvent<string, SetSimpleItemsByUserIdRequest, SetSimpleItemsByUserIdResult> SetSimpleItemsByUserIdComplete = new UnityEvent<string, SetSimpleItemsByUserIdRequest, SetSimpleItemsByUserIdResult>();
    #else
        public static Action<string, SetSimpleItemsByUserIdRequest, SetSimpleItemsByUserIdResult> SetSimpleItemsByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AcquireBigItemByUserIdRequest, AcquireBigItemByUserIdResult> AcquireBigItemByUserIdComplete = new UnityEvent<string, AcquireBigItemByUserIdRequest, AcquireBigItemByUserIdResult>();
    #else
        public static Action<string, AcquireBigItemByUserIdRequest, AcquireBigItemByUserIdResult> AcquireBigItemByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetBigItemByUserIdRequest, SetBigItemByUserIdResult> SetBigItemByUserIdComplete = new UnityEvent<string, SetBigItemByUserIdRequest, SetBigItemByUserIdResult>();
    #else
        public static Action<string, SetBigItemByUserIdRequest, SetBigItemByUserIdResult> SetBigItemByUserIdComplete;
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

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

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

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

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

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        AcquireItemSetByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireItemSetWithGradeByUserId": {
                        var requestModel = AcquireItemSetWithGradeByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireItemSetWithGradeByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        AcquireItemSetWithGradeByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AddReferenceOfByUserId": {
                        var requestModel = AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

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

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

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

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        AcquireSimpleItemsByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetSimpleItemsByUserId": {
                        var requestModel = SetSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        SetSimpleItemsByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireBigItemByUserId": {
                        var requestModel = AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        AcquireBigItemByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetBigItemByUserId": {
                        var requestModel = SetBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        SetBigItemByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdComplete = new UnityEvent<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult>();
    #else
        public static Action<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdComplete = new UnityEvent<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult>();
    #else
        public static Action<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeBigItemByUserIdRequest, ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdComplete = new UnityEvent<string, ConsumeBigItemByUserIdRequest, ConsumeBigItemByUserIdResult>();
    #else
        public static Action<string, ConsumeBigItemByUserIdRequest, ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdComplete;
    #endif

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "ConsumeItemSetByUserId": {
                        var requestModel = ConsumeItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeItemSetByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        ConsumeItemSetByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ConsumeSimpleItemsByUserId": {
                        var requestModel = ConsumeSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        ConsumeSimpleItemsByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ConsumeBigItemByUserId": {
                        var requestModel = ConsumeBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        ConsumeBigItemByUserIdComplete?.Invoke(
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

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

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

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

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

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    AcquireItemSetByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_item_set_with_grade_by_user_id": {
                    var requestModel = AcquireItemSetWithGradeByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireItemSetWithGradeByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    AcquireItemSetWithGradeByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "add_reference_of_by_user_id": {
                    var requestModel = AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

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

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

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

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    AcquireSimpleItemsByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_simple_items_by_user_id": {
                    var requestModel = SetSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    SetSimpleItemsByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_big_item_by_user_id": {
                    var requestModel = AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    AcquireBigItemByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_big_item_by_user_id": {
                    var requestModel = SetBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetBigItemByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    SetBigItemByUserIdComplete?.Invoke(
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
        }
    }
}

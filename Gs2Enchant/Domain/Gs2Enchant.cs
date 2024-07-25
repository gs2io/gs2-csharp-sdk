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
using Gs2.Gs2Enchant.Domain.Iterator;
using Gs2.Gs2Enchant.Model.Cache;
using Gs2.Gs2Enchant.Domain.Model;
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Enchant.Model;
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

namespace Gs2.Gs2Enchant.Domain
{

    public class Gs2Enchant {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnchantRestClient _client;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2Enchant(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnchantRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> self)
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
                var domain = new Gs2.Gs2Enchant.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Enchant.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enchant> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Enchant> self)
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
            return new Gs2InlineFuture<Gs2Enchant>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enchant> DumpUserDataAsync(
            #else
        public async Task<Gs2Enchant> DumpUserDataAsync(
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
        public IFuture<Gs2Enchant> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Enchant> self)
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
            return new Gs2InlineFuture<Gs2Enchant>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enchant> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Enchant> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Enchant> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Enchant> self)
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
            return new Gs2InlineFuture<Gs2Enchant>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enchant> CleanUserDataAsync(
            #else
        public async Task<Gs2Enchant> CleanUserDataAsync(
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
        public IFuture<Gs2Enchant> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Enchant> self)
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
            return new Gs2InlineFuture<Gs2Enchant>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enchant> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Enchant> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Enchant> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Enchant> self)
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
            return new Gs2InlineFuture<Gs2Enchant>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enchant> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Enchant> PrepareImportUserDataAsync(
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
        public IFuture<Gs2Enchant> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Enchant> self)
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
            return new Gs2InlineFuture<Gs2Enchant>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enchant> ImportUserDataAsync(
            #else
        public async Task<Gs2Enchant> ImportUserDataAsync(
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
        public IFuture<Gs2Enchant> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Enchant> self)
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
            return new Gs2InlineFuture<Gs2Enchant>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Enchant> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Enchant> CheckImportUserDataAsync(
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
        public Gs2Iterator<Gs2.Gs2Enchant.Model.Namespace> Namespaces(
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
        public IUniTaskAsyncEnumerable<Gs2.Gs2Enchant.Model.Namespace> NamespacesAsync(
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
            Action<Gs2.Gs2Enchant.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enchant.Model.Namespace>(
                (null as Gs2.Gs2Enchant.Model.Namespace).CacheParentKey(
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2Enchant.Model.Namespace[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enchant.Model.Namespace>(
                (null as Gs2.Gs2Enchant.Model.Namespace).CacheParentKey(
                ),
                callbackId
            );
        }

        public Gs2.Gs2Enchant.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ReDrawBalanceParameterStatusByUserIdRequest, ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdComplete = new UnityEvent<string, ReDrawBalanceParameterStatusByUserIdRequest, ReDrawBalanceParameterStatusByUserIdResult>();
    #else
        public static Action<string, ReDrawBalanceParameterStatusByUserIdRequest, ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetBalanceParameterStatusByUserIdRequest, SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdComplete = new UnityEvent<string, SetBalanceParameterStatusByUserIdRequest, SetBalanceParameterStatusByUserIdResult>();
    #else
        public static Action<string, SetBalanceParameterStatusByUserIdRequest, SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ReDrawRarityParameterStatusByUserIdRequest, ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdComplete = new UnityEvent<string, ReDrawRarityParameterStatusByUserIdRequest, ReDrawRarityParameterStatusByUserIdResult>();
    #else
        public static Action<string, ReDrawRarityParameterStatusByUserIdRequest, ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AddRarityParameterStatusByUserIdRequest, AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdComplete = new UnityEvent<string, AddRarityParameterStatusByUserIdRequest, AddRarityParameterStatusByUserIdResult>();
    #else
        public static Action<string, AddRarityParameterStatusByUserIdRequest, AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetRarityParameterStatusByUserIdRequest, SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdComplete = new UnityEvent<string, SetRarityParameterStatusByUserIdRequest, SetRarityParameterStatusByUserIdResult>();
    #else
        public static Action<string, SetRarityParameterStatusByUserIdRequest, SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdComplete;
    #endif

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "ReDrawBalanceParameterStatusByUserId": {
                        var requestModel = ReDrawBalanceParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ReDrawBalanceParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        ReDrawBalanceParameterStatusByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetBalanceParameterStatusByUserId": {
                        var requestModel = SetBalanceParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetBalanceParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        SetBalanceParameterStatusByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ReDrawRarityParameterStatusByUserId": {
                        var requestModel = ReDrawRarityParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ReDrawRarityParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        ReDrawRarityParameterStatusByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AddRarityParameterStatusByUserId": {
                        var requestModel = AddRarityParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddRarityParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        AddRarityParameterStatusByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetRarityParameterStatusByUserId": {
                        var requestModel = SetRarityParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetRarityParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        SetRarityParameterStatusByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromJobResult(
                string method,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
            switch (method) {
                case "re_draw_balance_parameter_status_by_user_id": {
                    var requestModel = ReDrawBalanceParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = ReDrawBalanceParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    ReDrawBalanceParameterStatusByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_balance_parameter_status_by_user_id": {
                    var requestModel = SetBalanceParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetBalanceParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    SetBalanceParameterStatusByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "re_draw_rarity_parameter_status_by_user_id": {
                    var requestModel = ReDrawRarityParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = ReDrawRarityParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    ReDrawRarityParameterStatusByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "add_rarity_parameter_status_by_user_id": {
                    var requestModel = AddRarityParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddRarityParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    AddRarityParameterStatusByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_rarity_parameter_status_by_user_id": {
                    var requestModel = SetRarityParameterStatusByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetRarityParameterStatusByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    SetRarityParameterStatusByUserIdComplete?.Invoke(
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

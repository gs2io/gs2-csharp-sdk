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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
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
        ) => CreateNamespaceAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Enchant.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enchant> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) => DumpUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Enchant> DumpUserDataAsync(
        #else
        public async Task<Gs2Enchant> DumpUserDataAsync(
        #endif
            DumpUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.DumpUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enchant> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) => CheckDumpUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Enchant> CheckDumpUserDataAsync(
        #else
        public async Task<Gs2Enchant> CheckDumpUserDataAsync(
        #endif
            CheckDumpUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CheckDumpUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enchant> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) => CleanUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Enchant> CleanUserDataAsync(
        #else
        public async Task<Gs2Enchant> CleanUserDataAsync(
        #endif
            CleanUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CleanUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enchant> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) => CheckCleanUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Enchant> CheckCleanUserDataAsync(
        #else
        public async Task<Gs2Enchant> CheckCleanUserDataAsync(
        #endif
            CheckCleanUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CheckCleanUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enchant> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) => PrepareImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Enchant> PrepareImportUserDataAsync(
        #else
        public async Task<Gs2Enchant> PrepareImportUserDataAsync(
        #endif
            PrepareImportUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.PrepareImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.UploadToken = domain.UploadToken = result?.UploadToken;
            this.UploadUrl = domain.UploadUrl = result?.UploadUrl;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enchant> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) => ImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Enchant> ImportUserDataAsync(
        #else
        public async Task<Gs2Enchant> ImportUserDataAsync(
        #endif
            ImportUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.ImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Enchant> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) => CheckImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Enchant> CheckImportUserDataAsync(
        #else
        public async Task<Gs2Enchant> CheckImportUserDataAsync(
        #endif
            CheckImportUserDataByUserIdRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CheckImportUserDataByUserIdAsync(request)
            );
            var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Enchant.Model.Namespace> Namespaces(
            string namePrefix = null
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Enchant.Model.Namespace> NamespacesAsync(
        #else
        public DescribeNamespacesIterator NamespacesAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client,
                namePrefix
            );
        }

        public ulong SubscribeNamespaces(
            Action<Gs2.Gs2Enchant.Model.Namespace[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enchant.Model.Namespace>(
                (null as Gs2.Gs2Enchant.Model.Namespace).CacheParentKey(
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => namePrefix == null || item.Name.StartsWith(namePrefix))
                    .ToArray()),
                () =>
                {
        #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        #else
                    async Task Impl() {
        #endif
                        try {
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await NamespacesAsync(
                                namePrefix
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeNamespacesWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Enchant.Model.Namespace[]> callback,
            string namePrefix = null
        )
        {
            var items = await NamespacesAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeNamespaces(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeNamespaces(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enchant.Model.Namespace>(
                (null as Gs2.Gs2Enchant.Model.Namespace).CacheParentKey(
                    null
                ),
                callbackId
            );
        }

        public void InvalidateNamespaces(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Enchant.Model.Namespace>(
                (null as Gs2.Gs2Enchant.Model.Namespace).CacheParentKey(
                    null
                )
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
                int? timeOffset,
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
                            timeOffset,
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
                            timeOffset,
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
                            timeOffset,
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
                            timeOffset,
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
                            timeOffset,
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
                int? timeOffset,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromJobResult(
                string method,
                int? timeOffset,
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
                        timeOffset,
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
                        timeOffset,
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
                        timeOffset,
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
                        timeOffset,
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
                        timeOffset,
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

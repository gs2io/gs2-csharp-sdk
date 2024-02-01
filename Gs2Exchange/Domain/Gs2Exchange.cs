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
using Gs2.Gs2Exchange.Domain.Iterator;
using Gs2.Gs2Exchange.Model.Cache;
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
using Cysharp.Threading.Tasks.Linq;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Exchange.Domain
{

    public class Gs2Exchange {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ExchangeRestClient _client;
        public string Url { get; set; }
        public string UploadToken { get; set; }
        public string UploadUrl { get; set; }

        public Gs2Exchange(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ExchangeRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> self)
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
                var domain = new Gs2.Gs2Exchange.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Exchange.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Exchange.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Exchange> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Exchange> self)
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
            return new Gs2InlineFuture<Gs2Exchange>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Exchange> DumpUserDataAsync(
            #else
        public async Task<Gs2Exchange> DumpUserDataAsync(
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
        public IFuture<Gs2Exchange> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Exchange> self)
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
            return new Gs2InlineFuture<Gs2Exchange>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Exchange> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Exchange> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Exchange> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Exchange> self)
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
            return new Gs2InlineFuture<Gs2Exchange>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Exchange> CleanUserDataAsync(
            #else
        public async Task<Gs2Exchange> CleanUserDataAsync(
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
        public IFuture<Gs2Exchange> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Exchange> self)
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
            return new Gs2InlineFuture<Gs2Exchange>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Exchange> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Exchange> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Exchange> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Exchange> self)
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
            return new Gs2InlineFuture<Gs2Exchange>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Exchange> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Exchange> PrepareImportUserDataAsync(
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
        public IFuture<Gs2Exchange> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Exchange> self)
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
            return new Gs2InlineFuture<Gs2Exchange>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Exchange> ImportUserDataAsync(
            #else
        public async Task<Gs2Exchange> ImportUserDataAsync(
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
        public IFuture<Gs2Exchange> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Exchange> self)
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
            return new Gs2InlineFuture<Gs2Exchange>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Exchange> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Exchange> CheckImportUserDataAsync(
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
        public Gs2Iterator<Gs2.Gs2Exchange.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2.Cache,
                this._client
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Exchange.Model.Namespace> NamespacesAsync(
            #else
        public DescribeNamespacesIterator NamespacesAsync(
            #endif
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2.Cache,
                this._client
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeNamespaces(
            Action<Gs2.Gs2Exchange.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Exchange.Model.Namespace>(
                (null as Gs2.Gs2Exchange.Model.Namespace).CacheParentKey(
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2Exchange.Model.Namespace[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Exchange.Model.Namespace>(
                (null as Gs2.Gs2Exchange.Model.Namespace).CacheParentKey(
                ),
                callbackId
            );
        }

        public Gs2.Gs2Exchange.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Exchange.Domain.Model.NamespaceDomain(
                this._gs2,
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

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "ExchangeByUserId": {
                        var requestModel = ExchangeByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ExchangeByUserIdResult.FromJson(JsonMapper.ToObject(result));

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

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

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

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DeleteAwaitByUserId": {
                        var requestModel = DeleteAwaitByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DeleteAwaitByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        DeleteAwaitByUserIdComplete?.Invoke(
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
                case "exchange_by_user_id": {
                    var requestModel = ExchangeByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = ExchangeByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

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

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

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

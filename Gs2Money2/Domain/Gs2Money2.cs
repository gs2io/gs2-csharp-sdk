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
using Gs2.Gs2Money2.Domain.Iterator;
using Gs2.Gs2Money2.Model.Cache;
using Gs2.Gs2Money2.Domain.Model;
using Gs2.Gs2Money2.Request;
using Gs2.Gs2Money2.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Money2.Model;
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

namespace Gs2.Gs2Money2.Domain
{

    public class Gs2Money2 {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Money2RestClient _client;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2Money2(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Money2RestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> self)
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
                var domain = new Gs2.Gs2Money2.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money2.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Money2.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Money2.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Money2> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Money2> self)
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
            return new Gs2InlineFuture<Gs2Money2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Money2> DumpUserDataAsync(
            #else
        public async Task<Gs2Money2> DumpUserDataAsync(
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
        public IFuture<Gs2Money2> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Money2> self)
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
            return new Gs2InlineFuture<Gs2Money2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Money2> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Money2> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Money2> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Money2> self)
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
            return new Gs2InlineFuture<Gs2Money2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Money2> CleanUserDataAsync(
            #else
        public async Task<Gs2Money2> CleanUserDataAsync(
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
        public IFuture<Gs2Money2> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Money2> self)
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
            return new Gs2InlineFuture<Gs2Money2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Money2> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Money2> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Money2> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Money2> self)
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
            return new Gs2InlineFuture<Gs2Money2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Money2> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Money2> PrepareImportUserDataAsync(
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
        public IFuture<Gs2Money2> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Money2> self)
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
            return new Gs2InlineFuture<Gs2Money2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Money2> ImportUserDataAsync(
            #else
        public async Task<Gs2Money2> ImportUserDataAsync(
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
        public IFuture<Gs2Money2> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Money2> self)
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
            return new Gs2InlineFuture<Gs2Money2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Money2> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Money2> CheckImportUserDataAsync(
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
        public Gs2Iterator<Gs2.Gs2Money2.Model.Namespace> Namespaces(
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
        public IUniTaskAsyncEnumerable<Gs2.Gs2Money2.Model.Namespace> NamespacesAsync(
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
            Action<Gs2.Gs2Money2.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money2.Model.Namespace>(
                (null as Gs2.Gs2Money2.Model.Namespace).CacheParentKey(
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await NamespacesAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2Money2.Model.Namespace[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money2.Model.Namespace>(
                (null as Gs2.Gs2Money2.Model.Namespace).CacheParentKey(
                ),
                callbackId
            );
        }

        public void InvalidateNamespaces(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Money2.Model.Namespace>(
                (null as Gs2.Gs2Money2.Model.Namespace).CacheParentKey(
                )
            );
        }

        public Gs2.Gs2Money2.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Money2.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DepositByUserIdRequest, DepositByUserIdResult> DepositByUserIdComplete = new UnityEvent<string, DepositByUserIdRequest, DepositByUserIdResult>();
    #else
        public static Action<string, DepositByUserIdRequest, DepositByUserIdResult> DepositByUserIdComplete;
    #endif

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DepositByUserId": {
                        var requestModel = DepositByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DepositByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        DepositByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, WithdrawByUserIdRequest, WithdrawByUserIdResult> WithdrawByUserIdComplete = new UnityEvent<string, WithdrawByUserIdRequest, WithdrawByUserIdResult>();
    #else
        public static Action<string, WithdrawByUserIdRequest, WithdrawByUserIdResult> WithdrawByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, VerifyReceiptByUserIdRequest, VerifyReceiptByUserIdResult> VerifyReceiptByUserIdComplete = new UnityEvent<string, VerifyReceiptByUserIdRequest, VerifyReceiptByUserIdResult>();
    #else
        public static Action<string, VerifyReceiptByUserIdRequest, VerifyReceiptByUserIdResult> VerifyReceiptByUserIdComplete;
    #endif

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "WithdrawByUserId": {
                        var requestModel = WithdrawByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = WithdrawByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        WithdrawByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "VerifyReceiptByUserId": {
                        var requestModel = VerifyReceiptByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = VerifyReceiptByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        VerifyReceiptByUserIdComplete?.Invoke(
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
                case "deposit_by_user_id": {
                    var requestModel = DepositByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = DepositByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    DepositByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
            }
        }
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class ChangeSubscriptionStatusEvent : UnityEvent<ChangeSubscriptionStatus>
        {

        }

        [SerializeField]
        private ChangeSubscriptionStatusEvent onChangeSubscriptionStatus = new ChangeSubscriptionStatusEvent();

        public event UnityAction<ChangeSubscriptionStatus> OnChangeSubscriptionStatus
        {
            add => onChangeSubscriptionStatus.AddListener(value);
            remove => onChangeSubscriptionStatus.RemoveListener(value);
        }
    #endif

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
            switch (action) {
                case "ChangeSubscriptionStatus": {
                    var notification = ChangeSubscriptionStatus.FromJson(JsonMapper.ToObject(payload));
                    _gs2.Cache.ClearListCache<Gs2.Gs2Money2.Model.SubscriptionStatus>(
                        (null as Gs2.Gs2Money2.Model.SubscriptionStatus).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId
                        )
                    );
                    (null as Gs2.Gs2Money2.Model.SubscriptionStatus).DeleteCache(
                        _gs2.Cache,
                        notification.NamespaceName,
                        notification.UserId,
                        notification.ContentName
                    );
    #if UNITY_2017_1_OR_NEWER
                    onChangeSubscriptionStatus.Invoke(notification);
    #endif
                    break;
                }
            }
        }
    }
}

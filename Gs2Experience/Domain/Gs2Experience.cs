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
using Gs2.Gs2Experience.Domain.Iterator;
using Gs2.Gs2Experience.Model.Cache;
using Gs2.Gs2Experience.Domain.Model;
using Gs2.Gs2Experience.Request;
using Gs2.Gs2Experience.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Experience.Model;
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

namespace Gs2.Gs2Experience.Domain
{

    public class Gs2Experience {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ExperienceRestClient _client;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2Experience(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ExperienceRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Experience.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Experience.Domain.Model.NamespaceDomain> self)
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
                var domain = new Gs2.Gs2Experience.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Experience.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Experience.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Experience.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Experience.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Experience> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Experience> self)
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
            return new Gs2InlineFuture<Gs2Experience>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Experience> DumpUserDataAsync(
            #else
        public async Task<Gs2Experience> DumpUserDataAsync(
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
        public IFuture<Gs2Experience> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Experience> self)
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
            return new Gs2InlineFuture<Gs2Experience>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Experience> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Experience> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Experience> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Experience> self)
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
            return new Gs2InlineFuture<Gs2Experience>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Experience> CleanUserDataAsync(
            #else
        public async Task<Gs2Experience> CleanUserDataAsync(
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
        public IFuture<Gs2Experience> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Experience> self)
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
            return new Gs2InlineFuture<Gs2Experience>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Experience> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Experience> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Experience> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Experience> self)
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
            return new Gs2InlineFuture<Gs2Experience>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Experience> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Experience> PrepareImportUserDataAsync(
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
        public IFuture<Gs2Experience> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Experience> self)
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
            return new Gs2InlineFuture<Gs2Experience>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Experience> ImportUserDataAsync(
            #else
        public async Task<Gs2Experience> ImportUserDataAsync(
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
        public IFuture<Gs2Experience> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Experience> self)
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
            return new Gs2InlineFuture<Gs2Experience>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Experience> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Experience> CheckImportUserDataAsync(
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
        public Gs2Iterator<Gs2.Gs2Experience.Model.Namespace> Namespaces(
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
        public IUniTaskAsyncEnumerable<Gs2.Gs2Experience.Model.Namespace> NamespacesAsync(
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
            Action<Gs2.Gs2Experience.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Experience.Model.Namespace>(
                (null as Gs2.Gs2Experience.Model.Namespace).CacheParentKey(
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2Experience.Model.Namespace[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Experience.Model.Namespace>(
                (null as Gs2.Gs2Experience.Model.Namespace).CacheParentKey(
                ),
                callbackId
            );
        }

        public Gs2.Gs2Experience.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Experience.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AddExperienceByUserIdRequest, AddExperienceByUserIdResult> AddExperienceByUserIdComplete = new UnityEvent<string, AddExperienceByUserIdRequest, AddExperienceByUserIdResult>();
    #else
        public static Action<string, AddExperienceByUserIdRequest, AddExperienceByUserIdResult> AddExperienceByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetExperienceByUserIdRequest, SetExperienceByUserIdResult> SetExperienceByUserIdComplete = new UnityEvent<string, SetExperienceByUserIdRequest, SetExperienceByUserIdResult>();
    #else
        public static Action<string, SetExperienceByUserIdRequest, SetExperienceByUserIdResult> SetExperienceByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AddRankCapByUserIdRequest, AddRankCapByUserIdResult> AddRankCapByUserIdComplete = new UnityEvent<string, AddRankCapByUserIdRequest, AddRankCapByUserIdResult>();
    #else
        public static Action<string, AddRankCapByUserIdRequest, AddRankCapByUserIdResult> AddRankCapByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetRankCapByUserIdRequest, SetRankCapByUserIdResult> SetRankCapByUserIdComplete = new UnityEvent<string, SetRankCapByUserIdRequest, SetRankCapByUserIdResult>();
    #else
        public static Action<string, SetRankCapByUserIdRequest, SetRankCapByUserIdResult> SetRankCapByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, MultiplyAcquireActionsByUserIdRequest, MultiplyAcquireActionsByUserIdResult> MultiplyAcquireActionsByUserIdComplete = new UnityEvent<string, MultiplyAcquireActionsByUserIdRequest, MultiplyAcquireActionsByUserIdResult>();
    #else
        public static Action<string, MultiplyAcquireActionsByUserIdRequest, MultiplyAcquireActionsByUserIdResult> MultiplyAcquireActionsByUserIdComplete;
    #endif

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "AddExperienceByUserId": {
                        var requestModel = AddExperienceByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddExperienceByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        AddExperienceByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetExperienceByUserId": {
                        var requestModel = SetExperienceByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetExperienceByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        SetExperienceByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AddRankCapByUserId": {
                        var requestModel = AddRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddRankCapByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        AddRankCapByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetRankCapByUserId": {
                        var requestModel = SetRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetRankCapByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        SetRankCapByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "MultiplyAcquireActionsByUserId": {
                        var requestModel = MultiplyAcquireActionsByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = MultiplyAcquireActionsByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        MultiplyAcquireActionsByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SubExperienceByUserIdRequest, SubExperienceByUserIdResult> SubExperienceByUserIdComplete = new UnityEvent<string, SubExperienceByUserIdRequest, SubExperienceByUserIdResult>();
    #else
        public static Action<string, SubExperienceByUserIdRequest, SubExperienceByUserIdResult> SubExperienceByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SubRankCapByUserIdRequest, SubRankCapByUserIdResult> SubRankCapByUserIdComplete = new UnityEvent<string, SubRankCapByUserIdRequest, SubRankCapByUserIdResult>();
    #else
        public static Action<string, SubRankCapByUserIdRequest, SubRankCapByUserIdResult> SubRankCapByUserIdComplete;
    #endif

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "SubExperienceByUserId": {
                        var requestModel = SubExperienceByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SubExperienceByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        SubExperienceByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SubRankCapByUserId": {
                        var requestModel = SubRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SubRankCapByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        SubRankCapByUserIdComplete?.Invoke(
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
                case "add_experience_by_user_id": {
                    var requestModel = AddExperienceByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddExperienceByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    AddExperienceByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_experience_by_user_id": {
                    var requestModel = SetExperienceByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetExperienceByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    SetExperienceByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "add_rank_cap_by_user_id": {
                    var requestModel = AddRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddRankCapByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    AddRankCapByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_rank_cap_by_user_id": {
                    var requestModel = SetRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetRankCapByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    SetRankCapByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "multiply_acquire_actions_by_user_id": {
                    var requestModel = MultiplyAcquireActionsByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = MultiplyAcquireActionsByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    MultiplyAcquireActionsByUserIdComplete?.Invoke(
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

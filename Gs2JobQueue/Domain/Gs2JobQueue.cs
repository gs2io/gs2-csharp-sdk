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
using Gs2.Gs2JobQueue.Domain.Iterator;
using Gs2.Gs2JobQueue.Model.Cache;
using Gs2.Gs2JobQueue.Domain.Model;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2JobQueue.Model;
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

namespace Gs2.Gs2JobQueue.Domain
{

    public class Gs2JobQueue {

        private static readonly List<RunNotification> _completedJobs = new List<RunNotification>();
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2JobQueueRestClient _client;
        public string Url { get; set; }
        public string UploadToken { get; set; }
        public string UploadUrl { get; set; }

        public Gs2JobQueue(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2JobQueueRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> self)
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
                var domain = new Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2JobQueue> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2JobQueue> self)
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
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2JobQueue> DumpUserDataAsync(
            #else
        public async Task<Gs2JobQueue> DumpUserDataAsync(
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
        public IFuture<Gs2JobQueue> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2JobQueue> self)
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
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2JobQueue> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2JobQueue> CheckDumpUserDataAsync(
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
        public IFuture<Gs2JobQueue> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2JobQueue> self)
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
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2JobQueue> CleanUserDataAsync(
            #else
        public async Task<Gs2JobQueue> CleanUserDataAsync(
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
        public IFuture<Gs2JobQueue> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2JobQueue> self)
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
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2JobQueue> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2JobQueue> CheckCleanUserDataAsync(
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
        public IFuture<Gs2JobQueue> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2JobQueue> self)
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
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2JobQueue> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2JobQueue> PrepareImportUserDataAsync(
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
        public IFuture<Gs2JobQueue> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2JobQueue> self)
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
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2JobQueue> ImportUserDataAsync(
            #else
        public async Task<Gs2JobQueue> ImportUserDataAsync(
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
        public IFuture<Gs2JobQueue> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2JobQueue> self)
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
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2JobQueue> CheckImportUserDataAsync(
            #else
        public async Task<Gs2JobQueue> CheckImportUserDataAsync(
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
        public Gs2Iterator<Gs2.Gs2JobQueue.Model.Namespace> Namespaces(
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
        public IUniTaskAsyncEnumerable<Gs2.Gs2JobQueue.Model.Namespace> NamespacesAsync(
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
            Action<Gs2.Gs2JobQueue.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2JobQueue.Model.Namespace>(
                (null as Gs2.Gs2JobQueue.Model.Namespace).CacheParentKey(
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2JobQueue.Model.Namespace[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2JobQueue.Model.Namespace>(
                (null as Gs2.Gs2JobQueue.Model.Namespace).CacheParentKey(
                ),
                callbackId
            );
        }

        public Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, PushByUserIdRequest, PushByUserIdResult> PushByUserIdComplete = new UnityEvent<string, PushByUserIdRequest, PushByUserIdResult>();
    #else
        public static Action<string, PushByUserIdRequest, PushByUserIdResult> PushByUserIdComplete;
    #endif

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "PushByUserId": {
                        var requestModel = PushByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = PushByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        PushByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DeleteJobByUserIdRequest, DeleteJobByUserIdResult> DeleteJobByUserIdComplete = new UnityEvent<string, DeleteJobByUserIdRequest, DeleteJobByUserIdResult>();
    #else
        public static Action<string, DeleteJobByUserIdRequest, DeleteJobByUserIdResult> DeleteJobByUserIdComplete;
    #endif

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DeleteJobByUserId": {
                        var requestModel = DeleteJobByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DeleteJobByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        DeleteJobByUserIdComplete?.Invoke(
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
                case "push_by_user_id": {
                    var requestModel = PushByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    PushByUserIdComplete?.Invoke(
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
        private class PushNotificationEvent : UnityEvent<PushNotification>
        {

        }

        [SerializeField]
        private PushNotificationEvent onPushNotification = new PushNotificationEvent();

        public event UnityAction<PushNotification> OnPushNotification
        {
            add => onPushNotification.AddListener(value);
            remove => onPushNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class RunNotificationEvent : UnityEvent<RunNotification>
        {

        }

        [SerializeField]
        private RunNotificationEvent onRunNotification = new RunNotificationEvent();

        public event UnityAction<RunNotification> OnRunNotification
        {
            add => onRunNotification.AddListener(value);
            remove => onRunNotification.RemoveListener(value);
        }
    #endif

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
    #if UNITY_2017_1_OR_NEWER
            switch (action) {
                case "Push": {
                    onPushNotification.Invoke(PushNotification.FromJson(JsonMapper.ToObject(payload)));
                    break;
                }
                case "RunNotification": {
                    lock (_completedJobs)
                    {
                        var notification = RunNotification.FromJson(JsonMapper.ToObject(payload));
                        _completedJobs.Add(notification);
                        onRunNotification.Invoke(notification);
                    }
                    break;
                }
            }
    #endif
        }
        
#if UNITY_2017_1_OR_NEWER
        public Gs2Future DispatchFuture(
            AccessToken accessToken
        )
        {
            RunNotification[] copiedCompletedJobs;
            IEnumerator Impl(Gs2Future self)
            {
                lock (_completedJobs)
                {
                    if (_completedJobs.Count == 0) {
                        yield break;
                    }
                    copiedCompletedJobs = new RunNotification[_completedJobs.Count];
                    _completedJobs.Where(v => v.UserId == accessToken.UserId).ToList().CopyTo(copiedCompletedJobs);
                    foreach (var copiedCompletedJob in copiedCompletedJobs) {
                        _completedJobs.Remove(copiedCompletedJob);
                    }
                }
                foreach (var completedJob in copiedCompletedJobs)
                {
                    if (completedJob == null) continue;
                    {
                        var future = Namespace(
                            completedJob.NamespaceName
                        ).AccessToken(
                            accessToken
                        ).Job(
                            completedJob.JobName
                        ).JobResult().ModelNoCacheFuture();
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is Gs2.Core.Exception.NotFoundException) {
                            }
                            else {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                    }
                    {
                        var autoRun = new AutoJobQueueAccessTokenDomain(
                            _gs2,
                            accessToken,
                            completedJob.NamespaceName,
                            completedJob.JobName
                        );
                        var future = autoRun.WaitFuture();
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is Gs2.Core.Exception.NotFoundException) {
                            }
                            else {
                                self.OnError(future.Error);
                            }
                            yield break;
                        }
                    }
                }
                self.OnComplete(null);
                yield return null;
            }

            return new Gs2InlineFuture(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask DispatchAsync(
    #else
        public async Task DispatchAsync(
    #endif
            AccessToken accessToken
        )
        {
            RunNotification[] copiedCompletedJobs;
            lock (_completedJobs)
            {
                if (_completedJobs.Count == 0) {
                    return;
                }
                copiedCompletedJobs = new RunNotification[_completedJobs.Count];
                _completedJobs.Where(v => v.UserId == accessToken.UserId).ToList().CopyTo(copiedCompletedJobs);
                foreach (var copiedCompletedJob in copiedCompletedJobs) {
                    _completedJobs.Remove(copiedCompletedJob);
                }
            }
            foreach (var completedJob in copiedCompletedJobs)
            {
                if (completedJob == null) continue;
                var autoRun = new AutoJobQueueAccessTokenDomain(
                    _gs2,
                    accessToken,
                    completedJob.NamespaceName,
                    completedJob.JobName
                );
                try
                {
                    await Namespace(
                        completedJob.NamespaceName
                    ).AccessToken(
                        accessToken
                    ).Job(
                        completedJob.JobName
                    ).JobResult().ModelNoCacheAsync();
                    await autoRun.WaitAsync();
                }
                catch (NotFoundException)
                {
                }
            }
        }
#endif
        
#if UNITY_2017_1_OR_NEWER
        public Gs2Future DispatchByUserIdFuture(
            string userId
        )
        {
            RunNotification[] copiedCompletedJobs;
            IEnumerator Impl(Gs2Future self)
            {
                lock (_completedJobs)
                {
                    if (_completedJobs.Count == 0) {
                        yield break;
                    }
                    copiedCompletedJobs = new RunNotification[_completedJobs.Count];
                    _completedJobs.Where(v => v.UserId == userId).ToList().CopyTo(copiedCompletedJobs);
                    foreach (var copiedCompletedJob in copiedCompletedJobs) {
                        _completedJobs.Remove(copiedCompletedJob);
                    }
                }
                foreach (var completedJob in copiedCompletedJobs)
                {
                    if (completedJob == null) continue;
                    {
                        var future = Namespace(
                            completedJob.NamespaceName
                        ).User(
                            userId
                        ).Job(
                            completedJob.JobName
                        ).JobResult().ModelNoCacheFuture();
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is Gs2.Core.Exception.NotFoundException) {
                            }
                            else {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                    }
                    {
                        var autoRun = new AutoJobQueueDomain(
                            _gs2,
                            userId,
                            completedJob.NamespaceName,
                            completedJob.JobName
                        );
                        var future = autoRun.WaitFuture();
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is Gs2.Core.Exception.NotFoundException) {
                            }
                            else {
                                self.OnError(future.Error);
                            }
                            yield break;
                        }
                    }
                }
            }

            return new Gs2InlineFuture(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask DispatchByUserIdAsync(
    #else
        public async Task DispatchByUserIdAsync(
    #endif
            string userId
        )
        {
            RunNotification[] copiedCompletedJobs;
            lock (_completedJobs)
            {
                if (_completedJobs.Count == 0) {
                    return;
                }
                copiedCompletedJobs = new RunNotification[_completedJobs.Count];
                _completedJobs.Where(v => v.UserId == userId).ToList().CopyTo(copiedCompletedJobs);
                foreach (var copiedCompletedJob in copiedCompletedJobs) {
                    _completedJobs.Remove(copiedCompletedJob);
                }
            }
            foreach (var completedJob in copiedCompletedJobs)
            {
                if (completedJob == null) continue;
                var autoRun = new AutoJobQueueDomain(
                    _gs2,
                    userId,
                    completedJob.NamespaceName,
                    completedJob.JobName
                );
                try
                {
                    await Namespace(
                        completedJob.NamespaceName
                    ).User(
                        userId
                    ).Job(
                        completedJob.JobName
                    ).JobResult().ModelNoCacheAsync();
                    await autoRun.WaitAsync();
                }
                catch (NotFoundException)
                {
                }
            }
        }
#endif
    }
}

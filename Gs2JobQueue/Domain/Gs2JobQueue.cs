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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2JobQueue.Domain.Iterator;
using Gs2.Gs2JobQueue.Domain.Model;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Gs2JobQueue.Model;
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
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2JobQueue.Domain
{

    public class Gs2JobQueue {

        private static readonly List<RunNotification> _completedJobs = new List<RunNotification>();
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2JobQueueRestClient _client;

        private readonly String _parentKey;
        public string Url { get; set; }

        public Gs2JobQueue(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2JobQueueRestClient(
                session
            );
            this._parentKey = "jobQueue";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                #else
                CreateNamespaceResult result = null;
                    result = await this._client.CreateNamespaceAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "jobQueue",
                            "Namespace"
                        );
                        var key = Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            CreateNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            #else
            CreateNamespaceResult result = null;
                result = await this._client.CreateNamespaceAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "jobQueue",
                        "Namespace"
                    );
                    var key = Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            CreateNamespaceRequest request
        ) {
            var future = CreateNamespaceFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateNamespaceFuture.")]
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain> CreateNamespace(
            CreateNamespaceRequest request
        ) {
            return CreateNamespaceFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2JobQueue> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2JobQueue> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                #else
                DumpUserDataByUserIdResult result = null;
                    result = await this._client.DumpUserDataByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #else
        public async Task<Gs2JobQueue> DumpUserDataAsync(
            DumpUserDataByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            #else
            DumpUserDataByUserIdResult result = null;
                result = await this._client.DumpUserDataByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2JobQueue> DumpUserDataAsync(
            DumpUserDataByUserIdRequest request
        ) {
            var future = DumpUserDataFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DumpUserDataFuture.")]
        public IFuture<Gs2JobQueue> DumpUserData(
            DumpUserDataByUserIdRequest request
        ) {
            return DumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2JobQueue> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2JobQueue> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                #else
                CheckDumpUserDataByUserIdResult result = null;
                    result = await this._client.CheckDumpUserDataByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Url = domain.Url = result?.Url;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #else
        public async Task<Gs2JobQueue> CheckDumpUserDataAsync(
            CheckDumpUserDataByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            #else
            CheckDumpUserDataByUserIdResult result = null;
                result = await this._client.CheckDumpUserDataByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2JobQueue> CheckDumpUserDataAsync(
            CheckDumpUserDataByUserIdRequest request
        ) {
            var future = CheckDumpUserDataFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CheckDumpUserDataFuture.")]
        public IFuture<Gs2JobQueue> CheckDumpUserData(
            CheckDumpUserDataByUserIdRequest request
        ) {
            return CheckDumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2JobQueue> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2JobQueue> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                #else
                CleanUserDataByUserIdResult result = null;
                    result = await this._client.CleanUserDataByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #else
        public async Task<Gs2JobQueue> CleanUserDataAsync(
            CleanUserDataByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            #else
            CleanUserDataByUserIdResult result = null;
                result = await this._client.CleanUserDataByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2JobQueue> CleanUserDataAsync(
            CleanUserDataByUserIdRequest request
        ) {
            var future = CleanUserDataFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CleanUserDataFuture.")]
        public IFuture<Gs2JobQueue> CleanUserData(
            CleanUserDataByUserIdRequest request
        ) {
            return CleanUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2JobQueue> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2JobQueue> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                #else
                CheckCleanUserDataByUserIdResult result = null;
                    result = await this._client.CheckCleanUserDataByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2JobQueue>(Impl);
        }
        #else
        public async Task<Gs2JobQueue> CheckCleanUserDataAsync(
            CheckCleanUserDataByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            #else
            CheckCleanUserDataByUserIdResult result = null;
                result = await this._client.CheckCleanUserDataByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2JobQueue> CheckCleanUserDataAsync(
            CheckCleanUserDataByUserIdRequest request
        ) {
            var future = CheckCleanUserDataFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CheckCleanUserDataFuture.")]
        public IFuture<Gs2JobQueue> CheckCleanUserData(
            CheckCleanUserDataByUserIdRequest request
        ) {
            return CheckCleanUserDataFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2JobQueue.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2JobQueue.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2JobQueue.Model.Namespace> Namespaces(
            #endif
        #else
        public DescribeNamespacesIterator Namespaces(
        #endif
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
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
            return this._cache.ListSubscribe<Gs2.Gs2JobQueue.Model.Namespace>(
                "jobQueue:Namespace",
                callback
            );
        }

        public void UnsubscribeNamespaces(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2JobQueue.Model.Namespace>(
                "jobQueue:Namespace",
                callbackId
            );
        }

        public Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2JobQueue.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, PushByUserIdRequest, PushByUserIdResult> PushByUserIdComplete = new UnityEvent<string, PushByUserIdRequest, PushByUserIdResult>();
    #else
        public static Action<string, PushByUserIdRequest, PushByUserIdResult> PushByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampSheet(
                CacheDatabase cache,
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "PushByUserId": {
                        var requestModel = PushByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = PushByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Job"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                                    item.Name.ToString()
                                );
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }

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

        public static void UpdateCacheFromStampTask(
                CacheDatabase cache,
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DeleteJobByUserId": {
                        var requestModel = DeleteJobByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DeleteJobByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Job"
                            );
                            var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Delete<Gs2.Gs2JobQueue.Model.Job>(parentKey, key);
                        }

                        DeleteJobByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

        public static void UpdateCacheFromJobResult(
                CacheDatabase cache,
                string method,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
            switch (method) {
                case "push_by_user_id": {
                    var requestModel = PushByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    {
                        var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Job"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2JobQueue.Domain.Model.JobDomain.CreateCacheKey(
                                item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }

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
    #if GS2_ENABLE_UNITASK
        public static async UniTask Dispatch(
    #else
        public static Gs2Future Dispatch(
    #endif
            CacheDatabase cache,
            Gs2RestSession session,
            AccessToken accessToken
        )
        {
            RunNotification[] copiedCompletedJobs;
    #if !GS2_ENABLE_UNITASK
            IEnumerator Impl(Gs2Future self)
            {
    #endif
            lock (_completedJobs)
            {
                if (_completedJobs.Count == 0)
                {
    #if GS2_ENABLE_UNITASK
                    return;
    #else
                    yield break;
    #endif
                }
                copiedCompletedJobs = new RunNotification[_completedJobs.Count];
                _completedJobs.CopyTo(copiedCompletedJobs);
                _completedJobs.Clear();
            }
            foreach (var completedJob in copiedCompletedJobs)
            {
                var client = new Gs2JobQueueRestClient(
                    session
                );
    #if GS2_ENABLE_UNITASK
                GetJobResultResult result = null;
                try
                {
                    result = await client.GetJobResultAsync(
                        new GetJobResultRequest()
                            .WithNamespaceName(completedJob.NamespaceName)
                            .WithJobName(completedJob.JobName)
                            .WithAccessToken(accessToken.Token)
                    );
                }
                catch (NotFoundException)
                {
                }
    #else
                var future = client.GetJobResultFuture(
                    new GetJobResultRequest()
                        .WithNamespaceName(completedJob.NamespaceName)
                        .WithJobName(completedJob.JobName)
                        .WithAccessToken(accessToken.Token)
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
    #endif
                if (result != null)
                {
                    Gs2.Core.Domain.Gs2.UpdateCacheFromJobResult(
                        cache,
                        new Job
                        {
                            ScriptId = result?.Item.ScriptId,
                            Args = result?.Item.Args,
                        },
                        new JobResultBody
                        {
                            TryNumber = result?.Item.TryNumber,
                            StatusCode = result?.Item.StatusCode,
                            Result = result?.Item.Result,
                            TryAt = result?.Item.TryAt
                        }
                    );
                }
            }
    #if !GS2_ENABLE_UNITASK
            }

            return new Gs2InlineFuture(Impl);
    #endif
        }
    #endif
    }
}

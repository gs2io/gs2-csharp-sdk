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
using Gs2.Gs2Inbox.Domain.Iterator;
using Gs2.Gs2Inbox.Domain.Model;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Result;
using Gs2.Gs2Inbox.Model;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Inbox.Model;
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

namespace Gs2.Gs2Inbox.Domain
{

    public class Gs2Inbox {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InboxRestClient _client;

        private readonly String _parentKey;
        public string Url { get; set; }

        public Gs2Inbox(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2InboxRestClient(
                session
            );
            this._parentKey = "inbox";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.NamespaceDomain> self)
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
                            "inbox",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Inbox.Domain.Model.NamespaceDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
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
                        "inbox",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Inbox.Domain.Model.NamespaceDomain(
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
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
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
        public IFuture<Gs2.Gs2Inbox.Domain.Model.NamespaceDomain> CreateNamespace(
            CreateNamespaceRequest request
        ) {
            return CreateNamespaceFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inbox> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inbox> self)
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
            return new Gs2InlineFuture<Gs2Inbox>(Impl);
        }
        #else
        public async Task<Gs2Inbox> DumpUserDataAsync(
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
        public async UniTask<Gs2Inbox> DumpUserDataAsync(
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
        public IFuture<Gs2Inbox> DumpUserData(
            DumpUserDataByUserIdRequest request
        ) {
            return DumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inbox> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inbox> self)
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
            return new Gs2InlineFuture<Gs2Inbox>(Impl);
        }
        #else
        public async Task<Gs2Inbox> CheckDumpUserDataAsync(
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
        public async UniTask<Gs2Inbox> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Inbox> CheckDumpUserData(
            CheckDumpUserDataByUserIdRequest request
        ) {
            return CheckDumpUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inbox> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inbox> self)
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
            return new Gs2InlineFuture<Gs2Inbox>(Impl);
        }
        #else
        public async Task<Gs2Inbox> CleanUserDataAsync(
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
        public async UniTask<Gs2Inbox> CleanUserDataAsync(
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
        public IFuture<Gs2Inbox> CleanUserData(
            CleanUserDataByUserIdRequest request
        ) {
            return CleanUserDataFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Inbox> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2Inbox> self)
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
            return new Gs2InlineFuture<Gs2Inbox>(Impl);
        }
        #else
        public async Task<Gs2Inbox> CheckCleanUserDataAsync(
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
        public async UniTask<Gs2Inbox> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Inbox> CheckCleanUserData(
            CheckCleanUserDataByUserIdRequest request
        ) {
            return CheckCleanUserDataFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inbox.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inbox.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inbox.Model.Namespace> Namespaces(
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
            return this._cache.ListSubscribe<Gs2.Gs2Inbox.Model.Namespace>(
                "inbox:Namespace",
                callback
            );
        }

        public void UnsubscribeNamespaces(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Inbox.Model.Namespace>(
                "inbox:Namespace",
                callbackId
            );
        }

        public Gs2.Gs2Inbox.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SendMessageByUserIdRequest, SendMessageByUserIdResult> SendMessageByUserIdComplete = new UnityEvent<string, SendMessageByUserIdRequest, SendMessageByUserIdResult>();
    #else
        public static Action<string, SendMessageByUserIdRequest, SendMessageByUserIdResult> SendMessageByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampSheet(
                CacheDatabase cache,
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "SendMessageByUserId": {
                        var requestModel = SendMessageByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SendMessageByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Message"
                            );
                            var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }

                        SendMessageByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, OpenMessageByUserIdRequest, OpenMessageByUserIdResult> OpenMessageByUserIdComplete = new UnityEvent<string, OpenMessageByUserIdRequest, OpenMessageByUserIdResult>();
    #else
        public static Action<string, OpenMessageByUserIdRequest, OpenMessageByUserIdResult> OpenMessageByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DeleteMessageByUserIdRequest, DeleteMessageByUserIdResult> DeleteMessageByUserIdComplete = new UnityEvent<string, DeleteMessageByUserIdRequest, DeleteMessageByUserIdResult>();
    #else
        public static Action<string, DeleteMessageByUserIdRequest, DeleteMessageByUserIdResult> DeleteMessageByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampTask(
                CacheDatabase cache,
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "OpenMessageByUserId": {
                        var requestModel = OpenMessageByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = OpenMessageByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Message"
                            );
                            var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Delete<Gs2.Gs2Inbox.Model.Message>(parentKey, key);
                            cache.ClearListCache<Gs2.Gs2Inbox.Model.Message>(
                                parentKey
                            );
                        }

                        OpenMessageByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "DeleteMessageByUserId": {
                        var requestModel = DeleteMessageByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DeleteMessageByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Message"
                            );
                            var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                                resultModel.Item.Name.ToString()
                            );
                            cache.Delete<Gs2.Gs2Inbox.Model.Message>(parentKey, key);
                        }

                        DeleteMessageByUserIdComplete?.Invoke(
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
                case "send_message_by_user_id": {
                    var requestModel = SendMessageByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SendMessageByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Message"
                        );
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }

                    SendMessageByUserIdComplete?.Invoke(
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
        private class ReceiveNotificationEvent : UnityEvent<ReceiveNotification>
        {

        }

        [SerializeField]
        private ReceiveNotificationEvent onReceiveNotification = new ReceiveNotificationEvent();

        public event UnityAction<ReceiveNotification> OnReceiveNotification
        {
            add => onReceiveNotification.AddListener(value);
            remove => onReceiveNotification.RemoveListener(value);
        }
    #endif

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
    #if UNITY_2017_1_OR_NEWER
            switch (action) {
                case "ReceiveNotification": {
                    var notification = ReceiveNotification.FromJson(JsonMapper.ToObject(payload));
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        notification.NamespaceName,
                        notification.UserId,
                        "Message"
                    );
                    cache.ClearListCache <Message>(
                        parentKey
                    );
                    onReceiveNotification.Invoke(notification);
                    break;
                }
            }
    #endif
        }
    }
}

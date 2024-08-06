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
using Gs2.Gs2Friend.Domain.Iterator;
using Gs2.Gs2Friend.Model.Cache;
using Gs2.Gs2Friend.Domain.Model;
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Friend.Model;
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

namespace Gs2.Gs2Friend.Domain
{

    public class Gs2Friend {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2Friend(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.NamespaceDomain> self)
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
                var domain = new Gs2.Gs2Friend.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Friend.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Friend.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Friend> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Friend> self)
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
            return new Gs2InlineFuture<Gs2Friend>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Friend> DumpUserDataAsync(
            #else
        public async Task<Gs2Friend> DumpUserDataAsync(
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
        public IFuture<Gs2Friend> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Friend> self)
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
            return new Gs2InlineFuture<Gs2Friend>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Friend> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Friend> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Friend> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Friend> self)
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
            return new Gs2InlineFuture<Gs2Friend>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Friend> CleanUserDataAsync(
            #else
        public async Task<Gs2Friend> CleanUserDataAsync(
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
        public IFuture<Gs2Friend> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Friend> self)
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
            return new Gs2InlineFuture<Gs2Friend>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Friend> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Friend> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Friend> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Friend> self)
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
            return new Gs2InlineFuture<Gs2Friend>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Friend> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Friend> PrepareImportUserDataAsync(
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
        public IFuture<Gs2Friend> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Friend> self)
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
            return new Gs2InlineFuture<Gs2Friend>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Friend> ImportUserDataAsync(
            #else
        public async Task<Gs2Friend> ImportUserDataAsync(
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
        public IFuture<Gs2Friend> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Friend> self)
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
            return new Gs2InlineFuture<Gs2Friend>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Friend> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Friend> CheckImportUserDataAsync(
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
        public Gs2Iterator<Gs2.Gs2Friend.Model.Namespace> Namespaces(
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
        public IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.Namespace> NamespacesAsync(
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

        public Gs2.Gs2Friend.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Friend.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, UpdateProfileByUserIdRequest, UpdateProfileByUserIdResult> UpdateProfileByUserIdComplete = new UnityEvent<string, UpdateProfileByUserIdRequest, UpdateProfileByUserIdResult>();
    #else
        public static Action<string, UpdateProfileByUserIdRequest, UpdateProfileByUserIdResult> UpdateProfileByUserIdComplete;
    #endif

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "UpdateProfileByUserId": {
                        var requestModel = UpdateProfileByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = UpdateProfileByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.UserId,
                            requestModel
                        );

                        UpdateProfileByUserIdComplete?.Invoke(
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
                case "update_profile_by_user_id": {
                    var requestModel = UpdateProfileByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = UpdateProfileByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.UserId,
                        requestModel
                    );

                    UpdateProfileByUserIdComplete?.Invoke(
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
        private class FollowNotificationEvent : UnityEvent<FollowNotification>
        {

        }

        [SerializeField]
        private FollowNotificationEvent onFollowNotification = new FollowNotificationEvent();

        public event UnityAction<FollowNotification> OnFollowNotification
        {
            add => onFollowNotification.AddListener(value);
            remove => onFollowNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class AcceptRequestNotificationEvent : UnityEvent<AcceptRequestNotification>
        {

        }

        [SerializeField]
        private AcceptRequestNotificationEvent onAcceptRequestNotification = new AcceptRequestNotificationEvent();

        public event UnityAction<AcceptRequestNotification> OnAcceptRequestNotification
        {
            add => onAcceptRequestNotification.AddListener(value);
            remove => onAcceptRequestNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class RejectRequestNotificationEvent : UnityEvent<RejectRequestNotification>
        {

        }

        [SerializeField]
        private RejectRequestNotificationEvent onRejectRequestNotification = new RejectRequestNotificationEvent();

        public event UnityAction<RejectRequestNotification> OnRejectRequestNotification
        {
            add => onRejectRequestNotification.AddListener(value);
            remove => onRejectRequestNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class DeleteFriendNotificationEvent : UnityEvent<DeleteFriendNotification>
        {

        }

        [SerializeField]
        private DeleteFriendNotificationEvent onDeleteFriendNotification = new DeleteFriendNotificationEvent();

        public event UnityAction<DeleteFriendNotification> OnDeleteFriendNotification
        {
            add => onDeleteFriendNotification.AddListener(value);
            remove => onDeleteFriendNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class ReceiveRequestNotificationEvent : UnityEvent<ReceiveRequestNotification>
        {

        }

        [SerializeField]
        private ReceiveRequestNotificationEvent onReceiveRequestNotification = new ReceiveRequestNotificationEvent();

        public event UnityAction<ReceiveRequestNotification> OnReceiveRequestNotification
        {
            add => onReceiveRequestNotification.AddListener(value);
            remove => onReceiveRequestNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class CancelRequestNotificationEvent : UnityEvent<CancelRequestNotification>
        {

        }

        [SerializeField]
        private CancelRequestNotificationEvent onCancelRequestNotification = new CancelRequestNotificationEvent();

        public event UnityAction<CancelRequestNotification> OnCancelRequestNotification
        {
            add => onCancelRequestNotification.AddListener(value);
            remove => onCancelRequestNotification.RemoveListener(value);
        }
    #endif

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
            switch (action) {
                case "FollowNotification": {
    #if UNITY_2017_1_OR_NEWER
                    onFollowNotification.Invoke(FollowNotification.FromJson(JsonMapper.ToObject(payload)));
    #endif
                    break;
                }
                case "AcceptRequestNotification": {
                    var notification = AcceptRequestNotification.FromJson(JsonMapper.ToObject(payload));
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.Friend>(
                        (null as Gs2.Gs2Friend.Model.Friend).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId
                        )
                    );
    #if UNITY_2017_1_OR_NEWER
                    onAcceptRequestNotification.Invoke(notification);
    #endif
                    break;
                }
                case "RejectRequestNotification": {
                    var notification = RejectRequestNotification.FromJson(JsonMapper.ToObject(payload));
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.Friend>(
                        (null as Gs2.Gs2Friend.Model.Friend).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId
                        )
                    );
    #if UNITY_2017_1_OR_NEWER
                    onRejectRequestNotification.Invoke(notification);
    #endif
                    break;
                }
                case "DeleteFriendNotification": {
                    var notification = DeleteFriendNotification.FromJson(JsonMapper.ToObject(payload));
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.Friend>(
                        (null as Gs2.Gs2Friend.Model.Friend).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId
                        )
                    );
                    break;
                }
                case "ReceiveRequestNotification": {
                    var notification = ReceiveRequestNotification.FromJson(JsonMapper.ToObject(payload));
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId
                        )
                    );
    #if UNITY_2017_1_OR_NEWER
                    onReceiveRequestNotification.Invoke(notification);
    #endif
                    break;
                }
                case "CancelRequest": {
    #if UNITY_2017_1_OR_NEWER
                    onCancelRequestNotification.Invoke(CancelRequestNotification.FromJson(JsonMapper.ToObject(payload)));
    #endif
                    break;
                }
            }
        }
    }
}

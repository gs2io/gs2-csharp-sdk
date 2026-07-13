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
using Gs2.Gs2Matchmaking.Domain.Iterator;
using Gs2.Gs2Matchmaking.Model.Cache;
using Gs2.Gs2Matchmaking.Domain.Model;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Matchmaking.Model;
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

namespace Gs2.Gs2Matchmaking.Domain
{

    public class Gs2Matchmaking {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2Matchmaking(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) => CreateNamespaceAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Matchmaking> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) => DumpUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Matchmaking> DumpUserDataAsync(
        #else
        public async Task<Gs2Matchmaking> DumpUserDataAsync(
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
        public IFuture<Gs2Matchmaking> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) => CheckDumpUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Matchmaking> CheckDumpUserDataAsync(
        #else
        public async Task<Gs2Matchmaking> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Matchmaking> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) => CleanUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Matchmaking> CleanUserDataAsync(
        #else
        public async Task<Gs2Matchmaking> CleanUserDataAsync(
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
        public IFuture<Gs2Matchmaking> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) => CheckCleanUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Matchmaking> CheckCleanUserDataAsync(
        #else
        public async Task<Gs2Matchmaking> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Matchmaking> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) => PrepareImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Matchmaking> PrepareImportUserDataAsync(
        #else
        public async Task<Gs2Matchmaking> PrepareImportUserDataAsync(
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
        public IFuture<Gs2Matchmaking> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) => ImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Matchmaking> ImportUserDataAsync(
        #else
        public async Task<Gs2Matchmaking> ImportUserDataAsync(
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
        public IFuture<Gs2Matchmaking> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) => CheckImportUserDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2Matchmaking> CheckImportUserDataAsync(
        #else
        public async Task<Gs2Matchmaking> CheckImportUserDataAsync(
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
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Namespace> Namespaces(
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
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Namespace> NamespacesAsync(
            #else
        public DescribeNamespacesIterator NamespacesAsync(
            #endif
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client
            );
        }
        #endif

        public ulong SubscribeNamespaces(
            Action<Gs2.Gs2Matchmaking.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Namespace>(
                (null as Gs2.Gs2Matchmaking.Model.Namespace).CacheParentKey(
                    null
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
            Action<Gs2.Gs2Matchmaking.Model.Namespace[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Namespace>(
                (null as Gs2.Gs2Matchmaking.Model.Namespace).CacheParentKey(
                    null
                ),
                callbackId
            );
        }

        public void InvalidateNamespaces(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Matchmaking.Model.Namespace>(
                (null as Gs2.Gs2Matchmaking.Model.Namespace).CacheParentKey(
                    null
                )
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

        public void UpdateCacheFromStampSheet(
                string transactionId,
                int? timeOffset,
                string method,
                string request,
                string result
        ) {
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
        }
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class JoinNotificationEvent : UnityEvent<JoinNotification>
        {

        }

        [SerializeField]
        private JoinNotificationEvent onJoinNotification = new JoinNotificationEvent();

        public event UnityAction<JoinNotification> OnJoinNotification
        {
            add => onJoinNotification.AddListener(value);
            remove => onJoinNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class LeaveNotificationEvent : UnityEvent<LeaveNotification>
        {

        }

        [SerializeField]
        private LeaveNotificationEvent onLeaveNotification = new LeaveNotificationEvent();

        public event UnityAction<LeaveNotification> OnLeaveNotification
        {
            add => onLeaveNotification.AddListener(value);
            remove => onLeaveNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class CompleteNotificationEvent : UnityEvent<CompleteNotification>
        {

        }

        [SerializeField]
        private CompleteNotificationEvent onCompleteNotification = new CompleteNotificationEvent();

        public event UnityAction<CompleteNotification> OnCompleteNotification
        {
            add => onCompleteNotification.AddListener(value);
            remove => onCompleteNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class ChangeRatingNotificationEvent : UnityEvent<ChangeRatingNotification>
        {

        }

        [SerializeField]
        private ChangeRatingNotificationEvent onChangeRatingNotification = new ChangeRatingNotificationEvent();

        public event UnityAction<ChangeRatingNotification> OnChangeRatingNotification
        {
            add => onChangeRatingNotification.AddListener(value);
            remove => onChangeRatingNotification.RemoveListener(value);
        }
    #endif

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
            switch (action) {
                case "Join": {
    #if UNITY_2017_1_OR_NEWER
                    onJoinNotification.Invoke(JoinNotification.FromJson(JsonMapper.ToObject(payload)));
    #endif
                    break;
                }
                case "Leave": {
    #if UNITY_2017_1_OR_NEWER
                    onLeaveNotification.Invoke(LeaveNotification.FromJson(JsonMapper.ToObject(payload)));
    #endif
                    break;
                }
                case "Complete": {
    #if UNITY_2017_1_OR_NEWER
                    onCompleteNotification.Invoke(CompleteNotification.FromJson(JsonMapper.ToObject(payload)));
    #endif
                    break;
                }
                case "ChangeRatingNotification": {
                    var notification = ChangeRatingNotification.FromJson(JsonMapper.ToObject(payload));
                    _gs2.Cache.ClearListCache<Gs2.Gs2Matchmaking.Model.Rating>(
                        (null as Gs2.Gs2Matchmaking.Model.Rating).CacheParentKey(
                            notification.NamespaceName,
                            notification.UserId,
                            null
                        )
                    );
    #if UNITY_2017_1_OR_NEWER
                    onChangeRatingNotification.Invoke(notification);
    #endif
                    break;
                }
            }
        }
    }
}

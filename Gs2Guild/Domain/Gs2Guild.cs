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
using Gs2.Gs2Guild.Domain.Iterator;
using Gs2.Gs2Guild.Model.Cache;
using Gs2.Gs2Guild.Domain.Model;
using Gs2.Gs2Guild.Request;
using Gs2.Gs2Guild.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Guild.Model;
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

namespace Gs2.Gs2Guild.Domain
{

    public class Gs2Guild {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GuildRestClient _client;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;

        public Gs2Guild(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GuildRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Guild.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Guild.Domain.Model.NamespaceDomain> self)
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
                var domain = new Gs2.Gs2Guild.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Guild.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Guild.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Guild.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Guild.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2Guild> DumpUserDataFuture(
            DumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Guild> self)
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
            return new Gs2InlineFuture<Gs2Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Guild> DumpUserDataAsync(
            #else
        public async Task<Gs2Guild> DumpUserDataAsync(
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
        public IFuture<Gs2Guild> CheckDumpUserDataFuture(
            CheckDumpUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Guild> self)
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
            return new Gs2InlineFuture<Gs2Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Guild> CheckDumpUserDataAsync(
            #else
        public async Task<Gs2Guild> CheckDumpUserDataAsync(
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
        public IFuture<Gs2Guild> CleanUserDataFuture(
            CleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Guild> self)
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
            return new Gs2InlineFuture<Gs2Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Guild> CleanUserDataAsync(
            #else
        public async Task<Gs2Guild> CleanUserDataAsync(
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
        public IFuture<Gs2Guild> CheckCleanUserDataFuture(
            CheckCleanUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Guild> self)
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
            return new Gs2InlineFuture<Gs2Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Guild> CheckCleanUserDataAsync(
            #else
        public async Task<Gs2Guild> CheckCleanUserDataAsync(
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
        public IFuture<Gs2Guild> PrepareImportUserDataFuture(
            PrepareImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Guild> self)
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
            return new Gs2InlineFuture<Gs2Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Guild> PrepareImportUserDataAsync(
            #else
        public async Task<Gs2Guild> PrepareImportUserDataAsync(
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
        public IFuture<Gs2Guild> ImportUserDataFuture(
            ImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Guild> self)
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
            return new Gs2InlineFuture<Gs2Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Guild> ImportUserDataAsync(
            #else
        public async Task<Gs2Guild> ImportUserDataAsync(
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
        public IFuture<Gs2Guild> CheckImportUserDataFuture(
            CheckImportUserDataByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2Guild> self)
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
            return new Gs2InlineFuture<Gs2Guild>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2Guild> CheckImportUserDataAsync(
            #else
        public async Task<Gs2Guild> CheckImportUserDataAsync(
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
        public Gs2Iterator<Gs2.Gs2Guild.Model.Namespace> Namespaces(
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
        public IUniTaskAsyncEnumerable<Gs2.Gs2Guild.Model.Namespace> NamespacesAsync(
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
            Action<Gs2.Gs2Guild.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Guild.Model.Namespace>(
                (null as Gs2.Gs2Guild.Model.Namespace).CacheParentKey(
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2Guild.Model.Namespace[]> callback
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
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Guild.Model.Namespace>(
                (null as Gs2.Gs2Guild.Model.Namespace).CacheParentKey(
                ),
                callbackId
            );
        }

        public Gs2.Gs2Guild.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Guild.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest, IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> IncreaseMaximumCurrentMaximumMemberCountByGuildNameComplete = new UnityEvent<string, IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest, IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult>();
    #else
        public static Action<string, IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest, IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> IncreaseMaximumCurrentMaximumMemberCountByGuildNameComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetMaximumCurrentMaximumMemberCountByGuildNameRequest, SetMaximumCurrentMaximumMemberCountByGuildNameResult> SetMaximumCurrentMaximumMemberCountByGuildNameComplete = new UnityEvent<string, SetMaximumCurrentMaximumMemberCountByGuildNameRequest, SetMaximumCurrentMaximumMemberCountByGuildNameResult>();
    #else
        public static Action<string, SetMaximumCurrentMaximumMemberCountByGuildNameRequest, SetMaximumCurrentMaximumMemberCountByGuildNameResult> SetMaximumCurrentMaximumMemberCountByGuildNameComplete;
    #endif

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "IncreaseMaximumCurrentMaximumMemberCountByGuildName": {
                        var requestModel = IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.GuildName,
                            requestModel
                        );

                        IncreaseMaximumCurrentMaximumMemberCountByGuildNameComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetMaximumCurrentMaximumMemberCountByGuildName": {
                        var requestModel = SetMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.GuildName,
                            requestModel
                        );

                        SetMaximumCurrentMaximumMemberCountByGuildNameComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest, DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> DecreaseMaximumCurrentMaximumMemberCountByGuildNameComplete = new UnityEvent<string, DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest, DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult>();
    #else
        public static Action<string, DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest, DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> DecreaseMaximumCurrentMaximumMemberCountByGuildNameComplete;
    #endif

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "DecreaseMaximumCurrentMaximumMemberCountByGuildName": {
                        var requestModel = DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(JsonMapper.ToObject(result));

                        resultModel.PutCache(
                            _gs2.Cache,
                            requestModel.GuildName,
                            requestModel
                        );

                        DecreaseMaximumCurrentMaximumMemberCountByGuildNameComplete?.Invoke(
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
                case "increase_maximum_current_maximum_member_count_by_guild_name": {
                    var requestModel = IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.GuildName,
                        requestModel
                    );

                    IncreaseMaximumCurrentMaximumMemberCountByGuildNameComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_maximum_current_maximum_member_count_by_guild_name": {
                    var requestModel = SetMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(JsonMapper.ToObject(result.Result));

                    resultModel.PutCache(
                        _gs2.Cache,
                        requestModel.GuildName,
                        requestModel
                    );

                    SetMaximumCurrentMaximumMemberCountByGuildNameComplete?.Invoke(
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
        private class RemoveRequestNotificationEvent : UnityEvent<RemoveRequestNotification>
        {

        }

        [SerializeField]
        private RemoveRequestNotificationEvent onRemoveRequestNotification = new RemoveRequestNotificationEvent();

        public event UnityAction<RemoveRequestNotification> OnRemoveRequestNotification
        {
            add => onRemoveRequestNotification.AddListener(value);
            remove => onRemoveRequestNotification.RemoveListener(value);
        }
    #endif
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class ChangeNotificationEvent : UnityEvent<ChangeNotification>
        {

        }

        [SerializeField]
        private ChangeNotificationEvent onChangeNotification = new ChangeNotificationEvent();

        public event UnityAction<ChangeNotification> OnChangeNotification
        {
            add => onChangeNotification.AddListener(value);
            remove => onChangeNotification.RemoveListener(value);
        }
    #endif
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
        private class ChangeMemberNotificationEvent : UnityEvent<ChangeMemberNotification>
        {

        }

        [SerializeField]
        private ChangeMemberNotificationEvent onChangeMemberNotification = new ChangeMemberNotificationEvent();

        public event UnityAction<ChangeMemberNotification> OnChangeMemberNotification
        {
            add => onChangeMemberNotification.AddListener(value);
            remove => onChangeMemberNotification.RemoveListener(value);
        }
    #endif

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
            switch (action) {
                case "ReceiveRequestNotification": {
                    var notification = ReceiveRequestNotification.FromJson(JsonMapper.ToObject(payload));
                    _gs2.Cache.ClearListCache<ReceiveMemberRequest>(
                        (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                            notification.NamespaceName,
                            notification.GuildModelName,
                            notification.GuildName
                        )
                    );
    #if UNITY_2017_1_OR_NEWER
                    onReceiveRequestNotification.Invoke(notification);
    #endif
                    break;
                }
                case "RemoveRequestNotification": {
                    var notification = RemoveRequestNotification.FromJson(JsonMapper.ToObject(payload));
                    (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).DeleteCache(
                        _gs2.Cache,
                        notification.NamespaceName,
                        notification.GuildModelName,
                        notification.GuildName,
                        notification.FromUserId
                    );
                    (null as Gs2.Gs2Guild.Model.SendMemberRequest).DeleteCache(
                        _gs2.Cache,
                        notification.NamespaceName,
                        notification.FromUserId,
                        notification.GuildModelName,
                        notification.GuildName
                    );
                    _gs2.Cache.ClearListCache<ReceiveMemberRequest>(
                        (null as Gs2.Gs2Guild.Model.ReceiveMemberRequest).CacheParentKey(
                            notification.NamespaceName,
                            notification.GuildModelName,
                            notification.GuildName
                        )
                    );
                    _gs2.Cache.ClearListCache<SendMemberRequest>(
                        (null as Gs2.Gs2Guild.Model.SendMemberRequest).CacheParentKey(
                            notification.NamespaceName,
                            notification.GuildModelName,
                            notification.FromUserId
                        )
                    );
    #if UNITY_2017_1_OR_NEWER
                    onRemoveRequestNotification.Invoke(notification);
    #endif
                    break;
                }
                case "ChangeNotification": {
    #if UNITY_2017_1_OR_NEWER
                    onChangeNotification.Invoke(ChangeNotification.FromJson(JsonMapper.ToObject(payload)));
    #endif
                    break;
                }
                case "JoinNotification": {
                    var notification = JoinNotification.FromJson(JsonMapper.ToObject(payload));
                    (null as Gs2.Gs2Guild.Model.Guild).DeleteCache(
                        _gs2.Cache,
                        notification.NamespaceName,
                        notification.GuildModelName,
                        notification.GuildName
                    );
    #if UNITY_2017_1_OR_NEWER
                    onJoinNotification.Invoke(notification);
    #endif
                    break;
                }
                case "LeaveNotification": {
                    var notification = LeaveNotification.FromJson(JsonMapper.ToObject(payload));
                    (null as Gs2.Gs2Guild.Model.Guild).DeleteCache(
                        _gs2.Cache,
                        notification.NamespaceName,
                        notification.GuildModelName,
                        notification.GuildName
                    );
    #if UNITY_2017_1_OR_NEWER
                    onLeaveNotification.Invoke(notification);
    #endif
                    break;
                }
                case "ChangeMemberNotification": {
                    var notification = ChangeMemberNotification.FromJson(JsonMapper.ToObject(payload));
                    (null as Gs2.Gs2Guild.Model.Guild).DeleteCache(
                        _gs2.Cache,
                        notification.NamespaceName,
                        notification.GuildModelName,
                        notification.GuildName
                    );
    #if UNITY_2017_1_OR_NEWER
                    onChangeMemberNotification.Invoke(notification);
    #endif
                    break;
                }
            }
        }
    }
}

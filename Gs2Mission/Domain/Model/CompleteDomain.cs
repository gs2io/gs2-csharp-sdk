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
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Mission.Domain.Iterator;
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Mission.Domain.Model
{

    public partial class CompleteDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MissionRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _missionGroupName;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string MissionGroupName => _missionGroupName;

        public CompleteDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string missionGroupName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2MissionRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._missionGroupName = missionGroupName;
            this._parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Complete"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string missionGroupName,
            string childType
        )
        {
            return string.Join(
                ":",
                "mission",
                namespaceName ?? "null",
                userId ?? "null",
                missionGroupName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string missionGroupName
        )
        {
            return string.Join(
                ":",
                missionGroupName ?? "null"
            );
        }

    }

    public partial class CompleteDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> CompleteFuture(
            CompleteByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.CompleteByUserIdFuture(
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                CompleteByUserIdResult result = null;
                    result = await this._client.CompleteByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var stampSheet = new Gs2.Core.Domain.TransactionDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    this.UserId,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId

                );
                if (result?.StampSheet != null)
                {
                    var future2 = stampSheet.Wait();
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }

            self.OnComplete(stampSheet);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        }
        #else
        public async Task<Gs2.Core.Domain.TransactionDomain> CompleteAsync(
            CompleteByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.CompleteByUserIdFuture(
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            CompleteByUserIdResult result = null;
                result = await this._client.CompleteByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
            var stampSheet = new Gs2.Core.Domain.TransactionDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.UserId,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId

            );
            if (result?.StampSheet != null)
            {
                await stampSheet.WaitAsync();
            }

            return stampSheet;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionDomain> CompleteAsync(
            CompleteByUserIdRequest request
        ) {
            var future = CompleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CompleteFuture.")]
        public IFuture<Gs2.Core.Domain.TransactionDomain> Complete(
            CompleteByUserIdRequest request
        ) {
            return CompleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> ReceiveFuture(
            ReceiveByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.ReceiveByUserIdFuture(
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                ReceiveByUserIdResult result = null;
                    result = await this._client.ReceiveByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Complete"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                            resultModel.Item.MissionGroupName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.NextResetAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.CompleteDomain> ReceiveAsync(
            ReceiveByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.ReceiveByUserIdFuture(
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            ReceiveByUserIdResult result = null;
                result = await this._client.ReceiveByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Complete"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        resultModel.Item.MissionGroupName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.NextResetAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.CompleteDomain> ReceiveAsync(
            ReceiveByUserIdRequest request
        ) {
            var future = ReceiveFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ReceiveFuture.")]
        public IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> Receive(
            ReceiveByUserIdRequest request
        ) {
            return ReceiveFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> RevertReceiveFuture(
            RevertReceiveByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.RevertReceiveByUserIdFuture(
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                RevertReceiveByUserIdResult result = null;
                    result = await this._client.RevertReceiveByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Complete"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                            resultModel.Item.MissionGroupName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.NextResetAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.CompleteDomain> RevertReceiveAsync(
            RevertReceiveByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.RevertReceiveByUserIdFuture(
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            RevertReceiveByUserIdResult result = null;
                result = await this._client.RevertReceiveByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Complete"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        resultModel.Item.MissionGroupName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.NextResetAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.CompleteDomain> RevertReceiveAsync(
            RevertReceiveByUserIdRequest request
        ) {
            var future = RevertReceiveFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to RevertReceiveFuture.")]
        public IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> RevertReceive(
            RevertReceiveByUserIdRequest request
        ) {
            return RevertReceiveFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Mission.Model.Complete> GetFuture(
            GetCompleteByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.Complete> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.GetCompleteByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                            request.MissionGroupName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "complete")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                GetCompleteByUserIdResult result = null;
                try {
                    result = await this._client.GetCompleteByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        request.MissionGroupName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "complete")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Complete"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                            resultModel.Item.MissionGroupName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.NextResetAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.Complete>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Mission.Model.Complete> GetAsync(
            GetCompleteByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.GetCompleteByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        request.MissionGroupName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "complete")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            GetCompleteByUserIdResult result = null;
            try {
                result = await this._client.GetCompleteByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                    request.MissionGroupName.ToString()
                    );
                _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "complete")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Complete"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        resultModel.Item.MissionGroupName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.NextResetAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> DeleteFuture(
            DeleteCompleteByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.DeleteCompleteByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                            request.MissionGroupName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "complete")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithMissionGroupName(this.MissionGroupName);
                DeleteCompleteByUserIdResult result = null;
                try {
                    result = await this._client.DeleteCompleteByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        request.MissionGroupName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "complete")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Complete"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                            resultModel.Item.MissionGroupName.ToString()
                        );
                        cache.Delete<Gs2.Gs2Mission.Model.Complete>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.CompleteDomain> DeleteAsync(
            DeleteCompleteByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.DeleteCompleteByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        request.MissionGroupName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "complete")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithMissionGroupName(this.MissionGroupName);
            DeleteCompleteByUserIdResult result = null;
            try {
                result = await this._client.DeleteCompleteByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                    request.MissionGroupName.ToString()
                    );
                _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "complete")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Complete"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        resultModel.Item.MissionGroupName.ToString()
                    );
                    cache.Delete<Gs2.Gs2Mission.Model.Complete>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.CompleteDomain> DeleteAsync(
            DeleteCompleteByUserIdRequest request
        ) {
            var future = DeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Mission.Domain.Model.CompleteDomain> Delete(
            DeleteCompleteByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class CompleteDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Model.Complete> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.Complete> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Mission.Model.Complete>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        this.MissionGroupName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetCompleteByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                                    this.MissionGroupName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "complete")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _cache.Get<Gs2.Gs2Mission.Model.Complete>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                            this.MissionGroupName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.Complete>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Model.Complete> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Mission.Model.Complete>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                        this.MissionGroupName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetCompleteByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                                    this.MissionGroupName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Mission.Model.Complete>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "complete")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Mission.Model.Complete>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                            this.MissionGroupName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Model.Complete> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Mission.Model.Complete> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Mission.Model.Complete> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Mission.Model.Complete> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

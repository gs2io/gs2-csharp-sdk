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
using Gs2.Gs2LoginReward.Domain.Iterator;
using Gs2.Gs2LoginReward.Request;
using Gs2.Gs2LoginReward.Result;
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

namespace Gs2.Gs2LoginReward.Domain.Model
{

    public partial class ReceiveStatusDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LoginRewardRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _bonusModelName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string BonusModelName => _bonusModelName;

        public ReceiveStatusDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string bonusModelName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LoginRewardRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._bonusModelName = bonusModelName;
            this._parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "ReceiveStatus"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string bonusModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "loginReward",
                namespaceName ?? "null",
                userId ?? "null",
                bonusModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string bonusModelName
        )
        {
            return string.Join(
                ":",
                bonusModelName ?? "null"
            );
        }

    }

    public partial class ReceiveStatusDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2LoginReward.Model.ReceiveStatus> GetFuture(
            GetReceiveStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2LoginReward.Model.ReceiveStatus> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithBonusModelName(this.BonusModelName);
                var future = this._client.GetReceiveStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                            request.BonusModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "receiveStatus")
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
                    .WithBonusModelName(this.BonusModelName);
                GetReceiveStatusByUserIdResult result = null;
                try {
                    result = await this._client.GetReceiveStatusByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        request.BonusModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "receiveStatus")
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
                        var parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "ReceiveStatus"
                        );
                        var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                            resultModel.Item.BonusModelName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.BonusModel != null) {
                        var parentKey = Gs2.Gs2LoginReward.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BonusModel"
                        );
                        var key = Gs2.Gs2LoginReward.Domain.Model.BonusModelDomain.CreateCacheKey(
                            resultModel.BonusModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.BonusModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2LoginReward.Model.ReceiveStatus>(Impl);
        }
        #else
        private async Task<Gs2.Gs2LoginReward.Model.ReceiveStatus> GetAsync(
            GetReceiveStatusByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithBonusModelName(this.BonusModelName);
            var future = this._client.GetReceiveStatusByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        request.BonusModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "receiveStatus")
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
                .WithBonusModelName(this.BonusModelName);
            GetReceiveStatusByUserIdResult result = null;
            try {
                result = await this._client.GetReceiveStatusByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                    request.BonusModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "receiveStatus")
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
                    var parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "ReceiveStatus"
                    );
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        resultModel.Item.BonusModelName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.BonusModel != null) {
                    var parentKey = Gs2.Gs2LoginReward.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BonusModel"
                    );
                    var key = Gs2.Gs2LoginReward.Domain.Model.BonusModelDomain.CreateCacheKey(
                        resultModel.BonusModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.BonusModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> DeleteFuture(
            DeleteReceiveStatusByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithBonusModelName(this.BonusModelName);
                var future = this._client.DeleteReceiveStatusByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                            request.BonusModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "receiveStatus")
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
                    .WithBonusModelName(this.BonusModelName);
                DeleteReceiveStatusByUserIdResult result = null;
                try {
                    result = await this._client.DeleteReceiveStatusByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        request.BonusModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "receiveStatus")
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
                        var parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "ReceiveStatus"
                        );
                        var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                            resultModel.Item.BonusModelName.ToString()
                        );
                        cache.Delete<Gs2.Gs2LoginReward.Model.ReceiveStatus>(parentKey, key);
                    }
                    if (resultModel.BonusModel != null) {
                        var parentKey = Gs2.Gs2LoginReward.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BonusModel"
                        );
                        var key = Gs2.Gs2LoginReward.Domain.Model.BonusModelDomain.CreateCacheKey(
                            resultModel.BonusModel.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2LoginReward.Model.BonusModel>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> DeleteAsync(
            DeleteReceiveStatusByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithBonusModelName(this.BonusModelName);
            var future = this._client.DeleteReceiveStatusByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        request.BonusModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "receiveStatus")
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
                .WithBonusModelName(this.BonusModelName);
            DeleteReceiveStatusByUserIdResult result = null;
            try {
                result = await this._client.DeleteReceiveStatusByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                    request.BonusModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "receiveStatus")
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
                    var parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "ReceiveStatus"
                    );
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        resultModel.Item.BonusModelName.ToString()
                    );
                    cache.Delete<Gs2.Gs2LoginReward.Model.ReceiveStatus>(parentKey, key);
                }
                if (resultModel.BonusModel != null) {
                    var parentKey = Gs2.Gs2LoginReward.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BonusModel"
                    );
                    var key = Gs2.Gs2LoginReward.Domain.Model.BonusModelDomain.CreateCacheKey(
                        resultModel.BonusModel.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2LoginReward.Model.BonusModel>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> DeleteAsync(
            DeleteReceiveStatusByUserIdRequest request
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
        public IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> Delete(
            DeleteReceiveStatusByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> MarkReceivedFuture(
            MarkReceivedByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithBonusModelName(this.BonusModelName);
                var future = this._client.MarkReceivedByUserIdFuture(
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
                    .WithBonusModelName(this.BonusModelName);
                MarkReceivedByUserIdResult result = null;
                    result = await this._client.MarkReceivedByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "ReceiveStatus"
                        );
                        var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                            resultModel.Item.BonusModelName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.BonusModel != null) {
                        var parentKey = Gs2.Gs2LoginReward.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BonusModel"
                        );
                        var key = Gs2.Gs2LoginReward.Domain.Model.BonusModelDomain.CreateCacheKey(
                            resultModel.BonusModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.BonusModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> MarkReceivedAsync(
            MarkReceivedByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithBonusModelName(this.BonusModelName);
            var future = this._client.MarkReceivedByUserIdFuture(
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
                .WithBonusModelName(this.BonusModelName);
            MarkReceivedByUserIdResult result = null;
                result = await this._client.MarkReceivedByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "ReceiveStatus"
                    );
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        resultModel.Item.BonusModelName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.BonusModel != null) {
                    var parentKey = Gs2.Gs2LoginReward.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BonusModel"
                    );
                    var key = Gs2.Gs2LoginReward.Domain.Model.BonusModelDomain.CreateCacheKey(
                        resultModel.BonusModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.BonusModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> MarkReceivedAsync(
            MarkReceivedByUserIdRequest request
        ) {
            var future = MarkReceivedFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to MarkReceivedFuture.")]
        public IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> MarkReceived(
            MarkReceivedByUserIdRequest request
        ) {
            return MarkReceivedFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> UnmarkReceivedFuture(
            UnmarkReceivedByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithBonusModelName(this.BonusModelName);
                var future = this._client.UnmarkReceivedByUserIdFuture(
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
                    .WithBonusModelName(this.BonusModelName);
                UnmarkReceivedByUserIdResult result = null;
                    result = await this._client.UnmarkReceivedByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "ReceiveStatus"
                        );
                        var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                            resultModel.Item.BonusModelName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.BonusModel != null) {
                        var parentKey = Gs2.Gs2LoginReward.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BonusModel"
                        );
                        var key = Gs2.Gs2LoginReward.Domain.Model.BonusModelDomain.CreateCacheKey(
                            resultModel.BonusModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.BonusModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> UnmarkReceivedAsync(
            UnmarkReceivedByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithBonusModelName(this.BonusModelName);
            var future = this._client.UnmarkReceivedByUserIdFuture(
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
                .WithBonusModelName(this.BonusModelName);
            UnmarkReceivedByUserIdResult result = null;
                result = await this._client.UnmarkReceivedByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2LoginReward.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "ReceiveStatus"
                    );
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        resultModel.Item.BonusModelName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.BonusModel != null) {
                    var parentKey = Gs2.Gs2LoginReward.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BonusModel"
                    );
                    var key = Gs2.Gs2LoginReward.Domain.Model.BonusModelDomain.CreateCacheKey(
                        resultModel.BonusModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.BonusModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> UnmarkReceivedAsync(
            UnmarkReceivedByUserIdRequest request
        ) {
            var future = UnmarkReceivedFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UnmarkReceivedFuture.")]
        public IFuture<Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain> UnmarkReceived(
            UnmarkReceivedByUserIdRequest request
        ) {
            return UnmarkReceivedFuture(request);
        }
        #endif

    }

    public partial class ReceiveStatusDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2LoginReward.Model.ReceiveStatus> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2LoginReward.Model.ReceiveStatus> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                    _parentKey,
                    Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        this.BonusModelName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetReceiveStatusByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                                    this.BonusModelName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "receiveStatus")
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
                    (value, _) = _cache.Get<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                        _parentKey,
                        Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                            this.BonusModelName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2LoginReward.Model.ReceiveStatus>(Impl);
        }
        #else
        public async Task<Gs2.Gs2LoginReward.Model.ReceiveStatus> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                    _parentKey,
                    Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                        this.BonusModelName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetReceiveStatusByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                                    this.BonusModelName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "receiveStatus")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2LoginReward.Model.ReceiveStatus>(
                        _parentKey,
                        Gs2.Gs2LoginReward.Domain.Model.ReceiveStatusDomain.CreateCacheKey(
                            this.BonusModelName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2LoginReward.Model.ReceiveStatus> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2LoginReward.Model.ReceiveStatus> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2LoginReward.Model.ReceiveStatus> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2LoginReward.Model.ReceiveStatus> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

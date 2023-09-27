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
using Gs2.Gs2Ranking.Domain.Iterator;
using Gs2.Gs2Ranking.Request;
using Gs2.Gs2Ranking.Result;
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

namespace Gs2.Gs2Ranking.Domain.Model
{

    public partial class SubscribeUserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _categoryName;
        private readonly string _targetUserId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string CategoryName => _categoryName;
        public string TargetUserId => _targetUserId;

        public SubscribeUserDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string categoryName,
            string targetUserId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2RankingRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._categoryName = categoryName;
            this._targetUserId = targetUserId;
            this._parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "SubscribeUser"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string categoryName,
            string targetUserId,
            string childType
        )
        {
            return string.Join(
                ":",
                "ranking",
                namespaceName ?? "null",
                userId ?? "null",
                categoryName ?? "null",
                targetUserId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string categoryName,
            string targetUserId
        )
        {
            return string.Join(
                ":",
                categoryName ?? "null",
                targetUserId ?? "null"
            );
        }

    }

    public partial class SubscribeUserDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Ranking.Model.SubscribeUser> GetFuture(
            GetSubscribeByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.SubscribeUser> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithCategoryName(this.CategoryName)
                    .WithTargetUserId(this.TargetUserId);
                var future = this._client.GetSubscribeByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                            request.CategoryName.ToString(),
                            request.TargetUserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "subscribeUser")
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
                    .WithCategoryName(this.CategoryName)
                    .WithTargetUserId(this.TargetUserId);
                GetSubscribeByUserIdResult result = null;
                try {
                    result = await this._client.GetSubscribeByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        request.CategoryName.ToString(),
                        request.TargetUserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "subscribeUser")
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
                        var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "SubscribeUser"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                            resultModel.Item.CategoryName.ToString(),
                            resultModel.Item.TargetUserId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.SubscribeUser>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Ranking.Model.SubscribeUser> GetAsync(
            GetSubscribeByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithCategoryName(this.CategoryName)
                .WithTargetUserId(this.TargetUserId);
            var future = this._client.GetSubscribeByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        request.CategoryName.ToString(),
                        request.TargetUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "subscribeUser")
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
                .WithCategoryName(this.CategoryName)
                .WithTargetUserId(this.TargetUserId);
            GetSubscribeByUserIdResult result = null;
            try {
                result = await this._client.GetSubscribeByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                    request.CategoryName.ToString(),
                    request.TargetUserId.ToString()
                    );
                _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "subscribeUser")
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
                    var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SubscribeUser"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        resultModel.Item.CategoryName.ToString(),
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> UnsubscribeFuture(
            UnsubscribeByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithCategoryName(this.CategoryName)
                    .WithTargetUserId(this.TargetUserId);
                var future = this._client.UnsubscribeByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                            request.CategoryName.ToString(),
                            request.TargetUserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "subscribeUser")
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
                    .WithCategoryName(this.CategoryName)
                    .WithTargetUserId(this.TargetUserId);
                UnsubscribeByUserIdResult result = null;
                try {
                    result = await this._client.UnsubscribeByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        request.CategoryName.ToString(),
                        request.TargetUserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "subscribeUser")
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
                        var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "SubscribeUser"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                            resultModel.Item.CategoryName.ToString(),
                            resultModel.Item.TargetUserId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Ranking.Model.SubscribeUser>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> UnsubscribeAsync(
            UnsubscribeByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithCategoryName(this.CategoryName)
                .WithTargetUserId(this.TargetUserId);
            var future = this._client.UnsubscribeByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        request.CategoryName.ToString(),
                        request.TargetUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "subscribeUser")
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
                .WithCategoryName(this.CategoryName)
                .WithTargetUserId(this.TargetUserId);
            UnsubscribeByUserIdResult result = null;
            try {
                result = await this._client.UnsubscribeByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                    request.CategoryName.ToString(),
                    request.TargetUserId.ToString()
                    );
                _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "subscribeUser")
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
                    var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SubscribeUser"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        resultModel.Item.CategoryName.ToString(),
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Ranking.Model.SubscribeUser>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> UnsubscribeAsync(
            UnsubscribeByUserIdRequest request
        ) {
            var future = UnsubscribeFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UnsubscribeFuture.")]
        public IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> Unsubscribe(
            UnsubscribeByUserIdRequest request
        ) {
            return UnsubscribeFuture(request);
        }
        #endif

    }

    public partial class SubscribeUserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Model.SubscribeUser> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.SubscribeUser> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Ranking.Model.SubscribeUser>(
                    _parentKey,
                    Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        this.CategoryName?.ToString(),
                        this.TargetUserId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetSubscribeByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                                    this.CategoryName?.ToString(),
                                    this.TargetUserId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "subscribeUser")
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
                    (value, _) = _cache.Get<Gs2.Gs2Ranking.Model.SubscribeUser>(
                        _parentKey,
                        Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                            this.CategoryName?.ToString(),
                            this.TargetUserId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.SubscribeUser>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Ranking.Model.SubscribeUser> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Ranking.Model.SubscribeUser>(
                    _parentKey,
                    Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        this.CategoryName?.ToString(),
                        this.TargetUserId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetSubscribeByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                                    this.CategoryName?.ToString(),
                                    this.TargetUserId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Ranking.Model.SubscribeUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "subscribeUser")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Ranking.Model.SubscribeUser>(
                        _parentKey,
                        Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                            this.CategoryName?.ToString(),
                            this.TargetUserId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Model.SubscribeUser> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Ranking.Model.SubscribeUser> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Ranking.Model.SubscribeUser> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Ranking.Model.SubscribeUser> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Ranking.Model.SubscribeUser> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                    this.CategoryName.ToString(),
                    this.TargetUserId.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Ranking.Model.SubscribeUser>(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                    this.CategoryName.ToString(),
                    this.TargetUserId.ToString()
                ),
                callbackId
            );
        }

    }
}

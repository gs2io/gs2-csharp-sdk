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
using Gs2.Gs2Matchmaking.Domain.Iterator;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
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

namespace Gs2.Gs2Matchmaking.Domain.Model
{

    public partial class GatheringDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _gatheringName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string GatheringName => _gatheringName;

        public GatheringDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string gatheringName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2MatchmakingRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._gatheringName = gatheringName;
            this._parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Singleton",
                "Gathering"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string gatheringName,
            string childType
        )
        {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName ?? "null",
                userId ?? "null",
                gatheringName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string gatheringName
        )
        {
            return string.Join(
                ":",
                gatheringName ?? "null"
            );
        }

    }

    public partial class GatheringDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> UpdateFuture(
            UpdateGatheringByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGatheringName(this.GatheringName);
                var future = this._client.UpdateGatheringByUserIdFuture(
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
                    .WithGatheringName(this.GatheringName);
                UpdateGatheringByUserIdResult result = null;
                    result = await this._client.UpdateGatheringByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            "Gathering"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> UpdateAsync(
            UpdateGatheringByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGatheringName(this.GatheringName);
            var future = this._client.UpdateGatheringByUserIdFuture(
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
                .WithGatheringName(this.GatheringName);
            UpdateGatheringByUserIdResult result = null;
                result = await this._client.UpdateGatheringByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        "Gathering"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> UpdateAsync(
            UpdateGatheringByUserIdRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> Update(
            UpdateGatheringByUserIdRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Matchmaking.Model.Gathering> GetFuture(
            GetGatheringRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.Gathering> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithGatheringName(this.GatheringName);
                var future = this._client.GetGatheringFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            request.GatheringName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "gathering")
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
                    .WithGatheringName(this.GatheringName);
                GetGatheringResult result = null;
                try {
                    result = await this._client.GetGatheringAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        request.GatheringName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "gathering")
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
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            "Gathering"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Model.Gathering>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Matchmaking.Model.Gathering> GetAsync(
            GetGatheringRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithGatheringName(this.GatheringName);
            var future = this._client.GetGatheringFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        request.GatheringName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "gathering")
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
                .WithGatheringName(this.GatheringName);
            GetGatheringResult result = null;
            try {
                result = await this._client.GetGatheringAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                    request.GatheringName.ToString()
                    );
                _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "gathering")
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
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        "Gathering"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CancelMatchmakingFuture(
            CancelMatchmakingByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithGatheringName(this.GatheringName);
                var future = this._client.CancelMatchmakingByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            request.GatheringName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "gathering")
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
                    .WithGatheringName(this.GatheringName);
                CancelMatchmakingByUserIdResult result = null;
                try {
                    result = await this._client.CancelMatchmakingByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        request.GatheringName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "gathering")
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
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Singleton",
                            "Gathering"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Matchmaking.Model.Gathering>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CancelMatchmakingAsync(
            CancelMatchmakingByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithGatheringName(this.GatheringName);
            var future = this._client.CancelMatchmakingByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        request.GatheringName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "gathering")
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
                .WithGatheringName(this.GatheringName);
            CancelMatchmakingByUserIdResult result = null;
            try {
                result = await this._client.CancelMatchmakingByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                    request.GatheringName.ToString()
                    );
                _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "gathering")
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
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Singleton",
                        "Gathering"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Matchmaking.Model.Gathering>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CancelMatchmakingAsync(
            CancelMatchmakingByUserIdRequest request
        ) {
            var future = CancelMatchmakingFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CancelMatchmakingFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CancelMatchmaking(
            CancelMatchmakingByUserIdRequest request
        ) {
            return CancelMatchmakingFuture(request);
        }
        #endif

    }

    public partial class GatheringDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Model.Gathering> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.Gathering> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Matchmaking.Model.Gathering>(
                    _parentKey,
                    Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        this.GatheringName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetGatheringRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                                    this.GatheringName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "gathering")
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
                    (value, _) = _cache.Get<Gs2.Gs2Matchmaking.Model.Gathering>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            this.GatheringName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Model.Gathering>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Matchmaking.Model.Gathering> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Matchmaking.Model.Gathering>(
                    _parentKey,
                    Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        this.GatheringName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetGatheringRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                                    this.GatheringName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Matchmaking.Model.Gathering>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "gathering")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Matchmaking.Model.Gathering>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                            this.GatheringName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Model.Gathering> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Matchmaking.Model.Gathering> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Model.Gathering> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Matchmaking.Model.Gathering> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Matchmaking.Model.Gathering> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                    this.GatheringName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                    this.GatheringName.ToString()
                ),
                callbackId
            );
        }

    }
}

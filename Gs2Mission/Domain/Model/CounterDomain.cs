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

    public partial class CounterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MissionRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _counterName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string CounterName => _counterName;

        public CounterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string counterName
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
            this._counterName = counterName;
            this._parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Counter"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string counterName,
            string childType
        )
        {
            return string.Join(
                ":",
                "mission",
                namespaceName ?? "null",
                userId ?? "null",
                counterName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string counterName
        )
        {
            return string.Join(
                ":",
                counterName ?? "null"
            );
        }

    }

    public partial class CounterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> IncreaseFuture(
            IncreaseCounterByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithCounterName(this.CounterName);
                var future = this._client.IncreaseCounterByUserIdFuture(
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
                    .WithCounterName(this.CounterName);
                IncreaseCounterByUserIdResult result = null;
                    result = await this._client.IncreaseCounterByUserIdAsync(
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
                            "Counter"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                        cache.ClearListCache<Gs2.Gs2Mission.Model.Complete>(
                            parentKey.Replace("Counter", "Complete")
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.CounterDomain> IncreaseAsync(
            IncreaseCounterByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithCounterName(this.CounterName);
            var future = this._client.IncreaseCounterByUserIdFuture(
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
                .WithCounterName(this.CounterName);
            IncreaseCounterByUserIdResult result = null;
                result = await this._client.IncreaseCounterByUserIdAsync(
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
                        "Counter"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    cache.ClearListCache<Gs2.Gs2Mission.Model.Complete>(
                        parentKey.Replace("Counter", "Complete")
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.CounterDomain> IncreaseAsync(
            IncreaseCounterByUserIdRequest request
        ) {
            var future = IncreaseFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to IncreaseFuture.")]
        public IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> Increase(
            IncreaseCounterByUserIdRequest request
        ) {
            return IncreaseFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> DecreaseFuture(
            DecreaseCounterByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithCounterName(this.CounterName);
                var future = this._client.DecreaseCounterByUserIdFuture(
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
                    .WithCounterName(this.CounterName);
                DecreaseCounterByUserIdResult result = null;
                    result = await this._client.DecreaseCounterByUserIdAsync(
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
                            "Counter"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.CounterDomain> DecreaseAsync(
            DecreaseCounterByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithCounterName(this.CounterName);
            var future = this._client.DecreaseCounterByUserIdFuture(
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
                .WithCounterName(this.CounterName);
            DecreaseCounterByUserIdResult result = null;
                result = await this._client.DecreaseCounterByUserIdAsync(
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
                        "Counter"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.CounterDomain> DecreaseAsync(
            DecreaseCounterByUserIdRequest request
        ) {
            var future = DecreaseFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DecreaseFuture.")]
        public IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> Decrease(
            DecreaseCounterByUserIdRequest request
        ) {
            return DecreaseFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Mission.Model.Counter> GetFuture(
            GetCounterByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.Counter> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithCounterName(this.CounterName);
                var future = this._client.GetCounterByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                            request.CounterName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "counter")
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
                    .WithCounterName(this.CounterName);
                GetCounterByUserIdResult result = null;
                try {
                    result = await this._client.GetCounterByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                        request.CounterName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "counter")
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
                            "Counter"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
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
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.Counter>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Mission.Model.Counter> GetAsync(
            GetCounterByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithCounterName(this.CounterName);
            var future = this._client.GetCounterByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                        request.CounterName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "counter")
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
                .WithCounterName(this.CounterName);
            GetCounterByUserIdResult result = null;
            try {
                result = await this._client.GetCounterByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                    request.CounterName.ToString()
                    );
                _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "counter")
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
                        "Counter"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
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
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> DeleteFuture(
            DeleteCounterByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithCounterName(this.CounterName);
                var future = this._client.DeleteCounterByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                            request.CounterName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "counter")
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
                    .WithCounterName(this.CounterName);
                DeleteCounterByUserIdResult result = null;
                try {
                    result = await this._client.DeleteCounterByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                        request.CounterName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "counter")
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
                            "Counter"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Mission.Model.Counter>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.CounterDomain> DeleteAsync(
            DeleteCounterByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithCounterName(this.CounterName);
            var future = this._client.DeleteCounterByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                        request.CounterName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "counter")
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
                .WithCounterName(this.CounterName);
            DeleteCounterByUserIdResult result = null;
            try {
                result = await this._client.DeleteCounterByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                    request.CounterName.ToString()
                    );
                _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "counter")
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
                        "Counter"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Mission.Model.Counter>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.CounterDomain> DeleteAsync(
            DeleteCounterByUserIdRequest request
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
        public IFuture<Gs2.Gs2Mission.Domain.Model.CounterDomain> Delete(
            DeleteCounterByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class CounterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Model.Counter> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.Counter> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Mission.Model.Counter>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                        this.CounterName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetCounterByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                                    this.CounterName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "counter")
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
                    (value, _) = _cache.Get<Gs2.Gs2Mission.Model.Counter>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                            this.CounterName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.Counter>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Model.Counter> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Mission.Model.Counter>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                        this.CounterName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetCounterByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                                    this.CounterName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Mission.Model.Counter>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "counter")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Mission.Model.Counter>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.CounterDomain.CreateCacheKey(
                            this.CounterName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Model.Counter> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Mission.Model.Counter> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Mission.Model.Counter> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Mission.Model.Counter> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

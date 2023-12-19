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
#pragma warning disable CS0169, CS0168

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Limit.Domain.Iterator;
using Gs2.Gs2Limit.Request;
using Gs2.Gs2Limit.Result;
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

namespace Gs2.Gs2Limit.Domain.Model
{

    public partial class CounterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LimitRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _limitName;
        private readonly string _counterName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string LimitName => _limitName;
        public string CounterName => _counterName;

        public CounterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string limitName,
            string counterName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LimitRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._limitName = limitName;
            this._counterName = counterName;
            this._parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Counter"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string limitName,
            string counterName,
            string childType
        )
        {
            return string.Join(
                ":",
                "limit",
                namespaceName ?? "null",
                userId ?? "null",
                limitName ?? "null",
                counterName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string limitName,
            string counterName
        )
        {
            return string.Join(
                ":",
                limitName ?? "null",
                counterName ?? "null"
            );
        }

    }

    public partial class CounterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Limit.Model.Counter> GetFuture(
            GetCounterByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Model.Counter> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithLimitName(this.LimitName)
                    .WithCounterName(this.CounterName);
                var future = this._client.GetCounterByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                            request.LimitName.ToString(),
                            request.CounterName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Limit.Model.Counter>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "counter")
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Counter"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                            resultModel.Item.LimitName.ToString(),
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Limit.Model.Counter>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Limit.Model.Counter> GetAsync(
            #else
        private async Task<Gs2.Gs2Limit.Model.Counter> GetAsync(
            #endif
            GetCounterByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithLimitName(this.LimitName)
                .WithCounterName(this.CounterName);
            GetCounterByUserIdResult result = null;
            try {
                result = await this._client.GetCounterByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                    request.LimitName.ToString(),
                    request.CounterName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Limit.Model.Counter>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "counter")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Counter"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                        resultModel.Item.LimitName.ToString(),
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
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
        public IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> CountUpFuture(
            CountUpByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithLimitName(this.LimitName)
                    .WithCounterName(this.CounterName);
                var future = this._client.CountUpByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Counter"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                            resultModel.Item.LimitName.ToString(),
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
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
            return new Gs2InlineFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Domain.Model.CounterDomain> CountUpAsync(
            #else
        public async Task<Gs2.Gs2Limit.Domain.Model.CounterDomain> CountUpAsync(
            #endif
            CountUpByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithLimitName(this.LimitName)
                .WithCounterName(this.CounterName);
            CountUpByUserIdResult result = null;
                result = await this._client.CountUpByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Counter"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                        resultModel.Item.LimitName.ToString(),
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to CountUpFuture.")]
        public IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> CountUp(
            CountUpByUserIdRequest request
        ) {
            return CountUpFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> CountDownFuture(
            CountDownByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithLimitName(this.LimitName)
                    .WithCounterName(this.CounterName);
                var future = this._client.CountDownByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Counter"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                            resultModel.Item.LimitName.ToString(),
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
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
            return new Gs2InlineFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Domain.Model.CounterDomain> CountDownAsync(
            #else
        public async Task<Gs2.Gs2Limit.Domain.Model.CounterDomain> CountDownAsync(
            #endif
            CountDownByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithLimitName(this.LimitName)
                .WithCounterName(this.CounterName);
            CountDownByUserIdResult result = null;
                result = await this._client.CountDownByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Counter"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                        resultModel.Item.LimitName.ToString(),
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
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
        [Obsolete("The name has been changed to CountDownFuture.")]
        public IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> CountDown(
            CountDownByUserIdRequest request
        ) {
            return CountDownFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> DeleteFuture(
            DeleteCounterByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithLimitName(this.LimitName)
                    .WithCounterName(this.CounterName);
                var future = this._client.DeleteCounterByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                            request.LimitName.ToString(),
                            request.CounterName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Limit.Model.Counter>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "counter")
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Counter"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                            resultModel.Item.LimitName.ToString(),
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Limit.Model.Counter>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Domain.Model.CounterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Limit.Domain.Model.CounterDomain> DeleteAsync(
            #endif
            DeleteCounterByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithLimitName(this.LimitName)
                .WithCounterName(this.CounterName);
            DeleteCounterByUserIdResult result = null;
            try {
                result = await this._client.DeleteCounterByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                    request.LimitName.ToString(),
                    request.CounterName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Limit.Model.Counter>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "counter")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Counter"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                        resultModel.Item.LimitName.ToString(),
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Limit.Model.Counter>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> Delete(
            DeleteCounterByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> VerifyFuture(
            VerifyCounterByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithLimitName(this.LimitName)
                    .WithCounterName(this.CounterName);
                var future = this._client.VerifyCounterByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Domain.Model.CounterDomain> VerifyAsync(
            #else
        public async Task<Gs2.Gs2Limit.Domain.Model.CounterDomain> VerifyAsync(
            #endif
            VerifyCounterByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithLimitName(this.LimitName)
                .WithCounterName(this.CounterName);
            VerifyCounterByUserIdResult result = null;
                result = await this._client.VerifyCounterByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to VerifyFuture.")]
        public IFuture<Gs2.Gs2Limit.Domain.Model.CounterDomain> Verify(
            VerifyCounterByUserIdRequest request
        ) {
            return VerifyFuture(request);
        }
        #endif

    }

    public partial class CounterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Limit.Model.Counter> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Model.Counter> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.Counter>(
                    _parentKey,
                    Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                        this.LimitName?.ToString(),
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
                            var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                                    this.LimitName?.ToString(),
                                    this.CounterName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Limit.Model.Counter>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "counter")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.Counter>(
                        _parentKey,
                        Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                            this.LimitName?.ToString(),
                            this.CounterName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Limit.Model.Counter>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Model.Counter> ModelAsync()
            #else
        public async Task<Gs2.Gs2Limit.Model.Counter> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Limit.Model.Counter>(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                    this.LimitName?.ToString(),
                    this.CounterName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.Counter>(
                    _parentKey,
                    Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                        this.LimitName?.ToString(),
                        this.CounterName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetCounterByUserIdRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                                    this.LimitName?.ToString(),
                                    this.CounterName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Limit.Model.Counter>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "counter")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.Counter>(
                        _parentKey,
                        Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                            this.LimitName?.ToString(),
                            this.CounterName?.ToString()
                        )
                    );
                }
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Limit.Model.Counter> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Limit.Model.Counter> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Limit.Model.Counter> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Limit.Model.Counter>(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                    this.LimitName.ToString(),
                    this.CounterName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Limit.Model.Counter> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                    this.LimitName.ToString(),
                    this.CounterName.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Limit.Model.Counter>(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.CounterDomain.CreateCacheKey(
                    this.LimitName.ToString(),
                    this.CounterName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Limit.Model.Counter> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Limit.Model.Counter> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Limit.Model.Counter> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}

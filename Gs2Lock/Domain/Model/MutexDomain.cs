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
using Gs2.Gs2Lock.Domain.Iterator;
using Gs2.Gs2Lock.Request;
using Gs2.Gs2Lock.Result;
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

namespace Gs2.Gs2Lock.Domain.Model
{

    public partial class MutexDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LockRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _propertyId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string PropertyId => _propertyId;

        public MutexDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string propertyId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LockRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Mutex"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string propertyId,
            string childType
        )
        {
            return string.Join(
                ":",
                "lock",
                namespaceName ?? "null",
                userId ?? "null",
                propertyId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string propertyId
        )
        {
            return string.Join(
                ":",
                propertyId ?? "null"
            );
        }

    }

    public partial class MutexDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> LockFuture(
            LockByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.LockByUserIdFuture(
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
                    .WithPropertyId(this.PropertyId);
                LockByUserIdResult result = null;
                    result = await this._client.LockByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mutex"
                        );
                        var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                            resultModel.Item.PropertyId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Lock.Domain.Model.MutexDomain> LockAsync(
            LockByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyId(this.PropertyId);
            var future = this._client.LockByUserIdFuture(
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
                .WithPropertyId(this.PropertyId);
            LockByUserIdResult result = null;
                result = await this._client.LockByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mutex"
                    );
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        resultModel.Item.PropertyId.ToString()
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
        public async UniTask<Gs2.Gs2Lock.Domain.Model.MutexDomain> LockAsync(
            LockByUserIdRequest request
        ) {
            var future = LockFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to LockFuture.")]
        public IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> Lock(
            LockByUserIdRequest request
        ) {
            return LockFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> UnlockFuture(
            UnlockByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.UnlockByUserIdFuture(
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
                    .WithPropertyId(this.PropertyId);
                UnlockByUserIdResult result = null;
                    result = await this._client.UnlockByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mutex"
                        );
                        var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                            resultModel.Item.PropertyId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Lock.Domain.Model.MutexDomain> UnlockAsync(
            UnlockByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyId(this.PropertyId);
            var future = this._client.UnlockByUserIdFuture(
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
                .WithPropertyId(this.PropertyId);
            UnlockByUserIdResult result = null;
                result = await this._client.UnlockByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mutex"
                    );
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        resultModel.Item.PropertyId.ToString()
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
        public async UniTask<Gs2.Gs2Lock.Domain.Model.MutexDomain> UnlockAsync(
            UnlockByUserIdRequest request
        ) {
            var future = UnlockFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UnlockFuture.")]
        public IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> Unlock(
            UnlockByUserIdRequest request
        ) {
            return UnlockFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Lock.Model.Mutex> GetFuture(
            GetMutexByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lock.Model.Mutex> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.GetMutexByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                            request.PropertyId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "mutex")
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
                    .WithPropertyId(this.PropertyId);
                GetMutexByUserIdResult result = null;
                try {
                    result = await this._client.GetMutexByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        request.PropertyId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "mutex")
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
                        var parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mutex"
                        );
                        var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                            resultModel.Item.PropertyId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Lock.Model.Mutex>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Lock.Model.Mutex> GetAsync(
            GetMutexByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyId(this.PropertyId);
            var future = this._client.GetMutexByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        request.PropertyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "mutex")
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
                .WithPropertyId(this.PropertyId);
            GetMutexByUserIdResult result = null;
            try {
                result = await this._client.GetMutexByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                    request.PropertyId.ToString()
                    );
                _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "mutex")
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
                    var parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mutex"
                    );
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        resultModel.Item.PropertyId.ToString()
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
        public IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> DeleteFuture(
            DeleteMutexByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithPropertyId(this.PropertyId);
                var future = this._client.DeleteMutexByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                            request.PropertyId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "mutex")
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
                    .WithPropertyId(this.PropertyId);
                DeleteMutexByUserIdResult result = null;
                try {
                    result = await this._client.DeleteMutexByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        request.PropertyId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "mutex")
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
                        var parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mutex"
                        );
                        var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                            resultModel.Item.PropertyId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Lock.Model.Mutex>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Lock.Domain.Model.MutexDomain> DeleteAsync(
            DeleteMutexByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithPropertyId(this.PropertyId);
            var future = this._client.DeleteMutexByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        request.PropertyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "mutex")
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
                .WithPropertyId(this.PropertyId);
            DeleteMutexByUserIdResult result = null;
            try {
                result = await this._client.DeleteMutexByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                    request.PropertyId.ToString()
                    );
                _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "mutex")
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
                    var parentKey = Gs2.Gs2Lock.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mutex"
                    );
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        resultModel.Item.PropertyId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Lock.Model.Mutex>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lock.Domain.Model.MutexDomain> DeleteAsync(
            DeleteMutexByUserIdRequest request
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
        public IFuture<Gs2.Gs2Lock.Domain.Model.MutexDomain> Delete(
            DeleteMutexByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class MutexDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lock.Model.Mutex> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Lock.Model.Mutex> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Lock.Model.Mutex>(
                    _parentKey,
                    Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        this.PropertyId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetMutexByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                                    this.PropertyId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "mutex")
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
                    (value, _) = _cache.Get<Gs2.Gs2Lock.Model.Mutex>(
                        _parentKey,
                        Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                            this.PropertyId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lock.Model.Mutex>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Lock.Model.Mutex> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Lock.Model.Mutex>(
                    _parentKey,
                    Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                        this.PropertyId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetMutexByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                                    this.PropertyId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Lock.Model.Mutex>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "mutex")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Lock.Model.Mutex>(
                        _parentKey,
                        Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                            this.PropertyId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lock.Model.Mutex> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Lock.Model.Mutex> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Lock.Model.Mutex> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Lock.Model.Mutex> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Lock.Model.Mutex> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                    this.PropertyId.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Lock.Model.Mutex>(
                _parentKey,
                Gs2.Gs2Lock.Domain.Model.MutexDomain.CreateCacheKey(
                    this.PropertyId.ToString()
                ),
                callbackId
            );
        }

    }
}

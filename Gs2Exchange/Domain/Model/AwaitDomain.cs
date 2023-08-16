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
using Gs2.Gs2Exchange.Domain.Iterator;
using Gs2.Gs2Exchange.Request;
using Gs2.Gs2Exchange.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
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

namespace Gs2.Gs2Exchange.Domain.Model
{

    public partial class AwaitDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ExchangeRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _awaitName;

        private readonly String _parentKey;
        public long? UnlockAt { get; set; }
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string AwaitName => _awaitName;

        public AwaitDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string awaitName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ExchangeRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._awaitName = awaitName;
            this._parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Await"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Exchange.Model.Await> GetAsync(
            #else
        private IFuture<Gs2.Gs2Exchange.Model.Await> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Exchange.Model.Await> GetAsync(
        #endif
            GetAwaitByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Model.Await> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithAwaitName(this.AwaitName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetAwaitByUserIdFuture(
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
            var result = await this._client.GetAwaitByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Await"
                    );
                    var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Model.Await>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionDomain> AcquireAsync(
            #else
        public IFuture<Gs2.Core.Domain.TransactionDomain> Acquire(
            #endif
        #else
        public async Task<Gs2.Core.Domain.TransactionDomain> AcquireAsync(
        #endif
            AcquireByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithAwaitName(this.AwaitName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.AcquireByUserIdFuture(
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
            var result = await this._client.AcquireByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Await"
                    );
                    var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future2 = stampSheet.Wait();
                yield return future2;
                if (future2.Error != null)
                {
                    self.OnError(future2.Error);
                    yield break;
                }
        #else
                await stampSheet.WaitAsync();
        #endif
            }

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(stampSheet);
        #else
            return stampSheet;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionDomain> AcquireForceAsync(
            #else
        public IFuture<Gs2.Core.Domain.TransactionDomain> AcquireForce(
            #endif
        #else
        public async Task<Gs2.Core.Domain.TransactionDomain> AcquireForceAsync(
        #endif
            AcquireForceByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithAwaitName(this.AwaitName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.AcquireForceByUserIdFuture(
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
            var result = await this._client.AcquireForceByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Await"
                    );
                    var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future2 = stampSheet.Wait();
                yield return future2;
                if (future2.Error != null)
                {
                    self.OnError(future2.Error);
                    yield break;
                }
        #else
                await stampSheet.WaitAsync();
        #endif
            }

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(stampSheet);
        #else
            return stampSheet;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionDomain> SkipAsync(
            #else
        public IFuture<Gs2.Core.Domain.TransactionDomain> Skip(
            #endif
        #else
        public async Task<Gs2.Core.Domain.TransactionDomain> SkipAsync(
        #endif
            SkipByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithAwaitName(this.AwaitName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SkipByUserIdFuture(
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
            var result = await this._client.SkipByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Await"
                    );
                    var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future2 = stampSheet.Wait();
                yield return future2;
                if (future2.Error != null)
                {
                    self.OnError(future2.Error);
                    yield break;
                }
        #else
                await stampSheet.WaitAsync();
        #endif
            }

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(stampSheet);
        #else
            return stampSheet;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Exchange.Domain.Model.AwaitDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Exchange.Domain.Model.AwaitDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Exchange.Domain.Model.AwaitDomain> DeleteAsync(
        #endif
            DeleteAwaitByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Domain.Model.AwaitDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithAwaitName(this.AwaitName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteAwaitByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                        request.AwaitName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Exchange.Model.Await>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            DeleteAwaitByUserIdResult result = null;
            try {
                result = await this._client.DeleteAwaitByUserIdAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "await")
                {
                    var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                        request.AwaitName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Exchange.Model.Await>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Await"
                    );
                    var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Exchange.Model.Await>(parentKey, key);
                }
            }
            Gs2.Gs2Exchange.Domain.Model.AwaitDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Domain.Model.AwaitDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string awaitName,
            string childType
        )
        {
            return string.Join(
                ":",
                "exchange",
                namespaceName ?? "null",
                userId ?? "null",
                awaitName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string awaitName
        )
        {
            return string.Join(
                ":",
                awaitName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Exchange.Model.Await> Model() {
            #else
        public IFuture<Gs2.Gs2Exchange.Model.Await> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Exchange.Model.Await> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Model.Await> self)
            {
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2Exchange.Model.Await>(
                       _parentKey,
                       Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                            this.AwaitName?.ToString()
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2Exchange.Model.Await>(
                _parentKey,
                Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                    this.AwaitName?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetAwaitByUserIdRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                                    this.AwaitName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Exchange.Model.Await>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "await")
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                            this.AwaitName?.ToString()
                        );
                    _cache.Put<Gs2.Gs2Exchange.Model.Await>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "await")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Exchange.Model.Await>(
                    _parentKey,
                    Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                        this.AwaitName?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Model.Await>(Impl);
        #endif
        }

    }
}

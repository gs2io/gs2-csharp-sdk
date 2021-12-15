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

    public partial class AwaitAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ExchangeRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;
        private readonly string _awaitName;
        private readonly string _rateName;

        private readonly String _parentKey;
        public long? UnlockAt { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public string AwaitName => _awaitName;
        public string RateName => _rateName;

        public AwaitAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string awaitName,
            string rateName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ExchangeRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._awaitName = awaitName;
            this._rateName = rateName;
            this._parentKey = Gs2.Gs2Exchange.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
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
            GetAwaitRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Model.Await> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithAwaitName(this._awaitName)
                .WithRateName(this._rateName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetAwaitFuture(
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
            var result = await this._client.GetAwaitAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                        request.AwaitName != null ? request.AwaitName.ToString() : null,
                        request.RateName != null ? request.RateName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
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
        public async UniTask<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> AcquireAsync(
            #else
        public IFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> Acquire(
            #endif
        #else
        public async Task<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> AcquireAsync(
        #endif
            AcquireRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithAwaitName(this._awaitName)
                .WithRateName(this._rateName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.AcquireFuture(
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
            var result = await this._client.AcquireAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                        request.AwaitName != null ? request.AwaitName.ToString() : null,
                        request.RateName != null ? request.RateName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            if (result?.StampSheet != null)
            {
                Gs2.Core.Domain.StampSheetDomain stampSheet = new Gs2.Core.Domain.StampSheetDomain(
                        _cache,
                        _jobQueueDomain,
                        _session,
                        result?.StampSheet,
                        result?.StampSheetEncryptionKeyId,
                        _stampSheetConfiguration.NamespaceName,
                        _stampSheetConfiguration.StampTaskEventHandler,
                        _stampSheetConfiguration.StampSheetEventHandler
                );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                yield return stampSheet.Run();
        #else
                try {
                    await stampSheet.RunAsync();
                } catch (Gs2.Core.Exception.Gs2Exception e) {
                    throw new Gs2.Core.Exception.TransactionException(stampSheet, e);
                }
        #endif
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(this);
        #else
            return this;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> SkipAsync(
            #else
        public IFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> Skip(
            #endif
        #else
        public async Task<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> SkipAsync(
        #endif
            SkipRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithAwaitName(this._awaitName)
                .WithRateName(this._rateName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SkipFuture(
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
            var result = await this._client.SkipAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                        request.AwaitName != null ? request.AwaitName.ToString() : null,
                        request.RateName != null ? request.RateName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            if (result?.StampSheet != null)
            {
                Gs2.Core.Domain.StampSheetDomain stampSheet = new Gs2.Core.Domain.StampSheetDomain(
                        _cache,
                        _jobQueueDomain,
                        _session,
                        result?.StampSheet,
                        result?.StampSheetEncryptionKeyId,
                        _stampSheetConfiguration.NamespaceName,
                        _stampSheetConfiguration.StampTaskEventHandler,
                        _stampSheetConfiguration.StampSheetEventHandler
                );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                yield return stampSheet.Run();
        #else
                try {
                    await stampSheet.RunAsync();
                } catch (Gs2.Core.Exception.Gs2Exception e) {
                    throw new Gs2.Core.Exception.TransactionException(stampSheet, e);
                }
        #endif
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(this);
        #else
            return this;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> DeleteAsync(
        #endif
            DeleteAwaitRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithAwaitName(this._awaitName)
                .WithRateName(this._rateName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteAwaitFuture(
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
            DeleteAwaitResult result = null;
            try {
                result = await this._client.DeleteAwaitAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            _cache.Delete<Gs2.Gs2Exchange.Model.Await>(
                _parentKey,
                Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                    request.AwaitName != null ? request.AwaitName.ToString() : null,
                    request.RateName != null ? request.RateName.ToString() : null
                )
            );
            Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Domain.Model.AwaitAccessTokenDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string awaitName,
            string rateName,
            string childType
        )
        {
            return string.Join(
                ":",
                "exchange",
                namespaceName ?? "null",
                userId ?? "null",
                awaitName ?? "null",
                rateName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string awaitName,
            string rateName
        )
        {
            return string.Join(
                ":",
                awaitName ?? "null",
                rateName ?? "null"
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
            Gs2.Gs2Exchange.Model.Await value = _cache.Get<Gs2.Gs2Exchange.Model.Await>(
                _parentKey,
                Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                    this.AwaitName?.ToString(),
                    this.RateName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetAwaitRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Exchange.Model.Await>(
                            _parentKey,
                            Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                                this.AwaitName?.ToString(),
                                this.RateName?.ToString()
                            )
                        );
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException) {
                    _cache.Delete<Gs2.Gs2Exchange.Model.Await>(
                        _parentKey,
                        Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                            this.AwaitName?.ToString(),
                            this.RateName?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Exchange.Model.Await>(
                _parentKey,
                Gs2.Gs2Exchange.Domain.Model.AwaitDomain.CreateCacheKey(
                    this.AwaitName?.ToString(),
                    this.RateName?.ToString()
                )
            );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Exchange.Model.Await>(Impl);
        #endif
        }

    }
}

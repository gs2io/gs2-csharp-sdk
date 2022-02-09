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
using Gs2.Gs2Account.Domain.Iterator;
using Gs2.Gs2Account.Request;
using Gs2.Gs2Account.Result;
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

namespace Gs2.Gs2Account.Domain.Model
{

    public partial class TakeOverAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2AccountRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;
        private readonly int? _type;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public int? Type => _type;

        public TakeOverAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            int? type
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2AccountRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._type = type;
            this._parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                "TakeOver"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> CreateAsync(
            #else
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> Create(
            #endif
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> CreateAsync(
        #endif
            CreateTakeOverRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithType(this._type);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateTakeOverFuture(
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
            var result = await this._client.CreateTakeOverAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "TakeOver"
                );
                var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                    resultModel.Item.Type.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Account.Model.TakeOver> GetAsync(
            #else
        private IFuture<Gs2.Gs2Account.Model.TakeOver> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Account.Model.TakeOver> GetAsync(
        #endif
            GetTakeOverRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.TakeOver> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithType(this._type);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetTakeOverFuture(
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
            var result = await this._client.GetTakeOverAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "TakeOver"
                );
                var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                    resultModel.Item.Type.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.TakeOver>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> UpdateAsync(
        #endif
            UpdateTakeOverRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithType(this._type);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateTakeOverFuture(
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
            var result = await this._client.UpdateTakeOverAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "TakeOver"
                );
                var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                    resultModel.Item.Type.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> DeleteAsync(
        #endif
            DeleteTakeOverRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithType(this._type);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteTakeOverFuture(
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
            DeleteTakeOverResult result = null;
            try {
                result = await this._client.DeleteTakeOverAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "TakeOver"
                );
                var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                    resultModel.Item.Type.ToString()
                );
                cache.Delete<Gs2.Gs2Account.Model.TakeOver>(parentKey, key);
            }
            Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string type,
            string childType
        )
        {
            return string.Join(
                ":",
                "account",
                namespaceName ?? "null",
                userId ?? "null",
                type ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string type
        )
        {
            return string.Join(
                ":",
                type ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Model.TakeOver> Model() {
            #else
        public IFuture<Gs2.Gs2Account.Model.TakeOver> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Account.Model.TakeOver> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.TakeOver> self)
            {
        #endif
            Gs2.Gs2Account.Model.TakeOver value = _cache.Get<Gs2.Gs2Account.Model.TakeOver>(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                    this.Type?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetTakeOverRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "takeOver")
                            {
                                _cache.Delete<Gs2.Gs2Account.Model.TakeOver>(
                                    _parentKey,
                                    Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                                        this.Type?.ToString()
                                    )
                                );
                            }
                            else
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
                    if (e.errors[0].component == "takeOver")
                    {
                    _cache.Delete<Gs2.Gs2Account.Model.TakeOver>(
                            _parentKey,
                            Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                                this.Type?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Account.Model.TakeOver>(
                _parentKey,
                Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                    this.Type?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.TakeOver>(Impl);
        #endif
        }

    }
}

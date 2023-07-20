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
using Gs2.Gs2Enchant.Domain.Iterator;
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Result;
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

namespace Gs2.Gs2Enchant.Domain.Model
{

    public partial class BalanceParameterStatusDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2EnchantRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _parameterName;
        private readonly string _propertyId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string ParameterName => _parameterName;
        public string PropertyId => _propertyId;

        public BalanceParameterStatusDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2EnchantRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parameterName = parameterName;
            this._propertyId = propertyId;
            this._parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "BalanceParameterStatus"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Enchant.Model.BalanceParameterStatus> GetAsync(
            #else
        private IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Enchant.Model.BalanceParameterStatus> GetAsync(
        #endif
            GetBalanceParameterStatusByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetBalanceParameterStatusByUserIdFuture(
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
            var result = await this._client.GetBalanceParameterStatusByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BalanceParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> DeleteAsync(
        #endif
            DeleteBalanceParameterStatusByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteBalanceParameterStatusByUserIdFuture(
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
            DeleteBalanceParameterStatusByUserIdResult result = null;
            try {
                result = await this._client.DeleteBalanceParameterStatusByUserIdAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "balanceParameterStatus")
                {
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        request.ParameterName.ToString(),
                        request.PropertyId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
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
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BalanceParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
                        resultModel.Item.PropertyId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(parentKey, key);
                }
            }
            Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> ReDrawAsync(
            #else
        public IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> ReDraw(
            #endif
        #else
        public async Task<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> ReDrawAsync(
        #endif
            ReDrawBalanceParameterStatusByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithParameterName(this.ParameterName)
                .WithPropertyId(this.PropertyId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ReDrawBalanceParameterStatusByUserIdFuture(
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
            var result = await this._client.ReDrawBalanceParameterStatusByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BalanceParameterStatus"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        resultModel.Item.ParameterName.ToString(),
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
            Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId,
            string childType
        )
        {
            return string.Join(
                ":",
                "enchant",
                namespaceName ?? "null",
                userId ?? "null",
                parameterName ?? "null",
                propertyId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string parameterName,
            string propertyId
        )
        {
            return string.Join(
                ":",
                parameterName ?? "null",
                propertyId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enchant.Model.BalanceParameterStatus> Model() {
            #else
        public IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Enchant.Model.BalanceParameterStatus> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus> self)
            {
        #endif
            var (value, find) = _cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                    this.ParameterName?.ToString(),
                    this.PropertyId?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetBalanceParameterStatusByUserIdRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                                    this.ParameterName?.ToString(),
                                    this.PropertyId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "balanceParameterStatus")
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
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                            this.ParameterName?.ToString(),
                            this.PropertyId?.ToString()
                        );
                    _cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "balanceParameterStatus")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                    _parentKey,
                    Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain.CreateCacheKey(
                        this.ParameterName?.ToString(),
                        this.PropertyId?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(Impl);
        #endif
        }

    }
}

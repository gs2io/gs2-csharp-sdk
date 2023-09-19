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

namespace Gs2.Gs2Account.Domain.Model
{

    public partial class TakeOverDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2AccountRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly int? _type;
        private readonly string _userIdentifier;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public int? Type => _type;

        public TakeOverDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
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
            this._userId = userId;
            this._type = type;
            this._parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "TakeOver"
            );
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

    }

    public partial class TakeOverDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> CreateFuture(
            CreateTakeOverByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithType(this.Type);
                var future = this._client.CreateTakeOverByUserIdFuture(
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
                    .WithType(this.Type);
                CreateTakeOverByUserIdResult result = null;
                    result = await this._client.CreateTakeOverByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
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
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverDomain> CreateAsync(
            CreateTakeOverByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithType(this.Type);
            var future = this._client.CreateTakeOverByUserIdFuture(
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
                .WithType(this.Type);
            CreateTakeOverByUserIdResult result = null;
                result = await this._client.CreateTakeOverByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
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
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverDomain> CreateAsync(
            CreateTakeOverByUserIdRequest request
        ) {
            var future = CreateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateFuture.")]
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> Create(
            CreateTakeOverByUserIdRequest request
        ) {
            return CreateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Account.Model.TakeOver> GetFuture(
            GetTakeOverByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.TakeOver> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithType(this.Type);
                var future = this._client.GetTakeOverByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
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
                    .WithType(this.Type);
                GetTakeOverByUserIdResult result = null;
                try {
                    result = await this._client.GetTakeOverByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
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
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.TakeOver>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Account.Model.TakeOver> GetAsync(
            GetTakeOverByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithType(this.Type);
            var future = this._client.GetTakeOverByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
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
                .WithType(this.Type);
            GetTakeOverByUserIdResult result = null;
            try {
                result = await this._client.GetTakeOverByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
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
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> UpdateFuture(
            UpdateTakeOverByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithType(this.Type);
                var future = this._client.UpdateTakeOverByUserIdFuture(
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
                    .WithType(this.Type);
                UpdateTakeOverByUserIdResult result = null;
                    result = await this._client.UpdateTakeOverByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
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
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverDomain> UpdateAsync(
            UpdateTakeOverByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithType(this.Type);
            var future = this._client.UpdateTakeOverByUserIdFuture(
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
                .WithType(this.Type);
            UpdateTakeOverByUserIdResult result = null;
                result = await this._client.UpdateTakeOverByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Account.Domain.Model.AccountDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
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
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverDomain> UpdateAsync(
            UpdateTakeOverByUserIdRequest request
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
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverDomain> Update(
            UpdateTakeOverByUserIdRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

    }

    public partial class TakeOverDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Model.TakeOver> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Account.Model.TakeOver> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Account.Model.TakeOver>(
                    _parentKey,
                    Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                        this.Type?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetTakeOverByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                                    this.Type?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Account.Model.TakeOver>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "takeOver")
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
                    (value, _) = _cache.Get<Gs2.Gs2Account.Model.TakeOver>(
                        _parentKey,
                        Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                            this.Type?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Account.Model.TakeOver>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Account.Model.TakeOver> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Account.Model.TakeOver>(
                    _parentKey,
                    Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                        this.Type?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetTakeOverByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                                    this.Type?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Account.Model.TakeOver>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "takeOver")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Account.Model.TakeOver>(
                        _parentKey,
                        Gs2.Gs2Account.Domain.Model.TakeOverDomain.CreateCacheKey(
                            this.Type?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Model.TakeOver> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Account.Model.TakeOver> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Account.Model.TakeOver> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Account.Model.TakeOver> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

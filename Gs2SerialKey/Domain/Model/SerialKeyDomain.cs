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
 *
 * deny overwrite
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
using Gs2.Gs2SerialKey.Domain.Iterator;
using Gs2.Gs2SerialKey.Request;
using Gs2.Gs2SerialKey.Result;
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

namespace Gs2.Gs2SerialKey.Domain.Model
{

    public partial class SerialKeyDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2SerialKeyRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _serialKeyCode;

        private readonly String _parentKey;
        public string Url { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string SerialKeyCode => _serialKeyCode;

        public SerialKeyDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string serialKeyCode
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2SerialKeyRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._serialKeyCode = serialKeyCode;
            this._parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "SerialKey"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string serialKeyCode,
            string childType
        )
        {
            return string.Join(
                ":",
                "serialKey",
                namespaceName ?? "null",
                userId ?? "null",
                serialKeyCode ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string serialKeyCode
        )
        {
            return string.Join(
                ":",
                serialKeyCode ?? "null"
            );
        }

    }

    public partial class SerialKeyDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2SerialKey.Model.SerialKey> GetFuture(
            GetSerialKeyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Model.SerialKey> self)
            {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCode(this.SerialKeyCode);
            var future = this._client.GetSerialKeyFuture(
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
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SerialKey"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                        resultModel.Item.Code.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.CampaignModel != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CampaignModel"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        resultModel.CampaignModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.CampaignModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.SerialKey>(Impl);
        }
        #else
        private async Task<Gs2.Gs2SerialKey.Model.SerialKey> GetAsync(
            GetSerialKeyRequest request
        ) {

            request
                .WithNamespaceName(this.NamespaceName)
                .WithCode(this.SerialKeyCode);
            var result = await this._client.GetSerialKeyAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SerialKey"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                        resultModel.Item.Code.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.CampaignModel != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CampaignModel"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        resultModel.CampaignModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.CampaignModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> UseFuture(
            UseByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithCode(this.SerialKeyCode);
                var future = this._client.UseByUserIdFuture(
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
                    .WithCode(this.SerialKeyCode);
                UseByUserIdResult result = null;
                    result = await this._client.UseByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "SerialKey"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                            resultModel.Item.Code.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.CampaignModel != null) {
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CampaignModel"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            resultModel.CampaignModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.CampaignModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> UseAsync(
            UseByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithCode(this.SerialKeyCode);
            var future = this._client.UseByUserIdFuture(
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
                .WithCode(this.SerialKeyCode);
            UseByUserIdResult result = null;
                result = await this._client.UseByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SerialKey"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                        resultModel.Item.Code.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.CampaignModel != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CampaignModel"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        resultModel.CampaignModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.CampaignModel,
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
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> UseAsync(
            UseByUserIdRequest request
        ) {
            var future = UseFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UseFuture.")]
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> Use(
            UseByUserIdRequest request
        ) {
            return UseFuture(request);
        }
        #endif

    }

    public partial class SerialKeyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Model.SerialKey> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Model.SerialKey> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2SerialKey.Model.SerialKey>(
                    _parentKey,
                    Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                        this.SerialKeyCode?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetSerialKeyRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                                    this.SerialKeyCode?.ToString()
                                );
                            _cache.Put<Gs2.Gs2SerialKey.Model.SerialKey>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "serialKey")
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
                    (value, _) = _cache.Get<Gs2.Gs2SerialKey.Model.SerialKey>(
                        _parentKey,
                        Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                            this.SerialKeyCode?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.SerialKey>(Impl);
        }
        #else
        public async Task<Gs2.Gs2SerialKey.Model.SerialKey> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2SerialKey.Model.SerialKey>(
                    _parentKey,
                    Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                        this.SerialKeyCode?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetSerialKeyRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                                    this.SerialKeyCode?.ToString()
                                );
                    _cache.Put<Gs2.Gs2SerialKey.Model.SerialKey>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "serialKey")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2SerialKey.Model.SerialKey>(
                        _parentKey,
                        Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                            this.SerialKeyCode?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SerialKey.Model.SerialKey> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2SerialKey.Model.SerialKey> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2SerialKey.Model.SerialKey> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2SerialKey.Model.SerialKey> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

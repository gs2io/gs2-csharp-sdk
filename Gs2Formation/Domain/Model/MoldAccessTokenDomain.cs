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
using Gs2.Gs2Formation.Domain.Iterator;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
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

namespace Gs2.Gs2Formation.Domain.Model
{

    public partial class MoldAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _moldModelName;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string MoldModelName => _moldModelName;

        public MoldAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string moldModelName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FormationRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._moldModelName = moldModelName;
            this._parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Mold"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.Mold> GetFuture(
            GetMoldRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Mold> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMoldModelName(this.MoldModelName);
                var future = this._client.GetMoldFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            request.MoldModelName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Mold>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "mold")
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.MoldModel != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MoldModel"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                            resultModel.MoldModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.MoldModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.Mold>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Formation.Model.Mold> GetAsync(
            #else
        private async Task<Gs2.Gs2Formation.Model.Mold> GetAsync(
            #endif
            GetMoldRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMoldModelName(this.MoldModelName);
            GetMoldResult result = null;
            try {
                result = await this._client.GetMoldAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                    request.MoldModelName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Mold>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "mold")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mold"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.MoldModel != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MoldModel"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelDomain.CreateCacheKey(
                        resultModel.MoldModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.MoldModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> DeleteFuture(
            DeleteMoldRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMoldModelName(this.MoldModelName);
                var future = this._client.DeleteMoldFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            request.MoldModelName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Mold>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "mold")
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Mold"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.Mold>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> DeleteAsync(
            #endif
            DeleteMoldRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMoldModelName(this.MoldModelName);
            DeleteMoldResult result = null;
            try {
                result = await this._client.DeleteMoldAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                    request.MoldModelName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Mold>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "mold")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Mold"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.Mold>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> Delete(
            DeleteMoldRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.Form> Forms(
        )
        {
            return new DescribeFormsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.MoldModelName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.Form> FormsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.Form> Forms(
            #endif
        #else
        public DescribeFormsIterator FormsAsync(
        #endif
        )
        {
            return new DescribeFormsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.MoldModelName,
                this.AccessToken
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeForms(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Formation.Model.Form>(
                Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    "Form"
                ),
                callback
            );
        }

        public void UnsubscribeForms(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Formation.Model.Form>(
                Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    "Form"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain Form(
            int? index
        ) {
            return new Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                this.MoldModelName,
                index
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string moldModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "formation",
                namespaceName ?? "null",
                userId ?? "null",
                moldModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string moldModelName
        )
        {
            return string.Join(
                ":",
                moldModelName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.Mold> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.Mold> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.Mold>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                        this.MoldModelName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetMoldRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                                    this.MoldModelName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Mold>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "mold")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.Mold>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            this.MoldModelName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.Mold>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Formation.Model.Mold> ModelAsync()
            #else
        public async Task<Gs2.Gs2Formation.Model.Mold> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Formation.Model.Mold>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                    this.MoldModelName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.Mold>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                        this.MoldModelName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetMoldRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                                    this.MoldModelName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Formation.Model.Mold>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "mold")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Formation.Model.Mold>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                            this.MoldModelName?.ToString()
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
        public async UniTask<Gs2.Gs2Formation.Model.Mold> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Formation.Model.Mold> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Formation.Model.Mold> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Formation.Model.Mold> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                    this.MoldModelName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Formation.Model.Mold>(
                _parentKey,
                Gs2.Gs2Formation.Domain.Model.MoldDomain.CreateCacheKey(
                    this.MoldModelName.ToString()
                ),
                callbackId
            );
        }

    }
}

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
using Gs2.Gs2Enhance.Domain.Iterator;
using Gs2.Gs2Enhance.Request;
using Gs2.Gs2Enhance.Result;
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
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Enhance.Domain.Model
{

    public partial class UnleashRateModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnhanceRestClient _client;
        private readonly string _namespaceName;
        private readonly string _rateName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string RateName => _rateName;

        public UnleashRateModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string rateName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnhanceRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._rateName = rateName;
            this._parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "UnleashRateModelMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string rateName,
            string childType
        )
        {
            return string.Join(
                ":",
                "enhance",
                namespaceName ?? "null",
                rateName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string rateName
        )
        {
            return string.Join(
                ":",
                rateName ?? "null"
            );
        }

    }

    public partial class UnleashRateModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> GetFuture(
            GetUnleashRateModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRateName(this.RateName);
                var future = this._client.GetUnleashRateModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                            request.RateName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "unleashRateModelMaster")
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
                        var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "UnleashRateModelMaster"
                        );
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> GetAsync(
            #endif
            GetUnleashRateModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRateName(this.RateName);
            GetUnleashRateModelMasterResult result = null;
            try {
                result = await this._client.GetUnleashRateModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                    request.RateName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "unleashRateModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "UnleashRateModelMaster"
                    );
                    var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> UpdateFuture(
            UpdateUnleashRateModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRateName(this.RateName);
                var future = this._client.UpdateUnleashRateModelMasterFuture(
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
                        var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "UnleashRateModelMaster"
                        );
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> UpdateAsync(
            #endif
            UpdateUnleashRateModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRateName(this.RateName);
            UpdateUnleashRateModelMasterResult result = null;
                result = await this._client.UpdateUnleashRateModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "UnleashRateModelMaster"
                    );
                    var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
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
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> Update(
            UpdateUnleashRateModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> DeleteFuture(
            DeleteUnleashRateModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRateName(this.RateName);
                var future = this._client.DeleteUnleashRateModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                            request.RateName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "unleashRateModelMaster")
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
                        var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "UnleashRateModelMaster"
                        );
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> DeleteAsync(
            #endif
            DeleteUnleashRateModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRateName(this.RateName);
            DeleteUnleashRateModelMasterResult result = null;
            try {
                result = await this._client.DeleteUnleashRateModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                    request.RateName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "unleashRateModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enhance.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "UnleashRateModelMaster"
                    );
                    var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> Delete(
            DeleteUnleashRateModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class UnleashRateModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                    _parentKey,
                    Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                        this.RateName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetUnleashRateModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                                    this.RateName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "unleashRateModelMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                        _parentKey,
                        Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                            this.RateName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                _parentKey,
                Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                    this.RateName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                    _parentKey,
                    Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                        this.RateName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetUnleashRateModelMasterRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                                    this.RateName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "unleashRateModelMaster")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                        _parentKey,
                        Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                            this.RateName?.ToString()
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
        public async UniTask<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                _parentKey,
                Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                    this.RateName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                    this.RateName.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                _parentKey,
                Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain.CreateCacheKey(
                    this.RateName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> callback)
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

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
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Enhance.Domain.Model
{

    public partial class UnleashRateModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnhanceRestClient _client;
        private readonly string _namespaceName;
        private readonly string _rateName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string RateName => _rateName;

        public UnleashRateModelDomain(
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
                "UnleashRateModel"
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

    public partial class UnleashRateModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Enhance.Model.UnleashRateModel> GetFuture(
            GetUnleashRateModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enhance.Model.UnleashRateModel> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRateName(this.RateName);
                var future = this._client.GetUnleashRateModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                            request.RateName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "unleashRateModel")
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
                            "UnleashRateModel"
                        );
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enhance.Model.UnleashRateModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Enhance.Model.UnleashRateModel> GetAsync(
            #else
        private async Task<Gs2.Gs2Enhance.Model.UnleashRateModel> GetAsync(
            #endif
            GetUnleashRateModelRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRateName(this.RateName);
            GetUnleashRateModelResult result = null;
            try {
                result = await this._client.GetUnleashRateModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                    request.RateName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "unleashRateModel")
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
                        "UnleashRateModel"
                    );
                    var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
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

    }

    public partial class UnleashRateModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enhance.Model.UnleashRateModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Enhance.Model.UnleashRateModel> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                    _parentKey,
                    Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                        this.RateName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetUnleashRateModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                                    this.RateName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "unleashRateModel")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                        _parentKey,
                        Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                            this.RateName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enhance.Model.UnleashRateModel>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enhance.Model.UnleashRateModel> ModelAsync()
            #else
        public async Task<Gs2.Gs2Enhance.Model.UnleashRateModel> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                _parentKey,
                Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                    this.RateName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                    _parentKey,
                    Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                        this.RateName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetUnleashRateModelRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                                    this.RateName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "unleashRateModel")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                        _parentKey,
                        Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Enhance.Model.UnleashRateModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Enhance.Model.UnleashRateModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Enhance.Model.UnleashRateModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                _parentKey,
                Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                    this.RateName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Enhance.Model.UnleashRateModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                _parentKey,
                Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain.CreateCacheKey(
                    this.RateName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Enhance.Model.UnleashRateModel> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Enhance.Model.UnleashRateModel> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Enhance.Model.UnleashRateModel> callback)
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

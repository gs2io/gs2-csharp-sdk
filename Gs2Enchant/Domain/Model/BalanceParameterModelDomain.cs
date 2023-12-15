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
using Gs2.Gs2Enchant.Domain.Iterator;
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Result;
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

namespace Gs2.Gs2Enchant.Domain.Model
{

    public partial class BalanceParameterModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnchantRestClient _client;
        private readonly string _namespaceName;
        private readonly string _parameterName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string ParameterName => _parameterName;

        public BalanceParameterModelDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string parameterName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnchantRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._parameterName = parameterName;
            this._parentKey = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "BalanceParameterModel"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string parameterName,
            string childType
        )
        {
            return string.Join(
                ":",
                "enchant",
                namespaceName ?? "null",
                parameterName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string parameterName
        )
        {
            return string.Join(
                ":",
                parameterName ?? "null"
            );
        }

    }

    public partial class BalanceParameterModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Enchant.Model.BalanceParameterModel> GetFuture(
            GetBalanceParameterModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.BalanceParameterModel> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithParameterName(this.ParameterName);
                var future = this._client.GetBalanceParameterModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                            request.ParameterName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "balanceParameterModel")
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
                        var parentKey = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BalanceParameterModel"
                        );
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.BalanceParameterModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Enchant.Model.BalanceParameterModel> GetAsync(
            #else
        private async Task<Gs2.Gs2Enchant.Model.BalanceParameterModel> GetAsync(
            #endif
            GetBalanceParameterModelRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithParameterName(this.ParameterName);
            GetBalanceParameterModelResult result = null;
            try {
                result = await this._client.GetBalanceParameterModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                    request.ParameterName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "balanceParameterModel")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Enchant.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BalanceParameterModel"
                    );
                    var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
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

    public partial class BalanceParameterModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enchant.Model.BalanceParameterModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Enchant.Model.BalanceParameterModel> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                    _parentKey,
                    Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                        this.ParameterName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetBalanceParameterModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                                    this.ParameterName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "balanceParameterModel")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                        _parentKey,
                        Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                            this.ParameterName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Enchant.Model.BalanceParameterModel>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Enchant.Model.BalanceParameterModel> ModelAsync()
            #else
        public async Task<Gs2.Gs2Enchant.Model.BalanceParameterModel> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                    this.ParameterName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                    _parentKey,
                    Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                        this.ParameterName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetBalanceParameterModelRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                                    this.ParameterName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "balanceParameterModel")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                        _parentKey,
                        Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                            this.ParameterName?.ToString()
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
        public async UniTask<Gs2.Gs2Enchant.Model.BalanceParameterModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Enchant.Model.BalanceParameterModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Enchant.Model.BalanceParameterModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Enchant.Model.BalanceParameterModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                    this.ParameterName.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Enchant.Model.BalanceParameterModel>(
                _parentKey,
                Gs2.Gs2Enchant.Domain.Model.BalanceParameterModelDomain.CreateCacheKey(
                    this.ParameterName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Enchant.Model.BalanceParameterModel> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Enchant.Model.BalanceParameterModel> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Enchant.Model.BalanceParameterModel> callback)
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

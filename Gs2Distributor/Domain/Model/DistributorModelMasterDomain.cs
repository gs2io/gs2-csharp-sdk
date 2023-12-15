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
using Gs2.Gs2Distributor.Domain.Iterator;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
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

namespace Gs2.Gs2Distributor.Domain.Model
{

    public partial class DistributorModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DistributorRestClient _client;
        private readonly string _namespaceName;
        private readonly string _distributorName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string DistributorName => _distributorName;

        public DistributorModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string distributorName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DistributorRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._distributorName = distributorName;
            this._parentKey = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "DistributorModelMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string distributorName,
            string childType
        )
        {
            return string.Join(
                ":",
                "distributor",
                namespaceName ?? "null",
                distributorName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string distributorName
        )
        {
            return string.Join(
                ":",
                distributorName ?? "null"
            );
        }

    }

    public partial class DistributorModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Distributor.Model.DistributorModelMaster> GetFuture(
            GetDistributorModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Model.DistributorModelMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithDistributorName(this.DistributorName);
                var future = this._client.GetDistributorModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                            request.DistributorName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "distributorModelMaster")
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
                        var parentKey = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "DistributorModelMaster"
                        );
                        var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Model.DistributorModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Distributor.Model.DistributorModelMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Distributor.Model.DistributorModelMaster> GetAsync(
            #endif
            GetDistributorModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithDistributorName(this.DistributorName);
            GetDistributorModelMasterResult result = null;
            try {
                result = await this._client.GetDistributorModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                    request.DistributorName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "distributorModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "DistributorModelMaster"
                    );
                    var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> UpdateFuture(
            UpdateDistributorModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithDistributorName(this.DistributorName);
                var future = this._client.UpdateDistributorModelMasterFuture(
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
                        var parentKey = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "DistributorModelMaster"
                        );
                        var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> UpdateAsync(
            #endif
            UpdateDistributorModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithDistributorName(this.DistributorName);
            UpdateDistributorModelMasterResult result = null;
                result = await this._client.UpdateDistributorModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "DistributorModelMaster"
                    );
                    var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> Update(
            UpdateDistributorModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> DeleteFuture(
            DeleteDistributorModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithDistributorName(this.DistributorName);
                var future = this._client.DeleteDistributorModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                            request.DistributorName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "distributorModelMaster")
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
                        var parentKey = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "DistributorModelMaster"
                        );
                        var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Distributor.Model.DistributorModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> DeleteAsync(
            #endif
            DeleteDistributorModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithDistributorName(this.DistributorName);
            DeleteDistributorModelMasterResult result = null;
            try {
                result = await this._client.DeleteDistributorModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                    request.DistributorName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "distributorModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "DistributorModelMaster"
                    );
                    var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Distributor.Model.DistributorModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain> Delete(
            DeleteDistributorModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class DistributorModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Model.DistributorModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Model.DistributorModelMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                    _parentKey,
                    Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                        this.DistributorName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetDistributorModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                                    this.DistributorName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "distributorModelMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                        _parentKey,
                        Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                            this.DistributorName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Model.DistributorModelMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Distributor.Model.DistributorModelMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Distributor.Model.DistributorModelMaster> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                _parentKey,
                Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                    this.DistributorName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                    _parentKey,
                    Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                        this.DistributorName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetDistributorModelMasterRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                                    this.DistributorName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "distributorModelMaster")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                        _parentKey,
                        Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                            this.DistributorName?.ToString()
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
        public async UniTask<Gs2.Gs2Distributor.Model.DistributorModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Distributor.Model.DistributorModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Distributor.Model.DistributorModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Distributor.Model.DistributorModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                    this.DistributorName.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Distributor.Model.DistributorModelMaster>(
                _parentKey,
                Gs2.Gs2Distributor.Domain.Model.DistributorModelMasterDomain.CreateCacheKey(
                    this.DistributorName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Distributor.Model.DistributorModelMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Distributor.Model.DistributorModelMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Distributor.Model.DistributorModelMaster> callback)
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

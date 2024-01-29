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
using Gs2.Gs2Showcase.Domain.Iterator;
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
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

namespace Gs2.Gs2Showcase.Domain.Model
{

    public partial class SalesItemGroupMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private readonly string _salesItemGroupName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string SalesItemGroupName => _salesItemGroupName;

        public SalesItemGroupMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string salesItemGroupName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ShowcaseRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._salesItemGroupName = salesItemGroupName;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "SalesItemGroupMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string salesItemGroupName,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                salesItemGroupName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string salesItemGroupName
        )
        {
            return string.Join(
                ":",
                salesItemGroupName ?? "null"
            );
        }

    }

    public partial class SalesItemGroupMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> GetFuture(
            GetSalesItemGroupMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                var future = this._client.GetSalesItemGroupMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            request.SalesItemGroupName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "salesItemGroupMaster")
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
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "SalesItemGroupMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> GetAsync(
            #endif
            GetSalesItemGroupMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithSalesItemGroupName(this.SalesItemGroupName);
            GetSalesItemGroupMasterResult result = null;
            try {
                result = await this._client.GetSalesItemGroupMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                    request.SalesItemGroupName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "salesItemGroupMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "SalesItemGroupMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> UpdateFuture(
            UpdateSalesItemGroupMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                var future = this._client.UpdateSalesItemGroupMasterFuture(
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
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "SalesItemGroupMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> UpdateAsync(
            #endif
            UpdateSalesItemGroupMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithSalesItemGroupName(this.SalesItemGroupName);
            UpdateSalesItemGroupMasterResult result = null;
                result = await this._client.UpdateSalesItemGroupMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "SalesItemGroupMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> Update(
            UpdateSalesItemGroupMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> DeleteFuture(
            DeleteSalesItemGroupMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                var future = this._client.DeleteSalesItemGroupMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            request.SalesItemGroupName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "salesItemGroupMaster")
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
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "SalesItemGroupMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> DeleteAsync(
            #endif
            DeleteSalesItemGroupMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithSalesItemGroupName(this.SalesItemGroupName);
            DeleteSalesItemGroupMasterResult result = null;
            try {
                result = await this._client.DeleteSalesItemGroupMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                    request.SalesItemGroupName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "salesItemGroupMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "SalesItemGroupMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> Delete(
            DeleteSalesItemGroupMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class SalesItemGroupMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        this.SalesItemGroupName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetSalesItemGroupMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                                    this.SalesItemGroupName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "salesItemGroupMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            this.SalesItemGroupName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                    this.SalesItemGroupName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        this.SalesItemGroupName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetSalesItemGroupMasterRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                                    this.SalesItemGroupName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "salesItemGroupMaster")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            this.SalesItemGroupName?.ToString()
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
        public async UniTask<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                    this.SalesItemGroupName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                    this.SalesItemGroupName.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                    this.SalesItemGroupName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> callback)
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

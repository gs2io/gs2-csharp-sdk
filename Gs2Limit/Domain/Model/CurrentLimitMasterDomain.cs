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
using Gs2.Gs2Limit.Domain.Iterator;
using Gs2.Gs2Limit.Request;
using Gs2.Gs2Limit.Result;
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

namespace Gs2.Gs2Limit.Domain.Model
{

    public partial class CurrentLimitMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LimitRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;

        public CurrentLimitMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LimitRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "CurrentLimitMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "limit",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

    }

    public partial class CurrentLimitMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> ExportMasterFuture(
            ExportMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.ExportMasterFuture(
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
                        var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CurrentLimitMaster"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> ExportMasterAsync(
            #else
        public async Task<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> ExportMasterAsync(
            #endif
            ExportMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            ExportMasterResult result = null;
                result = await this._client.ExportMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CurrentLimitMaster"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
        [Obsolete("The name has been changed to ExportMasterFuture.")]
        public IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> ExportMaster(
            ExportMasterRequest request
        ) {
            return ExportMasterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Limit.Model.CurrentLimitMaster> GetFuture(
            GetCurrentLimitMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Model.CurrentLimitMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetCurrentLimitMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "currentLimitMaster")
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
                        var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CurrentLimitMaster"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Limit.Model.CurrentLimitMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Limit.Model.CurrentLimitMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Limit.Model.CurrentLimitMaster> GetAsync(
            #endif
            GetCurrentLimitMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            GetCurrentLimitMasterResult result = null;
            try {
                result = await this._client.GetCurrentLimitMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                    );
                this._gs2.Cache.Put<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "currentLimitMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CurrentLimitMaster"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> UpdateFuture(
            UpdateCurrentLimitMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateCurrentLimitMasterFuture(
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
                        var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CurrentLimitMaster"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> UpdateAsync(
            #endif
            UpdateCurrentLimitMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            UpdateCurrentLimitMasterResult result = null;
                result = await this._client.UpdateCurrentLimitMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CurrentLimitMaster"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> Update(
            UpdateCurrentLimitMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> UpdateFromGitHubFuture(
            UpdateCurrentLimitMasterFromGitHubRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateCurrentLimitMasterFromGitHubFuture(
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
                        var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CurrentLimitMaster"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> UpdateFromGitHubAsync(
            #else
        public async Task<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> UpdateFromGitHubAsync(
            #endif
            UpdateCurrentLimitMasterFromGitHubRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            UpdateCurrentLimitMasterFromGitHubResult result = null;
                result = await this._client.UpdateCurrentLimitMasterFromGitHubAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CurrentLimitMaster"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
        [Obsolete("The name has been changed to UpdateFromGitHubFuture.")]
        public IFuture<Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain> UpdateFromGitHub(
            UpdateCurrentLimitMasterFromGitHubRequest request
        ) {
            return UpdateFromGitHubFuture(request);
        }
        #endif

    }

    public partial class CurrentLimitMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Limit.Model.CurrentLimitMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Model.CurrentLimitMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                    _parentKey,
                    Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetCurrentLimitMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "currentLimitMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                        _parentKey,
                        Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Limit.Model.CurrentLimitMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Model.CurrentLimitMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Limit.Model.CurrentLimitMaster> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                    _parentKey,
                    Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetCurrentLimitMasterRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "currentLimitMaster")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                        _parentKey,
                        Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Limit.Model.CurrentLimitMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Limit.Model.CurrentLimitMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Limit.Model.CurrentLimitMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Limit.Model.CurrentLimitMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Limit.Model.CurrentLimitMaster>(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.CurrentLimitMasterDomain.CreateCacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Limit.Model.CurrentLimitMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Limit.Model.CurrentLimitMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Limit.Model.CurrentLimitMaster> callback)
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

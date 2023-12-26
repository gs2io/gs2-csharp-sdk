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
using Gs2.Gs2Grade.Domain.Iterator;
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Grade.Result;
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

namespace Gs2.Gs2Grade.Domain.Model
{

    public partial class CurrentGradeMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2GradeRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;

        public CurrentGradeMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2GradeRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "CurrentGradeMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "grade",
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

    public partial class CurrentGradeMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> ExportMasterFuture(
            ExportMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> self)
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CurrentGradeMaster"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> ExportMasterAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> ExportMasterAsync(
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
                    var parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CurrentGradeMaster"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> ExportMaster(
            ExportMasterRequest request
        ) {
            return ExportMasterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Grade.Model.CurrentGradeMaster> GetFuture(
            GetCurrentGradeMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Model.CurrentGradeMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetCurrentGradeMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "currentGradeMaster")
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CurrentGradeMaster"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Model.CurrentGradeMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Grade.Model.CurrentGradeMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Grade.Model.CurrentGradeMaster> GetAsync(
            #endif
            GetCurrentGradeMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            GetCurrentGradeMasterResult result = null;
            try {
                result = await this._client.GetCurrentGradeMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                    );
                this._gs2.Cache.Put<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "currentGradeMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CurrentGradeMaster"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> UpdateFuture(
            UpdateCurrentGradeMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateCurrentGradeMasterFuture(
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CurrentGradeMaster"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> UpdateAsync(
            #endif
            UpdateCurrentGradeMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            UpdateCurrentGradeMasterResult result = null;
                result = await this._client.UpdateCurrentGradeMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CurrentGradeMaster"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> Update(
            UpdateCurrentGradeMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> UpdateFromGitHubFuture(
            UpdateCurrentGradeMasterFromGitHubRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateCurrentGradeMasterFromGitHubFuture(
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
                        var parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CurrentGradeMaster"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> UpdateFromGitHubAsync(
            #else
        public async Task<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> UpdateFromGitHubAsync(
            #endif
            UpdateCurrentGradeMasterFromGitHubRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            UpdateCurrentGradeMasterFromGitHubResult result = null;
                result = await this._client.UpdateCurrentGradeMasterFromGitHubAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Grade.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CurrentGradeMaster"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain> UpdateFromGitHub(
            UpdateCurrentGradeMasterFromGitHubRequest request
        ) {
            return UpdateFromGitHubFuture(request);
        }
        #endif

    }

    public partial class CurrentGradeMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Grade.Model.CurrentGradeMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Grade.Model.CurrentGradeMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                    _parentKey,
                    Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetCurrentGradeMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "currentGradeMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                        _parentKey,
                        Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Grade.Model.CurrentGradeMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Grade.Model.CurrentGradeMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Grade.Model.CurrentGradeMaster> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                _parentKey,
                Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                    _parentKey,
                    Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetCurrentGradeMasterRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "currentGradeMaster")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                        _parentKey,
                        Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Grade.Model.CurrentGradeMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Grade.Model.CurrentGradeMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Grade.Model.CurrentGradeMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                _parentKey,
                Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Grade.Model.CurrentGradeMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Grade.Model.CurrentGradeMaster>(
                _parentKey,
                Gs2.Gs2Grade.Domain.Model.CurrentGradeMasterDomain.CreateCacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Grade.Model.CurrentGradeMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Grade.Model.CurrentGradeMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Grade.Model.CurrentGradeMaster> callback)
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

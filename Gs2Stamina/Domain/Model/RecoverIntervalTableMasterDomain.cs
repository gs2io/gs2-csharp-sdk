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
using Gs2.Gs2Stamina.Domain.Iterator;
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Result;
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

namespace Gs2.Gs2Stamina.Domain.Model
{

    public partial class RecoverIntervalTableMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2StaminaRestClient _client;
        private readonly string _namespaceName;
        private readonly string _recoverIntervalTableName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string RecoverIntervalTableName => _recoverIntervalTableName;

        public RecoverIntervalTableMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string recoverIntervalTableName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2StaminaRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._recoverIntervalTableName = recoverIntervalTableName;
            this._parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "RecoverIntervalTableMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string recoverIntervalTableName,
            string childType
        )
        {
            return string.Join(
                ":",
                "stamina",
                namespaceName ?? "null",
                recoverIntervalTableName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string recoverIntervalTableName
        )
        {
            return string.Join(
                ":",
                recoverIntervalTableName ?? "null"
            );
        }

    }

    public partial class RecoverIntervalTableMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> GetFuture(
            GetRecoverIntervalTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRecoverIntervalTableName(this.RecoverIntervalTableName);
                var future = this._client.GetRecoverIntervalTableMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                            request.RecoverIntervalTableName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "recoverIntervalTableMaster")
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
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RecoverIntervalTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> GetAsync(
            #endif
            GetRecoverIntervalTableMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRecoverIntervalTableName(this.RecoverIntervalTableName);
            GetRecoverIntervalTableMasterResult result = null;
            try {
                result = await this._client.GetRecoverIntervalTableMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                    request.RecoverIntervalTableName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "recoverIntervalTableMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RecoverIntervalTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> UpdateFuture(
            UpdateRecoverIntervalTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRecoverIntervalTableName(this.RecoverIntervalTableName);
                var future = this._client.UpdateRecoverIntervalTableMasterFuture(
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
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RecoverIntervalTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> UpdateAsync(
            #endif
            UpdateRecoverIntervalTableMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRecoverIntervalTableName(this.RecoverIntervalTableName);
            UpdateRecoverIntervalTableMasterResult result = null;
                result = await this._client.UpdateRecoverIntervalTableMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RecoverIntervalTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> Update(
            UpdateRecoverIntervalTableMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> DeleteFuture(
            DeleteRecoverIntervalTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRecoverIntervalTableName(this.RecoverIntervalTableName);
                var future = this._client.DeleteRecoverIntervalTableMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                            request.RecoverIntervalTableName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "recoverIntervalTableMaster")
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
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RecoverIntervalTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> DeleteAsync(
            #endif
            DeleteRecoverIntervalTableMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRecoverIntervalTableName(this.RecoverIntervalTableName);
            DeleteRecoverIntervalTableMasterResult result = null;
            try {
                result = await this._client.DeleteRecoverIntervalTableMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                    request.RecoverIntervalTableName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "recoverIntervalTableMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RecoverIntervalTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain> Delete(
            DeleteRecoverIntervalTableMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class RecoverIntervalTableMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                        this.RecoverIntervalTableName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetRecoverIntervalTableMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                                    this.RecoverIntervalTableName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "recoverIntervalTableMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                            this.RecoverIntervalTableName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                    this.RecoverIntervalTableName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                        this.RecoverIntervalTableName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetRecoverIntervalTableMasterRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                                    this.RecoverIntervalTableName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "recoverIntervalTableMaster")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                            this.RecoverIntervalTableName?.ToString()
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
        public async UniTask<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                    this.RecoverIntervalTableName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                    this.RecoverIntervalTableName.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.RecoverIntervalTableMasterDomain.CreateCacheKey(
                    this.RecoverIntervalTableName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Stamina.Model.RecoverIntervalTableMaster> callback)
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

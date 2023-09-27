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

    public partial class RecoverValueTableMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2StaminaRestClient _client;
        private readonly string _namespaceName;
        private readonly string _recoverValueTableName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string RecoverValueTableName => _recoverValueTableName;

        public RecoverValueTableMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string recoverValueTableName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2StaminaRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._recoverValueTableName = recoverValueTableName;
            this._parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "RecoverValueTableMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string recoverValueTableName,
            string childType
        )
        {
            return string.Join(
                ":",
                "stamina",
                namespaceName ?? "null",
                recoverValueTableName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string recoverValueTableName
        )
        {
            return string.Join(
                ":",
                recoverValueTableName ?? "null"
            );
        }

    }

    public partial class RecoverValueTableMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> GetFuture(
            GetRecoverValueTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRecoverValueTableName(this.RecoverValueTableName);
                var future = this._client.GetRecoverValueTableMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                            request.RecoverValueTableName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "recoverValueTableMaster")
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRecoverValueTableName(this.RecoverValueTableName);
                GetRecoverValueTableMasterResult result = null;
                try {
                    result = await this._client.GetRecoverValueTableMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        request.RecoverValueTableName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "recoverValueTableMaster")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RecoverValueTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> GetAsync(
            GetRecoverValueTableMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRecoverValueTableName(this.RecoverValueTableName);
            var future = this._client.GetRecoverValueTableMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        request.RecoverValueTableName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "recoverValueTableMaster")
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRecoverValueTableName(this.RecoverValueTableName);
            GetRecoverValueTableMasterResult result = null;
            try {
                result = await this._client.GetRecoverValueTableMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                    request.RecoverValueTableName.ToString()
                    );
                _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "recoverValueTableMaster")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RecoverValueTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> UpdateFuture(
            UpdateRecoverValueTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRecoverValueTableName(this.RecoverValueTableName);
                var future = this._client.UpdateRecoverValueTableMasterFuture(
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
                    .WithRecoverValueTableName(this.RecoverValueTableName);
                UpdateRecoverValueTableMasterResult result = null;
                    result = await this._client.UpdateRecoverValueTableMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RecoverValueTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
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
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> UpdateAsync(
            UpdateRecoverValueTableMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRecoverValueTableName(this.RecoverValueTableName);
            var future = this._client.UpdateRecoverValueTableMasterFuture(
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
                .WithRecoverValueTableName(this.RecoverValueTableName);
            UpdateRecoverValueTableMasterResult result = null;
                result = await this._client.UpdateRecoverValueTableMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RecoverValueTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> UpdateAsync(
            UpdateRecoverValueTableMasterRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> Update(
            UpdateRecoverValueTableMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> DeleteFuture(
            DeleteRecoverValueTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRecoverValueTableName(this.RecoverValueTableName);
                var future = this._client.DeleteRecoverValueTableMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                            request.RecoverValueTableName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "recoverValueTableMaster")
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRecoverValueTableName(this.RecoverValueTableName);
                DeleteRecoverValueTableMasterResult result = null;
                try {
                    result = await this._client.DeleteRecoverValueTableMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        request.RecoverValueTableName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "recoverValueTableMaster")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RecoverValueTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> DeleteAsync(
            DeleteRecoverValueTableMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRecoverValueTableName(this.RecoverValueTableName);
            var future = this._client.DeleteRecoverValueTableMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        request.RecoverValueTableName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "recoverValueTableMaster")
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRecoverValueTableName(this.RecoverValueTableName);
            DeleteRecoverValueTableMasterResult result = null;
            try {
                result = await this._client.DeleteRecoverValueTableMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                    request.RecoverValueTableName.ToString()
                    );
                _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "recoverValueTableMaster")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RecoverValueTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> DeleteAsync(
            DeleteRecoverValueTableMasterRequest request
        ) {
            var future = DeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain> Delete(
            DeleteRecoverValueTableMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class RecoverValueTableMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        this.RecoverValueTableName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetRecoverValueTableMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                                    this.RecoverValueTableName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "recoverValueTableMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                            this.RecoverValueTableName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                        this.RecoverValueTableName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetRecoverValueTableMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                                    this.RecoverValueTableName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "recoverValueTableMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                            this.RecoverValueTableName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Stamina.Model.RecoverValueTableMaster> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                    this.RecoverValueTableName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Stamina.Model.RecoverValueTableMaster>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.RecoverValueTableMasterDomain.CreateCacheKey(
                    this.RecoverValueTableName.ToString()
                ),
                callbackId
            );
        }

    }
}

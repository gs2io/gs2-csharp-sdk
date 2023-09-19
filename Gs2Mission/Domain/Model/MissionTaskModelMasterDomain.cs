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
using Gs2.Gs2Mission.Domain.Iterator;
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Result;
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

namespace Gs2.Gs2Mission.Domain.Model
{

    public partial class MissionTaskModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MissionRestClient _client;
        private readonly string _namespaceName;
        private readonly string _missionGroupName;
        private readonly string _missionTaskName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string MissionGroupName => _missionGroupName;
        public string MissionTaskName => _missionTaskName;

        public MissionTaskModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string missionGroupName,
            string missionTaskName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2MissionRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._missionGroupName = missionGroupName;
            this._missionTaskName = missionTaskName;
            this._parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.MissionGroupName,
                "MissionTaskModelMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string missionGroupName,
            string missionTaskName,
            string childType
        )
        {
            return string.Join(
                ":",
                "mission",
                namespaceName ?? "null",
                missionGroupName ?? "null",
                missionTaskName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string missionTaskName
        )
        {
            return string.Join(
                ":",
                missionTaskName ?? "null"
            );
        }

    }

    public partial class MissionTaskModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> GetFuture(
            GetMissionTaskModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName)
                    .WithMissionTaskName(this.MissionTaskName);
                var future = this._client.GetMissionTaskModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                            request.MissionTaskName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "missionTaskModelMaster")
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
                    .WithMissionGroupName(this.MissionGroupName)
                    .WithMissionTaskName(this.MissionTaskName);
                GetMissionTaskModelMasterResult result = null;
                try {
                    result = await this._client.GetMissionTaskModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        request.MissionTaskName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "missionTaskModelMaster")
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
                        var parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.MissionGroupName,
                            "MissionTaskModelMaster"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Mission.Model.MissionTaskModelMaster> GetAsync(
            GetMissionTaskModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            var future = this._client.GetMissionTaskModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        request.MissionTaskName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "missionTaskModelMaster")
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
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            GetMissionTaskModelMasterResult result = null;
            try {
                result = await this._client.GetMissionTaskModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                    request.MissionTaskName.ToString()
                    );
                _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "missionTaskModelMaster")
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
                    var parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.MissionGroupName,
                        "MissionTaskModelMaster"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> UpdateFuture(
            UpdateMissionTaskModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName)
                    .WithMissionTaskName(this.MissionTaskName);
                var future = this._client.UpdateMissionTaskModelMasterFuture(
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
                    .WithMissionGroupName(this.MissionGroupName)
                    .WithMissionTaskName(this.MissionTaskName);
                UpdateMissionTaskModelMasterResult result = null;
                    result = await this._client.UpdateMissionTaskModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.MissionGroupName,
                            "MissionTaskModelMaster"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> UpdateAsync(
            UpdateMissionTaskModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            var future = this._client.UpdateMissionTaskModelMasterFuture(
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
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            UpdateMissionTaskModelMasterResult result = null;
                result = await this._client.UpdateMissionTaskModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.MissionGroupName,
                        "MissionTaskModelMaster"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> UpdateAsync(
            UpdateMissionTaskModelMasterRequest request
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
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> Update(
            UpdateMissionTaskModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> DeleteFuture(
            DeleteMissionTaskModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName)
                    .WithMissionTaskName(this.MissionTaskName);
                var future = this._client.DeleteMissionTaskModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                            request.MissionTaskName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "missionTaskModelMaster")
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
                    .WithMissionGroupName(this.MissionGroupName)
                    .WithMissionTaskName(this.MissionTaskName);
                DeleteMissionTaskModelMasterResult result = null;
                try {
                    result = await this._client.DeleteMissionTaskModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        request.MissionTaskName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "missionTaskModelMaster")
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
                        var parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.MissionGroupName,
                            "MissionTaskModelMaster"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> DeleteAsync(
            DeleteMissionTaskModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            var future = this._client.DeleteMissionTaskModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        request.MissionTaskName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "missionTaskModelMaster")
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
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            DeleteMissionTaskModelMasterResult result = null;
            try {
                result = await this._client.DeleteMissionTaskModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                    request.MissionTaskName.ToString()
                    );
                _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "missionTaskModelMaster")
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
                    var parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.MissionGroupName,
                        "MissionTaskModelMaster"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> DeleteAsync(
            DeleteMissionTaskModelMasterRequest request
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
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> Delete(
            DeleteMissionTaskModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class MissionTaskModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        this.MissionTaskName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetMissionTaskModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                                    this.MissionTaskName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "missionTaskModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                            this.MissionTaskName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Model.MissionTaskModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        this.MissionTaskName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetMissionTaskModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                                    this.MissionTaskName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "missionTaskModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                            this.MissionTaskName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Model.MissionTaskModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Mission.Model.MissionTaskModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Mission.Model.MissionTaskModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}

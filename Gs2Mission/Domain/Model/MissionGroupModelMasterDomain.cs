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

    public partial class MissionGroupModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MissionRestClient _client;
        private readonly string _namespaceName;
        private readonly string _missionGroupName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string MissionGroupName => _missionGroupName;

        public MissionGroupModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string missionGroupName
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
            this._parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "MissionGroupModelMaster"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Mission.Model.MissionTaskModelMaster> MissionTaskModelMasters(
        )
        {
            return new DescribeMissionTaskModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.MissionGroupName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Mission.Model.MissionTaskModelMaster> MissionTaskModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Mission.Model.MissionTaskModelMaster> MissionTaskModelMasters(
            #endif
        #else
        public DescribeMissionTaskModelMastersIterator MissionTaskModelMasters(
        #endif
        )
        {
            return new DescribeMissionTaskModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.MissionGroupName
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeMissionTaskModelMasters(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName,
                    "MissionTaskModelMaster"
                ),
                callback
            );
        }

        public void UnsubscribeMissionTaskModelMasters(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName,
                    "MissionTaskModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain MissionTaskModelMaster(
            string missionTaskName
        ) {
            return new Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.MissionGroupName,
                missionTaskName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string missionGroupName,
            string childType
        )
        {
            return string.Join(
                ":",
                "mission",
                namespaceName ?? "null",
                missionGroupName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string missionGroupName
        )
        {
            return string.Join(
                ":",
                missionGroupName ?? "null"
            );
        }

    }

    public partial class MissionGroupModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> GetFuture(
            GetMissionGroupModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.GetMissionGroupModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                            request.MissionGroupName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "missionGroupModelMaster")
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
                    .WithMissionGroupName(this.MissionGroupName);
                GetMissionGroupModelMasterResult result = null;
                try {
                    result = await this._client.GetMissionGroupModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        request.MissionGroupName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "missionGroupModelMaster")
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
                        var parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MissionGroupModelMaster"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Mission.Model.MissionGroupModelMaster> GetAsync(
            GetMissionGroupModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.GetMissionGroupModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        request.MissionGroupName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "missionGroupModelMaster")
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
                .WithMissionGroupName(this.MissionGroupName);
            GetMissionGroupModelMasterResult result = null;
            try {
                result = await this._client.GetMissionGroupModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                    request.MissionGroupName.ToString()
                    );
                _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "missionGroupModelMaster")
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
                    var parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MissionGroupModelMaster"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> UpdateFuture(
            UpdateMissionGroupModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.UpdateMissionGroupModelMasterFuture(
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
                    .WithMissionGroupName(this.MissionGroupName);
                UpdateMissionGroupModelMasterResult result = null;
                    result = await this._client.UpdateMissionGroupModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MissionGroupModelMaster"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> UpdateAsync(
            UpdateMissionGroupModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.UpdateMissionGroupModelMasterFuture(
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
                .WithMissionGroupName(this.MissionGroupName);
            UpdateMissionGroupModelMasterResult result = null;
                result = await this._client.UpdateMissionGroupModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MissionGroupModelMaster"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> UpdateAsync(
            UpdateMissionGroupModelMasterRequest request
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
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> Update(
            UpdateMissionGroupModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> DeleteFuture(
            DeleteMissionGroupModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.DeleteMissionGroupModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                            request.MissionGroupName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "missionGroupModelMaster")
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
                    .WithMissionGroupName(this.MissionGroupName);
                DeleteMissionGroupModelMasterResult result = null;
                try {
                    result = await this._client.DeleteMissionGroupModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        request.MissionGroupName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "missionGroupModelMaster")
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
                        var parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MissionGroupModelMaster"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> DeleteAsync(
            DeleteMissionGroupModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.DeleteMissionGroupModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        request.MissionGroupName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "missionGroupModelMaster")
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
                .WithMissionGroupName(this.MissionGroupName);
            DeleteMissionGroupModelMasterResult result = null;
            try {
                result = await this._client.DeleteMissionGroupModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                    request.MissionGroupName.ToString()
                    );
                _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "missionGroupModelMaster")
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
                    var parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MissionGroupModelMaster"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> DeleteAsync(
            DeleteMissionGroupModelMasterRequest request
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
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> Delete(
            DeleteMissionGroupModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMasterFuture(
            CreateMissionTaskModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.CreateMissionTaskModelMasterFuture(
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
                    .WithMissionGroupName(this.MissionGroupName);
                CreateMissionTaskModelMasterResult result = null;
                    result = await this._client.CreateMissionTaskModelMasterAsync(
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
                var domain = new Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.MissionGroupName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMasterAsync(
            CreateMissionTaskModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            var future = this._client.CreateMissionTaskModelMasterFuture(
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
                .WithMissionGroupName(this.MissionGroupName);
            CreateMissionTaskModelMasterResult result = null;
                result = await this._client.CreateMissionTaskModelMasterAsync(
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
                var domain = new Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.MissionGroupName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMasterAsync(
            CreateMissionTaskModelMasterRequest request
        ) {
            var future = CreateMissionTaskModelMasterFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateMissionTaskModelMasterFuture.")]
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMaster(
            CreateMissionTaskModelMasterRequest request
        ) {
            return CreateMissionTaskModelMasterFuture(request);
        }
        #endif

    }

    public partial class MissionGroupModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        this.MissionGroupName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetMissionGroupModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                                    this.MissionGroupName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "missionGroupModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                            this.MissionGroupName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Mission.Model.MissionGroupModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        this.MissionGroupName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetMissionGroupModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                                    this.MissionGroupName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "missionGroupModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                            this.MissionGroupName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Model.MissionGroupModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Mission.Model.MissionGroupModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Mission.Model.MissionGroupModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Mission.Model.MissionGroupModelMaster> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                    this.MissionGroupName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                    this.MissionGroupName.ToString()
                ),
                callbackId
            );
        }

    }
}

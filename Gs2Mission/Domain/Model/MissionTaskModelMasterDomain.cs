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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Mission.Model.MissionTaskModelMaster> GetAsync(
            #else
        private IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Mission.Model.MissionTaskModelMaster> GetAsync(
        #endif
            GetMissionTaskModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetMissionTaskModelMasterFuture(
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
            var result = await this._client.GetMissionTaskModelMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> UpdateAsync(
        #endif
            UpdateMissionTaskModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.UpdateMissionTaskModelMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
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
            Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> DeleteAsync(
        #endif
            DeleteMissionTaskModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName)
                .WithMissionTaskName(this.MissionTaskName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteMissionTaskModelMasterFuture(
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
            DeleteMissionTaskModelMasterResult result = null;
            try {
                result = await this._client.DeleteMissionTaskModelMasterAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "missionTaskModelMaster")
                {
                    var parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName,
                    "MissionTaskModelMaster"
                );
                    var key = Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        request.MissionTaskName.ToString()
                    );
                    _cache.Delete<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(parentKey, key);
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
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
            Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain>(Impl);
        #endif
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Model.MissionTaskModelMaster> Model() {
            #else
        public IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Mission.Model.MissionTaskModelMaster> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster> self)
            {
        #endif
            Gs2.Gs2Mission.Model.MissionTaskModelMaster value = _cache.Get<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                    this.MissionTaskName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetMissionTaskModelMasterRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "missionTaskModelMaster")
                            {
                                _cache.Delete<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                                    _parentKey,
                                    Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                                        this.MissionTaskName?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "missionTaskModelMaster")
                    {
                        _cache.Delete<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                            _parentKey,
                            Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                                this.MissionTaskName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        this.MissionTaskName?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(Impl);
        #endif
        }

    }
}

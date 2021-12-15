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
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "MissionGroupModelMaster"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Mission.Model.MissionGroupModelMaster> GetAsync(
            #else
        private IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Mission.Model.MissionGroupModelMaster> GetAsync(
        #endif
            GetMissionGroupModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithMissionGroupName(this._missionGroupName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetMissionGroupModelMasterFuture(
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
            var result = await this._client.GetMissionGroupModelMasterAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        request.MissionGroupName != null ? request.MissionGroupName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> UpdateAsync(
        #endif
            UpdateMissionGroupModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithMissionGroupName(this._missionGroupName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.UpdateMissionGroupModelMasterAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                        request.MissionGroupName != null ? request.MissionGroupName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> DeleteAsync(
        #endif
            DeleteMissionGroupModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithMissionGroupName(this._missionGroupName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteMissionGroupModelMasterFuture(
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
            DeleteMissionGroupModelMasterResult result = null;
            try {
                result = await this._client.DeleteMissionGroupModelMasterAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            _cache.Delete<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                    request.MissionGroupName != null ? request.MissionGroupName.ToString() : null
                )
            );
            Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMasterAsync(
            #else
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMaster(
            #endif
        #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMasterAsync(
        #endif
            CreateMissionTaskModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithMissionGroupName(this._missionGroupName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            var result = await this._client.CreateMissionTaskModelMasterAsync(
                request
            );
            #endif
            string parentKey = Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._missionGroupName != null ? this._missionGroupName.ToString() : null,
                "MissionTaskModelMaster"
            );
                    
            if (result.Item != null) {
                _cache.Put(
                    parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain.CreateCacheKey(
                        result.Item?.Name?.ToString()
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain domain = new Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                request.MissionGroupName,
                result?.Item?.Name
            );

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
        public IUniTaskAsyncEnumerable<Gs2.Gs2Mission.Model.MissionTaskModelMaster> MissionTaskModelMasters(
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
                this._namespaceName,
                this._missionGroupName
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

        public Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain MissionTaskModelMaster(
            string missionTaskName
        ) {
            return new Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._missionGroupName,
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Mission.Model.MissionGroupModelMaster> Model() {
            #else
        public IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Mission.Model.MissionGroupModelMaster> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> self)
            {
        #endif
            Gs2.Gs2Mission.Model.MissionGroupModelMaster value = _cache.Get<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                    this.MissionGroupName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetMissionGroupModelMasterRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                            _parentKey,
                            Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                                this.MissionGroupName?.ToString()
                            )
                        );
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException) {
                    _cache.Delete<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                            this.MissionGroupName?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain.CreateCacheKey(
                    this.MissionGroupName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(Impl);
        #endif
        }

    }
}

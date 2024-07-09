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
using Gs2.Gs2Mission.Domain.Iterator;
using Gs2.Gs2Mission.Model.Cache;
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
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

namespace Gs2.Gs2Mission.Domain.Model
{

    public partial class MissionGroupModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MissionRestClient _client;
        public string NamespaceName { get; } = null!;
        public string MissionGroupName { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public MissionGroupModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string missionGroupName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MissionRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.MissionGroupName = missionGroupName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Mission.Model.MissionTaskModelMaster> MissionTaskModelMasters(
        )
        {
            return new DescribeMissionTaskModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.MissionGroupName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Mission.Model.MissionTaskModelMaster> MissionTaskModelMastersAsync(
            #else
        public DescribeMissionTaskModelMastersIterator MissionTaskModelMastersAsync(
            #endif
        )
        {
            return new DescribeMissionTaskModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.MissionGroupName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeMissionTaskModelMasters(
            Action<Gs2.Gs2Mission.Model.MissionTaskModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                (null as Gs2.Gs2Mission.Model.MissionTaskModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeMissionTaskModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2Mission.Model.MissionTaskModelMaster[]> callback
        )
        {
            var items = await MissionTaskModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeMissionTaskModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeMissionTaskModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Mission.Model.MissionTaskModelMaster>(
                (null as Gs2.Gs2Mission.Model.MissionTaskModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain MissionTaskModelMaster(
            string missionTaskName
        ) {
            return new Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                this.MissionGroupName,
                missionTaskName
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
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetMissionGroupModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Mission.Model.MissionGroupModelMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Mission.Model.MissionGroupModelMaster> GetAsync(
            #endif
            GetMissionGroupModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetMissionGroupModelMasterAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> UpdateFuture(
            UpdateMissionGroupModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateMissionGroupModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> UpdateAsync(
            #endif
            UpdateMissionGroupModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateMissionGroupModelMasterAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> DeleteFuture(
            DeleteMissionGroupModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteMissionGroupModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionGroupModelMasterDomain> DeleteAsync(
            #endif
            DeleteMissionGroupModelMasterRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteMissionGroupModelMasterAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMasterFuture(
            CreateMissionTaskModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateMissionTaskModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.MissionGroupName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMasterAsync(
            #else
        public async Task<Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain> CreateMissionTaskModelMasterAsync(
            #endif
            CreateMissionTaskModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateMissionTaskModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Mission.Domain.Model.MissionTaskModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                this.MissionGroupName,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

    }

    public partial class MissionGroupModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster> self)
            {
                var (value, find) = (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.MissionGroupName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.MissionGroupName,
                    () => this.GetFuture(
                        new GetMissionGroupModelMasterRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Mission.Model.MissionGroupModelMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Mission.Model.MissionGroupModelMaster> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.MissionGroupName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.MissionGroupName,
                () => this.GetAsync(
                    new GetMissionGroupModelMasterRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
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


        public void Invalidate()
        {
            (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.MissionGroupName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Mission.Model.MissionGroupModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).CacheKey(
                    this.MissionGroupName
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Mission.Model.MissionGroupModelMaster>(
                (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Mission.Model.MissionGroupModelMaster).CacheKey(
                    this.MissionGroupName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Mission.Model.MissionGroupModelMaster> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Mission.Model.MissionGroupModelMaster> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Mission.Model.MissionGroupModelMaster> callback)
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

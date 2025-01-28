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

    public partial class MissionGroupModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MissionRestClient _client;
        public string NamespaceName { get; } = null!;
        public string MissionGroupName { get; } = null!;

        public MissionGroupModelDomain(
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
        public Gs2Iterator<Gs2.Gs2Mission.Model.MissionTaskModel> MissionTaskModels(
        )
        {
            return new DescribeMissionTaskModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.MissionGroupName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Mission.Model.MissionTaskModel> MissionTaskModelsAsync(
            #else
        public DescribeMissionTaskModelsIterator MissionTaskModelsAsync(
            #endif
        )
        {
            return new DescribeMissionTaskModelsIterator(
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

        public ulong SubscribeMissionTaskModels(
            Action<Gs2.Gs2Mission.Model.MissionTaskModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Mission.Model.MissionTaskModel>(
                (null as Gs2.Gs2Mission.Model.MissionTaskModel).CacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await MissionTaskModelsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeMissionTaskModelsWithInitialCallAsync(
            Action<Gs2.Gs2Mission.Model.MissionTaskModel[]> callback
        )
        {
            var items = await MissionTaskModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeMissionTaskModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeMissionTaskModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Mission.Model.MissionTaskModel>(
                (null as Gs2.Gs2Mission.Model.MissionTaskModel).CacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName
                ),
                callbackId
            );
        }

        public void InvalidateMissionTaskModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Mission.Model.MissionTaskModel>(
                (null as Gs2.Gs2Mission.Model.MissionTaskModel).CacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName
                )
            );
        }

        public Gs2.Gs2Mission.Domain.Model.MissionTaskModelDomain MissionTaskModel(
            string missionTaskName
        ) {
            return new Gs2.Gs2Mission.Domain.Model.MissionTaskModelDomain(
                this._gs2,
                this.NamespaceName,
                this.MissionGroupName,
                missionTaskName
            );
        }

    }

    public partial class MissionGroupModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> GetFuture(
            GetMissionGroupModelRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetMissionGroupModelFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionGroupModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Mission.Model.MissionGroupModel> GetAsync(
            #else
        private async Task<Gs2.Gs2Mission.Model.MissionGroupModel> GetAsync(
            #endif
            GetMissionGroupModelRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetMissionGroupModelAsync(request)
            );
            return result?.Item;
        }
        #endif

    }

    public partial class MissionGroupModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> self)
            {
                var (value, find) = (null as Gs2.Gs2Mission.Model.MissionGroupModel).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.MissionGroupName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Mission.Model.MissionGroupModel).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.MissionGroupName,
                    () => this.GetFuture(
                        new GetMissionGroupModelRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Mission.Model.MissionGroupModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Mission.Model.MissionGroupModel> ModelAsync()
            #else
        public async Task<Gs2.Gs2Mission.Model.MissionGroupModel> ModelAsync()
            #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Mission.Model.MissionGroupModel>(
                        (null as Gs2.Gs2Mission.Model.MissionGroupModel).CacheParentKey(
                            this.NamespaceName
                        ),
                        (null as Gs2.Gs2Mission.Model.MissionGroupModel).CacheKey(
                            this.MissionGroupName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Mission.Model.MissionGroupModel).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.MissionGroupName
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Mission.Model.MissionGroupModel).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.MissionGroupName,
                    () => this.GetAsync(
                        new GetMissionGroupModelRequest()
                    )
                );
            }
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Mission.Model.MissionGroupModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Mission.Model.MissionGroupModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Mission.Model.MissionGroupModel).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.MissionGroupName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Mission.Model.MissionGroupModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Mission.Model.MissionGroupModel).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Mission.Model.MissionGroupModel).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Mission.Model.MissionGroupModel>(
                (null as Gs2.Gs2Mission.Model.MissionGroupModel).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Mission.Model.MissionGroupModel).CacheKey(
                    this.MissionGroupName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Mission.Model.MissionGroupModel> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Mission.Model.MissionGroupModel> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Mission.Model.MissionGroupModel> callback)
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

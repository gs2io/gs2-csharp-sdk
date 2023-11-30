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

    public partial class MissionGroupModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MissionRestClient _client;
        private readonly string _namespaceName;
        private readonly string _missionGroupName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string MissionGroupName => _missionGroupName;

        public MissionGroupModelDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string missionGroupName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MissionRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._missionGroupName = missionGroupName;
            this._parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "MissionGroupModel"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Mission.Model.MissionTaskModel> MissionTaskModels(
        )
        {
            return new DescribeMissionTaskModelsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.MissionGroupName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Mission.Model.MissionTaskModel> MissionTaskModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Mission.Model.MissionTaskModel> MissionTaskModels(
            #endif
        #else
        public DescribeMissionTaskModelsIterator MissionTaskModelsAsync(
        #endif
        )
        {
            return new DescribeMissionTaskModelsIterator(
                this._gs2.Cache,
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

        public ulong SubscribeMissionTaskModels(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Mission.Model.MissionTaskModel>(
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName,
                    "MissionTaskModel"
                ),
                callback
            );
        }

        public void UnsubscribeMissionTaskModels(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Mission.Model.MissionTaskModel>(
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.MissionGroupName,
                    "MissionTaskModel"
                ),
                callbackId
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

    public partial class MissionGroupModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> GetFuture(
            GetMissionGroupModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMissionGroupName(this.MissionGroupName);
                var future = this._client.GetMissionGroupModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                            request.MissionGroupName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Mission.Model.MissionGroupModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "missionGroupModel")
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
                        var parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MissionGroupModel"
                        );
                        var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMissionGroupName(this.MissionGroupName);
            GetMissionGroupModelResult result = null;
            try {
                result = await this._client.GetMissionGroupModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                    request.MissionGroupName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Mission.Model.MissionGroupModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "missionGroupModel")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Mission.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MissionGroupModel"
                    );
                    var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
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

    }

    public partial class MissionGroupModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Mission.Model.MissionGroupModel> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Mission.Model.MissionGroupModel>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                        this.MissionGroupName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetMissionGroupModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                                    this.MissionGroupName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Mission.Model.MissionGroupModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "missionGroupModel")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Mission.Model.MissionGroupModel>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                            this.MissionGroupName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
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
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Mission.Model.MissionGroupModel>(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                    this.MissionGroupName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Mission.Model.MissionGroupModel>(
                    _parentKey,
                    Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                        this.MissionGroupName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetMissionGroupModelRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                                    this.MissionGroupName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Mission.Model.MissionGroupModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "missionGroupModel")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Mission.Model.MissionGroupModel>(
                        _parentKey,
                        Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                            this.MissionGroupName?.ToString()
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


        public ulong Subscribe(Action<Gs2.Gs2Mission.Model.MissionGroupModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                    this.MissionGroupName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Mission.Model.MissionGroupModel>(
                _parentKey,
                Gs2.Gs2Mission.Domain.Model.MissionGroupModelDomain.CreateCacheKey(
                    this.MissionGroupName.ToString()
                ),
                callbackId
            );
        }

    }
}

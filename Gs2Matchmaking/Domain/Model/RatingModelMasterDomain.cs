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
using Gs2.Gs2Matchmaking.Domain.Iterator;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
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

namespace Gs2.Gs2Matchmaking.Domain.Model
{

    public partial class RatingModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _ratingName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string RatingName => _ratingName;

        public RatingModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string ratingName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._ratingName = ratingName;
            this._parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "RatingModelMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string ratingName,
            string childType
        )
        {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName ?? "null",
                ratingName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string ratingName
        )
        {
            return string.Join(
                ":",
                ratingName ?? "null"
            );
        }

    }

    public partial class RatingModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Matchmaking.Model.RatingModelMaster> GetFuture(
            GetRatingModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.RatingModelMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRatingName(this.RatingName);
                var future = this._client.GetRatingModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                            request.RatingName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "ratingModelMaster")
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
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RatingModelMaster"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Matchmaking.Model.RatingModelMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Matchmaking.Model.RatingModelMaster> GetAsync(
            #endif
            GetRatingModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRatingName(this.RatingName);
            GetRatingModelMasterResult result = null;
            try {
                result = await this._client.GetRatingModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                    request.RatingName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "ratingModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RatingModelMaster"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> UpdateFuture(
            UpdateRatingModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRatingName(this.RatingName);
                var future = this._client.UpdateRatingModelMasterFuture(
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
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RatingModelMaster"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> UpdateAsync(
            #endif
            UpdateRatingModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRatingName(this.RatingName);
            UpdateRatingModelMasterResult result = null;
                result = await this._client.UpdateRatingModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RatingModelMaster"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> Update(
            UpdateRatingModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> DeleteFuture(
            DeleteRatingModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithRatingName(this.RatingName);
                var future = this._client.DeleteRatingModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                            request.RatingName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "ratingModelMaster")
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
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "RatingModelMaster"
                        );
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> DeleteAsync(
            #endif
            DeleteRatingModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithRatingName(this.RatingName);
            DeleteRatingModelMasterResult result = null;
            try {
                result = await this._client.DeleteRatingModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                    request.RatingName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "ratingModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "RatingModelMaster"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain> Delete(
            DeleteRatingModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class RatingModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Model.RatingModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Model.RatingModelMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                    _parentKey,
                    Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                        this.RatingName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetRatingModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                                    this.RatingName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "ratingModelMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                            this.RatingName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Model.RatingModelMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Matchmaking.Model.RatingModelMaster> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                    this.RatingName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                    _parentKey,
                    Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                        this.RatingName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetRatingModelMasterRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                                    this.RatingName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "ratingModelMaster")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                        _parentKey,
                        Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                            this.RatingName?.ToString()
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
        public async UniTask<Gs2.Gs2Matchmaking.Model.RatingModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Model.RatingModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Matchmaking.Model.RatingModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Matchmaking.Model.RatingModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                    this.RatingName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Matchmaking.Model.RatingModelMaster>(
                _parentKey,
                Gs2.Gs2Matchmaking.Domain.Model.RatingModelMasterDomain.CreateCacheKey(
                    this.RatingName.ToString()
                ),
                callbackId
            );
        }

    }
}

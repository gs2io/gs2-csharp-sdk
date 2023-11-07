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
using Gs2.Gs2Limit.Domain.Iterator;
using Gs2.Gs2Limit.Request;
using Gs2.Gs2Limit.Result;
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

namespace Gs2.Gs2Limit.Domain.Model
{

    public partial class LimitModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LimitRestClient _client;
        private readonly string _namespaceName;
        private readonly string _limitName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string LimitName => _limitName;

        public LimitModelDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string limitName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LimitRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._limitName = limitName;
            this._parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "LimitModel"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string limitName,
            string childType
        )
        {
            return string.Join(
                ":",
                "limit",
                namespaceName ?? "null",
                limitName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string limitName
        )
        {
            return string.Join(
                ":",
                limitName ?? "null"
            );
        }

    }

    public partial class LimitModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Limit.Model.LimitModel> GetFuture(
            GetLimitModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Model.LimitModel> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithLimitName(this.LimitName);
                var future = this._client.GetLimitModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                            request.LimitName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Limit.Model.LimitModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "limitModel")
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "LimitModel"
                        );
                        var key = Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Limit.Model.LimitModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Limit.Model.LimitModel> GetAsync(
            #else
        private async Task<Gs2.Gs2Limit.Model.LimitModel> GetAsync(
            #endif
            GetLimitModelRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithLimitName(this.LimitName);
            GetLimitModelResult result = null;
            try {
                result = await this._client.GetLimitModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                    request.LimitName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Limit.Model.LimitModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "limitModel")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Limit.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "LimitModel"
                    );
                    var key = Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
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

    }

    public partial class LimitModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Limit.Model.LimitModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Limit.Model.LimitModel> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.LimitModel>(
                    _parentKey,
                    Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                        this.LimitName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetLimitModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                                    this.LimitName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Limit.Model.LimitModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "limitModel")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.LimitModel>(
                        _parentKey,
                        Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                            this.LimitName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Limit.Model.LimitModel>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Limit.Model.LimitModel> ModelAsync()
            #else
        public async Task<Gs2.Gs2Limit.Model.LimitModel> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.LimitModel>(
                    _parentKey,
                    Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                        this.LimitName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetLimitModelRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                                    this.LimitName?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Limit.Model.LimitModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors.Length == 0 || e.errors[0].component != "limitModel")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Limit.Model.LimitModel>(
                        _parentKey,
                        Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                            this.LimitName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Limit.Model.LimitModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Limit.Model.LimitModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Limit.Model.LimitModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Limit.Model.LimitModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                    this.LimitName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Limit.Model.LimitModel>(
                _parentKey,
                Gs2.Gs2Limit.Domain.Model.LimitModelDomain.CreateCacheKey(
                    this.LimitName.ToString()
                ),
                callbackId
            );
        }

    }
}

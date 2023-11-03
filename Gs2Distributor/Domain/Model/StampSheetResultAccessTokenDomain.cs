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
using Gs2.Gs2Distributor.Domain.Iterator;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
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

namespace Gs2.Gs2Distributor.Domain.Model
{

    public partial class StampSheetResultAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DistributorRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _transactionId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string TransactionId => _transactionId;

        public StampSheetResultAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string transactionId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DistributorRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._transactionId = transactionId;
            this._parentKey = Gs2.Gs2Distributor.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "StampSheetResult"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Distributor.Model.StampSheetResult> GetFuture(
            GetStampSheetResultRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Model.StampSheetResult> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithTransactionId(this.TransactionId);
                var future = this._client.GetStampSheetResultFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                            request.TransactionId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.StampSheetResult>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "stampSheetResult")
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
                        var parentKey = Gs2.Gs2Distributor.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "StampSheetResult"
                        );
                        var key = Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                            resultModel.Item.TransactionId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Model.StampSheetResult>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Distributor.Model.StampSheetResult> GetAsync(
            #else
        private async Task<Gs2.Gs2Distributor.Model.StampSheetResult> GetAsync(
            #endif
            GetStampSheetResultRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithTransactionId(this.TransactionId);
            GetStampSheetResultResult result = null;
            try {
                result = await this._client.GetStampSheetResultAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                    request.TransactionId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.StampSheetResult>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "stampSheetResult")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Distributor.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "StampSheetResult"
                    );
                    var key = Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                        resultModel.Item.TransactionId.ToString()
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

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string transactionId,
            string childType
        )
        {
            return string.Join(
                ":",
                "distributor",
                namespaceName ?? "null",
                userId ?? "null",
                transactionId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string transactionId
        )
        {
            return string.Join(
                ":",
                transactionId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Model.StampSheetResult> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Model.StampSheetResult> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Distributor.Model.StampSheetResult>(
                    _parentKey,
                    Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                        this.TransactionId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetStampSheetResultRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                                    this.TransactionId?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.StampSheetResult>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "stampSheetResult")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Distributor.Model.StampSheetResult>(
                        _parentKey,
                        Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                            this.TransactionId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Model.StampSheetResult>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Distributor.Model.StampSheetResult> ModelAsync()
            #else
        public async Task<Gs2.Gs2Distributor.Model.StampSheetResult> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Distributor.Model.StampSheetResult>(
                    _parentKey,
                    Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                        this.TransactionId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetStampSheetResultRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                                    this.TransactionId?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Distributor.Model.StampSheetResult>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "stampSheetResult")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Distributor.Model.StampSheetResult>(
                        _parentKey,
                        Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                            this.TransactionId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Distributor.Model.StampSheetResult> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Distributor.Model.StampSheetResult> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Distributor.Model.StampSheetResult> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Distributor.Model.StampSheetResult> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                    this.TransactionId.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Distributor.Model.StampSheetResult>(
                _parentKey,
                Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                    this.TransactionId.ToString()
                ),
                callbackId
            );
        }

    }
}

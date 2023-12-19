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
 *
 * deny overwrite
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
using Gs2.Gs2Showcase.Domain.Iterator;
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
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

namespace Gs2.Gs2Showcase.Domain.Model
{

    public partial class DisplayItemAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _showcaseName;
        private readonly string _displayItemId;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string ShowcaseName => _showcaseName;
        public string DisplayItemId => _displayItemId;

        public DisplayItemAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string showcaseName,
            string displayItemId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ShowcaseRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._showcaseName = showcaseName;
            this._displayItemId = displayItemId;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.ShowcaseName,
                "DisplayItem"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Showcase.Model.Showcase> GetFuture(
            GetShowcaseRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.Showcase> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithShowcaseName(this.ShowcaseName);
                var future = this._client.GetShowcaseFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheKey(
                            request.ShowcaseName.ToString()
                        );
                        _gs2.Cache.Put<Gs2.Gs2Showcase.Model.Showcase>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "showcase")
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithShowcaseName(this.ShowcaseName);
                GetShowcaseResult result = null;
                try {
                    result = await this._client.GetShowcaseAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheKey(
                        request.ShowcaseName.ToString()
                        );
                    _gs2.Cache.Put<Gs2.Gs2Showcase.Model.Showcase>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "showcase")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Showcase"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                        foreach (var displayItem in resultModel.Item.DisplayItems) {
                            cache.Put(
                                Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheParentKey(
                                    this.NamespaceName.ToString(),
                                    this.UserId.ToString(),
                                    resultModel.Item.Name.ToString(),
                                    "DisplayItem"
                                ),
                                Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheKey(
                                    displayItem.DisplayItemId.ToString()
                                ),
                                displayItem,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        cache.SetListCached<Gs2.Gs2Showcase.Model.DisplayItem>(
                            Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheParentKey(
                                this.NamespaceName.ToString(),
                                this.UserId.ToString(),
                                resultModel.Item.Name.ToString(),
                                "DisplayItem"
                            )
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.Showcase>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Showcase.Model.Showcase> GetAsync(
            #else
        private async Task<Gs2.Gs2Showcase.Model.Showcase> GetAsync(
            #endif
            GetShowcaseRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithShowcaseName(this.ShowcaseName);
            GetShowcaseResult result = null;
            try {
                result = await this._client.GetShowcaseAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheKey(
                    request.ShowcaseName.ToString()
                    );
                _gs2.Cache.Put<Gs2.Gs2Showcase.Model.Showcase>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "showcase")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = _gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Showcase"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseDomain.CreateCacheKey(
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
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> BuyFuture(
            BuyRequest request,
            bool speculativeExecute = true
        ) {

            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithShowcaseName(this.ShowcaseName)
                    .WithDisplayItemId(this.DisplayItemId);

                if (speculativeExecute) {
                    var speculativeExecuteFuture = Transaction.SpeculativeExecutor.BuyByUserIdSpeculativeExecutor.ExecuteFuture(
                        this._gs2,
                        AccessToken,
                        BuyByUserIdRequest.FromJson(request.ToJson())
                    );
                    yield return speculativeExecuteFuture;
                    if (speculativeExecuteFuture.Error != null)
                    {
                        self.OnError(speculativeExecuteFuture.Error);
                        yield break;
                    }
                    var commit = speculativeExecuteFuture.Result;
                    commit?.Invoke();
                }
                var future = this._client.BuyFuture(
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
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.DisplayItemDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.ShowcaseName,
                            this.DisplayItemId,
                            "SalesItem"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemDomain.CreateCacheKey(
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.AccessToken,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId
                );
                if (result.StampSheet != null) {
                    var future2 = transaction.WaitFuture(true);
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }
                self.OnComplete(transaction);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> BuyAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> BuyAsync(
            #endif
            BuyRequest request,
            bool speculativeExecute = true
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithShowcaseName(this.ShowcaseName)
                .WithDisplayItemId(this.DisplayItemId);

            if (speculativeExecute) {
                var commit = await Transaction.SpeculativeExecutor.BuyByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    BuyByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            BuyResult result = null;
                result = await this._client.BuyAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.DisplayItemDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.ShowcaseName,
                        this.DisplayItemId,
                        "SalesItem"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemDomain.CreateCacheKey(
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to BuyFuture.")]
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> Buy(
            BuyRequest request
        ) {
            return BuyFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemId,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                userId ?? "null",
                showcaseName ?? "null",
                displayItemId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string displayItemId
        )
        {
            return string.Join(
                ":",
                displayItemId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.DisplayItem> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.DisplayItem> self) {
                var future = this._gs2.Showcase.Namespace(
                    NamespaceName
                ).AccessToken(
                    AccessToken
                ).Showcase(
                    ShowcaseName
                ).ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result.DisplayItems.FirstOrDefault(v => v.DisplayItemId == DisplayItemId));
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.DisplayItem>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Showcase.Model.DisplayItem> ModelAsync()
            #else
        public async Task<Gs2.Gs2Showcase.Model.DisplayItem> ModelAsync()
            #endif
        {
            var item = await this._gs2.Showcase.Namespace(
                NamespaceName
            ).AccessToken(
                AccessToken
            ).Showcase(
                ShowcaseName
            ).ModelAsync();
            return item.DisplayItems.FirstOrDefault(v => v.DisplayItemId == DisplayItemId);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Showcase.Model.DisplayItem> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.DisplayItem> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Showcase.Model.DisplayItem> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Showcase.Namespace(
                NamespaceName
            ).AccessToken(
                AccessToken
            ).Showcase(
                ShowcaseName
            ).Invalidate();
        }

        public ulong Subscribe(Action<Gs2.Gs2Showcase.Model.DisplayItem> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.DisplayItemDomain.CreateCacheKey(
                    this.DisplayItemId.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Showcase.Model.DisplayItem>(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.DisplayItemDomain.CreateCacheKey(
                    this.DisplayItemId.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Showcase.Model.DisplayItem> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Showcase.Model.DisplayItem> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Showcase.Model.DisplayItem> callback)
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

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
using Gs2.Gs2Money.Domain.Iterator;
using Gs2.Gs2Money.Request;
using Gs2.Gs2Money.Result;
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

namespace Gs2.Gs2Money.Domain.Model
{

    public partial class WalletAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MoneyRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly int? _slot;

        private readonly String _parentKey;
        public float? Price { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public int? Slot => _slot;

        public WalletAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            int? slot
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MoneyRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._slot = slot;
            this._parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Wallet"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Money.Model.Wallet> GetFuture(
            GetWalletRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Money.Model.Wallet> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithSlot(this.Slot);
                var future = this._client.GetWalletFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                            request.Slot.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Money.Model.Wallet>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "wallet")
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
                        var parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Wallet"
                        );
                        var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                            this.Slot?.ToString() ?? "0"
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
            return new Gs2InlineFuture<Gs2.Gs2Money.Model.Wallet>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Money.Model.Wallet> GetAsync(
            #else
        private async Task<Gs2.Gs2Money.Model.Wallet> GetAsync(
            #endif
            GetWalletRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithSlot(this.Slot);
            GetWalletResult result = null;
            try {
                result = await this._client.GetWalletAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                    request.Slot.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Money.Model.Wallet>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "wallet")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Wallet"
                    );
                    var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                        this.Slot?.ToString() ?? "0"
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
        public IFuture<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> WithdrawFuture(
            WithdrawRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithSlot(this.Slot);
                var future = this._client.WithdrawFuture(
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
                        var parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Wallet"
                        );
                        var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                            this.Slot?.ToString() ?? "0"
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
                domain.Price = result?.Price;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> WithdrawAsync(
            #else
        public async Task<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> WithdrawAsync(
            #endif
            WithdrawRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithSlot(this.Slot);
            WithdrawResult result = null;
                result = await this._client.WithdrawAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Wallet"
                    );
                    var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                        this.Slot?.ToString() ?? "0"
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
            domain.Price = result?.Price;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to WithdrawFuture.")]
        public IFuture<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> Withdraw(
            WithdrawRequest request
        ) {
            return WithdrawFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string slot,
            string childType
        )
        {
            return string.Join(
                ":",
                "money",
                namespaceName ?? "null",
                userId ?? "null",
                slot ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string slot
        )
        {
            return string.Join(
                ":",
                slot ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money.Model.Wallet> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Money.Model.Wallet> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Money.Model.Wallet>(
                    _parentKey,
                    Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                        this.Slot?.ToString() ?? "0"
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetWalletRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                                    this.Slot?.ToString() ?? "0"
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Money.Model.Wallet>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "wallet")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Money.Model.Wallet>(
                        _parentKey,
                        Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                            this.Slot?.ToString() ?? "0"
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money.Model.Wallet>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money.Model.Wallet> ModelAsync()
            #else
        public async Task<Gs2.Gs2Money.Model.Wallet> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Money.Model.Wallet>(
                _parentKey,
                Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                    this.Slot?.ToString() ?? "0"
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Money.Model.Wallet>(
                    _parentKey,
                    Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                        this.Slot?.ToString() ?? "0"
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetWalletRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                                    this.Slot?.ToString() ?? "0"
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Money.Model.Wallet>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "wallet")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Money.Model.Wallet>(
                        _parentKey,
                        Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                            this.Slot?.ToString() ?? "0"
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
        public async UniTask<Gs2.Gs2Money.Model.Wallet> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Money.Model.Wallet> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Money.Model.Wallet> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Money.Model.Wallet>(
                _parentKey,
                Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                    this.Slot.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Money.Model.Wallet> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                    this.Slot.ToString()
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Money.Model.Wallet>(
                _parentKey,
                Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                    this.Slot.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Money.Model.Wallet> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Money.Model.Wallet> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Money.Model.Wallet> callback)
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

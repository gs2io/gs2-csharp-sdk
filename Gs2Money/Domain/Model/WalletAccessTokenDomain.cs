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
using Gs2.Gs2Money.Domain.Iterator;
using Gs2.Gs2Money.Request;
using Gs2.Gs2Money.Result;
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

namespace Gs2.Gs2Money.Domain.Model
{

    public partial class WalletAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MoneyRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;
        private readonly int? _slot;

        private readonly String _parentKey;
        public float? Price { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public int? Slot => _slot;

        public WalletAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            int? slot
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2MoneyRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._slot = slot;
            this._parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                "Wallet"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Money.Model.Wallet> GetAsync(
            #else
        private IFuture<Gs2.Gs2Money.Model.Wallet> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Money.Model.Wallet> GetAsync(
        #endif
            GetWalletRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Money.Model.Wallet> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithSlot(this._slot);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetWalletFuture(
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
            var result = await this._client.GetWalletAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "Wallet"
                );
                var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                        "null"
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
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
            return new Gs2InlineFuture<Gs2.Gs2Money.Model.Wallet>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> WithdrawAsync(
            #else
        public IFuture<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> Withdraw(
            #endif
        #else
        public async Task<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> WithdrawAsync(
        #endif
            WithdrawRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithSlot(this._slot);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            #else
            var result = await this._client.WithdrawAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "Wallet"
                );
                var key = Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                        "null"
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain domain = this;
            domain.Price = result?.Price;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Money.Domain.Model.WalletAccessTokenDomain>(Impl);
        #endif
        }

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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Money.Model.Wallet> Model() {
            #else
        public IFuture<Gs2.Gs2Money.Model.Wallet> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Money.Model.Wallet> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Money.Model.Wallet> self)
            {
        #endif
            Gs2.Gs2Money.Model.Wallet value = _cache.Get<Gs2.Gs2Money.Model.Wallet>(
                _parentKey,
                Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                          "null"
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetWalletRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "wallet")
                            {
                                _cache.Delete<Gs2.Gs2Money.Model.Wallet>(
                                    _parentKey,
                                    Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                                              "null"
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
                    if (e.errors[0].component == "wallet")
                    {
                    _cache.Delete<Gs2.Gs2Money.Model.Wallet>(
                            _parentKey,
                            Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                                      "null"
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Money.Model.Wallet>(
                _parentKey,
                Gs2.Gs2Money.Domain.Model.WalletDomain.CreateCacheKey(
                          "null"
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
            return new Gs2InlineFuture<Gs2.Gs2Money.Model.Wallet>(Impl);
        #endif
        }

    }
}

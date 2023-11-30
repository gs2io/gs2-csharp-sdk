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

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MoneyRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public float? Price { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MoneyRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Money.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Money.Model.Wallet> Wallets(
        )
        {
            return new DescribeWalletsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Money.Model.Wallet> WalletsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Money.Model.Wallet> Wallets(
            #endif
        #else
        public DescribeWalletsByUserIdIterator WalletsAsync(
        #endif
        )
        {
            return new DescribeWalletsByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
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

        public ulong SubscribeWallets(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money.Model.Wallet>(
                Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Wallet"
                ),
                callback
            );
        }

        public void UnsubscribeWallets(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money.Model.Wallet>(
                Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Wallet"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Money.Domain.Model.WalletDomain Wallet(
            int? slot
        ) {
            return new Gs2.Gs2Money.Domain.Model.WalletDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                slot
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Money.Model.Receipt> Receipts(
            int? slot,
            long? begin,
            long? end
        )
        {
            return new DescribeReceiptsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                slot,
                begin,
                end
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Money.Model.Receipt> ReceiptsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Money.Model.Receipt> Receipts(
            #endif
        #else
        public DescribeReceiptsIterator ReceiptsAsync(
        #endif
            int? slot,
            long? begin,
            long? end
        )
        {
            return new DescribeReceiptsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId,
                slot,
                begin,
                end
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

        public ulong SubscribeReceipts(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money.Model.Receipt>(
                Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Receipt"
                ),
                callback
            );
        }

        public void UnsubscribeReceipts(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money.Model.Receipt>(
                Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Receipt"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Money.Domain.Model.ReceiptDomain Receipt(
            string transactionId
        ) {
            return new Gs2.Gs2Money.Domain.Model.ReceiptDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                transactionId
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "money",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money.Domain.Model.ReceiptDomain> RecordReceiptFuture(
            RecordReceiptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Money.Domain.Model.ReceiptDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.RecordReceiptFuture(
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
                            "Receipt"
                        );
                        var key = Gs2.Gs2Money.Domain.Model.ReceiptDomain.CreateCacheKey(
                            resultModel.Item.TransactionId.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Money.Domain.Model.ReceiptDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TransactionId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money.Domain.Model.ReceiptDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money.Domain.Model.ReceiptDomain> RecordReceiptAsync(
            #else
        public async Task<Gs2.Gs2Money.Domain.Model.ReceiptDomain> RecordReceiptAsync(
            #endif
            RecordReceiptRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            RecordReceiptResult result = null;
                result = await this._client.RecordReceiptAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Receipt"
                    );
                    var key = Gs2.Gs2Money.Domain.Model.ReceiptDomain.CreateCacheKey(
                        resultModel.Item.TransactionId.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Money.Domain.Model.ReceiptDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TransactionId
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to RecordReceiptFuture.")]
        public IFuture<Gs2.Gs2Money.Domain.Model.ReceiptDomain> RecordReceipt(
            RecordReceiptRequest request
        ) {
            return RecordReceiptFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money.Domain.Model.ReceiptDomain> RevertRecordReceiptFuture(
            RevertRecordReceiptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Money.Domain.Model.ReceiptDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.RevertRecordReceiptFuture(
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
                            "Receipt"
                        );
                        var key = Gs2.Gs2Money.Domain.Model.ReceiptDomain.CreateCacheKey(
                            resultModel.Item.TransactionId.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Money.Domain.Model.ReceiptDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TransactionId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money.Domain.Model.ReceiptDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money.Domain.Model.ReceiptDomain> RevertRecordReceiptAsync(
            #else
        public async Task<Gs2.Gs2Money.Domain.Model.ReceiptDomain> RevertRecordReceiptAsync(
            #endif
            RevertRecordReceiptRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            RevertRecordReceiptResult result = null;
                result = await this._client.RevertRecordReceiptAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Money.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Receipt"
                    );
                    var key = Gs2.Gs2Money.Domain.Model.ReceiptDomain.CreateCacheKey(
                        resultModel.Item.TransactionId.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Money.Domain.Model.ReceiptDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.TransactionId
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to RevertRecordReceiptFuture.")]
        public IFuture<Gs2.Gs2Money.Domain.Model.ReceiptDomain> RevertRecordReceipt(
            RevertRecordReceiptRequest request
        ) {
            return RevertRecordReceiptFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

    }
}

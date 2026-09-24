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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Distributor.Domain.Iterator;
using Gs2.Gs2Distributor.Model.Cache;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Distributor.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DistributorRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
/* diff +++ start */
        public string NextPageToken { get; set; } = null!;
/* diff +++ end */

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DistributorRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }

/* diff +++ start */
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Distributor.Model.UserDataEntry> UserData(
            string timeOffsetToken = null
        )
        {
            return new DescribeUserDataByUserIdIterator(
                this._gs2,
                this._client,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Distributor.Model.UserDataEntry> UserDataAsync(
        #else
        public DescribeUserDataByUserIdIterator UserDataAsync(
        #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeUserDataByUserIdIterator(
                this._gs2,
                this._client,
                this.UserId,
                timeOffsetToken
            );
        }
/* diff +++ end */
        public Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain StampSheetResult(
            string transactionId
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.StampSheetResultDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                transactionId
            );
        }

        public Gs2.Gs2Distributor.Domain.Model.TransactionResultDomain TransactionResult(
            string transactionId
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.TransactionResultDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                transactionId
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
/* diff +++ start */
        public IFuture<Gs2.Core.Domain.Gs2> FreezeMasterDataFuture(
            FreezeMasterDataByUserIdRequest request
        ) => FreezeMasterDataAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.Gs2> FreezeMasterDataAsync(
        #else
        public async Task<Gs2.Core.Domain.Gs2> FreezeMasterDataAsync(
        #endif
            FreezeMasterDataByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.FreezeMasterDataByUserIdAsync(request)
            );
            var domain = this;
            var newGs2 =  new Core.Domain.Gs2(
                this._gs2.RestSession,
                this._gs2.WebSocketSession,
                this._gs2.DistributorNamespaceName
            );
            newGs2.DefaultContextStack = result?.NewContextStack;
            return newGs2;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff +++ end */
        public IFuture<Gs2.Gs2Distributor.Domain.Model.TransactionResultDomain> RunTransactionFuture(
            RunTransactionRequest request
        ) => RunTransactionAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.TransactionResultDomain> RunTransactionAsync(
        #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.TransactionResultDomain> RunTransactionAsync(
        #endif
            RunTransactionRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.RunTransactionAsync(request)
            );
            var domain = new Gs2.Gs2Distributor.Domain.Model.TransactionResultDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.TransactionId
            );

            return domain;
        }

    }

    public partial class UserDomain {

    }
}

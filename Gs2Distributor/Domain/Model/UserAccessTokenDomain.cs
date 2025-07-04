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

namespace Gs2.Gs2Distributor.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DistributorRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;

        public Gs2.Core.Domain.Gs2 Gs2 => this._gs2;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DistributorRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        public Gs2.Gs2Distributor.Domain.Model.StampSheetResultAccessTokenDomain StampSheetResult(
            string transactionId
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.StampSheetResultAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                transactionId
            );
        }
    
        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.Gs2> FreezeMasterDataFuture(
            FreezeMasterDataRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.Gs2> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    this.AccessToken?.TimeOffset,
                    () => this._client.FreezeMasterDataFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                var newGs2 =  new Core.Domain.Gs2(
                    this._gs2.RestSession,
                    this._gs2.WebSocketSession,
                    this._gs2.DistributorNamespaceName
                );
                newGs2.DefaultContextStack = result?.NewContextStack;
                self.OnComplete(newGs2);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.Gs2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.Gs2> FreezeMasterDataAsync(
            #else
        public async Task<Gs2.Core.Domain.Gs2> FreezeMasterDataAsync(
            #endif
            FreezeMasterDataRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                this.AccessToken?.TimeOffset,
                () => this._client.FreezeMasterDataAsync(request)
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
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.Gs2> FreezeMasterDataBySignedTimestampFuture(
            FreezeMasterDataBySignedTimestampRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.Gs2> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    this.AccessToken?.TimeOffset,
                    () => this._client.FreezeMasterDataBySignedTimestampFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                var newGs2 =  new Core.Domain.Gs2(
                    this._gs2.RestSession,
                    this._gs2.WebSocketSession,
                    this._gs2.DistributorNamespaceName
                );
                newGs2.DefaultContextStack = result?.NewContextStack;
                self.OnComplete(newGs2);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.Gs2>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.Gs2> FreezeMasterDataBySignedTimestampAsync(
            #else
        public async Task<Gs2.Core.Domain.Gs2> FreezeMasterDataBySignedTimestampAsync(
            #endif
            FreezeMasterDataBySignedTimestampRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                this.AccessToken?.TimeOffset,
                () => this._client.FreezeMasterDataBySignedTimestampAsync(request)
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
        #endif

        public Gs2.Gs2Distributor.Domain.Model.TransactionResultAccessTokenDomain TransactionResult(
            string transactionId
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.TransactionResultAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                transactionId
            );
        }

    }
}

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
using Gs2.Gs2Enhance.Domain.Iterator;
using Gs2.Gs2Enhance.Model.Cache;
using Gs2.Gs2Enhance.Request;
using Gs2.Gs2Enhance.Result;
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

namespace Gs2.Gs2Enhance.Domain.Model
{

    public partial class EnhanceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnhanceRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public long? AcquireExperience { get; set; } = null!;
        public float? BonusRate { get; set; } = null!;

        public EnhanceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnhanceRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }

    }

    public partial class EnhanceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> DirectFuture(
            DirectEnhanceByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DirectEnhanceByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.UserId,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId,
                    result.AtomicCommit,
                    result.TransactionResult,
                    result.Metadata
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
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionDomain> DirectAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionDomain> DirectAsync(
            #endif
            DirectEnhanceByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.DirectEnhanceByUserIdAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.UserId,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult,
                result.Metadata
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> UnleashFuture(
            UnleashByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.UnleashByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.UserId,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId,
                    result.AtomicCommit,
                    result.TransactionResult,
                    result.Metadata
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
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionDomain> UnleashAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionDomain> UnleashAsync(
            #endif
            UnleashByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.UnleashByUserIdAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.UserId,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult,
                result.Metadata
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

    }

    public partial class EnhanceDomain {

    }
}

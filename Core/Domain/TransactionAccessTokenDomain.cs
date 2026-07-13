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

using System;
using System.Collections;
using System.Collections.Generic;
using Gs2.Core.Exception;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public partial class TransactionAccessTokenDomain
    {

        protected readonly Gs2 Gs2;
        protected readonly AccessToken AccessToken;
        protected List<TransactionAccessTokenDomain> Actions;

        public TransactionAccessTokenDomain(
            Gs2 gs2,
            AccessToken accessToken,
            List<TransactionAccessTokenDomain> actions
        ) {
            this.Gs2 = gs2;
            this.AccessToken = accessToken;
            this.Actions = actions;
        }

        public string GetTransactionId()
        {
            return this switch {
                RanTransactionAccessTokenDomain => (this as RanTransactionAccessTokenDomain)?.TransactionId,
                AutoTransactionAccessTokenDomain => (this as AutoTransactionAccessTokenDomain)?.TransactionId,
                AutoStampSheetAccessTokenDomain => (this as AutoStampSheetAccessTokenDomain)?.TransactionId,
                ManualStampSheetAccessTokenDomain => (this as ManualStampSheetAccessTokenDomain)?.TransactionId,
                _ => null
            };
        }
        
        public string GetJobName()
        {
            return this switch {
                AutoJobQueueAccessTokenDomain => (this as AutoJobQueueAccessTokenDomain)?.JobName,
                ManualJobQueueAccessTokenDomain => (this as ManualJobQueueAccessTokenDomain)?.JobName,
                _ => null
            };
        }

#if UNITY_2017_1_OR_NEWER
        public virtual IFuture<TransactionAccessTokenDomain> WaitFuture(
            bool all = false
        ) => WaitAsync(all).ToGs2Future();
#endif
        
#if GS2_ENABLE_UNITASK
        public virtual async UniTask<TransactionAccessTokenDomain> WaitAsync(
#else
        public virtual async Task<TransactionAccessTokenDomain> WaitAsync(
#endif
            bool all = false
        ) {
            if (this.Actions.Count == 0) {
                return null;
            }
            var nextActions = new List<TransactionAccessTokenDomain>();
            foreach (var action in this.Actions) {
                var innerNext = await action.WaitAsync();
                if (innerNext != null) {
                    nextActions.Add(innerNext);
                }
            }
            var next = new TransactionAccessTokenDomain(
                this.Gs2,
                this.AccessToken,
                nextActions
            );
            if (!all) {
                await this.Gs2.DispatchAsync(this.AccessToken);
                return next;
            }
            await next.WaitAsync(true);
            await this.Gs2.DispatchAsync(this.AccessToken);
            return null;
        }
    }
}

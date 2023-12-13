
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
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public partial class TransactionDomain
    {

        protected readonly Gs2 Gs2;
        protected readonly string UserId;
        protected List<TransactionDomain> Actions;

        public TransactionDomain(
            Gs2 gs2,
            string userId,
            List<TransactionDomain> actions
        ) {
            this.Gs2 = gs2;
            this.UserId = userId;
            this.Actions = actions;
        }

#if UNITY_2017_1_OR_NEWER
        public virtual IFuture<TransactionDomain> WaitFuture(
            bool all = false
        ) {
            IEnumerator Impl(IFuture<TransactionDomain> self) {
                if (this.Actions.Count == 0) {
                    self.OnComplete(null);
                    yield break;
                }
                var nextActions = new List<TransactionDomain>();
                foreach (var action in this.Actions) {
                    var innerFuture = action.WaitFuture();
                    yield return innerFuture;
                    if (innerFuture.Error != null) {
                        self.OnError(innerFuture.Error);
                        yield break;
                    }
                    var innerNext = innerFuture.Result;
                    if (innerNext != null) {
                        nextActions.Add(innerNext);
                    }
                }
                var next = new TransactionDomain(
                    this.Gs2,
                    this.UserId,
                    nextActions
                );
                if (!all) {
                    self.OnComplete(next);
                    yield break;
                }
                var nextFuture = next.WaitFuture(true);
                yield return nextFuture;
                if (nextFuture.Error != null) {
                    self.OnError(nextFuture.Error);
                    yield break;
                }
                self.OnComplete(null);
            }
            return new Gs2InlineFuture<TransactionDomain>(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK

    #if UNITY_2017_1_OR_NEWER
        public virtual async UniTask<TransactionDomain> WaitAsync(
    #else
        public virtual async Task<TransactionDomain> WaitAsync(
    #endif
            bool all = false
        ) {
            if (this.Actions.Count == 0) {
                return null;
            }
            var nextActions = new List<TransactionDomain>();
            foreach (var action in this.Actions) {
                var innerNext = await action.WaitAsync();
                if (innerNext != null) {
                    nextActions.Add(innerNext);
                }
            }
            var next = new TransactionDomain(
                this.Gs2,
                this.UserId,
                nextActions
            );
            if (!all) {
                return next;
            }
            await next.WaitAsync(true);
            return null;
        }
#endif
    }
}

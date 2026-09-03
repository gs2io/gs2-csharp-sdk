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

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Gs2Lottery.Request;

namespace Gs2.Gs2Lottery.Model.Transaction
{
    public static partial class BoxItemsExt
    {
        public static bool IsExecutable(
            this BoxItems self,
            ResetBoxByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
            try {
                changed.Validate();
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
 diff --- end */
            return self.SpeculativeExecution(request) != null; /* diff +++ */
        }

        public static BoxItems SpeculativeExecution(
            this BoxItems self,
            ResetBoxByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Lottery:ResetBoxByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Lottery:ResetBoxByUserId");
//#endif
            return self.Clone() as BoxItems;
 diff --- end */
/* diff +++ start */
            if (self?.Items == null) {
                return null;
            }
            var items = new BoxItem[self.Items.Length];
            var hasKnownInitial = false;
            for (var i = 0; i < self.Items.Length; i++) {
                var item = self.Items[i];
                if (item == null) {
                    items[i] = null;
                    continue;
                }
                Gs2.Core.Model.AcquireAction[] acquireActions = null;
                if (item.AcquireActions != null) {
                    acquireActions = new Gs2.Core.Model.AcquireAction[
                        item.AcquireActions.Length
                    ];
                    for (var j = 0; j < item.AcquireActions.Length; j++) {
                        acquireActions[j] = item.AcquireActions[j]?.Clone() as
                            Gs2.Core.Model.AcquireAction;
                    }
                }
                hasKnownInitial |= item.Initial.HasValue;
                items[i] = new BoxItem()
                    .WithPrizeId(item.PrizeId)
                    .WithAcquireActions(acquireActions)
                    .WithRemaining(item.Initial ?? item.Remaining)
                    .WithInitial(item.Initial);
            }
            if (!hasKnownInitial) {
                return null;
            }
            return new BoxItems()
                .WithBoxId(self.BoxId)
                .WithPrizeTableName(self.PrizeTableName)
                .WithUserId(self.UserId)
                .WithItems(items);
/* diff +++ end */
        }

        public static ResetBoxByUserIdRequest Rate(
            this ResetBoxByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Lottery:ResetBoxByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class ResetBoxByUserIdRequestExt
    {
        public static ResetBoxByUserIdRequest Rate(
            this ResetBoxByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Lottery:ResetBoxByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
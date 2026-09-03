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
using System.Numerics;
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class SimpleItemExt
    {
        public static bool IsExecutable(
/* diff --- start
            this SimpleItem self,
 diff --- end */
            this SimpleItem[] self, /* diff +++ */
            ConsumeSimpleItemsByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }

/* diff --- start
        public static SimpleItem SpeculativeExecution(
            this SimpleItem self,
 diff --- end */
/* diff +++ start */
        public static SimpleItem[] SpeculativeExecution(
            this SimpleItem[] self,
/* diff +++ end */
            ConsumeSimpleItemsByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Inventory:ConsumeSimpleItemsByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Inventory:ConsumeSimpleItemsByUserId");
//#endif
            return self.Clone() as SimpleItem;
 diff --- end */
/* diff +++ start */
            if (self == null || request?.ConsumeCounts == null) {
                throw new NullReferenceException();
            }
            var clone = new SimpleItem[self.Length];
            for (var i = 0; i < self.Length; i++) {
                clone[i] = self[i]?.Clone() as SimpleItem;
                var item = clone[i];
                if (item == null || !item.Count.HasValue) continue;
                var changed = false;
                foreach (var consumeCount in request.ConsumeCounts) {
                    if (consumeCount?.ItemName != item.ItemName ||
                        !consumeCount.Count.HasValue) continue;
                    var count = checked(item.Count.Value - consumeCount.Count.Value);
                    if (count < 0) throw new InvalidOperationException();
                    item.Count = count;
                    changed = true;
                }
                if (changed) item.Revision = 0;
            }
            return clone;
/* diff +++ end */
        }

        public static ConsumeSimpleItemsByUserIdRequest Rate(
            this ConsumeSimpleItemsByUserIdRequest request,
            double rate
        ) {
            if (request?.ConsumeCounts == null) return null;
            foreach (var consumeCount in request.ConsumeCounts) {
                if (consumeCount?.Count == null) continue;
                if (!SimpleItemCountRate.TryApply(
                        consumeCount.Count.Value, rate, out var value
                    )) return null;
                consumeCount.Count = value;
            }
            return request;
        }
    }

    public static partial class ConsumeSimpleItemsByUserIdRequestExt
    {
        public static ConsumeSimpleItemsByUserIdRequest Rate(
            this ConsumeSimpleItemsByUserIdRequest request,
            BigInteger rate
        ) {
            if (request?.ConsumeCounts == null) return null;
            foreach (var consumeCount in request.ConsumeCounts) {
                if (consumeCount?.Count != null) {
                    consumeCount.Count = SimpleItemCountRate.Apply(
                        consumeCount.Count.Value,
                        rate
                    );
                }
            }
            return request;
        }
    }
}

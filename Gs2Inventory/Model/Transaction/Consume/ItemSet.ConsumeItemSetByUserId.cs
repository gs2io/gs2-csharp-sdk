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
    public static partial class ItemSetExt
    {
        public static bool IsExecutable(
            this ItemSet self,
            ConsumeItemSetByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }

        public static ItemSet SpeculativeExecution(
            this ItemSet self,
            ConsumeItemSetByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not ItemSet clone)
 diff --- end */
/* diff +++ start */
            var clone = self.Clone() as ItemSet;
            if (clone == null)
/* diff +++ end */
            {
                throw new NullReferenceException();
            }
/* diff --- start
            clone.Count -= request.ConsumeCount;
 diff --- end */
/* diff +++ start */
            if (!clone.Count.HasValue || request == null ||
                !request.ConsumeCount.HasValue) {
                throw new NullReferenceException();
            }
            if (request.ConsumeCount.Value <= 0) {
                return clone;
            }
            var count = checked(clone.Count.Value - request.ConsumeCount.Value);
            if (count < 0) throw new InvalidOperationException();
            clone.Count = count;
            return clone;
        }

        public static ItemSet[] SpeculativeExecution(
            this ItemSet[] self,
            ConsumeItemSetByUserIdRequest request
        ) {
            if (self == null || request == null) {
                throw new NullReferenceException();
            }
            var clone = new ItemSet[self.Length];
            for (var i = 0; i < self.Length; i++) {
                clone[i] = self[i]?.Clone() as ItemSet;
            }
            if (!request.ConsumeCount.HasValue || clone.Length == 0) {
                return clone;
            }
            if (request.ConsumeCount.Value <= 0) {
                return clone;
            }
            var target = -1;
            for (var i = clone.Length - 1; i >= 0; i--) {
                if (clone[i] == null) continue;
                if (request.ItemSetName != null) {
                    if (clone[i].Name == request.ItemSetName) {
                        target = i;
                        break;
                    }
                }
                else if (clone.Length == 1) {
                    target = i;
                }
            }
            if (target >= 0 && clone[target].Count.HasValue) {
                var count = checked(
                    clone[target].Count.Value - request.ConsumeCount.Value
                );
                if (count < 0) throw new InvalidOperationException();
                clone[target].Count = count;
            }
/* diff +++ end */
            return clone;
        }

        public static ConsumeItemSetByUserIdRequest Rate(
            this ConsumeItemSetByUserIdRequest request,
            double rate
        ) {
            if (request == null || !request.ConsumeCount.HasValue ||
                !VerifySimpleItemByUserIdRequestExt.TryApplyRate(
                    request.ConsumeCount.Value, rate, out var value
                )) return null;
            request.ConsumeCount = value;
            return request;
        }
    }

    public static partial class ConsumeItemSetByUserIdRequestExt
    {
        public static ConsumeItemSetByUserIdRequest Rate(
            this ConsumeItemSetByUserIdRequest request,
            BigInteger rate
        ) {
            if (request == null || !request.ConsumeCount.HasValue) return null;
            var value = new BigInteger(request.ConsumeCount.Value) * rate;
            if (value < long.MinValue || value > long.MaxValue) return null;
            request.ConsumeCount = (long)value;
            return request;
        }
    }
}

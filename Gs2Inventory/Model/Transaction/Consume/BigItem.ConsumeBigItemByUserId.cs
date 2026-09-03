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
    public static partial class BigItemExt
    {
        public static bool IsExecutable(
            this BigItem self,
            ConsumeBigItemByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }

        public static BigItem SpeculativeExecution(
            this BigItem self,
            ConsumeBigItemByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not BigItem clone)
 diff --- end */
/* diff +++ start */
            var clone = self.Clone() as BigItem;
            if (clone == null)
/* diff +++ end */
            {
                throw new NullReferenceException();
            }
/* diff --- start
            clone.Count -= request.ConsumeCount;
 diff --- end */
            if (!BigInteger.TryParse(clone.Count, out var currentCount) ||
                !BigInteger.TryParse(request?.ConsumeCount, out var consumeCount)) {
                throw new FormatException();
            }
            var changedCount = BigInteger.Subtract(currentCount, consumeCount);
            if (changedCount < BigInteger.Zero) {
                throw new InvalidOperationException();
            }
            clone.Count = changedCount.ToString("D"); /* diff +++ */
            clone.Revision = 0;
            return clone;
        }

        public static ConsumeBigItemByUserIdRequest Rate(
            this ConsumeBigItemByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inventory:ConsumeBigItemByUserId");
 diff --- end */
/* diff +++ start */
            if (request == null ||
                !VerifyBigItemByUserIdRequestExt.TryApplyRate(
                    SetBigItemByUserIdRequestExt.ParseCountOrOne(request.ConsumeCount),
                    rate, out var value
                )) {
                return null;
            }
            request.ConsumeCount = value.ToString("D");
            return request;
/* diff +++ end */
        }
    }

    public static partial class ConsumeBigItemByUserIdRequestExt
    {
        public static ConsumeBigItemByUserIdRequest Rate(
            this ConsumeBigItemByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inventory:ConsumeBigItemByUserId");
 diff --- end */
/* diff +++ start */
            if (request == null) return null;
            request.ConsumeCount = BigInteger.Multiply(
                SetBigItemByUserIdRequestExt.ParseCountOrOne(request.ConsumeCount),
                rate
            ).ToString("D");
            return request;
/* diff +++ end */
        }
    }
}

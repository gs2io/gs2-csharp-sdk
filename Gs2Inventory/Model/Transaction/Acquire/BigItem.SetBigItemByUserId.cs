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
            SetBigItemByUserIdRequest request
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
            SetBigItemByUserIdRequest request
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
            if (request?.Count == null) {
                throw new NullReferenceException();
            }
            clone.Count = request.Count;
            clone.Revision = 0;
            return clone;
        }

        public static SetBigItemByUserIdRequest Rate(
            this SetBigItemByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inventory:SetBigItemByUserId");
 diff --- end */
/* diff +++ start */
            if (request == null ||
                !VerifyBigItemByUserIdRequestExt.TryApplyRate(
                    SetBigItemByUserIdRequestExt.ParseCountOrOne(request.Count),
                    rate, out var value
                )) {
                return null;
            }
            request.Count = value.ToString("D");
            return request;
/* diff +++ end */
        }
    }

    public static partial class SetBigItemByUserIdRequestExt
    {
        public static SetBigItemByUserIdRequest Rate(
            this SetBigItemByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inventory:SetBigItemByUserId");
 diff --- end */
/* diff +++ start */
            if (request == null) return null;
            request.Count = BigInteger.Multiply(
                ParseCountOrOne(request.Count), rate
            ).ToString("D");
            return request;
/* diff +++ end */
        }

        internal static BigInteger ParseCountOrOne(string value) {
            if (string.IsNullOrEmpty(value)) return BigInteger.One;
            var sign = 1;
            var index = 0;
            if (value[0] == '+' || value[0] == '-') {
                sign = value[0] == '-' ? -1 : 1;
                index = 1;
            }
            if (index == value.Length) return BigInteger.One;
            var result = BigInteger.Zero;
            for (; index < value.Length; index++) {
                if (value[index] < '0' || value[index] > '9') {
                    return BigInteger.One;
                }
                result = result * 10 + (value[index] - '0');
            }
            return sign < 0 ? -result : result;
        }
    }
}

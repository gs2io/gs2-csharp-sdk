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
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class BigItemExt
    {
        public static bool IsExecutable(
            this BigItem self,
            VerifyBigItemByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    return self.Count < request.Count;
 diff --- end */
                    return BigInteger.Parse(self.Count) < BigInteger.Parse(request.Count); /* diff +++ */
                case "lessEqual":
/* diff --- start
                    return self.Count <= request.Count;
 diff --- end */
                    return BigInteger.Parse(self.Count) <= BigInteger.Parse(request.Count); /* diff +++ */
                case "greater":
/* diff --- start
                    return self.Count > request.Count;
 diff --- end */
                    return BigInteger.Parse(self.Count) > BigInteger.Parse(request.Count); /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    return self.Count >= request.Count;
 diff --- end */
                    return BigInteger.Parse(self.Count) >= BigInteger.Parse(request.Count); /* diff +++ */
                case "equal":
                    return BigInteger.Parse(self.Count) == BigInteger.Parse(request.Count);
                case "notEqual":
                    return BigInteger.Parse(self.Count) != BigInteger.Parse(request.Count);
            }
            return false;
        }

        public static BigItem SpeculativeExecution(
            this BigItem self,
            VerifyBigItemByUserIdRequest request
        ) {
            return self.Clone() as BigItem;
        }

        public static VerifyBigItemByUserIdRequest Rate(
            this VerifyBigItemByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inventory:VerifyBigItemByUserId");
 diff --- end */
/* diff +++ start */
            if (!VerifyBigItemByUserIdRequestExt.ShouldApplyRate(request)) {
                return request;
            }
            if (!BigInteger.TryParse(request.Count, out var count) ||
                !VerifyBigItemByUserIdRequestExt.TryApplyRate(
                    count,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.Count = value.ToString("D");
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyBigItemByUserIdRequestExt
    {
        public static VerifyBigItemByUserIdRequest Rate(
            this VerifyBigItemByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inventory:VerifyBigItemByUserId");
 diff --- end */
/* diff +++ start */
            if (!ShouldApplyRate(request)) {
                return request;
            }
            if (!BigInteger.TryParse(request.Count, out var count)) return null;
            request.Count = BigInteger.Multiply(count, rate).ToString("D");
            return request;
/* diff +++ end */
        }

        internal static bool ShouldApplyRate(VerifyBigItemByUserIdRequest request) {
            if (request == null) {
                return false;
            }
            if (request.MultiplyValueSpecifyingQuantity.HasValue) {
                return request.MultiplyValueSpecifyingQuantity.Value;
            }
            return request.VerifyType == "greater" ||
                   request.VerifyType == "greaterEqual";
        }

        internal static bool TryApplyRate(
            BigInteger count,
            double rate,
            out BigInteger value
        ) {
            value = count;
            if (double.IsNaN(rate) || double.IsInfinity(rate)) {
                return false;
            }
            var countExponent = 0;
            RoundTo100Bits(ref count, ref countExponent);
            var bits = BitConverter.DoubleToInt64Bits(rate);
            var exponentBits = (int)((bits >> 52) & 0x7ffL);
            var mantissa = new BigInteger(
                bits & 0x000fffffffffffffL
            );
            var exponent = -1074;
            if (exponentBits != 0) {
                mantissa += BigInteger.One << 52;
                exponent = exponentBits - 1075;
            }
            var product = count * mantissa;
            if (bits < 0) {
                product = -product;
            }
            exponent += countExponent;
            RoundTo100Bits(ref product, ref exponent);
            value = exponent >= 0
                ? product << exponent
                : product / (BigInteger.One << -exponent);
            return true;
        }

        private static void RoundTo100Bits(
            ref BigInteger value,
            ref int exponent
        ) {
            var absolute = BigInteger.Abs(value);
            var bitLength = 0;
            for (var remaining = absolute;
                 remaining > BigInteger.Zero;
                 remaining >>= 1) {
                bitLength++;
            }
            if (bitLength <= 100) {
                return;
            }
            var shift = bitLength - 100;
            var rounded = absolute >> shift;
            var remainder = absolute - (rounded << shift);
            var half = BigInteger.One << (shift - 1);
            if (remainder > half ||
                (remainder == half && !rounded.IsEven)) {
                rounded += BigInteger.One;
            }
            if ((rounded >> 100) != BigInteger.Zero) {
                rounded >>= 1;
                shift++;
            }
            value = value.Sign < 0 ? -rounded : rounded;
            exponent += shift;
        }
    }
}

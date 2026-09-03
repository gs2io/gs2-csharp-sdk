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
    public static partial class ItemSetExt
    {
        public static bool IsExecutable(
            this ItemSet self,
            VerifyItemSetByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.Count == null || request?.Count == null) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
                    return self.Count < request.Count;
                case "lessEqual":
                    return self.Count <= request.Count;
                case "greater":
                    return self.Count > request.Count;
                case "greaterEqual":
                    return self.Count >= request.Count;
                case "equal":
                    return self.Count == request.Count;
                case "notEqual":
                    return self.Count != request.Count;
            }
            return false;
        }

        public static ItemSet SpeculativeExecution(
            this ItemSet self,
            VerifyItemSetByUserIdRequest request
        ) {
            return self.Clone() as ItemSet;
        }

        public static VerifyItemSetByUserIdRequest Rate(
            this VerifyItemSetByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.Count = (long?) (request.Count * rate);
 diff --- end */
/* diff +++ start */
            if (!VerifyItemSetByUserIdRequestExt.ShouldApplyRate(request)) {
                return request;
            }
            if (!request.Count.HasValue ||
                !VerifyItemSetByUserIdRequestExt.TryApplyRate(
                    request.Count.Value, rate, out var value
                )) {
                return null;
            }
            request.Count = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class VerifyItemSetByUserIdRequestExt
    {
        public static VerifyItemSetByUserIdRequest Rate(
            this VerifyItemSetByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.Count = (long?) ((request.Count ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (!ShouldApplyRate(request)) {
                return request;
            }
            if (!request.Count.HasValue) return null;
            var value = new BigInteger(request.Count.Value) * rate;
            if (value < long.MinValue || value > long.MaxValue) {
                return null;
            }
            request.Count = (long)value;
/* diff +++ end */
            return request;
/* diff +++ start */
        }

        internal static bool ShouldApplyRate(VerifyItemSetByUserIdRequest request) {
            if (request == null) return false;
            if (request.MultiplyValueSpecifyingQuantity.HasValue) {
                return request.MultiplyValueSpecifyingQuantity.Value;
            }
            return request.VerifyType == "greater" ||
                   request.VerifyType == "greaterEqual";
        }

        internal static bool TryApplyRate(long count, double rate, out long value) {
            value = count;
            if (double.IsNaN(rate) || double.IsInfinity(rate)) return false;
            var bits = BitConverter.DoubleToInt64Bits(rate);
            var exponentBits = (int)((bits >> 52) & 0x7ffL);
            var mantissa = new BigInteger(bits & 0x000fffffffffffffL);
            var exponent = -1074;
            if (exponentBits != 0) {
                mantissa += BigInteger.One << 52;
                exponent = exponentBits - 1075;
            }
            var product = new BigInteger(count) * mantissa;
            if (bits < 0) product = -product;
            RoundTo100Bits(ref product, ref exponent);
            var result = exponent >= 0
                ? product << exponent
                : product / (BigInteger.One << -exponent);
            if (result < long.MinValue || result > long.MaxValue) return false;
            value = (long)result;
            return true;
        }

        private static void RoundTo100Bits(ref BigInteger value, ref int exponent) {
            var absolute = BigInteger.Abs(value);
            var bitLength = 0;
            for (var remaining = absolute; remaining > BigInteger.Zero; remaining >>= 1) {
                bitLength++;
            }
            if (bitLength <= 100) return;
            var shift = bitLength - 100;
            var rounded = absolute >> shift;
            var remainder = absolute - (rounded << shift);
            var half = BigInteger.One << (shift - 1);
            if (remainder > half || (remainder == half && !rounded.IsEven)) {
                rounded += BigInteger.One;
            }
            if ((rounded >> 100) != BigInteger.Zero) {
                rounded >>= 1;
                shift++;
            }
            value = value.Sign < 0 ? -rounded : rounded;
            exponent += shift;
/* diff +++ end */
        }
    }
}
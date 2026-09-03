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
    public static partial class InventoryExt
    {
        public static bool IsExecutable(
            this Inventory self,
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.CurrentInventoryMaxCapacity == null ||
                request?.CurrentInventoryMaxCapacity == null) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
                    return self.CurrentInventoryMaxCapacity < request.CurrentInventoryMaxCapacity;
                case "lessEqual":
                    return self.CurrentInventoryMaxCapacity <= request.CurrentInventoryMaxCapacity;
                case "greater":
                    return self.CurrentInventoryMaxCapacity > request.CurrentInventoryMaxCapacity;
                case "greaterEqual":
                    return self.CurrentInventoryMaxCapacity >= request.CurrentInventoryMaxCapacity;
                case "equal":
                    return self.CurrentInventoryMaxCapacity == request.CurrentInventoryMaxCapacity;
                case "notEqual":
                    return self.CurrentInventoryMaxCapacity != request.CurrentInventoryMaxCapacity;
            }
            return false;
        }

        public static Inventory SpeculativeExecution(
            this Inventory self,
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            return self.Clone() as Inventory;
        }

        public static VerifyInventoryCurrentMaxCapacityByUserIdRequest Rate(
            this VerifyInventoryCurrentMaxCapacityByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.CurrentInventoryMaxCapacity = (int?) (request.CurrentInventoryMaxCapacity * rate);
 diff --- end */
/* diff +++ start */
            if (!VerifyInventoryCurrentMaxCapacityByUserIdRequestExt.ShouldApplyRate(request)) {
                return request;
            }
            if (!request.CurrentInventoryMaxCapacity.HasValue ||
                !VerifyInventoryCurrentMaxCapacityByUserIdRequestExt.TryApplyRate(
                    request.CurrentInventoryMaxCapacity.Value,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.CurrentInventoryMaxCapacity = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class VerifyInventoryCurrentMaxCapacityByUserIdRequestExt
    {
        public static VerifyInventoryCurrentMaxCapacityByUserIdRequest Rate(
            this VerifyInventoryCurrentMaxCapacityByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.CurrentInventoryMaxCapacity = (int?) ((request.CurrentInventoryMaxCapacity ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (!ShouldApplyRate(request)) {
                return request;
            }
            if (!request.CurrentInventoryMaxCapacity.HasValue) return null;
            var value = new BigInteger(
                request.CurrentInventoryMaxCapacity.Value
            ) * rate;
            if (value < int.MinValue || value > int.MaxValue) {
                return null;
            }
            request.CurrentInventoryMaxCapacity = (int)value;
/* diff +++ end */
            return request;
/* diff +++ start */
        }

        internal static bool ShouldApplyRate(
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            return request?.MultiplyValueSpecifyingQuantity == true;
        }

        internal static bool TryApplyRate(
            int count,
            double rate,
            out int value
        ) {
            value = count;
            if (double.IsNaN(rate) || double.IsInfinity(rate)) {
                return false;
            }
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
            var product = new BigInteger(count) * mantissa;
            if (bits < 0) {
                product = -product;
            }
            RoundTo100Bits(ref product, ref exponent);
            var result = exponent >= 0
                ? product << exponent
                : product / (BigInteger.One << -exponent);
            if (result < int.MinValue || result > int.MaxValue) {
                return false;
            }
            value = (int)result;
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
/* diff +++ end */
        }
    }
}
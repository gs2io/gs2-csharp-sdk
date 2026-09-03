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
/* diff --- start
using System.Linq;
 diff --- end */
using System.Numerics;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class InventoryExt
    {
        public static bool IsExecutable(
            this Inventory self,
            SetCapacityByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
                self.SpeculativeExecution(request); /* diff +++ */
                return true;
            }
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (System.Exception) { /* diff +++ */
                return false;
            }
        }

        public static Inventory SpeculativeExecution(
            this Inventory self,
            SetCapacityByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not Inventory clone)
 diff --- end */
/* diff +++ start */
            if (self.Clone() is not Inventory clone ||
                request?.NewCapacityValue == null)
/* diff +++ end */
            {
                throw new NullReferenceException();
            }
            clone.CurrentInventoryMaxCapacity = request.NewCapacityValue;
            clone.Revision = 0; /* diff +++ */
            return clone;
        }

        public static SetCapacityByUserIdRequest Rate(
            this SetCapacityByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.NewCapacityValue = (int?) (request.NewCapacityValue * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !request.NewCapacityValue.HasValue ||
                !SetCapacityByUserIdRequestExt.TryApplyRate(
                    request.NewCapacityValue.Value, rate, out var value
                ) || !value.HasValue) {
                return null;
            }
            request.NewCapacityValue = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class SetCapacityByUserIdRequestExt
    {
        public static SetCapacityByUserIdRequest Rate(
            this SetCapacityByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.NewCapacityValue = (int?) ((request.NewCapacityValue ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !request.NewCapacityValue.HasValue) return null;
            var value = new BigInteger(request.NewCapacityValue.Value) * rate;
            if (value < int.MinValue || value > int.MaxValue) return null;
            request.NewCapacityValue = (int)value;
/* diff +++ end */
            return request;
/* diff +++ start */
        }

        internal static bool TryApplyRate(int count, double rate, out int? value) {
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
            value = result < int.MinValue || result > int.MaxValue
                ? (int?)null
                : (int)result;
            return true;
        }

        private static void RoundTo100Bits(ref BigInteger value, ref int exponent) {
            var absolute = BigInteger.Abs(value);
            var bitLength = 0;
            for (var remaining = absolute; remaining > 0; remaining >>= 1) bitLength++;
            if (bitLength <= 100) return;
            var shift = bitLength - 100;
            var rounded = absolute >> shift;
            var remainder = absolute - (rounded << shift);
            var half = BigInteger.One << (shift - 1);
            if (remainder > half || (remainder == half && !rounded.IsEven)) rounded++;
            if ((rounded >> 100) != 0) { rounded >>= 1; shift++; }
            value = value.Sign < 0 ? -rounded : rounded;
            exponent += shift;
/* diff +++ end */
        }
    }
}
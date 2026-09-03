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
            AcquireSimpleItemsByUserIdRequest request
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
            AcquireSimpleItemsByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Inventory:AcquireSimpleItemsByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Inventory:AcquireSimpleItemsByUserId");
//#endif
            return self.Clone() as SimpleItem;
 diff --- end */
/* diff +++ start */
            if (self == null || request?.AcquireCounts == null) {
                throw new NullReferenceException();
            }
            var clone = new SimpleItem[self.Length];
            for (var i = 0; i < self.Length; i++) {
                clone[i] = self[i]?.Clone() as SimpleItem;
                var item = clone[i];
                if (item == null || !item.Count.HasValue) continue;
                var changed = false;
                foreach (var acquireCount in request.AcquireCounts) {
                    if (acquireCount?.ItemName != item.ItemName ||
                        !acquireCount.Count.HasValue) continue;
                    item.Count = checked(item.Count.Value + acquireCount.Count.Value);
                    changed = true;
                }
                if (changed) item.Revision = 0;
            }
            return clone;
/* diff +++ end */
        }

        public static AcquireSimpleItemsByUserIdRequest Rate(
            this AcquireSimpleItemsByUserIdRequest request,
            double rate
        ) {
            if (request?.AcquireCounts == null) return null;
            foreach (var acquireCount in request.AcquireCounts) {
                if (acquireCount?.Count == null) continue;
                if (!SimpleItemCountRate.TryApply(
                        acquireCount.Count.Value, rate, out var value
                    )) return null;
                acquireCount.Count = value;
            }
            return request;
        }
    }

    public static partial class AcquireSimpleItemsByUserIdRequestExt
    {
        public static AcquireSimpleItemsByUserIdRequest Rate(
            this AcquireSimpleItemsByUserIdRequest request,
            BigInteger rate
        ) {
            if (request?.AcquireCounts == null) return null;
            foreach (var acquireCount in request.AcquireCounts) {
                if (acquireCount?.Count != null) {
                    acquireCount.Count = SimpleItemCountRate.Apply(
                        acquireCount.Count.Value,
                        rate
                    );
                }
            }
            return request;
        }
    }

    internal static class SimpleItemCountRate
    {
        internal static bool TryApply(
            long count,
            double rate,
            out long value
        ) {
            value = count;
            if (double.IsNaN(rate) || double.IsInfinity(rate)) {
                return false;
            }

            var bits = (ulong)BitConverter.DoubleToInt64Bits(rate);
            var exponentBits = (int)((bits >> 52) & 0x7ffUL);
            var fraction = bits & 0x000fffffffffffffUL;
            var significand = exponentBits == 0
                ? new BigInteger(fraction)
                : new BigInteger(fraction | 0x0010000000000000UL);
            if ((bits & 0x8000000000000000UL) != 0) {
                significand = BigInteger.Negate(significand);
            }
            var exponent = exponentBits == 0 ? -1074 : exponentBits - 1075;
            var product = new BigInteger(count) * significand;
            RoundTo100Bits(ref product, ref exponent);
            var scaled = exponent >= 0
                ? product << exponent
                : product / (BigInteger.One << -exponent);
            value = Saturate(scaled);
            return true;
        }

        internal static long Apply(long count, BigInteger rate) {
            return Saturate(new BigInteger(count) * rate);
        }

        private static long Saturate(BigInteger value) {
            if (value < long.MinValue) return long.MinValue;
            if (value > long.MaxValue) return long.MaxValue;
            return (long)value;
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
            if (bitLength <= 100) return;
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

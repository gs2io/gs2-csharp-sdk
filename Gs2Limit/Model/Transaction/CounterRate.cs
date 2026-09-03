/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Numerics;

namespace Gs2.Gs2Limit.Model.Transaction
{
    internal static class CounterRate
    {
        internal static bool TryApply(
            int count,
            double rate,
            out int value
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
            var exponent = exponentBits == 0
                ? -1074
                : exponentBits - 1075;

            var product = new BigInteger(count) * significand;
            var magnitude = BigInteger.Abs(product);
            var bytes = magnitude.ToByteArray();
            var last = bytes.Length - 1;
            while (last > 0 && bytes[last] == 0) {
                last--;
            }
            var mostSignificant = bytes[last];
            var mostSignificantBits = 0;
            while (mostSignificant != 0) {
                mostSignificantBits++;
                mostSignificant >>= 1;
            }
            var bitLength = last * 8 + mostSignificantBits;
            if (bitLength > 100) {
                var shift = bitLength - 100;
                var quotient = magnitude >> shift;
                var remainder = magnitude - (quotient << shift);
                var halfway = BigInteger.One << (shift - 1);
                if (remainder > halfway ||
                    (remainder == halfway && !quotient.IsEven)) {
                    quotient += BigInteger.One;
                }
                product = product.Sign < 0 ? -quotient : quotient;
                exponent += shift;
            }

            var scaled = exponent >= 0
                ? product << exponent
                : product / (BigInteger.One << -exponent);
            return TryConvert(scaled, count, out value);
        }

        internal static bool TryApply(
            int count,
            BigInteger rate,
            out int value
        ) {
            return TryConvert(
                new BigInteger(count) * rate,
                count,
                out value
            );
        }

        private static bool TryConvert(
            BigInteger scaled,
            int original,
            out int value
        ) {
            value = original;
            if (scaled <= int.MinValue || scaled > int.MaxValue) {
                return false;
            }
            value = (int)scaled;
            return true;
        }
    }
}

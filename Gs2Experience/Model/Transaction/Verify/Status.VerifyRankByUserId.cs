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
using Gs2.Gs2Experience.Request;

namespace Gs2.Gs2Experience.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            VerifyRankByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.RankValue == null || request == null) {
                return false;
            }
            var rankValue = request.RankValue ?? 0L;

/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
 diff --- end */
                    return self.RankValue.Value < rankValue; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
 diff --- end */
                    return self.RankValue.Value <= rankValue; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
 diff --- end */
                    return self.RankValue.Value > rankValue; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
 diff --- end */
                    return self.RankValue.Value >= rankValue; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
 diff --- end */
                    return self.RankValue.Value == rankValue; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
 diff --- end */
                    return self.RankValue.Value != rankValue; /* diff +++ */
            }
            return false;
        }

        public static Status SpeculativeExecution(
            this Status self,
            VerifyRankByUserIdRequest request
        ) {
            return self.Clone() as Status;
        }

        public static VerifyRankByUserIdRequest Rate(
            this VerifyRankByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Experience:VerifyRankByUserId");
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true) {
                return request;
            }
            if (!TryApplyServerRate(
                    request.RankValue ?? 0L,
                    rate,
                    out var value
                )) {
                return request;
            }
            request.RankValue = value;
            return request;
        }

        internal static bool TryApplyServerRate(
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
            if (scaled <= long.MinValue || scaled >= long.MaxValue) {
                return false;
            }
            value = (long)scaled;
            return true;
        }

        internal static bool TryApplyServerRate(
            long count,
            BigInteger rate,
            out long value
        ) {
            value = count;
            var scaled = new BigInteger(count) * rate;
            if (scaled <= long.MinValue || scaled >= long.MaxValue) {
                return false;
            }
            value = (long)scaled;
            return true;
/* diff +++ end */
        }
    }

    public static partial class VerifyRankByUserIdRequestExt
    {
        public static VerifyRankByUserIdRequest Rate(
            this VerifyRankByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Experience:VerifyRankByUserId");
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true) {
                return request;
            }
            var value = (request.RankValue ?? 0L) * rate;
            if (value <= long.MinValue || value >= long.MaxValue) {
                return request;
            }
            request.RankValue = (long)value;
            return request;
/* diff +++ end */
        }
    }
}
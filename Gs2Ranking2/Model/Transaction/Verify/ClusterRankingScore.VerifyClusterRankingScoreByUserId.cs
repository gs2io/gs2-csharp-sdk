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
using Gs2.Core.Model; /* diff +++ */
using Gs2.Gs2Ranking2.Request;

namespace Gs2.Gs2Ranking2.Model.Transaction
{
    public static partial class ClusterRankingScoreExt
    {
        public static bool IsExecutable(
            this ClusterRankingScore self,
            VerifyClusterRankingScoreByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.Score == null || request?.Score == null) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
                    return self.Score < request.Score;
                case "lessEqual":
                    return self.Score <= request.Score;
                case "greater":
                    return self.Score > request.Score;
                case "greaterEqual":
                    return self.Score >= request.Score;
                case "equal":
                    return self.Score == request.Score;
                case "notEqual":
                    return self.Score != request.Score;
            }
            return false;
        }

        public static ClusterRankingScore SpeculativeExecution(
            this ClusterRankingScore self,
            VerifyClusterRankingScoreByUserIdRequest request
        ) {
/* diff +++ start */
            if (!self.IsExecutable(request)) {
                var reason = FailureReason(request?.VerifyType);
                throw new BadRequestException(new [] {
                    new RequestError(
                        "score",
                        $"ranking2.clusterRankingScore.score.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as ClusterRankingScore;
/* diff +++ start */
        }

        private static string FailureReason(string verifyType)
        {
            switch (verifyType) {
                case "less": return "greaterEqual";
                case "lessEqual": return "greater";
                case "greater": return "lessEqual";
                case "greaterEqual": return "less";
                case "equal": return "notEqual";
                case "notEqual": return "equal";
                default: return "invalid";
            }
/* diff +++ end */
        }

        public static VerifyClusterRankingScoreByUserIdRequest Rate(
            this VerifyClusterRankingScoreByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.Score = (long?) (request.Score * rate);
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true) {
                return request;
            }
            if (!RankingScoreRate.TryApply(
                    request.Score ?? 1L,
                    rate,
                    out var value
                )) {
                return request;
            }
            request.Score = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class VerifyClusterRankingScoreByUserIdRequestExt
    {
        public static VerifyClusterRankingScoreByUserIdRequest Rate(
            this VerifyClusterRankingScoreByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.Score = (long?) ((request.Score ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true) {
                return request;
            }
            var value = (request.Score ?? 1L) * rate;
            if (value <= long.MinValue || value >= long.MaxValue) {
                return request;
            }
            request.Score = (long)value;
/* diff +++ end */
            return request;
        }
    }
/* diff +++ start */

    internal static class RankingScoreRate
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
    }
/* diff +++ end */
}
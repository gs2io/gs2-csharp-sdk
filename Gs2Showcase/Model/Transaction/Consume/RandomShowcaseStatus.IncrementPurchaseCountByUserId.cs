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
using Gs2.Gs2Showcase.Request;

namespace Gs2.Gs2Showcase.Model.Transaction
{
/* diff --- start
    public static partial class RandomShowcaseStatusExt
 diff --- end */
    public static partial class RandomDisplayItemExt /* diff +++ */
    {
        public static bool IsExecutable(
/* diff --- start
            this RandomShowcaseStatus self,
 diff --- end */
            this RandomDisplayItem self, /* diff +++ */
            IncrementPurchaseCountByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

/* diff --- start
        public static RandomShowcaseStatus SpeculativeExecution(
            this RandomShowcaseStatus self,
 diff --- end */
/* diff +++ start */
        public static RandomDisplayItem SpeculativeExecution(
            this RandomDisplayItem self,
/* diff +++ end */
            IncrementPurchaseCountByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not RandomShowcaseStatus clone)
 diff --- end */
/* diff +++ start */
            var clone = self.Clone() as RandomDisplayItem;
            if (clone == null)
/* diff +++ end */
            {
                throw new NullReferenceException();
            }
            if (request == null || clone.CurrentPurchaseCount == null ||
                clone.MaximumPurchaseCount == null) {
                throw new InvalidOperationException();
            }
            var changed = checked(
                clone.CurrentPurchaseCount.Value +
                request.Count.GetValueOrDefault(1)
            );
            if (changed > clone.MaximumPurchaseCount) {
                throw new InvalidOperationException();
            }
            clone.CurrentPurchaseCount = changed;
            return clone;
        }

        public static IncrementPurchaseCountByUserIdRequest Rate(
            this IncrementPurchaseCountByUserIdRequest request,
            double rate
        ) {
            if (request == null || !PurchaseCountRate.TryApply(
                    request.Count ?? 1,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.Count = value;
            return request;
        }
    }

    public static partial class IncrementPurchaseCountByUserIdRequestExt
    {
        public static IncrementPurchaseCountByUserIdRequest Rate(
            this IncrementPurchaseCountByUserIdRequest request,
            BigInteger rate
        ) {
            if (request == null || !PurchaseCountRate.TryApply(
                    request.Count ?? 1,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.Count = value;
            return request;
        }
    }

    internal static class PurchaseCountRate
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
            var scaled = exponent >= 0
                ? product << exponent
                : product / (BigInteger.One << -exponent);
            return TryConvert(scaled, out value);
        }

        internal static bool TryApply(
            int count,
            BigInteger rate,
            out int value
        ) {
            return TryConvert(new BigInteger(count) * rate, out value);
        }

        private static bool TryConvert(BigInteger value, out int result) {
            result = 0;
            if (value < int.MinValue || value > int.MaxValue) {
                return false;
            }
            result = (int)value;
            return true;
        }
    }
}

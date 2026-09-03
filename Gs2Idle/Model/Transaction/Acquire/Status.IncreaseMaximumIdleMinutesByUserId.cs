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
using Gs2.Core.Util; /* diff +++ */
using Gs2.Gs2Idle.Request;

namespace Gs2.Gs2Idle.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            IncreaseMaximumIdleMinutesByUserIdRequest request
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
            catch (Exception) { /* diff +++ */
                return false;
            }
        }

        public static Status SpeculativeExecution(
            this Status self,
            IncreaseMaximumIdleMinutesByUserIdRequest request
        ) {
/* diff +++ start */
            return self.SpeculativeExecutionAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        internal static Status SpeculativeExecutionAt(
            this Status self,
            IncreaseMaximumIdleMinutesByUserIdRequest request,
            long currentTimeMillis
        ) {
/* diff +++ end */
            if (self.Clone() is not Status clone)
            {
                throw new NullReferenceException();
            }
/* diff --- start
            clone.MaximumIdleMinutes += request.IncreaseMinutes;
 diff --- end */
/* diff +++ start */
            var value = request?.IncreaseMinutes ?? 1;
            if (clone.MaximumIdleMinutes == null) {
                throw new InvalidOperationException();
            }
            var changed = checked(clone.MaximumIdleMinutes.Value + value);
            clone.MaximumIdleMinutes = changed;
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
/* diff +++ end */
            return clone;
        }

        public static IncreaseMaximumIdleMinutesByUserIdRequest Rate(
            this IncreaseMaximumIdleMinutesByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.IncreaseMinutes = (int?) (request.IncreaseMinutes * rate);
 diff --- end */
/* diff +++ start */
            if (request != null) {
                request.IncreaseMinutes = MaximumIdleMinutesRate.TryApply(
                    request.IncreaseMinutes ?? 1,
                    rate,
                    out var value
                ) ? value : 0;
            }
/* diff +++ end */
            return request;
        }
    }

    public static partial class IncreaseMaximumIdleMinutesByUserIdRequestExt
    {
        public static IncreaseMaximumIdleMinutesByUserIdRequest Rate(
            this IncreaseMaximumIdleMinutesByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.IncreaseMinutes = (int?) ((request.IncreaseMinutes ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request != null) {
                request.IncreaseMinutes = MaximumIdleMinutesRate.TryApply(
                    request.IncreaseMinutes ?? 1,
                    rate,
                    out var value
                ) ? value : 0;
            }
/* diff +++ end */
            return request;
        }
    }
/* diff +++ start */

    internal static class MaximumIdleMinutesRate
    {
        internal static bool TryApply(int count, double rate, out int value) {
            value = 0;
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
                significand = -significand;
            }
            var exponent = exponentBits == 0 ? -1074 : exponentBits - 1075;
            var product = new BigInteger(count) * significand;
            var scaled = exponent >= 0
                ? product << exponent
                : product / (BigInteger.One << -exponent);
            return TryConvert(scaled, out value);
        }

        internal static bool TryApply(int count, BigInteger rate, out int value) {
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
/* diff +++ end */
}
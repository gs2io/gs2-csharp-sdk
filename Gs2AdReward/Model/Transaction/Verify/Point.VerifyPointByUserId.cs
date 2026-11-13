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
using Gs2.Gs2AdReward.Request;

namespace Gs2.Gs2AdReward.Model.Transaction
{
    public static partial class PointExt
    {
        public static bool IsExecutable(
            this Point self,
            VerifyPointByUserIdRequest request
        ) {
/* diff +++ start */
            // C# のモデルでは残高のプロパティは Point ではなく Value（クラス名と衝突するため）。
            if (self?.Value == null || request?.Point == null) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    return self.Point < request.Point;
 diff --- end */
                    return self.Value < request.Point; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    return self.Point <= request.Point;
 diff --- end */
                    return self.Value <= request.Point; /* diff +++ */
                case "greater":
/* diff --- start
                    return self.Point > request.Point;
 diff --- end */
                    return self.Value > request.Point; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    return self.Point >= request.Point;
 diff --- end */
                    return self.Value >= request.Point; /* diff +++ */
                case "equal":
/* diff --- start
                    return self.Point == request.Point;
 diff --- end */
                    return self.Value == request.Point; /* diff +++ */
                case "notEqual":
/* diff --- start
                    return self.Point != request.Point;
 diff --- end */
                    return self.Value != request.Point; /* diff +++ */
            }
            return false;
        }

        public static Point SpeculativeExecution(
            this Point self,
            VerifyPointByUserIdRequest request
        ) {
/* diff +++ start */
            var reason = PointVerificationFailureReason(request?.VerifyType);
            if (self?.Value != null && request?.Point != null &&
                reason != null && !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "point",
                        $"adReward.point.point.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as Point;
        }

/* diff +++ start */
        internal static string PointVerificationFailureReason(
            string verifyType
        ) {
            switch (verifyType) {
                case "less": return "greaterEqual";
                case "lessEqual": return "greater";
                case "greater": return "lessEqual";
                case "greaterEqual": return "less";
                case "equal": return "notEqual";
                case "notEqual": return "equal";
                default: return null;
            }
        }

/* diff +++ end */
        public static VerifyPointByUserIdRequest Rate(
            this VerifyPointByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.Point = (long?) (request.Point * rate);
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、64 ビットに収まらないときはしきい値を据え置く。
            if (!VerifyPointByUserIdRequestExt.ShouldApplyRate(request) ||
                !Gs2.Gs2Inventory.Model.Transaction
                    .VerifySimpleItemByUserIdRequestExt.TryApplyRate(
                        request.Point ?? 1, rate, out var value
                    ) ||
                !VerifyPointByUserIdRequestExt.IsWritable(value)) return request;
            request.Point = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class VerifyPointByUserIdRequestExt
    {
        public static VerifyPointByUserIdRequest Rate(
            this VerifyPointByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.Point = (long?) ((request.Point ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (!ShouldApplyRate(request)) return request;
            var value = new BigInteger(request.Point ?? 1) * rate;
            if (value <= long.MinValue || value >= long.MaxValue) return request;
            request.Point = (long)value;
/* diff +++ end */
            return request;
/* diff +++ start */
        }

        internal static bool ShouldApplyRate(
            VerifyPointByUserIdRequest request
        ) {
            if (request == null) return false;
            if (request.MultiplyValueSpecifyingQuantity.HasValue) {
                return request.MultiplyValueSpecifyingQuantity.Value;
            }
            return request.VerifyType == "greater" ||
                   request.VerifyType == "greaterEqual";
        }

        // サーバーは long の最小値と最大値の両方をしきい値として書かない (verify_count_write.go)。
        internal static bool IsWritable(long value)
        {
            return value != long.MinValue && value != long.MaxValue;
/* diff +++ end */
        }
    }
}
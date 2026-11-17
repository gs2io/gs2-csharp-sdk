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
using Gs2.Gs2Matchmaking.Request;

namespace Gs2.Gs2Matchmaking.Model.Transaction
{
    public static partial class RatingExt
    {
        public static bool IsExecutable(
            this Rating self,
            VerifyRatingByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.RateValue == null || request?.RateValue == null) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Matchmaking:VerifyRatingByUserId");
 diff --- end */
                    return self.RateValue < request.RateValue; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Matchmaking:VerifyRatingByUserId");
 diff --- end */
                    return self.RateValue <= request.RateValue; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Matchmaking:VerifyRatingByUserId");
 diff --- end */
                    return self.RateValue > request.RateValue; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Matchmaking:VerifyRatingByUserId");
 diff --- end */
                    return self.RateValue >= request.RateValue; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Matchmaking:VerifyRatingByUserId");
 diff --- end */
                    return self.RateValue == request.RateValue; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Matchmaking:VerifyRatingByUserId");
 diff --- end */
                    return self.RateValue != request.RateValue; /* diff +++ */
            }
            return false;
        }

        public static Rating SpeculativeExecution(
            this Rating self,
            VerifyRatingByUserIdRequest request
        ) {
/* diff +++ start */
            var reason = RatingVerificationFailureReason(request?.VerifyType);
            if (self?.RateValue != null && request?.RateValue != null &&
                reason != null && !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "rateValue",
                        $"matchmaking.rating.rateValue.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as Rating;
        }

/* diff +++ start */
        internal static string RatingVerificationFailureReason(
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
        public static VerifyRatingByUserIdRequest Rate(
            this VerifyRatingByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Matchmaking:VerifyRatingByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、レート値は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyRatingByUserIdRequestExt
    {
        public static VerifyRatingByUserIdRequest Rate(
            this VerifyRatingByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Matchmaking:VerifyRatingByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
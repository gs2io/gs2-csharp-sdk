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
using Gs2.Gs2Stamina.Request;

namespace Gs2.Gs2Stamina.Model.Transaction
{
    public static partial class StaminaExt
    {
        public static bool IsExecutable(
            this Stamina self,
            VerifyStaminaMaxValueByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "less":
                    return self.MaxValue < request.Value;
                case "lessEqual":
                    return self.MaxValue <= request.Value;
                case "greater":
                    return self.MaxValue > request.Value;
                case "greaterEqual":
                    return self.MaxValue >= request.Value;
                case "equal":
                    return self.MaxValue == request.Value;
                case "notEqual":
                    return self.MaxValue != request.Value;
            }
            return false;
        }

        public static Stamina SpeculativeExecution(
            this Stamina self,
            VerifyStaminaMaxValueByUserIdRequest request
        ) {
/* diff +++ start */
            var reason = StaminaVerificationFailureReason(request?.VerifyType);
            if (self?.MaxValue != null && request?.Value != null &&
                reason != null && !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "count",
                        $"stamina.stamina.count.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as Stamina;
/* diff +++ start */
        }

        internal static string StaminaVerificationFailureReason(
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
/* diff +++ end */
        }

        public static VerifyStaminaMaxValueByUserIdRequest Rate(
            this VerifyStaminaMaxValueByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.Value = (int?) (request.Value * rate);
 diff --- end */
/* diff +++ start */
            if (!VerifyStaminaMaxValueByUserIdRequestExt.ShouldApplyRate(request) ||
                !Gs2.Gs2Inventory.Model.Transaction
                    .SetCapacityByUserIdRequestExt.TryApplyRate(
                        request.Value ?? 1, rate, out var value
                    ) || !value.HasValue) return request;
            request.Value = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class VerifyStaminaMaxValueByUserIdRequestExt
    {
        public static VerifyStaminaMaxValueByUserIdRequest Rate(
            this VerifyStaminaMaxValueByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.Value = (int?) ((request.Value ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (!ShouldApplyRate(request)) return request;
            var value = new BigInteger(request.Value ?? 1) * rate;
            if (value <= int.MinValue || value > int.MaxValue) return request;
            request.Value = (int)value;
/* diff +++ end */
            return request;
/* diff +++ start */
        }

        internal static bool ShouldApplyRate(
            VerifyStaminaMaxValueByUserIdRequest request
        ) {
            if (request == null) return false;
            if (request.MultiplyValueSpecifyingQuantity.HasValue) {
                return request.MultiplyValueSpecifyingQuantity.Value;
            }
            return request.VerifyType == "greater" ||
                   request.VerifyType == "greaterEqual";
/* diff +++ end */
        }
    }
}
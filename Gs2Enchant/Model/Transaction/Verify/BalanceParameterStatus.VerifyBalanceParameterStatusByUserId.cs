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
using Gs2.Gs2Enchant.Request;

namespace Gs2.Gs2Enchant.Model.Transaction
{
    public static partial class BalanceParameterStatusExt
    {
        public static bool IsExecutable(
            this BalanceParameterStatus self,
            VerifyBalanceParameterStatusByUserIdRequest request
        ) {
/* diff +++ start */
            var current = BalanceParameterValueOf(self, request?.ParameterValueName);
            if (current == null || request?.ParameterValue == null) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Enchant:VerifyBalanceParameterStatusByUserId");
 diff --- end */
                    return current < request.ParameterValue; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Enchant:VerifyBalanceParameterStatusByUserId");
 diff --- end */
                    return current <= request.ParameterValue; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Enchant:VerifyBalanceParameterStatusByUserId");
 diff --- end */
                    return current > request.ParameterValue; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Enchant:VerifyBalanceParameterStatusByUserId");
 diff --- end */
                    return current >= request.ParameterValue; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Enchant:VerifyBalanceParameterStatusByUserId");
 diff --- end */
                    return current == request.ParameterValue; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Enchant:VerifyBalanceParameterStatusByUserId");
 diff --- end */
                    return current != request.ParameterValue; /* diff +++ */
            }
            return false;
        }

        public static BalanceParameterStatus SpeculativeExecution(
            this BalanceParameterStatus self,
            VerifyBalanceParameterStatusByUserIdRequest request
        ) {
/* diff +++ start */
            // 行に無いパラメータ名や、未知の検証の種類はサーバーが判定するので、ここでは投げない。
            var reason = BalanceParameterVerificationFailureReason(request?.VerifyType);
            if (BalanceParameterValueOf(self, request?.ParameterValueName) != null &&
                request?.ParameterValue != null &&
                reason != null && !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "parameterValue",
                        $"enchant.balanceParameterStatus.parameterValue.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as BalanceParameterStatus;
        }

/* diff +++ start */
        internal static long? BalanceParameterValueOf(
            BalanceParameterStatus self,
            string parameterValueName
        ) {
            if (self?.ParameterValues == null || parameterValueName == null) {
                return null;
            }
            return self.ParameterValues
                .FirstOrDefault(value => value?.Name == parameterValueName)?.Value;
        }

        internal static string BalanceParameterVerificationFailureReason(
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
        public static VerifyBalanceParameterStatusByUserIdRequest Rate(
            this VerifyBalanceParameterStatusByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Enchant:VerifyBalanceParameterStatusByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、比べる値は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyBalanceParameterStatusByUserIdRequestExt
    {
        public static VerifyBalanceParameterStatusByUserIdRequest Rate(
            this VerifyBalanceParameterStatusByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Enchant:VerifyBalanceParameterStatusByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
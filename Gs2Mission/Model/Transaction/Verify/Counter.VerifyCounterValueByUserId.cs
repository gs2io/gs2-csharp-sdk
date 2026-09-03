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
using Gs2.Core.Model;
using Gs2.Gs2Mission.Request;

namespace Gs2.Gs2Mission.Model.Transaction
{
    public static partial class CounterExt
    {
        public static bool IsExecutable(
            this Counter self,
            VerifyCounterValueByUserIdRequest request
        ) {
            if (self?.Values == null || request?.Value == null) return false;
            var current = self.Values.FirstOrDefault(v =>
                MatchesVerifyScope(v, request)
            )?.Value ?? 0;
            switch (request.VerifyType) {
                case "less":
                    return current < request.Value;
                case "lessEqual":
                    return current <= request.Value;
                case "greater":
                    return current > request.Value;
                case "greaterEqual":
                    return current >= request.Value;
                case "equal":
                    return current == request.Value;
                case "notEqual":
                    return current != request.Value;
            }
            return false;
        }

        internal static bool MatchesVerifyScope(
            ScopedValue value,
            VerifyCounterValueByUserIdRequest request
        ) {
            if (value == null || request == null ||
                value.ScopeType != request.ScopeType) return false;
            return request.ScopeType == "resetTiming"
                ? value.ResetType == request.ResetType
                : request.ScopeType == "verifyAction" &&
                  value.ConditionName == request.ConditionName;
        }

        public static Counter SpeculativeExecution(
            this Counter self,
            VerifyCounterValueByUserIdRequest request
        ) {
            if (!self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("value", "invalid"),
                });
            }
            return self.Clone() as Counter;
        }

        public static VerifyCounterValueByUserIdRequest Rate(
            this VerifyCounterValueByUserIdRequest request,
            double rate
        ) {
            if (request?.MultiplyValueSpecifyingQuantity != true ||
                !Gs2.Gs2Experience.Model.Transaction.StatusExt.TryApplyServerRate(
                    request.Value ?? 1L, rate, out var value
                )) return request;
            request.Value = value;
            return request;
        }
    }

    public static partial class VerifyCounterValueByUserIdRequestExt
    {
        public static VerifyCounterValueByUserIdRequest Rate(
            this VerifyCounterValueByUserIdRequest request,
            BigInteger rate
        ) {
            if (request?.MultiplyValueSpecifyingQuantity != true) return request;
            var value = new BigInteger(request.Value ?? 1L) * rate;
            if (value <= long.MinValue || value >= long.MaxValue) return request;
            request.Value = (long)value;
            return request;
        }
    }
}

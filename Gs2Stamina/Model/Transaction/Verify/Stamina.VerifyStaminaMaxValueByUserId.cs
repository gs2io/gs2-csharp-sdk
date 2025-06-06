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
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
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
            return self.Clone() as Stamina;
        }

        public static VerifyStaminaMaxValueByUserIdRequest Rate(
            this VerifyStaminaMaxValueByUserIdRequest request,
            double rate
        ) {
            request.Value = (int?) (request.Value * rate);
            return request;
        }
    }

    public static partial class VerifyStaminaMaxValueByUserIdRequestExt
    {
        public static VerifyStaminaMaxValueByUserIdRequest Rate(
            this VerifyStaminaMaxValueByUserIdRequest request,
            BigInteger rate
        ) {
            request.Value = (int?) ((request.Value ?? 0) * rate);
            return request;
        }
    }
}
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
 *
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Gs2Mission.Request;

namespace Gs2.Gs2Mission.Model.Transaction
{
    public static partial class CounterExt
    {
        public static bool IsExecutable(
            this Counter self,
            VerifyCounterValueByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "less":
                    return (self.Values?.FirstOrDefault(v => v.ResetType == request.ResetType)?.Value ?? 0) < request.Value;
                case "lessEqual":
                    return (self.Values?.FirstOrDefault(v => v.ResetType == request.ResetType)?.Value ?? 0) <= request.Value;
                case "greater":
                    return (self.Values?.FirstOrDefault(v => v.ResetType == request.ResetType)?.Value ?? 0) > request.Value;
                case "greaterEqual":
                    return (self.Values?.FirstOrDefault(v => v.ResetType == request.ResetType)?.Value ?? 0) >= request.Value;
                case "equal":
                    return (self.Values?.FirstOrDefault(v => v.ResetType == request.ResetType)?.Value ?? 0) == request.Value;
                case "notEqual":
                    return (self.Values?.FirstOrDefault(v => v.ResetType == request.ResetType)?.Value ?? 0) != request.Value;
            }
            return false;
        }

        public static Counter SpeculativeExecution(
            this Counter self,
            VerifyCounterValueByUserIdRequest request
        ) {
            return self.Clone() as Counter;
        }

        public static VerifyCounterValueByUserIdRequest Rate(
            this VerifyCounterValueByUserIdRequest request,
            double rate
        ) {
            request.Value = (long?) (request.Value * rate);
            return request;
        }
    }

    public static partial class VerifyCounterValueByUserIdRequestExt
    {
        public static VerifyCounterValueByUserIdRequest Rate(
            this VerifyCounterValueByUserIdRequest request,
            BigInteger rate
        ) {
            request.Value = (long?) ((request.Value ?? 0) * rate);
            return request;
        }
    }
}
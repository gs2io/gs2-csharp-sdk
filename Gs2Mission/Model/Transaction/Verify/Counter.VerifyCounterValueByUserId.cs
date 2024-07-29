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
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCounterValueByUserId");
                case "lessEqual":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCounterValueByUserId");
                case "greater":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCounterValueByUserId");
                case "greaterEqual":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCounterValueByUserId");
                case "equal":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCounterValueByUserId");
                case "notEqual":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCounterValueByUserId");
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
            throw new NotSupportedException($"not supported rate action Gs2Mission:VerifyCounterValueByUserId");
        }
    }

    public static partial class VerifyCounterValueByUserIdRequestExt
    {
        public static VerifyCounterValueByUserIdRequest Rate(
            this VerifyCounterValueByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Mission:VerifyCounterValueByUserId");
        }
    }
}
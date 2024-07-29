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
    public static partial class CompleteExt
    {
        public static bool IsExecutable(
            this Complete self,
            VerifyCompleteByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "completed":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
                case "notCompleted":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
                case "received":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
                case "notReceived":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
                case "completedAndNotReceived":
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
            }
            return false;
        }

        public static Complete SpeculativeExecution(
            this Complete self,
            VerifyCompleteByUserIdRequest request
        ) {
            return self.Clone() as Complete;
        }

        public static VerifyCompleteByUserIdRequest Rate(
            this VerifyCompleteByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Mission:VerifyCompleteByUserId");
        }
    }

    public static partial class VerifyCompleteByUserIdRequestExt
    {
        public static VerifyCompleteByUserIdRequest Rate(
            this VerifyCompleteByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Mission:VerifyCompleteByUserId");
        }
    }
}
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
using Gs2.Gs2Schedule.Request;

namespace Gs2.Gs2Schedule.Model.Transaction
{
    public static partial class TriggerExt
    {
        public static bool IsExecutable(
            this Trigger self,
            VerifyTriggerByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "notTriggerd":
                    throw new NotImplementedException($"not implemented action Gs2Schedule:VerifyTriggerByUserId");
                case "elapsed":
                    throw new NotImplementedException($"not implemented action Gs2Schedule:VerifyTriggerByUserId");
                case "notElapsed":
                    throw new NotImplementedException($"not implemented action Gs2Schedule:VerifyTriggerByUserId");
            }
            return false;
        }

        public static Trigger SpeculativeExecution(
            this Trigger self,
            VerifyTriggerByUserIdRequest request
        ) {
            return self.Clone() as Trigger;
        }

        public static VerifyTriggerByUserIdRequest Rate(
            this VerifyTriggerByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Schedule:VerifyTriggerByUserId");
        }
    }

    public static partial class VerifyTriggerByUserIdRequestExt
    {
        public static VerifyTriggerByUserIdRequest Rate(
            this VerifyTriggerByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Schedule:VerifyTriggerByUserId");
        }
    }
}
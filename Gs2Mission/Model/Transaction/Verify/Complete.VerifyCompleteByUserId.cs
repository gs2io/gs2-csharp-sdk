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
using Gs2.Gs2Mission.Request;

namespace Gs2.Gs2Mission.Model.Transaction
{
    public static partial class CompleteExt
    {
        public static bool IsExecutable(
            this Complete self,
            VerifyCompleteByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.CompletedMissionTaskNames == null ||
                self.ReceivedMissionTaskNames == null ||
                request?.MissionTaskName == null) {
                return false;
            }

            var completed = self.CompletedMissionTaskNames.Contains(request.MissionTaskName);
            var received = self.ReceivedMissionTaskNames.Contains(request.MissionTaskName);
/* diff +++ end */
            switch (request.VerifyType) {
                case "completed":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
 diff --- end */
                    return completed; /* diff +++ */
                case "notCompleted":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
 diff --- end */
                    return !completed; /* diff +++ */
                case "received":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
 diff --- end */
                    return received; /* diff +++ */
                case "notReceived":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
 diff --- end */
                    return !received; /* diff +++ */
                case "completedAndNotReceived":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Mission:VerifyCompleteByUserId");
 diff --- end */
                    return completed && !received; /* diff +++ */
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
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Mission:VerifyCompleteByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class VerifyCompleteByUserIdRequestExt
    {
        public static VerifyCompleteByUserIdRequest Rate(
            this VerifyCompleteByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Mission:VerifyCompleteByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
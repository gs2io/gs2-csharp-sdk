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
using Gs2.Core.Util; /* diff +++ */
using Gs2.Gs2Schedule.Request;

namespace Gs2.Gs2Schedule.Model.Transaction
{
    public static partial class TriggerExt
    {
        public static bool IsExecutable(
            this Trigger self,
            VerifyTriggerByUserIdRequest request
        ) {
/* diff +++ start */
            return self.IsExecutable(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static bool IsExecutable(
            this Trigger self,
            VerifyTriggerByUserIdRequest request,
            long currentTimeMillis
        ) {
            if (request == null) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "notTriggerd":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Schedule:VerifyTriggerByUserId");
 diff --- end */
/* diff +++ start */
                    return self == null ||
                           self.ExpiresAt != null &&
                           self.ExpiresAt.Value <= currentTimeMillis;
/* diff +++ end */
                case "elapsed":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Schedule:VerifyTriggerByUserId");
 diff --- end */
                case "notElapsed":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Schedule:VerifyTriggerByUserId");
 diff --- end */
/* diff +++ start */
                    if (self?.CreatedAt == null ||
                        request.ElapsedMinutes == null) {
                        return false;
                    }
                    var elapsedMinutes =
                        (new BigInteger(currentTimeMillis) - self.CreatedAt.Value) /
                        60000;
                    return request.VerifyType == "elapsed"
                        ? elapsedMinutes >= request.ElapsedMinutes.Value
                        : elapsedMinutes < request.ElapsedMinutes.Value;
/* diff +++ end */
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
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Schedule:VerifyTriggerByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class VerifyTriggerByUserIdRequestExt
    {
        public static VerifyTriggerByUserIdRequest Rate(
            this VerifyTriggerByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Schedule:VerifyTriggerByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
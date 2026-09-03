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
using Gs2.Gs2Schedule.Request;

namespace Gs2.Gs2Schedule.Model.Transaction
{
    public static partial class EventExt
    {
        public static bool IsExecutable(
            this Event self,
            VerifyEventByUserIdRequest request
        ) {
/* diff +++ start */
            return self.IsExecutable(request, null);
        }

        public static bool IsExecutable(
            this Event self,
            VerifyEventByUserIdRequest request,
            bool? inSchedule
        ) {
            if (self == null || request == null || inSchedule == null) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "inSchedule":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Schedule:VerifyEventByUserId");
 diff --- end */
                    return inSchedule.Value; /* diff +++ */
                case "notInSchedule":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Schedule:VerifyEventByUserId");
 diff --- end */
                    return !inSchedule.Value; /* diff +++ */
            }
            return false;
        }

        public static Event SpeculativeExecution(
            this Event self,
            VerifyEventByUserIdRequest request
        ) {
            return self.Clone() as Event;
        }

        public static VerifyEventByUserIdRequest Rate(
            this VerifyEventByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Schedule:VerifyEventByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class VerifyEventByUserIdRequestExt
    {
        public static VerifyEventByUserIdRequest Rate(
            this VerifyEventByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Schedule:VerifyEventByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
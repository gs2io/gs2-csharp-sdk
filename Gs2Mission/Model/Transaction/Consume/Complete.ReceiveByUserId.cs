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
    public static partial class CompleteExt
    {
        public static bool IsExecutable(
            this Complete self,
            ReceiveByUserIdRequest request
        ) {
            var changed = self.SpeculativeExecution(request);
            try {
                changed.Validate();
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

        public static Complete SpeculativeExecution(
            this Complete self,
            ReceiveByUserIdRequest request
        ) {
            var clone = self.Clone() as Complete;
            if (clone == null)
            {
                throw new NullReferenceException();
            }
            if (clone.ReceivedMissionTaskNames.Count(v => v == request.MissionTaskName) != 0) {
                return clone;
            }

            clone.ReceivedMissionTaskNames = clone.ReceivedMissionTaskNames.Concat(
                new[] {
                    request.MissionTaskName,
                }
            ).ToArray();
            
            return clone;
        }

        public static ReceiveByUserIdRequest Rate(
            this ReceiveByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Mission:ReceiveByUserId");
        }
    }

    public static partial class ReceiveByUserIdRequestExt
    {
        public static ReceiveByUserIdRequest Rate(
            this ReceiveByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Mission:ReceiveByUserId");
        }
    }
}
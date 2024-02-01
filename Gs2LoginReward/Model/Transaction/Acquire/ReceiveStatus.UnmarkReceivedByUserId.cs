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
using Gs2.Gs2LoginReward.Request;

namespace Gs2.Gs2LoginReward.Model.Transaction
{
    public static partial class ReceiveStatusExt
    {
        public static bool IsExecutable(
            this ReceiveStatus self,
            UnmarkReceivedByUserIdRequest request
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

        public static ReceiveStatus SpeculativeExecution(
            this ReceiveStatus self,
            UnmarkReceivedByUserIdRequest request
        ) {
            var clone = self.Clone() as ReceiveStatus;
            if (clone == null) {
                throw new NullReferenceException();
            }
            if ((request.StepNumber ?? 0) >= clone.ReceivedSteps.Length) {
                clone.ReceivedSteps = clone.ReceivedSteps.Concat(new bool[request.StepNumber ?? 0 - clone.ReceivedSteps.Length]).ToArray();
            }
            clone.ReceivedSteps[request.StepNumber ?? 0] = false;
            return clone;
        }

        public static UnmarkReceivedByUserIdRequest Rate(
            this UnmarkReceivedByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2LoginReward:UnmarkReceivedByUserId");
        }
    }

    public static partial class UnmarkReceivedByUserIdRequestExt
    {
        public static UnmarkReceivedByUserIdRequest Rate(
            this UnmarkReceivedByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2LoginReward:UnmarkReceivedByUserId");
        }
    }
}
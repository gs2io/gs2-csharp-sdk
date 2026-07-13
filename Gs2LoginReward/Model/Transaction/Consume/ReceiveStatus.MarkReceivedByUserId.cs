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
using Gs2.Core.Model; /* diff +++ */
using Gs2.Gs2LoginReward.Request;

namespace Gs2.Gs2LoginReward.Model.Transaction
{
    public static partial class ReceiveStatusExt
    {
        public static bool IsExecutable(
            this ReceiveStatus self,
            MarkReceivedByUserIdRequest request
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
            MarkReceivedByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2LoginReward:MarkReceivedByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2LoginReward:MarkReceivedByUserId");
//#endif
            return self.Clone() as ReceiveStatus;
 diff --- end */
/* diff +++ start */
            var clone = self.Clone() as ReceiveStatus;
            if (clone == null) {
                throw new NullReferenceException();
            }
            if (self.ReceivedSteps[request.StepNumber ?? 0]) {
                return clone;
            }
            if ((request.StepNumber ?? 0) >= self.ReceivedSteps.Length) {
                self.ReceivedSteps = self.ReceivedSteps.Concat(new bool[request.StepNumber ?? 0 - self.ReceivedSteps.Length]).ToArray();
            }
            self.ReceivedSteps[request.StepNumber ?? 0] = true;
            return clone;
/* diff +++ end */
        }

        public static MarkReceivedByUserIdRequest Rate(
            this MarkReceivedByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2LoginReward:MarkReceivedByUserId");
        }
    }

    public static partial class MarkReceivedByUserIdRequestExt
    {
        public static MarkReceivedByUserIdRequest Rate(
            this MarkReceivedByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2LoginReward:MarkReceivedByUserId");
        }
    }
}
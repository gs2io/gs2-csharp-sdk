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
using Gs2.Gs2Mission.Request;

namespace Gs2.Gs2Mission.Model.Transaction
{
    public static partial class CompleteExt
    {
        public static bool IsExecutable(
            this Complete self,
            RevertReceiveByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }

        public static Complete SpeculativeExecution(
            this Complete self,
            RevertReceiveByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Mission:RevertReceiveByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Mission:RevertReceiveByUserId");
//#endif
            return self.Clone() as Complete;
 diff --- end */
/* diff +++ start */
            if (self?.ReceivedMissionTaskNames == null || request == null) {
                throw new NullReferenceException();
            }
            if (!self.ReceivedMissionTaskNames.Contains(request.MissionTaskName)) {
                throw new InvalidOperationException("mission task is not received");
            }

            var clone = self.Clone() as Complete;
            clone.ReceivedMissionTaskNames = clone.ReceivedMissionTaskNames
                .Where(v => v != request.MissionTaskName)
                .ToArray();
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static RevertReceiveByUserIdRequest Rate(
            this RevertReceiveByUserIdRequest request,
            double rate
        ) {
            return request;
        }
    }

    public static partial class RevertReceiveByUserIdRequestExt
    {
        public static RevertReceiveByUserIdRequest Rate(
            this RevertReceiveByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}

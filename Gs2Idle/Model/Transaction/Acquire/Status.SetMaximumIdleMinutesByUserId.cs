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
using Gs2.Gs2Idle.Request;

namespace Gs2.Gs2Idle.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            SetMaximumIdleMinutesByUserIdRequest request
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

        public static Status SpeculativeExecution(
            this Status self,
            SetMaximumIdleMinutesByUserIdRequest request
        ) {
#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Idle:SetMaximumIdleMinutesByUserId");
#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Idle:SetMaximumIdleMinutesByUserId");
#endif
            return self.Clone() as Status;
        }

        public static SetMaximumIdleMinutesByUserIdRequest Rate(
            this SetMaximumIdleMinutesByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Idle:SetMaximumIdleMinutesByUserId");
        }
    }

    public static partial class SetMaximumIdleMinutesByUserIdRequestExt
    {
        public static SetMaximumIdleMinutesByUserIdRequest Rate(
            this SetMaximumIdleMinutesByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Idle:SetMaximumIdleMinutesByUserId");
        }
    }
}
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
    public static partial class CounterExt
    {
        public static bool IsExecutable(
            this Counter self,
            ResetCounterByUserIdRequest request
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

        public static Counter SpeculativeExecution(
            this Counter self,
            ResetCounterByUserIdRequest request
        ) {
#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Mission:ResetCounterByUserId");
#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Mission:ResetCounterByUserId");
#endif
            return self.Clone() as Counter;
        }

        public static ResetCounterByUserIdRequest Rate(
            this ResetCounterByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Mission:ResetCounterByUserId");
        }
    }

    public static partial class ResetCounterByUserIdRequestExt
    {
        public static ResetCounterByUserIdRequest Rate(
            this ResetCounterByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Mission:ResetCounterByUserId");
        }
    }
}
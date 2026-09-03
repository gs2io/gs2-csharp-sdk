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
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Enchant.Request;

namespace Gs2.Gs2Enchant.Model.Transaction
{
    public static partial class BalanceParameterStatusExt
    {
        public static bool IsExecutable(
            this BalanceParameterStatus self,
            SetBalanceParameterStatusByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
                self.SpeculativeExecution(request); /* diff +++ */
                return true;
            }
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (System.Exception) { /* diff +++ */
                return false;
            }
        }

        public static BalanceParameterStatus SpeculativeExecution(
            this BalanceParameterStatus self,
            SetBalanceParameterStatusByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Enchant:SetBalanceParameterStatusByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Enchant:SetBalanceParameterStatusByUserId");
//#endif
            return self.Clone() as BalanceParameterStatus;
 diff --- end */
/* diff +++ start */
            if (self?.Clone() is not BalanceParameterStatus clone ||
                request?.ParameterValues == null) {
                throw new NullReferenceException();
            }
            clone.ParameterValues = request.ParameterValues
                .Where(value => value != null)
                .Select(value => value.Clone() as BalanceParameterValue)
                .ToArray();
            if (clone.ParameterValues.Length == 0) {
                throw new NullReferenceException();
            }
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static SetBalanceParameterStatusByUserIdRequest Rate(
            this SetBalanceParameterStatusByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Enchant:SetBalanceParameterStatusByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class SetBalanceParameterStatusByUserIdRequestExt
    {
        public static SetBalanceParameterStatusByUserIdRequest Rate(
            this SetBalanceParameterStatusByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Enchant:SetBalanceParameterStatusByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
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
    public static partial class RarityParameterStatusExt
    {
        public static bool IsExecutable(
            this RarityParameterStatus self,
            SetRarityParameterStatusByUserIdRequest request
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

        public static RarityParameterStatus SpeculativeExecution(
            this RarityParameterStatus self,
            SetRarityParameterStatusByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Enchant:SetRarityParameterStatusByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Enchant:SetRarityParameterStatusByUserId");
//#endif
            return self.Clone() as RarityParameterStatus;
 diff --- end */
/* diff +++ start */
            if (self?.Clone() is not RarityParameterStatus clone ||
                request == null) {
                throw new NullReferenceException();
            }
            var parameterValues = request.ParameterValues ??
                                  Array.Empty<RarityParameterValue>();
            clone.ParameterValues = parameterValues
                .Where(value => value != null)
                .Select(value => value.Clone() as RarityParameterValue)
                .ToArray();
            if (parameterValues.Length > 0 &&
                clone.ParameterValues.Length == 0) {
                throw new NullReferenceException();
            }
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static SetRarityParameterStatusByUserIdRequest Rate(
            this SetRarityParameterStatusByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Enchant:SetRarityParameterStatusByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class SetRarityParameterStatusByUserIdRequestExt
    {
        public static SetRarityParameterStatusByUserIdRequest Rate(
            this SetRarityParameterStatusByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Enchant:SetRarityParameterStatusByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
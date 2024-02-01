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
using Gs2.Gs2Enchant.Request;

namespace Gs2.Gs2Enchant.Model.Transaction
{
    public static partial class RarityParameterStatusExt
    {
        public static bool IsExecutable(
            this RarityParameterStatus self,
            SetRarityParameterStatusByUserIdRequest request
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

        public static RarityParameterStatus SpeculativeExecution(
            this RarityParameterStatus self,
            SetRarityParameterStatusByUserIdRequest request
        ) {
            if (self.Clone() is not RarityParameterStatus clone)
            {
                throw new NullReferenceException();
            }
            clone.ParameterValues = request.ParameterValues;
            return clone;
        }

        public static SetRarityParameterStatusByUserIdRequest Rate(
            this SetRarityParameterStatusByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Enchant:SetRarityParameterStatusByUserId");
        }
    }

    public static partial class SetRarityParameterStatusByUserIdRequestExt
    {
        public static SetRarityParameterStatusByUserIdRequest Rate(
            this SetRarityParameterStatusByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Enchant:SetRarityParameterStatusByUserId");
        }
    }
}
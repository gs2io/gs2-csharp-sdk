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
            AddRarityParameterStatusByUserIdRequest request
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
            AddRarityParameterStatusByUserIdRequest request
        ) {
#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Enchant:AddRarityParameterStatusByUserId");
#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Enchant:AddRarityParameterStatusByUserId");
#endif
            return self.Clone() as RarityParameterStatus;
        }

        public static AddRarityParameterStatusByUserIdRequest Rate(
            this AddRarityParameterStatusByUserIdRequest request,
            double rate
        ) {
            request.Count = (int?) (request.Count * rate);
            return request;
        }
    }

    public static partial class AddRarityParameterStatusByUserIdRequestExt
    {
        public static AddRarityParameterStatusByUserIdRequest Rate(
            this AddRarityParameterStatusByUserIdRequest request,
            BigInteger rate
        ) {
            request.Count = (int?) ((request.Count ?? 0) * rate);
            return request;
        }
    }
}
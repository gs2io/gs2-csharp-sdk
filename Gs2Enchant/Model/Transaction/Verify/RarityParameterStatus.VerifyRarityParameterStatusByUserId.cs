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
using Gs2.Gs2Enchant.Request;

namespace Gs2.Gs2Enchant.Model.Transaction
{
    public static partial class RarityParameterStatusExt
    {
        public static bool IsExecutable(
            this RarityParameterStatus self,
            VerifyRarityParameterStatusByUserIdRequest request
        ) {
            if (self?.ParameterValues == null ||
                request == null) {
                return false;
            }

            switch (request.VerifyType) {
                case "havent":
                    return request.ParameterValueName != null &&
                           !self.ParameterValues.Any(v => v?.Name == request.ParameterValueName);
                case "have":
                    return request.ParameterValueName != null &&
                           self.ParameterValues.Any(v => v?.Name == request.ParameterValueName);
                case "count":
                    return request.ParameterCount.HasValue &&
                           self.ParameterValues.Length == request.ParameterCount.Value;
            }
            return false;
        }

        public static RarityParameterStatus SpeculativeExecution(
            this RarityParameterStatus self,
            VerifyRarityParameterStatusByUserIdRequest request
        ) {
            return self.Clone() as RarityParameterStatus;
        }

        public static VerifyRarityParameterStatusByUserIdRequest Rate(
            this VerifyRarityParameterStatusByUserIdRequest request,
            double rate
        ) {
            return request;
        }
    }

    public static partial class VerifyRarityParameterStatusByUserIdRequestExt
    {
        public static VerifyRarityParameterStatusByUserIdRequest Rate(
            this VerifyRarityParameterStatusByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}

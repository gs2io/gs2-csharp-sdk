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
using Gs2.Gs2Experience.Request;

namespace Gs2.Gs2Experience.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            VerifyRankByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "less":
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
                case "lessEqual":
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
                case "greater":
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
                case "greaterEqual":
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
                case "equal":
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
                case "notEqual":
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankByUserId");
            }
            return false;
        }

        public static Status SpeculativeExecution(
            this Status self,
            VerifyRankByUserIdRequest request
        ) {
            return self.Clone() as Status;
        }

        public static VerifyRankByUserIdRequest Rate(
            this VerifyRankByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Experience:VerifyRankByUserId");
        }
    }

    public static partial class VerifyRankByUserIdRequestExt
    {
        public static VerifyRankByUserIdRequest Rate(
            this VerifyRankByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Experience:VerifyRankByUserId");
        }
    }
}
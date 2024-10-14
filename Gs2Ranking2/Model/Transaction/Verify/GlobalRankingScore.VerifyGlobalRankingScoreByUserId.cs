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
using Gs2.Gs2Ranking2.Request;

namespace Gs2.Gs2Ranking2.Model.Transaction
{
    public static partial class GlobalRankingScoreExt
    {
        public static bool IsExecutable(
            this GlobalRankingScore self,
            VerifyGlobalRankingScoreByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "less":
                    return self.Score < request.Score;
                case "lessEqual":
                    return self.Score <= request.Score;
                case "greater":
                    return self.Score > request.Score;
                case "greaterEqual":
                    return self.Score >= request.Score;
                case "equal":
                    return self.Score == request.Score;
                case "notEqual":
                    return self.Score != request.Score;
            }
            return false;
        }

        public static GlobalRankingScore SpeculativeExecution(
            this GlobalRankingScore self,
            VerifyGlobalRankingScoreByUserIdRequest request
        ) {
            return self.Clone() as GlobalRankingScore;
        }

        public static VerifyGlobalRankingScoreByUserIdRequest Rate(
            this VerifyGlobalRankingScoreByUserIdRequest request,
            double rate
        ) {
            request.Score = (long?) (request.Score * rate);
            return request;
        }
    }

    public static partial class VerifyGlobalRankingScoreByUserIdRequestExt
    {
        public static VerifyGlobalRankingScoreByUserIdRequest Rate(
            this VerifyGlobalRankingScoreByUserIdRequest request,
            BigInteger rate
        ) {
            request.Score = (long?) ((request.Score ?? 0) * rate);
            return request;
        }
    }
}
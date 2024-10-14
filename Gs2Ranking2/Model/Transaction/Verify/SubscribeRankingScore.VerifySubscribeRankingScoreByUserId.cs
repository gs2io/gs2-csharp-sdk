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
    public static partial class SubscribeRankingScoreExt
    {
        public static bool IsExecutable(
            this SubscribeRankingScore self,
            VerifySubscribeRankingScoreByUserIdRequest request
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

        public static SubscribeRankingScore SpeculativeExecution(
            this SubscribeRankingScore self,
            VerifySubscribeRankingScoreByUserIdRequest request
        ) {
            return self.Clone() as SubscribeRankingScore;
        }

        public static VerifySubscribeRankingScoreByUserIdRequest Rate(
            this VerifySubscribeRankingScoreByUserIdRequest request,
            double rate
        ) {
            request.Score = (long?) (request.Score * rate);
            return request;
        }
    }

    public static partial class VerifySubscribeRankingScoreByUserIdRequestExt
    {
        public static VerifySubscribeRankingScoreByUserIdRequest Rate(
            this VerifySubscribeRankingScoreByUserIdRequest request,
            BigInteger rate
        ) {
            request.Score = (long?) ((request.Score ?? 0) * rate);
            return request;
        }
    }
}
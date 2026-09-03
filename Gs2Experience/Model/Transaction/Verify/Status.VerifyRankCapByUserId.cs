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
using Gs2.Gs2Experience.Request;

namespace Gs2.Gs2Experience.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            VerifyRankCapByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.RankCapValue == null || request?.RankCapValue == null) {
                return false;
            }

/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankCapByUserId");
 diff --- end */
                    return self.RankCapValue.Value < request.RankCapValue.Value; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankCapByUserId");
 diff --- end */
                    return self.RankCapValue.Value <= request.RankCapValue.Value; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankCapByUserId");
 diff --- end */
                    return self.RankCapValue.Value > request.RankCapValue.Value; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankCapByUserId");
 diff --- end */
                    return self.RankCapValue.Value >= request.RankCapValue.Value; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankCapByUserId");
 diff --- end */
                    return self.RankCapValue.Value == request.RankCapValue.Value; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Experience:VerifyRankCapByUserId");
 diff --- end */
                    return self.RankCapValue.Value != request.RankCapValue.Value; /* diff +++ */
            }
            return false;
        }

        public static Status SpeculativeExecution(
            this Status self,
            VerifyRankCapByUserIdRequest request
        ) {
            return self.Clone() as Status;
        }

        public static VerifyRankCapByUserIdRequest Rate(
            this VerifyRankCapByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Experience:VerifyRankCapByUserId");
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true) {
                return request;
            }
            if (!TryApplyServerRate(
                    request.RankCapValue ?? 0L,
                    rate,
                    out var value
                )) {
                return request;
            }
            request.RankCapValue = value;
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyRankCapByUserIdRequestExt
    {
        public static VerifyRankCapByUserIdRequest Rate(
            this VerifyRankCapByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Experience:VerifyRankCapByUserId");
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true) {
                return request;
            }
            var value = (request.RankCapValue ?? 0L) * rate;
            if (value <= long.MinValue || value >= long.MaxValue) {
                return request;
            }
            request.RankCapValue = (long)value;
            return request;
/* diff +++ end */
        }
    }
}
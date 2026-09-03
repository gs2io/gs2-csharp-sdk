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
using Gs2.Gs2Guild.Request;

namespace Gs2.Gs2Guild.Model.Transaction
{
    public static partial class GuildExt
    {
        public static bool IsExecutable(
            this Guild self,
            VerifyCurrentMaximumMemberCountByGuildNameRequest request
        ) {
/* diff +++ start */
            if (self?.CurrentMaximumMemberCount == null || request?.Value == null) {
                return false;
            }

/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Guild:VerifyCurrentMaximumMemberCountByGuildName");
 diff --- end */
                    return self.CurrentMaximumMemberCount.Value < request.Value.Value; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Guild:VerifyCurrentMaximumMemberCountByGuildName");
 diff --- end */
                    return self.CurrentMaximumMemberCount.Value <= request.Value.Value; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Guild:VerifyCurrentMaximumMemberCountByGuildName");
 diff --- end */
                    return self.CurrentMaximumMemberCount.Value > request.Value.Value; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Guild:VerifyCurrentMaximumMemberCountByGuildName");
 diff --- end */
                    return self.CurrentMaximumMemberCount.Value >= request.Value.Value; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Guild:VerifyCurrentMaximumMemberCountByGuildName");
 diff --- end */
                    return self.CurrentMaximumMemberCount.Value == request.Value.Value; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Guild:VerifyCurrentMaximumMemberCountByGuildName");
 diff --- end */
                    return self.CurrentMaximumMemberCount.Value != request.Value.Value; /* diff +++ */
            }
            return false;
        }

        public static Guild SpeculativeExecution(
            this Guild self,
            VerifyCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            return self.Clone() as Guild;
        }

        public static VerifyCurrentMaximumMemberCountByGuildNameRequest Rate(
            this VerifyCurrentMaximumMemberCountByGuildNameRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Guild:VerifyCurrentMaximumMemberCountByGuildName");
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true ||
                !Gs2.Gs2Inventory.Model.Transaction
                    .SetCapacityByUserIdRequestExt.TryApplyRate(
                        request.Value ?? 0, rate, out var value
                    )) return request;
            request.Value = value;
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyCurrentMaximumMemberCountByGuildNameRequestExt
    {
        public static VerifyCurrentMaximumMemberCountByGuildNameRequest Rate(
            this VerifyCurrentMaximumMemberCountByGuildNameRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Guild:VerifyCurrentMaximumMemberCountByGuildName");
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true) return request;
            var value = new BigInteger(request.Value ?? 0) * rate;
            if (value <= int.MinValue || value > int.MaxValue) return request;
            request.Value = (int)value;
            return request;
/* diff +++ end */
        }
    }
}
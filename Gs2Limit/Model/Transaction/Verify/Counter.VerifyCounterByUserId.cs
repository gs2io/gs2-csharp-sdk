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
using Gs2.Gs2Limit.Request;

namespace Gs2.Gs2Limit.Model.Transaction
{
    public static partial class CounterExt
    {
        public static bool IsExecutable(
            this Counter self,
            VerifyCounterByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.Count == null || request == null) {
                return false;
            }
            var count = request.Count ?? 0;

/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Limit:VerifyCounterByUserId");
 diff --- end */
                    return self.Count.Value < count; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Limit:VerifyCounterByUserId");
 diff --- end */
                    return self.Count.Value <= count; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Limit:VerifyCounterByUserId");
 diff --- end */
                    return self.Count.Value > count; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Limit:VerifyCounterByUserId");
 diff --- end */
                    return self.Count.Value >= count; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Limit:VerifyCounterByUserId");
 diff --- end */
                    return self.Count.Value == count; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Limit:VerifyCounterByUserId");
 diff --- end */
                    return self.Count.Value != count; /* diff +++ */
            }
            return false;
        }

        public static Counter SpeculativeExecution(
            this Counter self,
            VerifyCounterByUserIdRequest request
        ) {
            return self.Clone() as Counter;
        }

        public static VerifyCounterByUserIdRequest Rate(
            this VerifyCounterByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Limit:VerifyCounterByUserId");
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true ||
                !CounterRate.TryApply(request.Count ?? 0, rate, out var value)) {
                return request;
            }
            request.Count = value;
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyCounterByUserIdRequestExt
    {
        public static VerifyCounterByUserIdRequest Rate(
            this VerifyCounterByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Limit:VerifyCounterByUserId");
 diff --- end */
/* diff +++ start */
            if (request?.MultiplyValueSpecifyingQuantity != true ||
                !CounterRate.TryApply(request.Count ?? 0, rate, out var value)) {
                return request;
            }
            request.Count = value;
            return request;
/* diff +++ end */
        }
    }
}
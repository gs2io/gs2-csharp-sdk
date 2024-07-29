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
 *
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class BigItemExt
    {
        public static bool IsExecutable(
            this BigItem self,
            VerifyBigItemByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "less":
                    return BigInteger.Parse(self.Count) < BigInteger.Parse(request.Count);
                case "lessEqual":
                    return BigInteger.Parse(self.Count) <= BigInteger.Parse(request.Count);
                case "greater":
                    return BigInteger.Parse(self.Count) > BigInteger.Parse(request.Count);
                case "greaterEqual":
                    return BigInteger.Parse(self.Count) >= BigInteger.Parse(request.Count);
                case "equal":
                    return self.Count == request.Count;
                case "notEqual":
                    return self.Count != request.Count;
            }
            return false;
        }

        public static BigItem SpeculativeExecution(
            this BigItem self,
            VerifyBigItemByUserIdRequest request
        ) {
            return self.Clone() as BigItem;
        }

        public static VerifyBigItemByUserIdRequest Rate(
            this VerifyBigItemByUserIdRequest request,
            double rate
        ) {
            request.Count = BigInteger.Multiply(BigInteger.Parse(request.Count), new BigInteger(rate)).ToString("D");
            return request;
        }
    }

    public static partial class VerifyBigItemByUserIdRequestExt
    {
        public static VerifyBigItemByUserIdRequest Rate(
            this VerifyBigItemByUserIdRequest request,
            BigInteger rate
        ) {
            request.Count = BigInteger.Multiply(BigInteger.Parse(request.Count), rate).ToString("D");
            return request;
        }
    }
}
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
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class ItemSetExt
    {
        public static bool IsExecutable(
            this ItemSet self,
            VerifyItemSetByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "less":
                    return self.Count < request.Count;
                case "lessEqual":
                    return self.Count <= request.Count;
                case "greater":
                    return self.Count > request.Count;
                case "greaterEqual":
                    return self.Count >= request.Count;
                case "equal":
                    return self.Count == request.Count;
                case "notEqual":
                    return self.Count != request.Count;
            }
            return false;
        }

        public static ItemSet SpeculativeExecution(
            this ItemSet self,
            VerifyItemSetByUserIdRequest request
        ) {
            return self.Clone() as ItemSet;
        }

        public static VerifyItemSetByUserIdRequest Rate(
            this VerifyItemSetByUserIdRequest request,
            double rate
        ) {
            request.Count = (long?) (request.Count * rate);
            return request;
        }
    }

    public static partial class VerifyItemSetByUserIdRequestExt
    {
        public static VerifyItemSetByUserIdRequest Rate(
            this VerifyItemSetByUserIdRequest request,
            BigInteger rate
        ) {
            request.Count = (long?) ((request.Count ?? 0) * rate);
            return request;
        }
    }
}
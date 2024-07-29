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
    public static partial class SimpleItemExt
    {
        public static bool IsExecutable(
            this SimpleItem self,
            VerifySimpleItemByUserIdRequest request
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

        public static SimpleItem SpeculativeExecution(
            this SimpleItem self,
            VerifySimpleItemByUserIdRequest request
        ) {
            return self.Clone() as SimpleItem;
        }

        public static VerifySimpleItemByUserIdRequest Rate(
            this VerifySimpleItemByUserIdRequest request,
            double rate
        ) {
            request.Count = (long?) (request.Count * rate);
            return request;
        }
    }

    public static partial class VerifySimpleItemByUserIdRequestExt
    {
        public static VerifySimpleItemByUserIdRequest Rate(
            this VerifySimpleItemByUserIdRequest request,
            BigInteger rate
        ) {
            request.Count = (long?) ((request.Count ?? 0) * rate);
            return request;
        }
    }
}
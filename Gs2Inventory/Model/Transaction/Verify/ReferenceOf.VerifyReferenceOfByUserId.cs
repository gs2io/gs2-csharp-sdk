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
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class ReferenceOfExt
    {
        public static bool IsExecutable(
/* diff --- start
            this ReferenceOf self,
 diff --- end */
            this ItemSet self, /* diff +++ */
            VerifyReferenceOfByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "not_entry":
/* diff --- start
                    return self.Name.Contains(request.ReferenceOf);
 diff --- end */
                    return self.ReferenceOf.Contains(request.ReferenceOf); /* diff +++ */
                case "already_entry":
/* diff --- start
                    return !self.Name.Contains(request.ReferenceOf);
 diff --- end */
                    return !self.ReferenceOf.Contains(request.ReferenceOf); /* diff +++ */
                case "empty":
/* diff --- start
                    return self.Name.Length != 0;
 diff --- end */
                    return self.ReferenceOf.Length != 0; /* diff +++ */
                case "not_empty":
/* diff --- start
                    return self.Name.Length != 0;
 diff --- end */
                    return self.ReferenceOf.Length != 0; /* diff +++ */
            }
            return false;
        }

        public static ReferenceOf SpeculativeExecution(
            this ReferenceOf self,
            VerifyReferenceOfByUserIdRequest request
        ) {
            return self.Clone() as ReferenceOf;
        }

        public static VerifyReferenceOfByUserIdRequest Rate(
            this VerifyReferenceOfByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Inventory:VerifyReferenceOfByUserId");
        }
    }

    public static partial class VerifyReferenceOfByUserIdRequestExt
    {
        public static VerifyReferenceOfByUserIdRequest Rate(
            this VerifyReferenceOfByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Inventory:VerifyReferenceOfByUserId");
        }
    }
}
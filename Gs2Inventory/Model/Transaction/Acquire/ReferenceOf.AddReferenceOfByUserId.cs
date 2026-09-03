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
            AddReferenceOfByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }

/* diff --- start
        public static ReferenceOf SpeculativeExecution(
            this ReferenceOf self,
 diff --- end */
/* diff +++ start */
        public static ItemSet SpeculativeExecution(
            this ItemSet self,
/* diff +++ end */
            AddReferenceOfByUserIdRequest request
        ) {
            if (self?.ReferenceOf == null || request == null ||
                request.ReferenceOf == null) {
                throw new InvalidOperationException();
            }
            if (self.ReferenceOf.Contains(request.ReferenceOf)) {
                throw new BadRequestException(new [] {
                    new Gs2.Core.Model.RequestError("referenceOf", "duplicate"),
                });
            }
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Inventory:AddReferenceOfByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Inventory:AddReferenceOfByUserId");
//#endif
            return self.Clone() as ReferenceOf;
 diff --- end */
/* diff +++ start */
            var clone = self.Clone() as ItemSet;
            if (clone == null) {
                throw new NullReferenceException();
            }
            clone.ReferenceOf = clone.ReferenceOf
                .Concat(new []{ request.ReferenceOf })
                .ToArray();
            return clone;
/* diff +++ end */
        }

        public static AddReferenceOfByUserIdRequest Rate(
            this AddReferenceOfByUserIdRequest request,
            double rate
        ) {
            return request;
        }
    }

    public static partial class AddReferenceOfByUserIdRequestExt
    {
        public static AddReferenceOfByUserIdRequest Rate(
            this AddReferenceOfByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}

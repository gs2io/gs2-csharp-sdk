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
    public static partial class ItemSetExt
    {
        public static bool IsExecutable(
            this ItemSet self,
            AcquireItemSetByUserIdRequest request
        ) {
            var changed = self.SpeculativeExecution(request);
            try {
                changed.Validate();
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

        public static ItemSet SpeculativeExecution(
            this ItemSet self,
            AcquireItemSetByUserIdRequest request
        ) {
            var clone = self.Clone() as ItemSet;
            if (clone == null)
            {
                throw new NullReferenceException();
            }
            clone.Count += request.AcquireCount;
            return clone;
        }

        public static ItemSet[] SpeculativeExecution(
            this ItemSet[] self,
            AcquireItemSetByUserIdRequest request
        ) {
            var clone = self.Clone() as ItemSet[];
            if (clone == null)
            {
                throw new NullReferenceException();
            }
            if (clone.Length > 0) {
                clone[clone.Length - 1].Count += request.AcquireCount;
            }
            return clone;
        }

        public static AcquireItemSetByUserIdRequest Rate(
            this AcquireItemSetByUserIdRequest request,
            double rate
        ) {
            request.AcquireCount = (long?) (request.AcquireCount * rate);
            return request;
        }
    }

    public static partial class AcquireItemSetByUserIdRequestExt
    {
        public static AcquireItemSetByUserIdRequest Rate(
            this AcquireItemSetByUserIdRequest request,
            BigInteger rate
        ) {
            request.AcquireCount = (long?) ((request.AcquireCount ?? 0) * rate);
            return request;
        }
    }
}
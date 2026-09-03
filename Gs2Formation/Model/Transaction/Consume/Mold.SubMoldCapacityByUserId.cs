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
using Gs2.Gs2Formation.Request;

namespace Gs2.Gs2Formation.Model.Transaction
{
    public static partial class MoldExt
    {
        public static bool IsExecutable(
            this Mold self,
            SubMoldCapacityByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
                var changed = self.SpeculativeExecution(request); /* diff +++ */
                changed.Validate();
                return true;
            }
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (Exception) { /* diff +++ */
                return false;
            }
        }

        public static Mold SpeculativeExecution(
            this Mold self,
            SubMoldCapacityByUserIdRequest request
        ) {
            if (self.Clone() is not Mold clone)
            {
                throw new NullReferenceException();
            }
/* diff --- start
            clone.Capacity -= request.Capacity;
 diff --- end */
/* diff +++ start */
            if (!clone.Capacity.HasValue || request?.Capacity == null) {
                throw new InvalidOperationException();
            }
            var changed = (long)clone.Capacity.Value - request.Capacity.Value;
            if (changed <= 0) {
                throw new InvalidOperationException();
            }
            clone.Capacity = (int)changed;
            clone.Revision = 0;
/* diff +++ end */
            return clone;
        }

        public static SubMoldCapacityByUserIdRequest Rate(
            this SubMoldCapacityByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.Capacity = (int?) (request.Capacity * rate);
 diff --- end */
/* diff +++ start */
            if (request == null) return request;
            if (!Gs2.Gs2Inventory.Model.Transaction
                    .SetCapacityByUserIdRequestExt.TryApplyRate(
                        request.Capacity ?? 1, rate, out var value
                    )) {
                request.Capacity = null;
                return request;
            }
            request.Capacity = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class SubMoldCapacityByUserIdRequestExt
    {
        public static SubMoldCapacityByUserIdRequest Rate(
            this SubMoldCapacityByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.Capacity = (int?) ((request.Capacity ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request == null) return request;
            var value = new BigInteger(request.Capacity ?? 1) * rate;
            request.Capacity = value <= int.MinValue || value > int.MaxValue
                ? (int?)null
                : (int)value;
/* diff +++ end */
            return request;
        }
    }
}
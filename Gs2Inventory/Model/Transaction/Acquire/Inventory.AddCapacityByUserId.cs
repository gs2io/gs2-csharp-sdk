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
/* diff --- start
using System.Linq;
 diff --- end */
using System.Numerics;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class InventoryExt
    {
        public static bool IsExecutable(
            this Inventory self,
            AddCapacityByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
                self.SpeculativeExecution(request); /* diff +++ */
                return true;
            }
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (System.Exception) { /* diff +++ */
                return false;
            }
        }

        public static Inventory SpeculativeExecution(
            this Inventory self,
            AddCapacityByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not Inventory clone)
 diff --- end */
/* diff +++ start */
            if (self.Clone() is not Inventory clone ||
                clone.CurrentInventoryMaxCapacity == null ||
                request?.AddCapacityValue == null)
/* diff +++ end */
            {
                throw new NullReferenceException();
            }
/* diff --- start
            clone.CurrentInventoryMaxCapacity += request.AddCapacityValue;
 diff --- end */
/* diff +++ start */
            clone.CurrentInventoryMaxCapacity = checked(
                clone.CurrentInventoryMaxCapacity.Value +
                request.AddCapacityValue.Value
            );
            clone.Revision = 0;
/* diff +++ end */
            return clone;
        }

        public static AddCapacityByUserIdRequest Rate(
            this AddCapacityByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.AddCapacityValue = (int?) (request.AddCapacityValue * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !request.AddCapacityValue.HasValue ||
                !SetCapacityByUserIdRequestExt.TryApplyRate(
                    request.AddCapacityValue.Value, rate, out var value
                ) || !value.HasValue) {
                return null;
            }
            request.AddCapacityValue = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class AddCapacityByUserIdRequestExt
    {
        public static AddCapacityByUserIdRequest Rate(
            this AddCapacityByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.AddCapacityValue = (int?) ((request.AddCapacityValue ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !request.AddCapacityValue.HasValue) return null;
            var value = new BigInteger(request.AddCapacityValue.Value) * rate;
            if (value < int.MinValue || value > int.MaxValue) return null;
            request.AddCapacityValue = (int)value;
/* diff +++ end */
            return request;
        }
    }
}
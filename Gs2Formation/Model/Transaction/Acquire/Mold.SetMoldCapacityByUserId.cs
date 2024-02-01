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
using Gs2.Gs2Formation.Request;

namespace Gs2.Gs2Formation.Model.Transaction
{
    public static partial class MoldExt
    {
        public static bool IsExecutable(
            this Mold self,
            SetMoldCapacityByUserIdRequest request
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

        public static Mold SpeculativeExecution(
            this Mold self,
            SetMoldCapacityByUserIdRequest request
        ) {
            if (self.Clone() is not Mold clone)
            {
                throw new NullReferenceException();
            }
            clone.Capacity = request.Capacity;
            return clone;
        }

        public static SetMoldCapacityByUserIdRequest Rate(
            this SetMoldCapacityByUserIdRequest request,
            double rate
        ) {
            request.Capacity = (int?) (request.Capacity * rate);
            return request;
        }
    }

    public static partial class SetMoldCapacityByUserIdRequestExt
    {
        public static SetMoldCapacityByUserIdRequest Rate(
            this SetMoldCapacityByUserIdRequest request,
            BigInteger rate
        ) {
            request.Capacity = (int?) ((request.Capacity ?? 0) * rate);
            return request;
        }
    }
}
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
using Gs2.Gs2Stamina.Request;

namespace Gs2.Gs2Stamina.Model.Transaction
{
    public static partial class StaminaExt
    {
        public static bool IsExecutable(
            this Stamina self,
            RaiseMaxValueByUserIdRequest request
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

        public static Stamina SpeculativeExecution(
            this Stamina self,
            RaiseMaxValueByUserIdRequest request
        ) {
            var clone = self.Clone() as Stamina;
            if (clone == null) {
                throw new NullReferenceException();
            }
            clone.MaxValue += request.RaiseValue;
            return clone;
        }

        public static RaiseMaxValueByUserIdRequest Rate(
            this RaiseMaxValueByUserIdRequest request,
            double rate
        ) {
            request.RaiseValue = (int?) (request.RaiseValue * rate);
            return request;
        }
    }

    public static partial class RaiseMaxValueByUserIdRequestExt
    {
        public static RaiseMaxValueByUserIdRequest Rate(
            this RaiseMaxValueByUserIdRequest request,
            BigInteger rate
        ) {
            request.RaiseValue = (int?) ((request.RaiseValue ?? 0) * rate);
            return request;
        }
    }
}
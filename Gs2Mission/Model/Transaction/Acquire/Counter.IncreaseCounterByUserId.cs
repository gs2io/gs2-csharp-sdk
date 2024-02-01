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
using Gs2.Gs2Mission.Request;

namespace Gs2.Gs2Mission.Model.Transaction
{
    public static partial class CounterExt
    {
        public static bool IsExecutable(
            this Counter self,
            IncreaseCounterByUserIdRequest request
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

        public static Counter SpeculativeExecution(
            this Counter self,
            IncreaseCounterByUserIdRequest request
        ) {
            var clone = self.Clone() as Counter;
            if (clone == null)
            {
                throw new NullReferenceException();
            }
            clone.Values = clone.Values?.Select(v =>
            {
                if (v.Value != null) {
                    v.Value += request.Value;
                }
                return v;
            }).ToArray() ?? Array.Empty<ScopedValue>();
            return clone;
        }

        public static IncreaseCounterByUserIdRequest Rate(
            this IncreaseCounterByUserIdRequest request,
            double rate
        ) {
            request.Value = (long?) (request.Value * rate);
            return request;
        }
    }

    public static partial class IncreaseCounterByUserIdRequestExt
    {
        public static IncreaseCounterByUserIdRequest Rate(
            this IncreaseCounterByUserIdRequest request,
            BigInteger rate
        ) {
            request.Value = (long?) ((request.Value ?? 0) * rate);
            return request;
        }
    }
}
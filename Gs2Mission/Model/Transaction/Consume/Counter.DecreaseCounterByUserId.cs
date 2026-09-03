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
using Gs2.Gs2Mission.Request;

namespace Gs2.Gs2Mission.Model.Transaction
{
    public static partial class CounterExt
    {
        public static bool IsExecutable(
            this Counter self,
            DecreaseCounterByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }

        public static Counter SpeculativeExecution(
            this Counter self,
            DecreaseCounterByUserIdRequest request
        ) {
            if (self?.Values == null || request?.Value == null) {
                throw new NullReferenceException();
            }
            var clone = self.Clone() as Counter;
            clone.Values = self.Values
                .Where(value => value != null)
                .Select(value => value.Clone() as ScopedValue)
                .Where(value => value != null)
                .Select(value => {
                    if (!value.Value.HasValue) return value;
                    var decreased = new BigInteger(value.Value.Value) -
                                    request.Value.Value;
                    value.Value = decreased < 0 ? 0 : (long)decreased;
                    return value;
                })
                .ToArray();
            clone.Revision = 0;
            return clone;
        }

        public static DecreaseCounterByUserIdRequest Rate(
            this DecreaseCounterByUserIdRequest request,
            double rate
        ) {
            request.Value = (long?) (request.Value * rate);
            return request;
        }
    }

    public static partial class DecreaseCounterByUserIdRequestExt
    {
        public static DecreaseCounterByUserIdRequest Rate(
            this DecreaseCounterByUserIdRequest request,
            BigInteger rate
        ) {
            request.Value = (long?) ((request.Value ?? 0) * rate);
            return request;
        }
    }
}

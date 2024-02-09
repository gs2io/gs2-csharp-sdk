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
using Gs2.Gs2Limit.Request;

namespace Gs2.Gs2Limit.Model.Transaction
{
    public static partial class CounterExt
    {
        public static bool IsExecutable(
            this Counter self,
            CountDownByUserIdRequest request
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
            CountDownByUserIdRequest request
        ) {
            if (self.Clone() is not Counter clone)
            {
                throw new NullReferenceException();
            }
            clone.Count -= request.CountDownValue;
            return clone;
        }

        public static CountDownByUserIdRequest Rate(
            this CountDownByUserIdRequest request,
            double rate
        ) {
            request.CountDownValue = (int?) (request.CountDownValue * rate);
            return request;
        }
    }

    public static partial class CountDownByUserIdRequestExt
    {
        public static CountDownByUserIdRequest Rate(
            this CountDownByUserIdRequest request,
            BigInteger rate
        ) {
            request.CountDownValue = (int?) ((request.CountDownValue ?? 0) * rate);
            return request;
        }
    }
}
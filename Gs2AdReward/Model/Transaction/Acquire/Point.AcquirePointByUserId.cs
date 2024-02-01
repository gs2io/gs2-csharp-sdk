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
using Gs2.Gs2AdReward.Request;

namespace Gs2.Gs2AdReward.Model.Transaction
{
    public static partial class PointExt
    {
        public static bool IsExecutable(
            this Point self,
            AcquirePointByUserIdRequest request
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

        public static Point SpeculativeExecution(
            this Point self,
            AcquirePointByUserIdRequest request
        ) {
            var clone = self.Clone() as Point;
            if (clone == null) {
                throw new NullReferenceException();
            }
            clone.Value += request.Point;
            return clone;
        }

        public static AcquirePointByUserIdRequest Rate(
            this AcquirePointByUserIdRequest request,
            double rate
        ) {
            request.Point = (long?) (request.Point * rate);
            return request;
        }
    }

    public static partial class AcquirePointByUserIdRequestExt
    {
        public static AcquirePointByUserIdRequest Rate(
            this AcquirePointByUserIdRequest request,
            BigInteger rate
        ) {
            request.Point = (long?) ((request.Point ?? 0) * rate);
            return request;
        }
    }
}
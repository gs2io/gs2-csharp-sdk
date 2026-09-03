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
using Gs2.Core.Exception;
/* diff +++ start */
using Gs2.Core.Model;
using Gs2.Core.Util;
/* diff +++ end */
using Gs2.Gs2AdReward.Request;

namespace Gs2.Gs2AdReward.Model.Transaction
{
    public static partial class PointExt
    {
        public static bool IsExecutable(
            this Point self,
            ConsumePointByUserIdRequest request
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

        public static Point SpeculativeExecution(
            this Point self,
            ConsumePointByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not Point clone)
            {
                throw new NullReferenceException();
 diff --- end */
/* diff +++ start */
            return self.SpeculativeExecutionAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static Point SpeculativeExecutionAt(
            this Point self,
            ConsumePointByUserIdRequest request,
            long currentTimeMillis
        ) {
            if (self?.Value == null || request?.Point == null) {
                throw new InvalidOperationException("point is unavailable");
/* diff +++ end */
            }
/* diff --- start
            clone.Value -= request.Point;
 diff --- end */
/* diff +++ start */
            if (request.Point.Value > 0 &&
                self.Value.Value < request.Point.Value) {
                throw new BadRequestException(new[] {
                    new RequestError("point", "insufficient"),
                });
            }
            var value = new BigInteger(self.Value.Value) - request.Point.Value;
            if (value < long.MinValue || value > long.MaxValue) {
                throw new OverflowException();
            }
            var clone = self.Clone() as Point;
            if (clone == null) {
                throw new InvalidOperationException("point is unavailable");
            }
            clone.Value = (long)value;
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
/* diff +++ end */
            return clone;
        }

        public static ConsumePointByUserIdRequest Rate(
            this ConsumePointByUserIdRequest request,
            double rate
        ) {
            request.Point = (long?) (request.Point * rate);
            return request;
        }
    }

    public static partial class ConsumePointByUserIdRequestExt
    {
        public static ConsumePointByUserIdRequest Rate(
            this ConsumePointByUserIdRequest request,
            BigInteger rate
        ) {
            request.Point = (long?) ((request.Point ?? 0) * rate);
            return request;
        }
    }
}
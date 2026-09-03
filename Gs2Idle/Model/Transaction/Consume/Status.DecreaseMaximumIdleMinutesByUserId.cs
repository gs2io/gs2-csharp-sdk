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
using Gs2.Core.Util; /* diff +++ */
using Gs2.Gs2Idle.Request;

namespace Gs2.Gs2Idle.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            DecreaseMaximumIdleMinutesByUserIdRequest request
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
            catch (Exception) { /* diff +++ */
                return false;
            }
        }

        public static Status SpeculativeExecution(
            this Status self,
            DecreaseMaximumIdleMinutesByUserIdRequest request
        ) {
/* diff +++ start */
            return self.SpeculativeExecutionAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        internal static Status SpeculativeExecutionAt(
            this Status self,
            DecreaseMaximumIdleMinutesByUserIdRequest request,
            long currentTimeMillis
        ) {
/* diff +++ end */
            if (self.Clone() is not Status clone)
            {
                throw new NullReferenceException();
            }
/* diff --- start
            clone.MaximumIdleMinutes -= request.DecreaseMinutes;
 diff --- end */
/* diff +++ start */
            var value = request?.DecreaseMinutes ?? 1;
            if (clone.MaximumIdleMinutes == null) {
                throw new InvalidOperationException();
            }
            var changed = checked(clone.MaximumIdleMinutes.Value - value);
            if (changed <= 0) {
                throw new InvalidOperationException();
            }
            clone.MaximumIdleMinutes = changed;
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
/* diff +++ end */
            return clone;
        }

        public static DecreaseMaximumIdleMinutesByUserIdRequest Rate(
            this DecreaseMaximumIdleMinutesByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.DecreaseMinutes = (int?) (request.DecreaseMinutes * rate);
 diff --- end */
/* diff +++ start */
            if (request != null) {
                request.DecreaseMinutes = MaximumIdleMinutesRate.TryApply(
                    request.DecreaseMinutes ?? 1,
                    rate,
                    out var value
                ) ? value : 0;
            }
/* diff +++ end */
            return request;
        }
    }

    public static partial class DecreaseMaximumIdleMinutesByUserIdRequestExt
    {
        public static DecreaseMaximumIdleMinutesByUserIdRequest Rate(
            this DecreaseMaximumIdleMinutesByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.DecreaseMinutes = (int?) ((request.DecreaseMinutes ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request != null) {
                request.DecreaseMinutes = MaximumIdleMinutesRate.TryApply(
                    request.DecreaseMinutes ?? 1,
                    rate,
                    out var value
                ) ? value : 0;
            }
/* diff +++ end */
            return request;
        }
    }
}
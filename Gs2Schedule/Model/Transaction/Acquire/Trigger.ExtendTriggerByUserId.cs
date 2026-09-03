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
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Core.Util; /* diff +++ */
using Gs2.Gs2Schedule.Request;

namespace Gs2.Gs2Schedule.Model.Transaction
{
    public static partial class TriggerExt
    {
        public static bool IsExecutable(
            this Trigger self,
            ExtendTriggerByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            return self.IsExtendExecutableAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static bool IsExtendExecutableAt(
            this Trigger self,
            ExtendTriggerByUserIdRequest request,
            long currentTimeMillis
        ) {
/* diff +++ end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
                self.SpeculativeExtendAt(request, currentTimeMillis); /* diff +++ */
                return true;
            }
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (Exception) { /* diff +++ */
                return false;
            }
        }

        public static Trigger SpeculativeExecution(
            this Trigger self,
            ExtendTriggerByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not Trigger clone)
            {
                throw new NullReferenceException();
 diff --- end */
/* diff +++ start */
            return self.SpeculativeExtendAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static Trigger SpeculativeExtendAt(
            this Trigger self,
            ExtendTriggerByUserIdRequest request,
            long currentTimeMillis
        ) {
            if (request?.ExtendSeconds == null) {
                throw new InvalidOperationException();
/* diff +++ end */
            }
/* diff --- start
            throw new NotImplementedException($"not implemented action Gs2Schedule:ExtendTriggerByUserId");
 diff --- end */
/* diff +++ start */

            var expired = self == null;
            if (self != null) {
                if (self.ExpiresAt == null) {
                    throw new InvalidOperationException();
                }
                expired = self.ExpiresAt.Value <= currentTimeMillis;
            }

            var baseTimeMillis = expired
                ? currentTimeMillis
                : self.ExpiresAt.Value;
            var expiresAt = checked(
                baseTimeMillis + request.ExtendSeconds.Value * 1000L
            );

            var clone = self?.Clone() as Trigger ?? new Trigger()
                .WithName(request.TriggerName)
                .WithUserId(request.UserId);
            if (expired) {
                clone.TriggeredAt = currentTimeMillis;
            }
            clone.ExpiresAt = expiresAt;
            clone.CreatedAt = currentTimeMillis;
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static ExtendTriggerByUserIdRequest Rate(
            this ExtendTriggerByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.ExtendSeconds = (int?) (request.ExtendSeconds * rate);
 diff --- end */
/* diff +++ start */
            if (request?.ExtendSeconds == null) {
                return request;
            }
            var scaled = request.ExtendSeconds.Value * rate;
            if (double.IsNaN(scaled) ||
                double.IsInfinity(scaled) ||
                scaled < int.MinValue || scaled > int.MaxValue) {
                return null;
            }
            request.ExtendSeconds = (int) scaled;
/* diff +++ end */
            return request;
        }
    }

    public static partial class ExtendTriggerByUserIdRequestExt
    {
        public static ExtendTriggerByUserIdRequest Rate(
            this ExtendTriggerByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.ExtendSeconds = (int?) ((request.ExtendSeconds ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request?.ExtendSeconds == null) {
                return request;
            }
            var scaled = new BigInteger(request.ExtendSeconds.Value) * rate;
            if (scaled < int.MinValue || scaled > int.MaxValue) {
                return null;
            }
            request.ExtendSeconds = (int) scaled;
/* diff +++ end */
            return request;
        }
    }
}
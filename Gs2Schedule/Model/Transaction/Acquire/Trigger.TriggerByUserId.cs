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
            TriggerByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            return self.IsTriggerExecutableAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static bool IsTriggerExecutableAt(
            this Trigger self,
            TriggerByUserIdRequest request,
            long currentTimeMillis,
            long? eventExpirationMillis = null
        ) {
/* diff +++ end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
/* diff +++ start */
                self.SpeculativeTriggerAt(
                    request,
                    currentTimeMillis,
                    eventExpirationMillis
                );
/* diff +++ end */
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
            TriggerByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Schedule:TriggerByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Schedule:TriggerByUserId");
//#endif
            return self.Clone() as Trigger;
 diff --- end */
/* diff +++ start */
            return self.SpeculativeTriggerAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static Trigger SpeculativeTriggerAt(
            this Trigger self,
            TriggerByUserIdRequest request,
            long currentTimeMillis,
            long? eventExpirationMillis = null
        ) {
            if (request == null) {
                throw new InvalidOperationException();
            }

            var isTtlStrategy = request.TriggerStrategy == "renew" ||
                                request.TriggerStrategy == "extend" ||
                                request.TriggerStrategy == "drop";
            var isEventStrategy = request.TriggerStrategy == "repeatCycleEnd" ||
                                  request.TriggerStrategy == "repeatCycleNextStart" ||
                                  request.TriggerStrategy == "absoluteEnd";
            if (!isTtlStrategy && !isEventStrategy) {
                throw new InvalidOperationException();
            }

            if (isTtlStrategy && request.Ttl == null) {
                throw new InvalidOperationException();
            }
            if (isEventStrategy && eventExpirationMillis == null) {
                throw new InvalidOperationException();
            }

            var active = self?.ExpiresAt != null &&
                         self.ExpiresAt.Value > currentTimeMillis;
            if (self != null && self.ExpiresAt == null) {
                throw new InvalidOperationException();
            }

            if (request.TriggerStrategy == "drop" && active) {
                return self.Clone() as Trigger;
            }

            var clone = self?.Clone() as Trigger ?? new Trigger()
                .WithName(request.TriggerName)
                .WithUserId(request.UserId);
            long expiresAt;
            if (isEventStrategy) {
                clone.TriggeredAt = currentTimeMillis;
                expiresAt = eventExpirationMillis.Value;
            }
            else if (request.TriggerStrategy == "extend" && active) {
                expiresAt = checked(
                    self.ExpiresAt.Value + request.Ttl.Value * 1000L
                );
            }
            else {
                clone.TriggeredAt = currentTimeMillis;
                expiresAt = checked(
                    currentTimeMillis + request.Ttl.Value * 1000L
                );
            }
            clone.ExpiresAt = expiresAt;
            clone.CreatedAt = currentTimeMillis;
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static TriggerByUserIdRequest Rate(
            this TriggerByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Schedule:TriggerByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class TriggerByUserIdRequestExt
    {
        public static TriggerByUserIdRequest Rate(
            this TriggerByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Schedule:TriggerByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
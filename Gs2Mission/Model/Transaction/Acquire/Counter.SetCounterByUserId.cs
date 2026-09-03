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
using System.Text.RegularExpressions; /* diff +++ */
using Gs2.Core.Exception;
/* diff +++ start */
using Gs2.Core.Model;
using Gs2.Core.Util;
/* diff +++ end */
using Gs2.Gs2Mission.Request;

namespace Gs2.Gs2Mission.Model.Transaction
{
    public static partial class CounterExt
    {
/* diff +++ start */
        public static Counter CreateSpeculativeCounter(
            string namespaceName,
            string counterName,
            string userId,
            string region,
            string ownerId,
            long createdAtMillis
        ) {
            if (string.IsNullOrEmpty(namespaceName) ||
                string.IsNullOrEmpty(counterName) ||
                string.IsNullOrEmpty(userId) ||
                string.IsNullOrEmpty(region) || string.IsNullOrEmpty(ownerId)) {
                throw new InvalidOperationException();
            }
            return new Counter {
                CounterId = $"grn:gs2:{region}:{ownerId}:mission:{namespaceName}:user:{userId}:counter:{counterName}",
                UserId = userId,
                Name = counterName,
                Values = Array.Empty<ScopedValue>(),
                CreatedAt = createdAtMillis,
                UpdatedAt = createdAtMillis,
                Revision = 0,
            };
        }

/* diff +++ end */
        public static bool IsExecutable(
            this Counter self,
            SetCounterByUserIdRequest request
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

        public static Counter SpeculativeExecution(
            this Counter self,
            SetCounterByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Mission:SetCounterByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Mission:SetCounterByUserId");
//#endif
            return self.Clone() as Counter;
 diff --- end */
/* diff +++ start */
            return self.SpeculativeExecutionAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static Counter SpeculativeExecutionAt(
            this Counter self,
            SetCounterByUserIdRequest request,
            long currentTimeMillis
        ) {
            if (self == null || request == null ||
                self.UserId != request.UserId || self.Name != request.CounterName) {
                throw new BadRequestException(new [] {
                    new RequestError("counter", "invalid"),
                });
            }
            var clone = self.Clone() as Counter;
            if (clone == null) {
                throw new BadRequestException(new [] {
                    new RequestError("counter", "invalid"),
                });
            }
            clone.Values = (request.Values ?? Array.Empty<ScopedValue>())
                .Where(v => v != null)
                .Select(v => new ScopedValue {
                ScopeType = string.IsNullOrEmpty(v.ScopeType)
                    ? "resetTiming"
                    : v.ScopeType,
                ResetType = v.ResetType,
                ConditionName = v.ConditionName,
                Value = v.Value ?? 0,
                NextResetAt = v.NextResetAt,
                UpdatedAt = currentTimeMillis,
            }).ToArray();
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static SetCounterByUserIdRequest Rate(
            this SetCounterByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Mission:SetCounterByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class SetCounterByUserIdRequestExt
    {
        public static SetCounterByUserIdRequest Rate(
            this SetCounterByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Mission:SetCounterByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
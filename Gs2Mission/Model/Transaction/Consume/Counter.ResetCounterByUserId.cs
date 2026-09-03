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
        public static bool IsExecutable(
            this Counter self,
            ResetCounterByUserIdRequest request
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
            ResetCounterByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Mission:ResetCounterByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Mission:ResetCounterByUserId");
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
            ResetCounterByUserIdRequest request,
            long currentTimeMillis
        ) {
            if (self == null || request == null || self.Values == null ||
                self.UserId != request.UserId || self.Name != request.CounterName) {
                throw new BadRequestException(new [] {
                    new RequestError("counter", "invalid"),
                });
            }
            var scopes = (request.Scopes ?? Array.Empty<ScopedValue>())
                .Where(scope => scope != null)
                .ToArray();
            if (scopes.Length == 0) {
                return self;
            }
            var clone = self.Clone() as Counter;
            if (clone == null) {
                throw new BadRequestException(new [] {
                    new RequestError("counter", "invalid"),
                });
            }
            var retained = clone.Values
                .Where(v => v != null && !scopes.Any(scope =>
                    (string.IsNullOrEmpty(scope.ScopeType) ? "resetTiming" : scope.ScopeType) ==
                        (string.IsNullOrEmpty(v.ScopeType) ? "resetTiming" : v.ScopeType) &&
                    NormalizeResetType(scope.ResetType) == NormalizeResetType(v.ResetType) &&
                    (scope.ConditionName ?? "") == (v.ConditionName ?? "")
                ))
                .Select(v => v.Clone() as ScopedValue);
            var reset = scopes.Select(v => new ScopedValue {
                ScopeType = string.IsNullOrEmpty(v.ScopeType)
                    ? "resetTiming"
                    : v.ScopeType,
                ResetType = NormalizeResetType(v.ResetType),
                ConditionName = v.ConditionName,
                Value = 0,
                NextResetAt = null,
                UpdatedAt = currentTimeMillis,
            });
            clone.Values = retained.Concat(reset).ToArray();
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
            return clone;
        }

        private static string NormalizeResetType(string resetType) {
            return resetType ?? "notReset";
/* diff +++ end */
        }

        public static ResetCounterByUserIdRequest Rate(
            this ResetCounterByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Mission:ResetCounterByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class ResetCounterByUserIdRequestExt
    {
        public static ResetCounterByUserIdRequest Rate(
            this ResetCounterByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Mission:ResetCounterByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
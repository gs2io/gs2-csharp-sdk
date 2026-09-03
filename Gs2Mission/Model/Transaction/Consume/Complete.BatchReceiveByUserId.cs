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
using System.Collections.Generic; /* diff +++ */
using System.Linq;
using System.Numerics;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Mission.Request;

namespace Gs2.Gs2Mission.Model.Transaction
{
    public static partial class CompleteExt
    {
        public static bool IsExecutable(
            this Complete self,
            BatchReceiveByUserIdRequest request
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

        public static Complete SpeculativeExecution(
            this Complete self,
            BatchReceiveByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Mission:BatchReceiveByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Mission:BatchReceiveByUserId");
//#endif
            return self.Clone() as Complete;
 diff --- end */
/* diff +++ start */
            if (self?.ReceivedMissionTaskNames == null ||
                request?.MissionTaskNames == null) {
                throw new NullReferenceException();
            }
            var seen = new HashSet<string>();
            var requested = request.MissionTaskNames
                .Where(value => value != null && seen.Add(value))
                .ToArray();
            if (requested.Length == 0) {
                return self;
            }
            if (requested.Any(self.ReceivedMissionTaskNames.Contains)) {
                throw new InvalidOperationException("mission task is already received");
            }

            var clone = self.Clone() as Complete;
            clone.ReceivedMissionTaskNames = clone.ReceivedMissionTaskNames
                .Concat(requested)
                .ToArray();
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static BatchReceiveByUserIdRequest Rate(
            this BatchReceiveByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Mission:BatchReceiveByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class BatchReceiveByUserIdRequestExt
    {
        public static BatchReceiveByUserIdRequest Rate(
            this BatchReceiveByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Mission:BatchReceiveByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
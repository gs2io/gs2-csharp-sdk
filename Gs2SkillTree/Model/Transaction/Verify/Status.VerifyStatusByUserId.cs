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
using Gs2.Core.Model; /* diff +++ */
using Gs2.Gs2SkillTree.Request;

namespace Gs2.Gs2SkillTree.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            VerifyStatusByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.ReleasedNodeNames == null || request?.NodeModelName == null) {
                return false;
            }
            var released = self.ReleasedNodeNames.Contains(request.NodeModelName);
/* diff +++ end */
            switch (request.VerifyType) {
                case "released":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2SkillTree:VerifyStatusByUserId");
 diff --- end */
                    return released; /* diff +++ */
                case "notReleased":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2SkillTree:VerifyStatusByUserId");
 diff --- end */
                    return !released; /* diff +++ */
            }
            return false;
        }

        public static Status SpeculativeExecution(
            this Status self,
            VerifyStatusByUserIdRequest request
        ) {
/* diff +++ start */
            var reason = StatusVerificationFailureReason(request?.VerifyType);
            if (self?.ReleasedNodeNames != null && request?.NodeModelName != null &&
                reason != null && !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "nodeModelName",
                        $"skillTree.status.nodeModelName.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as Status;
        }

/* diff +++ start */
        internal static string StatusVerificationFailureReason(
            string verifyType
        ) {
            switch (verifyType) {
                case "released": return "notReleased";
                case "notReleased": return "released";
                default: return null;
            }
        }

/* diff +++ end */
        public static VerifyStatusByUserIdRequest Rate(
            this VerifyStatusByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2SkillTree:VerifyStatusByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、数量を掛けても検証は変わらない。
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyStatusByUserIdRequestExt
    {
        public static VerifyStatusByUserIdRequest Rate(
            this VerifyStatusByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2SkillTree:VerifyStatusByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
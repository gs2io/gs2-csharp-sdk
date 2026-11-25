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
using Gs2.Gs2Friend.Request;

namespace Gs2.Gs2Friend.Model.Transaction
{
    public static partial class FollowExt
    {
        public static bool IsExecutable(
            this Follow self,
            VerifyFollowByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.TargetUserIds == null || request?.TargetUserId == null) {
                return false;
            }
            var included = self.TargetUserIds.Contains(request.TargetUserId);
/* diff +++ end */
            switch (request.VerifyType) {
                case "include":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Friend:VerifyFollowByUserId");
 diff --- end */
                    return included; /* diff +++ */
                case "notInclude":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Friend:VerifyFollowByUserId");
 diff --- end */
                    return !included; /* diff +++ */
            }
            return false;
        }

        public static Follow SpeculativeExecution(
            this Follow self,
            VerifyFollowByUserIdRequest request
        ) {
/* diff +++ start */
            // 未知の検証の種類はサーバーが判定するので、ここでは投げない。
            var reason = FollowIncludeVerificationFailureReason(request?.VerifyType);
            if (self?.TargetUserIds != null && request?.TargetUserId != null &&
                reason != null && !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "targetUserId",
                        $"friend.follow.targetUserId.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as Follow;
        }

/* diff +++ start */
        internal static string FollowIncludeVerificationFailureReason(
            string verifyType
        ) {
            switch (verifyType) {
                case "include": return "notInclude";
                case "notInclude": return "include";
                default: return null;
            }
        }
/* diff +++ end */
        public static VerifyFollowByUserIdRequest Rate(
            this VerifyFollowByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Friend:VerifyFollowByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、検証は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyFollowByUserIdRequestExt
    {
        public static VerifyFollowByUserIdRequest Rate(
            this VerifyFollowByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Friend:VerifyFollowByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、検証は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }
}
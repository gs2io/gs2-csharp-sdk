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
    public static partial class FriendExt
    {
        public static bool IsExecutable(
            this Friend self,
            VerifyFriendCountByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.TargetUserIds == null || request?.Count == null) {
                return false;
            }
            var current = self.TargetUserIds.Length;
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Friend:VerifyFriendCountByUserId");
 diff --- end */
                    return current < request.Count; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Friend:VerifyFriendCountByUserId");
 diff --- end */
                    return current <= request.Count; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Friend:VerifyFriendCountByUserId");
 diff --- end */
                    return current > request.Count; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Friend:VerifyFriendCountByUserId");
 diff --- end */
                    return current >= request.Count; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Friend:VerifyFriendCountByUserId");
 diff --- end */
                    return current == request.Count; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Friend:VerifyFriendCountByUserId");
 diff --- end */
                    return current != request.Count; /* diff +++ */
            }
            return false;
        }

        public static Friend SpeculativeExecution(
            this Friend self,
            VerifyFriendCountByUserIdRequest request
        ) {
/* diff +++ start */
            // 未知の検証の種類はサーバーが判定するので、ここでは投げない。
            var reason = FriendCountVerificationFailureReason(request?.VerifyType);
            if (self?.TargetUserIds != null && request?.Count != null &&
                reason != null && !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "count",
                        $"friend.friend.count.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as Friend;
        }

/* diff +++ start */
        internal static string FriendCountVerificationFailureReason(
            string verifyType
        ) {
            switch (verifyType) {
                case "less": return "greaterEqual";
                case "lessEqual": return "greater";
                case "greater": return "lessEqual";
                case "greaterEqual": return "less";
                case "equal": return "notEqual";
                case "notEqual": return "equal";
                default: return null;
            }
        }
/* diff +++ end */
        public static VerifyFriendCountByUserIdRequest Rate(
            this VerifyFriendCountByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Friend:VerifyFriendCountByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、検証は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyFriendCountByUserIdRequestExt
    {
        public static VerifyFriendCountByUserIdRequest Rate(
            this VerifyFriendCountByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Friend:VerifyFriendCountByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、検証は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }
}
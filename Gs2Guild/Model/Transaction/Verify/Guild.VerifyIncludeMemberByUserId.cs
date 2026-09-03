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
using Gs2.Gs2Guild.Request;

namespace Gs2.Gs2Guild.Model.Transaction
{
    public static partial class GuildExt
    {
        public static bool IsExecutable(
            this Guild self,
            VerifyIncludeMemberByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.Members == null || request?.UserId == null) {
                return false;
            }
            var included = self.Members.Any(v => v?.UserId == request.UserId);
/* diff +++ end */
            switch (request.VerifyType) {
                case "include":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Guild:VerifyIncludeMemberByUserId");
 diff --- end */
                    return included; /* diff +++ */
                case "notInclude":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Guild:VerifyIncludeMemberByUserId");
 diff --- end */
                    return !included; /* diff +++ */
            }
            return false;
        }

        public static Guild SpeculativeExecution(
            this Guild self,
            VerifyIncludeMemberByUserIdRequest request
        ) {
            return self.Clone() as Guild;
        }

        public static VerifyIncludeMemberByUserIdRequest Rate(
            this VerifyIncludeMemberByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Guild:VerifyIncludeMemberByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class VerifyIncludeMemberByUserIdRequestExt
    {
        public static VerifyIncludeMemberByUserIdRequest Rate(
            this VerifyIncludeMemberByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Guild:VerifyIncludeMemberByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
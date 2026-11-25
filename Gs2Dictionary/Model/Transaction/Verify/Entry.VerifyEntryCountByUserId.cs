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
using Gs2.Gs2Dictionary.Request;

namespace Gs2.Gs2Dictionary.Model.Transaction
{
    public static partial class EntryExt
    {
        public static bool IsExecutable(
/* diff --- start
            this Entry self,
 diff --- end */
            this Entry[] self, /* diff +++ */
            VerifyEntryCountByUserIdRequest request
        ) {
/* diff +++ start */
            if (self == null || request?.Count == null || self.Any(v => v?.Name == null)) {
                return false;
            }
            // 名前を指定したときはその名前だけを数え、同じ名前を重ねて指定しても 1 件と数える (サーバーと同じ)。
            var targets = request.EntryModelNames == null || request.EntryModelNames.Length == 0
                ? null
                : new System.Collections.Generic.HashSet<string>(request.EntryModelNames);
            var current = self.Count(v => targets == null || targets.Contains(v.Name));
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Dictionary:VerifyEntryCountByUserId");
 diff --- end */
                    return current < request.Count; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Dictionary:VerifyEntryCountByUserId");
 diff --- end */
                    return current <= request.Count; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Dictionary:VerifyEntryCountByUserId");
 diff --- end */
                    return current > request.Count; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Dictionary:VerifyEntryCountByUserId");
 diff --- end */
                    return current >= request.Count; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Dictionary:VerifyEntryCountByUserId");
 diff --- end */
                    return current == request.Count; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Dictionary:VerifyEntryCountByUserId");
 diff --- end */
                    return current != request.Count; /* diff +++ */
            }
            return false;
        }

/* diff --- start
        public static Entry SpeculativeExecution(
            this Entry self,
 diff --- end */
/* diff +++ start */
        public static Entry[] SpeculativeExecution(
            this Entry[] self,
/* diff +++ end */
            VerifyEntryCountByUserIdRequest request
        ) {
/* diff --- start
            return self.Clone() as Entry;
 diff --- end */
/* diff +++ start */
            // 未知の検証の種類はサーバーが判定するので、ここでは投げない。
            var reason = EntryCountVerificationFailureReason(request?.VerifyType);
            if (self != null && request?.Count != null &&
                self.All(v => v?.Name != null) && reason != null &&
                !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "count",
                        $"dictionary.entry.count.error.{reason}"
                    ),
                });
            }
            return self.Clone() as Entry[];
        }

        internal static string EntryCountVerificationFailureReason(string verifyType)
        {
            switch (verifyType) {
                case "less": return "greaterEqual";
                case "lessEqual": return "greater";
                case "greater": return "lessEqual";
                case "greaterEqual": return "less";
                case "equal": return "notEqual";
                case "notEqual": return "equal";
                default: return null;
            }
/* diff +++ end */
        }

        public static VerifyEntryCountByUserIdRequest Rate(
            this VerifyEntryCountByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Dictionary:VerifyEntryCountByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、比べる数は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyEntryCountByUserIdRequestExt
    {
        public static VerifyEntryCountByUserIdRequest Rate(
            this VerifyEntryCountByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Dictionary:VerifyEntryCountByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、比べる数は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }
}
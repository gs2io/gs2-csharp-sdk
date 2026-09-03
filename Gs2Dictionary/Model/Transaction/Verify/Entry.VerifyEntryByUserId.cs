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
using Gs2.Core.Model;
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
            VerifyEntryByUserIdRequest request
        ) {
            if (self == null ||
                request?.EntryModelName == null ||
                self.Any(v => v?.Name == null)) {
                return false;
            }

            switch (request.VerifyType) {
                case "havent":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Dictionary:VerifyEntryByUserId");
 diff --- end */
                    return !self.Any(v => v.Name == request.EntryModelName); /* diff +++ */
                case "have":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Dictionary:VerifyEntryByUserId");
 diff --- end */
                    return self.Any(v => v.Name == request.EntryModelName); /* diff +++ */
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
            VerifyEntryByUserIdRequest request
        ) {
/* diff --- start
            return self.Clone() as Entry;
 diff --- end */
            var reason = EntryVerificationFailureReason(request?.VerifyType);
            if (self != null && request?.EntryModelName != null &&
                self.All(v => v?.Name != null) && reason != null &&
                !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "entry",
                        $"dictionary.entry.entry.error.{reason}"
                    ),
                });
            }
            return self.Clone() as Entry[]; /* diff +++ */
        }

        internal static string EntryVerificationFailureReason(string verifyType)
        {
            switch (verifyType) {
                case "havent": return "have";
                case "have": return "havent";
                default: return null;
            }
        }

        public static VerifyEntryByUserIdRequest Rate(
            this VerifyEntryByUserIdRequest request,
            double rate
        ) {
            return request;
        }
    }

    public static partial class VerifyEntryByUserIdRequestExt
    {
        public static VerifyEntryByUserIdRequest Rate(
            this VerifyEntryByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}

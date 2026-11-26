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
using Gs2.Gs2Formation.Request;

namespace Gs2.Gs2Formation.Model.Transaction
{
    public static partial class FormExt
    {
        public static bool IsExecutable(
            this Form self,
            VerifyFormByUserIdRequest request
        ) {
/* diff +++ start */
            // propertyIdRegex は呼び手が {region}・{ownerId}・{userId} を置き換えたもの。
            var regex = FormPropertyIdRegex(request?.PropertyIdRegex);
            var countMatched = FormCountMatched(request?.CountType);
            if (self?.Slots == null || regex == null || countMatched == null || request.Count == null) {
                return false;
            }
            int current;
            try {
                current = self.Slots.Count(v =>
                    v != null &&
                    (string.IsNullOrEmpty(request.SlotName) || v.Name == request.SlotName) &&
                    !string.IsNullOrEmpty(v.PropertyId) &&
                    regex.IsMatch(v.PropertyId) == countMatched.Value
                );
            }
            catch (System.Text.RegularExpressions.RegexMatchTimeoutException) {
                return false;
            }
/* diff +++ end */
            switch (request.VerifyType) {
                case "less":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Formation:VerifyFormByUserId");
 diff --- end */
                    return current < request.Count; /* diff +++ */
                case "lessEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Formation:VerifyFormByUserId");
 diff --- end */
                    return current <= request.Count; /* diff +++ */
                case "greater":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Formation:VerifyFormByUserId");
 diff --- end */
                    return current > request.Count; /* diff +++ */
                case "greaterEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Formation:VerifyFormByUserId");
 diff --- end */
                    return current >= request.Count; /* diff +++ */
                case "equal":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Formation:VerifyFormByUserId");
 diff --- end */
                    return current == request.Count; /* diff +++ */
                case "notEqual":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Formation:VerifyFormByUserId");
 diff --- end */
                    return current != request.Count; /* diff +++ */
            }
            return false;
        }

        public static Form SpeculativeExecution(
            this Form self,
            VerifyFormByUserIdRequest request
        ) {
/* diff +++ start */
            // 判定できない正規表現や未知の種類はサーバーが判定するので、ここでは投げない。
            var reason = FormCountVerificationFailureReason(request?.VerifyType);
            if (self?.Slots != null && request?.Count != null && reason != null &&
                FormPropertyIdRegex(request.PropertyIdRegex) != null &&
                FormCountMatched(request.CountType) != null &&
                !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "count",
                        $"formation.form.count.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as Form;
        }

/* diff +++ start */
        // サーバーは Go の regexp (RE2) で照合する。.NET だけが受け付ける書き方 (後方参照・先読み・後読み・
        // 名前付きの参照) が入っていると、サーバーでは不正になるので、手元では判定しない (null)。
        internal static System.Text.RegularExpressions.Regex FormPropertyIdRegex(string pattern)
        {
            if (pattern == null ||
                pattern.Contains("(?=") || pattern.Contains("(?!") ||
                pattern.Contains("(?<=") || pattern.Contains("(?<!") ||
                pattern.Contains("\\k<") ||
                System.Text.RegularExpressions.Regex.IsMatch(pattern, @"\\[1-9]")) {
                return null;
            }
            try {
                return new System.Text.RegularExpressions.Regex(
                    pattern,
                    System.Text.RegularExpressions.RegexOptions.CultureInvariant,
                    TimeSpan.FromMilliseconds(100)
                );
            }
            catch (ArgumentException) {
                return null;
            }
        }

        // countType は未指定ならサーバーと同じく match。
        internal static bool? FormCountMatched(string countType)
        {
            switch (countType) {
                case null:
                case "":
                case "match": return true;
                case "notMatch": return false;
                default: return null;
            }
        }

        internal static string FormCountVerificationFailureReason(string verifyType)
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
        }
/* diff +++ end */
        public static VerifyFormByUserIdRequest Rate(
            this VerifyFormByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Formation:VerifyFormByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、比べる数は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyFormByUserIdRequestExt
    {
        public static VerifyFormByUserIdRequest Rate(
            this VerifyFormByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Formation:VerifyFormByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、比べる数は数量を掛けずに据え置く。
            return request;
/* diff +++ end */
        }
    }
}
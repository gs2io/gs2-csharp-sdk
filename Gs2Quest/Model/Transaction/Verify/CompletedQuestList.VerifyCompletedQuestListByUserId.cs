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
using Gs2.Gs2Quest.Request;

namespace Gs2.Gs2Quest.Model.Transaction
{
    public static partial class CompletedQuestListExt
    {
        public static bool IsExecutable(
            this CompletedQuestList self,
            VerifyCompletedQuestListByUserIdRequest request
        ) {
/* diff +++ start */
            if (self?.CompleteQuestNames == null || request?.QuestName == null) {
                return false;
            }
            var completed = self.CompleteQuestNames.Contains(request.QuestName);
/* diff +++ end */
            switch (request.VerifyType) {
                case "completed":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Quest:VerifyCompletedQuestListByUserId");
 diff --- end */
                    return completed; /* diff +++ */
                case "notCompleted":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Quest:VerifyCompletedQuestListByUserId");
 diff --- end */
                    return !completed; /* diff +++ */
            }
            return false;
        }

        public static CompletedQuestList SpeculativeExecution(
            this CompletedQuestList self,
            VerifyCompletedQuestListByUserIdRequest request
        ) {
/* diff +++ start */
            var reason = CompletedQuestListVerificationFailureReason(request?.VerifyType);
            if (self?.CompleteQuestNames != null && request?.QuestName != null &&
                reason != null && !self.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "questName",
                        $"quest.completedQuestList.questName.error.{reason}"
                    ),
                });
            }
/* diff +++ end */
            return self.Clone() as CompletedQuestList;
        }

/* diff +++ start */
        internal static string CompletedQuestListVerificationFailureReason(
            string verifyType
        ) {
            switch (verifyType) {
                case "completed": return "notCompleted";
                case "notCompleted": return "completed";
                default: return null;
            }
        }

/* diff +++ end */
        public static VerifyCompletedQuestListByUserIdRequest Rate(
            this VerifyCompletedQuestListByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Quest:VerifyCompletedQuestListByUserId");
 diff --- end */
/* diff +++ start */
            // サーバー (core/transaction/action) と同じく、数量を掛けても検証は変わらない。
            return request;
/* diff +++ end */
        }
    }

    public static partial class VerifyCompletedQuestListByUserIdRequestExt
    {
        public static VerifyCompletedQuestListByUserIdRequest Rate(
            this VerifyCompletedQuestListByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Quest:VerifyCompletedQuestListByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
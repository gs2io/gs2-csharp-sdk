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
using Gs2.Gs2Inbox.Request;

namespace Gs2.Gs2Inbox.Model.Transaction
{
    public static partial class MessageExt
    {
        public static bool IsExecutable(
            this Message self,
            DeleteMessageByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
            try {
                changed.Validate();
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
 diff --- end */
/* diff +++ start */
            return self != null &&
                   request != null &&
                   !string.IsNullOrEmpty(self.UserId) &&
                   !string.IsNullOrEmpty(request.UserId) &&
                   !string.IsNullOrEmpty(self.Name) &&
                   !string.IsNullOrEmpty(request.MessageName) &&
                   self.UserId == request.UserId &&
                   self.Name == request.MessageName;
/* diff +++ end */
        }

        public static Message SpeculativeExecution(
            this Message self,
            DeleteMessageByUserIdRequest request
        ) {
            return null;
        }

        public static DeleteMessageByUserIdRequest Rate(
            this DeleteMessageByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inbox:DeleteMessageByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class DeleteMessageByUserIdRequestExt
    {
        public static DeleteMessageByUserIdRequest Rate(
            this DeleteMessageByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inbox:DeleteMessageByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
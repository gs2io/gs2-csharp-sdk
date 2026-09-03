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
using Gs2.Gs2LoginReward.Request;

namespace Gs2.Gs2LoginReward.Model.Transaction
{
    public static partial class ReceiveStatusExt
    {
        public static bool IsExecutable(
            this ReceiveStatus self,
            DeleteReceiveStatusByUserIdRequest request
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
                   !string.IsNullOrEmpty(self.BonusModelName) &&
                   !string.IsNullOrEmpty(request.BonusModelName) &&
                   self.UserId == request.UserId &&
                   self.BonusModelName == request.BonusModelName;
/* diff +++ end */
        }

        public static ReceiveStatus SpeculativeExecution(
            this ReceiveStatus self,
            DeleteReceiveStatusByUserIdRequest request
        ) {
            return null;
        }

        public static DeleteReceiveStatusByUserIdRequest Rate(
            this DeleteReceiveStatusByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2LoginReward:DeleteReceiveStatusByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class DeleteReceiveStatusByUserIdRequestExt
    {
        public static DeleteReceiveStatusByUserIdRequest Rate(
            this DeleteReceiveStatusByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2LoginReward:DeleteReceiveStatusByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
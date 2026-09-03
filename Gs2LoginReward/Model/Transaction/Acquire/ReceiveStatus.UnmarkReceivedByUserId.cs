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
using Gs2.Core.Util;
using Gs2.Gs2LoginReward.Request;

namespace Gs2.Gs2LoginReward.Model.Transaction
{
    public static partial class ReceiveStatusExt
    {
        public static bool IsExecutable(
            this ReceiveStatus self,
            UnmarkReceivedByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }

        public static ReceiveStatus SpeculativeExecution(
            this ReceiveStatus self,
            UnmarkReceivedByUserIdRequest request
        ) {
            return self.SpeculativeExecutionAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static ReceiveStatus SpeculativeExecutionAt(
            this ReceiveStatus self,
            UnmarkReceivedByUserIdRequest request,
            long currentTimeMillis
        ) {
            if (self == null || request?.StepNumber == null ||
                request.StepNumber < 0) {
                throw new InvalidOperationException();
            }
            var stepNumber = request.StepNumber.Value;
            if (stepNumber >= (self.ReceivedSteps?.Length ?? 0) || !self.ReceivedSteps[stepNumber]) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "receivedStep",
                        "loginReward.receiveStatus.receivedStep.error.notReceived"
                    ),
                });
            }

            var clone = self.Clone() as ReceiveStatus;
            if (clone == null) {
                throw new NullReferenceException();
            }

            var receivedSteps = clone.ReceivedSteps;
            receivedSteps[stepNumber] = false;
            clone.ReceivedSteps = receivedSteps;
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
            return clone;
        }

        public static UnmarkReceivedByUserIdRequest Rate(
            this UnmarkReceivedByUserIdRequest request,
            double rate
        ) {
            return request;
        }
    }

    public static partial class UnmarkReceivedByUserIdRequestExt
    {
        public static UnmarkReceivedByUserIdRequest Rate(
            this UnmarkReceivedByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}

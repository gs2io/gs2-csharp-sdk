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
/* diff +++ start */
using Gs2.Core.Model;
using Gs2.Core.Util;
/* diff +++ end */
using Gs2.Gs2SerialKey.Request;

namespace Gs2.Gs2SerialKey.Model.Transaction
{
    public static partial class SerialKeyExt
    {
        public static bool IsExecutable(
            this SerialKey self,
            UseByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
                self.SpeculativeExecution(request); /* diff +++ */
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

        public static SerialKey SpeculativeExecution(
            this SerialKey self,
            UseByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2SerialKey:UseByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2SerialKey:UseByUserId");
//#endif
            return self.Clone() as SerialKey;
 diff --- end */
/* diff +++ start */
            return self.SpeculativeUseAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static bool IsUseExecutableAt(
            this SerialKey self,
            UseByUserIdRequest request,
            long currentTimeMillis
        ) {
            try {
                self.SpeculativeUseAt(request, currentTimeMillis);
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

        public static SerialKey SpeculativeUseAt(
            this SerialKey self,
            UseByUserIdRequest request,
            long currentTimeMillis
        ) {
            if (string.IsNullOrEmpty(request?.UserId)) {
                throw new BadRequestException(new [] {
                    new RequestError("userId", "invalid"),
                });
            }
            if (string.IsNullOrEmpty(request.Code)) {
                throw new BadRequestException(new [] {
                    new RequestError("code", "invalid"),
                });
            }
            if (currentTimeMillis < 0) {
                throw new BadRequestException(new [] {
                    new RequestError("timeOffsetToken", "invalid"),
                });
            }
            if (self == null || self.Code != request.Code || self.Status != "ACTIVE") {
                throw new BadRequestException(new [] {
                    new RequestError("code", "invalid"),
                });
            }

            var clone = self.Clone() as SerialKey;
            clone.Status = "USED";
            clone.UsedUserId = request.UserId;
            clone.UsedAt = currentTimeMillis;
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static UseByUserIdRequest Rate(
            this UseByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2SerialKey:UseByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class UseByUserIdRequestExt
    {
        public static UseByUserIdRequest Rate(
            this UseByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2SerialKey:UseByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
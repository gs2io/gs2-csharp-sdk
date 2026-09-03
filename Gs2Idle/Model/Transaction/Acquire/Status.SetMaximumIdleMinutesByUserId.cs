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
/* diff --- start
using System.Linq;
 diff --- end */
using System.Numerics;
using System.Text.RegularExpressions; /* diff +++ */
using Gs2.Core.Exception;
/* diff +++ start */
using Gs2.Core.Model;
using Gs2.Core.Util;
/* diff +++ end */
using Gs2.Gs2Idle.Request;

namespace Gs2.Gs2Idle.Model.Transaction
{
    public static partial class StatusExt
    {
/* diff +++ start */
        private static bool IsStatusIdentityValid(
            Status self,
            SetMaximumIdleMinutesByUserIdRequest request,
            string region,
            string ownerId
        ) {
            if (self.CategoryName != request.CategoryName ||
                self.UserId != request.UserId) {
                return false;
            }
            if (!string.IsNullOrEmpty(region) && !string.IsNullOrEmpty(ownerId)) {
                return self.StatusId ==
                       $"grn:gs2:{region}:{ownerId}:idle:{request.NamespaceName}:user:{request.UserId}:categoryModel:{request.CategoryName}";
            }
            var statusIdRegex = new Regex(
                @"\Agrn:gs2:(ap-northeast-1|us-east-1|eu-west-1|ap-southeast-1):" +
                @"[-_.a-zA-Z0-9]{1,64}:idle:" +
                Regex.Escape(request.NamespaceName) + ":user:" +
                Regex.Escape(request.UserId) + ":categoryModel:" +
                Regex.Escape(request.CategoryName) + @"\z",
                RegexOptions.CultureInvariant
            );
            return statusIdRegex.IsMatch(self.StatusId);
        }

/* diff +++ end */
        public static bool IsExecutable(
            this Status self,
            SetMaximumIdleMinutesByUserIdRequest request
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
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (System.Exception) { /* diff +++ */
                return false;
            }
        }

        public static Status SpeculativeExecution(
            this Status self,
            SetMaximumIdleMinutesByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Idle:SetMaximumIdleMinutesByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Idle:SetMaximumIdleMinutesByUserId");
//#endif
            return self.Clone() as Status;
 diff --- end */
/* diff +++ start */
            return self.SpeculativeExecutionAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static Status SpeculativeExecutionAt(
            this Status self,
            SetMaximumIdleMinutesByUserIdRequest request,
            long currentTimeMillis,
            string region = null,
            string ownerId = null
        ) {
            if (self == null || request == null ||
                !IsStatusIdentityValid(self, request, region, ownerId)) {
                throw new BadRequestException(new [] {
                    new RequestError("status", "invalid"),
                });
            }
            var clone = self.Clone() as Status;
            clone.MaximumIdleMinutes = request.MaximumIdleMinutes ?? 0;
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static SetMaximumIdleMinutesByUserIdRequest Rate(
            this SetMaximumIdleMinutesByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Idle:SetMaximumIdleMinutesByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class SetMaximumIdleMinutesByUserIdRequestExt
    {
        public static SetMaximumIdleMinutesByUserIdRequest Rate(
            this SetMaximumIdleMinutesByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Idle:SetMaximumIdleMinutesByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
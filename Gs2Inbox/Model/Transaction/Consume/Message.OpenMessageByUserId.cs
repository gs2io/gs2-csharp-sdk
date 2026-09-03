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
using Gs2.Gs2Inbox.Request;

namespace Gs2.Gs2Inbox.Model.Transaction
{
    public static partial class MessageExt
    {
/* diff +++ start */
        private static bool IsMessageStateValid(
            Message self
        ) {
            return self != null &&
                   !string.IsNullOrEmpty(self.MessageId) &&
                   !string.IsNullOrEmpty(self.Name) &&
                   !string.IsNullOrEmpty(self.UserId) &&
                   self.IsRead != null &&
                   self.ReceivedAt != null &&
                   self.Revision != null;
        }

        private static bool IsMessageIdentityValid(
            Message self,
            OpenMessageByUserIdRequest request,
            string region,
            string ownerId
        ) {
            if (self.Name != request.MessageName || self.UserId != request.UserId) {
                return false;
            }
            if (!string.IsNullOrEmpty(region) && !string.IsNullOrEmpty(ownerId)) {
                return self.MessageId ==
                       $"grn:gs2:{region}:{ownerId}:inbox:{request.NamespaceName}:user:{request.UserId}:message:{request.MessageName}";
            }
            var messageIdRegex = new Regex(
                @"\Agrn:gs2:(ap-northeast-1|us-east-1|eu-west-1|ap-southeast-1):" +
                @"[-_.a-zA-Z0-9]{1,64}:inbox:" +
                Regex.Escape(request.NamespaceName) + ":user:" +
                Regex.Escape(request.UserId) + ":message:" +
                Regex.Escape(request.MessageName) + @"\z",
                RegexOptions.CultureInvariant
            );
            return messageIdRegex.IsMatch(self.MessageId);
        }

/* diff +++ end */
        public static bool IsExecutable(
            this Message self,
            OpenMessageByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
/* diff +++ start */
                self.SpeculativeExecution(request);
                return false;
            }
            catch (System.Exception) {
                return false;
            }
        }

        public static bool IsExecutable(
            this Message self,
            OpenMessageByUserIdRequest request,
            bool automaticDeletingEnabled
        ) {
            return false;
        }

        public static bool IsExecutable(
            this Message self,
            OpenMessageByUserIdRequest request,
            bool automaticDeletingEnabled,
            long namespaceCreatedAt
        ) {
            try {
                self.SpeculativeExecution(
                    request,
                    automaticDeletingEnabled,
                    namespaceCreatedAt
                );
/* diff +++ end */
                return true;
            }
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (System.Exception) { /* diff +++ */
                return false;
            }
        }

        public static Message SpeculativeExecution(
            this Message self,
            OpenMessageByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Inbox:OpenMessageByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Inbox:OpenMessageByUserId");
//#endif
            return self.Clone() as Message;
 diff --- end */
/* diff +++ start */
            throw new NotSupportedException(
                "Namespace.IsAutomaticDeletingEnabled is required for speculative " +
                "execution of Gs2Inbox:OpenMessageByUserId"
            );
        }

        public static Message SpeculativeExecution(
            this Message self,
            OpenMessageByUserIdRequest request,
            bool automaticDeletingEnabled
        ) {
            throw new NotSupportedException(
                "Namespace.CreatedAt is required for speculative execution of " +
                "Gs2Inbox:OpenMessageByUserId"
            );
        }

        public static Message SpeculativeExecution(
            this Message self,
            OpenMessageByUserIdRequest request,
            bool automaticDeletingEnabled,
            long namespaceCreatedAt
        ) {
            var now = UnixTime.ToUnixTime(DateTime.Now);
            return self.SpeculativeExecutionAt(
                request,
                automaticDeletingEnabled,
                namespaceCreatedAt,
                now,
                now
            );
        }

        public static Message SpeculativeExecutionAt(
            this Message self,
            OpenMessageByUserIdRequest request,
            bool automaticDeletingEnabled,
            long namespaceCreatedAt,
            long currentTimeMillis,
            long physicalTimeMillis,
            string region = null,
            string ownerId = null
        ) {
            if (!IsMessageStateValid(self) ||
                !IsMessageIdentityValid(self, request, region, ownerId)) {
                throw new BadRequestException(new [] {
                    new RequestError("message", "invalid"),
                });
            }
            if (self.ExpiresAt != null && physicalTimeMillis > self.ExpiresAt) {
                throw new BadRequestException(new [] {
                    new RequestError("message", "notFound"),
                });
            }
            if (namespaceCreatedAt > self.ReceivedAt) {
                throw new BadRequestException(new [] {
                    new RequestError("message", "notFound"),
                });
            }
            if (self.IsRead == true) {
                throw new BadRequestException(new [] {
                    new RequestError("read", "alreadyRead"),
                });
            }
            if (automaticDeletingEnabled) {
                return null;
            }
            var clone = self.Clone() as Message;
            clone.IsRead = true;
            clone.ReadAt = currentTimeMillis;
            clone.Revision = clone.Revision + 1;
            return clone;
/* diff +++ end */
        }

        public static OpenMessageByUserIdRequest Rate(
            this OpenMessageByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inbox:OpenMessageByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class OpenMessageByUserIdRequestExt
    {
        public static OpenMessageByUserIdRequest Rate(
            this OpenMessageByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Inbox:OpenMessageByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
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
using Gs2.Gs2Formation.Request;

namespace Gs2.Gs2Formation.Model.Transaction
{
    public static partial class FormExt
    {
/* diff +++ start */
        private static bool TryGetFormContext(
            Form self,
            SetFormByUserIdRequest request,
            out string region,
            out string ownerId
        ) {
            region = null;
            ownerId = null;
            if (self?.FormId == null || request == null) {
                return false;
            }

            var suffix =
                $":formation:{request.NamespaceName}:user:{request.UserId}:mold:{request.MoldModelName}:form:{request.Index}";
            const string prefix = "grn:gs2:";
            if (!self.FormId.StartsWith(prefix, StringComparison.Ordinal) ||
                !self.FormId.EndsWith(suffix, StringComparison.Ordinal)) {
                return false;
            }

            var contextLength = self.FormId.Length - prefix.Length - suffix.Length;
            if (contextLength <= 0) {
                return false;
            }
            var context = self.FormId.Substring(prefix.Length, contextLength);
            var separator = context.IndexOf(':');
            if (separator <= 0 || separator == context.Length - 1 ||
                context.IndexOf(':', separator + 1) >= 0) {
                return false;
            }

            region = context.Substring(0, separator);
            ownerId = context.Substring(separator + 1);
            return !string.IsNullOrEmpty(region) && !string.IsNullOrEmpty(ownerId);
        }

/* diff +++ end */
        public static bool IsExecutable(
            this Form self,
            SetFormByUserIdRequest request
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

        public static Form SpeculativeExecution(
            this Form self,
            SetFormByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Formation:SetFormByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Formation:SetFormByUserId");
//#endif
            return self.Clone() as Form;
 diff --- end */
/* diff +++ start */
            if (!TryGetFormContext(self, request, out var region, out var ownerId)) {
                throw new BadRequestException(new[] {
                    new RequestError("form", "invalid"),
                });
            }
            return self.SpeculativeExecutionAt(
                request,
                UnixTime.ToUnixTime(DateTime.Now),
                region,
                ownerId
            );
        }

        public static Form SpeculativeExecutionAt(
            this Form self,
            SetFormByUserIdRequest request,
            long currentTimeMillis,
            string region,
            string ownerId
        ) {
            if (request?.Slots == null ||
                string.IsNullOrEmpty(region) ||
                string.IsNullOrEmpty(ownerId)) {
                throw new NullReferenceException();
            }
            var expectedFormId =
                $"grn:gs2:{region}:{ownerId}:formation:{request.NamespaceName}:user:{request.UserId}:mold:{request.MoldModelName}:form:{request.Index}";
            if (self == null || string.IsNullOrEmpty(self.FormId) ||
                self.FormId != expectedFormId || self.Name != request.MoldModelName ||
                self.Index != request.Index) {
                throw new BadRequestException(new[] {
                    new RequestError("form", "invalid"),
                });
            }
            if (self.Clone() is not Form clone) {
                throw new NullReferenceException();
            }
            clone.Slots = request.Slots
                .Select(slot => slot?.Clone() as Slot)
                .ToArray();
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = checked((clone.Revision ?? 0) + 1);
            return clone;
/* diff +++ end */
        }

        public static SetFormByUserIdRequest Rate(
            this SetFormByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Formation:SetFormByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class SetFormByUserIdRequestExt
    {
        public static SetFormByUserIdRequest Rate(
            this SetFormByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Formation:SetFormByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
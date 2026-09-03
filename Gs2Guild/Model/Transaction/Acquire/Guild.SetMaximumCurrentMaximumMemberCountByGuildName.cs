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
using Gs2.Core.Exception;
/* diff +++ start */
using Gs2.Core.Model;
using Gs2.Core.Util;
/* diff +++ end */
using Gs2.Gs2Guild.Request;

namespace Gs2.Gs2Guild.Model.Transaction
{
    public static partial class GuildExt
    {
/* diff +++ start */
        private static bool IsGuildIdentityValid(
            Guild self,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            string region,
            string ownerId
        ) {
            if (self?.GuildId == null || request == null ||
                self.GuildModelName != request.GuildModelName ||
                self.Name != request.GuildName) {
                return false;
            }
            var identityRegion = string.IsNullOrEmpty(region)
                ? Guild.GetRegionFromGrn(self.GuildId)
                : region;
            var identityOwnerId = string.IsNullOrEmpty(ownerId)
                ? Guild.GetOwnerIdFromGrn(self.GuildId)
                : ownerId;
            return !string.IsNullOrEmpty(identityRegion) &&
                   !string.IsNullOrEmpty(identityOwnerId) &&
                   self.GuildId ==
                   $"grn:gs2:{identityRegion}:{identityOwnerId}:guild:{request.NamespaceName}:guild:{request.GuildModelName}:{request.GuildName}";
        }

        private static bool IsGuildModelIdentityValid(
            GuildModel guildModel,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            string region,
            string ownerId
        ) {
            if (guildModel == null ||
                guildModel.Name != request.GuildModelName ||
                string.IsNullOrEmpty(guildModel.GuildModelId)) {
                return false;
            }
            return guildModel.GuildModelId ==
                   $"grn:gs2:{region}:{ownerId}:guild:{request.NamespaceName}:model:{request.GuildModelName}";
        }

/* diff +++ end */
        public static bool IsExecutable(
            this Guild self,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
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
/* diff +++ start */
            catch (System.Exception) {
                return false;
            }
        }

        public static bool IsExecutable(
            this Guild self,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            GuildModel guildModel
        ) {
            try {
                self.SpeculativeExecution(request, guildModel);
                return true;
            }
            catch (System.Exception) {
/* diff +++ end */
                return false;
            }
        }

        public static Guild SpeculativeExecution(
            this Guild self,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Guild:SetMaximumCurrentMaximumMemberCountByGuildName");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Guild:SetMaximumCurrentMaximumMemberCountByGuildName");
//#endif
            return self.Clone() as Guild;
 diff --- end */
/* diff +++ start */
            throw new NotSupportedException(
                "GuildModel is required for speculative execution of " +
                "Gs2Guild:SetMaximumCurrentMaximumMemberCountByGuildName"
            );
        }

        public static Guild SpeculativeExecution(
            this Guild self,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            GuildModel guildModel
        ) {
            var now = UnixTime.ToUnixTime(DateTime.Now);
            return self.SpeculativeExecutionAt(request, guildModel, now);
        }

        public static Guild SpeculativeExecutionAt(
            this Guild self,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            GuildModel guildModel,
            long currentTimeMillis,
            string region = null,
            string ownerId = null
        ) {
            if (request?.Value == null || self?.Members == null) {
                throw new NullReferenceException();
            }
            if (!IsGuildIdentityValid(self, request, region, ownerId)) {
                throw new BadRequestException(new [] {
                    new RequestError("guild", "invalid"),
                });
            }
            var identityRegion = string.IsNullOrEmpty(region)
                ? Guild.GetRegionFromGrn(self.GuildId)
                : region;
            var identityOwnerId = string.IsNullOrEmpty(ownerId)
                ? Guild.GetOwnerIdFromGrn(self.GuildId)
                : ownerId;
            if (!IsGuildModelIdentityValid(
                    guildModel,
                    request,
                    identityRegion,
                    identityOwnerId
                )) {
                throw new BadRequestException(new [] {
                    new RequestError("guildModel", "invalid"),
                });
            }
            var upperBound = guildModel.MaximumMemberCount ?? int.MaxValue;
            var nextValue = Math.Min(request.Value.Value, upperBound);
            nextValue = Math.Max(nextValue, self.Members.Length);

            var clone = self.Clone() as Guild;
            if (clone == null) {
                throw new NullReferenceException();
            }
            clone.CurrentMaximumMemberCount = nextValue;
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = checked((clone.Revision ?? 0) + 1);
            return clone;
/* diff +++ end */
        }

        public static SetMaximumCurrentMaximumMemberCountByGuildNameRequest Rate(
            this SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Guild:SetMaximumCurrentMaximumMemberCountByGuildName");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class SetMaximumCurrentMaximumMemberCountByGuildNameRequestExt
    {
        public static SetMaximumCurrentMaximumMemberCountByGuildNameRequest Rate(
            this SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Guild:SetMaximumCurrentMaximumMemberCountByGuildName");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}
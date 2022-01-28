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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Stamina.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Stamina : IComparable
	{
        public string StaminaId { set; get; }
        public string StaminaName { set; get; }
        public string UserId { set; get; }
        public int? Value { set; get; }
        public int? MaxValue { set; get; }
        public int? RecoverIntervalMinutes { set; get; }
        public int? RecoverValue { set; get; }
        public int? OverflowValue { set; get; }
        public long? NextRecoverAt { set; get; }
        public long? LastRecoveredAt { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Stamina WithStaminaId(string staminaId) {
            this.StaminaId = staminaId;
            return this;
        }

        public Stamina WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }

        public Stamina WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Stamina WithValue(int? value) {
            this.Value = value;
            return this;
        }

        public Stamina WithMaxValue(int? maxValue) {
            this.MaxValue = maxValue;
            return this;
        }

        public Stamina WithRecoverIntervalMinutes(int? recoverIntervalMinutes) {
            this.RecoverIntervalMinutes = recoverIntervalMinutes;
            return this;
        }

        public Stamina WithRecoverValue(int? recoverValue) {
            this.RecoverValue = recoverValue;
            return this;
        }

        public Stamina WithOverflowValue(int? overflowValue) {
            this.OverflowValue = overflowValue;
            return this;
        }

        public Stamina WithNextRecoverAt(long? nextRecoverAt) {
            this.NextRecoverAt = nextRecoverAt;
            return this;
        }

        public Stamina WithLastRecoveredAt(long? lastRecoveredAt) {
            this.LastRecoveredAt = lastRecoveredAt;
            return this;
        }

        public Stamina WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Stamina WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):user:(?<userId>.+):stamina:(?<staminaName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):user:(?<userId>.+):stamina:(?<staminaName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):user:(?<userId>.+):stamina:(?<staminaName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):user:(?<userId>.+):stamina:(?<staminaName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _staminaNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):user:(?<userId>.+):stamina:(?<staminaName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetStaminaNameFromGrn(
            string grn
        )
        {
            var match = _staminaNameRegex.Match(grn);
            if (!match.Success || !match.Groups["staminaName"].Success)
            {
                return null;
            }
            return match.Groups["staminaName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Stamina FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Stamina()
                .WithStaminaId(!data.Keys.Contains("staminaId") || data["staminaId"] == null ? null : data["staminaId"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (int?)int.Parse(data["value"].ToString()))
                .WithMaxValue(!data.Keys.Contains("maxValue") || data["maxValue"] == null ? null : (int?)int.Parse(data["maxValue"].ToString()))
                .WithRecoverIntervalMinutes(!data.Keys.Contains("recoverIntervalMinutes") || data["recoverIntervalMinutes"] == null ? null : (int?)int.Parse(data["recoverIntervalMinutes"].ToString()))
                .WithRecoverValue(!data.Keys.Contains("recoverValue") || data["recoverValue"] == null ? null : (int?)int.Parse(data["recoverValue"].ToString()))
                .WithOverflowValue(!data.Keys.Contains("overflowValue") || data["overflowValue"] == null ? null : (int?)int.Parse(data["overflowValue"].ToString()))
                .WithNextRecoverAt(!data.Keys.Contains("nextRecoverAt") || data["nextRecoverAt"] == null ? null : (long?)long.Parse(data["nextRecoverAt"].ToString()))
                .WithLastRecoveredAt(!data.Keys.Contains("lastRecoveredAt") || data["lastRecoveredAt"] == null ? null : (long?)long.Parse(data["lastRecoveredAt"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["staminaId"] = StaminaId,
                ["staminaName"] = StaminaName,
                ["userId"] = UserId,
                ["value"] = Value,
                ["maxValue"] = MaxValue,
                ["recoverIntervalMinutes"] = RecoverIntervalMinutes,
                ["recoverValue"] = RecoverValue,
                ["overflowValue"] = OverflowValue,
                ["nextRecoverAt"] = NextRecoverAt,
                ["lastRecoveredAt"] = LastRecoveredAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StaminaId != null) {
                writer.WritePropertyName("staminaId");
                writer.Write(StaminaId.ToString());
            }
            if (StaminaName != null) {
                writer.WritePropertyName("staminaName");
                writer.Write(StaminaName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(int.Parse(Value.ToString()));
            }
            if (MaxValue != null) {
                writer.WritePropertyName("maxValue");
                writer.Write(int.Parse(MaxValue.ToString()));
            }
            if (RecoverIntervalMinutes != null) {
                writer.WritePropertyName("recoverIntervalMinutes");
                writer.Write(int.Parse(RecoverIntervalMinutes.ToString()));
            }
            if (RecoverValue != null) {
                writer.WritePropertyName("recoverValue");
                writer.Write(int.Parse(RecoverValue.ToString()));
            }
            if (OverflowValue != null) {
                writer.WritePropertyName("overflowValue");
                writer.Write(int.Parse(OverflowValue.ToString()));
            }
            if (NextRecoverAt != null) {
                writer.WritePropertyName("nextRecoverAt");
                writer.Write(long.Parse(NextRecoverAt.ToString()));
            }
            if (LastRecoveredAt != null) {
                writer.WritePropertyName("lastRecoveredAt");
                writer.Write(long.Parse(LastRecoveredAt.ToString()));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Stamina;
            var diff = 0;
            if (StaminaId == null && StaminaId == other.StaminaId)
            {
                // null and null
            }
            else
            {
                diff += StaminaId.CompareTo(other.StaminaId);
            }
            if (StaminaName == null && StaminaName == other.StaminaName)
            {
                // null and null
            }
            else
            {
                diff += StaminaName.CompareTo(other.StaminaName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
            }
            if (MaxValue == null && MaxValue == other.MaxValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxValue - other.MaxValue);
            }
            if (RecoverIntervalMinutes == null && RecoverIntervalMinutes == other.RecoverIntervalMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(RecoverIntervalMinutes - other.RecoverIntervalMinutes);
            }
            if (RecoverValue == null && RecoverValue == other.RecoverValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(RecoverValue - other.RecoverValue);
            }
            if (OverflowValue == null && OverflowValue == other.OverflowValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(OverflowValue - other.OverflowValue);
            }
            if (NextRecoverAt == null && NextRecoverAt == other.NextRecoverAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(NextRecoverAt - other.NextRecoverAt);
            }
            if (LastRecoveredAt == null && LastRecoveredAt == other.LastRecoveredAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(LastRecoveredAt - other.LastRecoveredAt);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}
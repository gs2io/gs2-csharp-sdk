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
        public string StaminaId { set; get; } = null!;
        public string StaminaName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public int? Value { set; get; } = null!;
        public int? MaxValue { set; get; } = null!;
        public int? RecoverIntervalMinutes { set; get; } = null!;
        public int? RecoverValue { set; get; } = null!;
        public int? OverflowValue { set; get; } = null!;
        public long? NextRecoverAt { set; get; } = null!;
        public long? LastRecoveredAt { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public Stamina WithRevision(long? revision) {
            this.Revision = revision;
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
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (int?)(data["value"].ToString().Contains(".") ? (int)double.Parse(data["value"].ToString()) : int.Parse(data["value"].ToString())))
                .WithMaxValue(!data.Keys.Contains("maxValue") || data["maxValue"] == null ? null : (int?)(data["maxValue"].ToString().Contains(".") ? (int)double.Parse(data["maxValue"].ToString()) : int.Parse(data["maxValue"].ToString())))
                .WithRecoverIntervalMinutes(!data.Keys.Contains("recoverIntervalMinutes") || data["recoverIntervalMinutes"] == null ? null : (int?)(data["recoverIntervalMinutes"].ToString().Contains(".") ? (int)double.Parse(data["recoverIntervalMinutes"].ToString()) : int.Parse(data["recoverIntervalMinutes"].ToString())))
                .WithRecoverValue(!data.Keys.Contains("recoverValue") || data["recoverValue"] == null ? null : (int?)(data["recoverValue"].ToString().Contains(".") ? (int)double.Parse(data["recoverValue"].ToString()) : int.Parse(data["recoverValue"].ToString())))
                .WithOverflowValue(!data.Keys.Contains("overflowValue") || data["overflowValue"] == null ? null : (int?)(data["overflowValue"].ToString().Contains(".") ? (int)double.Parse(data["overflowValue"].ToString()) : int.Parse(data["overflowValue"].ToString())))
                .WithNextRecoverAt(!data.Keys.Contains("nextRecoverAt") || data["nextRecoverAt"] == null ? null : (long?)(data["nextRecoverAt"].ToString().Contains(".") ? (long)double.Parse(data["nextRecoverAt"].ToString()) : long.Parse(data["nextRecoverAt"].ToString())))
                .WithLastRecoveredAt(!data.Keys.Contains("lastRecoveredAt") || data["lastRecoveredAt"] == null ? null : (long?)(data["lastRecoveredAt"].ToString().Contains(".") ? (long)double.Parse(data["lastRecoveredAt"].ToString()) : long.Parse(data["lastRecoveredAt"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["revision"] = Revision,
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
                writer.Write((Value.ToString().Contains(".") ? (int)double.Parse(Value.ToString()) : int.Parse(Value.ToString())));
            }
            if (MaxValue != null) {
                writer.WritePropertyName("maxValue");
                writer.Write((MaxValue.ToString().Contains(".") ? (int)double.Parse(MaxValue.ToString()) : int.Parse(MaxValue.ToString())));
            }
            if (RecoverIntervalMinutes != null) {
                writer.WritePropertyName("recoverIntervalMinutes");
                writer.Write((RecoverIntervalMinutes.ToString().Contains(".") ? (int)double.Parse(RecoverIntervalMinutes.ToString()) : int.Parse(RecoverIntervalMinutes.ToString())));
            }
            if (RecoverValue != null) {
                writer.WritePropertyName("recoverValue");
                writer.Write((RecoverValue.ToString().Contains(".") ? (int)double.Parse(RecoverValue.ToString()) : int.Parse(RecoverValue.ToString())));
            }
            if (OverflowValue != null) {
                writer.WritePropertyName("overflowValue");
                writer.Write((OverflowValue.ToString().Contains(".") ? (int)double.Parse(OverflowValue.ToString()) : int.Parse(OverflowValue.ToString())));
            }
            if (NextRecoverAt != null) {
                writer.WritePropertyName("nextRecoverAt");
                writer.Write((NextRecoverAt.ToString().Contains(".") ? (long)double.Parse(NextRecoverAt.ToString()) : long.Parse(NextRecoverAt.ToString())));
            }
            if (LastRecoveredAt != null) {
                writer.WritePropertyName("lastRecoveredAt");
                writer.Write((LastRecoveredAt.ToString().Contains(".") ? (long)double.Parse(LastRecoveredAt.ToString()) : long.Parse(LastRecoveredAt.ToString())));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (StaminaId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.staminaId.error.tooLong"),
                    });
                }
            }
            {
                if (StaminaName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.staminaName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Value < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.value.error.invalid"),
                    });
                }
                if (Value > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.value.error.invalid"),
                    });
                }
            }
            {
                if (MaxValue < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.maxValue.error.invalid"),
                    });
                }
                if (MaxValue > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.maxValue.error.invalid"),
                    });
                }
            }
            {
                if (RecoverIntervalMinutes < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.recoverIntervalMinutes.error.invalid"),
                    });
                }
                if (RecoverIntervalMinutes > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.recoverIntervalMinutes.error.invalid"),
                    });
                }
            }
            {
                if (RecoverValue < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.recoverValue.error.invalid"),
                    });
                }
                if (RecoverValue > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.recoverValue.error.invalid"),
                    });
                }
            }
            {
                if (OverflowValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.overflowValue.error.invalid"),
                    });
                }
                if (OverflowValue > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.overflowValue.error.invalid"),
                    });
                }
            }
            {
                if (NextRecoverAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.nextRecoverAt.error.invalid"),
                    });
                }
                if (NextRecoverAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.nextRecoverAt.error.invalid"),
                    });
                }
            }
            {
                if (LastRecoveredAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.lastRecoveredAt.error.invalid"),
                    });
                }
                if (LastRecoveredAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.lastRecoveredAt.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stamina", "stamina.stamina.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Stamina {
                StaminaId = StaminaId,
                StaminaName = StaminaName,
                UserId = UserId,
                Value = Value,
                MaxValue = MaxValue,
                RecoverIntervalMinutes = RecoverIntervalMinutes,
                RecoverValue = RecoverValue,
                OverflowValue = OverflowValue,
                NextRecoverAt = NextRecoverAt,
                LastRecoveredAt = LastRecoveredAt,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
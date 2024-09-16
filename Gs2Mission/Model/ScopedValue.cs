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

namespace Gs2.Gs2Mission.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ScopedValue : IComparable
	{
        public string ScopeType { set; get; } = null!;
        public string ResetType { set; get; } = null!;
        public string ConditionName { set; get; } = null!;
        public long? Value { set; get; } = null!;
        public long? NextResetAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public ScopedValue WithScopeType(string scopeType) {
            this.ScopeType = scopeType;
            return this;
        }
        public ScopedValue WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }
        public ScopedValue WithConditionName(string conditionName) {
            this.ConditionName = conditionName;
            return this;
        }
        public ScopedValue WithValue(long? value) {
            this.Value = value;
            return this;
        }
        public ScopedValue WithNextResetAt(long? nextResetAt) {
            this.NextResetAt = nextResetAt;
            return this;
        }
        public ScopedValue WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ScopedValue FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ScopedValue()
                .WithScopeType(!data.Keys.Contains("scopeType") || data["scopeType"] == null ? null : data["scopeType"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithConditionName(!data.Keys.Contains("conditionName") || data["conditionName"] == null ? null : data["conditionName"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (long?)(data["value"].ToString().Contains(".") ? (long)double.Parse(data["value"].ToString()) : long.Parse(data["value"].ToString())))
                .WithNextResetAt(!data.Keys.Contains("nextResetAt") || data["nextResetAt"] == null ? null : (long?)(data["nextResetAt"].ToString().Contains(".") ? (long)double.Parse(data["nextResetAt"].ToString()) : long.Parse(data["nextResetAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["scopeType"] = ScopeType,
                ["resetType"] = ResetType,
                ["conditionName"] = ConditionName,
                ["value"] = Value,
                ["nextResetAt"] = NextResetAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ScopeType != null) {
                writer.WritePropertyName("scopeType");
                writer.Write(ScopeType.ToString());
            }
            if (ResetType != null) {
                writer.WritePropertyName("resetType");
                writer.Write(ResetType.ToString());
            }
            if (ConditionName != null) {
                writer.WritePropertyName("conditionName");
                writer.Write(ConditionName.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write((Value.ToString().Contains(".") ? (long)double.Parse(Value.ToString()) : long.Parse(Value.ToString())));
            }
            if (NextResetAt != null) {
                writer.WritePropertyName("nextResetAt");
                writer.Write((NextResetAt.ToString().Contains(".") ? (long)double.Parse(NextResetAt.ToString()) : long.Parse(NextResetAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ScopedValue;
            var diff = 0;
            if (ScopeType == null && ScopeType == other.ScopeType)
            {
                // null and null
            }
            else
            {
                diff += ScopeType.CompareTo(other.ScopeType);
            }
            if (ResetType == null && ResetType == other.ResetType)
            {
                // null and null
            }
            else
            {
                diff += ResetType.CompareTo(other.ResetType);
            }
            if (ConditionName == null && ConditionName == other.ConditionName)
            {
                // null and null
            }
            else
            {
                diff += ConditionName.CompareTo(other.ConditionName);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
            }
            if (NextResetAt == null && NextResetAt == other.NextResetAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(NextResetAt - other.NextResetAt);
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

        public void Validate() {
            {
                switch (ScopeType) {
                    case "resetTiming":
                    case "verifyAction":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("scopedValue", "mission.scopedValue.scopeType.error.invalid"),
                        });
                }
            }
            if (ScopeType == "resetTiming") {
                switch (ResetType) {
                    case "notReset":
                    case "daily":
                    case "weekly":
                    case "monthly":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("scopedValue", "mission.scopedValue.resetType.error.invalid"),
                        });
                }
            }
            if (ScopeType == "verifyAction") {
                if (ConditionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scopedValue", "mission.scopedValue.conditionName.error.tooLong"),
                    });
                }
            }
            {
                if (Value < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scopedValue", "mission.scopedValue.value.error.invalid"),
                    });
                }
                if (Value > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scopedValue", "mission.scopedValue.value.error.invalid"),
                    });
                }
            }
            {
                if (NextResetAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scopedValue", "mission.scopedValue.nextResetAt.error.invalid"),
                    });
                }
                if (NextResetAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scopedValue", "mission.scopedValue.nextResetAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scopedValue", "mission.scopedValue.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scopedValue", "mission.scopedValue.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ScopedValue {
                ScopeType = ScopeType,
                ResetType = ResetType,
                ConditionName = ConditionName,
                Value = Value,
                NextResetAt = NextResetAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}
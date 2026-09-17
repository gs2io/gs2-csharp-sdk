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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class ScopedValue : IComparable
	{
        public string ScopeType { set; get; }
        public string ResetType { set; get; }
        public string ConditionName { set; get; }
        public long? Value { set; get; }
        public long? NextResetAt { set; get; }
        public long? UpdatedAt { set; get; }
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
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["value"].ToString()))
                .WithNextResetAt(!data.Keys.Contains("nextResetAt") || data["nextResetAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["nextResetAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()));
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type ScopedValue.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(ScopeType, other.ScopeType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ResetType, other.ResetType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ConditionName, other.ConditionName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Value, other.Value);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(NextResetAt, other.NextResetAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
                    case "days":
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
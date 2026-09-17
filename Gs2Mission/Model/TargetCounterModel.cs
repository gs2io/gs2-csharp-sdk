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
	public partial class TargetCounterModel : IComparable
	{
        public string CounterName { set; get; }
        public string ScopeType { set; get; }
        public string ResetType { set; get; }
        public string ConditionName { set; get; }
        public long? Value { set; get; }
        public TargetCounterModel WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        public TargetCounterModel WithScopeType(string scopeType) {
            this.ScopeType = scopeType;
            return this;
        }
        public TargetCounterModel WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }
        public TargetCounterModel WithConditionName(string conditionName) {
            this.ConditionName = conditionName;
            return this;
        }
        public TargetCounterModel WithValue(long? value) {
            this.Value = value;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TargetCounterModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new TargetCounterModel()
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithScopeType(!data.Keys.Contains("scopeType") || data["scopeType"] == null ? null : data["scopeType"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithConditionName(!data.Keys.Contains("conditionName") || data["conditionName"] == null ? null : data["conditionName"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["value"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["counterName"] = CounterName,
                ["scopeType"] = ScopeType,
                ["resetType"] = ResetType,
                ["conditionName"] = ConditionName,
                ["value"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CounterName != null) {
                writer.WritePropertyName("counterName");
                writer.Write(CounterName.ToString());
            }
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TargetCounterModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type TargetCounterModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(CounterName, other.CounterName);
            if (diff != 0)
            {
                return diff;
            }
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
            return 0;
        }

        public void Validate() {
            {
                if (CounterName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("targetCounterModel", "mission.targetCounterModel.counterName.error.tooLong"),
                    });
                }
            }
            {
                switch (ScopeType) {
                    case "resetTiming":
                    case "verifyAction":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("targetCounterModel", "mission.targetCounterModel.scopeType.error.invalid"),
                        });
                }
            }
            {
                switch (ResetType) {
                    case "notReset":
                    case "daily":
                    case "weekly":
                    case "monthly":
                    case "days":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("targetCounterModel", "mission.targetCounterModel.resetType.error.invalid"),
                        });
                }
            }
            if (ScopeType == "verifyAction") {
                if (ConditionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("targetCounterModel", "mission.targetCounterModel.conditionName.error.tooLong"),
                    });
                }
            }
            {
                if (Value < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("targetCounterModel", "mission.targetCounterModel.value.error.invalid"),
                    });
                }
                if (Value > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("targetCounterModel", "mission.targetCounterModel.value.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new TargetCounterModel {
                CounterName = CounterName,
                ScopeType = ScopeType,
                ResetType = ResetType,
                ConditionName = ConditionName,
                Value = Value,
            };
        }
    }
}
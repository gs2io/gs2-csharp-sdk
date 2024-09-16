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
	public class TargetCounterModel : IComparable
	{
        public string CounterName { set; get; } = null!;
        public string ScopeType { set; get; } = null!;
        public string ResetType { set; get; } = null!;
        public string ConditionName { set; get; } = null!;
        public long? Value { set; get; } = null!;
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
            return new TargetCounterModel()
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithScopeType(!data.Keys.Contains("scopeType") || data["scopeType"] == null ? null : data["scopeType"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithConditionName(!data.Keys.Contains("conditionName") || data["conditionName"] == null ? null : data["conditionName"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (long?)(data["value"].ToString().Contains(".") ? (long)double.Parse(data["value"].ToString()) : long.Parse(data["value"].ToString())));
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
            var diff = 0;
            if (CounterName == null && CounterName == other.CounterName)
            {
                // null and null
            }
            else
            {
                diff += CounterName.CompareTo(other.CounterName);
            }
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
            return diff;
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
                if (Value < 1) {
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
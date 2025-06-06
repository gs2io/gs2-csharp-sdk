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
	public partial class CounterScopeModel : IComparable
	{
        public string ScopeType { set; get; }
        public string ResetType { set; get; }
        public int? ResetDayOfMonth { set; get; }
        public string ResetDayOfWeek { set; get; }
        public int? ResetHour { set; get; }
        public string ConditionName { set; get; }
        public Gs2.Core.Model.VerifyAction Condition { set; get; }
        public long? AnchorTimestamp { set; get; }
        public int? Days { set; get; }
        public CounterScopeModel WithScopeType(string scopeType) {
            this.ScopeType = scopeType;
            return this;
        }
        public CounterScopeModel WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }
        public CounterScopeModel WithResetDayOfMonth(int? resetDayOfMonth) {
            this.ResetDayOfMonth = resetDayOfMonth;
            return this;
        }
        public CounterScopeModel WithResetDayOfWeek(string resetDayOfWeek) {
            this.ResetDayOfWeek = resetDayOfWeek;
            return this;
        }
        public CounterScopeModel WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }
        public CounterScopeModel WithConditionName(string conditionName) {
            this.ConditionName = conditionName;
            return this;
        }
        public CounterScopeModel WithCondition(Gs2.Core.Model.VerifyAction condition) {
            this.Condition = condition;
            return this;
        }
        public CounterScopeModel WithAnchorTimestamp(long? anchorTimestamp) {
            this.AnchorTimestamp = anchorTimestamp;
            return this;
        }
        public CounterScopeModel WithDays(int? days) {
            this.Days = days;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CounterScopeModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CounterScopeModel()
                .WithScopeType(!data.Keys.Contains("scopeType") || data["scopeType"] == null ? null : data["scopeType"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithResetDayOfMonth(!data.Keys.Contains("resetDayOfMonth") || data["resetDayOfMonth"] == null ? null : (int?)(data["resetDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["resetDayOfMonth"].ToString()) : int.Parse(data["resetDayOfMonth"].ToString())))
                .WithResetDayOfWeek(!data.Keys.Contains("resetDayOfWeek") || data["resetDayOfWeek"] == null ? null : data["resetDayOfWeek"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)(data["resetHour"].ToString().Contains(".") ? (int)double.Parse(data["resetHour"].ToString()) : int.Parse(data["resetHour"].ToString())))
                .WithConditionName(!data.Keys.Contains("conditionName") || data["conditionName"] == null ? null : data["conditionName"].ToString())
                .WithCondition(!data.Keys.Contains("condition") || data["condition"] == null ? null : Gs2.Core.Model.VerifyAction.FromJson(data["condition"]))
                .WithAnchorTimestamp(!data.Keys.Contains("anchorTimestamp") || data["anchorTimestamp"] == null ? null : (long?)(data["anchorTimestamp"].ToString().Contains(".") ? (long)double.Parse(data["anchorTimestamp"].ToString()) : long.Parse(data["anchorTimestamp"].ToString())))
                .WithDays(!data.Keys.Contains("days") || data["days"] == null ? null : (int?)(data["days"].ToString().Contains(".") ? (int)double.Parse(data["days"].ToString()) : int.Parse(data["days"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["scopeType"] = ScopeType,
                ["resetType"] = ResetType,
                ["resetDayOfMonth"] = ResetDayOfMonth,
                ["resetDayOfWeek"] = ResetDayOfWeek,
                ["resetHour"] = ResetHour,
                ["conditionName"] = ConditionName,
                ["condition"] = Condition?.ToJson(),
                ["anchorTimestamp"] = AnchorTimestamp,
                ["days"] = Days,
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
            if (ResetDayOfMonth != null) {
                writer.WritePropertyName("resetDayOfMonth");
                writer.Write((ResetDayOfMonth.ToString().Contains(".") ? (int)double.Parse(ResetDayOfMonth.ToString()) : int.Parse(ResetDayOfMonth.ToString())));
            }
            if (ResetDayOfWeek != null) {
                writer.WritePropertyName("resetDayOfWeek");
                writer.Write(ResetDayOfWeek.ToString());
            }
            if (ResetHour != null) {
                writer.WritePropertyName("resetHour");
                writer.Write((ResetHour.ToString().Contains(".") ? (int)double.Parse(ResetHour.ToString()) : int.Parse(ResetHour.ToString())));
            }
            if (ConditionName != null) {
                writer.WritePropertyName("conditionName");
                writer.Write(ConditionName.ToString());
            }
            if (Condition != null) {
                writer.WritePropertyName("condition");
                Condition.WriteJson(writer);
            }
            if (AnchorTimestamp != null) {
                writer.WritePropertyName("anchorTimestamp");
                writer.Write((AnchorTimestamp.ToString().Contains(".") ? (long)double.Parse(AnchorTimestamp.ToString()) : long.Parse(AnchorTimestamp.ToString())));
            }
            if (Days != null) {
                writer.WritePropertyName("days");
                writer.Write((Days.ToString().Contains(".") ? (int)double.Parse(Days.ToString()) : int.Parse(Days.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CounterScopeModel;
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
            if (ResetDayOfMonth == null && ResetDayOfMonth == other.ResetDayOfMonth)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResetDayOfMonth - other.ResetDayOfMonth);
            }
            if (ResetDayOfWeek == null && ResetDayOfWeek == other.ResetDayOfWeek)
            {
                // null and null
            }
            else
            {
                diff += ResetDayOfWeek.CompareTo(other.ResetDayOfWeek);
            }
            if (ResetHour == null && ResetHour == other.ResetHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResetHour - other.ResetHour);
            }
            if (ConditionName == null && ConditionName == other.ConditionName)
            {
                // null and null
            }
            else
            {
                diff += ConditionName.CompareTo(other.ConditionName);
            }
            if (Condition == null && Condition == other.Condition)
            {
                // null and null
            }
            else
            {
                diff += Condition.CompareTo(other.Condition);
            }
            if (AnchorTimestamp == null && AnchorTimestamp == other.AnchorTimestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(AnchorTimestamp - other.AnchorTimestamp);
            }
            if (Days == null && Days == other.Days)
            {
                // null and null
            }
            else
            {
                diff += (int)(Days - other.Days);
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
                            new RequestError("counterScopeModel", "mission.counterScopeModel.scopeType.error.invalid"),
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
                            new RequestError("counterScopeModel", "mission.counterScopeModel.resetType.error.invalid"),
                        });
                }
            }
            if (ResetType == "monthly") {
                if (ResetDayOfMonth < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.resetDayOfMonth.error.invalid"),
                    });
                }
                if (ResetDayOfMonth > 31) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.resetDayOfMonth.error.invalid"),
                    });
                }
            }
            if (ResetType == "weekly") {
                switch (ResetDayOfWeek) {
                    case "sunday":
                    case "monday":
                    case "tuesday":
                    case "wednesday":
                    case "thursday":
                    case "friday":
                    case "saturday":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("counterScopeModel", "mission.counterScopeModel.resetDayOfWeek.error.invalid"),
                        });
                }
            }
            if ((ResetType =="monthly" || ResetType == "weekly" || ResetType == "daily")) {
                if (ResetHour < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.resetHour.error.invalid"),
                    });
                }
                if (ResetHour > 23) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.resetHour.error.invalid"),
                    });
                }
            }
            if (ScopeType == "verifyAction") {
                if (ConditionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.conditionName.error.tooLong"),
                    });
                }
            }
            if (ScopeType == "verifyAction") {
            }
            if (ResetType == "days") {
                if (AnchorTimestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.anchorTimestamp.error.invalid"),
                    });
                }
                if (AnchorTimestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.anchorTimestamp.error.invalid"),
                    });
                }
            }
            if (ResetType == "days") {
                if (Days < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.days.error.invalid"),
                    });
                }
                if (Days > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterScopeModel", "mission.counterScopeModel.days.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new CounterScopeModel {
                ScopeType = ScopeType,
                ResetType = ResetType,
                ResetDayOfMonth = ResetDayOfMonth,
                ResetDayOfWeek = ResetDayOfWeek,
                ResetHour = ResetHour,
                ConditionName = ConditionName,
                Condition = Condition?.Clone() as Gs2.Core.Model.VerifyAction,
                AnchorTimestamp = AnchorTimestamp,
                Days = Days,
            };
        }
    }
}
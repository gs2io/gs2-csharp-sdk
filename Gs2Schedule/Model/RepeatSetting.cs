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

namespace Gs2.Gs2Schedule.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RepeatSetting : IComparable
	{
        public string RepeatType { set; get; } = null!;
        public int? BeginDayOfMonth { set; get; } = null!;
        public int? EndDayOfMonth { set; get; } = null!;
        public string BeginDayOfWeek { set; get; } = null!;
        public string EndDayOfWeek { set; get; } = null!;
        public int? BeginHour { set; get; } = null!;
        public int? EndHour { set; get; } = null!;
        public long? AnchorTimestamp { set; get; } = null!;
        public int? ActiveDays { set; get; } = null!;
        public int? InactiveDays { set; get; } = null!;
        public RepeatSetting WithRepeatType(string repeatType) {
            this.RepeatType = repeatType;
            return this;
        }
        public RepeatSetting WithBeginDayOfMonth(int? beginDayOfMonth) {
            this.BeginDayOfMonth = beginDayOfMonth;
            return this;
        }
        public RepeatSetting WithEndDayOfMonth(int? endDayOfMonth) {
            this.EndDayOfMonth = endDayOfMonth;
            return this;
        }
        public RepeatSetting WithBeginDayOfWeek(string beginDayOfWeek) {
            this.BeginDayOfWeek = beginDayOfWeek;
            return this;
        }
        public RepeatSetting WithEndDayOfWeek(string endDayOfWeek) {
            this.EndDayOfWeek = endDayOfWeek;
            return this;
        }
        public RepeatSetting WithBeginHour(int? beginHour) {
            this.BeginHour = beginHour;
            return this;
        }
        public RepeatSetting WithEndHour(int? endHour) {
            this.EndHour = endHour;
            return this;
        }
        public RepeatSetting WithAnchorTimestamp(long? anchorTimestamp) {
            this.AnchorTimestamp = anchorTimestamp;
            return this;
        }
        public RepeatSetting WithActiveDays(int? activeDays) {
            this.ActiveDays = activeDays;
            return this;
        }
        public RepeatSetting WithInactiveDays(int? inactiveDays) {
            this.InactiveDays = inactiveDays;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RepeatSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RepeatSetting()
                .WithRepeatType(!data.Keys.Contains("repeatType") || data["repeatType"] == null ? null : data["repeatType"].ToString())
                .WithBeginDayOfMonth(!data.Keys.Contains("beginDayOfMonth") || data["beginDayOfMonth"] == null ? null : (int?)(data["beginDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["beginDayOfMonth"].ToString()) : int.Parse(data["beginDayOfMonth"].ToString())))
                .WithEndDayOfMonth(!data.Keys.Contains("endDayOfMonth") || data["endDayOfMonth"] == null ? null : (int?)(data["endDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["endDayOfMonth"].ToString()) : int.Parse(data["endDayOfMonth"].ToString())))
                .WithBeginDayOfWeek(!data.Keys.Contains("beginDayOfWeek") || data["beginDayOfWeek"] == null ? null : data["beginDayOfWeek"].ToString())
                .WithEndDayOfWeek(!data.Keys.Contains("endDayOfWeek") || data["endDayOfWeek"] == null ? null : data["endDayOfWeek"].ToString())
                .WithBeginHour(!data.Keys.Contains("beginHour") || data["beginHour"] == null ? null : (int?)(data["beginHour"].ToString().Contains(".") ? (int)double.Parse(data["beginHour"].ToString()) : int.Parse(data["beginHour"].ToString())))
                .WithEndHour(!data.Keys.Contains("endHour") || data["endHour"] == null ? null : (int?)(data["endHour"].ToString().Contains(".") ? (int)double.Parse(data["endHour"].ToString()) : int.Parse(data["endHour"].ToString())))
                .WithAnchorTimestamp(!data.Keys.Contains("anchorTimestamp") || data["anchorTimestamp"] == null ? null : (long?)(data["anchorTimestamp"].ToString().Contains(".") ? (long)double.Parse(data["anchorTimestamp"].ToString()) : long.Parse(data["anchorTimestamp"].ToString())))
                .WithActiveDays(!data.Keys.Contains("activeDays") || data["activeDays"] == null ? null : (int?)(data["activeDays"].ToString().Contains(".") ? (int)double.Parse(data["activeDays"].ToString()) : int.Parse(data["activeDays"].ToString())))
                .WithInactiveDays(!data.Keys.Contains("inactiveDays") || data["inactiveDays"] == null ? null : (int?)(data["inactiveDays"].ToString().Contains(".") ? (int)double.Parse(data["inactiveDays"].ToString()) : int.Parse(data["inactiveDays"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["repeatType"] = RepeatType,
                ["beginDayOfMonth"] = BeginDayOfMonth,
                ["endDayOfMonth"] = EndDayOfMonth,
                ["beginDayOfWeek"] = BeginDayOfWeek,
                ["endDayOfWeek"] = EndDayOfWeek,
                ["beginHour"] = BeginHour,
                ["endHour"] = EndHour,
                ["anchorTimestamp"] = AnchorTimestamp,
                ["activeDays"] = ActiveDays,
                ["inactiveDays"] = InactiveDays,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RepeatType != null) {
                writer.WritePropertyName("repeatType");
                writer.Write(RepeatType.ToString());
            }
            if (BeginDayOfMonth != null) {
                writer.WritePropertyName("beginDayOfMonth");
                writer.Write((BeginDayOfMonth.ToString().Contains(".") ? (int)double.Parse(BeginDayOfMonth.ToString()) : int.Parse(BeginDayOfMonth.ToString())));
            }
            if (EndDayOfMonth != null) {
                writer.WritePropertyName("endDayOfMonth");
                writer.Write((EndDayOfMonth.ToString().Contains(".") ? (int)double.Parse(EndDayOfMonth.ToString()) : int.Parse(EndDayOfMonth.ToString())));
            }
            if (BeginDayOfWeek != null) {
                writer.WritePropertyName("beginDayOfWeek");
                writer.Write(BeginDayOfWeek.ToString());
            }
            if (EndDayOfWeek != null) {
                writer.WritePropertyName("endDayOfWeek");
                writer.Write(EndDayOfWeek.ToString());
            }
            if (BeginHour != null) {
                writer.WritePropertyName("beginHour");
                writer.Write((BeginHour.ToString().Contains(".") ? (int)double.Parse(BeginHour.ToString()) : int.Parse(BeginHour.ToString())));
            }
            if (EndHour != null) {
                writer.WritePropertyName("endHour");
                writer.Write((EndHour.ToString().Contains(".") ? (int)double.Parse(EndHour.ToString()) : int.Parse(EndHour.ToString())));
            }
            if (AnchorTimestamp != null) {
                writer.WritePropertyName("anchorTimestamp");
                writer.Write((AnchorTimestamp.ToString().Contains(".") ? (long)double.Parse(AnchorTimestamp.ToString()) : long.Parse(AnchorTimestamp.ToString())));
            }
            if (ActiveDays != null) {
                writer.WritePropertyName("activeDays");
                writer.Write((ActiveDays.ToString().Contains(".") ? (int)double.Parse(ActiveDays.ToString()) : int.Parse(ActiveDays.ToString())));
            }
            if (InactiveDays != null) {
                writer.WritePropertyName("inactiveDays");
                writer.Write((InactiveDays.ToString().Contains(".") ? (int)double.Parse(InactiveDays.ToString()) : int.Parse(InactiveDays.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RepeatSetting;
            var diff = 0;
            if (RepeatType == null && RepeatType == other.RepeatType)
            {
                // null and null
            }
            else
            {
                diff += RepeatType.CompareTo(other.RepeatType);
            }
            if (BeginDayOfMonth == null && BeginDayOfMonth == other.BeginDayOfMonth)
            {
                // null and null
            }
            else
            {
                diff += (int)(BeginDayOfMonth - other.BeginDayOfMonth);
            }
            if (EndDayOfMonth == null && EndDayOfMonth == other.EndDayOfMonth)
            {
                // null and null
            }
            else
            {
                diff += (int)(EndDayOfMonth - other.EndDayOfMonth);
            }
            if (BeginDayOfWeek == null && BeginDayOfWeek == other.BeginDayOfWeek)
            {
                // null and null
            }
            else
            {
                diff += BeginDayOfWeek.CompareTo(other.BeginDayOfWeek);
            }
            if (EndDayOfWeek == null && EndDayOfWeek == other.EndDayOfWeek)
            {
                // null and null
            }
            else
            {
                diff += EndDayOfWeek.CompareTo(other.EndDayOfWeek);
            }
            if (BeginHour == null && BeginHour == other.BeginHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(BeginHour - other.BeginHour);
            }
            if (EndHour == null && EndHour == other.EndHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(EndHour - other.EndHour);
            }
            if (AnchorTimestamp == null && AnchorTimestamp == other.AnchorTimestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(AnchorTimestamp - other.AnchorTimestamp);
            }
            if (ActiveDays == null && ActiveDays == other.ActiveDays)
            {
                // null and null
            }
            else
            {
                diff += (int)(ActiveDays - other.ActiveDays);
            }
            if (InactiveDays == null && InactiveDays == other.InactiveDays)
            {
                // null and null
            }
            else
            {
                diff += (int)(InactiveDays - other.InactiveDays);
            }
            return diff;
        }

        public void Validate() {
            {
                switch (RepeatType) {
                    case "always":
                    case "daily":
                    case "weekly":
                    case "monthly":
                    case "custom":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("repeatSetting", "schedule.repeatSetting.repeatType.error.invalid"),
                        });
                }
            }
            if (RepeatType == "monthly") {
                if (BeginDayOfMonth < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.beginDayOfMonth.error.invalid"),
                    });
                }
                if (BeginDayOfMonth > 31) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.beginDayOfMonth.error.invalid"),
                    });
                }
            }
            if (RepeatType == "monthly") {
                if (EndDayOfMonth < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.endDayOfMonth.error.invalid"),
                    });
                }
                if (EndDayOfMonth > 31) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.endDayOfMonth.error.invalid"),
                    });
                }
            }
            if (RepeatType == "weekly") {
                switch (BeginDayOfWeek) {
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
                            new RequestError("repeatSetting", "schedule.repeatSetting.beginDayOfWeek.error.invalid"),
                        });
                }
            }
            if (RepeatType == "weekly") {
                switch (EndDayOfWeek) {
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
                            new RequestError("repeatSetting", "schedule.repeatSetting.endDayOfWeek.error.invalid"),
                        });
                }
            }
            if ((RepeatType =="daily" || RepeatType == "weekly" || RepeatType == "monthly")) {
                if (BeginHour < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.beginHour.error.invalid"),
                    });
                }
                if (BeginHour > 23) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.beginHour.error.invalid"),
                    });
                }
            }
            if ((RepeatType =="daily" || RepeatType == "weekly" || RepeatType == "monthly")) {
                if (EndHour < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.endHour.error.invalid"),
                    });
                }
                if (EndHour > 24) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.endHour.error.invalid"),
                    });
                }
            }
            if (RepeatType == "custom") {
                if (AnchorTimestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.anchorTimestamp.error.invalid"),
                    });
                }
                if (AnchorTimestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.anchorTimestamp.error.invalid"),
                    });
                }
            }
            if (RepeatType == "custom") {
                if (ActiveDays < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.activeDays.error.invalid"),
                    });
                }
                if (ActiveDays > 2147483646.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.activeDays.error.invalid"),
                    });
                }
            }
            if (RepeatType == "custom") {
                if (InactiveDays < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.inactiveDays.error.invalid"),
                    });
                }
                if (InactiveDays > 2147483646.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSetting", "schedule.repeatSetting.inactiveDays.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RepeatSetting {
                RepeatType = RepeatType,
                BeginDayOfMonth = BeginDayOfMonth,
                EndDayOfMonth = EndDayOfMonth,
                BeginDayOfWeek = BeginDayOfWeek,
                EndDayOfWeek = EndDayOfWeek,
                BeginHour = BeginHour,
                EndHour = EndHour,
                AnchorTimestamp = AnchorTimestamp,
                ActiveDays = ActiveDays,
                InactiveDays = InactiveDays,
            };
        }
    }
}
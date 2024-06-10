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
	public class Event : IComparable
	{
        public string EventId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string ScheduleType { set; get; } = null!;
        public long? AbsoluteBegin { set; get; } = null!;
        public long? AbsoluteEnd { set; get; } = null!;
        public string RelativeTriggerName { set; get; } = null!;
        public Gs2.Gs2Schedule.Model.RepeatSetting RepeatSetting { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public string RepeatType { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public int? RepeatBeginDayOfMonth { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public int? RepeatEndDayOfMonth { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public string RepeatBeginDayOfWeek { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public string RepeatEndDayOfWeek { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public int? RepeatBeginHour { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public int? RepeatEndHour { set; get; } = null!;
        public Event WithEventId(string eventId) {
            this.EventId = eventId;
            return this;
        }
        public Event WithName(string name) {
            this.Name = name;
            return this;
        }
        public Event WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public Event WithScheduleType(string scheduleType) {
            this.ScheduleType = scheduleType;
            return this;
        }
        public Event WithAbsoluteBegin(long? absoluteBegin) {
            this.AbsoluteBegin = absoluteBegin;
            return this;
        }
        public Event WithAbsoluteEnd(long? absoluteEnd) {
            this.AbsoluteEnd = absoluteEnd;
            return this;
        }
        public Event WithRelativeTriggerName(string relativeTriggerName) {
            this.RelativeTriggerName = relativeTriggerName;
            return this;
        }
        public Event WithRepeatSetting(Gs2.Gs2Schedule.Model.RepeatSetting repeatSetting) {
            this.RepeatSetting = repeatSetting;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Event WithRepeatType(string repeatType) {
            this.RepeatType = repeatType;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Event WithRepeatBeginDayOfMonth(int? repeatBeginDayOfMonth) {
            this.RepeatBeginDayOfMonth = repeatBeginDayOfMonth;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Event WithRepeatEndDayOfMonth(int? repeatEndDayOfMonth) {
            this.RepeatEndDayOfMonth = repeatEndDayOfMonth;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Event WithRepeatBeginDayOfWeek(string repeatBeginDayOfWeek) {
            this.RepeatBeginDayOfWeek = repeatBeginDayOfWeek;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Event WithRepeatEndDayOfWeek(string repeatEndDayOfWeek) {
            this.RepeatEndDayOfWeek = repeatEndDayOfWeek;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Event WithRepeatBeginHour(int? repeatBeginHour) {
            this.RepeatBeginHour = repeatBeginHour;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Event WithRepeatEndHour(int? repeatEndHour) {
            this.RepeatEndHour = repeatEndHour;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):schedule:(?<namespaceName>.+):event:(?<eventName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):schedule:(?<namespaceName>.+):event:(?<eventName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):schedule:(?<namespaceName>.+):event:(?<eventName>.+)",
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

        private static System.Text.RegularExpressions.Regex _eventNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):schedule:(?<namespaceName>.+):event:(?<eventName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetEventNameFromGrn(
            string grn
        )
        {
            var match = _eventNameRegex.Match(grn);
            if (!match.Success || !match.Groups["eventName"].Success)
            {
                return null;
            }
            return match.Groups["eventName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Event FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Event()
                .WithEventId(!data.Keys.Contains("eventId") || data["eventId"] == null ? null : data["eventId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScheduleType(!data.Keys.Contains("scheduleType") || data["scheduleType"] == null ? null : data["scheduleType"].ToString())
                .WithAbsoluteBegin(!data.Keys.Contains("absoluteBegin") || data["absoluteBegin"] == null ? null : (long?)(data["absoluteBegin"].ToString().Contains(".") ? (long)double.Parse(data["absoluteBegin"].ToString()) : long.Parse(data["absoluteBegin"].ToString())))
                .WithAbsoluteEnd(!data.Keys.Contains("absoluteEnd") || data["absoluteEnd"] == null ? null : (long?)(data["absoluteEnd"].ToString().Contains(".") ? (long)double.Parse(data["absoluteEnd"].ToString()) : long.Parse(data["absoluteEnd"].ToString())))
                .WithRelativeTriggerName(!data.Keys.Contains("relativeTriggerName") || data["relativeTriggerName"] == null ? null : data["relativeTriggerName"].ToString())
                .WithRepeatSetting(!data.Keys.Contains("repeatSetting") || data["repeatSetting"] == null ? null : Gs2.Gs2Schedule.Model.RepeatSetting.FromJson(data["repeatSetting"]))
                .WithRepeatType(!data.Keys.Contains("repeatType") || data["repeatType"] == null ? null : data["repeatType"].ToString())
                .WithRepeatBeginDayOfMonth(!data.Keys.Contains("repeatBeginDayOfMonth") || data["repeatBeginDayOfMonth"] == null ? null : (int?)(data["repeatBeginDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["repeatBeginDayOfMonth"].ToString()) : int.Parse(data["repeatBeginDayOfMonth"].ToString())))
                .WithRepeatEndDayOfMonth(!data.Keys.Contains("repeatEndDayOfMonth") || data["repeatEndDayOfMonth"] == null ? null : (int?)(data["repeatEndDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["repeatEndDayOfMonth"].ToString()) : int.Parse(data["repeatEndDayOfMonth"].ToString())))
                .WithRepeatBeginDayOfWeek(!data.Keys.Contains("repeatBeginDayOfWeek") || data["repeatBeginDayOfWeek"] == null ? null : data["repeatBeginDayOfWeek"].ToString())
                .WithRepeatEndDayOfWeek(!data.Keys.Contains("repeatEndDayOfWeek") || data["repeatEndDayOfWeek"] == null ? null : data["repeatEndDayOfWeek"].ToString())
                .WithRepeatBeginHour(!data.Keys.Contains("repeatBeginHour") || data["repeatBeginHour"] == null ? null : (int?)(data["repeatBeginHour"].ToString().Contains(".") ? (int)double.Parse(data["repeatBeginHour"].ToString()) : int.Parse(data["repeatBeginHour"].ToString())))
                .WithRepeatEndHour(!data.Keys.Contains("repeatEndHour") || data["repeatEndHour"] == null ? null : (int?)(data["repeatEndHour"].ToString().Contains(".") ? (int)double.Parse(data["repeatEndHour"].ToString()) : int.Parse(data["repeatEndHour"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["eventId"] = EventId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["scheduleType"] = ScheduleType,
                ["absoluteBegin"] = AbsoluteBegin,
                ["absoluteEnd"] = AbsoluteEnd,
                ["relativeTriggerName"] = RelativeTriggerName,
                ["repeatSetting"] = RepeatSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (EventId != null) {
                writer.WritePropertyName("eventId");
                writer.Write(EventId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ScheduleType != null) {
                writer.WritePropertyName("scheduleType");
                writer.Write(ScheduleType.ToString());
            }
            if (AbsoluteBegin != null) {
                writer.WritePropertyName("absoluteBegin");
                writer.Write((AbsoluteBegin.ToString().Contains(".") ? (long)double.Parse(AbsoluteBegin.ToString()) : long.Parse(AbsoluteBegin.ToString())));
            }
            if (AbsoluteEnd != null) {
                writer.WritePropertyName("absoluteEnd");
                writer.Write((AbsoluteEnd.ToString().Contains(".") ? (long)double.Parse(AbsoluteEnd.ToString()) : long.Parse(AbsoluteEnd.ToString())));
            }
            if (RelativeTriggerName != null) {
                writer.WritePropertyName("relativeTriggerName");
                writer.Write(RelativeTriggerName.ToString());
            }
            if (RepeatSetting != null) {
                writer.WritePropertyName("repeatSetting");
                RepeatSetting.WriteJson(writer);
            }
            if (RepeatType != null) {
                writer.WritePropertyName("repeatType");
                writer.Write(RepeatType.ToString());
            }
            if (RepeatBeginDayOfMonth != null) {
                writer.WritePropertyName("repeatBeginDayOfMonth");
                writer.Write((RepeatBeginDayOfMonth.ToString().Contains(".") ? (int)double.Parse(RepeatBeginDayOfMonth.ToString()) : int.Parse(RepeatBeginDayOfMonth.ToString())));
            }
            if (RepeatEndDayOfMonth != null) {
                writer.WritePropertyName("repeatEndDayOfMonth");
                writer.Write((RepeatEndDayOfMonth.ToString().Contains(".") ? (int)double.Parse(RepeatEndDayOfMonth.ToString()) : int.Parse(RepeatEndDayOfMonth.ToString())));
            }
            if (RepeatBeginDayOfWeek != null) {
                writer.WritePropertyName("repeatBeginDayOfWeek");
                writer.Write(RepeatBeginDayOfWeek.ToString());
            }
            if (RepeatEndDayOfWeek != null) {
                writer.WritePropertyName("repeatEndDayOfWeek");
                writer.Write(RepeatEndDayOfWeek.ToString());
            }
            if (RepeatBeginHour != null) {
                writer.WritePropertyName("repeatBeginHour");
                writer.Write((RepeatBeginHour.ToString().Contains(".") ? (int)double.Parse(RepeatBeginHour.ToString()) : int.Parse(RepeatBeginHour.ToString())));
            }
            if (RepeatEndHour != null) {
                writer.WritePropertyName("repeatEndHour");
                writer.Write((RepeatEndHour.ToString().Contains(".") ? (int)double.Parse(RepeatEndHour.ToString()) : int.Parse(RepeatEndHour.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Event;
            var diff = 0;
            if (EventId == null && EventId == other.EventId)
            {
                // null and null
            }
            else
            {
                diff += EventId.CompareTo(other.EventId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (ScheduleType == null && ScheduleType == other.ScheduleType)
            {
                // null and null
            }
            else
            {
                diff += ScheduleType.CompareTo(other.ScheduleType);
            }
            if (AbsoluteBegin == null && AbsoluteBegin == other.AbsoluteBegin)
            {
                // null and null
            }
            else
            {
                diff += (int)(AbsoluteBegin - other.AbsoluteBegin);
            }
            if (AbsoluteEnd == null && AbsoluteEnd == other.AbsoluteEnd)
            {
                // null and null
            }
            else
            {
                diff += (int)(AbsoluteEnd - other.AbsoluteEnd);
            }
            if (RelativeTriggerName == null && RelativeTriggerName == other.RelativeTriggerName)
            {
                // null and null
            }
            else
            {
                diff += RelativeTriggerName.CompareTo(other.RelativeTriggerName);
            }
            if (RepeatSetting == null && RepeatSetting == other.RepeatSetting)
            {
                // null and null
            }
            else
            {
                diff += RepeatSetting.CompareTo(other.RepeatSetting);
            }
            if (RepeatType == null && RepeatType == other.RepeatType)
            {
                // null and null
            }
            else
            {
                diff += RepeatType.CompareTo(other.RepeatType);
            }
            if (RepeatBeginDayOfMonth == null && RepeatBeginDayOfMonth == other.RepeatBeginDayOfMonth)
            {
                // null and null
            }
            else
            {
                diff += (int)(RepeatBeginDayOfMonth - other.RepeatBeginDayOfMonth);
            }
            if (RepeatEndDayOfMonth == null && RepeatEndDayOfMonth == other.RepeatEndDayOfMonth)
            {
                // null and null
            }
            else
            {
                diff += (int)(RepeatEndDayOfMonth - other.RepeatEndDayOfMonth);
            }
            if (RepeatBeginDayOfWeek == null && RepeatBeginDayOfWeek == other.RepeatBeginDayOfWeek)
            {
                // null and null
            }
            else
            {
                diff += RepeatBeginDayOfWeek.CompareTo(other.RepeatBeginDayOfWeek);
            }
            if (RepeatEndDayOfWeek == null && RepeatEndDayOfWeek == other.RepeatEndDayOfWeek)
            {
                // null and null
            }
            else
            {
                diff += RepeatEndDayOfWeek.CompareTo(other.RepeatEndDayOfWeek);
            }
            if (RepeatBeginHour == null && RepeatBeginHour == other.RepeatBeginHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(RepeatBeginHour - other.RepeatBeginHour);
            }
            if (RepeatEndHour == null && RepeatEndHour == other.RepeatEndHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(RepeatEndHour - other.RepeatEndHour);
            }
            return diff;
        }

        public void Validate() {
            {
                if (EventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "schedule.event.eventId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "schedule.event.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "schedule.event.metadata.error.tooLong"),
                    });
                }
            }
            {
                switch (ScheduleType) {
                    case "absolute":
                    case "relative":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("event", "schedule.event.scheduleType.error.invalid"),
                        });
                }
            }
            {
                if (AbsoluteBegin < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "schedule.event.absoluteBegin.error.invalid"),
                    });
                }
                if (AbsoluteBegin > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "schedule.event.absoluteBegin.error.invalid"),
                    });
                }
            }
            {
                if (AbsoluteEnd < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "schedule.event.absoluteEnd.error.invalid"),
                    });
                }
                if (AbsoluteEnd > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "schedule.event.absoluteEnd.error.invalid"),
                    });
                }
            }
            if (ScheduleType == "relative") {
                if (RelativeTriggerName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "schedule.event.relativeTriggerName.error.tooLong"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new Event {
                EventId = EventId,
                Name = Name,
                Metadata = Metadata,
                ScheduleType = ScheduleType,
                AbsoluteBegin = AbsoluteBegin,
                AbsoluteEnd = AbsoluteEnd,
                RelativeTriggerName = RelativeTriggerName,
                RepeatSetting = RepeatSetting.Clone() as Gs2.Gs2Schedule.Model.RepeatSetting,
                RepeatType = RepeatType,
                RepeatBeginDayOfMonth = RepeatBeginDayOfMonth,
                RepeatEndDayOfMonth = RepeatEndDayOfMonth,
                RepeatBeginDayOfWeek = RepeatBeginDayOfWeek,
                RepeatEndDayOfWeek = RepeatEndDayOfWeek,
                RepeatBeginHour = RepeatBeginHour,
                RepeatEndHour = RepeatEndHour,
            };
        }
    }
}
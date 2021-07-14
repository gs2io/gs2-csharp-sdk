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
using UnityEngine.Scripting;

namespace Gs2.Gs2Schedule.Model
{

	[Preserve]
	public class Event : IComparable
	{
        public string EventId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string ScheduleType { set; get; }
        public string RepeatType { set; get; }
        public long? AbsoluteBegin { set; get; }
        public long? AbsoluteEnd { set; get; }
        public int? RepeatBeginDayOfMonth { set; get; }
        public int? RepeatEndDayOfMonth { set; get; }
        public string RepeatBeginDayOfWeek { set; get; }
        public string RepeatEndDayOfWeek { set; get; }
        public int? RepeatBeginHour { set; get; }
        public int? RepeatEndHour { set; get; }
        public string RelativeTriggerName { set; get; }
        public int? RelativeDuration { set; get; }

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

        public Event WithRepeatType(string repeatType) {
            this.RepeatType = repeatType;
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

        public Event WithRepeatBeginDayOfMonth(int? repeatBeginDayOfMonth) {
            this.RepeatBeginDayOfMonth = repeatBeginDayOfMonth;
            return this;
        }

        public Event WithRepeatEndDayOfMonth(int? repeatEndDayOfMonth) {
            this.RepeatEndDayOfMonth = repeatEndDayOfMonth;
            return this;
        }

        public Event WithRepeatBeginDayOfWeek(string repeatBeginDayOfWeek) {
            this.RepeatBeginDayOfWeek = repeatBeginDayOfWeek;
            return this;
        }

        public Event WithRepeatEndDayOfWeek(string repeatEndDayOfWeek) {
            this.RepeatEndDayOfWeek = repeatEndDayOfWeek;
            return this;
        }

        public Event WithRepeatBeginHour(int? repeatBeginHour) {
            this.RepeatBeginHour = repeatBeginHour;
            return this;
        }

        public Event WithRepeatEndHour(int? repeatEndHour) {
            this.RepeatEndHour = repeatEndHour;
            return this;
        }

        public Event WithRelativeTriggerName(string relativeTriggerName) {
            this.RelativeTriggerName = relativeTriggerName;
            return this;
        }

        public Event WithRelativeDuration(int? relativeDuration) {
            this.RelativeDuration = relativeDuration;
            return this;
        }

    	[Preserve]
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
                .WithRepeatType(!data.Keys.Contains("repeatType") || data["repeatType"] == null ? null : data["repeatType"].ToString())
                .WithAbsoluteBegin(!data.Keys.Contains("absoluteBegin") || data["absoluteBegin"] == null ? null : (long?)long.Parse(data["absoluteBegin"].ToString()))
                .WithAbsoluteEnd(!data.Keys.Contains("absoluteEnd") || data["absoluteEnd"] == null ? null : (long?)long.Parse(data["absoluteEnd"].ToString()))
                .WithRepeatBeginDayOfMonth(!data.Keys.Contains("repeatBeginDayOfMonth") || data["repeatBeginDayOfMonth"] == null ? null : (int?)int.Parse(data["repeatBeginDayOfMonth"].ToString()))
                .WithRepeatEndDayOfMonth(!data.Keys.Contains("repeatEndDayOfMonth") || data["repeatEndDayOfMonth"] == null ? null : (int?)int.Parse(data["repeatEndDayOfMonth"].ToString()))
                .WithRepeatBeginDayOfWeek(!data.Keys.Contains("repeatBeginDayOfWeek") || data["repeatBeginDayOfWeek"] == null ? null : data["repeatBeginDayOfWeek"].ToString())
                .WithRepeatEndDayOfWeek(!data.Keys.Contains("repeatEndDayOfWeek") || data["repeatEndDayOfWeek"] == null ? null : data["repeatEndDayOfWeek"].ToString())
                .WithRepeatBeginHour(!data.Keys.Contains("repeatBeginHour") || data["repeatBeginHour"] == null ? null : (int?)int.Parse(data["repeatBeginHour"].ToString()))
                .WithRepeatEndHour(!data.Keys.Contains("repeatEndHour") || data["repeatEndHour"] == null ? null : (int?)int.Parse(data["repeatEndHour"].ToString()))
                .WithRelativeTriggerName(!data.Keys.Contains("relativeTriggerName") || data["relativeTriggerName"] == null ? null : data["relativeTriggerName"].ToString())
                .WithRelativeDuration(!data.Keys.Contains("relativeDuration") || data["relativeDuration"] == null ? null : (int?)int.Parse(data["relativeDuration"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["eventId"] = EventId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["scheduleType"] = ScheduleType,
                ["repeatType"] = RepeatType,
                ["absoluteBegin"] = AbsoluteBegin,
                ["absoluteEnd"] = AbsoluteEnd,
                ["repeatBeginDayOfMonth"] = RepeatBeginDayOfMonth,
                ["repeatEndDayOfMonth"] = RepeatEndDayOfMonth,
                ["repeatBeginDayOfWeek"] = RepeatBeginDayOfWeek,
                ["repeatEndDayOfWeek"] = RepeatEndDayOfWeek,
                ["repeatBeginHour"] = RepeatBeginHour,
                ["repeatEndHour"] = RepeatEndHour,
                ["relativeTriggerName"] = RelativeTriggerName,
                ["relativeDuration"] = RelativeDuration,
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
            if (RepeatType != null) {
                writer.WritePropertyName("repeatType");
                writer.Write(RepeatType.ToString());
            }
            if (AbsoluteBegin != null) {
                writer.WritePropertyName("absoluteBegin");
                writer.Write(long.Parse(AbsoluteBegin.ToString()));
            }
            if (AbsoluteEnd != null) {
                writer.WritePropertyName("absoluteEnd");
                writer.Write(long.Parse(AbsoluteEnd.ToString()));
            }
            if (RepeatBeginDayOfMonth != null) {
                writer.WritePropertyName("repeatBeginDayOfMonth");
                writer.Write(int.Parse(RepeatBeginDayOfMonth.ToString()));
            }
            if (RepeatEndDayOfMonth != null) {
                writer.WritePropertyName("repeatEndDayOfMonth");
                writer.Write(int.Parse(RepeatEndDayOfMonth.ToString()));
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
                writer.Write(int.Parse(RepeatBeginHour.ToString()));
            }
            if (RepeatEndHour != null) {
                writer.WritePropertyName("repeatEndHour");
                writer.Write(int.Parse(RepeatEndHour.ToString()));
            }
            if (RelativeTriggerName != null) {
                writer.WritePropertyName("relativeTriggerName");
                writer.Write(RelativeTriggerName.ToString());
            }
            if (RelativeDuration != null) {
                writer.WritePropertyName("relativeDuration");
                writer.Write(int.Parse(RelativeDuration.ToString()));
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
            if (RepeatType == null && RepeatType == other.RepeatType)
            {
                // null and null
            }
            else
            {
                diff += RepeatType.CompareTo(other.RepeatType);
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
            if (RelativeTriggerName == null && RelativeTriggerName == other.RelativeTriggerName)
            {
                // null and null
            }
            else
            {
                diff += RelativeTriggerName.CompareTo(other.RelativeTriggerName);
            }
            if (RelativeDuration == null && RelativeDuration == other.RelativeDuration)
            {
                // null and null
            }
            else
            {
                diff += (int)(RelativeDuration - other.RelativeDuration);
            }
            return diff;
        }
    }
}
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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Schedule.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Schedule.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateEventMasterRequest : Gs2Request<UpdateEventMasterRequest>
	{
         public string NamespaceName { set; get; }
         public string EventName { set; get; }
         public string Description { set; get; }
         public string Metadata { set; get; }
         public string ScheduleType { set; get; }
         public long? AbsoluteBegin { set; get; }
         public long? AbsoluteEnd { set; get; }
         public string RepeatType { set; get; }
         public int? RepeatBeginDayOfMonth { set; get; }
         public int? RepeatEndDayOfMonth { set; get; }
         public string RepeatBeginDayOfWeek { set; get; }
         public string RepeatEndDayOfWeek { set; get; }
         public int? RepeatBeginHour { set; get; }
         public int? RepeatEndHour { set; get; }
         public string RelativeTriggerName { set; get; }
        public UpdateEventMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateEventMasterRequest WithEventName(string eventName) {
            this.EventName = eventName;
            return this;
        }
        public UpdateEventMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateEventMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateEventMasterRequest WithScheduleType(string scheduleType) {
            this.ScheduleType = scheduleType;
            return this;
        }
        public UpdateEventMasterRequest WithAbsoluteBegin(long? absoluteBegin) {
            this.AbsoluteBegin = absoluteBegin;
            return this;
        }
        public UpdateEventMasterRequest WithAbsoluteEnd(long? absoluteEnd) {
            this.AbsoluteEnd = absoluteEnd;
            return this;
        }
        public UpdateEventMasterRequest WithRepeatType(string repeatType) {
            this.RepeatType = repeatType;
            return this;
        }
        public UpdateEventMasterRequest WithRepeatBeginDayOfMonth(int? repeatBeginDayOfMonth) {
            this.RepeatBeginDayOfMonth = repeatBeginDayOfMonth;
            return this;
        }
        public UpdateEventMasterRequest WithRepeatEndDayOfMonth(int? repeatEndDayOfMonth) {
            this.RepeatEndDayOfMonth = repeatEndDayOfMonth;
            return this;
        }
        public UpdateEventMasterRequest WithRepeatBeginDayOfWeek(string repeatBeginDayOfWeek) {
            this.RepeatBeginDayOfWeek = repeatBeginDayOfWeek;
            return this;
        }
        public UpdateEventMasterRequest WithRepeatEndDayOfWeek(string repeatEndDayOfWeek) {
            this.RepeatEndDayOfWeek = repeatEndDayOfWeek;
            return this;
        }
        public UpdateEventMasterRequest WithRepeatBeginHour(int? repeatBeginHour) {
            this.RepeatBeginHour = repeatBeginHour;
            return this;
        }
        public UpdateEventMasterRequest WithRepeatEndHour(int? repeatEndHour) {
            this.RepeatEndHour = repeatEndHour;
            return this;
        }
        public UpdateEventMasterRequest WithRelativeTriggerName(string relativeTriggerName) {
            this.RelativeTriggerName = relativeTriggerName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateEventMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateEventMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithEventName(!data.Keys.Contains("eventName") || data["eventName"] == null ? null : data["eventName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScheduleType(!data.Keys.Contains("scheduleType") || data["scheduleType"] == null ? null : data["scheduleType"].ToString())
                .WithAbsoluteBegin(!data.Keys.Contains("absoluteBegin") || data["absoluteBegin"] == null ? null : (long?)(data["absoluteBegin"].ToString().Contains(".") ? (long)double.Parse(data["absoluteBegin"].ToString()) : long.Parse(data["absoluteBegin"].ToString())))
                .WithAbsoluteEnd(!data.Keys.Contains("absoluteEnd") || data["absoluteEnd"] == null ? null : (long?)(data["absoluteEnd"].ToString().Contains(".") ? (long)double.Parse(data["absoluteEnd"].ToString()) : long.Parse(data["absoluteEnd"].ToString())))
                .WithRepeatType(!data.Keys.Contains("repeatType") || data["repeatType"] == null ? null : data["repeatType"].ToString())
                .WithRepeatBeginDayOfMonth(!data.Keys.Contains("repeatBeginDayOfMonth") || data["repeatBeginDayOfMonth"] == null ? null : (int?)(data["repeatBeginDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["repeatBeginDayOfMonth"].ToString()) : int.Parse(data["repeatBeginDayOfMonth"].ToString())))
                .WithRepeatEndDayOfMonth(!data.Keys.Contains("repeatEndDayOfMonth") || data["repeatEndDayOfMonth"] == null ? null : (int?)(data["repeatEndDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["repeatEndDayOfMonth"].ToString()) : int.Parse(data["repeatEndDayOfMonth"].ToString())))
                .WithRepeatBeginDayOfWeek(!data.Keys.Contains("repeatBeginDayOfWeek") || data["repeatBeginDayOfWeek"] == null ? null : data["repeatBeginDayOfWeek"].ToString())
                .WithRepeatEndDayOfWeek(!data.Keys.Contains("repeatEndDayOfWeek") || data["repeatEndDayOfWeek"] == null ? null : data["repeatEndDayOfWeek"].ToString())
                .WithRepeatBeginHour(!data.Keys.Contains("repeatBeginHour") || data["repeatBeginHour"] == null ? null : (int?)(data["repeatBeginHour"].ToString().Contains(".") ? (int)double.Parse(data["repeatBeginHour"].ToString()) : int.Parse(data["repeatBeginHour"].ToString())))
                .WithRepeatEndHour(!data.Keys.Contains("repeatEndHour") || data["repeatEndHour"] == null ? null : (int?)(data["repeatEndHour"].ToString().Contains(".") ? (int)double.Parse(data["repeatEndHour"].ToString()) : int.Parse(data["repeatEndHour"].ToString())))
                .WithRelativeTriggerName(!data.Keys.Contains("relativeTriggerName") || data["relativeTriggerName"] == null ? null : data["relativeTriggerName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["eventName"] = EventName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["scheduleType"] = ScheduleType,
                ["absoluteBegin"] = AbsoluteBegin,
                ["absoluteEnd"] = AbsoluteEnd,
                ["repeatType"] = RepeatType,
                ["repeatBeginDayOfMonth"] = RepeatBeginDayOfMonth,
                ["repeatEndDayOfMonth"] = RepeatEndDayOfMonth,
                ["repeatBeginDayOfWeek"] = RepeatBeginDayOfWeek,
                ["repeatEndDayOfWeek"] = RepeatEndDayOfWeek,
                ["repeatBeginHour"] = RepeatBeginHour,
                ["repeatEndHour"] = RepeatEndHour,
                ["relativeTriggerName"] = RelativeTriggerName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (EventName != null) {
                writer.WritePropertyName("eventName");
                writer.Write(EventName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
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
            if (RelativeTriggerName != null) {
                writer.WritePropertyName("relativeTriggerName");
                writer.Write(RelativeTriggerName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += EventName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ScheduleType + ":";
            key += AbsoluteBegin + ":";
            key += AbsoluteEnd + ":";
            key += RepeatType + ":";
            key += RepeatBeginDayOfMonth + ":";
            key += RepeatEndDayOfMonth + ":";
            key += RepeatBeginDayOfWeek + ":";
            key += RepeatEndDayOfWeek + ":";
            key += RepeatBeginHour + ":";
            key += RepeatEndHour + ":";
            key += RelativeTriggerName + ":";
            return key;
        }
    }
}
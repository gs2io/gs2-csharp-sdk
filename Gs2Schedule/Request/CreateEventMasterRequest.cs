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
	public class CreateEventMasterRequest : Gs2Request<CreateEventMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string ScheduleType { set; get; } = null!;
         public long? AbsoluteBegin { set; get; } = null!;
         public long? AbsoluteEnd { set; get; } = null!;
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
         public string RelativeTriggerName { set; get; } = null!;
         public Gs2.Gs2Schedule.Model.RepeatSetting RepeatSetting { set; get; } = null!;
        public CreateEventMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateEventMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateEventMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateEventMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateEventMasterRequest WithScheduleType(string scheduleType) {
            this.ScheduleType = scheduleType;
            return this;
        }
        public CreateEventMasterRequest WithAbsoluteBegin(long? absoluteBegin) {
            this.AbsoluteBegin = absoluteBegin;
            return this;
        }
        public CreateEventMasterRequest WithAbsoluteEnd(long? absoluteEnd) {
            this.AbsoluteEnd = absoluteEnd;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateEventMasterRequest WithRepeatType(string repeatType) {
            this.RepeatType = repeatType;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateEventMasterRequest WithRepeatBeginDayOfMonth(int? repeatBeginDayOfMonth) {
            this.RepeatBeginDayOfMonth = repeatBeginDayOfMonth;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateEventMasterRequest WithRepeatEndDayOfMonth(int? repeatEndDayOfMonth) {
            this.RepeatEndDayOfMonth = repeatEndDayOfMonth;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateEventMasterRequest WithRepeatBeginDayOfWeek(string repeatBeginDayOfWeek) {
            this.RepeatBeginDayOfWeek = repeatBeginDayOfWeek;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateEventMasterRequest WithRepeatEndDayOfWeek(string repeatEndDayOfWeek) {
            this.RepeatEndDayOfWeek = repeatEndDayOfWeek;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateEventMasterRequest WithRepeatBeginHour(int? repeatBeginHour) {
            this.RepeatBeginHour = repeatBeginHour;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateEventMasterRequest WithRepeatEndHour(int? repeatEndHour) {
            this.RepeatEndHour = repeatEndHour;
            return this;
        }
        public CreateEventMasterRequest WithRelativeTriggerName(string relativeTriggerName) {
            this.RelativeTriggerName = relativeTriggerName;
            return this;
        }
        public CreateEventMasterRequest WithRepeatSetting(Gs2.Gs2Schedule.Model.RepeatSetting repeatSetting) {
            this.RepeatSetting = repeatSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateEventMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateEventMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScheduleType(!data.Keys.Contains("scheduleType") || data["scheduleType"] == null ? null : data["scheduleType"].ToString())
                .WithAbsoluteBegin(!data.Keys.Contains("absoluteBegin") || data["absoluteBegin"] == null ? null : (long?)(data["absoluteBegin"].ToString().Contains(".") ? (long)double.Parse(data["absoluteBegin"].ToString()) : long.Parse(data["absoluteBegin"].ToString())))
                .WithAbsoluteEnd(!data.Keys.Contains("absoluteEnd") || data["absoluteEnd"] == null ? null : (long?)(data["absoluteEnd"].ToString().Contains(".") ? (long)double.Parse(data["absoluteEnd"].ToString()) : long.Parse(data["absoluteEnd"].ToString())))
                .WithRelativeTriggerName(!data.Keys.Contains("relativeTriggerName") || data["relativeTriggerName"] == null ? null : data["relativeTriggerName"].ToString())
                .WithRepeatSetting(!data.Keys.Contains("repeatSetting") || data["repeatSetting"] == null ? null : Gs2.Gs2Schedule.Model.RepeatSetting.FromJson(data["repeatSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
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
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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
            if (RepeatSetting != null) {
                RepeatSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ScheduleType + ":";
            key += AbsoluteBegin + ":";
            key += AbsoluteEnd + ":";
            key += RelativeTriggerName + ":";
            key += RepeatSetting + ":";
            return key;
        }
    }
}
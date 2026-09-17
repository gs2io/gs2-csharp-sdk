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

namespace Gs2.Gs2Schedule.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class EventMaster : IComparable
	{
        public string EventId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string ScheduleType { set; get; }
        public long? AbsoluteBegin { set; get; }
        public long? AbsoluteEnd { set; get; }
        public string RelativeTriggerName { set; get; }
        public Gs2.Gs2Schedule.Model.RepeatSetting RepeatSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        [Obsolete("This method is deprecated")]
        public string RepeatType { set; get; }
        [Obsolete("This method is deprecated")]
        public int? RepeatBeginDayOfMonth { set; get; }
        [Obsolete("This method is deprecated")]
        public int? RepeatEndDayOfMonth { set; get; }
        [Obsolete("This method is deprecated")]
        public string RepeatBeginDayOfWeek { set; get; }
        [Obsolete("This method is deprecated")]
        public string RepeatEndDayOfWeek { set; get; }
        [Obsolete("This method is deprecated")]
        public int? RepeatBeginHour { set; get; }
        [Obsolete("This method is deprecated")]
        public int? RepeatEndHour { set; get; }
        public EventMaster WithEventId(string eventId) {
            this.EventId = eventId;
            return this;
        }
        public EventMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public EventMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public EventMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public EventMaster WithScheduleType(string scheduleType) {
            this.ScheduleType = scheduleType;
            return this;
        }
        public EventMaster WithAbsoluteBegin(long? absoluteBegin) {
            this.AbsoluteBegin = absoluteBegin;
            return this;
        }
        public EventMaster WithAbsoluteEnd(long? absoluteEnd) {
            this.AbsoluteEnd = absoluteEnd;
            return this;
        }
        public EventMaster WithRelativeTriggerName(string relativeTriggerName) {
            this.RelativeTriggerName = relativeTriggerName;
            return this;
        }
        public EventMaster WithRepeatSetting(Gs2.Gs2Schedule.Model.RepeatSetting repeatSetting) {
            this.RepeatSetting = repeatSetting;
            return this;
        }
        public EventMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public EventMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public EventMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public EventMaster WithRepeatType(string repeatType) {
            this.RepeatType = repeatType;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public EventMaster WithRepeatBeginDayOfMonth(int? repeatBeginDayOfMonth) {
            this.RepeatBeginDayOfMonth = repeatBeginDayOfMonth;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public EventMaster WithRepeatEndDayOfMonth(int? repeatEndDayOfMonth) {
            this.RepeatEndDayOfMonth = repeatEndDayOfMonth;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public EventMaster WithRepeatBeginDayOfWeek(string repeatBeginDayOfWeek) {
            this.RepeatBeginDayOfWeek = repeatBeginDayOfWeek;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public EventMaster WithRepeatEndDayOfWeek(string repeatEndDayOfWeek) {
            this.RepeatEndDayOfWeek = repeatEndDayOfWeek;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public EventMaster WithRepeatBeginHour(int? repeatBeginHour) {
            this.RepeatBeginHour = repeatBeginHour;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public EventMaster WithRepeatEndHour(int? repeatEndHour) {
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
        public static EventMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new EventMaster()
                .WithEventId(!data.Keys.Contains("eventId") || data["eventId"] == null ? null : data["eventId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScheduleType(!data.Keys.Contains("scheduleType") || data["scheduleType"] == null ? null : data["scheduleType"].ToString())
                .WithAbsoluteBegin(!data.Keys.Contains("absoluteBegin") || data["absoluteBegin"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["absoluteBegin"].ToString()))
                .WithAbsoluteEnd(!data.Keys.Contains("absoluteEnd") || data["absoluteEnd"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["absoluteEnd"].ToString()))
                .WithRelativeTriggerName(!data.Keys.Contains("relativeTriggerName") || data["relativeTriggerName"] == null ? null : data["relativeTriggerName"].ToString())
                .WithRepeatSetting(!data.Keys.Contains("repeatSetting") || data["repeatSetting"] == null ? null : Gs2.Gs2Schedule.Model.RepeatSetting.FromJson(data["repeatSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()))
                .WithRepeatType(!data.Keys.Contains("repeatType") || data["repeatType"] == null ? null : data["repeatType"].ToString())
                .WithRepeatBeginDayOfMonth(!data.Keys.Contains("repeatBeginDayOfMonth") || data["repeatBeginDayOfMonth"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["repeatBeginDayOfMonth"].ToString()))
                .WithRepeatEndDayOfMonth(!data.Keys.Contains("repeatEndDayOfMonth") || data["repeatEndDayOfMonth"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["repeatEndDayOfMonth"].ToString()))
                .WithRepeatBeginDayOfWeek(!data.Keys.Contains("repeatBeginDayOfWeek") || data["repeatBeginDayOfWeek"] == null ? null : data["repeatBeginDayOfWeek"].ToString())
                .WithRepeatEndDayOfWeek(!data.Keys.Contains("repeatEndDayOfWeek") || data["repeatEndDayOfWeek"] == null ? null : data["repeatEndDayOfWeek"].ToString())
                .WithRepeatBeginHour(!data.Keys.Contains("repeatBeginHour") || data["repeatBeginHour"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["repeatBeginHour"].ToString()))
                .WithRepeatEndHour(!data.Keys.Contains("repeatEndHour") || data["repeatEndHour"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["repeatEndHour"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["eventId"] = EventId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["scheduleType"] = ScheduleType,
                ["absoluteBegin"] = AbsoluteBegin,
                ["absoluteEnd"] = AbsoluteEnd,
                ["relativeTriggerName"] = RelativeTriggerName,
                ["repeatSetting"] = RepeatSetting?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
            if (RelativeTriggerName != null) {
                writer.WritePropertyName("relativeTriggerName");
                writer.Write(RelativeTriggerName.ToString());
            }
            if (RepeatSetting != null) {
                writer.WritePropertyName("repeatSetting");
                RepeatSetting.WriteJson(writer);
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            var other = obj as EventMaster;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type EventMaster.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(EventId, other.EventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ScheduleType, other.ScheduleType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(AbsoluteBegin, other.AbsoluteBegin);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(AbsoluteEnd, other.AbsoluteEnd);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RelativeTriggerName, other.RelativeTriggerName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RepeatSetting, other.RepeatSetting);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RepeatType, other.RepeatType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RepeatBeginDayOfMonth, other.RepeatBeginDayOfMonth);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RepeatEndDayOfMonth, other.RepeatEndDayOfMonth);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RepeatBeginDayOfWeek, other.RepeatBeginDayOfWeek);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RepeatEndDayOfWeek, other.RepeatEndDayOfWeek);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RepeatBeginHour, other.RepeatBeginHour);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RepeatEndHour, other.RepeatEndHour);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (EventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.eventId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.metadata.error.tooLong"),
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
                            new RequestError("eventMaster", "schedule.eventMaster.scheduleType.error.invalid"),
                        });
                }
            }
            {
                if (AbsoluteBegin < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.absoluteBegin.error.invalid"),
                    });
                }
                if (AbsoluteBegin > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.absoluteBegin.error.invalid"),
                    });
                }
            }
            {
                if (AbsoluteEnd < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.absoluteEnd.error.invalid"),
                    });
                }
                if (AbsoluteEnd > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.absoluteEnd.error.invalid"),
                    });
                }
            }
            if (ScheduleType == "relative") {
                if (RelativeTriggerName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.relativeTriggerName.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("eventMaster", "schedule.eventMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new EventMaster {
                EventId = EventId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                ScheduleType = ScheduleType,
                AbsoluteBegin = AbsoluteBegin,
                AbsoluteEnd = AbsoluteEnd,
                RelativeTriggerName = RelativeTriggerName,
                RepeatSetting = RepeatSetting?.Clone() as Gs2.Gs2Schedule.Model.RepeatSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
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
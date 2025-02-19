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

namespace Gs2.Gs2Schedule.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetEventResult : IResult
	{
        public Gs2.Gs2Schedule.Model.Event Item { set; get; }
        public bool? InSchedule { set; get; }
        public long? ScheduleStartAt { set; get; }
        public long? ScheduleEndAt { set; get; }
        public Gs2.Gs2Schedule.Model.RepeatSchedule RepeatSchedule { set; get; }
        public ResultMetadata Metadata { set; get; }

        public GetEventResult WithItem(Gs2.Gs2Schedule.Model.Event item) {
            this.Item = item;
            return this;
        }

        public GetEventResult WithInSchedule(bool? inSchedule) {
            this.InSchedule = inSchedule;
            return this;
        }

        public GetEventResult WithScheduleStartAt(long? scheduleStartAt) {
            this.ScheduleStartAt = scheduleStartAt;
            return this;
        }

        public GetEventResult WithScheduleEndAt(long? scheduleEndAt) {
            this.ScheduleEndAt = scheduleEndAt;
            return this;
        }

        public GetEventResult WithRepeatSchedule(Gs2.Gs2Schedule.Model.RepeatSchedule repeatSchedule) {
            this.RepeatSchedule = repeatSchedule;
            return this;
        }

        public GetEventResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetEventResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetEventResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Schedule.Model.Event.FromJson(data["item"]))
                .WithInSchedule(!data.Keys.Contains("inSchedule") || data["inSchedule"] == null ? null : (bool?)bool.Parse(data["inSchedule"].ToString()))
                .WithScheduleStartAt(!data.Keys.Contains("scheduleStartAt") || data["scheduleStartAt"] == null ? null : (long?)(data["scheduleStartAt"].ToString().Contains(".") ? (long)double.Parse(data["scheduleStartAt"].ToString()) : long.Parse(data["scheduleStartAt"].ToString())))
                .WithScheduleEndAt(!data.Keys.Contains("scheduleEndAt") || data["scheduleEndAt"] == null ? null : (long?)(data["scheduleEndAt"].ToString().Contains(".") ? (long)double.Parse(data["scheduleEndAt"].ToString()) : long.Parse(data["scheduleEndAt"].ToString())))
                .WithRepeatSchedule(!data.Keys.Contains("repeatSchedule") || data["repeatSchedule"] == null ? null : Gs2.Gs2Schedule.Model.RepeatSchedule.FromJson(data["repeatSchedule"]))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["inSchedule"] = InSchedule,
                ["scheduleStartAt"] = ScheduleStartAt,
                ["scheduleEndAt"] = ScheduleEndAt,
                ["repeatSchedule"] = RepeatSchedule?.ToJson(),
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (InSchedule != null) {
                writer.WritePropertyName("inSchedule");
                writer.Write(bool.Parse(InSchedule.ToString()));
            }
            if (ScheduleStartAt != null) {
                writer.WritePropertyName("scheduleStartAt");
                writer.Write((ScheduleStartAt.ToString().Contains(".") ? (long)double.Parse(ScheduleStartAt.ToString()) : long.Parse(ScheduleStartAt.ToString())));
            }
            if (ScheduleEndAt != null) {
                writer.WritePropertyName("scheduleEndAt");
                writer.Write((ScheduleEndAt.ToString().Contains(".") ? (long)double.Parse(ScheduleEndAt.ToString()) : long.Parse(ScheduleEndAt.ToString())));
            }
            if (RepeatSchedule != null) {
                RepeatSchedule.WriteJson(writer);
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}
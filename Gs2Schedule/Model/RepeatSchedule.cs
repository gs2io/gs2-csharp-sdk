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
	public class RepeatSchedule : IComparable
	{
        public int? RepeatCount { set; get; } = null!;
        public long? CurrentRepeatStartAt { set; get; } = null!;
        public long? CurrentRepeatEndAt { set; get; } = null!;
        public long? LastRepeatEndAt { set; get; } = null!;
        public long? NextRepeatStartAt { set; get; } = null!;
        public RepeatSchedule WithRepeatCount(int? repeatCount) {
            this.RepeatCount = repeatCount;
            return this;
        }
        public RepeatSchedule WithCurrentRepeatStartAt(long? currentRepeatStartAt) {
            this.CurrentRepeatStartAt = currentRepeatStartAt;
            return this;
        }
        public RepeatSchedule WithCurrentRepeatEndAt(long? currentRepeatEndAt) {
            this.CurrentRepeatEndAt = currentRepeatEndAt;
            return this;
        }
        public RepeatSchedule WithLastRepeatEndAt(long? lastRepeatEndAt) {
            this.LastRepeatEndAt = lastRepeatEndAt;
            return this;
        }
        public RepeatSchedule WithNextRepeatStartAt(long? nextRepeatStartAt) {
            this.NextRepeatStartAt = nextRepeatStartAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RepeatSchedule FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RepeatSchedule()
                .WithRepeatCount(!data.Keys.Contains("repeatCount") || data["repeatCount"] == null ? null : (int?)(data["repeatCount"].ToString().Contains(".") ? (int)double.Parse(data["repeatCount"].ToString()) : int.Parse(data["repeatCount"].ToString())))
                .WithCurrentRepeatStartAt(!data.Keys.Contains("currentRepeatStartAt") || data["currentRepeatStartAt"] == null ? null : (long?)(data["currentRepeatStartAt"].ToString().Contains(".") ? (long)double.Parse(data["currentRepeatStartAt"].ToString()) : long.Parse(data["currentRepeatStartAt"].ToString())))
                .WithCurrentRepeatEndAt(!data.Keys.Contains("currentRepeatEndAt") || data["currentRepeatEndAt"] == null ? null : (long?)(data["currentRepeatEndAt"].ToString().Contains(".") ? (long)double.Parse(data["currentRepeatEndAt"].ToString()) : long.Parse(data["currentRepeatEndAt"].ToString())))
                .WithLastRepeatEndAt(!data.Keys.Contains("lastRepeatEndAt") || data["lastRepeatEndAt"] == null ? null : (long?)(data["lastRepeatEndAt"].ToString().Contains(".") ? (long)double.Parse(data["lastRepeatEndAt"].ToString()) : long.Parse(data["lastRepeatEndAt"].ToString())))
                .WithNextRepeatStartAt(!data.Keys.Contains("nextRepeatStartAt") || data["nextRepeatStartAt"] == null ? null : (long?)(data["nextRepeatStartAt"].ToString().Contains(".") ? (long)double.Parse(data["nextRepeatStartAt"].ToString()) : long.Parse(data["nextRepeatStartAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["repeatCount"] = RepeatCount,
                ["currentRepeatStartAt"] = CurrentRepeatStartAt,
                ["currentRepeatEndAt"] = CurrentRepeatEndAt,
                ["lastRepeatEndAt"] = LastRepeatEndAt,
                ["nextRepeatStartAt"] = NextRepeatStartAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RepeatCount != null) {
                writer.WritePropertyName("repeatCount");
                writer.Write((RepeatCount.ToString().Contains(".") ? (int)double.Parse(RepeatCount.ToString()) : int.Parse(RepeatCount.ToString())));
            }
            if (CurrentRepeatStartAt != null) {
                writer.WritePropertyName("currentRepeatStartAt");
                writer.Write((CurrentRepeatStartAt.ToString().Contains(".") ? (long)double.Parse(CurrentRepeatStartAt.ToString()) : long.Parse(CurrentRepeatStartAt.ToString())));
            }
            if (CurrentRepeatEndAt != null) {
                writer.WritePropertyName("currentRepeatEndAt");
                writer.Write((CurrentRepeatEndAt.ToString().Contains(".") ? (long)double.Parse(CurrentRepeatEndAt.ToString()) : long.Parse(CurrentRepeatEndAt.ToString())));
            }
            if (LastRepeatEndAt != null) {
                writer.WritePropertyName("lastRepeatEndAt");
                writer.Write((LastRepeatEndAt.ToString().Contains(".") ? (long)double.Parse(LastRepeatEndAt.ToString()) : long.Parse(LastRepeatEndAt.ToString())));
            }
            if (NextRepeatStartAt != null) {
                writer.WritePropertyName("nextRepeatStartAt");
                writer.Write((NextRepeatStartAt.ToString().Contains(".") ? (long)double.Parse(NextRepeatStartAt.ToString()) : long.Parse(NextRepeatStartAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RepeatSchedule;
            var diff = 0;
            if (RepeatCount == null && RepeatCount == other.RepeatCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(RepeatCount - other.RepeatCount);
            }
            if (CurrentRepeatStartAt == null && CurrentRepeatStartAt == other.CurrentRepeatStartAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentRepeatStartAt - other.CurrentRepeatStartAt);
            }
            if (CurrentRepeatEndAt == null && CurrentRepeatEndAt == other.CurrentRepeatEndAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentRepeatEndAt - other.CurrentRepeatEndAt);
            }
            if (LastRepeatEndAt == null && LastRepeatEndAt == other.LastRepeatEndAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(LastRepeatEndAt - other.LastRepeatEndAt);
            }
            if (NextRepeatStartAt == null && NextRepeatStartAt == other.NextRepeatStartAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(NextRepeatStartAt - other.NextRepeatStartAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (RepeatCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.repeatCount.error.invalid"),
                    });
                }
                if (RepeatCount > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.repeatCount.error.invalid"),
                    });
                }
            }
            {
                if (CurrentRepeatStartAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.currentRepeatStartAt.error.invalid"),
                    });
                }
                if (CurrentRepeatStartAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.currentRepeatStartAt.error.invalid"),
                    });
                }
            }
            {
                if (CurrentRepeatEndAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.currentRepeatEndAt.error.invalid"),
                    });
                }
                if (CurrentRepeatEndAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.currentRepeatEndAt.error.invalid"),
                    });
                }
            }
            {
                if (LastRepeatEndAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.lastRepeatEndAt.error.invalid"),
                    });
                }
                if (LastRepeatEndAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.lastRepeatEndAt.error.invalid"),
                    });
                }
            }
            {
                if (NextRepeatStartAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.nextRepeatStartAt.error.invalid"),
                    });
                }
                if (NextRepeatStartAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("repeatSchedule", "schedule.repeatSchedule.nextRepeatStartAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RepeatSchedule {
                RepeatCount = RepeatCount,
                CurrentRepeatStartAt = CurrentRepeatStartAt,
                CurrentRepeatEndAt = CurrentRepeatEndAt,
                LastRepeatEndAt = LastRepeatEndAt,
                NextRepeatStartAt = NextRepeatStartAt,
            };
        }
    }
}
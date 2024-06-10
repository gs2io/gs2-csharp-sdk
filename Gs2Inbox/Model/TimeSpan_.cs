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

namespace Gs2.Gs2Inbox.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class TimeSpan_ : IComparable
	{
        public int? Days { set; get; } = null!;
        public int? Hours { set; get; } = null!;
        public int? Minutes { set; get; } = null!;
        public TimeSpan_ WithDays(int? days) {
            this.Days = days;
            return this;
        }
        public TimeSpan_ WithHours(int? hours) {
            this.Hours = hours;
            return this;
        }
        public TimeSpan_ WithMinutes(int? minutes) {
            this.Minutes = minutes;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TimeSpan_ FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TimeSpan_()
                .WithDays(!data.Keys.Contains("days") || data["days"] == null ? null : (int?)(data["days"].ToString().Contains(".") ? (int)double.Parse(data["days"].ToString()) : int.Parse(data["days"].ToString())))
                .WithHours(!data.Keys.Contains("hours") || data["hours"] == null ? null : (int?)(data["hours"].ToString().Contains(".") ? (int)double.Parse(data["hours"].ToString()) : int.Parse(data["hours"].ToString())))
                .WithMinutes(!data.Keys.Contains("minutes") || data["minutes"] == null ? null : (int?)(data["minutes"].ToString().Contains(".") ? (int)double.Parse(data["minutes"].ToString()) : int.Parse(data["minutes"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["days"] = Days,
                ["hours"] = Hours,
                ["minutes"] = Minutes,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Days != null) {
                writer.WritePropertyName("days");
                writer.Write((Days.ToString().Contains(".") ? (int)double.Parse(Days.ToString()) : int.Parse(Days.ToString())));
            }
            if (Hours != null) {
                writer.WritePropertyName("hours");
                writer.Write((Hours.ToString().Contains(".") ? (int)double.Parse(Hours.ToString()) : int.Parse(Hours.ToString())));
            }
            if (Minutes != null) {
                writer.WritePropertyName("minutes");
                writer.Write((Minutes.ToString().Contains(".") ? (int)double.Parse(Minutes.ToString()) : int.Parse(Minutes.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TimeSpan_;
            var diff = 0;
            if (Days == null && Days == other.Days)
            {
                // null and null
            }
            else
            {
                diff += (int)(Days - other.Days);
            }
            if (Hours == null && Hours == other.Hours)
            {
                // null and null
            }
            else
            {
                diff += (int)(Hours - other.Hours);
            }
            if (Minutes == null && Minutes == other.Minutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(Minutes - other.Minutes);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Days < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeSpan", "inbox.timeSpan.days.error.invalid"),
                    });
                }
                if (Days > 365) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeSpan", "inbox.timeSpan.days.error.invalid"),
                    });
                }
            }
            {
                if (Hours < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeSpan", "inbox.timeSpan.hours.error.invalid"),
                    });
                }
                if (Hours > 24) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeSpan", "inbox.timeSpan.hours.error.invalid"),
                    });
                }
            }
            {
                if (Minutes < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeSpan", "inbox.timeSpan.minutes.error.invalid"),
                    });
                }
                if (Minutes > 60) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeSpan", "inbox.timeSpan.minutes.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new TimeSpan_ {
                Days = Days,
                Hours = Hours,
                Minutes = Minutes,
            };
        }
    }
}
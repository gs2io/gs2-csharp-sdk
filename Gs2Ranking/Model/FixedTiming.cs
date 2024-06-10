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

namespace Gs2.Gs2Ranking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class FixedTiming : IComparable
	{
        public int? Hour { set; get; } = null!;
        public int? Minute { set; get; } = null!;
        public FixedTiming WithHour(int? hour) {
            this.Hour = hour;
            return this;
        }
        public FixedTiming WithMinute(int? minute) {
            this.Minute = minute;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static FixedTiming FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new FixedTiming()
                .WithHour(!data.Keys.Contains("hour") || data["hour"] == null ? null : (int?)(data["hour"].ToString().Contains(".") ? (int)double.Parse(data["hour"].ToString()) : int.Parse(data["hour"].ToString())))
                .WithMinute(!data.Keys.Contains("minute") || data["minute"] == null ? null : (int?)(data["minute"].ToString().Contains(".") ? (int)double.Parse(data["minute"].ToString()) : int.Parse(data["minute"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["hour"] = Hour,
                ["minute"] = Minute,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Hour != null) {
                writer.WritePropertyName("hour");
                writer.Write((Hour.ToString().Contains(".") ? (int)double.Parse(Hour.ToString()) : int.Parse(Hour.ToString())));
            }
            if (Minute != null) {
                writer.WritePropertyName("minute");
                writer.Write((Minute.ToString().Contains(".") ? (int)double.Parse(Minute.ToString()) : int.Parse(Minute.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as FixedTiming;
            var diff = 0;
            if (Hour == null && Hour == other.Hour)
            {
                // null and null
            }
            else
            {
                diff += (int)(Hour - other.Hour);
            }
            if (Minute == null && Minute == other.Minute)
            {
                // null and null
            }
            else
            {
                diff += (int)(Minute - other.Minute);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Hour < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("fixedTiming", "ranking.fixedTiming.hour.error.invalid"),
                    });
                }
                if (Hour > 23) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("fixedTiming", "ranking.fixedTiming.hour.error.invalid"),
                    });
                }
            }
            {
                if (Minute < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("fixedTiming", "ranking.fixedTiming.minute.error.invalid"),
                    });
                }
                if (Minute > 59) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("fixedTiming", "ranking.fixedTiming.minute.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new FixedTiming {
                Hour = Hour,
                Minute = Minute,
            };
        }
    }
}
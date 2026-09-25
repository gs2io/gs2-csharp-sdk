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

namespace Gs2.Gs2Ranking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class FixedTiming : IComparable
	{
        public int? Hour { set; get; }
        public int? Minute { set; get; }
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
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new FixedTiming()
                .WithHour(!data.Keys.Contains("hour") || data["hour"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["hour"].ToString()))
                .WithMinute(!data.Keys.Contains("minute") || data["minute"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["minute"].ToString()));
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type FixedTiming.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Hour, other.Hour);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Minute, other.Minute);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
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

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class TimeseriesPoint : IComparable
	{
        public long? Timestamp { set; get; }
        public Gs2.Gs2Log.Model.TimeseriesValue[] Values { set; get; }
        public TimeseriesPoint WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }
        public TimeseriesPoint WithValues(Gs2.Gs2Log.Model.TimeseriesValue[] values) {
            this.Values = values;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TimeseriesPoint FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new TimeseriesPoint()
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["timestamp"].ToString()))
                .WithValues(!data.Keys.Contains("values") || data["values"] == null || !data["values"].IsArray ? null : data["values"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.TimeseriesValue.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData valuesJsonData = null;
            if (Values != null && Values.Length > 0)
            {
                valuesJsonData = new JsonData();
                foreach (var value in Values)
                {
                    valuesJsonData.Add(value.ToJson());
                }
            }
            return new JsonData {
                ["timestamp"] = Timestamp,
                ["values"] = valuesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            if (Values != null) {
                writer.WritePropertyName("values");
                writer.WriteArrayStart();
                foreach (var value in Values)
                {
                    if (value != null) {
                        value.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TimeseriesPoint;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type TimeseriesPoint.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Timestamp, other.Timestamp);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Values, other.Values);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Timestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeseriesPoint", "log.timeseriesPoint.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeseriesPoint", "log.timeseriesPoint.timestamp.error.invalid"),
                    });
                }
            }
            {
                if (Values.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeseriesPoint", "log.timeseriesPoint.values.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new TimeseriesPoint {
                Timestamp = Timestamp,
                Values = Values?.Clone() as Gs2.Gs2Log.Model.TimeseriesValue[],
            };
        }
    }
}
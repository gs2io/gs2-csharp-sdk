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
	public partial class TimeseriesMetadata : IComparable
	{
        public string[] Keys { set; get; }
        public string[] GroupBy { set; get; }
        public TimeseriesMetadata WithKeys(string[] keys) {
            this.Keys = keys;
            return this;
        }
        public TimeseriesMetadata WithGroupBy(string[] groupBy) {
            this.GroupBy = groupBy;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TimeseriesMetadata FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TimeseriesMetadata()
                .WithKeys(!data.Keys.Contains("keys") || data["keys"] == null || !data["keys"].IsArray ? null : data["keys"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithGroupBy(!data.Keys.Contains("groupBy") || data["groupBy"] == null || !data["groupBy"].IsArray ? null : data["groupBy"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData keysJsonData = null;
            if (Keys != null && Keys.Length > 0)
            {
                keysJsonData = new JsonData();
                foreach (var key in Keys)
                {
                    keysJsonData.Add(key);
                }
            }
            JsonData groupByJsonData = null;
            if (GroupBy != null && GroupBy.Length > 0)
            {
                groupByJsonData = new JsonData();
                foreach (var groupB in GroupBy)
                {
                    groupByJsonData.Add(groupB);
                }
            }
            return new JsonData {
                ["keys"] = keysJsonData,
                ["groupBy"] = groupByJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Keys != null) {
                writer.WritePropertyName("keys");
                writer.WriteArrayStart();
                foreach (var key in Keys)
                {
                    if (key != null) {
                        writer.Write(key.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (GroupBy != null) {
                writer.WritePropertyName("groupBy");
                writer.WriteArrayStart();
                foreach (var groupB in GroupBy)
                {
                    if (groupB != null) {
                        writer.Write(groupB.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TimeseriesMetadata;
            var diff = 0;
            if (Keys == null && Keys == other.Keys)
            {
                // null and null
            }
            else
            {
                diff += Keys.Length - other.Keys.Length;
                for (var i = 0; i < Keys.Length; i++)
                {
                    diff += Keys[i].CompareTo(other.Keys[i]);
                }
            }
            if (GroupBy == null && GroupBy == other.GroupBy)
            {
                // null and null
            }
            else
            {
                diff += GroupBy.Length - other.GroupBy.Length;
                for (var i = 0; i < GroupBy.Length; i++)
                {
                    diff += GroupBy[i].CompareTo(other.GroupBy[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (Keys.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeseriesMetadata", "log.timeseriesMetadata.keys.error.tooMany"),
                    });
                }
            }
            {
                if (GroupBy.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("timeseriesMetadata", "log.timeseriesMetadata.groupBy.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new TimeseriesMetadata {
                Keys = Keys?.Clone() as string[],
                GroupBy = GroupBy?.Clone() as string[],
            };
        }
    }
}
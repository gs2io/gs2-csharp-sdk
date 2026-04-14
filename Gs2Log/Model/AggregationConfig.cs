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
	public partial class AggregationConfig : IComparable
	{
        public string Type { set; get; }
        public string Field { set; get; }
        public AggregationConfig WithType(string type) {
            this.Type = type;
            return this;
        }
        public AggregationConfig WithField(string field) {
            this.Field = field;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AggregationConfig FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AggregationConfig()
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithField(!data.Keys.Contains("field") || data["field"] == null ? null : data["field"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["type"] = Type,
                ["field"] = Field,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (Field != null) {
                writer.WritePropertyName("field");
                writer.Write(Field.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AggregationConfig;
            var diff = 0;
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += Type.CompareTo(other.Type);
            }
            if (Field == null && Field == other.Field)
            {
                // null and null
            }
            else
            {
                diff += Field.CompareTo(other.Field);
            }
            return diff;
        }

        public void Validate() {
            {
                switch (Type) {
                    case "count":
                    case "unique":
                    case "sum":
                    case "avg":
                    case "max":
                    case "min":
                    case "p90":
                    case "p95":
                    case "p99":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("aggregationConfig", "log.aggregationConfig.type.error.invalid"),
                        });
                }
            }
            {
                if (Field.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("aggregationConfig", "log.aggregationConfig.field.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new AggregationConfig {
                Type = Type,
                Field = Field,
            };
        }
    }
}
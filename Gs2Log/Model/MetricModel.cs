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
	public partial class MetricModel : IComparable
	{
        public string Name { set; get; }
        public string Type { set; get; }
        public string[] Labels { set; get; }
        public MetricModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public MetricModel WithType(string type) {
            this.Type = type;
            return this;
        }
        public MetricModel WithLabels(string[] labels) {
            this.Labels = labels;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MetricModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new MetricModel()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithLabels(!data.Keys.Contains("labels") || data["labels"] == null || !data["labels"].IsArray ? null : data["labels"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData labelsJsonData = null;
            if (Labels != null && Labels.Length > 0)
            {
                labelsJsonData = new JsonData();
                foreach (var label in Labels)
                {
                    labelsJsonData.Add(label);
                }
            }
            return new JsonData {
                ["name"] = Name,
                ["type"] = Type,
                ["labels"] = labelsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (Labels != null) {
                writer.WritePropertyName("labels");
                writer.WriteArrayStart();
                foreach (var label in Labels)
                {
                    if (label != null) {
                        writer.Write(label.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as MetricModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type MetricModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Type, other.Type);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Labels, other.Labels);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("metricModel", "log.metricModel.name.error.tooLong"),
                    });
                }
            }
            {
                switch (Type) {
                    case "string":
                    case "double":
                    case "measure":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("metricModel", "log.metricModel.type.error.invalid"),
                        });
                }
            }
            {
                if (Labels.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("metricModel", "log.metricModel.labels.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new MetricModel {
                Name = Name,
                Type = Type,
                Labels = Labels?.Clone() as string[],
            };
        }
    }
}
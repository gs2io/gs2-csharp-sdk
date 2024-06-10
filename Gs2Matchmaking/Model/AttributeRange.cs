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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AttributeRange : IComparable
	{
        public string Name { set; get; } = null!;
        public int? Min { set; get; } = null!;
        public int? Max { set; get; } = null!;
        public AttributeRange WithName(string name) {
            this.Name = name;
            return this;
        }
        public AttributeRange WithMin(int? min) {
            this.Min = min;
            return this;
        }
        public AttributeRange WithMax(int? max) {
            this.Max = max;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AttributeRange FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AttributeRange()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMin(!data.Keys.Contains("min") || data["min"] == null ? null : (int?)(data["min"].ToString().Contains(".") ? (int)double.Parse(data["min"].ToString()) : int.Parse(data["min"].ToString())))
                .WithMax(!data.Keys.Contains("max") || data["max"] == null ? null : (int?)(data["max"].ToString().Contains(".") ? (int)double.Parse(data["max"].ToString()) : int.Parse(data["max"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["min"] = Min,
                ["max"] = Max,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Min != null) {
                writer.WritePropertyName("min");
                writer.Write((Min.ToString().Contains(".") ? (int)double.Parse(Min.ToString()) : int.Parse(Min.ToString())));
            }
            if (Max != null) {
                writer.WritePropertyName("max");
                writer.Write((Max.ToString().Contains(".") ? (int)double.Parse(Max.ToString()) : int.Parse(Max.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AttributeRange;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Min == null && Min == other.Min)
            {
                // null and null
            }
            else
            {
                diff += (int)(Min - other.Min);
            }
            if (Max == null && Max == other.Max)
            {
                // null and null
            }
            else
            {
                diff += (int)(Max - other.Max);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("attributeRange", "matchmaking.attributeRange.name.error.tooLong"),
                    });
                }
            }
            {
                if (Min < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("attributeRange", "matchmaking.attributeRange.min.error.invalid"),
                    });
                }
                if (Min > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("attributeRange", "matchmaking.attributeRange.min.error.invalid"),
                    });
                }
            }
            {
                if (Max < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("attributeRange", "matchmaking.attributeRange.max.error.invalid"),
                    });
                }
                if (Max > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("attributeRange", "matchmaking.attributeRange.max.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new AttributeRange {
                Name = Name,
                Min = Min,
                Max = Max,
            };
        }
    }
}
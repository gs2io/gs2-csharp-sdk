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
using UnityEngine.Scripting;

namespace Gs2.Gs2Matchmaking.Model
{

	[Preserve]
	public class AttributeRange : IComparable
	{
        public string Name { set; get; }
        public int? Min { set; get; }
        public int? Max { set; get; }

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

    	[Preserve]
        public static AttributeRange FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AttributeRange()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMin(!data.Keys.Contains("min") || data["min"] == null ? null : (int?)int.Parse(data["min"].ToString()))
                .WithMax(!data.Keys.Contains("max") || data["max"] == null ? null : (int?)int.Parse(data["max"].ToString()));
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
                writer.Write(int.Parse(Min.ToString()));
            }
            if (Max != null) {
                writer.WritePropertyName("max");
                writer.Write(int.Parse(Max.ToString()));
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
    }
}
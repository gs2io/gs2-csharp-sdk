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

namespace Gs2.Gs2Stamina.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class MaxStaminaTable : IComparable
	{
        public string MaxStaminaTableId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string ExperienceModelId { set; get; }
        public int[] Values { set; get; }

        public MaxStaminaTable WithMaxStaminaTableId(string maxStaminaTableId) {
            this.MaxStaminaTableId = maxStaminaTableId;
            return this;
        }

        public MaxStaminaTable WithName(string name) {
            this.Name = name;
            return this;
        }

        public MaxStaminaTable WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public MaxStaminaTable WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }

        public MaxStaminaTable WithValues(int[] values) {
            this.Values = values;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MaxStaminaTable FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MaxStaminaTable()
                .WithMaxStaminaTableId(!data.Keys.Contains("maxStaminaTableId") || data["maxStaminaTableId"] == null ? null : data["maxStaminaTableId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithValues(!data.Keys.Contains("values") || data["values"] == null ? new int[]{} : data["values"].Cast<JsonData>().Select(v => {
                    return int.Parse(v.ToString());
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["maxStaminaTableId"] = MaxStaminaTableId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["experienceModelId"] = ExperienceModelId,
                ["values"] = new JsonData(Values == null ? new JsonData[]{} :
                        Values.Select(v => {
                            return new JsonData((int?)int.Parse(v.ToString()));
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MaxStaminaTableId != null) {
                writer.WritePropertyName("maxStaminaTableId");
                writer.Write(MaxStaminaTableId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (Values != null) {
                writer.WritePropertyName("values");
                writer.WriteArrayStart();
                foreach (var value in Values)
                {
                    if (value != null) {
                        writer.Write(int.Parse(value.ToString()));
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as MaxStaminaTable;
            var diff = 0;
            if (MaxStaminaTableId == null && MaxStaminaTableId == other.MaxStaminaTableId)
            {
                // null and null
            }
            else
            {
                diff += MaxStaminaTableId.CompareTo(other.MaxStaminaTableId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (ExperienceModelId == null && ExperienceModelId == other.ExperienceModelId)
            {
                // null and null
            }
            else
            {
                diff += ExperienceModelId.CompareTo(other.ExperienceModelId);
            }
            if (Values == null && Values == other.Values)
            {
                // null and null
            }
            else
            {
                diff += Values.Length - other.Values.Length;
                for (var i = 0; i < Values.Length; i++)
                {
                    diff += (int)(Values[i] - other.Values[i]);
                }
            }
            return diff;
        }
    }
}
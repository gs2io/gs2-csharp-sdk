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

namespace Gs2.Gs2Experience.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ThresholdMaster : IComparable
	{
        public string ThresholdId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public long[] Values { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public ThresholdMaster WithThresholdId(string thresholdId) {
            this.ThresholdId = thresholdId;
            return this;
        }

        public ThresholdMaster WithName(string name) {
            this.Name = name;
            return this;
        }

        public ThresholdMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public ThresholdMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public ThresholdMaster WithValues(long[] values) {
            this.Values = values;
            return this;
        }

        public ThresholdMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public ThresholdMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ThresholdMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ThresholdMaster()
                .WithThresholdId(!data.Keys.Contains("thresholdId") || data["thresholdId"] == null ? null : data["thresholdId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithValues(!data.Keys.Contains("values") || data["values"] == null ? new long[]{} : data["values"].Cast<JsonData>().Select(v => {
                    return long.Parse(v.ToString());
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["thresholdId"] = ThresholdId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["values"] = new JsonData(Values == null ? new JsonData[]{} :
                        Values.Select(v => {
                            return new JsonData((long?)long.Parse(v.ToString()));
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ThresholdId != null) {
                writer.WritePropertyName("thresholdId");
                writer.Write(ThresholdId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Values != null) {
                writer.WritePropertyName("values");
                writer.WriteArrayStart();
                foreach (var value in Values)
                {
                    if (value != null) {
                        writer.Write(long.Parse(value.ToString()));
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ThresholdMaster;
            var diff = 0;
            if (ThresholdId == null && ThresholdId == other.ThresholdId)
            {
                // null and null
            }
            else
            {
                diff += ThresholdId.CompareTo(other.ThresholdId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
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
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}
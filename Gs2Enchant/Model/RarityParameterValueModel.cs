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

namespace Gs2.Gs2Enchant.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RarityParameterValueModel : IComparable
	{
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string ResourceName { set; get; }
        public long? ResourceValue { set; get; }
        public int? Weight { set; get; }
        public RarityParameterValueModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public RarityParameterValueModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RarityParameterValueModel WithResourceName(string resourceName) {
            this.ResourceName = resourceName;
            return this;
        }
        public RarityParameterValueModel WithResourceValue(long? resourceValue) {
            this.ResourceValue = resourceValue;
            return this;
        }
        public RarityParameterValueModel WithWeight(int? weight) {
            this.Weight = weight;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RarityParameterValueModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RarityParameterValueModel()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithResourceName(!data.Keys.Contains("resourceName") || data["resourceName"] == null ? null : data["resourceName"].ToString())
                .WithResourceValue(!data.Keys.Contains("resourceValue") || data["resourceValue"] == null ? null : (long?)long.Parse(data["resourceValue"].ToString()))
                .WithWeight(!data.Keys.Contains("weight") || data["weight"] == null ? null : (int?)int.Parse(data["weight"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["resourceName"] = ResourceName,
                ["resourceValue"] = ResourceValue,
                ["weight"] = Weight,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ResourceName != null) {
                writer.WritePropertyName("resourceName");
                writer.Write(ResourceName.ToString());
            }
            if (ResourceValue != null) {
                writer.WritePropertyName("resourceValue");
                writer.Write(long.Parse(ResourceValue.ToString()));
            }
            if (Weight != null) {
                writer.WritePropertyName("weight");
                writer.Write(int.Parse(Weight.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RarityParameterValueModel;
            var diff = 0;
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
            if (ResourceName == null && ResourceName == other.ResourceName)
            {
                // null and null
            }
            else
            {
                diff += ResourceName.CompareTo(other.ResourceName);
            }
            if (ResourceValue == null && ResourceValue == other.ResourceValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResourceValue - other.ResourceValue);
            }
            if (Weight == null && Weight == other.Weight)
            {
                // null and null
            }
            else
            {
                diff += (int)(Weight - other.Weight);
            }
            return diff;
        }
    }
}
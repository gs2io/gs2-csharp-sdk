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
	public class RarityParameterValue : IComparable
	{
        public string Name { set; get; } = null!;
        public string ResourceName { set; get; } = null!;
        public long? ResourceValue { set; get; } = null!;
        public RarityParameterValue WithName(string name) {
            this.Name = name;
            return this;
        }
        public RarityParameterValue WithResourceName(string resourceName) {
            this.ResourceName = resourceName;
            return this;
        }
        public RarityParameterValue WithResourceValue(long? resourceValue) {
            this.ResourceValue = resourceValue;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RarityParameterValue FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RarityParameterValue()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithResourceName(!data.Keys.Contains("resourceName") || data["resourceName"] == null ? null : data["resourceName"].ToString())
                .WithResourceValue(!data.Keys.Contains("resourceValue") || data["resourceValue"] == null ? null : (long?)(data["resourceValue"].ToString().Contains(".") ? (long)double.Parse(data["resourceValue"].ToString()) : long.Parse(data["resourceValue"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["resourceName"] = ResourceName,
                ["resourceValue"] = ResourceValue,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (ResourceName != null) {
                writer.WritePropertyName("resourceName");
                writer.Write(ResourceName.ToString());
            }
            if (ResourceValue != null) {
                writer.WritePropertyName("resourceValue");
                writer.Write((ResourceValue.ToString().Contains(".") ? (long)double.Parse(ResourceValue.ToString()) : long.Parse(ResourceValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RarityParameterValue;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
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
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterValue", "enchant.rarityParameterValue.name.error.tooLong"),
                    });
                }
            }
            {
                if (ResourceName.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterValue", "enchant.rarityParameterValue.resourceName.error.tooLong"),
                    });
                }
            }
            {
                if (ResourceValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterValue", "enchant.rarityParameterValue.resourceValue.error.invalid"),
                    });
                }
                if (ResourceValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterValue", "enchant.rarityParameterValue.resourceValue.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RarityParameterValue {
                Name = Name,
                ResourceName = ResourceName,
                ResourceValue = ResourceValue,
            };
        }
    }
}
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

namespace Gs2.Gs2Enhance.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class UnleashRecipe : IComparable
	{
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string[] TargetGroupKeys { set; get; }
        public Gs2.Gs2Enhance.Model.UnleashMaterial[] Materials { set; get; }
        public UnleashRecipe WithName(string name) {
            this.Name = name;
            return this;
        }
        public UnleashRecipe WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UnleashRecipe WithTargetGroupKeys(string[] targetGroupKeys) {
            this.TargetGroupKeys = targetGroupKeys;
            return this;
        }
        public UnleashRecipe WithMaterials(Gs2.Gs2Enhance.Model.UnleashMaterial[] materials) {
            this.Materials = materials;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnleashRecipe FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new UnleashRecipe()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTargetGroupKeys(!data.Keys.Contains("targetGroupKeys") || data["targetGroupKeys"] == null || !data["targetGroupKeys"].IsArray ? null : data["targetGroupKeys"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithMaterials(!data.Keys.Contains("materials") || data["materials"] == null || !data["materials"].IsArray ? null : data["materials"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.UnleashMaterial.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData targetGroupKeysJsonData = null;
            if (TargetGroupKeys != null && TargetGroupKeys.Length > 0)
            {
                targetGroupKeysJsonData = new JsonData();
                foreach (var targetGroupKey in TargetGroupKeys)
                {
                    targetGroupKeysJsonData.Add(targetGroupKey);
                }
            }
            JsonData materialsJsonData = null;
            if (Materials != null && Materials.Length > 0)
            {
                materialsJsonData = new JsonData();
                foreach (var material in Materials)
                {
                    materialsJsonData.Add(material.ToJson());
                }
            }
            return new JsonData {
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["targetGroupKeys"] = targetGroupKeysJsonData,
                ["materials"] = materialsJsonData,
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
            if (TargetGroupKeys != null) {
                writer.WritePropertyName("targetGroupKeys");
                writer.WriteArrayStart();
                foreach (var targetGroupKey in TargetGroupKeys)
                {
                    if (targetGroupKey != null) {
                        writer.Write(targetGroupKey.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Materials != null) {
                writer.WritePropertyName("materials");
                writer.WriteArrayStart();
                foreach (var material in Materials)
                {
                    if (material != null) {
                        material.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnleashRecipe;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type UnleashRecipe.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(TargetGroupKeys, other.TargetGroupKeys);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Materials, other.Materials);
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
                        new RequestError("unleashRecipe", "enhance.unleashRecipe.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRecipe", "enhance.unleashRecipe.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (TargetGroupKeys.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRecipe", "enhance.unleashRecipe.targetGroupKeys.error.tooMany"),
                    });
                }
            }
            {
                if (Materials.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRecipe", "enhance.unleashRecipe.materials.error.tooFew"),
                    });
                }
                if (Materials.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRecipe", "enhance.unleashRecipe.materials.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnleashRecipe {
                Name = Name,
                Metadata = Metadata,
                TargetGroupKeys = TargetGroupKeys?.Clone() as string[],
                Materials = Materials?.Clone() as Gs2.Gs2Enhance.Model.UnleashMaterial[],
            };
        }
    }
}
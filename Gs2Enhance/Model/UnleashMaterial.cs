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
	public partial class UnleashMaterial : IComparable
	{
        public string Name { set; get; }
        public string MaterialType { set; get; }
        public Gs2.Gs2Enhance.Model.UnleashIndividualMaterialSetting IndividualSetting { set; get; }
        public Gs2.Gs2Enhance.Model.UnleashQuantityMaterialSetting QuantitySetting { set; get; }
        public UnleashMaterial WithName(string name) {
            this.Name = name;
            return this;
        }
        public UnleashMaterial WithMaterialType(string materialType) {
            this.MaterialType = materialType;
            return this;
        }
        public UnleashMaterial WithIndividualSetting(Gs2.Gs2Enhance.Model.UnleashIndividualMaterialSetting individualSetting) {
            this.IndividualSetting = individualSetting;
            return this;
        }
        public UnleashMaterial WithQuantitySetting(Gs2.Gs2Enhance.Model.UnleashQuantityMaterialSetting quantitySetting) {
            this.QuantitySetting = quantitySetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnleashMaterial FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new UnleashMaterial()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMaterialType(!data.Keys.Contains("materialType") || data["materialType"] == null ? null : data["materialType"].ToString())
                .WithIndividualSetting(!data.Keys.Contains("individualSetting") || data["individualSetting"] == null ? null : Gs2.Gs2Enhance.Model.UnleashIndividualMaterialSetting.FromJson(data["individualSetting"]))
                .WithQuantitySetting(!data.Keys.Contains("quantitySetting") || data["quantitySetting"] == null ? null : Gs2.Gs2Enhance.Model.UnleashQuantityMaterialSetting.FromJson(data["quantitySetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["materialType"] = MaterialType,
                ["individualSetting"] = IndividualSetting?.ToJson(),
                ["quantitySetting"] = QuantitySetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (MaterialType != null) {
                writer.WritePropertyName("materialType");
                writer.Write(MaterialType.ToString());
            }
            if (IndividualSetting != null) {
                writer.WritePropertyName("individualSetting");
                IndividualSetting.WriteJson(writer);
            }
            if (QuantitySetting != null) {
                writer.WritePropertyName("quantitySetting");
                QuantitySetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnleashMaterial;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type UnleashMaterial.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MaterialType, other.MaterialType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(IndividualSetting, other.IndividualSetting);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(QuantitySetting, other.QuantitySetting);
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
                        new RequestError("unleashMaterial", "enhance.unleashMaterial.name.error.tooLong"),
                    });
                }
            }
            {
                switch (MaterialType) {
                    case "individual":
                    case "quantity":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("unleashMaterial", "enhance.unleashMaterial.materialType.error.invalid"),
                        });
                }
            }
            if (MaterialType == "individual") {
            }
            if (MaterialType == "quantity") {
            }
        }

        public object Clone() {
            return new UnleashMaterial {
                Name = Name,
                MaterialType = MaterialType,
                IndividualSetting = IndividualSetting?.Clone() as Gs2.Gs2Enhance.Model.UnleashIndividualMaterialSetting,
                QuantitySetting = QuantitySetting?.Clone() as Gs2.Gs2Enhance.Model.UnleashQuantityMaterialSetting,
            };
        }
    }
}
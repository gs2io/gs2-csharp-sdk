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

namespace Gs2.Gs2MegaField.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AreaModel : IComparable
	{
        public string AreaModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2MegaField.Model.LayerModel[] LayerModels { set; get; } = null!;
        public AreaModel WithAreaModelId(string areaModelId) {
            this.AreaModelId = areaModelId;
            return this;
        }
        public AreaModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public AreaModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public AreaModel WithLayerModels(Gs2.Gs2MegaField.Model.LayerModel[] layerModels) {
            this.LayerModels = layerModels;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):model:area:(?<areaModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):model:area:(?<areaModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):model:area:(?<areaModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _areaModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):model:area:(?<areaModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAreaModelNameFromGrn(
            string grn
        )
        {
            var match = _areaModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["areaModelName"].Success)
            {
                return null;
            }
            return match.Groups["areaModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AreaModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AreaModel()
                .WithAreaModelId(!data.Keys.Contains("areaModelId") || data["areaModelId"] == null ? null : data["areaModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithLayerModels(!data.Keys.Contains("layerModels") || data["layerModels"] == null || !data["layerModels"].IsArray ? null : data["layerModels"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2MegaField.Model.LayerModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData layerModelsJsonData = null;
            if (LayerModels != null && LayerModels.Length > 0)
            {
                layerModelsJsonData = new JsonData();
                foreach (var layerModel in LayerModels)
                {
                    layerModelsJsonData.Add(layerModel.ToJson());
                }
            }
            return new JsonData {
                ["areaModelId"] = AreaModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["layerModels"] = layerModelsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AreaModelId != null) {
                writer.WritePropertyName("areaModelId");
                writer.Write(AreaModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (LayerModels != null) {
                writer.WritePropertyName("layerModels");
                writer.WriteArrayStart();
                foreach (var layerModel in LayerModels)
                {
                    if (layerModel != null) {
                        layerModel.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AreaModel;
            var diff = 0;
            if (AreaModelId == null && AreaModelId == other.AreaModelId)
            {
                // null and null
            }
            else
            {
                diff += AreaModelId.CompareTo(other.AreaModelId);
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
            if (LayerModels == null && LayerModels == other.LayerModels)
            {
                // null and null
            }
            else
            {
                diff += LayerModels.Length - other.LayerModels.Length;
                for (var i = 0; i < LayerModels.Length; i++)
                {
                    diff += LayerModels[i].CompareTo(other.LayerModels[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (AreaModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("areaModel", "megaField.areaModel.areaModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("areaModel", "megaField.areaModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("areaModel", "megaField.areaModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (LayerModels.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("areaModel", "megaField.areaModel.layerModels.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new AreaModel {
                AreaModelId = AreaModelId,
                Name = Name,
                Metadata = Metadata,
                LayerModels = LayerModels.Clone() as Gs2.Gs2MegaField.Model.LayerModel[],
            };
        }
    }
}
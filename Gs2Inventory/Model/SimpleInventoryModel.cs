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

namespace Gs2.Gs2Inventory.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SimpleInventoryModel : IComparable
	{
        public string InventoryModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.SimpleItemModel[] SimpleItemModels { set; get; } = null!;
        public SimpleInventoryModel WithInventoryModelId(string inventoryModelId) {
            this.InventoryModelId = inventoryModelId;
            return this;
        }
        public SimpleInventoryModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public SimpleInventoryModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SimpleInventoryModel WithSimpleItemModels(Gs2.Gs2Inventory.Model.SimpleItemModel[] simpleItemModels) {
            this.SimpleItemModels = simpleItemModels;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):simple:model:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):simple:model:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):simple:model:(?<inventoryName>.+)",
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

        private static System.Text.RegularExpressions.Regex _inventoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):simple:model:(?<inventoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetInventoryNameFromGrn(
            string grn
        )
        {
            var match = _inventoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["inventoryName"].Success)
            {
                return null;
            }
            return match.Groups["inventoryName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SimpleInventoryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SimpleInventoryModel()
                .WithInventoryModelId(!data.Keys.Contains("inventoryModelId") || data["inventoryModelId"] == null ? null : data["inventoryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithSimpleItemModels(!data.Keys.Contains("simpleItemModels") || data["simpleItemModels"] == null || !data["simpleItemModels"].IsArray ? null : data["simpleItemModels"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inventory.Model.SimpleItemModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData simpleItemModelsJsonData = null;
            if (SimpleItemModels != null && SimpleItemModels.Length > 0)
            {
                simpleItemModelsJsonData = new JsonData();
                foreach (var simpleItemModel in SimpleItemModels)
                {
                    simpleItemModelsJsonData.Add(simpleItemModel.ToJson());
                }
            }
            return new JsonData {
                ["inventoryModelId"] = InventoryModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["simpleItemModels"] = simpleItemModelsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (InventoryModelId != null) {
                writer.WritePropertyName("inventoryModelId");
                writer.Write(InventoryModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (SimpleItemModels != null) {
                writer.WritePropertyName("simpleItemModels");
                writer.WriteArrayStart();
                foreach (var simpleItemModel in SimpleItemModels)
                {
                    if (simpleItemModel != null) {
                        simpleItemModel.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SimpleInventoryModel;
            var diff = 0;
            if (InventoryModelId == null && InventoryModelId == other.InventoryModelId)
            {
                // null and null
            }
            else
            {
                diff += InventoryModelId.CompareTo(other.InventoryModelId);
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
            if (SimpleItemModels == null && SimpleItemModels == other.SimpleItemModels)
            {
                // null and null
            }
            else
            {
                diff += SimpleItemModels.Length - other.SimpleItemModels.Length;
                for (var i = 0; i < SimpleItemModels.Length; i++)
                {
                    diff += SimpleItemModels[i].CompareTo(other.SimpleItemModels[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (InventoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("simpleInventoryModel", "inventory.simpleInventoryModel.inventoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("simpleInventoryModel", "inventory.simpleInventoryModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("simpleInventoryModel", "inventory.simpleInventoryModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (SimpleItemModels.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("simpleInventoryModel", "inventory.simpleInventoryModel.simpleItemModels.error.tooFew"),
                    });
                }
                if (SimpleItemModels.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("simpleInventoryModel", "inventory.simpleInventoryModel.simpleItemModels.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new SimpleInventoryModel {
                InventoryModelId = InventoryModelId,
                Name = Name,
                Metadata = Metadata,
                SimpleItemModels = SimpleItemModels.Clone() as Gs2.Gs2Inventory.Model.SimpleItemModel[],
            };
        }
    }
}
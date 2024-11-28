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
	public class BigInventoryModel : IComparable
	{
        public string InventoryModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.BigItemModel[] BigItemModels { set; get; } = null!;
        public BigInventoryModel WithInventoryModelId(string inventoryModelId) {
            this.InventoryModelId = inventoryModelId;
            return this;
        }
        public BigInventoryModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public BigInventoryModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public BigInventoryModel WithBigItemModels(Gs2.Gs2Inventory.Model.BigItemModel[] bigItemModels) {
            this.BigItemModels = bigItemModels;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):big:model:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):big:model:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):big:model:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):big:model:(?<inventoryName>.+)",
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
        public static BigInventoryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BigInventoryModel()
                .WithInventoryModelId(!data.Keys.Contains("inventoryModelId") || data["inventoryModelId"] == null ? null : data["inventoryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithBigItemModels(!data.Keys.Contains("bigItemModels") || data["bigItemModels"] == null || !data["bigItemModels"].IsArray ? null : data["bigItemModels"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inventory.Model.BigItemModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData bigItemModelsJsonData = null;
            if (BigItemModels != null && BigItemModels.Length > 0)
            {
                bigItemModelsJsonData = new JsonData();
                foreach (var bigItemModel in BigItemModels)
                {
                    bigItemModelsJsonData.Add(bigItemModel.ToJson());
                }
            }
            return new JsonData {
                ["inventoryModelId"] = InventoryModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["bigItemModels"] = bigItemModelsJsonData,
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
            if (BigItemModels != null) {
                writer.WritePropertyName("bigItemModels");
                writer.WriteArrayStart();
                foreach (var bigItemModel in BigItemModels)
                {
                    if (bigItemModel != null) {
                        bigItemModel.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BigInventoryModel;
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
            if (BigItemModels == null && BigItemModels == other.BigItemModels)
            {
                // null and null
            }
            else
            {
                diff += BigItemModels.Length - other.BigItemModels.Length;
                for (var i = 0; i < BigItemModels.Length; i++)
                {
                    diff += BigItemModels[i].CompareTo(other.BigItemModels[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (InventoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bigInventoryModel", "inventory.bigInventoryModel.inventoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bigInventoryModel", "inventory.bigInventoryModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bigInventoryModel", "inventory.bigInventoryModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (BigItemModels.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bigInventoryModel", "inventory.bigInventoryModel.bigItemModels.error.tooFew"),
                    });
                }
                if (BigItemModels.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bigInventoryModel", "inventory.bigInventoryModel.bigItemModels.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new BigInventoryModel {
                InventoryModelId = InventoryModelId,
                Name = Name,
                Metadata = Metadata,
                BigItemModels = BigItemModels.Clone() as Gs2.Gs2Inventory.Model.BigItemModel[],
            };
        }
    }
}
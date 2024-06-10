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
	public class ItemModelMaster : IComparable
	{
        public string ItemModelId { set; get; } = null!;
        public string InventoryName { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? StackingLimit { set; get; } = null!;
        public bool? AllowMultipleStacks { set; get; } = null!;
        public int? SortValue { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public ItemModelMaster WithItemModelId(string itemModelId) {
            this.ItemModelId = itemModelId;
            return this;
        }
        public ItemModelMaster WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public ItemModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public ItemModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public ItemModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public ItemModelMaster WithStackingLimit(long? stackingLimit) {
            this.StackingLimit = stackingLimit;
            return this;
        }
        public ItemModelMaster WithAllowMultipleStacks(bool? allowMultipleStacks) {
            this.AllowMultipleStacks = allowMultipleStacks;
            return this;
        }
        public ItemModelMaster WithSortValue(int? sortValue) {
            this.SortValue = sortValue;
            return this;
        }
        public ItemModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public ItemModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public ItemModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+):item:(?<itemName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+):item:(?<itemName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+):item:(?<itemName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+):item:(?<itemName>.+)",
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

        private static System.Text.RegularExpressions.Regex _itemNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+):item:(?<itemName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetItemNameFromGrn(
            string grn
        )
        {
            var match = _itemNameRegex.Match(grn);
            if (!match.Success || !match.Groups["itemName"].Success)
            {
                return null;
            }
            return match.Groups["itemName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ItemModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ItemModelMaster()
                .WithItemModelId(!data.Keys.Contains("itemModelId") || data["itemModelId"] == null ? null : data["itemModelId"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithStackingLimit(!data.Keys.Contains("stackingLimit") || data["stackingLimit"] == null ? null : (long?)(data["stackingLimit"].ToString().Contains(".") ? (long)double.Parse(data["stackingLimit"].ToString()) : long.Parse(data["stackingLimit"].ToString())))
                .WithAllowMultipleStacks(!data.Keys.Contains("allowMultipleStacks") || data["allowMultipleStacks"] == null ? null : (bool?)bool.Parse(data["allowMultipleStacks"].ToString()))
                .WithSortValue(!data.Keys.Contains("sortValue") || data["sortValue"] == null ? null : (int?)(data["sortValue"].ToString().Contains(".") ? (int)double.Parse(data["sortValue"].ToString()) : int.Parse(data["sortValue"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["itemModelId"] = ItemModelId,
                ["inventoryName"] = InventoryName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["stackingLimit"] = StackingLimit,
                ["allowMultipleStacks"] = AllowMultipleStacks,
                ["sortValue"] = SortValue,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ItemModelId != null) {
                writer.WritePropertyName("itemModelId");
                writer.Write(ItemModelId.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
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
            if (StackingLimit != null) {
                writer.WritePropertyName("stackingLimit");
                writer.Write((StackingLimit.ToString().Contains(".") ? (long)double.Parse(StackingLimit.ToString()) : long.Parse(StackingLimit.ToString())));
            }
            if (AllowMultipleStacks != null) {
                writer.WritePropertyName("allowMultipleStacks");
                writer.Write(bool.Parse(AllowMultipleStacks.ToString()));
            }
            if (SortValue != null) {
                writer.WritePropertyName("sortValue");
                writer.Write((SortValue.ToString().Contains(".") ? (int)double.Parse(SortValue.ToString()) : int.Parse(SortValue.ToString())));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ItemModelMaster;
            var diff = 0;
            if (ItemModelId == null && ItemModelId == other.ItemModelId)
            {
                // null and null
            }
            else
            {
                diff += ItemModelId.CompareTo(other.ItemModelId);
            }
            if (InventoryName == null && InventoryName == other.InventoryName)
            {
                // null and null
            }
            else
            {
                diff += InventoryName.CompareTo(other.InventoryName);
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
            if (StackingLimit == null && StackingLimit == other.StackingLimit)
            {
                // null and null
            }
            else
            {
                diff += (int)(StackingLimit - other.StackingLimit);
            }
            if (AllowMultipleStacks == null && AllowMultipleStacks == other.AllowMultipleStacks)
            {
                // null and null
            }
            else
            {
                diff += AllowMultipleStacks == other.AllowMultipleStacks ? 0 : 1;
            }
            if (SortValue == null && SortValue == other.SortValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(SortValue - other.SortValue);
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ItemModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.itemModelId.error.tooLong"),
                    });
                }
            }
            {
                if (InventoryName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.inventoryName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (StackingLimit < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.stackingLimit.error.invalid"),
                    });
                }
                if (StackingLimit > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.stackingLimit.error.invalid"),
                    });
                }
            }
            {
            }
            {
                if (SortValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.sortValue.error.invalid"),
                    });
                }
                if (SortValue > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.sortValue.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemModelMaster", "inventory.itemModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ItemModelMaster {
                ItemModelId = ItemModelId,
                InventoryName = InventoryName,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                StackingLimit = StackingLimit,
                AllowMultipleStacks = AllowMultipleStacks,
                SortValue = SortValue,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
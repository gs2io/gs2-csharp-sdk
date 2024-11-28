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
	public class ItemSet : IComparable
	{
        public string ItemSetId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string InventoryName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string ItemName { set; get; } = null!;
        public long? Count { set; get; } = null!;
        public string[] ReferenceOf { set; get; } = null!;
        public int? SortValue { set; get; } = null!;
        public long? ExpiresAt { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public ItemSet WithItemSetId(string itemSetId) {
            this.ItemSetId = itemSetId;
            return this;
        }
        public ItemSet WithName(string name) {
            this.Name = name;
            return this;
        }
        public ItemSet WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public ItemSet WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ItemSet WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public ItemSet WithCount(long? count) {
            this.Count = count;
            return this;
        }
        public ItemSet WithReferenceOf(string[] referenceOf) {
            this.ReferenceOf = referenceOf;
            return this;
        }
        public ItemSet WithSortValue(int? sortValue) {
            this.SortValue = sortValue;
            return this;
        }
        public ItemSet WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }
        public ItemSet WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public ItemSet WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+)",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _inventoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+)",
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

        private static System.Text.RegularExpressions.Regex _itemSetNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetItemSetNameFromGrn(
            string grn
        )
        {
            var match = _itemSetNameRegex.Match(grn);
            if (!match.Success || !match.Groups["itemSetName"].Success)
            {
                return null;
            }
            return match.Groups["itemSetName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ItemSet FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ItemSet()
                .WithItemSetId(!data.Keys.Contains("itemSetId") || data["itemSetId"] == null ? null : data["itemSetId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (long?)(data["count"].ToString().Contains(".") ? (long)double.Parse(data["count"].ToString()) : long.Parse(data["count"].ToString())))
                .WithReferenceOf(!data.Keys.Contains("referenceOf") || data["referenceOf"] == null || !data["referenceOf"].IsArray ? null : data["referenceOf"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithSortValue(!data.Keys.Contains("sortValue") || data["sortValue"] == null ? null : (int?)(data["sortValue"].ToString().Contains(".") ? (int)double.Parse(data["sortValue"].ToString()) : int.Parse(data["sortValue"].ToString())))
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)(data["expiresAt"].ToString().Contains(".") ? (long)double.Parse(data["expiresAt"].ToString()) : long.Parse(data["expiresAt"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData referenceOfJsonData = null;
            if (ReferenceOf != null && ReferenceOf.Length > 0)
            {
                referenceOfJsonData = new JsonData();
                foreach (var referenceO in ReferenceOf)
                {
                    referenceOfJsonData.Add(referenceO);
                }
            }
            return new JsonData {
                ["itemSetId"] = ItemSetId,
                ["name"] = Name,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["itemName"] = ItemName,
                ["count"] = Count,
                ["referenceOf"] = referenceOfJsonData,
                ["sortValue"] = SortValue,
                ["expiresAt"] = ExpiresAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ItemSetId != null) {
                writer.WritePropertyName("itemSetId");
                writer.Write(ItemSetId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ItemName != null) {
                writer.WritePropertyName("itemName");
                writer.Write(ItemName.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (long)double.Parse(Count.ToString()) : long.Parse(Count.ToString())));
            }
            if (ReferenceOf != null) {
                writer.WritePropertyName("referenceOf");
                writer.WriteArrayStart();
                foreach (var referenceO in ReferenceOf)
                {
                    if (referenceO != null) {
                        writer.Write(referenceO.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (SortValue != null) {
                writer.WritePropertyName("sortValue");
                writer.Write((SortValue.ToString().Contains(".") ? (int)double.Parse(SortValue.ToString()) : int.Parse(SortValue.ToString())));
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ItemSet;
            var diff = 0;
            if (ItemSetId == null && ItemSetId == other.ItemSetId)
            {
                // null and null
            }
            else
            {
                diff += ItemSetId.CompareTo(other.ItemSetId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (InventoryName == null && InventoryName == other.InventoryName)
            {
                // null and null
            }
            else
            {
                diff += InventoryName.CompareTo(other.InventoryName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (ItemName == null && ItemName == other.ItemName)
            {
                // null and null
            }
            else
            {
                diff += ItemName.CompareTo(other.ItemName);
            }
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            if (ReferenceOf == null && ReferenceOf == other.ReferenceOf)
            {
                // null and null
            }
            else
            {
                diff += ReferenceOf.Length - other.ReferenceOf.Length;
                for (var i = 0; i < ReferenceOf.Length; i++)
                {
                    diff += ReferenceOf[i].CompareTo(other.ReferenceOf[i]);
                }
            }
            if (SortValue == null && SortValue == other.SortValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(SortValue - other.SortValue);
            }
            if (ExpiresAt == null && ExpiresAt == other.ExpiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExpiresAt - other.ExpiresAt);
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

        public void Validate() {
            {
                if (ItemSetId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.itemSetId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.name.error.tooLong"),
                    });
                }
            }
            {
                if (InventoryName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.inventoryName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.userId.error.tooLong"),
                    });
                }
            }
            {
                if (ItemName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.itemName.error.tooLong"),
                    });
                }
            }
            {
                if (Count < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.count.error.invalid"),
                    });
                }
                if (Count > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.count.error.invalid"),
                    });
                }
            }
            {
                if (ReferenceOf.Length > 24) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.referenceOf.error.tooMany"),
                    });
                }
            }
            {
                if (SortValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.sortValue.error.invalid"),
                    });
                }
                if (SortValue > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.sortValue.error.invalid"),
                    });
                }
            }
            {
                if (ExpiresAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.expiresAt.error.invalid"),
                    });
                }
                if (ExpiresAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.expiresAt.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("itemSet", "inventory.itemSet.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ItemSet {
                ItemSetId = ItemSetId,
                Name = Name,
                InventoryName = InventoryName,
                UserId = UserId,
                ItemName = ItemName,
                Count = Count,
                ReferenceOf = ReferenceOf.Clone() as string[],
                SortValue = SortValue,
                ExpiresAt = ExpiresAt,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}
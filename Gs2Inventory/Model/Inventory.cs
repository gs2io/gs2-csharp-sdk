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
	public class Inventory : IComparable
	{
        public string InventoryId { set; get; } = null!;
        public string InventoryName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public int? CurrentInventoryCapacityUsage { set; get; } = null!;
        public int? CurrentInventoryMaxCapacity { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Inventory WithInventoryId(string inventoryId) {
            this.InventoryId = inventoryId;
            return this;
        }
        public Inventory WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public Inventory WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Inventory WithCurrentInventoryCapacityUsage(int? currentInventoryCapacityUsage) {
            this.CurrentInventoryCapacityUsage = currentInventoryCapacityUsage;
            return this;
        }
        public Inventory WithCurrentInventoryMaxCapacity(int? currentInventoryMaxCapacity) {
            this.CurrentInventoryMaxCapacity = currentInventoryMaxCapacity;
            return this;
        }
        public Inventory WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Inventory WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Inventory WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+)",
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
        public static Inventory FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Inventory()
                .WithInventoryId(!data.Keys.Contains("inventoryId") || data["inventoryId"] == null ? null : data["inventoryId"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCurrentInventoryCapacityUsage(!data.Keys.Contains("currentInventoryCapacityUsage") || data["currentInventoryCapacityUsage"] == null ? null : (int?)(data["currentInventoryCapacityUsage"].ToString().Contains(".") ? (int)double.Parse(data["currentInventoryCapacityUsage"].ToString()) : int.Parse(data["currentInventoryCapacityUsage"].ToString())))
                .WithCurrentInventoryMaxCapacity(!data.Keys.Contains("currentInventoryMaxCapacity") || data["currentInventoryMaxCapacity"] == null ? null : (int?)(data["currentInventoryMaxCapacity"].ToString().Contains(".") ? (int)double.Parse(data["currentInventoryMaxCapacity"].ToString()) : int.Parse(data["currentInventoryMaxCapacity"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["inventoryId"] = InventoryId,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["currentInventoryCapacityUsage"] = CurrentInventoryCapacityUsage,
                ["currentInventoryMaxCapacity"] = CurrentInventoryMaxCapacity,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (InventoryId != null) {
                writer.WritePropertyName("inventoryId");
                writer.Write(InventoryId.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (CurrentInventoryCapacityUsage != null) {
                writer.WritePropertyName("currentInventoryCapacityUsage");
                writer.Write((CurrentInventoryCapacityUsage.ToString().Contains(".") ? (int)double.Parse(CurrentInventoryCapacityUsage.ToString()) : int.Parse(CurrentInventoryCapacityUsage.ToString())));
            }
            if (CurrentInventoryMaxCapacity != null) {
                writer.WritePropertyName("currentInventoryMaxCapacity");
                writer.Write((CurrentInventoryMaxCapacity.ToString().Contains(".") ? (int)double.Parse(CurrentInventoryMaxCapacity.ToString()) : int.Parse(CurrentInventoryMaxCapacity.ToString())));
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
            var other = obj as Inventory;
            var diff = 0;
            if (InventoryId == null && InventoryId == other.InventoryId)
            {
                // null and null
            }
            else
            {
                diff += InventoryId.CompareTo(other.InventoryId);
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
            if (CurrentInventoryCapacityUsage == null && CurrentInventoryCapacityUsage == other.CurrentInventoryCapacityUsage)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentInventoryCapacityUsage - other.CurrentInventoryCapacityUsage);
            }
            if (CurrentInventoryMaxCapacity == null && CurrentInventoryMaxCapacity == other.CurrentInventoryMaxCapacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentInventoryMaxCapacity - other.CurrentInventoryMaxCapacity);
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
                if (InventoryId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.inventoryId.error.tooLong"),
                    });
                }
            }
            {
                if (InventoryName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.inventoryName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.userId.error.tooLong"),
                    });
                }
            }
            {
                if (CurrentInventoryCapacityUsage < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.currentInventoryCapacityUsage.error.invalid"),
                    });
                }
                if (CurrentInventoryCapacityUsage > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.currentInventoryCapacityUsage.error.invalid"),
                    });
                }
            }
            {
                if (CurrentInventoryMaxCapacity < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.currentInventoryMaxCapacity.error.invalid"),
                    });
                }
                if (CurrentInventoryMaxCapacity > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.currentInventoryMaxCapacity.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventory", "inventory.inventory.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Inventory {
                InventoryId = InventoryId,
                InventoryName = InventoryName,
                UserId = UserId,
                CurrentInventoryCapacityUsage = CurrentInventoryCapacityUsage,
                CurrentInventoryMaxCapacity = CurrentInventoryMaxCapacity,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Inventory.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inventory.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AcquireItemSetByUserIdRequest : Gs2Request<AcquireItemSetByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string InventoryName { set; get; } = null!;
         public string ItemName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public long? AcquireCount { set; get; } = null!;
         public long? ExpiresAt { set; get; } = null!;
         public bool? CreateNewItemSet { set; get; } = null!;
         public string ItemSetName { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public AcquireItemSetByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public AcquireItemSetByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public AcquireItemSetByUserIdRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public AcquireItemSetByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public AcquireItemSetByUserIdRequest WithAcquireCount(long? acquireCount) {
            this.AcquireCount = acquireCount;
            return this;
        }
        public AcquireItemSetByUserIdRequest WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }
        public AcquireItemSetByUserIdRequest WithCreateNewItemSet(bool? createNewItemSet) {
            this.CreateNewItemSet = createNewItemSet;
            return this;
        }
        public AcquireItemSetByUserIdRequest WithItemSetName(string itemSetName) {
            this.ItemSetName = itemSetName;
            return this;
        }
        public AcquireItemSetByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public AcquireItemSetByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireItemSetByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireItemSetByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAcquireCount(!data.Keys.Contains("acquireCount") || data["acquireCount"] == null ? null : (long?)(data["acquireCount"].ToString().Contains(".") ? (long)double.Parse(data["acquireCount"].ToString()) : long.Parse(data["acquireCount"].ToString())))
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)(data["expiresAt"].ToString().Contains(".") ? (long)double.Parse(data["expiresAt"].ToString()) : long.Parse(data["expiresAt"].ToString())))
                .WithCreateNewItemSet(!data.Keys.Contains("createNewItemSet") || data["createNewItemSet"] == null ? null : (bool?)bool.Parse(data["createNewItemSet"].ToString()))
                .WithItemSetName(!data.Keys.Contains("itemSetName") || data["itemSetName"] == null ? null : data["itemSetName"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["itemName"] = ItemName,
                ["userId"] = UserId,
                ["acquireCount"] = AcquireCount,
                ["expiresAt"] = ExpiresAt,
                ["createNewItemSet"] = CreateNewItemSet,
                ["itemSetName"] = ItemSetName,
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (ItemName != null) {
                writer.WritePropertyName("itemName");
                writer.Write(ItemName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (AcquireCount != null) {
                writer.WritePropertyName("acquireCount");
                writer.Write((AcquireCount.ToString().Contains(".") ? (long)double.Parse(AcquireCount.ToString()) : long.Parse(AcquireCount.ToString())));
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
            }
            if (CreateNewItemSet != null) {
                writer.WritePropertyName("createNewItemSet");
                writer.Write(bool.Parse(CreateNewItemSet.ToString()));
            }
            if (ItemSetName != null) {
                writer.WritePropertyName("itemSetName");
                writer.Write(ItemSetName.ToString());
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += InventoryName + ":";
            key += ItemName + ":";
            key += UserId + ":";
            key += ExpiresAt + ":";
            key += CreateNewItemSet + ":";
            key += ItemSetName + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}
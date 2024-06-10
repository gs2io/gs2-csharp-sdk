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
	public class ConsumeItemSetByUserIdRequest : Gs2Request<ConsumeItemSetByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string InventoryName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string ItemName { set; get; } = null!;
         public long? ConsumeCount { set; get; } = null!;
         public string ItemSetName { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public ConsumeItemSetByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ConsumeItemSetByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public ConsumeItemSetByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ConsumeItemSetByUserIdRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public ConsumeItemSetByUserIdRequest WithConsumeCount(long? consumeCount) {
            this.ConsumeCount = consumeCount;
            return this;
        }
        public ConsumeItemSetByUserIdRequest WithItemSetName(string itemSetName) {
            this.ItemSetName = itemSetName;
            return this;
        }
        public ConsumeItemSetByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public ConsumeItemSetByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ConsumeItemSetByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ConsumeItemSetByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithConsumeCount(!data.Keys.Contains("consumeCount") || data["consumeCount"] == null ? null : (long?)(data["consumeCount"].ToString().Contains(".") ? (long)double.Parse(data["consumeCount"].ToString()) : long.Parse(data["consumeCount"].ToString())))
                .WithItemSetName(!data.Keys.Contains("itemSetName") || data["itemSetName"] == null ? null : data["itemSetName"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["itemName"] = ItemName,
                ["consumeCount"] = ConsumeCount,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ItemName != null) {
                writer.WritePropertyName("itemName");
                writer.Write(ItemName.ToString());
            }
            if (ConsumeCount != null) {
                writer.WritePropertyName("consumeCount");
                writer.Write((ConsumeCount.ToString().Contains(".") ? (long)double.Parse(ConsumeCount.ToString()) : long.Parse(ConsumeCount.ToString())));
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
            key += UserId + ":";
            key += ItemName + ":";
            key += ItemSetName + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}
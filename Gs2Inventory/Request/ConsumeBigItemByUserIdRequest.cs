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
using System.Numerics;
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
	public class ConsumeBigItemByUserIdRequest : Gs2Request<ConsumeBigItemByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string InventoryName { set; get; }
        public string UserId { set; get; }
        public string ItemName { set; get; }
        public string ConsumeCount { set; get; }
        public string DuplicationAvoider { set; get; }
        public ConsumeBigItemByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ConsumeBigItemByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public ConsumeBigItemByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ConsumeBigItemByUserIdRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public ConsumeBigItemByUserIdRequest WithConsumeCount(string consumeCount) {
            this.ConsumeCount = consumeCount;
            return this;
        }

        public ConsumeBigItemByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ConsumeBigItemByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ConsumeBigItemByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithConsumeCount(!data.Keys.Contains("consumeCount") || data["consumeCount"] == null ? null : data["consumeCount"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["itemName"] = ItemName,
                ["consumeCount"] = ConsumeCount,
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
                writer.Write(ConsumeCount.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += InventoryName + ":";
            key += UserId + ":";
            key += ItemName + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new ConsumeBigItemByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                UserId = UserId,
                ItemName = ItemName,
                ConsumeCount = (BigInteger.Parse(ConsumeCount) * x).ToString("0"),
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (ConsumeBigItemByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values ConsumeBigItemByUserIdRequest::namespaceName");
            }
            if (InventoryName != y.InventoryName) {
                throw new ArithmeticException("mismatch parameter values ConsumeBigItemByUserIdRequest::inventoryName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values ConsumeBigItemByUserIdRequest::userId");
            }
            if (ItemName != y.ItemName) {
                throw new ArithmeticException("mismatch parameter values ConsumeBigItemByUserIdRequest::itemName");
            }
            return new ConsumeBigItemByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                UserId = UserId,
                ItemName = ItemName,
                ConsumeCount = ConsumeCount + y.ConsumeCount,
            };
        }
    }
}
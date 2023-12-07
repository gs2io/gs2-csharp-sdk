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
	public class SetBigItemByUserIdRequest : Gs2Request<SetBigItemByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string InventoryName { set; get; }
         public string UserId { set; get; }
         public string ItemName { set; get; }
         public string Count { set; get; }
        public string DuplicationAvoider { set; get; }
        public SetBigItemByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SetBigItemByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public SetBigItemByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SetBigItemByUserIdRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public SetBigItemByUserIdRequest WithCount(string count) {
            this.Count = count;
            return this;
        }

        public SetBigItemByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetBigItemByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetBigItemByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : data["count"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["itemName"] = ItemName,
                ["count"] = Count,
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
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write(Count.ToString());
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
            return new SetBigItemByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                UserId = UserId,
                ItemName = ItemName,
                Count = (BigInteger.Parse(Count) * x).ToString("0"),
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (SetBigItemByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values SetBigItemByUserIdRequest::namespaceName");
            }
            if (InventoryName != y.InventoryName) {
                throw new ArithmeticException("mismatch parameter values SetBigItemByUserIdRequest::inventoryName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values SetBigItemByUserIdRequest::userId");
            }
            if (ItemName != y.ItemName) {
                throw new ArithmeticException("mismatch parameter values SetBigItemByUserIdRequest::itemName");
            }
            return new SetBigItemByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                UserId = UserId,
                ItemName = ItemName,
                Count = Count + y.Count,
            };
        }
    }
}
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
	public class VerifyBigItemByUserIdRequest : Gs2Request<VerifyBigItemByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string InventoryName { set; get; }
        public string ItemName { set; get; }
        public string VerifyType { set; get; }
        public string Count { set; get; }
        public string DuplicationAvoider { set; get; }
        public VerifyBigItemByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyBigItemByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public VerifyBigItemByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public VerifyBigItemByUserIdRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public VerifyBigItemByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyBigItemByUserIdRequest WithCount(string count) {
            this.Count = count;
            return this;
        }

        public VerifyBigItemByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyBigItemByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyBigItemByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : data["count"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["inventoryName"] = InventoryName,
                ["itemName"] = ItemName,
                ["verifyType"] = VerifyType,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (ItemName != null) {
                writer.WritePropertyName("itemName");
                writer.Write(ItemName.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
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
            key += UserId + ":";
            key += InventoryName + ":";
            key += ItemName + ":";
            key += VerifyType + ":";
            key += Count + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new VerifyBigItemByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                InventoryName = InventoryName,
                ItemName = ItemName,
                VerifyType = VerifyType,
                Count = Count,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (VerifyBigItemByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values VerifyBigItemByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values VerifyBigItemByUserIdRequest::userId");
            }
            if (InventoryName != y.InventoryName) {
                throw new ArithmeticException("mismatch parameter values VerifyBigItemByUserIdRequest::inventoryName");
            }
            if (ItemName != y.ItemName) {
                throw new ArithmeticException("mismatch parameter values VerifyBigItemByUserIdRequest::itemName");
            }
            if (VerifyType != y.VerifyType) {
                throw new ArithmeticException("mismatch parameter values VerifyBigItemByUserIdRequest::verifyType");
            }
            if (Count != y.Count) {
                throw new ArithmeticException("mismatch parameter values VerifyBigItemByUserIdRequest::count");
            }
            return new VerifyBigItemByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                InventoryName = InventoryName,
                ItemName = ItemName,
                VerifyType = VerifyType,
                Count = Count,
            };
        }
    }
}
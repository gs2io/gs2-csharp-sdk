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
	public class VerifyItemSetRequest : Gs2Request<VerifyItemSetRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string InventoryName { set; get; } = null!;
         public string ItemName { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public string ItemSetName { set; get; } = null!;
         public long? Count { set; get; } = null!;
         public bool? MultiplyValueSpecifyingQuantity { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyItemSetRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyItemSetRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyItemSetRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public VerifyItemSetRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public VerifyItemSetRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyItemSetRequest WithItemSetName(string itemSetName) {
            this.ItemSetName = itemSetName;
            return this;
        }
        public VerifyItemSetRequest WithCount(long? count) {
            this.Count = count;
            return this;
        }
        public VerifyItemSetRequest WithMultiplyValueSpecifyingQuantity(bool? multiplyValueSpecifyingQuantity) {
            this.MultiplyValueSpecifyingQuantity = multiplyValueSpecifyingQuantity;
            return this;
        }

        public VerifyItemSetRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyItemSetRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyItemSetRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithItemSetName(!data.Keys.Contains("itemSetName") || data["itemSetName"] == null ? null : data["itemSetName"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (long?)(data["count"].ToString().Contains(".") ? (long)double.Parse(data["count"].ToString()) : long.Parse(data["count"].ToString())))
                .WithMultiplyValueSpecifyingQuantity(!data.Keys.Contains("multiplyValueSpecifyingQuantity") || data["multiplyValueSpecifyingQuantity"] == null ? null : (bool?)bool.Parse(data["multiplyValueSpecifyingQuantity"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["inventoryName"] = InventoryName,
                ["itemName"] = ItemName,
                ["verifyType"] = VerifyType,
                ["itemSetName"] = ItemSetName,
                ["count"] = Count,
                ["multiplyValueSpecifyingQuantity"] = MultiplyValueSpecifyingQuantity,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
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
            if (ItemSetName != null) {
                writer.WritePropertyName("itemSetName");
                writer.Write(ItemSetName.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (long)double.Parse(Count.ToString()) : long.Parse(Count.ToString())));
            }
            if (MultiplyValueSpecifyingQuantity != null) {
                writer.WritePropertyName("multiplyValueSpecifyingQuantity");
                writer.Write(bool.Parse(MultiplyValueSpecifyingQuantity.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += InventoryName + ":";
            key += ItemName + ":";
            key += VerifyType + ":";
            key += ItemSetName + ":";
            key += Count + ":";
            key += MultiplyValueSpecifyingQuantity + ":";
            return key;
        }
    }
}
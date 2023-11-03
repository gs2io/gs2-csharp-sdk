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
	public class VerifyReferenceOfByUserIdRequest : Gs2Request<VerifyReferenceOfByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string InventoryName { set; get; }
        public string UserId { set; get; }
        public string ItemName { set; get; }
        public string ItemSetName { set; get; }
        public string ReferenceOf { set; get; }
        public string VerifyType { set; get; }
        public string DuplicationAvoider { set; get; }

        public VerifyReferenceOfByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public VerifyReferenceOfByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }

        public VerifyReferenceOfByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public VerifyReferenceOfByUserIdRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }

        public VerifyReferenceOfByUserIdRequest WithItemSetName(string itemSetName) {
            this.ItemSetName = itemSetName;
            return this;
        }

        public VerifyReferenceOfByUserIdRequest WithReferenceOf(string referenceOf) {
            this.ReferenceOf = referenceOf;
            return this;
        }

        public VerifyReferenceOfByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }

        public VerifyReferenceOfByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyReferenceOfByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyReferenceOfByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithItemSetName(!data.Keys.Contains("itemSetName") || data["itemSetName"] == null ? null : data["itemSetName"].ToString())
                .WithReferenceOf(!data.Keys.Contains("referenceOf") || data["referenceOf"] == null ? null : data["referenceOf"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["itemName"] = ItemName,
                ["itemSetName"] = ItemSetName,
                ["referenceOf"] = ReferenceOf,
                ["verifyType"] = VerifyType,
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
            if (ItemSetName != null) {
                writer.WritePropertyName("itemSetName");
                writer.Write(ItemSetName.ToString());
            }
            if (ReferenceOf != null) {
                writer.WritePropertyName("referenceOf");
                writer.Write(ReferenceOf.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
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
            key += ReferenceOf + ":";
            key += VerifyType + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new VerifyReferenceOfByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                UserId = UserId,
                ItemName = ItemName,
                ItemSetName = ItemSetName,
                ReferenceOf = ReferenceOf,
                VerifyType = VerifyType,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (VerifyReferenceOfByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values VerifyReferenceOfByUserIdRequest::namespaceName");
            }
            if (InventoryName != y.InventoryName) {
                throw new ArithmeticException("mismatch parameter values VerifyReferenceOfByUserIdRequest::inventoryName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values VerifyReferenceOfByUserIdRequest::userId");
            }
            if (ItemName != y.ItemName) {
                throw new ArithmeticException("mismatch parameter values VerifyReferenceOfByUserIdRequest::itemName");
            }
            if (ItemSetName != y.ItemSetName) {
                throw new ArithmeticException("mismatch parameter values VerifyReferenceOfByUserIdRequest::itemSetName");
            }
            if (ReferenceOf != y.ReferenceOf) {
                throw new ArithmeticException("mismatch parameter values VerifyReferenceOfByUserIdRequest::referenceOf");
            }
            if (VerifyType != y.VerifyType) {
                throw new ArithmeticException("mismatch parameter values VerifyReferenceOfByUserIdRequest::verifyType");
            }
            return new VerifyReferenceOfByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                UserId = UserId,
                ItemName = ItemName,
                ItemSetName = ItemSetName,
                ReferenceOf = ReferenceOf,
                VerifyType = VerifyType,
            };
        }
    }
}
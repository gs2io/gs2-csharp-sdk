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
	public class DeleteReferenceOfByUserIdRequest : Gs2Request<DeleteReferenceOfByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string InventoryName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string ItemName { set; get; } = null!;
         public string ItemSetName { set; get; } = null!;
         public string ReferenceOf { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public DeleteReferenceOfByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DeleteReferenceOfByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public DeleteReferenceOfByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DeleteReferenceOfByUserIdRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public DeleteReferenceOfByUserIdRequest WithItemSetName(string itemSetName) {
            this.ItemSetName = itemSetName;
            return this;
        }
        public DeleteReferenceOfByUserIdRequest WithReferenceOf(string referenceOf) {
            this.ReferenceOf = referenceOf;
            return this;
        }
        public DeleteReferenceOfByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public DeleteReferenceOfByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DeleteReferenceOfByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DeleteReferenceOfByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithItemSetName(!data.Keys.Contains("itemSetName") || data["itemSetName"] == null ? null : data["itemSetName"].ToString())
                .WithReferenceOf(!data.Keys.Contains("referenceOf") || data["referenceOf"] == null ? null : data["referenceOf"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
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
            if (ItemSetName != null) {
                writer.WritePropertyName("itemSetName");
                writer.Write(ItemSetName.ToString());
            }
            if (ReferenceOf != null) {
                writer.WritePropertyName("referenceOf");
                writer.Write(ReferenceOf.ToString());
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
            key += ReferenceOf + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}
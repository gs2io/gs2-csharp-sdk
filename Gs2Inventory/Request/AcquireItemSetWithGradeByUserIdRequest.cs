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
	public class AcquireItemSetWithGradeByUserIdRequest : Gs2Request<AcquireItemSetWithGradeByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string InventoryName { set; get; }
         public string ItemName { set; get; }
         public string UserId { set; get; }
         public string GradeModelId { set; get; }
         public long? GradeValue { set; get; }
        public string DuplicationAvoider { set; get; }
        public AcquireItemSetWithGradeByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public AcquireItemSetWithGradeByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public AcquireItemSetWithGradeByUserIdRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public AcquireItemSetWithGradeByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public AcquireItemSetWithGradeByUserIdRequest WithGradeModelId(string gradeModelId) {
            this.GradeModelId = gradeModelId;
            return this;
        }
        public AcquireItemSetWithGradeByUserIdRequest WithGradeValue(long? gradeValue) {
            this.GradeValue = gradeValue;
            return this;
        }

        public AcquireItemSetWithGradeByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireItemSetWithGradeByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireItemSetWithGradeByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithGradeModelId(!data.Keys.Contains("gradeModelId") || data["gradeModelId"] == null ? null : data["gradeModelId"].ToString())
                .WithGradeValue(!data.Keys.Contains("gradeValue") || data["gradeValue"] == null ? null : (long?)(data["gradeValue"].ToString().Contains(".") ? (long)double.Parse(data["gradeValue"].ToString()) : long.Parse(data["gradeValue"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["itemName"] = ItemName,
                ["userId"] = UserId,
                ["gradeModelId"] = GradeModelId,
                ["gradeValue"] = GradeValue,
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
            if (GradeModelId != null) {
                writer.WritePropertyName("gradeModelId");
                writer.Write(GradeModelId.ToString());
            }
            if (GradeValue != null) {
                writer.WritePropertyName("gradeValue");
                writer.Write((GradeValue.ToString().Contains(".") ? (long)double.Parse(GradeValue.ToString()) : long.Parse(GradeValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += InventoryName + ":";
            key += ItemName + ":";
            key += UserId + ":";
            key += GradeModelId + ":";
            key += GradeValue + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new AcquireItemSetWithGradeByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                ItemName = ItemName,
                UserId = UserId,
                GradeModelId = GradeModelId,
                GradeValue = GradeValue,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (AcquireItemSetWithGradeByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values AcquireItemSetWithGradeByUserIdRequest::namespaceName");
            }
            if (InventoryName != y.InventoryName) {
                throw new ArithmeticException("mismatch parameter values AcquireItemSetWithGradeByUserIdRequest::inventoryName");
            }
            if (ItemName != y.ItemName) {
                throw new ArithmeticException("mismatch parameter values AcquireItemSetWithGradeByUserIdRequest::itemName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values AcquireItemSetWithGradeByUserIdRequest::userId");
            }
            if (GradeModelId != y.GradeModelId) {
                throw new ArithmeticException("mismatch parameter values AcquireItemSetWithGradeByUserIdRequest::gradeModelId");
            }
            if (GradeValue != y.GradeValue) {
                throw new ArithmeticException("mismatch parameter values AcquireItemSetWithGradeByUserIdRequest::gradeValue");
            }
            return new AcquireItemSetWithGradeByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                ItemName = ItemName,
                UserId = UserId,
                GradeModelId = GradeModelId,
                GradeValue = GradeValue,
            };
        }
    }
}
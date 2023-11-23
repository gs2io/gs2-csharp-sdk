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
	public class VerifyInventoryCurrentMaxCapacityByUserIdRequest : Gs2Request<VerifyInventoryCurrentMaxCapacityByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string UserId { set; get; }
         public string InventoryName { set; get; }
         public string VerifyType { set; get; }
         public int? CurrentInventoryMaxCapacity { set; get; }
        public string DuplicationAvoider { set; get; }
        public VerifyInventoryCurrentMaxCapacityByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyInventoryCurrentMaxCapacityByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public VerifyInventoryCurrentMaxCapacityByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public VerifyInventoryCurrentMaxCapacityByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyInventoryCurrentMaxCapacityByUserIdRequest WithCurrentInventoryMaxCapacity(int? currentInventoryMaxCapacity) {
            this.CurrentInventoryMaxCapacity = currentInventoryMaxCapacity;
            return this;
        }

        public VerifyInventoryCurrentMaxCapacityByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyInventoryCurrentMaxCapacityByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyInventoryCurrentMaxCapacityByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithCurrentInventoryMaxCapacity(!data.Keys.Contains("currentInventoryMaxCapacity") || data["currentInventoryMaxCapacity"] == null ? null : (int?)(data["currentInventoryMaxCapacity"].ToString().Contains(".") ? (int)double.Parse(data["currentInventoryMaxCapacity"].ToString()) : int.Parse(data["currentInventoryMaxCapacity"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["inventoryName"] = InventoryName,
                ["verifyType"] = VerifyType,
                ["currentInventoryMaxCapacity"] = CurrentInventoryMaxCapacity,
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
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (CurrentInventoryMaxCapacity != null) {
                writer.WritePropertyName("currentInventoryMaxCapacity");
                writer.Write((CurrentInventoryMaxCapacity.ToString().Contains(".") ? (int)double.Parse(CurrentInventoryMaxCapacity.ToString()) : int.Parse(CurrentInventoryMaxCapacity.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += InventoryName + ":";
            key += VerifyType + ":";
            key += CurrentInventoryMaxCapacity + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new VerifyInventoryCurrentMaxCapacityByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                InventoryName = InventoryName,
                VerifyType = VerifyType,
                CurrentInventoryMaxCapacity = CurrentInventoryMaxCapacity,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (VerifyInventoryCurrentMaxCapacityByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values VerifyInventoryCurrentMaxCapacityByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values VerifyInventoryCurrentMaxCapacityByUserIdRequest::userId");
            }
            if (InventoryName != y.InventoryName) {
                throw new ArithmeticException("mismatch parameter values VerifyInventoryCurrentMaxCapacityByUserIdRequest::inventoryName");
            }
            if (VerifyType != y.VerifyType) {
                throw new ArithmeticException("mismatch parameter values VerifyInventoryCurrentMaxCapacityByUserIdRequest::verifyType");
            }
            if (CurrentInventoryMaxCapacity != y.CurrentInventoryMaxCapacity) {
                throw new ArithmeticException("mismatch parameter values VerifyInventoryCurrentMaxCapacityByUserIdRequest::currentInventoryMaxCapacity");
            }
            return new VerifyInventoryCurrentMaxCapacityByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                InventoryName = InventoryName,
                VerifyType = VerifyType,
                CurrentInventoryMaxCapacity = CurrentInventoryMaxCapacity,
            };
        }
    }
}
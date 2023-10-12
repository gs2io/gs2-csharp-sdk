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
	public class VerifyInventoryCurrentMaxCapacityRequest : Gs2Request<VerifyInventoryCurrentMaxCapacityRequest>
	{
        public string NamespaceName { set; get; }
        public string AccessToken { set; get; }
        public string InventoryName { set; get; }
        public string VerifyType { set; get; }
        public int? CurrentInventoryMaxCapacity { set; get; }
        public string DuplicationAvoider { set; get; }
        public VerifyInventoryCurrentMaxCapacityRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyInventoryCurrentMaxCapacityRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyInventoryCurrentMaxCapacityRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public VerifyInventoryCurrentMaxCapacityRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyInventoryCurrentMaxCapacityRequest WithCurrentInventoryMaxCapacity(int? currentInventoryMaxCapacity) {
            this.CurrentInventoryMaxCapacity = currentInventoryMaxCapacity;
            return this;
        }

        public VerifyInventoryCurrentMaxCapacityRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyInventoryCurrentMaxCapacityRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyInventoryCurrentMaxCapacityRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithCurrentInventoryMaxCapacity(!data.Keys.Contains("currentInventoryMaxCapacity") || data["currentInventoryMaxCapacity"] == null ? null : (int?)int.Parse(data["currentInventoryMaxCapacity"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
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
                writer.Write(int.Parse(CurrentInventoryMaxCapacity.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += InventoryName + ":";
            key += VerifyType + ":";
            key += CurrentInventoryMaxCapacity + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply VerifyInventoryCurrentMaxCapacityRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (VerifyInventoryCurrentMaxCapacityRequest)x;
            return this;
        }
    }
}
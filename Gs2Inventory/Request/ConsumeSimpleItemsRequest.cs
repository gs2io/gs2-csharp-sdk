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
	public class ConsumeSimpleItemsRequest : Gs2Request<ConsumeSimpleItemsRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string InventoryName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public Gs2.Gs2Inventory.Model.ConsumeCount[] ConsumeCounts { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public ConsumeSimpleItemsRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ConsumeSimpleItemsRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public ConsumeSimpleItemsRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public ConsumeSimpleItemsRequest WithConsumeCounts(Gs2.Gs2Inventory.Model.ConsumeCount[] consumeCounts) {
            this.ConsumeCounts = consumeCounts;
            return this;
        }

        public ConsumeSimpleItemsRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ConsumeSimpleItemsRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ConsumeSimpleItemsRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithConsumeCounts(!data.Keys.Contains("consumeCounts") || data["consumeCounts"] == null || !data["consumeCounts"].IsArray ? null : data["consumeCounts"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inventory.Model.ConsumeCount.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData consumeCountsJsonData = null;
            if (ConsumeCounts != null && ConsumeCounts.Length > 0)
            {
                consumeCountsJsonData = new JsonData();
                foreach (var consumeCount in ConsumeCounts)
                {
                    consumeCountsJsonData.Add(consumeCount.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["accessToken"] = AccessToken,
                ["consumeCounts"] = consumeCountsJsonData,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (ConsumeCounts != null) {
                writer.WritePropertyName("consumeCounts");
                writer.WriteArrayStart();
                foreach (var consumeCount in ConsumeCounts)
                {
                    if (consumeCount != null) {
                        consumeCount.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += InventoryName + ":";
            key += AccessToken + ":";
            key += ConsumeCounts + ":";
            return key;
        }
    }
}
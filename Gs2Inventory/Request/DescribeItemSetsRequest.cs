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
	public class DescribeItemSetsRequest : Gs2Request<DescribeItemSetsRequest>
	{
         public string NamespaceName { set; get; }
         public string InventoryName { set; get; }
         public string AccessToken { set; get; }
         public string PageToken { set; get; }
         public int? Limit { set; get; }
        public DescribeItemSetsRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DescribeItemSetsRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public DescribeItemSetsRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public DescribeItemSetsRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public DescribeItemSetsRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeItemSetsRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeItemSetsRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["accessToken"] = AccessToken,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
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
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += InventoryName + ":";
            key += AccessToken + ":";
            key += PageToken + ":";
            key += Limit + ":";
            return key;
        }
    }
}
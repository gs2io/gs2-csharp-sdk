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
using Gs2.Gs2Showcase.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Showcase.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class IncrementPurchaseCountRequest : Gs2Request<IncrementPurchaseCountRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string ShowcaseName { set; get; } = null!;
         public string DisplayItemName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public int? Count { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public IncrementPurchaseCountRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public IncrementPurchaseCountRequest WithShowcaseName(string showcaseName) {
            this.ShowcaseName = showcaseName;
            return this;
        }
        public IncrementPurchaseCountRequest WithDisplayItemName(string displayItemName) {
            this.DisplayItemName = displayItemName;
            return this;
        }
        public IncrementPurchaseCountRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public IncrementPurchaseCountRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public IncrementPurchaseCountRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IncrementPurchaseCountRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IncrementPurchaseCountRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithShowcaseName(!data.Keys.Contains("showcaseName") || data["showcaseName"] == null ? null : data["showcaseName"].ToString())
                .WithDisplayItemName(!data.Keys.Contains("displayItemName") || data["displayItemName"] == null ? null : data["displayItemName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["showcaseName"] = ShowcaseName,
                ["displayItemName"] = DisplayItemName,
                ["accessToken"] = AccessToken,
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
            if (ShowcaseName != null) {
                writer.WritePropertyName("showcaseName");
                writer.Write(ShowcaseName.ToString());
            }
            if (DisplayItemName != null) {
                writer.WritePropertyName("displayItemName");
                writer.Write(DisplayItemName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += ShowcaseName + ":";
            key += DisplayItemName + ":";
            key += AccessToken + ":";
            key += Count + ":";
            return key;
        }
    }
}
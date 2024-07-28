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
using Gs2.Gs2Idle.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Idle.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DecreaseMaximumIdleMinutesRequest : Gs2Request<DecreaseMaximumIdleMinutesRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string CategoryName { set; get; } = null!;
         public int? DecreaseMinutes { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public DecreaseMaximumIdleMinutesRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DecreaseMaximumIdleMinutesRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public DecreaseMaximumIdleMinutesRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }
        public DecreaseMaximumIdleMinutesRequest WithDecreaseMinutes(int? decreaseMinutes) {
            this.DecreaseMinutes = decreaseMinutes;
            return this;
        }

        public DecreaseMaximumIdleMinutesRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DecreaseMaximumIdleMinutesRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DecreaseMaximumIdleMinutesRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithDecreaseMinutes(!data.Keys.Contains("decreaseMinutes") || data["decreaseMinutes"] == null ? null : (int?)(data["decreaseMinutes"].ToString().Contains(".") ? (int)double.Parse(data["decreaseMinutes"].ToString()) : int.Parse(data["decreaseMinutes"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["categoryName"] = CategoryName,
                ["decreaseMinutes"] = DecreaseMinutes,
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
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
            }
            if (DecreaseMinutes != null) {
                writer.WritePropertyName("decreaseMinutes");
                writer.Write((DecreaseMinutes.ToString().Contains(".") ? (int)double.Parse(DecreaseMinutes.ToString()) : int.Parse(DecreaseMinutes.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += CategoryName + ":";
            key += DecreaseMinutes + ":";
            return key;
        }
    }
}
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
	public class GetRandomDisplayItemByUserIdRequest : Gs2Request<GetRandomDisplayItemByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string ShowcaseName { set; get; }
         public string DisplayItemName { set; get; }
         public string UserId { set; get; }
        public GetRandomDisplayItemByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetRandomDisplayItemByUserIdRequest WithShowcaseName(string showcaseName) {
            this.ShowcaseName = showcaseName;
            return this;
        }
        public GetRandomDisplayItemByUserIdRequest WithDisplayItemName(string displayItemName) {
            this.DisplayItemName = displayItemName;
            return this;
        }
        public GetRandomDisplayItemByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetRandomDisplayItemByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetRandomDisplayItemByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithShowcaseName(!data.Keys.Contains("showcaseName") || data["showcaseName"] == null ? null : data["showcaseName"].ToString())
                .WithDisplayItemName(!data.Keys.Contains("displayItemName") || data["displayItemName"] == null ? null : data["displayItemName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["showcaseName"] = ShowcaseName,
                ["displayItemName"] = DisplayItemName,
                ["userId"] = UserId,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += ShowcaseName + ":";
            key += DisplayItemName + ":";
            key += UserId + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply GetRandomDisplayItemByUserIdRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (GetRandomDisplayItemByUserIdRequest)x;
            return this;
        }
    }
}
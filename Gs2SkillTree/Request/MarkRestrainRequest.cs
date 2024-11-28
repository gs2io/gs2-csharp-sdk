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
using Gs2.Gs2SkillTree.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SkillTree.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class MarkRestrainRequest : Gs2Request<MarkRestrainRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string PropertyId { set; get; } = null!;
         public string[] NodeModelNames { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public MarkRestrainRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public MarkRestrainRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public MarkRestrainRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public MarkRestrainRequest WithNodeModelNames(string[] nodeModelNames) {
            this.NodeModelNames = nodeModelNames;
            return this;
        }

        public MarkRestrainRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MarkRestrainRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MarkRestrainRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithNodeModelNames(!data.Keys.Contains("nodeModelNames") || data["nodeModelNames"] == null || !data["nodeModelNames"].IsArray ? null : data["nodeModelNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData nodeModelNamesJsonData = null;
            if (NodeModelNames != null && NodeModelNames.Length > 0)
            {
                nodeModelNamesJsonData = new JsonData();
                foreach (var nodeModelName in NodeModelNames)
                {
                    nodeModelNamesJsonData.Add(nodeModelName);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["propertyId"] = PropertyId,
                ["nodeModelNames"] = nodeModelNamesJsonData,
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
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (NodeModelNames != null) {
                writer.WritePropertyName("nodeModelNames");
                writer.WriteArrayStart();
                foreach (var nodeModelName in NodeModelNames)
                {
                    writer.Write(nodeModelName.ToString());
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += PropertyId + ":";
            key += NodeModelNames + ":";
            return key;
        }
    }
}
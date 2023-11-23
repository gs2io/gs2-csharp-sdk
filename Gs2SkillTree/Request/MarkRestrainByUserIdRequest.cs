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
	public class MarkRestrainByUserIdRequest : Gs2Request<MarkRestrainByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string UserId { set; get; }
         public string[] NodeModelNames { set; get; }
        public string DuplicationAvoider { set; get; }
        public MarkRestrainByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public MarkRestrainByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public MarkRestrainByUserIdRequest WithNodeModelNames(string[] nodeModelNames) {
            this.NodeModelNames = nodeModelNames;
            return this;
        }

        public MarkRestrainByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MarkRestrainByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MarkRestrainByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithNodeModelNames(!data.Keys.Contains("nodeModelNames") || data["nodeModelNames"] == null || !data["nodeModelNames"].IsArray ? new string[]{} : data["nodeModelNames"].Cast<JsonData>().Select(v => {
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
                ["userId"] = UserId,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
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
            key += UserId + ":";
            key += NodeModelNames + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new MarkRestrainByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                NodeModelNames = NodeModelNames,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (MarkRestrainByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values MarkRestrainByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values MarkRestrainByUserIdRequest::userId");
            }
            if (NodeModelNames != y.NodeModelNames) {
                throw new ArithmeticException("mismatch parameter values MarkRestrainByUserIdRequest::nodeModelNames");
            }
            return new MarkRestrainByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                NodeModelNames = NodeModelNames,
            };
        }
    }
}
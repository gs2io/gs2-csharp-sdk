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
	public class ReleaseByUserIdRequest : Gs2Request<ReleaseByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string[] NodeModelNames { set; get; }
        public Gs2.Gs2SkillTree.Model.Config[] Config { set; get; }
        public string DuplicationAvoider { set; get; }

        public ReleaseByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public ReleaseByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public ReleaseByUserIdRequest WithNodeModelNames(string[] nodeModelNames) {
            this.NodeModelNames = nodeModelNames;
            return this;
        }

        public ReleaseByUserIdRequest WithConfig(Gs2.Gs2SkillTree.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public ReleaseByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReleaseByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReleaseByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithNodeModelNames(!data.Keys.Contains("nodeModelNames") || data["nodeModelNames"] == null ? new string[]{} : data["nodeModelNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null ? new Gs2.Gs2SkillTree.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2SkillTree.Model.Config.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData nodeModelNamesJsonData = null;
            if (NodeModelNames != null)
            {
                nodeModelNamesJsonData = new JsonData();
                foreach (var nodeModelName in NodeModelNames)
                {
                    nodeModelNamesJsonData.Add(nodeModelName);
                }
            }
            JsonData configJsonData = null;
            if (Config != null)
            {
                configJsonData = new JsonData();
                foreach (var confi in Config)
                {
                    configJsonData.Add(confi.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["nodeModelNames"] = nodeModelNamesJsonData,
                ["config"] = configJsonData,
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
            writer.WriteArrayStart();
            foreach (var nodeModelName in NodeModelNames)
            {
                writer.Write(nodeModelName.ToString());
            }
            writer.WriteArrayEnd();
            writer.WriteArrayStart();
            foreach (var confi in Config)
            {
                if (confi != null) {
                    confi.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += NodeModelNames + ":";
            key += Config + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply ReleaseByUserIdRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (ReleaseByUserIdRequest)x;
            return this;
        }
    }
}
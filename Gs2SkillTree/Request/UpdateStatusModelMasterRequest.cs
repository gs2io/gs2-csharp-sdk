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
	public class UpdateStatusModelMasterRequest : Gs2Request<UpdateStatusModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string NodeModelName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ReleaseConsumeActions { set; get; }
        public float? RestrainReturnRate { set; get; }
        public string[] PremiseNodeNames { set; get; }
        public UpdateStatusModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateStatusModelMasterRequest WithNodeModelName(string nodeModelName) {
            this.NodeModelName = nodeModelName;
            return this;
        }
        public UpdateStatusModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateStatusModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateStatusModelMasterRequest WithReleaseConsumeActions(Gs2.Core.Model.ConsumeAction[] releaseConsumeActions) {
            this.ReleaseConsumeActions = releaseConsumeActions;
            return this;
        }
        public UpdateStatusModelMasterRequest WithRestrainReturnRate(float? restrainReturnRate) {
            this.RestrainReturnRate = restrainReturnRate;
            return this;
        }
        public UpdateStatusModelMasterRequest WithPremiseNodeNames(string[] premiseNodeNames) {
            this.PremiseNodeNames = premiseNodeNames;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateStatusModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateStatusModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithNodeModelName(!data.Keys.Contains("nodeModelName") || data["nodeModelName"] == null ? null : data["nodeModelName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithReleaseConsumeActions(!data.Keys.Contains("releaseConsumeActions") || data["releaseConsumeActions"] == null ? new Gs2.Core.Model.ConsumeAction[]{} : data["releaseConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithRestrainReturnRate(!data.Keys.Contains("restrainReturnRate") || data["restrainReturnRate"] == null ? null : (float?)float.Parse(data["restrainReturnRate"].ToString()))
                .WithPremiseNodeNames(!data.Keys.Contains("premiseNodeNames") || data["premiseNodeNames"] == null ? new string[]{} : data["premiseNodeNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData releaseConsumeActionsJsonData = null;
            if (ReleaseConsumeActions != null)
            {
                releaseConsumeActionsJsonData = new JsonData();
                foreach (var releaseConsumeAction in ReleaseConsumeActions)
                {
                    releaseConsumeActionsJsonData.Add(releaseConsumeAction.ToJson());
                }
            }
            JsonData premiseNodeNamesJsonData = null;
            if (PremiseNodeNames != null)
            {
                premiseNodeNamesJsonData = new JsonData();
                foreach (var premiseNodeName in PremiseNodeNames)
                {
                    premiseNodeNamesJsonData.Add(premiseNodeName);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["nodeModelName"] = NodeModelName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["releaseConsumeActions"] = releaseConsumeActionsJsonData,
                ["restrainReturnRate"] = RestrainReturnRate,
                ["premiseNodeNames"] = premiseNodeNamesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (NodeModelName != null) {
                writer.WritePropertyName("nodeModelName");
                writer.Write(NodeModelName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            writer.WriteArrayStart();
            foreach (var releaseConsumeAction in ReleaseConsumeActions)
            {
                if (releaseConsumeAction != null) {
                    releaseConsumeAction.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (RestrainReturnRate != null) {
                writer.WritePropertyName("restrainReturnRate");
                writer.Write(float.Parse(RestrainReturnRate.ToString()));
            }
            writer.WriteArrayStart();
            foreach (var premiseNodeName in PremiseNodeNames)
            {
                writer.Write(premiseNodeName.ToString());
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += NodeModelName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ReleaseConsumeActions + ":";
            key += RestrainReturnRate + ":";
            key += PremiseNodeNames + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateStatusModelMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateStatusModelMasterRequest)x;
            return this;
        }
    }
}
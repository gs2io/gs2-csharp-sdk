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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SkillTree.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class NodeModel : IComparable
	{
        public string NodeModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ReleaseConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] ReturnAcquireActions { set; get; }
        public float? RestrainReturnRate { set; get; }
        public string[] PremiseNodeNames { set; get; }

        public NodeModel WithNodeModelId(string nodeModelId) {
            this.NodeModelId = nodeModelId;
            return this;
        }

        public NodeModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public NodeModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public NodeModel WithReleaseConsumeActions(Gs2.Core.Model.ConsumeAction[] releaseConsumeActions) {
            this.ReleaseConsumeActions = releaseConsumeActions;
            return this;
        }

        public NodeModel WithReturnAcquireActions(Gs2.Core.Model.AcquireAction[] returnAcquireActions) {
            this.ReturnAcquireActions = returnAcquireActions;
            return this;
        }

        public NodeModel WithRestrainReturnRate(float? restrainReturnRate) {
            this.RestrainReturnRate = restrainReturnRate;
            return this;
        }

        public NodeModel WithPremiseNodeNames(string[] premiseNodeNames) {
            this.PremiseNodeNames = premiseNodeNames;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):skillTree:(?<namespaceName>.+):model:(?<nodeModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):skillTree:(?<namespaceName>.+):model:(?<nodeModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):skillTree:(?<namespaceName>.+):model:(?<nodeModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _nodeModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):skillTree:(?<namespaceName>.+):model:(?<nodeModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNodeModelNameFromGrn(
            string grn
        )
        {
            var match = _nodeModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["nodeModelName"].Success)
            {
                return null;
            }
            return match.Groups["nodeModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static NodeModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new NodeModel()
                .WithNodeModelId(!data.Keys.Contains("nodeModelId") || data["nodeModelId"] == null ? null : data["nodeModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithReleaseConsumeActions(!data.Keys.Contains("releaseConsumeActions") || data["releaseConsumeActions"] == null ? new Gs2.Core.Model.ConsumeAction[]{} : data["releaseConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithReturnAcquireActions(!data.Keys.Contains("returnAcquireActions") || data["returnAcquireActions"] == null ? new Gs2.Core.Model.AcquireAction[]{} : data["returnAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithRestrainReturnRate(!data.Keys.Contains("restrainReturnRate") || data["restrainReturnRate"] == null ? null : (float?)float.Parse(data["restrainReturnRate"].ToString()))
                .WithPremiseNodeNames(!data.Keys.Contains("premiseNodeNames") || data["premiseNodeNames"] == null ? new string[]{} : data["premiseNodeNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
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
            JsonData returnAcquireActionsJsonData = null;
            if (ReturnAcquireActions != null)
            {
                returnAcquireActionsJsonData = new JsonData();
                foreach (var returnAcquireAction in ReturnAcquireActions)
                {
                    returnAcquireActionsJsonData.Add(returnAcquireAction.ToJson());
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
                ["nodeModelId"] = NodeModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["releaseConsumeActions"] = releaseConsumeActionsJsonData,
                ["returnAcquireActions"] = returnAcquireActionsJsonData,
                ["restrainReturnRate"] = RestrainReturnRate,
                ["premiseNodeNames"] = premiseNodeNamesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NodeModelId != null) {
                writer.WritePropertyName("nodeModelId");
                writer.Write(NodeModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ReleaseConsumeActions != null) {
                writer.WritePropertyName("releaseConsumeActions");
                writer.WriteArrayStart();
                foreach (var releaseConsumeAction in ReleaseConsumeActions)
                {
                    if (releaseConsumeAction != null) {
                        releaseConsumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ReturnAcquireActions != null) {
                writer.WritePropertyName("returnAcquireActions");
                writer.WriteArrayStart();
                foreach (var returnAcquireAction in ReturnAcquireActions)
                {
                    if (returnAcquireAction != null) {
                        returnAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (RestrainReturnRate != null) {
                writer.WritePropertyName("restrainReturnRate");
                writer.Write(float.Parse(RestrainReturnRate.ToString()));
            }
            if (PremiseNodeNames != null) {
                writer.WritePropertyName("premiseNodeNames");
                writer.WriteArrayStart();
                foreach (var premiseNodeName in PremiseNodeNames)
                {
                    if (premiseNodeName != null) {
                        writer.Write(premiseNodeName.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as NodeModel;
            var diff = 0;
            if (NodeModelId == null && NodeModelId == other.NodeModelId)
            {
                // null and null
            }
            else
            {
                diff += NodeModelId.CompareTo(other.NodeModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (ReleaseConsumeActions == null && ReleaseConsumeActions == other.ReleaseConsumeActions)
            {
                // null and null
            }
            else
            {
                diff += ReleaseConsumeActions.Length - other.ReleaseConsumeActions.Length;
                for (var i = 0; i < ReleaseConsumeActions.Length; i++)
                {
                    diff += ReleaseConsumeActions[i].CompareTo(other.ReleaseConsumeActions[i]);
                }
            }
            if (ReturnAcquireActions == null && ReturnAcquireActions == other.ReturnAcquireActions)
            {
                // null and null
            }
            else
            {
                diff += ReturnAcquireActions.Length - other.ReturnAcquireActions.Length;
                for (var i = 0; i < ReturnAcquireActions.Length; i++)
                {
                    diff += ReturnAcquireActions[i].CompareTo(other.ReturnAcquireActions[i]);
                }
            }
            if (RestrainReturnRate == null && RestrainReturnRate == other.RestrainReturnRate)
            {
                // null and null
            }
            else
            {
                diff += (int)(RestrainReturnRate - other.RestrainReturnRate);
            }
            if (PremiseNodeNames == null && PremiseNodeNames == other.PremiseNodeNames)
            {
                // null and null
            }
            else
            {
                diff += PremiseNodeNames.Length - other.PremiseNodeNames.Length;
                for (var i = 0; i < PremiseNodeNames.Length; i++)
                {
                    diff += PremiseNodeNames[i].CompareTo(other.PremiseNodeNames[i]);
                }
            }
            return diff;
        }
    }
}
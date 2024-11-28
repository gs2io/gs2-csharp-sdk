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
	public class NodeModelMaster : IComparable
	{
        public string NodeModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Core.Model.VerifyAction[] ReleaseVerifyActions { set; get; } = null!;
        public Gs2.Core.Model.ConsumeAction[] ReleaseConsumeActions { set; get; } = null!;
        public float? RestrainReturnRate { set; get; } = null!;
        public string[] PremiseNodeNames { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public NodeModelMaster WithNodeModelId(string nodeModelId) {
            this.NodeModelId = nodeModelId;
            return this;
        }
        public NodeModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public NodeModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public NodeModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public NodeModelMaster WithReleaseVerifyActions(Gs2.Core.Model.VerifyAction[] releaseVerifyActions) {
            this.ReleaseVerifyActions = releaseVerifyActions;
            return this;
        }
        public NodeModelMaster WithReleaseConsumeActions(Gs2.Core.Model.ConsumeAction[] releaseConsumeActions) {
            this.ReleaseConsumeActions = releaseConsumeActions;
            return this;
        }
        public NodeModelMaster WithRestrainReturnRate(float? restrainReturnRate) {
            this.RestrainReturnRate = restrainReturnRate;
            return this;
        }
        public NodeModelMaster WithPremiseNodeNames(string[] premiseNodeNames) {
            this.PremiseNodeNames = premiseNodeNames;
            return this;
        }
        public NodeModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public NodeModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public NodeModelMaster WithRevision(long? revision) {
            this.Revision = revision;
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
        public static NodeModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new NodeModelMaster()
                .WithNodeModelId(!data.Keys.Contains("nodeModelId") || data["nodeModelId"] == null ? null : data["nodeModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithReleaseVerifyActions(!data.Keys.Contains("releaseVerifyActions") || data["releaseVerifyActions"] == null || !data["releaseVerifyActions"].IsArray ? null : data["releaseVerifyActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithReleaseConsumeActions(!data.Keys.Contains("releaseConsumeActions") || data["releaseConsumeActions"] == null || !data["releaseConsumeActions"].IsArray ? null : data["releaseConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithRestrainReturnRate(!data.Keys.Contains("restrainReturnRate") || data["restrainReturnRate"] == null ? null : (float?)float.Parse(data["restrainReturnRate"].ToString()))
                .WithPremiseNodeNames(!data.Keys.Contains("premiseNodeNames") || data["premiseNodeNames"] == null || !data["premiseNodeNames"].IsArray ? null : data["premiseNodeNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData releaseVerifyActionsJsonData = null;
            if (ReleaseVerifyActions != null && ReleaseVerifyActions.Length > 0)
            {
                releaseVerifyActionsJsonData = new JsonData();
                foreach (var releaseVerifyAction in ReleaseVerifyActions)
                {
                    releaseVerifyActionsJsonData.Add(releaseVerifyAction.ToJson());
                }
            }
            JsonData releaseConsumeActionsJsonData = null;
            if (ReleaseConsumeActions != null && ReleaseConsumeActions.Length > 0)
            {
                releaseConsumeActionsJsonData = new JsonData();
                foreach (var releaseConsumeAction in ReleaseConsumeActions)
                {
                    releaseConsumeActionsJsonData.Add(releaseConsumeAction.ToJson());
                }
            }
            JsonData premiseNodeNamesJsonData = null;
            if (PremiseNodeNames != null && PremiseNodeNames.Length > 0)
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
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["releaseVerifyActions"] = releaseVerifyActionsJsonData,
                ["releaseConsumeActions"] = releaseConsumeActionsJsonData,
                ["restrainReturnRate"] = RestrainReturnRate,
                ["premiseNodeNames"] = premiseNodeNamesJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ReleaseVerifyActions != null) {
                writer.WritePropertyName("releaseVerifyActions");
                writer.WriteArrayStart();
                foreach (var releaseVerifyAction in ReleaseVerifyActions)
                {
                    if (releaseVerifyAction != null) {
                        releaseVerifyAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as NodeModelMaster;
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
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (ReleaseVerifyActions == null && ReleaseVerifyActions == other.ReleaseVerifyActions)
            {
                // null and null
            }
            else
            {
                diff += ReleaseVerifyActions.Length - other.ReleaseVerifyActions.Length;
                for (var i = 0; i < ReleaseVerifyActions.Length; i++)
                {
                    diff += ReleaseVerifyActions[i].CompareTo(other.ReleaseVerifyActions[i]);
                }
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
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (NodeModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.nodeModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (ReleaseVerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.releaseVerifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (ReleaseConsumeActions.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.releaseConsumeActions.error.tooFew"),
                    });
                }
                if (ReleaseConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.releaseConsumeActions.error.tooMany"),
                    });
                }
            }
            {
                if (RestrainReturnRate < 0.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.restrainReturnRate.error.invalid"),
                    });
                }
                if (RestrainReturnRate > 1.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.restrainReturnRate.error.invalid"),
                    });
                }
            }
            {
                if (PremiseNodeNames.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.premiseNodeNames.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("nodeModelMaster", "skillTree.nodeModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new NodeModelMaster {
                NodeModelId = NodeModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                ReleaseVerifyActions = ReleaseVerifyActions.Clone() as Gs2.Core.Model.VerifyAction[],
                ReleaseConsumeActions = ReleaseConsumeActions.Clone() as Gs2.Core.Model.ConsumeAction[],
                RestrainReturnRate = RestrainReturnRate,
                PremiseNodeNames = PremiseNodeNames.Clone() as string[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
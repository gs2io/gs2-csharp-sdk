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
 *
 * deny overwrite
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

namespace Gs2.Gs2Version.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class VersionModelMaster : IComparable
	{
        public string VersionModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string Scope { set; get; } = null!;
        public string Type { set; get; } = null!;
        public Gs2.Gs2Version.Model.Version_ CurrentVersion { set; get; } = null!;
        public Gs2.Gs2Version.Model.Version_ WarningVersion { set; get; } = null!;
        public Gs2.Gs2Version.Model.Version_ ErrorVersion { set; get; } = null!;
        public Gs2.Gs2Version.Model.ScheduleVersion[] ScheduleVersions { set; get; } = null!;
        public bool? NeedSignature { set; get; } = null!;
        public string SignatureKeyId { set; get; } = null!;
        public string ApproveRequirement { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public VersionModelMaster WithVersionModelId(string versionModelId) {
            this.VersionModelId = versionModelId;
            return this;
        }
        public VersionModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public VersionModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public VersionModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public VersionModelMaster WithScope(string scope) {
            this.Scope = scope;
            return this;
        }
        public VersionModelMaster WithType(string type) {
            this.Type = type;
            return this;
        }
        public VersionModelMaster WithCurrentVersion(Gs2.Gs2Version.Model.Version_ currentVersion) {
            this.CurrentVersion = currentVersion;
            return this;
        }
        public VersionModelMaster WithWarningVersion(Gs2.Gs2Version.Model.Version_ warningVersion) {
            this.WarningVersion = warningVersion;
            return this;
        }
        public VersionModelMaster WithErrorVersion(Gs2.Gs2Version.Model.Version_ errorVersion) {
            this.ErrorVersion = errorVersion;
            return this;
        }
        public VersionModelMaster WithScheduleVersions(Gs2.Gs2Version.Model.ScheduleVersion[] scheduleVersions) {
            this.ScheduleVersions = scheduleVersions;
            return this;
        }
        public VersionModelMaster WithNeedSignature(bool? needSignature) {
            this.NeedSignature = needSignature;
            return this;
        }
        public VersionModelMaster WithSignatureKeyId(string signatureKeyId) {
            this.SignatureKeyId = signatureKeyId;
            return this;
        }
        public VersionModelMaster WithApproveRequirement(string approveRequirement) {
            this.ApproveRequirement = approveRequirement;
            return this;
        }
        public VersionModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public VersionModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public VersionModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):model:version:(?<versionName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):model:version:(?<versionName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):model:version:(?<versionName>.+)",
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

        private static System.Text.RegularExpressions.Regex _versionNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):model:version:(?<versionName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetVersionNameFromGrn(
            string grn
        )
        {
            var match = _versionNameRegex.Match(grn);
            if (!match.Success || !match.Groups["versionName"].Success)
            {
                return null;
            }
            return match.Groups["versionName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VersionModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VersionModelMaster()
                .WithVersionModelId(!data.Keys.Contains("versionModelId") || data["versionModelId"] == null ? null : data["versionModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithCurrentVersion(!data.Keys.Contains("currentVersion") || data["currentVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["currentVersion"]))
                .WithWarningVersion(!data.Keys.Contains("warningVersion") || data["warningVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["warningVersion"]))
                .WithErrorVersion(!data.Keys.Contains("errorVersion") || data["errorVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["errorVersion"]))
                .WithScheduleVersions(!data.Keys.Contains("scheduleVersions") || data["scheduleVersions"] == null || !data["scheduleVersions"].IsArray ? null : data["scheduleVersions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Version.Model.ScheduleVersion.FromJson(v);
                }).ToArray())
                .WithNeedSignature(!data.Keys.Contains("needSignature") || data["needSignature"] == null ? null : (bool?)bool.Parse(data["needSignature"].ToString()))
                .WithSignatureKeyId(!data.Keys.Contains("signatureKeyId") || data["signatureKeyId"] == null ? null : data["signatureKeyId"].ToString())
                .WithApproveRequirement(!data.Keys.Contains("approveRequirement") || data["approveRequirement"] == null ? null : data["approveRequirement"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData scheduleVersionsJsonData = null;
            if (ScheduleVersions != null && ScheduleVersions.Length > 0)
            {
                scheduleVersionsJsonData = new JsonData();
                foreach (var scheduleVersion in ScheduleVersions)
                {
                    scheduleVersionsJsonData.Add(scheduleVersion.ToJson());
                }
            }
            return new JsonData {
                ["versionModelId"] = VersionModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["scope"] = Scope,
                ["type"] = Type,
                ["currentVersion"] = CurrentVersion?.ToJson(),
                ["warningVersion"] = WarningVersion?.ToJson(),
                ["errorVersion"] = ErrorVersion?.ToJson(),
                ["scheduleVersions"] = scheduleVersionsJsonData,
                ["needSignature"] = NeedSignature,
                ["signatureKeyId"] = SignatureKeyId,
                ["approveRequirement"] = ApproveRequirement,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (VersionModelId != null) {
                writer.WritePropertyName("versionModelId");
                writer.Write(VersionModelId.ToString());
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
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (CurrentVersion != null) {
                writer.WritePropertyName("currentVersion");
                CurrentVersion.WriteJson(writer);
            }
            if (WarningVersion != null) {
                writer.WritePropertyName("warningVersion");
                WarningVersion.WriteJson(writer);
            }
            if (ErrorVersion != null) {
                writer.WritePropertyName("errorVersion");
                ErrorVersion.WriteJson(writer);
            }
            if (ScheduleVersions != null) {
                writer.WritePropertyName("scheduleVersions");
                writer.WriteArrayStart();
                foreach (var scheduleVersion in ScheduleVersions)
                {
                    if (scheduleVersion != null) {
                        scheduleVersion.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (NeedSignature != null) {
                writer.WritePropertyName("needSignature");
                writer.Write(bool.Parse(NeedSignature.ToString()));
            }
            if (SignatureKeyId != null) {
                writer.WritePropertyName("signatureKeyId");
                writer.Write(SignatureKeyId.ToString());
            }
            if (ApproveRequirement != null) {
                writer.WritePropertyName("approveRequirement");
                writer.Write(ApproveRequirement.ToString());
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
            var other = obj as VersionModelMaster;
            var diff = 0;
            if (VersionModelId == null && VersionModelId == other.VersionModelId)
            {
                // null and null
            }
            else
            {
                diff += VersionModelId.CompareTo(other.VersionModelId);
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
            if (Scope == null && Scope == other.Scope)
            {
                // null and null
            }
            else
            {
                diff += Scope.CompareTo(other.Scope);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += Type.CompareTo(other.Type);
            }
            if (CurrentVersion == null && CurrentVersion == other.CurrentVersion)
            {
                // null and null
            }
            else
            {
                diff += CurrentVersion.CompareTo(other.CurrentVersion);
            }
            if (WarningVersion == null && WarningVersion == other.WarningVersion)
            {
                // null and null
            }
            else
            {
                diff += WarningVersion.CompareTo(other.WarningVersion);
            }
            if (ErrorVersion == null && ErrorVersion == other.ErrorVersion)
            {
                // null and null
            }
            else
            {
                diff += ErrorVersion.CompareTo(other.ErrorVersion);
            }
            if (ScheduleVersions == null && ScheduleVersions == other.ScheduleVersions)
            {
                // null and null
            }
            else
            {
                diff += ScheduleVersions.Length - other.ScheduleVersions.Length;
                for (var i = 0; i < ScheduleVersions.Length; i++)
                {
                    diff += ScheduleVersions[i].CompareTo(other.ScheduleVersions[i]);
                }
            }
            if (NeedSignature == null && NeedSignature == other.NeedSignature)
            {
                // null and null
            }
            else
            {
                diff += NeedSignature == other.NeedSignature ? 0 : 1;
            }
            if (SignatureKeyId == null && SignatureKeyId == other.SignatureKeyId)
            {
                // null and null
            }
            else
            {
                diff += SignatureKeyId.CompareTo(other.SignatureKeyId);
            }
            if (ApproveRequirement == null && ApproveRequirement == other.ApproveRequirement)
            {
                // null and null
            }
            else
            {
                diff += ApproveRequirement.CompareTo(other.ApproveRequirement);
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
                if (VersionModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.versionModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                switch (Scope) {
                    case "passive":
                    case "active":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("versionModelMaster", "version.versionModelMaster.scope.error.invalid"),
                        });
                }
            }
            {
                switch (Type) {
                    case "simple":
                    case "schedule":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("versionModelMaster", "version.versionModelMaster.type.error.invalid"),
                        });
                }
            }
            if (Type == "simple" && Scope == "active") {
            }
            if (Type == "simple") {
            }
            if (Type == "simple") {
            }
            {
                if (ScheduleVersions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.scheduleVersions.error.tooMany"),
                    });
                }
            }
            if (Scope == "passive") {
            }
            if (NeedSignature ?? false) {
                if (SignatureKeyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.signatureKeyId.error.tooLong"),
                    });
                }
            }
            if (Scope == "active") {
                switch (ApproveRequirement) {
                    case "required":
                    case "optional":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("versionModelMaster", "version.versionModelMaster.approveRequirement.error.invalid"),
                        });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModelMaster", "version.versionModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new VersionModelMaster {
                VersionModelId = VersionModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                Scope = Scope,
                Type = Type,
                CurrentVersion = CurrentVersion.Clone() as Gs2.Gs2Version.Model.Version_,
                WarningVersion = WarningVersion.Clone() as Gs2.Gs2Version.Model.Version_,
                ErrorVersion = ErrorVersion.Clone() as Gs2.Gs2Version.Model.Version_,
                ScheduleVersions = ScheduleVersions?.Clone() as Gs2.Gs2Version.Model.ScheduleVersion[],
                NeedSignature = NeedSignature,
                SignatureKeyId = SignatureKeyId,
                ApproveRequirement = ApproveRequirement,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
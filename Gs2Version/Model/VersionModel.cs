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
 * deny overwrite
 */

#pragma warning disable CS0618 // Obsolete with a message

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
/* diff --- start
	public partial class VersionModel : IComparable
 diff --- end */
	public class VersionModel : IComparable /* diff +++ */
	{
/* diff --- start
        public string VersionModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string Scope { set; get; }
        public string Type { set; get; }
        public Gs2.Gs2Version.Model.Version_ CurrentVersion { set; get; }
        public Gs2.Gs2Version.Model.Version_ WarningVersion { set; get; }
        public Gs2.Gs2Version.Model.Version_ ErrorVersion { set; get; }
        public Gs2.Gs2Version.Model.ScheduleVersion[] ScheduleVersions { set; get; }
        public bool? NeedSignature { set; get; }
        public string SignatureKeyId { set; get; }
        public string ApproveRequirement { set; get; }
 diff --- end */
/* diff +++ start */
        public string VersionModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
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
/* diff +++ end */
        public VersionModel WithVersionModelId(string versionModelId) {
            this.VersionModelId = versionModelId;
            return this;
        }
        public VersionModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public VersionModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public VersionModel WithScope(string scope) {
            this.Scope = scope;
            return this;
        }
        public VersionModel WithType(string type) {
            this.Type = type;
            return this;
        }
        public VersionModel WithCurrentVersion(Gs2.Gs2Version.Model.Version_ currentVersion) {
            this.CurrentVersion = currentVersion;
            return this;
        }
        public VersionModel WithWarningVersion(Gs2.Gs2Version.Model.Version_ warningVersion) {
            this.WarningVersion = warningVersion;
            return this;
        }
        public VersionModel WithErrorVersion(Gs2.Gs2Version.Model.Version_ errorVersion) {
            this.ErrorVersion = errorVersion;
            return this;
        }
        public VersionModel WithScheduleVersions(Gs2.Gs2Version.Model.ScheduleVersion[] scheduleVersions) {
            this.ScheduleVersions = scheduleVersions;
            return this;
        }
        public VersionModel WithNeedSignature(bool? needSignature) {
            this.NeedSignature = needSignature;
            return this;
        }
        public VersionModel WithSignatureKeyId(string signatureKeyId) {
            this.SignatureKeyId = signatureKeyId;
            return this;
        }
        public VersionModel WithApproveRequirement(string approveRequirement) {
            this.ApproveRequirement = approveRequirement;
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
        public static VersionModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VersionModel()
                .WithVersionModelId(!data.Keys.Contains("versionModelId") || data["versionModelId"] == null ? null : data["versionModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
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
                .WithApproveRequirement(!data.Keys.Contains("approveRequirement") || data["approveRequirement"] == null ? null : data["approveRequirement"].ToString());
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as VersionModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type VersionModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(VersionModelId, other.VersionModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Scope, other.Scope);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Type, other.Type);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CurrentVersion, other.CurrentVersion);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(WarningVersion, other.WarningVersion);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ErrorVersion, other.ErrorVersion);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(ScheduleVersions, other.ScheduleVersions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(NeedSignature, other.NeedSignature);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(SignatureKeyId, other.SignatureKeyId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ApproveRequirement, other.ApproveRequirement);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (VersionModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModel", "version.versionModel.versionModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModel", "version.versionModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModel", "version.versionModel.metadata.error.tooLong"),
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
                            new RequestError("versionModel", "version.versionModel.scope.error.invalid"),
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
                            new RequestError("versionModel", "version.versionModel.type.error.invalid"),
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
                        new RequestError("versionModel", "version.versionModel.scheduleVersions.error.tooMany"),
                    });
                }
            }
            if (Scope == "passive") {
            }
/* diff --- start
            if (NeedSignature) {
 diff --- end */
            if (NeedSignature ?? false) { /* diff +++ */
                if (SignatureKeyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("versionModel", "version.versionModel.signatureKeyId.error.tooLong"),
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
                            new RequestError("versionModel", "version.versionModel.approveRequirement.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new VersionModel {
                VersionModelId = VersionModelId,
                Name = Name,
                Metadata = Metadata,
                Scope = Scope,
                Type = Type,
                CurrentVersion = CurrentVersion?.Clone() as Gs2.Gs2Version.Model.Version_,
                WarningVersion = WarningVersion?.Clone() as Gs2.Gs2Version.Model.Version_,
                ErrorVersion = ErrorVersion?.Clone() as Gs2.Gs2Version.Model.Version_,
                ScheduleVersions = ScheduleVersions?.Clone() as Gs2.Gs2Version.Model.ScheduleVersion[],
                NeedSignature = NeedSignature,
                SignatureKeyId = SignatureKeyId,
                ApproveRequirement = ApproveRequirement,
            };
        }
    }
}
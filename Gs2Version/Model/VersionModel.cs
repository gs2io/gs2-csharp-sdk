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

namespace Gs2.Gs2Version.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class VersionModel : IComparable
	{
        public string VersionModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Version.Model.Version_ WarningVersion { set; get; }
        public Gs2.Gs2Version.Model.Version_ ErrorVersion { set; get; }
        public string Scope { set; get; }
        public Gs2.Gs2Version.Model.Version_ CurrentVersion { set; get; }
        public bool? NeedSignature { set; get; }
        public string SignatureKeyId { set; get; }

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

        public VersionModel WithWarningVersion(Gs2.Gs2Version.Model.Version_ warningVersion) {
            this.WarningVersion = warningVersion;
            return this;
        }

        public VersionModel WithErrorVersion(Gs2.Gs2Version.Model.Version_ errorVersion) {
            this.ErrorVersion = errorVersion;
            return this;
        }

        public VersionModel WithScope(string scope) {
            this.Scope = scope;
            return this;
        }

        public VersionModel WithCurrentVersion(Gs2.Gs2Version.Model.Version_ currentVersion) {
            this.CurrentVersion = currentVersion;
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
                .WithWarningVersion(!data.Keys.Contains("warningVersion") || data["warningVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["warningVersion"]))
                .WithErrorVersion(!data.Keys.Contains("errorVersion") || data["errorVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["errorVersion"]))
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithCurrentVersion(!data.Keys.Contains("currentVersion") || data["currentVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["currentVersion"]))
                .WithNeedSignature(!data.Keys.Contains("needSignature") || data["needSignature"] == null ? null : (bool?)bool.Parse(data["needSignature"].ToString()))
                .WithSignatureKeyId(!data.Keys.Contains("signatureKeyId") || data["signatureKeyId"] == null ? null : data["signatureKeyId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["versionModelId"] = VersionModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["warningVersion"] = WarningVersion?.ToJson(),
                ["errorVersion"] = ErrorVersion?.ToJson(),
                ["scope"] = Scope,
                ["currentVersion"] = CurrentVersion?.ToJson(),
                ["needSignature"] = NeedSignature,
                ["signatureKeyId"] = SignatureKeyId,
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
            if (WarningVersion != null) {
                writer.WritePropertyName("warningVersion");
                WarningVersion.WriteJson(writer);
            }
            if (ErrorVersion != null) {
                writer.WritePropertyName("errorVersion");
                ErrorVersion.WriteJson(writer);
            }
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            if (CurrentVersion != null) {
                writer.WritePropertyName("currentVersion");
                CurrentVersion.WriteJson(writer);
            }
            if (NeedSignature != null) {
                writer.WritePropertyName("needSignature");
                writer.Write(bool.Parse(NeedSignature.ToString()));
            }
            if (SignatureKeyId != null) {
                writer.WritePropertyName("signatureKeyId");
                writer.Write(SignatureKeyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as VersionModel;
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
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
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
            if (Scope == null && Scope == other.Scope)
            {
                // null and null
            }
            else
            {
                diff += Scope.CompareTo(other.Scope);
            }
            if (CurrentVersion == null && CurrentVersion == other.CurrentVersion)
            {
                // null and null
            }
            else
            {
                diff += CurrentVersion.CompareTo(other.CurrentVersion);
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
            return diff;
        }
    }
}
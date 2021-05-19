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
using UnityEngine.Scripting;

namespace Gs2.Gs2Version.Model
{
	[Preserve]
	public class VersionModelMaster : IComparable
	{

        /** バージョンマスター */
        public string versionModelId { set; get; }

        /**
         * バージョンマスターを設定
         *
         * @param versionModelId バージョンマスター
         * @return this
         */
        public VersionModelMaster WithVersionModelId(string versionModelId) {
            this.versionModelId = versionModelId;
            return this;
        }

        /** バージョン名 */
        public string name { set; get; }

        /**
         * バージョン名を設定
         *
         * @param name バージョン名
         * @return this
         */
        public VersionModelMaster WithName(string name) {
            this.name = name;
            return this;
        }

        /** バージョンマスターの説明 */
        public string description { set; get; }

        /**
         * バージョンマスターの説明を設定
         *
         * @param description バージョンマスターの説明
         * @return this
         */
        public VersionModelMaster WithDescription(string description) {
            this.description = description;
            return this;
        }

        /** バージョンのメタデータ */
        public string metadata { set; get; }

        /**
         * バージョンのメタデータを設定
         *
         * @param metadata バージョンのメタデータ
         * @return this
         */
        public VersionModelMaster WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }

        /** バージョンアップを促すバージョン */
        public Gs2.Gs2Version.Model.Version_ warningVersion { set; get; }

        /**
         * バージョンアップを促すバージョンを設定
         *
         * @param warningVersion バージョンアップを促すバージョン
         * @return this
         */
        public VersionModelMaster WithWarningVersion(Gs2.Gs2Version.Model.Version_ warningVersion) {
            this.warningVersion = warningVersion;
            return this;
        }

        /** バージョンチェックを蹴るバージョン */
        public Gs2.Gs2Version.Model.Version_ errorVersion { set; get; }

        /**
         * バージョンチェックを蹴るバージョンを設定
         *
         * @param errorVersion バージョンチェックを蹴るバージョン
         * @return this
         */
        public VersionModelMaster WithErrorVersion(Gs2.Gs2Version.Model.Version_ errorVersion) {
            this.errorVersion = errorVersion;
            return this;
        }

        /** 判定に使用するバージョン値の種類 */
        public string scope { set; get; }

        /**
         * 判定に使用するバージョン値の種類を設定
         *
         * @param scope 判定に使用するバージョン値の種類
         * @return this
         */
        public VersionModelMaster WithScope(string scope) {
            this.scope = scope;
            return this;
        }

        /** 現在のバージョン */
        public Gs2.Gs2Version.Model.Version_ currentVersion { set; get; }

        /**
         * 現在のバージョンを設定
         *
         * @param currentVersion 現在のバージョン
         * @return this
         */
        public VersionModelMaster WithCurrentVersion(Gs2.Gs2Version.Model.Version_ currentVersion) {
            this.currentVersion = currentVersion;
            return this;
        }

        /** 判定するバージョン値に署名検証を必要とするか */
        public bool? needSignature { set; get; }

        /**
         * 判定するバージョン値に署名検証を必要とするかを設定
         *
         * @param needSignature 判定するバージョン値に署名検証を必要とするか
         * @return this
         */
        public VersionModelMaster WithNeedSignature(bool? needSignature) {
            this.needSignature = needSignature;
            return this;
        }

        /** 署名検証に使用する暗号鍵 のGRN */
        public string signatureKeyId { set; get; }

        /**
         * 署名検証に使用する暗号鍵 のGRNを設定
         *
         * @param signatureKeyId 署名検証に使用する暗号鍵 のGRN
         * @return this
         */
        public VersionModelMaster WithSignatureKeyId(string signatureKeyId) {
            this.signatureKeyId = signatureKeyId;
            return this;
        }

        /** 作成日時 */
        public long? createdAt { set; get; }

        /**
         * 作成日時を設定
         *
         * @param createdAt 作成日時
         * @return this
         */
        public VersionModelMaster WithCreatedAt(long? createdAt) {
            this.createdAt = createdAt;
            return this;
        }

        /** 最終更新日時 */
        public long? updatedAt { set; get; }

        /**
         * 最終更新日時を設定
         *
         * @param updatedAt 最終更新日時
         * @return this
         */
        public VersionModelMaster WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.versionModelId != null)
            {
                writer.WritePropertyName("versionModelId");
                writer.Write(this.versionModelId);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.description != null)
            {
                writer.WritePropertyName("description");
                writer.Write(this.description);
            }
            if(this.metadata != null)
            {
                writer.WritePropertyName("metadata");
                writer.Write(this.metadata);
            }
            if(this.warningVersion != null)
            {
                writer.WritePropertyName("warningVersion");
                this.warningVersion.WriteJson(writer);
            }
            if(this.errorVersion != null)
            {
                writer.WritePropertyName("errorVersion");
                this.errorVersion.WriteJson(writer);
            }
            if(this.scope != null)
            {
                writer.WritePropertyName("scope");
                writer.Write(this.scope);
            }
            if(this.currentVersion != null)
            {
                writer.WritePropertyName("currentVersion");
                this.currentVersion.WriteJson(writer);
            }
            if(this.needSignature.HasValue)
            {
                writer.WritePropertyName("needSignature");
                writer.Write(this.needSignature.Value);
            }
            if(this.signatureKeyId != null)
            {
                writer.WritePropertyName("signatureKeyId");
                writer.Write(this.signatureKeyId);
            }
            if(this.createdAt.HasValue)
            {
                writer.WritePropertyName("createdAt");
                writer.Write(this.createdAt.Value);
            }
            if(this.updatedAt.HasValue)
            {
                writer.WritePropertyName("updatedAt");
                writer.Write(this.updatedAt.Value);
            }
            writer.WriteObjectEnd();
        }

    public static string GetVersionNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):model:version:(?<versionName>.*)");
        if (!match.Groups["versionName"].Success)
        {
            return null;
        }
        return match.Groups["versionName"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):model:version:(?<versionName>.*)");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):model:version:(?<versionName>.*)");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):model:version:(?<versionName>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static VersionModelMaster FromDict(JsonData data)
        {
            return new VersionModelMaster()
                .WithVersionModelId(data.Keys.Contains("versionModelId") && data["versionModelId"] != null ? data["versionModelId"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithDescription(data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString() : null)
                .WithMetadata(data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString() : null)
                .WithWarningVersion(data.Keys.Contains("warningVersion") && data["warningVersion"] != null ? Gs2.Gs2Version.Model.Version_.FromDict(data["warningVersion"]) : null)
                .WithErrorVersion(data.Keys.Contains("errorVersion") && data["errorVersion"] != null ? Gs2.Gs2Version.Model.Version_.FromDict(data["errorVersion"]) : null)
                .WithScope(data.Keys.Contains("scope") && data["scope"] != null ? data["scope"].ToString() : null)
                .WithCurrentVersion(data.Keys.Contains("currentVersion") && data["currentVersion"] != null ? Gs2.Gs2Version.Model.Version_.FromDict(data["currentVersion"]) : null)
                .WithNeedSignature(data.Keys.Contains("needSignature") && data["needSignature"] != null ? (bool?)bool.Parse(data["needSignature"].ToString()) : null)
                .WithSignatureKeyId(data.Keys.Contains("signatureKeyId") && data["signatureKeyId"] != null ? data["signatureKeyId"].ToString() : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as VersionModelMaster;
            var diff = 0;
            if (versionModelId == null && versionModelId == other.versionModelId)
            {
                // null and null
            }
            else
            {
                diff += versionModelId.CompareTo(other.versionModelId);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (description == null && description == other.description)
            {
                // null and null
            }
            else
            {
                diff += description.CompareTo(other.description);
            }
            if (metadata == null && metadata == other.metadata)
            {
                // null and null
            }
            else
            {
                diff += metadata.CompareTo(other.metadata);
            }
            if (warningVersion == null && warningVersion == other.warningVersion)
            {
                // null and null
            }
            else
            {
                diff += warningVersion.CompareTo(other.warningVersion);
            }
            if (errorVersion == null && errorVersion == other.errorVersion)
            {
                // null and null
            }
            else
            {
                diff += errorVersion.CompareTo(other.errorVersion);
            }
            if (scope == null && scope == other.scope)
            {
                // null and null
            }
            else
            {
                diff += scope.CompareTo(other.scope);
            }
            if (currentVersion == null && currentVersion == other.currentVersion)
            {
                // null and null
            }
            else
            {
                diff += currentVersion.CompareTo(other.currentVersion);
            }
            if (needSignature == null && needSignature == other.needSignature)
            {
                // null and null
            }
            else
            {
                diff += needSignature == other.needSignature ? 0 : 1;
            }
            if (signatureKeyId == null && signatureKeyId == other.signatureKeyId)
            {
                // null and null
            }
            else
            {
                diff += signatureKeyId.CompareTo(other.signatureKeyId);
            }
            if (createdAt == null && createdAt == other.createdAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(createdAt - other.createdAt);
            }
            if (updatedAt == null && updatedAt == other.updatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(updatedAt - other.updatedAt);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["versionModelId"] = versionModelId;
            data["name"] = name;
            data["description"] = description;
            data["metadata"] = metadata;
            data["warningVersion"] = warningVersion.ToDict();
            data["errorVersion"] = errorVersion.ToDict();
            data["scope"] = scope;
            data["currentVersion"] = currentVersion.ToDict();
            data["needSignature"] = needSignature;
            data["signatureKeyId"] = signatureKeyId;
            data["createdAt"] = createdAt;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}
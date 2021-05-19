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
	public class AcceptVersion : IComparable
	{

        /** 承認したバージョン */
        public string acceptVersionId { set; get; }

        /**
         * 承認したバージョンを設定
         *
         * @param acceptVersionId 承認したバージョン
         * @return this
         */
        public AcceptVersion WithAcceptVersionId(string acceptVersionId) {
            this.acceptVersionId = acceptVersionId;
            return this;
        }

        /** 承認したバージョン名 */
        public string versionName { set; get; }

        /**
         * 承認したバージョン名を設定
         *
         * @param versionName 承認したバージョン名
         * @return this
         */
        public AcceptVersion WithVersionName(string versionName) {
            this.versionName = versionName;
            return this;
        }

        /** ユーザーID */
        public string userId { set; get; }

        /**
         * ユーザーIDを設定
         *
         * @param userId ユーザーID
         * @return this
         */
        public AcceptVersion WithUserId(string userId) {
            this.userId = userId;
            return this;
        }

        /** 承認したバージョン */
        public Gs2.Gs2Version.Model.Version_ version { set; get; }

        /**
         * 承認したバージョンを設定
         *
         * @param version 承認したバージョン
         * @return this
         */
        public AcceptVersion WithVersion(Gs2.Gs2Version.Model.Version_ version) {
            this.version = version;
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
        public AcceptVersion WithCreatedAt(long? createdAt) {
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
        public AcceptVersion WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.acceptVersionId != null)
            {
                writer.WritePropertyName("acceptVersionId");
                writer.Write(this.acceptVersionId);
            }
            if(this.versionName != null)
            {
                writer.WritePropertyName("versionName");
                writer.Write(this.versionName);
            }
            if(this.userId != null)
            {
                writer.WritePropertyName("userId");
                writer.Write(this.userId);
            }
            if(this.version != null)
            {
                writer.WritePropertyName("version");
                this.version.WriteJson(writer);
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
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):user:(?<userId>.*):version:(?<versionName>.*):accept:");
        if (!match.Groups["versionName"].Success)
        {
            return null;
        }
        return match.Groups["versionName"].Value;
    }

    public static string GetUserIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):user:(?<userId>.*):version:(?<versionName>.*):accept:");
        if (!match.Groups["userId"].Success)
        {
            return null;
        }
        return match.Groups["userId"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):user:(?<userId>.*):version:(?<versionName>.*):accept:");
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
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):user:(?<userId>.*):version:(?<versionName>.*):accept:");
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
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):version:(?<namespaceName>.*):user:(?<userId>.*):version:(?<versionName>.*):accept:");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static AcceptVersion FromDict(JsonData data)
        {
            return new AcceptVersion()
                .WithAcceptVersionId(data.Keys.Contains("acceptVersionId") && data["acceptVersionId"] != null ? data["acceptVersionId"].ToString() : null)
                .WithVersionName(data.Keys.Contains("versionName") && data["versionName"] != null ? data["versionName"].ToString() : null)
                .WithUserId(data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString() : null)
                .WithVersion(data.Keys.Contains("version") && data["version"] != null ? Gs2.Gs2Version.Model.Version_.FromDict(data["version"]) : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as AcceptVersion;
            var diff = 0;
            if (acceptVersionId == null && acceptVersionId == other.acceptVersionId)
            {
                // null and null
            }
            else
            {
                diff += acceptVersionId.CompareTo(other.acceptVersionId);
            }
            if (versionName == null && versionName == other.versionName)
            {
                // null and null
            }
            else
            {
                diff += versionName.CompareTo(other.versionName);
            }
            if (userId == null && userId == other.userId)
            {
                // null and null
            }
            else
            {
                diff += userId.CompareTo(other.userId);
            }
            if (version == null && version == other.version)
            {
                // null and null
            }
            else
            {
                diff += version.CompareTo(other.version);
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
            data["acceptVersionId"] = acceptVersionId;
            data["versionName"] = versionName;
            data["userId"] = userId;
            data["version"] = version.ToDict();
            data["createdAt"] = createdAt;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}
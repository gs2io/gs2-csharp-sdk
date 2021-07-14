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
        public string AcceptVersionId { set; get; }
        public string VersionName { set; get; }
        public string UserId { set; get; }
        public Gs2.Gs2Version.Model.Version_ Version { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public AcceptVersion WithAcceptVersionId(string acceptVersionId) {
            this.AcceptVersionId = acceptVersionId;
            return this;
        }

        public AcceptVersion WithVersionName(string versionName) {
            this.VersionName = versionName;
            return this;
        }

        public AcceptVersion WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public AcceptVersion WithVersion(Gs2.Gs2Version.Model.Version_ version) {
            this.Version = version;
            return this;
        }

        public AcceptVersion WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public AcceptVersion WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

    	[Preserve]
        public static AcceptVersion FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcceptVersion()
                .WithAcceptVersionId(!data.Keys.Contains("acceptVersionId") || data["acceptVersionId"] == null ? null : data["acceptVersionId"].ToString())
                .WithVersionName(!data.Keys.Contains("versionName") || data["versionName"] == null ? null : data["versionName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithVersion(!data.Keys.Contains("version") || data["version"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["version"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["acceptVersionId"] = AcceptVersionId,
                ["versionName"] = VersionName,
                ["userId"] = UserId,
                ["version"] = Version?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AcceptVersionId != null) {
                writer.WritePropertyName("acceptVersionId");
                writer.Write(AcceptVersionId.ToString());
            }
            if (VersionName != null) {
                writer.WritePropertyName("versionName");
                writer.Write(VersionName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Version != null) {
                writer.WritePropertyName("version");
                Version.WriteJson(writer);
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AcceptVersion;
            var diff = 0;
            if (AcceptVersionId == null && AcceptVersionId == other.AcceptVersionId)
            {
                // null and null
            }
            else
            {
                diff += AcceptVersionId.CompareTo(other.AcceptVersionId);
            }
            if (VersionName == null && VersionName == other.VersionName)
            {
                // null and null
            }
            else
            {
                diff += VersionName.CompareTo(other.VersionName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Version == null && Version == other.Version)
            {
                // null and null
            }
            else
            {
                diff += Version.CompareTo(other.Version);
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
            return diff;
        }
    }
}
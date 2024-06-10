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
	public class AcceptVersion : IComparable
	{
        public string AcceptVersionId { set; get; } = null!;
        public string VersionName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public Gs2.Gs2Version.Model.Version_ Version { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public AcceptVersion WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):user:(?<userId>.+):version:(?<versionName>.+):accept",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):user:(?<userId>.+):version:(?<versionName>.+):accept",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):user:(?<userId>.+):version:(?<versionName>.+):accept",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):user:(?<userId>.+):version:(?<versionName>.+):accept",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _versionNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):version:(?<namespaceName>.+):user:(?<userId>.+):version:(?<versionName>.+):accept",
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
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["revision"] = Revision,
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
                if (AcceptVersionId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.acceptVersionId.error.tooLong"),
                    });
                }
            }
            {
                if (VersionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.versionName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.userId.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acceptVersion", "version.acceptVersion.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new AcceptVersion {
                AcceptVersionId = AcceptVersionId,
                VersionName = VersionName,
                UserId = UserId,
                Version = Version.Clone() as Gs2.Gs2Version.Model.Version_,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
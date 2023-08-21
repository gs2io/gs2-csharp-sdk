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

namespace Gs2.Gs2News.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Progress : IComparable
	{
        public string ProgressId { set; get; }
        public string UploadToken { set; get; }
        public int? Generated { set; get; }
        public int? PatternCount { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public Progress WithProgressId(string progressId) {
            this.ProgressId = progressId;
            return this;
        }
        public Progress WithUploadToken(string uploadToken) {
            this.UploadToken = uploadToken;
            return this;
        }
        public Progress WithGenerated(int? generated) {
            this.Generated = generated;
            return this;
        }
        public Progress WithPatternCount(int? patternCount) {
            this.PatternCount = patternCount;
            return this;
        }
        public Progress WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Progress WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Progress WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):news:(?<namespaceName>.+):progress:(?<uploadToken>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):news:(?<namespaceName>.+):progress:(?<uploadToken>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):news:(?<namespaceName>.+):progress:(?<uploadToken>.+)",
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

        private static System.Text.RegularExpressions.Regex _uploadTokenRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):news:(?<namespaceName>.+):progress:(?<uploadToken>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUploadTokenFromGrn(
            string grn
        )
        {
            var match = _uploadTokenRegex.Match(grn);
            if (!match.Success || !match.Groups["uploadToken"].Success)
            {
                return null;
            }
            return match.Groups["uploadToken"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Progress FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Progress()
                .WithProgressId(!data.Keys.Contains("progressId") || data["progressId"] == null ? null : data["progressId"].ToString())
                .WithUploadToken(!data.Keys.Contains("uploadToken") || data["uploadToken"] == null ? null : data["uploadToken"].ToString())
                .WithGenerated(!data.Keys.Contains("generated") || data["generated"] == null ? null : (int?)int.Parse(data["generated"].ToString()))
                .WithPatternCount(!data.Keys.Contains("patternCount") || data["patternCount"] == null ? null : (int?)int.Parse(data["patternCount"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)long.Parse(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["progressId"] = ProgressId,
                ["uploadToken"] = UploadToken,
                ["generated"] = Generated,
                ["patternCount"] = PatternCount,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ProgressId != null) {
                writer.WritePropertyName("progressId");
                writer.Write(ProgressId.ToString());
            }
            if (UploadToken != null) {
                writer.WritePropertyName("uploadToken");
                writer.Write(UploadToken.ToString());
            }
            if (Generated != null) {
                writer.WritePropertyName("generated");
                writer.Write(int.Parse(Generated.ToString()));
            }
            if (PatternCount != null) {
                writer.WritePropertyName("patternCount");
                writer.Write(int.Parse(PatternCount.ToString()));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write(long.Parse(Revision.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Progress;
            var diff = 0;
            if (ProgressId == null && ProgressId == other.ProgressId)
            {
                // null and null
            }
            else
            {
                diff += ProgressId.CompareTo(other.ProgressId);
            }
            if (UploadToken == null && UploadToken == other.UploadToken)
            {
                // null and null
            }
            else
            {
                diff += UploadToken.CompareTo(other.UploadToken);
            }
            if (Generated == null && Generated == other.Generated)
            {
                // null and null
            }
            else
            {
                diff += (int)(Generated - other.Generated);
            }
            if (PatternCount == null && PatternCount == other.PatternCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(PatternCount - other.PatternCount);
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
    }
}
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

namespace Gs2.Gs2SerialKey.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class IssueJob : IComparable
	{
        public string IssueJobId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public int? IssuedCount { set; get; }
        public int? IssueRequestCount { set; get; }
        public string Status { set; get; }
        public long? CreatedAt { set; get; }
        public IssueJob WithIssueJobId(string issueJobId) {
            this.IssueJobId = issueJobId;
            return this;
        }
        public IssueJob WithName(string name) {
            this.Name = name;
            return this;
        }
        public IssueJob WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public IssueJob WithIssuedCount(int? issuedCount) {
            this.IssuedCount = issuedCount;
            return this;
        }
        public IssueJob WithIssueRequestCount(int? issueRequestCount) {
            this.IssueRequestCount = issueRequestCount;
            return this;
        }
        public IssueJob WithStatus(string status) {
            this.Status = status;
            return this;
        }
        public IssueJob WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):master:campaign:(?<campaignModelName>.+):issue:job:(?<issueJobName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):master:campaign:(?<campaignModelName>.+):issue:job:(?<issueJobName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):master:campaign:(?<campaignModelName>.+):issue:job:(?<issueJobName>.+)",
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

        private static System.Text.RegularExpressions.Regex _campaignModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):master:campaign:(?<campaignModelName>.+):issue:job:(?<issueJobName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCampaignModelNameFromGrn(
            string grn
        )
        {
            var match = _campaignModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["campaignModelName"].Success)
            {
                return null;
            }
            return match.Groups["campaignModelName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _issueJobNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):master:campaign:(?<campaignModelName>.+):issue:job:(?<issueJobName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetIssueJobNameFromGrn(
            string grn
        )
        {
            var match = _issueJobNameRegex.Match(grn);
            if (!match.Success || !match.Groups["issueJobName"].Success)
            {
                return null;
            }
            return match.Groups["issueJobName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IssueJob FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IssueJob()
                .WithIssueJobId(!data.Keys.Contains("issueJobId") || data["issueJobId"] == null ? null : data["issueJobId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithIssuedCount(!data.Keys.Contains("issuedCount") || data["issuedCount"] == null ? null : (int?)int.Parse(data["issuedCount"].ToString()))
                .WithIssueRequestCount(!data.Keys.Contains("issueRequestCount") || data["issueRequestCount"] == null ? null : (int?)int.Parse(data["issueRequestCount"].ToString()))
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["issueJobId"] = IssueJobId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["issuedCount"] = IssuedCount,
                ["issueRequestCount"] = IssueRequestCount,
                ["status"] = Status,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (IssueJobId != null) {
                writer.WritePropertyName("issueJobId");
                writer.Write(IssueJobId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (IssuedCount != null) {
                writer.WritePropertyName("issuedCount");
                writer.Write(int.Parse(IssuedCount.ToString()));
            }
            if (IssueRequestCount != null) {
                writer.WritePropertyName("issueRequestCount");
                writer.Write(int.Parse(IssueRequestCount.ToString()));
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as IssueJob;
            var diff = 0;
            if (IssueJobId == null && IssueJobId == other.IssueJobId)
            {
                // null and null
            }
            else
            {
                diff += IssueJobId.CompareTo(other.IssueJobId);
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
            if (IssuedCount == null && IssuedCount == other.IssuedCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(IssuedCount - other.IssuedCount);
            }
            if (IssueRequestCount == null && IssueRequestCount == other.IssueRequestCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(IssueRequestCount - other.IssueRequestCount);
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }
    }
}
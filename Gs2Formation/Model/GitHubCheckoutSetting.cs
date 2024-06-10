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

namespace Gs2.Gs2Formation.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class GitHubCheckoutSetting : IComparable
	{
        public string ApiKeyId { set; get; } = null!;
        public string RepositoryName { set; get; } = null!;
        public string SourcePath { set; get; } = null!;
        public string ReferenceType { set; get; } = null!;
        public string CommitHash { set; get; } = null!;
        public string BranchName { set; get; } = null!;
        public string TagName { set; get; } = null!;
        public GitHubCheckoutSetting WithApiKeyId(string apiKeyId) {
            this.ApiKeyId = apiKeyId;
            return this;
        }
        public GitHubCheckoutSetting WithRepositoryName(string repositoryName) {
            this.RepositoryName = repositoryName;
            return this;
        }
        public GitHubCheckoutSetting WithSourcePath(string sourcePath) {
            this.SourcePath = sourcePath;
            return this;
        }
        public GitHubCheckoutSetting WithReferenceType(string referenceType) {
            this.ReferenceType = referenceType;
            return this;
        }
        public GitHubCheckoutSetting WithCommitHash(string commitHash) {
            this.CommitHash = commitHash;
            return this;
        }
        public GitHubCheckoutSetting WithBranchName(string branchName) {
            this.BranchName = branchName;
            return this;
        }
        public GitHubCheckoutSetting WithTagName(string tagName) {
            this.TagName = tagName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GitHubCheckoutSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GitHubCheckoutSetting()
                .WithApiKeyId(!data.Keys.Contains("apiKeyId") || data["apiKeyId"] == null ? null : data["apiKeyId"].ToString())
                .WithRepositoryName(!data.Keys.Contains("repositoryName") || data["repositoryName"] == null ? null : data["repositoryName"].ToString())
                .WithSourcePath(!data.Keys.Contains("sourcePath") || data["sourcePath"] == null ? null : data["sourcePath"].ToString())
                .WithReferenceType(!data.Keys.Contains("referenceType") || data["referenceType"] == null ? null : data["referenceType"].ToString())
                .WithCommitHash(!data.Keys.Contains("commitHash") || data["commitHash"] == null ? null : data["commitHash"].ToString())
                .WithBranchName(!data.Keys.Contains("branchName") || data["branchName"] == null ? null : data["branchName"].ToString())
                .WithTagName(!data.Keys.Contains("tagName") || data["tagName"] == null ? null : data["tagName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["apiKeyId"] = ApiKeyId,
                ["repositoryName"] = RepositoryName,
                ["sourcePath"] = SourcePath,
                ["referenceType"] = ReferenceType,
                ["commitHash"] = CommitHash,
                ["branchName"] = BranchName,
                ["tagName"] = TagName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ApiKeyId != null) {
                writer.WritePropertyName("apiKeyId");
                writer.Write(ApiKeyId.ToString());
            }
            if (RepositoryName != null) {
                writer.WritePropertyName("repositoryName");
                writer.Write(RepositoryName.ToString());
            }
            if (SourcePath != null) {
                writer.WritePropertyName("sourcePath");
                writer.Write(SourcePath.ToString());
            }
            if (ReferenceType != null) {
                writer.WritePropertyName("referenceType");
                writer.Write(ReferenceType.ToString());
            }
            if (CommitHash != null) {
                writer.WritePropertyName("commitHash");
                writer.Write(CommitHash.ToString());
            }
            if (BranchName != null) {
                writer.WritePropertyName("branchName");
                writer.Write(BranchName.ToString());
            }
            if (TagName != null) {
                writer.WritePropertyName("tagName");
                writer.Write(TagName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GitHubCheckoutSetting;
            var diff = 0;
            if (ApiKeyId == null && ApiKeyId == other.ApiKeyId)
            {
                // null and null
            }
            else
            {
                diff += ApiKeyId.CompareTo(other.ApiKeyId);
            }
            if (RepositoryName == null && RepositoryName == other.RepositoryName)
            {
                // null and null
            }
            else
            {
                diff += RepositoryName.CompareTo(other.RepositoryName);
            }
            if (SourcePath == null && SourcePath == other.SourcePath)
            {
                // null and null
            }
            else
            {
                diff += SourcePath.CompareTo(other.SourcePath);
            }
            if (ReferenceType == null && ReferenceType == other.ReferenceType)
            {
                // null and null
            }
            else
            {
                diff += ReferenceType.CompareTo(other.ReferenceType);
            }
            if (CommitHash == null && CommitHash == other.CommitHash)
            {
                // null and null
            }
            else
            {
                diff += CommitHash.CompareTo(other.CommitHash);
            }
            if (BranchName == null && BranchName == other.BranchName)
            {
                // null and null
            }
            else
            {
                diff += BranchName.CompareTo(other.BranchName);
            }
            if (TagName == null && TagName == other.TagName)
            {
                // null and null
            }
            else
            {
                diff += TagName.CompareTo(other.TagName);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ApiKeyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubCheckoutSetting", "formation.gitHubCheckoutSetting.apiKeyId.error.tooLong"),
                    });
                }
            }
            {
                if (RepositoryName.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubCheckoutSetting", "formation.gitHubCheckoutSetting.repositoryName.error.tooLong"),
                    });
                }
            }
            {
                if (SourcePath.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubCheckoutSetting", "formation.gitHubCheckoutSetting.sourcePath.error.tooLong"),
                    });
                }
            }
            {
                switch (ReferenceType) {
                    case "commit_hash":
                    case "branch":
                    case "tag":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("gitHubCheckoutSetting", "formation.gitHubCheckoutSetting.referenceType.error.invalid"),
                        });
                }
            }
            if (ReferenceType == "commit_hash") {
                if (CommitHash.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubCheckoutSetting", "formation.gitHubCheckoutSetting.commitHash.error.tooLong"),
                    });
                }
            }
            if (ReferenceType == "branch") {
                if (BranchName.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubCheckoutSetting", "formation.gitHubCheckoutSetting.branchName.error.tooLong"),
                    });
                }
            }
            if (ReferenceType == "tag") {
                if (TagName.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubCheckoutSetting", "formation.gitHubCheckoutSetting.tagName.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new GitHubCheckoutSetting {
                ApiKeyId = ApiKeyId,
                RepositoryName = RepositoryName,
                SourcePath = SourcePath,
                ReferenceType = ReferenceType,
                CommitHash = CommitHash,
                BranchName = BranchName,
                TagName = TagName,
            };
        }
    }
}
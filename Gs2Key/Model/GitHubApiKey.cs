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

namespace Gs2.Gs2Key.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class GitHubApiKey : IComparable
	{
        public string ApiKeyId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string ApiKey { set; get; }
        public string EncryptionKeyName { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public GitHubApiKey WithApiKeyId(string apiKeyId) {
            this.ApiKeyId = apiKeyId;
            return this;
        }
        public GitHubApiKey WithName(string name) {
            this.Name = name;
            return this;
        }
        public GitHubApiKey WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public GitHubApiKey WithApiKey(string apiKey) {
            this.ApiKey = apiKey;
            return this;
        }
        public GitHubApiKey WithEncryptionKeyName(string encryptionKeyName) {
            this.EncryptionKeyName = encryptionKeyName;
            return this;
        }
        public GitHubApiKey WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public GitHubApiKey WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public GitHubApiKey WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):key:(?<namespaceName>.+):github:(?<apiKeyName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):key:(?<namespaceName>.+):github:(?<apiKeyName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):key:(?<namespaceName>.+):github:(?<apiKeyName>.+)",
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

        private static System.Text.RegularExpressions.Regex _apiKeyNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):key:(?<namespaceName>.+):github:(?<apiKeyName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetApiKeyNameFromGrn(
            string grn
        )
        {
            var match = _apiKeyNameRegex.Match(grn);
            if (!match.Success || !match.Groups["apiKeyName"].Success)
            {
                return null;
            }
            return match.Groups["apiKeyName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GitHubApiKey FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new GitHubApiKey()
                .WithApiKeyId(!data.Keys.Contains("apiKeyId") || data["apiKeyId"] == null ? null : data["apiKeyId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithApiKey(!data.Keys.Contains("apiKey") || data["apiKey"] == null ? null : data["apiKey"].ToString())
                .WithEncryptionKeyName(!data.Keys.Contains("encryptionKeyName") || data["encryptionKeyName"] == null ? null : data["encryptionKeyName"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["apiKeyId"] = ApiKeyId,
                ["name"] = Name,
                ["description"] = Description,
                ["apiKey"] = ApiKey,
                ["encryptionKeyName"] = EncryptionKeyName,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ApiKeyId != null) {
                writer.WritePropertyName("apiKeyId");
                writer.Write(ApiKeyId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (ApiKey != null) {
                writer.WritePropertyName("apiKey");
                writer.Write(ApiKey.ToString());
            }
            if (EncryptionKeyName != null) {
                writer.WritePropertyName("encryptionKeyName");
                writer.Write(EncryptionKeyName.ToString());
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
            var other = obj as GitHubApiKey;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type GitHubApiKey.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(ApiKeyId, other.ApiKeyId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ApiKey, other.ApiKey);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EncryptionKeyName, other.EncryptionKeyName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (ApiKeyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.apiKeyId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.description.error.tooLong"),
                    });
                }
            }
            {
                if (ApiKey.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.apiKey.error.tooLong"),
                    });
                }
            }
            {
                if (EncryptionKeyName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.encryptionKeyName.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gitHubApiKey", "key.gitHubApiKey.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new GitHubApiKey {
                ApiKeyId = ApiKeyId,
                Name = Name,
                Description = Description,
                ApiKey = ApiKey,
                EncryptionKeyName = EncryptionKeyName,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
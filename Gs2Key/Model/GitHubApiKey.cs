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

namespace Gs2.Gs2Key.Model
{

	[Preserve]
	public class GitHubApiKey : IComparable
	{
        public string ApiKeyId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string EncryptionKeyName { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

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

    	[Preserve]
        public static GitHubApiKey FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GitHubApiKey()
                .WithApiKeyId(!data.Keys.Contains("apiKeyId") || data["apiKeyId"] == null ? null : data["apiKeyId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithEncryptionKeyName(!data.Keys.Contains("encryptionKeyName") || data["encryptionKeyName"] == null ? null : data["encryptionKeyName"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["apiKeyId"] = ApiKeyId,
                ["name"] = Name,
                ["description"] = Description,
                ["encryptionKeyName"] = EncryptionKeyName,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
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
            if (EncryptionKeyName != null) {
                writer.WritePropertyName("encryptionKeyName");
                writer.Write(EncryptionKeyName.ToString());
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
            var other = obj as GitHubApiKey;
            var diff = 0;
            if (ApiKeyId == null && ApiKeyId == other.ApiKeyId)
            {
                // null and null
            }
            else
            {
                diff += ApiKeyId.CompareTo(other.ApiKeyId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (EncryptionKeyName == null && EncryptionKeyName == other.EncryptionKeyName)
            {
                // null and null
            }
            else
            {
                diff += EncryptionKeyName.CompareTo(other.EncryptionKeyName);
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
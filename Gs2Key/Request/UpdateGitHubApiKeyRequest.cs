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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Key.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Key.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateGitHubApiKeyRequest : Gs2Request<UpdateGitHubApiKeyRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string ApiKeyName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string ApiKey { set; get; } = null!;
         public string EncryptionKeyName { set; get; } = null!;
        public UpdateGitHubApiKeyRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateGitHubApiKeyRequest WithApiKeyName(string apiKeyName) {
            this.ApiKeyName = apiKeyName;
            return this;
        }
        public UpdateGitHubApiKeyRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateGitHubApiKeyRequest WithApiKey(string apiKey) {
            this.ApiKey = apiKey;
            return this;
        }
        public UpdateGitHubApiKeyRequest WithEncryptionKeyName(string encryptionKeyName) {
            this.EncryptionKeyName = encryptionKeyName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateGitHubApiKeyRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateGitHubApiKeyRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithApiKeyName(!data.Keys.Contains("apiKeyName") || data["apiKeyName"] == null ? null : data["apiKeyName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithApiKey(!data.Keys.Contains("apiKey") || data["apiKey"] == null ? null : data["apiKey"].ToString())
                .WithEncryptionKeyName(!data.Keys.Contains("encryptionKeyName") || data["encryptionKeyName"] == null ? null : data["encryptionKeyName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["apiKeyName"] = ApiKeyName,
                ["description"] = Description,
                ["apiKey"] = ApiKey,
                ["encryptionKeyName"] = EncryptionKeyName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (ApiKeyName != null) {
                writer.WritePropertyName("apiKeyName");
                writer.Write(ApiKeyName.ToString());
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += ApiKeyName + ":";
            key += Description + ":";
            key += ApiKey + ":";
            key += EncryptionKeyName + ":";
            return key;
        }
    }
}
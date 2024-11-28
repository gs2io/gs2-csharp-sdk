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
using Gs2.Gs2Datastore.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Datastore.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class PrepareUploadRequest : Gs2Request<PrepareUploadRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string ContentType { set; get; } = null!;
         public string Scope { set; get; } = null!;
         public string[] AllowUserIds { set; get; } = null!;
         public bool? UpdateIfExists { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public PrepareUploadRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PrepareUploadRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public PrepareUploadRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public PrepareUploadRequest WithContentType(string contentType) {
            this.ContentType = contentType;
            return this;
        }
        public PrepareUploadRequest WithScope(string scope) {
            this.Scope = scope;
            return this;
        }
        public PrepareUploadRequest WithAllowUserIds(string[] allowUserIds) {
            this.AllowUserIds = allowUserIds;
            return this;
        }
        public PrepareUploadRequest WithUpdateIfExists(bool? updateIfExists) {
            this.UpdateIfExists = updateIfExists;
            return this;
        }

        public PrepareUploadRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PrepareUploadRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrepareUploadRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithContentType(!data.Keys.Contains("contentType") || data["contentType"] == null ? null : data["contentType"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithAllowUserIds(!data.Keys.Contains("allowUserIds") || data["allowUserIds"] == null || !data["allowUserIds"].IsArray ? null : data["allowUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithUpdateIfExists(!data.Keys.Contains("updateIfExists") || data["updateIfExists"] == null ? null : (bool?)bool.Parse(data["updateIfExists"].ToString()));
        }

        public override JsonData ToJson()
        {
            JsonData allowUserIdsJsonData = null;
            if (AllowUserIds != null && AllowUserIds.Length > 0)
            {
                allowUserIdsJsonData = new JsonData();
                foreach (var allowUserId in AllowUserIds)
                {
                    allowUserIdsJsonData.Add(allowUserId);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["name"] = Name,
                ["contentType"] = ContentType,
                ["scope"] = Scope,
                ["allowUserIds"] = allowUserIdsJsonData,
                ["updateIfExists"] = UpdateIfExists,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (ContentType != null) {
                writer.WritePropertyName("contentType");
                writer.Write(ContentType.ToString());
            }
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            if (AllowUserIds != null) {
                writer.WritePropertyName("allowUserIds");
                writer.WriteArrayStart();
                foreach (var allowUserId in AllowUserIds)
                {
                    writer.Write(allowUserId.ToString());
                }
                writer.WriteArrayEnd();
            }
            if (UpdateIfExists != null) {
                writer.WritePropertyName("updateIfExists");
                writer.Write(bool.Parse(UpdateIfExists.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += Name + ":";
            key += ContentType + ":";
            key += Scope + ":";
            key += AllowUserIds + ":";
            key += UpdateIfExists + ":";
            return key;
        }
    }
}
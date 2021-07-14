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
using UnityEngine.Scripting;

namespace Gs2.Gs2Datastore.Request
{
	[Preserve]
	[System.Serializable]
	public class PrepareReUploadRequest : Gs2Request<PrepareReUploadRequest>
	{
        public string NamespaceName { set; get; }
        public string DataObjectName { set; get; }
        public string AccessToken { set; get; }
        public string ContentType { set; get; }

        public PrepareReUploadRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public PrepareReUploadRequest WithDataObjectName(string dataObjectName) {
            this.DataObjectName = dataObjectName;
            return this;
        }

        public PrepareReUploadRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public PrepareReUploadRequest WithContentType(string contentType) {
            this.ContentType = contentType;
            return this;
        }

    	[Preserve]
        public static PrepareReUploadRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrepareReUploadRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithDataObjectName(!data.Keys.Contains("dataObjectName") || data["dataObjectName"] == null ? null : data["dataObjectName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithContentType(!data.Keys.Contains("contentType") || data["contentType"] == null ? null : data["contentType"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["dataObjectName"] = DataObjectName,
                ["accessToken"] = AccessToken,
                ["contentType"] = ContentType,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (DataObjectName != null) {
                writer.WritePropertyName("dataObjectName");
                writer.Write(DataObjectName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (ContentType != null) {
                writer.WritePropertyName("contentType");
                writer.Write(ContentType.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}
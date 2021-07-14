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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Formation.Request
{
	[Preserve]
	[System.Serializable]
	public class GetFormWithSignatureRequest : Gs2Request<GetFormWithSignatureRequest>
	{
        public string NamespaceName { set; get; }
        public string AccessToken { set; get; }
        public string MoldName { set; get; }
        public int? Index { set; get; }
        public string KeyId { set; get; }

        public GetFormWithSignatureRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public GetFormWithSignatureRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public GetFormWithSignatureRequest WithMoldName(string moldName) {
            this.MoldName = moldName;
            return this;
        }

        public GetFormWithSignatureRequest WithIndex(int? index) {
            this.Index = index;
            return this;
        }

        public GetFormWithSignatureRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

    	[Preserve]
        public static GetFormWithSignatureRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetFormWithSignatureRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithMoldName(!data.Keys.Contains("moldName") || data["moldName"] == null ? null : data["moldName"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : (int?)int.Parse(data["index"].ToString()))
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["moldName"] = MoldName,
                ["index"] = Index,
                ["keyId"] = KeyId,
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
            if (MoldName != null) {
                writer.WritePropertyName("moldName");
                writer.Write(MoldName.ToString());
            }
            if (Index != null) {
                writer.WritePropertyName("index");
                writer.Write(int.Parse(Index.ToString()));
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}
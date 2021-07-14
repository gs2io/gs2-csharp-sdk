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
using Gs2.Gs2Stamina.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Stamina.Request
{
	[Preserve]
	[System.Serializable]
	public class SetRecoverValueByStatusRequest : Gs2Request<SetRecoverValueByStatusRequest>
	{
        public string NamespaceName { set; get; }
        public string StaminaName { set; get; }
        public string AccessToken { set; get; }
        public string KeyId { set; get; }
        public string SignedStatusBody { set; get; }
        public string SignedStatusSignature { set; get; }

        public SetRecoverValueByStatusRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public SetRecoverValueByStatusRequest WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }

        public SetRecoverValueByStatusRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public SetRecoverValueByStatusRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

        public SetRecoverValueByStatusRequest WithSignedStatusBody(string signedStatusBody) {
            this.SignedStatusBody = signedStatusBody;
            return this;
        }

        public SetRecoverValueByStatusRequest WithSignedStatusSignature(string signedStatusSignature) {
            this.SignedStatusSignature = signedStatusSignature;
            return this;
        }

    	[Preserve]
        public static SetRecoverValueByStatusRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetRecoverValueByStatusRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithSignedStatusBody(!data.Keys.Contains("signedStatusBody") || data["signedStatusBody"] == null ? null : data["signedStatusBody"].ToString())
                .WithSignedStatusSignature(!data.Keys.Contains("signedStatusSignature") || data["signedStatusSignature"] == null ? null : data["signedStatusSignature"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["staminaName"] = StaminaName,
                ["accessToken"] = AccessToken,
                ["keyId"] = KeyId,
                ["signedStatusBody"] = SignedStatusBody,
                ["signedStatusSignature"] = SignedStatusSignature,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (StaminaName != null) {
                writer.WritePropertyName("staminaName");
                writer.Write(StaminaName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (SignedStatusBody != null) {
                writer.WritePropertyName("signedStatusBody");
                writer.Write(SignedStatusBody.ToString());
            }
            if (SignedStatusSignature != null) {
                writer.WritePropertyName("signedStatusSignature");
                writer.Write(SignedStatusSignature.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}
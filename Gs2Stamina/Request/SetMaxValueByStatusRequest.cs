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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Stamina.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Stamina.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SetMaxValueByStatusRequest : Gs2Request<SetMaxValueByStatusRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string StaminaName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string KeyId { set; get; } = null!;
         public string SignedStatusBody { set; get; } = null!;
         public string SignedStatusSignature { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SetMaxValueByStatusRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SetMaxValueByStatusRequest WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }
        public SetMaxValueByStatusRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public SetMaxValueByStatusRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }
        public SetMaxValueByStatusRequest WithSignedStatusBody(string signedStatusBody) {
            this.SignedStatusBody = signedStatusBody;
            return this;
        }
        public SetMaxValueByStatusRequest WithSignedStatusSignature(string signedStatusSignature) {
            this.SignedStatusSignature = signedStatusSignature;
            return this;
        }

        public SetMaxValueByStatusRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetMaxValueByStatusRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetMaxValueByStatusRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithSignedStatusBody(!data.Keys.Contains("signedStatusBody") || data["signedStatusBody"] == null ? null : data["signedStatusBody"].ToString())
                .WithSignedStatusSignature(!data.Keys.Contains("signedStatusSignature") || data["signedStatusSignature"] == null ? null : data["signedStatusSignature"].ToString());
        }

        public override JsonData ToJson()
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

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += StaminaName + ":";
            key += AccessToken + ":";
            key += KeyId + ":";
            key += SignedStatusBody + ":";
            key += SignedStatusSignature + ":";
            return key;
        }
    }
}
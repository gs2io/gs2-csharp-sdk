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
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Auth.Request
{
	[Preserve]
	[System.Serializable]
	public class LoginBySignatureRequest : Gs2Request<LoginBySignatureRequest>
	{
        public string UserId { set; get; }
        public string KeyId { set; get; }
        public string Body { set; get; }
        public string Signature { set; get; }

        public LoginBySignatureRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public LoginBySignatureRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

        public LoginBySignatureRequest WithBody(string body) {
            this.Body = body;
            return this;
        }

        public LoginBySignatureRequest WithSignature(string signature) {
            this.Signature = signature;
            return this;
        }

    	[Preserve]
        public static LoginBySignatureRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LoginBySignatureRequest()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithBody(!data.Keys.Contains("body") || data["body"] == null ? null : data["body"].ToString())
                .WithSignature(!data.Keys.Contains("signature") || data["signature"] == null ? null : data["signature"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["keyId"] = KeyId,
                ["body"] = Body,
                ["signature"] = Signature,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (Body != null) {
                writer.WritePropertyName("body");
                writer.Write(Body.ToString());
            }
            if (Signature != null) {
                writer.WritePropertyName("signature");
                writer.Write(Signature.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}
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
using Gs2.Gs2Identifier.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Identifier.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class LoginRequest : Gs2Request<LoginRequest>
	{
         public string ClientId { set; get; } = null!;
         public string ClientSecret { set; get; } = null!;
        public LoginRequest WithClientId(string clientId) {
            this.ClientId = clientId;
            return this;
        }
        public LoginRequest WithClientSecret(string clientSecret) {
            this.ClientSecret = clientSecret;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LoginRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LoginRequest()
                .WithClientId(!data.Keys.Contains("client_id") || data["client_id"] == null ? null : data["client_id"].ToString())
                .WithClientSecret(!data.Keys.Contains("client_secret") || data["client_secret"] == null ? null : data["client_secret"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["clientId"] = ClientId,
                ["clientSecret"] = ClientSecret,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ClientId != null) {
                writer.WritePropertyName("clientId");
                writer.Write(ClientId.ToString());
            }
            if (ClientSecret != null) {
                writer.WritePropertyName("clientSecret");
                writer.Write(ClientSecret.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += ClientId + ":";
            key += ClientSecret + ":";
            return key;
        }
    }
}
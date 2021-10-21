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
 *
 * deny overwrite
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Identifier.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Identifier.Result
{
	[Preserve]
	[System.Serializable]
	public class LoginResult : IResult
	{
        public string AccessToken { set; get; }
        public string TokenType { set; get; }
        public int? ExpiresIn { set; get; }

        public LoginResult WithAccessToken(string access_token) {
            this.AccessToken = access_token;
            return this;
        }

        public LoginResult WithTokenType(string token_type) {
            this.TokenType = token_type;
            return this;
        }

        public LoginResult WithExpiresIn(int? expires_in) {
            this.ExpiresIn = expires_in;
            return this;
        }

    	[Preserve]
        public static LoginResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LoginResult()
                .WithAccessToken(!data.Keys.Contains("access_token") || data["access_token"] == null ? null : data["access_token"].ToString())
                .WithTokenType(!data.Keys.Contains("token_type") || data["token_type"] == null ? null : data["token_type"].ToString())
                .WithExpiresIn(!data.Keys.Contains("expires_in") || data["expires_in"] == null ? null : (int?)int.Parse(data["expires_in"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["access_token"] = AccessToken,
                ["token_type"] = TokenType,
                ["expires_in"] = ExpiresIn,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccessToken != null) {
                writer.WritePropertyName("access_token");
                writer.Write(AccessToken.ToString());
            }
            if (TokenType != null) {
                writer.WritePropertyName("token_type");
                writer.Write(TokenType.ToString());
            }
            if (ExpiresIn != null) {
                writer.WritePropertyName("expires_in");
                writer.Write(int.Parse(ExpiresIn.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}
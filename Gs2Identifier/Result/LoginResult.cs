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

        public LoginResult WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public LoginResult WithTokenType(string tokenType) {
            this.TokenType = tokenType;
            return this;
        }

        public LoginResult WithExpiresIn(int? expiresIn) {
            this.ExpiresIn = expiresIn;
            return this;
        }

    	[Preserve]
        public static LoginResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LoginResult()
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithTokenType(!data.Keys.Contains("tokenType") || data["tokenType"] == null ? null : data["tokenType"].ToString())
                .WithExpiresIn(!data.Keys.Contains("expiresIn") || data["expiresIn"] == null ? null : (int?)int.Parse(data["expiresIn"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["accessToken"] = AccessToken,
                ["tokenType"] = TokenType,
                ["expiresIn"] = ExpiresIn,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (TokenType != null) {
                writer.WritePropertyName("tokenType");
                writer.Write(TokenType.ToString());
            }
            if (ExpiresIn != null) {
                writer.WritePropertyName("expiresIn");
                writer.Write(int.Parse(ExpiresIn.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}
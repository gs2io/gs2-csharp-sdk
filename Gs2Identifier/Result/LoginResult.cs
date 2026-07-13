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
 * deny overwrite
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

namespace Gs2.Gs2Identifier.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class LoginResult : IResult
	{
        public string AccessToken { set; get; }
        public string TokenType { set; get; }
        public int? ExpiresIn { set; get; }
        public string OwnerId { set; get; }
/* diff --- start
        public ResultMetadata Metadata { set; get; }
 diff --- end */

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

        public LoginResult WithOwnerId(string ownerId) {
            this.OwnerId = ownerId;
            return this;
        }

/* diff --- start
        public LoginResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

 diff --- end */
#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LoginResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LoginResult()
                .WithAccessToken(!data.Keys.Contains("access_token") || data["access_token"] == null ? null : data["access_token"].ToString())
                .WithTokenType(!data.Keys.Contains("token_type") || data["token_type"] == null ? null : data["token_type"].ToString())
/* diff --- start
                .WithExpiresIn(!data.Keys.Contains("expires_in") || data["expires_in"] == null ? null : (int?)(data["expires_in"].ToString().Contains(".") ? (int)double.Parse(data["expires_in"].ToString()) : int.Parse(data["expires_in"].ToString())))
                .WithOwnerId(!data.Keys.Contains("owner_id") || data["owner_id"] == null ? null : data["owner_id"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
 diff --- end */
/* diff +++ start */
                .WithExpiresIn(!data.Keys.Contains("expires_in") || data["expires_in"] == null ? null : (int?)int.Parse(data["expires_in"].ToString()))
                .WithOwnerId(!data.Keys.Contains("owner_id") || data["owner_id"] == null ? null : data["owner_id"].ToString());
/* diff +++ end */
        }

        public JsonData ToJson()
        {
            return new JsonData {
/* diff --- start
                ["accessToken"] = AccessToken,
                ["tokenType"] = TokenType,
                ["expiresIn"] = ExpiresIn,
                ["ownerId"] = OwnerId,
                ["metadata"] = Metadata?.ToJson(),
 diff --- end */
/* diff +++ start */
                ["access_token"] = AccessToken,
                ["token_type"] = TokenType,
                ["expires_in"] = ExpiresIn,
                ["owner_id"] = OwnerId,
/* diff +++ end */
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccessToken != null) {
/* diff --- start
                writer.WritePropertyName("accessToken");
 diff --- end */
                writer.WritePropertyName("access_token"); /* diff +++ */
                writer.Write(AccessToken.ToString());
            }
            if (TokenType != null) {
/* diff --- start
                writer.WritePropertyName("tokenType");
 diff --- end */
                writer.WritePropertyName("token_type"); /* diff +++ */
                writer.Write(TokenType.ToString());
            }
            if (ExpiresIn != null) {
/* diff --- start
                writer.WritePropertyName("expiresIn");
                writer.Write((ExpiresIn.ToString().Contains(".") ? (int)double.Parse(ExpiresIn.ToString()) : int.Parse(ExpiresIn.ToString())));
 diff --- end */
/* diff +++ start */
                writer.WritePropertyName("expires_in");
                writer.Write(int.Parse(ExpiresIn.ToString()));
/* diff +++ end */
            }
            if (OwnerId != null) {
/* diff --- start
                writer.WritePropertyName("ownerId");
 diff --- end */
                writer.WritePropertyName("owner_id"); /* diff +++ */
                writer.Write(OwnerId.ToString());
/* diff --- start
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
 diff --- end */
            }
            writer.WriteObjectEnd();
        }
    }
}
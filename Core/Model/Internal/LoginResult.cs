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
using Gs2.Core.Model;
using Gs2.Gs2Identifier.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Core.Model.Internal
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class LoginResult : IResult
	{
		public string AccessToken { set; get; }
		public string TokenType { set; get; }
		public int? ExpiresIn { set; get; }
		public string OwnerId { set; get; }

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
				.WithExpiresIn(!data.Keys.Contains("expires_in") || data["expires_in"] == null ? null : (int?)int.Parse(data["expires_in"].ToString()))
				.WithOwnerId(!data.Keys.Contains("owner_id") || data["owner_id"] == null ? null : data["owner_id"].ToString());
		}

	}
}
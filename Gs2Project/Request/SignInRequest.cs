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
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Project.Request
{
	[Preserve]
	[System.Serializable]
	public class SignInRequest : Gs2Request<SignInRequest>
	{
        public string Email { set; get; }
        public string Password { set; get; }

        public SignInRequest WithEmail(string email) {
            this.Email = email;
            return this;
        }

        public SignInRequest WithPassword(string password) {
            this.Password = password;
            return this;
        }

    	[Preserve]
        public static SignInRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SignInRequest()
                .WithEmail(!data.Keys.Contains("email") || data["email"] == null ? null : data["email"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["email"] = Email,
                ["password"] = Password,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Email != null) {
                writer.WritePropertyName("email");
                writer.Write(Email.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}
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
using Gs2.Gs2Account.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Account.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateTakeOverRequest : Gs2Request<CreateTakeOverRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public int? Type { set; get; } = null!;
         public string UserIdentifier { set; get; } = null!;
         public string Password { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public CreateTakeOverRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateTakeOverRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public CreateTakeOverRequest WithType(int? type) {
            this.Type = type;
            return this;
        }
        public CreateTakeOverRequest WithUserIdentifier(string userIdentifier) {
            this.UserIdentifier = userIdentifier;
            return this;
        }
        public CreateTakeOverRequest WithPassword(string password) {
            this.Password = password;
            return this;
        }

        public CreateTakeOverRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateTakeOverRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateTakeOverRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : (int?)(data["type"].ToString().Contains(".") ? (int)double.Parse(data["type"].ToString()) : int.Parse(data["type"].ToString())))
                .WithUserIdentifier(!data.Keys.Contains("userIdentifier") || data["userIdentifier"] == null ? null : data["userIdentifier"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["type"] = Type,
                ["userIdentifier"] = UserIdentifier,
                ["password"] = Password,
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
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write((Type.ToString().Contains(".") ? (int)double.Parse(Type.ToString()) : int.Parse(Type.ToString())));
            }
            if (UserIdentifier != null) {
                writer.WritePropertyName("userIdentifier");
                writer.Write(UserIdentifier.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += Type + ":";
            key += UserIdentifier + ":";
            key += Password + ":";
            return key;
        }
    }
}
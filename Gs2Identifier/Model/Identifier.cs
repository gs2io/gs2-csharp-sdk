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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Identifier.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Identifier : IComparable
	{
        public string ClientId { set; get; }
        public string UserName { set; get; }
        public string ClientSecret { set; get; }
        public long? CreatedAt { set; get; }
        public Identifier WithClientId(string clientId) {
            this.ClientId = clientId;
            return this;
        }
        public Identifier WithUserName(string userName) {
            this.UserName = userName;
            return this;
        }
        public Identifier WithClientSecret(string clientSecret) {
            this.ClientSecret = clientSecret;
            return this;
        }
        public Identifier WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Identifier FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Identifier()
                .WithClientId(!data.Keys.Contains("clientId") || data["clientId"] == null ? null : data["clientId"].ToString())
                .WithUserName(!data.Keys.Contains("userName") || data["userName"] == null ? null : data["userName"].ToString())
                .WithClientSecret(!data.Keys.Contains("clientSecret") || data["clientSecret"] == null ? null : data["clientSecret"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["clientId"] = ClientId,
                ["userName"] = UserName,
                ["clientSecret"] = ClientSecret,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ClientId != null) {
                writer.WritePropertyName("clientId");
                writer.Write(ClientId.ToString());
            }
            if (UserName != null) {
                writer.WritePropertyName("userName");
                writer.Write(UserName.ToString());
            }
            if (ClientSecret != null) {
                writer.WritePropertyName("clientSecret");
                writer.Write(ClientSecret.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Identifier;
            var diff = 0;
            if (ClientId == null && ClientId == other.ClientId)
            {
                // null and null
            }
            else
            {
                diff += ClientId.CompareTo(other.ClientId);
            }
            if (UserName == null && UserName == other.UserName)
            {
                // null and null
            }
            else
            {
                diff += UserName.CompareTo(other.UserName);
            }
            if (ClientSecret == null && ClientSecret == other.ClientSecret)
            {
                // null and null
            }
            else
            {
                diff += ClientSecret.CompareTo(other.ClientSecret);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }
    }
}
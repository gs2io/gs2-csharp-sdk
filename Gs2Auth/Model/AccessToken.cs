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

namespace Gs2.Gs2Auth.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AccessToken : IComparable
	{
        public string Token { set; get; }
        public string UserId { set; get; }
        public long? Expire { set; get; }

        public AccessToken WithToken(string token) {
            this.Token = token;
            return this;
        }

        public AccessToken WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public AccessToken WithExpire(long? expire) {
            this.Expire = expire;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AccessToken FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AccessToken()
                .WithToken(!data.Keys.Contains("token") || data["token"] == null ? null : data["token"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithExpire(!data.Keys.Contains("expire") || data["expire"] == null ? null : (long?)long.Parse(data["expire"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["token"] = Token,
                ["userId"] = UserId,
                ["expire"] = Expire,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Token != null) {
                writer.WritePropertyName("token");
                writer.Write(Token.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Expire != null) {
                writer.WritePropertyName("expire");
                writer.Write(long.Parse(Expire.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AccessToken;
            var diff = 0;
            if (Token == null && Token == other.Token)
            {
                // null and null
            }
            else
            {
                diff += Token.CompareTo(other.Token);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Expire == null && Expire == other.Expire)
            {
                // null and null
            }
            else
            {
                diff += (int)(Expire - other.Expire);
            }
            return diff;
        }
    }
}
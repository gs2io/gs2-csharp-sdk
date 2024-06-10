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
        public string Token { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string FederationFromUserId { set; get; } = null!;
        public long? Expire { set; get; } = null!;
        public int? TimeOffset { set; get; } = null!;
        public AccessToken WithToken(string token) {
            this.Token = token;
            return this;
        }
        public AccessToken WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public AccessToken WithFederationFromUserId(string federationFromUserId) {
            this.FederationFromUserId = federationFromUserId;
            return this;
        }
        public AccessToken WithExpire(long? expire) {
            this.Expire = expire;
            return this;
        }
        public AccessToken WithTimeOffset(int? timeOffset) {
            this.TimeOffset = timeOffset;
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
                .WithFederationFromUserId(!data.Keys.Contains("federationFromUserId") || data["federationFromUserId"] == null ? null : data["federationFromUserId"].ToString())
                .WithExpire(!data.Keys.Contains("expire") || data["expire"] == null ? null : (long?)(data["expire"].ToString().Contains(".") ? (long)double.Parse(data["expire"].ToString()) : long.Parse(data["expire"].ToString())))
                .WithTimeOffset(!data.Keys.Contains("timeOffset") || data["timeOffset"] == null ? null : (int?)(data["timeOffset"].ToString().Contains(".") ? (int)double.Parse(data["timeOffset"].ToString()) : int.Parse(data["timeOffset"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["token"] = Token,
                ["userId"] = UserId,
                ["federationFromUserId"] = FederationFromUserId,
                ["expire"] = Expire,
                ["timeOffset"] = TimeOffset,
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
            if (FederationFromUserId != null) {
                writer.WritePropertyName("federationFromUserId");
                writer.Write(FederationFromUserId.ToString());
            }
            if (Expire != null) {
                writer.WritePropertyName("expire");
                writer.Write((Expire.ToString().Contains(".") ? (long)double.Parse(Expire.ToString()) : long.Parse(Expire.ToString())));
            }
            if (TimeOffset != null) {
                writer.WritePropertyName("timeOffset");
                writer.Write((TimeOffset.ToString().Contains(".") ? (int)double.Parse(TimeOffset.ToString()) : int.Parse(TimeOffset.ToString())));
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
            if (FederationFromUserId == null && FederationFromUserId == other.FederationFromUserId)
            {
                // null and null
            }
            else
            {
                diff += FederationFromUserId.CompareTo(other.FederationFromUserId);
            }
            if (Expire == null && Expire == other.Expire)
            {
                // null and null
            }
            else
            {
                diff += (int)(Expire - other.Expire);
            }
            if (TimeOffset == null && TimeOffset == other.TimeOffset)
            {
                // null and null
            }
            else
            {
                diff += (int)(TimeOffset - other.TimeOffset);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Token.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessToken", "auth.accessToken.token.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessToken", "auth.accessToken.userId.error.tooLong"),
                    });
                }
            }
            {
                if (FederationFromUserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessToken", "auth.accessToken.federationFromUserId.error.tooLong"),
                    });
                }
            }
            {
                if (Expire < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessToken", "auth.accessToken.expire.error.invalid"),
                    });
                }
                if (Expire > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessToken", "auth.accessToken.expire.error.invalid"),
                    });
                }
            }
            {
                if (TimeOffset < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessToken", "auth.accessToken.timeOffset.error.invalid"),
                    });
                }
                if (TimeOffset > 31536000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessToken", "auth.accessToken.timeOffset.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new AccessToken {
                Token = Token,
                UserId = UserId,
                FederationFromUserId = FederationFromUserId,
                Expire = Expire,
                TimeOffset = TimeOffset,
            };
        }
    }
}
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

namespace Gs2.Gs2Account.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class PlatformUser : IComparable
	{
        public int? Type { set; get; } = null!;
        public string UserIdentifier { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public PlatformUser WithType(int? type) {
            this.Type = type;
            return this;
        }
        public PlatformUser WithUserIdentifier(string userIdentifier) {
            this.UserIdentifier = userIdentifier;
            return this;
        }
        public PlatformUser WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PlatformUser FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PlatformUser()
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : (int?)(data["type"].ToString().Contains(".") ? (int)double.Parse(data["type"].ToString()) : int.Parse(data["type"].ToString())))
                .WithUserIdentifier(!data.Keys.Contains("userIdentifier") || data["userIdentifier"] == null ? null : data["userIdentifier"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["type"] = Type,
                ["userIdentifier"] = UserIdentifier,
                ["userId"] = UserId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write((Type.ToString().Contains(".") ? (int)double.Parse(Type.ToString()) : int.Parse(Type.ToString())));
            }
            if (UserIdentifier != null) {
                writer.WritePropertyName("userIdentifier");
                writer.Write(UserIdentifier.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as PlatformUser;
            var diff = 0;
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += (int)(Type - other.Type);
            }
            if (UserIdentifier == null && UserIdentifier == other.UserIdentifier)
            {
                // null and null
            }
            else
            {
                diff += UserIdentifier.CompareTo(other.UserIdentifier);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Type < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformUser", "account.platformUser.type.error.invalid"),
                    });
                }
                if (Type > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformUser", "account.platformUser.type.error.invalid"),
                    });
                }
            }
            {
                if (UserIdentifier.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformUser", "account.platformUser.userIdentifier.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformUser", "account.platformUser.userId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new PlatformUser {
                Type = Type,
                UserIdentifier = UserIdentifier,
                UserId = UserId,
            };
        }
    }
}
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
using UnityEngine.Scripting;

namespace Gs2.Gs2Account.Model
{

	[Preserve]
	public class TakeOver : IComparable
	{
        public string TakeOverId { set; get; }
        public string UserId { set; get; }
        public int? Type { set; get; }
        public string UserIdentifier { set; get; }
        public string Password { set; get; }
        public long? CreatedAt { set; get; }

        public TakeOver WithTakeOverId(string takeOverId) {
            this.TakeOverId = takeOverId;
            return this;
        }

        public TakeOver WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public TakeOver WithType(int? type) {
            this.Type = type;
            return this;
        }

        public TakeOver WithUserIdentifier(string userIdentifier) {
            this.UserIdentifier = userIdentifier;
            return this;
        }

        public TakeOver WithPassword(string password) {
            this.Password = password;
            return this;
        }

        public TakeOver WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

    	[Preserve]
        public static TakeOver FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TakeOver()
                .WithTakeOverId(!data.Keys.Contains("takeOverId") || data["takeOverId"] == null ? null : data["takeOverId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : (int?)int.Parse(data["type"].ToString()))
                .WithUserIdentifier(!data.Keys.Contains("userIdentifier") || data["userIdentifier"] == null ? null : data["userIdentifier"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["takeOverId"] = TakeOverId,
                ["userId"] = UserId,
                ["type"] = Type,
                ["userIdentifier"] = UserIdentifier,
                ["password"] = Password,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TakeOverId != null) {
                writer.WritePropertyName("takeOverId");
                writer.Write(TakeOverId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(int.Parse(Type.ToString()));
            }
            if (UserIdentifier != null) {
                writer.WritePropertyName("userIdentifier");
                writer.Write(UserIdentifier.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TakeOver;
            var diff = 0;
            if (TakeOverId == null && TakeOverId == other.TakeOverId)
            {
                // null and null
            }
            else
            {
                diff += TakeOverId.CompareTo(other.TakeOverId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
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
            if (Password == null && Password == other.Password)
            {
                // null and null
            }
            else
            {
                diff += Password.CompareTo(other.Password);
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
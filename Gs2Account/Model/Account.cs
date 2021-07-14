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
	public class Account : IComparable
	{
        public string AccountId { set; get; }
        public string UserId { set; get; }
        public string Password { set; get; }
        public int? TimeOffset { set; get; }
        public long? CreatedAt { set; get; }

        public Account WithAccountId(string accountId) {
            this.AccountId = accountId;
            return this;
        }

        public Account WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Account WithPassword(string password) {
            this.Password = password;
            return this;
        }

        public Account WithTimeOffset(int? timeOffset) {
            this.TimeOffset = timeOffset;
            return this;
        }

        public Account WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

    	[Preserve]
        public static Account FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Account()
                .WithAccountId(!data.Keys.Contains("accountId") || data["accountId"] == null ? null : data["accountId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithTimeOffset(!data.Keys.Contains("timeOffset") || data["timeOffset"] == null ? null : (int?)int.Parse(data["timeOffset"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["accountId"] = AccountId,
                ["userId"] = UserId,
                ["password"] = Password,
                ["timeOffset"] = TimeOffset,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccountId != null) {
                writer.WritePropertyName("accountId");
                writer.Write(AccountId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            if (TimeOffset != null) {
                writer.WritePropertyName("timeOffset");
                writer.Write(int.Parse(TimeOffset.ToString()));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Account;
            var diff = 0;
            if (AccountId == null && AccountId == other.AccountId)
            {
                // null and null
            }
            else
            {
                diff += AccountId.CompareTo(other.AccountId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Password == null && Password == other.Password)
            {
                // null and null
            }
            else
            {
                diff += Password.CompareTo(other.Password);
            }
            if (TimeOffset == null && TimeOffset == other.TimeOffset)
            {
                // null and null
            }
            else
            {
                diff += (int)(TimeOffset - other.TimeOffset);
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
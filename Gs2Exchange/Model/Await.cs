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

namespace Gs2.Gs2Exchange.Model
{

	[Preserve]
	public class Await : IComparable
	{
        public string AwaitId { set; get; }
        public string UserId { set; get; }
        public string RateName { set; get; }
        public string Name { set; get; }
        public int? Count { set; get; }
        public long? ExchangedAt { set; get; }

        public Await WithAwaitId(string awaitId) {
            this.AwaitId = awaitId;
            return this;
        }

        public Await WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Await WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }

        public Await WithName(string name) {
            this.Name = name;
            return this;
        }

        public Await WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public Await WithExchangedAt(long? exchangedAt) {
            this.ExchangedAt = exchangedAt;
            return this;
        }

    	[Preserve]
        public static Await FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Await()
                .WithAwaitId(!data.Keys.Contains("awaitId") || data["awaitId"] == null ? null : data["awaitId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)int.Parse(data["count"].ToString()))
                .WithExchangedAt(!data.Keys.Contains("exchangedAt") || data["exchangedAt"] == null ? null : (long?)long.Parse(data["exchangedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["awaitId"] = AwaitId,
                ["userId"] = UserId,
                ["rateName"] = RateName,
                ["name"] = Name,
                ["count"] = Count,
                ["exchangedAt"] = ExchangedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AwaitId != null) {
                writer.WritePropertyName("awaitId");
                writer.Write(AwaitId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write(int.Parse(Count.ToString()));
            }
            if (ExchangedAt != null) {
                writer.WritePropertyName("exchangedAt");
                writer.Write(long.Parse(ExchangedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Await;
            var diff = 0;
            if (AwaitId == null && AwaitId == other.AwaitId)
            {
                // null and null
            }
            else
            {
                diff += AwaitId.CompareTo(other.AwaitId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (RateName == null && RateName == other.RateName)
            {
                // null and null
            }
            else
            {
                diff += RateName.CompareTo(other.RateName);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            if (ExchangedAt == null && ExchangedAt == other.ExchangedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExchangedAt - other.ExchangedAt);
            }
            return diff;
        }
    }
}
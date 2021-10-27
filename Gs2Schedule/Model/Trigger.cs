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

namespace Gs2.Gs2Schedule.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Trigger : IComparable
	{
        public string TriggerId { set; get; }
        public string Name { set; get; }
        public string UserId { set; get; }
        public long? CreatedAt { set; get; }
        public long? ExpiresAt { set; get; }

        public Trigger WithTriggerId(string triggerId) {
            this.TriggerId = triggerId;
            return this;
        }

        public Trigger WithName(string name) {
            this.Name = name;
            return this;
        }

        public Trigger WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Trigger WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Trigger WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Trigger FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Trigger()
                .WithTriggerId(!data.Keys.Contains("triggerId") || data["triggerId"] == null ? null : data["triggerId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)long.Parse(data["expiresAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["triggerId"] = TriggerId,
                ["name"] = Name,
                ["userId"] = UserId,
                ["createdAt"] = CreatedAt,
                ["expiresAt"] = ExpiresAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TriggerId != null) {
                writer.WritePropertyName("triggerId");
                writer.Write(TriggerId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write(long.Parse(ExpiresAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Trigger;
            var diff = 0;
            if (TriggerId == null && TriggerId == other.TriggerId)
            {
                // null and null
            }
            else
            {
                diff += TriggerId.CompareTo(other.TriggerId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (ExpiresAt == null && ExpiresAt == other.ExpiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExpiresAt - other.ExpiresAt);
            }
            return diff;
        }
    }
}
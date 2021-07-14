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

namespace Gs2.Gs2Dictionary.Model
{

	[Preserve]
	public class Entry : IComparable
	{
        public string EntryId { set; get; }
        public string UserId { set; get; }
        public string Name { set; get; }
        public long? AcquiredAt { set; get; }

        public Entry WithEntryId(string entryId) {
            this.EntryId = entryId;
            return this;
        }

        public Entry WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Entry WithName(string name) {
            this.Name = name;
            return this;
        }

        public Entry WithAcquiredAt(long? acquiredAt) {
            this.AcquiredAt = acquiredAt;
            return this;
        }

    	[Preserve]
        public static Entry FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Entry()
                .WithEntryId(!data.Keys.Contains("entryId") || data["entryId"] == null ? null : data["entryId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithAcquiredAt(!data.Keys.Contains("acquiredAt") || data["acquiredAt"] == null ? null : (long?)long.Parse(data["acquiredAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["entryId"] = EntryId,
                ["userId"] = UserId,
                ["name"] = Name,
                ["acquiredAt"] = AcquiredAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (EntryId != null) {
                writer.WritePropertyName("entryId");
                writer.Write(EntryId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (AcquiredAt != null) {
                writer.WritePropertyName("acquiredAt");
                writer.Write(long.Parse(AcquiredAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Entry;
            var diff = 0;
            if (EntryId == null && EntryId == other.EntryId)
            {
                // null and null
            }
            else
            {
                diff += EntryId.CompareTo(other.EntryId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (AcquiredAt == null && AcquiredAt == other.AcquiredAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(AcquiredAt - other.AcquiredAt);
            }
            return diff;
        }
    }
}
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

namespace Gs2.Gs2Formation.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Mold : IComparable
	{
        public string MoldId { set; get; }
        public string Name { set; get; }
        public string UserId { set; get; }
        public int? Capacity { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Mold WithMoldId(string moldId) {
            this.MoldId = moldId;
            return this;
        }

        public Mold WithName(string name) {
            this.Name = name;
            return this;
        }

        public Mold WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Mold WithCapacity(int? capacity) {
            this.Capacity = capacity;
            return this;
        }

        public Mold WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Mold WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Mold FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Mold()
                .WithMoldId(!data.Keys.Contains("moldId") || data["moldId"] == null ? null : data["moldId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCapacity(!data.Keys.Contains("capacity") || data["capacity"] == null ? null : (int?)int.Parse(data["capacity"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["moldId"] = MoldId,
                ["name"] = Name,
                ["userId"] = UserId,
                ["capacity"] = Capacity,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MoldId != null) {
                writer.WritePropertyName("moldId");
                writer.Write(MoldId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Capacity != null) {
                writer.WritePropertyName("capacity");
                writer.Write(int.Parse(Capacity.ToString()));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Mold;
            var diff = 0;
            if (MoldId == null && MoldId == other.MoldId)
            {
                // null and null
            }
            else
            {
                diff += MoldId.CompareTo(other.MoldId);
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
            if (Capacity == null && Capacity == other.Capacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(Capacity - other.Capacity);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}
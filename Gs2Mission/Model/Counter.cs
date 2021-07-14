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

namespace Gs2.Gs2Mission.Model
{

	[Preserve]
	public class Counter : IComparable
	{
        public string CounterId { set; get; }
        public string UserId { set; get; }
        public string Name { set; get; }
        public Gs2.Gs2Mission.Model.ScopedValue[] Values { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Counter WithCounterId(string counterId) {
            this.CounterId = counterId;
            return this;
        }

        public Counter WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Counter WithName(string name) {
            this.Name = name;
            return this;
        }

        public Counter WithValues(Gs2.Gs2Mission.Model.ScopedValue[] values) {
            this.Values = values;
            return this;
        }

        public Counter WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Counter WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

    	[Preserve]
        public static Counter FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Counter()
                .WithCounterId(!data.Keys.Contains("counterId") || data["counterId"] == null ? null : data["counterId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithValues(!data.Keys.Contains("values") || data["values"] == null ? new Gs2.Gs2Mission.Model.ScopedValue[]{} : data["values"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.ScopedValue.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["counterId"] = CounterId,
                ["userId"] = UserId,
                ["name"] = Name,
                ["values"] = new JsonData(Values == null ? new JsonData[]{} :
                        Values.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CounterId != null) {
                writer.WritePropertyName("counterId");
                writer.Write(CounterId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Values != null) {
                writer.WritePropertyName("values");
                writer.WriteArrayStart();
                foreach (var value in Values)
                {
                    if (value != null) {
                        value.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            var other = obj as Counter;
            var diff = 0;
            if (CounterId == null && CounterId == other.CounterId)
            {
                // null and null
            }
            else
            {
                diff += CounterId.CompareTo(other.CounterId);
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
            if (Values == null && Values == other.Values)
            {
                // null and null
            }
            else
            {
                diff += Values.Length - other.Values.Length;
                for (var i = 0; i < Values.Length; i++)
                {
                    diff += Values[i].CompareTo(other.Values[i]);
                }
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
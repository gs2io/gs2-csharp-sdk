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

namespace Gs2.Gs2Watch.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Cumulative : IComparable
	{
        public string CumulativeId { set; get; }
        public string ResourceGrn { set; get; }
        public string Name { set; get; }
        public long? Value { set; get; }
        public long? UpdatedAt { set; get; }

        public Cumulative WithCumulativeId(string cumulativeId) {
            this.CumulativeId = cumulativeId;
            return this;
        }

        public Cumulative WithResourceGrn(string resourceGrn) {
            this.ResourceGrn = resourceGrn;
            return this;
        }

        public Cumulative WithName(string name) {
            this.Name = name;
            return this;
        }

        public Cumulative WithValue(long? value) {
            this.Value = value;
            return this;
        }

        public Cumulative WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Cumulative FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Cumulative()
                .WithCumulativeId(!data.Keys.Contains("cumulativeId") || data["cumulativeId"] == null ? null : data["cumulativeId"].ToString())
                .WithResourceGrn(!data.Keys.Contains("resourceGrn") || data["resourceGrn"] == null ? null : data["resourceGrn"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (long?)long.Parse(data["value"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["cumulativeId"] = CumulativeId,
                ["resourceGrn"] = ResourceGrn,
                ["name"] = Name,
                ["value"] = Value,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CumulativeId != null) {
                writer.WritePropertyName("cumulativeId");
                writer.Write(CumulativeId.ToString());
            }
            if (ResourceGrn != null) {
                writer.WritePropertyName("resourceGrn");
                writer.Write(ResourceGrn.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(long.Parse(Value.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Cumulative;
            var diff = 0;
            if (CumulativeId == null && CumulativeId == other.CumulativeId)
            {
                // null and null
            }
            else
            {
                diff += CumulativeId.CompareTo(other.CumulativeId);
            }
            if (ResourceGrn == null && ResourceGrn == other.ResourceGrn)
            {
                // null and null
            }
            else
            {
                diff += ResourceGrn.CompareTo(other.ResourceGrn);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
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
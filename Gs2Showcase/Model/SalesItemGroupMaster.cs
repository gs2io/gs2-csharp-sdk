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

namespace Gs2.Gs2Showcase.Model
{

	[Preserve]
	public class SalesItemGroupMaster : IComparable
	{
        public string SalesItemGroupId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string[] SalesItemNames { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public SalesItemGroupMaster WithSalesItemGroupId(string salesItemGroupId) {
            this.SalesItemGroupId = salesItemGroupId;
            return this;
        }

        public SalesItemGroupMaster WithName(string name) {
            this.Name = name;
            return this;
        }

        public SalesItemGroupMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public SalesItemGroupMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public SalesItemGroupMaster WithSalesItemNames(string[] salesItemNames) {
            this.SalesItemNames = salesItemNames;
            return this;
        }

        public SalesItemGroupMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public SalesItemGroupMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

    	[Preserve]
        public static SalesItemGroupMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SalesItemGroupMaster()
                .WithSalesItemGroupId(!data.Keys.Contains("salesItemGroupId") || data["salesItemGroupId"] == null ? null : data["salesItemGroupId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithSalesItemNames(!data.Keys.Contains("salesItemNames") || data["salesItemNames"] == null ? new string[]{} : data["salesItemNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["salesItemGroupId"] = SalesItemGroupId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["salesItemNames"] = new JsonData(SalesItemNames == null ? new JsonData[]{} :
                        SalesItemNames.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SalesItemGroupId != null) {
                writer.WritePropertyName("salesItemGroupId");
                writer.Write(SalesItemGroupId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (SalesItemNames != null) {
                writer.WritePropertyName("salesItemNames");
                writer.WriteArrayStart();
                foreach (var salesItemName in SalesItemNames)
                {
                    if (salesItemName != null) {
                        writer.Write(salesItemName.ToString());
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
            var other = obj as SalesItemGroupMaster;
            var diff = 0;
            if (SalesItemGroupId == null && SalesItemGroupId == other.SalesItemGroupId)
            {
                // null and null
            }
            else
            {
                diff += SalesItemGroupId.CompareTo(other.SalesItemGroupId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (SalesItemNames == null && SalesItemNames == other.SalesItemNames)
            {
                // null and null
            }
            else
            {
                diff += SalesItemNames.Length - other.SalesItemNames.Length;
                for (var i = 0; i < SalesItemNames.Length; i++)
                {
                    diff += SalesItemNames[i].CompareTo(other.SalesItemNames[i]);
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
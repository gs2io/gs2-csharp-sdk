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

namespace Gs2.Gs2Showcase.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SalesItemGroupMaster : IComparable
	{
        public string SalesItemGroupId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string[] SalesItemNames { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public SalesItemGroupMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):salesItemGroup:(?<salesItemGroupName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):salesItemGroup:(?<salesItemGroupName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):salesItemGroup:(?<salesItemGroupName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _salesItemGroupNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):salesItemGroup:(?<salesItemGroupName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSalesItemGroupNameFromGrn(
            string grn
        )
        {
            var match = _salesItemGroupNameRegex.Match(grn);
            if (!match.Success || !match.Groups["salesItemGroupName"].Success)
            {
                return null;
            }
            return match.Groups["salesItemGroupName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
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
                .WithSalesItemNames(!data.Keys.Contains("salesItemNames") || data["salesItemNames"] == null || !data["salesItemNames"].IsArray ? null : data["salesItemNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData salesItemNamesJsonData = null;
            if (SalesItemNames != null && SalesItemNames.Length > 0)
            {
                salesItemNamesJsonData = new JsonData();
                foreach (var salesItemName in SalesItemNames)
                {
                    salesItemNamesJsonData.Add(salesItemName);
                }
            }
            return new JsonData {
                ["salesItemGroupId"] = SalesItemGroupId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["salesItemNames"] = salesItemNamesJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (SalesItemGroupId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.salesItemGroupId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (SalesItemNames.Length < 2) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.salesItemNames.error.tooFew"),
                    });
                }
                if (SalesItemNames.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.salesItemNames.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemGroupMaster", "showcase.salesItemGroupMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SalesItemGroupMaster {
                SalesItemGroupId = SalesItemGroupId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                SalesItemNames = SalesItemNames.Clone() as string[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class StoreContentModelMaster : IComparable
	{
        public string StoreContentModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Money2.Model.AppleAppStoreContent AppleAppStore { set; get; } = null!;
        public Gs2.Gs2Money2.Model.GooglePlayContent GooglePlay { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public StoreContentModelMaster WithStoreContentModelId(string storeContentModelId) {
            this.StoreContentModelId = storeContentModelId;
            return this;
        }
        public StoreContentModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public StoreContentModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public StoreContentModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public StoreContentModelMaster WithAppleAppStore(Gs2.Gs2Money2.Model.AppleAppStoreContent appleAppStore) {
            this.AppleAppStore = appleAppStore;
            return this;
        }
        public StoreContentModelMaster WithGooglePlay(Gs2.Gs2Money2.Model.GooglePlayContent googlePlay) {
            this.GooglePlay = googlePlay;
            return this;
        }
        public StoreContentModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public StoreContentModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public StoreContentModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):master:content:(?<contentName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):master:content:(?<contentName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):master:content:(?<contentName>.+)",
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

        private static System.Text.RegularExpressions.Regex _contentNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):master:content:(?<contentName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetContentNameFromGrn(
            string grn
        )
        {
            var match = _contentNameRegex.Match(grn);
            if (!match.Success || !match.Groups["contentName"].Success)
            {
                return null;
            }
            return match.Groups["contentName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StoreContentModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StoreContentModelMaster()
                .WithStoreContentModelId(!data.Keys.Contains("storeContentModelId") || data["storeContentModelId"] == null ? null : data["storeContentModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithAppleAppStore(!data.Keys.Contains("appleAppStore") || data["appleAppStore"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreContent.FromJson(data["appleAppStore"]))
                .WithGooglePlay(!data.Keys.Contains("googlePlay") || data["googlePlay"] == null ? null : Gs2.Gs2Money2.Model.GooglePlayContent.FromJson(data["googlePlay"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["storeContentModelId"] = StoreContentModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["appleAppStore"] = AppleAppStore?.ToJson(),
                ["googlePlay"] = GooglePlay?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StoreContentModelId != null) {
                writer.WritePropertyName("storeContentModelId");
                writer.Write(StoreContentModelId.ToString());
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
            if (AppleAppStore != null) {
                writer.WritePropertyName("appleAppStore");
                AppleAppStore.WriteJson(writer);
            }
            if (GooglePlay != null) {
                writer.WritePropertyName("googlePlay");
                GooglePlay.WriteJson(writer);
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
            var other = obj as StoreContentModelMaster;
            var diff = 0;
            if (StoreContentModelId == null && StoreContentModelId == other.StoreContentModelId)
            {
                // null and null
            }
            else
            {
                diff += StoreContentModelId.CompareTo(other.StoreContentModelId);
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
            if (AppleAppStore == null && AppleAppStore == other.AppleAppStore)
            {
                // null and null
            }
            else
            {
                diff += AppleAppStore.CompareTo(other.AppleAppStore);
            }
            if (GooglePlay == null && GooglePlay == other.GooglePlay)
            {
                // null and null
            }
            else
            {
                diff += GooglePlay.CompareTo(other.GooglePlay);
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
                if (StoreContentModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.storeContentModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModelMaster", "money2.storeContentModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new StoreContentModelMaster {
                StoreContentModelId = StoreContentModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                AppleAppStore = AppleAppStore.Clone() as Gs2.Gs2Money2.Model.AppleAppStoreContent,
                GooglePlay = GooglePlay.Clone() as Gs2.Gs2Money2.Model.GooglePlayContent,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
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
	public class StoreSubscriptionContentModelMaster : IComparable
	{
        public string StoreSubscriptionContentModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string ScheduleNamespaceId { set; get; } = null!;
        public string TriggerName { set; get; } = null!;
        public Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent AppleAppStore { set; get; } = null!;
        public Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent GooglePlay { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public StoreSubscriptionContentModelMaster WithStoreSubscriptionContentModelId(string storeSubscriptionContentModelId) {
            this.StoreSubscriptionContentModelId = storeSubscriptionContentModelId;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithScheduleNamespaceId(string scheduleNamespaceId) {
            this.ScheduleNamespaceId = scheduleNamespaceId;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithTriggerName(string triggerName) {
            this.TriggerName = triggerName;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithAppleAppStore(Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent appleAppStore) {
            this.AppleAppStore = appleAppStore;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithGooglePlay(Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent googlePlay) {
            this.GooglePlay = googlePlay;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public StoreSubscriptionContentModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):master:subscription:content:(?<contentName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):master:subscription:content:(?<contentName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):master:subscription:content:(?<contentName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):master:subscription:content:(?<contentName>.+)",
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
        public static StoreSubscriptionContentModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StoreSubscriptionContentModelMaster()
                .WithStoreSubscriptionContentModelId(!data.Keys.Contains("storeSubscriptionContentModelId") || data["storeSubscriptionContentModelId"] == null ? null : data["storeSubscriptionContentModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScheduleNamespaceId(!data.Keys.Contains("scheduleNamespaceId") || data["scheduleNamespaceId"] == null ? null : data["scheduleNamespaceId"].ToString())
                .WithTriggerName(!data.Keys.Contains("triggerName") || data["triggerName"] == null ? null : data["triggerName"].ToString())
                .WithAppleAppStore(!data.Keys.Contains("appleAppStore") || data["appleAppStore"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent.FromJson(data["appleAppStore"]))
                .WithGooglePlay(!data.Keys.Contains("googlePlay") || data["googlePlay"] == null ? null : Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent.FromJson(data["googlePlay"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["storeSubscriptionContentModelId"] = StoreSubscriptionContentModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["scheduleNamespaceId"] = ScheduleNamespaceId,
                ["triggerName"] = TriggerName,
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
            if (StoreSubscriptionContentModelId != null) {
                writer.WritePropertyName("storeSubscriptionContentModelId");
                writer.Write(StoreSubscriptionContentModelId.ToString());
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
            if (ScheduleNamespaceId != null) {
                writer.WritePropertyName("scheduleNamespaceId");
                writer.Write(ScheduleNamespaceId.ToString());
            }
            if (TriggerName != null) {
                writer.WritePropertyName("triggerName");
                writer.Write(TriggerName.ToString());
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
            var other = obj as StoreSubscriptionContentModelMaster;
            var diff = 0;
            if (StoreSubscriptionContentModelId == null && StoreSubscriptionContentModelId == other.StoreSubscriptionContentModelId)
            {
                // null and null
            }
            else
            {
                diff += StoreSubscriptionContentModelId.CompareTo(other.StoreSubscriptionContentModelId);
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
            if (ScheduleNamespaceId == null && ScheduleNamespaceId == other.ScheduleNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += ScheduleNamespaceId.CompareTo(other.ScheduleNamespaceId);
            }
            if (TriggerName == null && TriggerName == other.TriggerName)
            {
                // null and null
            }
            else
            {
                diff += TriggerName.CompareTo(other.TriggerName);
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
                if (StoreSubscriptionContentModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.storeSubscriptionContentModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (ScheduleNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.scheduleNamespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (TriggerName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.triggerName.error.tooLong"),
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
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModelMaster", "money2.storeSubscriptionContentModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new StoreSubscriptionContentModelMaster {
                StoreSubscriptionContentModelId = StoreSubscriptionContentModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                ScheduleNamespaceId = ScheduleNamespaceId,
                TriggerName = TriggerName,
                AppleAppStore = AppleAppStore.Clone() as Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent,
                GooglePlay = GooglePlay.Clone() as Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
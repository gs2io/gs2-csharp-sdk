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

namespace Gs2.Gs2Idle.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class CategoryModelMaster : IComparable
	{
        public string CategoryModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public int? RewardIntervalMinutes { set; get; } = null!;
        public int? DefaultMaximumIdleMinutes { set; get; } = null!;
        public Gs2.Gs2Idle.Model.AcquireActionList[] AcquireActions { set; get; } = null!;
        public string IdlePeriodScheduleId { set; get; } = null!;
        public string ReceivePeriodScheduleId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public CategoryModelMaster WithCategoryModelId(string categoryModelId) {
            this.CategoryModelId = categoryModelId;
            return this;
        }
        public CategoryModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public CategoryModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CategoryModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CategoryModelMaster WithRewardIntervalMinutes(int? rewardIntervalMinutes) {
            this.RewardIntervalMinutes = rewardIntervalMinutes;
            return this;
        }
        public CategoryModelMaster WithDefaultMaximumIdleMinutes(int? defaultMaximumIdleMinutes) {
            this.DefaultMaximumIdleMinutes = defaultMaximumIdleMinutes;
            return this;
        }
        public CategoryModelMaster WithAcquireActions(Gs2.Gs2Idle.Model.AcquireActionList[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }
        public CategoryModelMaster WithIdlePeriodScheduleId(string idlePeriodScheduleId) {
            this.IdlePeriodScheduleId = idlePeriodScheduleId;
            return this;
        }
        public CategoryModelMaster WithReceivePeriodScheduleId(string receivePeriodScheduleId) {
            this.ReceivePeriodScheduleId = receivePeriodScheduleId;
            return this;
        }
        public CategoryModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public CategoryModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public CategoryModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):model:(?<categoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):model:(?<categoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):model:(?<categoryName>.+)",
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

        private static System.Text.RegularExpressions.Regex _categoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):model:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCategoryNameFromGrn(
            string grn
        )
        {
            var match = _categoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["categoryName"].Success)
            {
                return null;
            }
            return match.Groups["categoryName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CategoryModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CategoryModelMaster()
                .WithCategoryModelId(!data.Keys.Contains("categoryModelId") || data["categoryModelId"] == null ? null : data["categoryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithRewardIntervalMinutes(!data.Keys.Contains("rewardIntervalMinutes") || data["rewardIntervalMinutes"] == null ? null : (int?)(data["rewardIntervalMinutes"].ToString().Contains(".") ? (int)double.Parse(data["rewardIntervalMinutes"].ToString()) : int.Parse(data["rewardIntervalMinutes"].ToString())))
                .WithDefaultMaximumIdleMinutes(!data.Keys.Contains("defaultMaximumIdleMinutes") || data["defaultMaximumIdleMinutes"] == null ? null : (int?)(data["defaultMaximumIdleMinutes"].ToString().Contains(".") ? (int)double.Parse(data["defaultMaximumIdleMinutes"].ToString()) : int.Parse(data["defaultMaximumIdleMinutes"].ToString())))
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Idle.Model.AcquireActionList.FromJson(v);
                }).ToArray())
                .WithIdlePeriodScheduleId(!data.Keys.Contains("idlePeriodScheduleId") || data["idlePeriodScheduleId"] == null ? null : data["idlePeriodScheduleId"].ToString())
                .WithReceivePeriodScheduleId(!data.Keys.Contains("receivePeriodScheduleId") || data["receivePeriodScheduleId"] == null ? null : data["receivePeriodScheduleId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["categoryModelId"] = CategoryModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["rewardIntervalMinutes"] = RewardIntervalMinutes,
                ["defaultMaximumIdleMinutes"] = DefaultMaximumIdleMinutes,
                ["acquireActions"] = acquireActionsJsonData,
                ["idlePeriodScheduleId"] = IdlePeriodScheduleId,
                ["receivePeriodScheduleId"] = ReceivePeriodScheduleId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CategoryModelId != null) {
                writer.WritePropertyName("categoryModelId");
                writer.Write(CategoryModelId.ToString());
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
            if (RewardIntervalMinutes != null) {
                writer.WritePropertyName("rewardIntervalMinutes");
                writer.Write((RewardIntervalMinutes.ToString().Contains(".") ? (int)double.Parse(RewardIntervalMinutes.ToString()) : int.Parse(RewardIntervalMinutes.ToString())));
            }
            if (DefaultMaximumIdleMinutes != null) {
                writer.WritePropertyName("defaultMaximumIdleMinutes");
                writer.Write((DefaultMaximumIdleMinutes.ToString().Contains(".") ? (int)double.Parse(DefaultMaximumIdleMinutes.ToString()) : int.Parse(DefaultMaximumIdleMinutes.ToString())));
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (IdlePeriodScheduleId != null) {
                writer.WritePropertyName("idlePeriodScheduleId");
                writer.Write(IdlePeriodScheduleId.ToString());
            }
            if (ReceivePeriodScheduleId != null) {
                writer.WritePropertyName("receivePeriodScheduleId");
                writer.Write(ReceivePeriodScheduleId.ToString());
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
            var other = obj as CategoryModelMaster;
            var diff = 0;
            if (CategoryModelId == null && CategoryModelId == other.CategoryModelId)
            {
                // null and null
            }
            else
            {
                diff += CategoryModelId.CompareTo(other.CategoryModelId);
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
            if (RewardIntervalMinutes == null && RewardIntervalMinutes == other.RewardIntervalMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(RewardIntervalMinutes - other.RewardIntervalMinutes);
            }
            if (DefaultMaximumIdleMinutes == null && DefaultMaximumIdleMinutes == other.DefaultMaximumIdleMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(DefaultMaximumIdleMinutes - other.DefaultMaximumIdleMinutes);
            }
            if (AcquireActions == null && AcquireActions == other.AcquireActions)
            {
                // null and null
            }
            else
            {
                diff += AcquireActions.Length - other.AcquireActions.Length;
                for (var i = 0; i < AcquireActions.Length; i++)
                {
                    diff += AcquireActions[i].CompareTo(other.AcquireActions[i]);
                }
            }
            if (IdlePeriodScheduleId == null && IdlePeriodScheduleId == other.IdlePeriodScheduleId)
            {
                // null and null
            }
            else
            {
                diff += IdlePeriodScheduleId.CompareTo(other.IdlePeriodScheduleId);
            }
            if (ReceivePeriodScheduleId == null && ReceivePeriodScheduleId == other.ReceivePeriodScheduleId)
            {
                // null and null
            }
            else
            {
                diff += ReceivePeriodScheduleId.CompareTo(other.ReceivePeriodScheduleId);
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
                if (CategoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.categoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (RewardIntervalMinutes < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.rewardIntervalMinutes.error.invalid"),
                    });
                }
                if (RewardIntervalMinutes > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.rewardIntervalMinutes.error.invalid"),
                    });
                }
            }
            {
                if (DefaultMaximumIdleMinutes < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.defaultMaximumIdleMinutes.error.invalid"),
                    });
                }
                if (DefaultMaximumIdleMinutes > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.defaultMaximumIdleMinutes.error.invalid"),
                    });
                }
            }
            {
                if (AcquireActions.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.acquireActions.error.tooFew"),
                    });
                }
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.acquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (IdlePeriodScheduleId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.idlePeriodScheduleId.error.tooLong"),
                    });
                }
            }
            {
                if (ReceivePeriodScheduleId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.receivePeriodScheduleId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModelMaster", "idle.categoryModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new CategoryModelMaster {
                CategoryModelId = CategoryModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                RewardIntervalMinutes = RewardIntervalMinutes,
                DefaultMaximumIdleMinutes = DefaultMaximumIdleMinutes,
                AcquireActions = AcquireActions.Clone() as Gs2.Gs2Idle.Model.AcquireActionList[],
                IdlePeriodScheduleId = IdlePeriodScheduleId,
                ReceivePeriodScheduleId = ReceivePeriodScheduleId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
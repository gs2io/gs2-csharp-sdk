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
	public class Status : IComparable
	{
        public string StatusId { set; get; } = null!;
        public string CategoryName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public long? RandomSeed { set; get; } = null!;
        public int? IdleMinutes { set; get; } = null!;
        public long? NextRewardsAt { set; get; } = null!;
        public int? MaximumIdleMinutes { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Status WithStatusId(string statusId) {
            this.StatusId = statusId;
            return this;
        }
        public Status WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }
        public Status WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Status WithRandomSeed(long? randomSeed) {
            this.RandomSeed = randomSeed;
            return this;
        }
        public Status WithIdleMinutes(int? idleMinutes) {
            this.IdleMinutes = idleMinutes;
            return this;
        }
        public Status WithNextRewardsAt(long? nextRewardsAt) {
            this.NextRewardsAt = nextRewardsAt;
            return this;
        }
        public Status WithMaximumIdleMinutes(int? maximumIdleMinutes) {
            this.MaximumIdleMinutes = maximumIdleMinutes;
            return this;
        }
        public Status WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Status WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Status WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):user:(?<userId>.+):categoryModel:(?<categoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):user:(?<userId>.+):categoryModel:(?<categoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):user:(?<userId>.+):categoryModel:(?<categoryName>.+)",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):user:(?<userId>.+):categoryModel:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _categoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):user:(?<userId>.+):categoryModel:(?<categoryName>.+)",
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
        public static Status FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Status()
                .WithStatusId(!data.Keys.Contains("statusId") || data["statusId"] == null ? null : data["statusId"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRandomSeed(!data.Keys.Contains("randomSeed") || data["randomSeed"] == null ? null : (long?)(data["randomSeed"].ToString().Contains(".") ? (long)double.Parse(data["randomSeed"].ToString()) : long.Parse(data["randomSeed"].ToString())))
                .WithIdleMinutes(!data.Keys.Contains("idleMinutes") || data["idleMinutes"] == null ? null : (int?)(data["idleMinutes"].ToString().Contains(".") ? (int)double.Parse(data["idleMinutes"].ToString()) : int.Parse(data["idleMinutes"].ToString())))
                .WithNextRewardsAt(!data.Keys.Contains("nextRewardsAt") || data["nextRewardsAt"] == null ? null : (long?)(data["nextRewardsAt"].ToString().Contains(".") ? (long)double.Parse(data["nextRewardsAt"].ToString()) : long.Parse(data["nextRewardsAt"].ToString())))
                .WithMaximumIdleMinutes(!data.Keys.Contains("maximumIdleMinutes") || data["maximumIdleMinutes"] == null ? null : (int?)(data["maximumIdleMinutes"].ToString().Contains(".") ? (int)double.Parse(data["maximumIdleMinutes"].ToString()) : int.Parse(data["maximumIdleMinutes"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["statusId"] = StatusId,
                ["categoryName"] = CategoryName,
                ["userId"] = UserId,
                ["randomSeed"] = RandomSeed,
                ["idleMinutes"] = IdleMinutes,
                ["nextRewardsAt"] = NextRewardsAt,
                ["maximumIdleMinutes"] = MaximumIdleMinutes,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StatusId != null) {
                writer.WritePropertyName("statusId");
                writer.Write(StatusId.ToString());
            }
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RandomSeed != null) {
                writer.WritePropertyName("randomSeed");
                writer.Write((RandomSeed.ToString().Contains(".") ? (long)double.Parse(RandomSeed.ToString()) : long.Parse(RandomSeed.ToString())));
            }
            if (IdleMinutes != null) {
                writer.WritePropertyName("idleMinutes");
                writer.Write((IdleMinutes.ToString().Contains(".") ? (int)double.Parse(IdleMinutes.ToString()) : int.Parse(IdleMinutes.ToString())));
            }
            if (NextRewardsAt != null) {
                writer.WritePropertyName("nextRewardsAt");
                writer.Write((NextRewardsAt.ToString().Contains(".") ? (long)double.Parse(NextRewardsAt.ToString()) : long.Parse(NextRewardsAt.ToString())));
            }
            if (MaximumIdleMinutes != null) {
                writer.WritePropertyName("maximumIdleMinutes");
                writer.Write((MaximumIdleMinutes.ToString().Contains(".") ? (int)double.Parse(MaximumIdleMinutes.ToString()) : int.Parse(MaximumIdleMinutes.ToString())));
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
            var other = obj as Status;
            var diff = 0;
            if (StatusId == null && StatusId == other.StatusId)
            {
                // null and null
            }
            else
            {
                diff += StatusId.CompareTo(other.StatusId);
            }
            if (CategoryName == null && CategoryName == other.CategoryName)
            {
                // null and null
            }
            else
            {
                diff += CategoryName.CompareTo(other.CategoryName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (RandomSeed == null && RandomSeed == other.RandomSeed)
            {
                // null and null
            }
            else
            {
                diff += (int)(RandomSeed - other.RandomSeed);
            }
            if (IdleMinutes == null && IdleMinutes == other.IdleMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(IdleMinutes - other.IdleMinutes);
            }
            if (NextRewardsAt == null && NextRewardsAt == other.NextRewardsAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(NextRewardsAt - other.NextRewardsAt);
            }
            if (MaximumIdleMinutes == null && MaximumIdleMinutes == other.MaximumIdleMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumIdleMinutes - other.MaximumIdleMinutes);
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
                if (StatusId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.statusId.error.tooLong"),
                    });
                }
            }
            {
                if (CategoryName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.categoryName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.userId.error.tooLong"),
                    });
                }
            }
            {
                if (RandomSeed < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.randomSeed.error.invalid"),
                    });
                }
                if (RandomSeed > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.randomSeed.error.invalid"),
                    });
                }
            }
            {
                if (IdleMinutes < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.idleMinutes.error.invalid"),
                    });
                }
                if (IdleMinutes > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.idleMinutes.error.invalid"),
                    });
                }
            }
            {
                if (NextRewardsAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.nextRewardsAt.error.invalid"),
                    });
                }
                if (NextRewardsAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.nextRewardsAt.error.invalid"),
                    });
                }
            }
            {
                if (MaximumIdleMinutes < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.maximumIdleMinutes.error.invalid"),
                    });
                }
                if (MaximumIdleMinutes > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.maximumIdleMinutes.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "idle.status.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Status {
                StatusId = StatusId,
                CategoryName = CategoryName,
                UserId = UserId,
                RandomSeed = RandomSeed,
                IdleMinutes = IdleMinutes,
                NextRewardsAt = NextRewardsAt,
                MaximumIdleMinutes = MaximumIdleMinutes,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
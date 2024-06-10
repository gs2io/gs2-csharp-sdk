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

namespace Gs2.Gs2Experience.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Status : IComparable
	{
        public string StatusId { set; get; } = null!;
        public string ExperienceName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string PropertyId { set; get; } = null!;
        public long? ExperienceValue { set; get; } = null!;
        public long? RankValue { set; get; } = null!;
        public long? RankCapValue { set; get; } = null!;
        public long? NextRankUpExperienceValue { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Status WithStatusId(string statusId) {
            this.StatusId = statusId;
            return this;
        }
        public Status WithExperienceName(string experienceName) {
            this.ExperienceName = experienceName;
            return this;
        }
        public Status WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Status WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public Status WithExperienceValue(long? experienceValue) {
            this.ExperienceValue = experienceValue;
            return this;
        }
        public Status WithRankValue(long? rankValue) {
            this.RankValue = rankValue;
            return this;
        }
        public Status WithRankCapValue(long? rankCapValue) {
            this.RankCapValue = rankCapValue;
            return this;
        }
        public Status WithNextRankUpExperienceValue(long? nextRankUpExperienceValue) {
            this.NextRankUpExperienceValue = nextRankUpExperienceValue;
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):user:(?<userId>.+):experienceModel:(?<experienceName>.+):property:(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):user:(?<userId>.+):experienceModel:(?<experienceName>.+):property:(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):user:(?<userId>.+):experienceModel:(?<experienceName>.+):property:(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):user:(?<userId>.+):experienceModel:(?<experienceName>.+):property:(?<propertyId>.+)",
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

        private static System.Text.RegularExpressions.Regex _experienceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):user:(?<userId>.+):experienceModel:(?<experienceName>.+):property:(?<propertyId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetExperienceNameFromGrn(
            string grn
        )
        {
            var match = _experienceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["experienceName"].Success)
            {
                return null;
            }
            return match.Groups["experienceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _propertyIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):user:(?<userId>.+):experienceModel:(?<experienceName>.+):property:(?<propertyId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetPropertyIdFromGrn(
            string grn
        )
        {
            var match = _propertyIdRegex.Match(grn);
            if (!match.Success || !match.Groups["propertyId"].Success)
            {
                return null;
            }
            return match.Groups["propertyId"].Value;
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
                .WithExperienceName(!data.Keys.Contains("experienceName") || data["experienceName"] == null ? null : data["experienceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithExperienceValue(!data.Keys.Contains("experienceValue") || data["experienceValue"] == null ? null : (long?)(data["experienceValue"].ToString().Contains(".") ? (long)double.Parse(data["experienceValue"].ToString()) : long.Parse(data["experienceValue"].ToString())))
                .WithRankValue(!data.Keys.Contains("rankValue") || data["rankValue"] == null ? null : (long?)(data["rankValue"].ToString().Contains(".") ? (long)double.Parse(data["rankValue"].ToString()) : long.Parse(data["rankValue"].ToString())))
                .WithRankCapValue(!data.Keys.Contains("rankCapValue") || data["rankCapValue"] == null ? null : (long?)(data["rankCapValue"].ToString().Contains(".") ? (long)double.Parse(data["rankCapValue"].ToString()) : long.Parse(data["rankCapValue"].ToString())))
                .WithNextRankUpExperienceValue(!data.Keys.Contains("nextRankUpExperienceValue") || data["nextRankUpExperienceValue"] == null ? null : (long?)(data["nextRankUpExperienceValue"].ToString().Contains(".") ? (long)double.Parse(data["nextRankUpExperienceValue"].ToString()) : long.Parse(data["nextRankUpExperienceValue"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["statusId"] = StatusId,
                ["experienceName"] = ExperienceName,
                ["userId"] = UserId,
                ["propertyId"] = PropertyId,
                ["experienceValue"] = ExperienceValue,
                ["rankValue"] = RankValue,
                ["rankCapValue"] = RankCapValue,
                ["nextRankUpExperienceValue"] = NextRankUpExperienceValue,
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
            if (ExperienceName != null) {
                writer.WritePropertyName("experienceName");
                writer.Write(ExperienceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (ExperienceValue != null) {
                writer.WritePropertyName("experienceValue");
                writer.Write((ExperienceValue.ToString().Contains(".") ? (long)double.Parse(ExperienceValue.ToString()) : long.Parse(ExperienceValue.ToString())));
            }
            if (RankValue != null) {
                writer.WritePropertyName("rankValue");
                writer.Write((RankValue.ToString().Contains(".") ? (long)double.Parse(RankValue.ToString()) : long.Parse(RankValue.ToString())));
            }
            if (RankCapValue != null) {
                writer.WritePropertyName("rankCapValue");
                writer.Write((RankCapValue.ToString().Contains(".") ? (long)double.Parse(RankCapValue.ToString()) : long.Parse(RankCapValue.ToString())));
            }
            if (NextRankUpExperienceValue != null) {
                writer.WritePropertyName("nextRankUpExperienceValue");
                writer.Write((NextRankUpExperienceValue.ToString().Contains(".") ? (long)double.Parse(NextRankUpExperienceValue.ToString()) : long.Parse(NextRankUpExperienceValue.ToString())));
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
            if (ExperienceName == null && ExperienceName == other.ExperienceName)
            {
                // null and null
            }
            else
            {
                diff += ExperienceName.CompareTo(other.ExperienceName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (PropertyId == null && PropertyId == other.PropertyId)
            {
                // null and null
            }
            else
            {
                diff += PropertyId.CompareTo(other.PropertyId);
            }
            if (ExperienceValue == null && ExperienceValue == other.ExperienceValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExperienceValue - other.ExperienceValue);
            }
            if (RankValue == null && RankValue == other.RankValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(RankValue - other.RankValue);
            }
            if (RankCapValue == null && RankCapValue == other.RankCapValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(RankCapValue - other.RankCapValue);
            }
            if (NextRankUpExperienceValue == null && NextRankUpExperienceValue == other.NextRankUpExperienceValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(NextRankUpExperienceValue - other.NextRankUpExperienceValue);
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
                        new RequestError("status", "experience.status.statusId.error.tooLong"),
                    });
                }
            }
            {
                if (ExperienceName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.experienceName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.userId.error.tooLong"),
                    });
                }
            }
            {
                if (PropertyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.propertyId.error.tooLong"),
                    });
                }
            }
            {
                if (ExperienceValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.experienceValue.error.invalid"),
                    });
                }
                if (ExperienceValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.experienceValue.error.invalid"),
                    });
                }
            }
            {
                if (RankValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.rankValue.error.invalid"),
                    });
                }
                if (RankValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.rankValue.error.invalid"),
                    });
                }
            }
            {
                if (RankCapValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.rankCapValue.error.invalid"),
                    });
                }
                if (RankCapValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.rankCapValue.error.invalid"),
                    });
                }
            }
            {
                if (NextRankUpExperienceValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.nextRankUpExperienceValue.error.invalid"),
                    });
                }
                if (NextRankUpExperienceValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.nextRankUpExperienceValue.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "experience.status.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Status {
                StatusId = StatusId,
                ExperienceName = ExperienceName,
                UserId = UserId,
                PropertyId = PropertyId,
                ExperienceValue = ExperienceValue,
                RankValue = RankValue,
                RankCapValue = RankCapValue,
                NextRankUpExperienceValue = NextRankUpExperienceValue,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
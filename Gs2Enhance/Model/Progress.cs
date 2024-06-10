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

namespace Gs2.Gs2Enhance.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Progress : IComparable
	{
        public string ProgressId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string RateName { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string PropertyId { set; get; } = null!;
        public long? ExperienceValue { set; get; } = null!;
        public float? Rate { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Progress WithProgressId(string progressId) {
            this.ProgressId = progressId;
            return this;
        }
        public Progress WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Progress WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public Progress WithName(string name) {
            this.Name = name;
            return this;
        }
        public Progress WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public Progress WithExperienceValue(long? experienceValue) {
            this.ExperienceValue = experienceValue;
            return this;
        }
        public Progress WithRate(float? rate) {
            this.Rate = rate;
            return this;
        }
        public Progress WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Progress WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Progress WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):user:(?<userId>.+):progress",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):user:(?<userId>.+):progress",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):user:(?<userId>.+):progress",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):user:(?<userId>.+):progress",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Progress FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Progress()
                .WithProgressId(!data.Keys.Contains("progressId") || data["progressId"] == null ? null : data["progressId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithExperienceValue(!data.Keys.Contains("experienceValue") || data["experienceValue"] == null ? null : (long?)(data["experienceValue"].ToString().Contains(".") ? (long)double.Parse(data["experienceValue"].ToString()) : long.Parse(data["experienceValue"].ToString())))
                .WithRate(!data.Keys.Contains("rate") || data["rate"] == null ? null : (float?)float.Parse(data["rate"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["progressId"] = ProgressId,
                ["userId"] = UserId,
                ["rateName"] = RateName,
                ["name"] = Name,
                ["propertyId"] = PropertyId,
                ["experienceValue"] = ExperienceValue,
                ["rate"] = Rate,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ProgressId != null) {
                writer.WritePropertyName("progressId");
                writer.Write(ProgressId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (ExperienceValue != null) {
                writer.WritePropertyName("experienceValue");
                writer.Write((ExperienceValue.ToString().Contains(".") ? (long)double.Parse(ExperienceValue.ToString()) : long.Parse(ExperienceValue.ToString())));
            }
            if (Rate != null) {
                writer.WritePropertyName("rate");
                writer.Write(float.Parse(Rate.ToString()));
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
            var other = obj as Progress;
            var diff = 0;
            if (ProgressId == null && ProgressId == other.ProgressId)
            {
                // null and null
            }
            else
            {
                diff += ProgressId.CompareTo(other.ProgressId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (RateName == null && RateName == other.RateName)
            {
                // null and null
            }
            else
            {
                diff += RateName.CompareTo(other.RateName);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
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
            if (Rate == null && Rate == other.Rate)
            {
                // null and null
            }
            else
            {
                diff += (int)(Rate - other.Rate);
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
                if (ProgressId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.progressId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.userId.error.tooLong"),
                    });
                }
            }
            {
                if (RateName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.rateName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.name.error.tooLong"),
                    });
                }
            }
            {
                if (PropertyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.propertyId.error.tooLong"),
                    });
                }
            }
            {
                if (ExperienceValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.experienceValue.error.invalid"),
                    });
                }
                if (ExperienceValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.experienceValue.error.invalid"),
                    });
                }
            }
            {
                if (Rate < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.rate.error.invalid"),
                    });
                }
                if (Rate > 100.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.rate.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "enhance.progress.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Progress {
                ProgressId = ProgressId,
                UserId = UserId,
                RateName = RateName,
                Name = Name,
                PropertyId = PropertyId,
                ExperienceValue = ExperienceValue,
                Rate = Rate,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
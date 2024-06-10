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

namespace Gs2.Gs2Grade.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Status : IComparable
	{
        public string StatusId { set; get; } = null!;
        public string GradeName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string PropertyId { set; get; } = null!;
        public long? GradeValue { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Status WithStatusId(string statusId) {
            this.StatusId = statusId;
            return this;
        }
        public Status WithGradeName(string gradeName) {
            this.GradeName = gradeName;
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
        public Status WithGradeValue(long? gradeValue) {
            this.GradeValue = gradeValue;
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):user:(?<userId>.+):gradeModel:(?<gradeName>.+):property:(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):user:(?<userId>.+):gradeModel:(?<gradeName>.+):property:(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):user:(?<userId>.+):gradeModel:(?<gradeName>.+):property:(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):user:(?<userId>.+):gradeModel:(?<gradeName>.+):property:(?<propertyId>.+)",
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

        private static System.Text.RegularExpressions.Regex _gradeNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):user:(?<userId>.+):gradeModel:(?<gradeName>.+):property:(?<propertyId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetGradeNameFromGrn(
            string grn
        )
        {
            var match = _gradeNameRegex.Match(grn);
            if (!match.Success || !match.Groups["gradeName"].Success)
            {
                return null;
            }
            return match.Groups["gradeName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _propertyIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):user:(?<userId>.+):gradeModel:(?<gradeName>.+):property:(?<propertyId>.+)",
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
                .WithGradeName(!data.Keys.Contains("gradeName") || data["gradeName"] == null ? null : data["gradeName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithGradeValue(!data.Keys.Contains("gradeValue") || data["gradeValue"] == null ? null : (long?)(data["gradeValue"].ToString().Contains(".") ? (long)double.Parse(data["gradeValue"].ToString()) : long.Parse(data["gradeValue"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["statusId"] = StatusId,
                ["gradeName"] = GradeName,
                ["userId"] = UserId,
                ["propertyId"] = PropertyId,
                ["gradeValue"] = GradeValue,
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
            if (GradeName != null) {
                writer.WritePropertyName("gradeName");
                writer.Write(GradeName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (GradeValue != null) {
                writer.WritePropertyName("gradeValue");
                writer.Write((GradeValue.ToString().Contains(".") ? (long)double.Parse(GradeValue.ToString()) : long.Parse(GradeValue.ToString())));
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
            if (GradeName == null && GradeName == other.GradeName)
            {
                // null and null
            }
            else
            {
                diff += GradeName.CompareTo(other.GradeName);
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
            if (GradeValue == null && GradeValue == other.GradeValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(GradeValue - other.GradeValue);
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
                        new RequestError("status", "grade.status.statusId.error.tooLong"),
                    });
                }
            }
            {
                if (GradeName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.gradeName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.userId.error.tooLong"),
                    });
                }
            }
            {
                if (PropertyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.propertyId.error.tooLong"),
                    });
                }
            }
            {
                if (GradeValue < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.gradeValue.error.invalid"),
                    });
                }
                if (GradeValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.gradeValue.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "grade.status.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Status {
                StatusId = StatusId,
                GradeName = GradeName,
                UserId = UserId,
                PropertyId = PropertyId,
                GradeValue = GradeValue,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Freeze.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Stage : IComparable
	{
        public string StageId { set; get; }
        public string Name { set; get; }
        public string SourceStageName { set; get; }
        public int? SortNumber { set; get; }
        public string Status { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public Stage WithStageId(string stageId) {
            this.StageId = stageId;
            return this;
        }
        public Stage WithName(string name) {
            this.Name = name;
            return this;
        }
        public Stage WithSourceStageName(string sourceStageName) {
            this.SourceStageName = sourceStageName;
            return this;
        }
        public Stage WithSortNumber(int? sortNumber) {
            this.SortNumber = sortNumber;
            return this;
        }
        public Stage WithStatus(string status) {
            this.Status = status;
            return this;
        }
        public Stage WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Stage WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Stage WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):freeze:(?<stageName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):freeze:(?<stageName>.+)",
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

        private static System.Text.RegularExpressions.Regex _stageNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):freeze:(?<stageName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetStageNameFromGrn(
            string grn
        )
        {
            var match = _stageNameRegex.Match(grn);
            if (!match.Success || !match.Groups["stageName"].Success)
            {
                return null;
            }
            return match.Groups["stageName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Stage FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Stage()
                .WithStageId(!data.Keys.Contains("stageId") || data["stageId"] == null ? null : data["stageId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithSourceStageName(!data.Keys.Contains("sourceStageName") || data["sourceStageName"] == null ? null : data["sourceStageName"].ToString())
                .WithSortNumber(!data.Keys.Contains("sortNumber") || data["sortNumber"] == null ? null : (int?)(data["sortNumber"].ToString().Contains(".") ? (int)double.Parse(data["sortNumber"].ToString()) : int.Parse(data["sortNumber"].ToString())))
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["stageId"] = StageId,
                ["name"] = Name,
                ["sourceStageName"] = SourceStageName,
                ["sortNumber"] = SortNumber,
                ["status"] = Status,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StageId != null) {
                writer.WritePropertyName("stageId");
                writer.Write(StageId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (SourceStageName != null) {
                writer.WritePropertyName("sourceStageName");
                writer.Write(SourceStageName.ToString());
            }
            if (SortNumber != null) {
                writer.WritePropertyName("sortNumber");
                writer.Write((SortNumber.ToString().Contains(".") ? (int)double.Parse(SortNumber.ToString()) : int.Parse(SortNumber.ToString())));
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
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
            var other = obj as Stage;
            var diff = 0;
            if (StageId == null && StageId == other.StageId)
            {
                // null and null
            }
            else
            {
                diff += StageId.CompareTo(other.StageId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (SourceStageName == null && SourceStageName == other.SourceStageName)
            {
                // null and null
            }
            else
            {
                diff += SourceStageName.CompareTo(other.SourceStageName);
            }
            if (SortNumber == null && SortNumber == other.SortNumber)
            {
                // null and null
            }
            else
            {
                diff += (int)(SortNumber - other.SortNumber);
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
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
                if (StageId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.stageId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.name.error.tooLong"),
                    });
                }
            }
            {
                if (SourceStageName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.sourceStageName.error.tooLong"),
                    });
                }
            }
            {
                if (SortNumber < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.sortNumber.error.invalid"),
                    });
                }
                if (SortNumber > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.sortNumber.error.invalid"),
                    });
                }
            }
            {
                switch (Status) {
                    case "Active":
                    case "Updating":
                    case "UpdateFailed":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("stage", "freeze.stage.status.error.invalid"),
                        });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stage", "freeze.stage.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Stage {
                StageId = StageId,
                Name = Name,
                SourceStageName = SourceStageName,
                SortNumber = SortNumber,
                Status = Status,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
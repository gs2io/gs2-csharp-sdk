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

namespace Gs2.Gs2Quest.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class CompletedQuestList : IComparable
	{
        public string CompletedQuestListId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string QuestGroupName { set; get; } = null!;
        public string[] CompleteQuestNames { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public CompletedQuestList WithCompletedQuestListId(string completedQuestListId) {
            this.CompletedQuestListId = completedQuestListId;
            return this;
        }
        public CompletedQuestList WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public CompletedQuestList WithQuestGroupName(string questGroupName) {
            this.QuestGroupName = questGroupName;
            return this;
        }
        public CompletedQuestList WithCompleteQuestNames(string[] completeQuestNames) {
            this.CompleteQuestNames = completeQuestNames;
            return this;
        }
        public CompletedQuestList WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public CompletedQuestList WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public CompletedQuestList WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):completed:group:(?<questGroupName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):completed:group:(?<questGroupName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):completed:group:(?<questGroupName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):completed:group:(?<questGroupName>.+)",
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

        private static System.Text.RegularExpressions.Regex _questGroupNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):completed:group:(?<questGroupName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetQuestGroupNameFromGrn(
            string grn
        )
        {
            var match = _questGroupNameRegex.Match(grn);
            if (!match.Success || !match.Groups["questGroupName"].Success)
            {
                return null;
            }
            return match.Groups["questGroupName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CompletedQuestList FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CompletedQuestList()
                .WithCompletedQuestListId(!data.Keys.Contains("completedQuestListId") || data["completedQuestListId"] == null ? null : data["completedQuestListId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithQuestGroupName(!data.Keys.Contains("questGroupName") || data["questGroupName"] == null ? null : data["questGroupName"].ToString())
                .WithCompleteQuestNames(!data.Keys.Contains("completeQuestNames") || data["completeQuestNames"] == null || !data["completeQuestNames"].IsArray ? null : data["completeQuestNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData completeQuestNamesJsonData = null;
            if (CompleteQuestNames != null && CompleteQuestNames.Length > 0)
            {
                completeQuestNamesJsonData = new JsonData();
                foreach (var completeQuestName in CompleteQuestNames)
                {
                    completeQuestNamesJsonData.Add(completeQuestName);
                }
            }
            return new JsonData {
                ["completedQuestListId"] = CompletedQuestListId,
                ["userId"] = UserId,
                ["questGroupName"] = QuestGroupName,
                ["completeQuestNames"] = completeQuestNamesJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CompletedQuestListId != null) {
                writer.WritePropertyName("completedQuestListId");
                writer.Write(CompletedQuestListId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (QuestGroupName != null) {
                writer.WritePropertyName("questGroupName");
                writer.Write(QuestGroupName.ToString());
            }
            if (CompleteQuestNames != null) {
                writer.WritePropertyName("completeQuestNames");
                writer.WriteArrayStart();
                foreach (var completeQuestName in CompleteQuestNames)
                {
                    if (completeQuestName != null) {
                        writer.Write(completeQuestName.ToString());
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
            var other = obj as CompletedQuestList;
            var diff = 0;
            if (CompletedQuestListId == null && CompletedQuestListId == other.CompletedQuestListId)
            {
                // null and null
            }
            else
            {
                diff += CompletedQuestListId.CompareTo(other.CompletedQuestListId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (QuestGroupName == null && QuestGroupName == other.QuestGroupName)
            {
                // null and null
            }
            else
            {
                diff += QuestGroupName.CompareTo(other.QuestGroupName);
            }
            if (CompleteQuestNames == null && CompleteQuestNames == other.CompleteQuestNames)
            {
                // null and null
            }
            else
            {
                diff += CompleteQuestNames.Length - other.CompleteQuestNames.Length;
                for (var i = 0; i < CompleteQuestNames.Length; i++)
                {
                    diff += CompleteQuestNames[i].CompareTo(other.CompleteQuestNames[i]);
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
                if (CompletedQuestListId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.completedQuestListId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.userId.error.tooLong"),
                    });
                }
            }
            {
                if (QuestGroupName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.questGroupName.error.tooLong"),
                    });
                }
            }
            {
                if (CompleteQuestNames.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.completeQuestNames.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("completedQuestList", "quest.completedQuestList.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new CompletedQuestList {
                CompletedQuestListId = CompletedQuestListId,
                UserId = UserId,
                QuestGroupName = QuestGroupName,
                CompleteQuestNames = CompleteQuestNames.Clone() as string[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
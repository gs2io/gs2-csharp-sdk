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
using UnityEngine.Scripting;

namespace Gs2.Gs2Quest.Model
{

	[Preserve]
	public class CompletedQuestList : IComparable
	{
        public string CompletedQuestListId { set; get; }
        public string UserId { set; get; }
        public string QuestGroupName { set; get; }
        public string[] CompleteQuestNames { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

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

    	[Preserve]
        public static CompletedQuestList FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CompletedQuestList()
                .WithCompletedQuestListId(!data.Keys.Contains("completedQuestListId") || data["completedQuestListId"] == null ? null : data["completedQuestListId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithQuestGroupName(!data.Keys.Contains("questGroupName") || data["questGroupName"] == null ? null : data["questGroupName"].ToString())
                .WithCompleteQuestNames(!data.Keys.Contains("completeQuestNames") || data["completeQuestNames"] == null ? new string[]{} : data["completeQuestNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["completedQuestListId"] = CompletedQuestListId,
                ["userId"] = UserId,
                ["questGroupName"] = QuestGroupName,
                ["completeQuestNames"] = new JsonData(CompleteQuestNames == null ? new JsonData[]{} :
                        CompleteQuestNames.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
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
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
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
            return diff;
        }
    }
}
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
	public class QuestGroupModel : IComparable
	{
        public string QuestGroupModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Quest.Model.QuestModel[] Quests { set; get; } = null!;
        public string ChallengePeriodEventId { set; get; } = null!;
        public QuestGroupModel WithQuestGroupModelId(string questGroupModelId) {
            this.QuestGroupModelId = questGroupModelId;
            return this;
        }
        public QuestGroupModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public QuestGroupModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public QuestGroupModel WithQuests(Gs2.Gs2Quest.Model.QuestModel[] quests) {
            this.Quests = quests;
            return this;
        }
        public QuestGroupModel WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+)",
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

        private static System.Text.RegularExpressions.Regex _questGroupNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+)",
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
        public static QuestGroupModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new QuestGroupModel()
                .WithQuestGroupModelId(!data.Keys.Contains("questGroupModelId") || data["questGroupModelId"] == null ? null : data["questGroupModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithQuests(!data.Keys.Contains("quests") || data["quests"] == null || !data["quests"].IsArray ? null : data["quests"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.QuestModel.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData questsJsonData = null;
            if (Quests != null && Quests.Length > 0)
            {
                questsJsonData = new JsonData();
                foreach (var quest in Quests)
                {
                    questsJsonData.Add(quest.ToJson());
                }
            }
            return new JsonData {
                ["questGroupModelId"] = QuestGroupModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["quests"] = questsJsonData,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (QuestGroupModelId != null) {
                writer.WritePropertyName("questGroupModelId");
                writer.Write(QuestGroupModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Quests != null) {
                writer.WritePropertyName("quests");
                writer.WriteArrayStart();
                foreach (var quest in Quests)
                {
                    if (quest != null) {
                        quest.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ChallengePeriodEventId != null) {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(ChallengePeriodEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as QuestGroupModel;
            var diff = 0;
            if (QuestGroupModelId == null && QuestGroupModelId == other.QuestGroupModelId)
            {
                // null and null
            }
            else
            {
                diff += QuestGroupModelId.CompareTo(other.QuestGroupModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (Quests == null && Quests == other.Quests)
            {
                // null and null
            }
            else
            {
                diff += Quests.Length - other.Quests.Length;
                for (var i = 0; i < Quests.Length; i++)
                {
                    diff += Quests[i].CompareTo(other.Quests[i]);
                }
            }
            if (ChallengePeriodEventId == null && ChallengePeriodEventId == other.ChallengePeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += ChallengePeriodEventId.CompareTo(other.ChallengePeriodEventId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (QuestGroupModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questGroupModel", "quest.questGroupModel.questGroupModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questGroupModel", "quest.questGroupModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questGroupModel", "quest.questGroupModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Quests.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questGroupModel", "quest.questGroupModel.quests.error.tooMany"),
                    });
                }
            }
            {
                if (ChallengePeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questGroupModel", "quest.questGroupModel.challengePeriodEventId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new QuestGroupModel {
                QuestGroupModelId = QuestGroupModelId,
                Name = Name,
                Metadata = Metadata,
                Quests = Quests.Clone() as Gs2.Gs2Quest.Model.QuestModel[],
                ChallengePeriodEventId = ChallengePeriodEventId,
            };
        }
    }
}
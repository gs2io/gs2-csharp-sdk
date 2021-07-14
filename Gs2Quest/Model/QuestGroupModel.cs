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
	public class QuestGroupModel : IComparable
	{
        public string QuestGroupModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Quest.Model.QuestModel[] Quests { set; get; }
        public string ChallengePeriodEventId { set; get; }

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

    	[Preserve]
        public static QuestGroupModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new QuestGroupModel()
                .WithQuestGroupModelId(!data.Keys.Contains("questGroupModelId") || data["questGroupModelId"] == null ? null : data["questGroupModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithQuests(!data.Keys.Contains("quests") || data["quests"] == null ? new Gs2.Gs2Quest.Model.QuestModel[]{} : data["quests"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.QuestModel.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["questGroupModelId"] = QuestGroupModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["quests"] = new JsonData(Quests == null ? new JsonData[]{} :
                        Quests.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
    }
}
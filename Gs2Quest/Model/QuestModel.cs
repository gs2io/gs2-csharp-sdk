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
	public class QuestModel : IComparable
	{
        public string QuestModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Quest.Model.Contents[] Contents { set; get; }
        public string ChallengePeriodEventId { set; get; }
        public Gs2.Gs2Quest.Model.ConsumeAction[] ConsumeActions { set; get; }
        public Gs2.Gs2Quest.Model.AcquireAction[] FailedAcquireActions { set; get; }
        public string[] PremiseQuestNames { set; get; }

        public QuestModel WithQuestModelId(string questModelId) {
            this.QuestModelId = questModelId;
            return this;
        }

        public QuestModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public QuestModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public QuestModel WithContents(Gs2.Gs2Quest.Model.Contents[] contents) {
            this.Contents = contents;
            return this;
        }

        public QuestModel WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }

        public QuestModel WithConsumeActions(Gs2.Gs2Quest.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }

        public QuestModel WithFailedAcquireActions(Gs2.Gs2Quest.Model.AcquireAction[] failedAcquireActions) {
            this.FailedAcquireActions = failedAcquireActions;
            return this;
        }

        public QuestModel WithPremiseQuestNames(string[] premiseQuestNames) {
            this.PremiseQuestNames = premiseQuestNames;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static QuestModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new QuestModel()
                .WithQuestModelId(!data.Keys.Contains("questModelId") || data["questModelId"] == null ? null : data["questModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithContents(!data.Keys.Contains("contents") || data["contents"] == null ? new Gs2.Gs2Quest.Model.Contents[]{} : data["contents"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Contents.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null ? new Gs2.Gs2Quest.Model.ConsumeAction[]{} : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithFailedAcquireActions(!data.Keys.Contains("failedAcquireActions") || data["failedAcquireActions"] == null ? new Gs2.Gs2Quest.Model.AcquireAction[]{} : data["failedAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithPremiseQuestNames(!data.Keys.Contains("premiseQuestNames") || data["premiseQuestNames"] == null ? new string[]{} : data["premiseQuestNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["questModelId"] = QuestModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["contents"] = new JsonData(Contents == null ? new JsonData[]{} :
                        Contents.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["consumeActions"] = new JsonData(ConsumeActions == null ? new JsonData[]{} :
                        ConsumeActions.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["failedAcquireActions"] = new JsonData(FailedAcquireActions == null ? new JsonData[]{} :
                        FailedAcquireActions.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["premiseQuestNames"] = new JsonData(PremiseQuestNames == null ? new JsonData[]{} :
                        PremiseQuestNames.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (QuestModelId != null) {
                writer.WritePropertyName("questModelId");
                writer.Write(QuestModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Contents != null) {
                writer.WritePropertyName("contents");
                writer.WriteArrayStart();
                foreach (var content in Contents)
                {
                    if (content != null) {
                        content.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ChallengePeriodEventId != null) {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(ChallengePeriodEventId.ToString());
            }
            if (ConsumeActions != null) {
                writer.WritePropertyName("consumeActions");
                writer.WriteArrayStart();
                foreach (var consumeAction in ConsumeActions)
                {
                    if (consumeAction != null) {
                        consumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (FailedAcquireActions != null) {
                writer.WritePropertyName("failedAcquireActions");
                writer.WriteArrayStart();
                foreach (var failedAcquireAction in FailedAcquireActions)
                {
                    if (failedAcquireAction != null) {
                        failedAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (PremiseQuestNames != null) {
                writer.WritePropertyName("premiseQuestNames");
                writer.WriteArrayStart();
                foreach (var premiseQuestName in PremiseQuestNames)
                {
                    if (premiseQuestName != null) {
                        writer.Write(premiseQuestName.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as QuestModel;
            var diff = 0;
            if (QuestModelId == null && QuestModelId == other.QuestModelId)
            {
                // null and null
            }
            else
            {
                diff += QuestModelId.CompareTo(other.QuestModelId);
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
            if (Contents == null && Contents == other.Contents)
            {
                // null and null
            }
            else
            {
                diff += Contents.Length - other.Contents.Length;
                for (var i = 0; i < Contents.Length; i++)
                {
                    diff += Contents[i].CompareTo(other.Contents[i]);
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
            if (ConsumeActions == null && ConsumeActions == other.ConsumeActions)
            {
                // null and null
            }
            else
            {
                diff += ConsumeActions.Length - other.ConsumeActions.Length;
                for (var i = 0; i < ConsumeActions.Length; i++)
                {
                    diff += ConsumeActions[i].CompareTo(other.ConsumeActions[i]);
                }
            }
            if (FailedAcquireActions == null && FailedAcquireActions == other.FailedAcquireActions)
            {
                // null and null
            }
            else
            {
                diff += FailedAcquireActions.Length - other.FailedAcquireActions.Length;
                for (var i = 0; i < FailedAcquireActions.Length; i++)
                {
                    diff += FailedAcquireActions[i].CompareTo(other.FailedAcquireActions[i]);
                }
            }
            if (PremiseQuestNames == null && PremiseQuestNames == other.PremiseQuestNames)
            {
                // null and null
            }
            else
            {
                diff += PremiseQuestNames.Length - other.PremiseQuestNames.Length;
                for (var i = 0; i < PremiseQuestNames.Length; i++)
                {
                    diff += PremiseQuestNames[i].CompareTo(other.PremiseQuestNames[i]);
                }
            }
            return diff;
        }
    }
}
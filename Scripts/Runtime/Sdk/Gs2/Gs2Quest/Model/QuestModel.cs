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
	public class QuestModel : IComparable
	{

        /** クエストモデル */
        public string questModelId { set; get; }

        /**
         * クエストモデルを設定
         *
         * @param questModelId クエストモデル
         * @return this
         */
        public QuestModel WithQuestModelId(string questModelId) {
            this.questModelId = questModelId;
            return this;
        }

        /** クエストモデル名 */
        public string name { set; get; }

        /**
         * クエストモデル名を設定
         *
         * @param name クエストモデル名
         * @return this
         */
        public QuestModel WithName(string name) {
            this.name = name;
            return this;
        }

        /** クエストモデルのメタデータ */
        public string metadata { set; get; }

        /**
         * クエストモデルのメタデータを設定
         *
         * @param metadata クエストモデルのメタデータ
         * @return this
         */
        public QuestModel WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }

        /** クエストの内容 */
        public List<Contents> contents { set; get; }

        /**
         * クエストの内容を設定
         *
         * @param contents クエストの内容
         * @return this
         */
        public QuestModel WithContents(List<Contents> contents) {
            this.contents = contents;
            return this;
        }

        /** 挑戦可能な期間を指定するイベントマスター のGRN */
        public string challengePeriodEventId { set; get; }

        /**
         * 挑戦可能な期間を指定するイベントマスター のGRNを設定
         *
         * @param challengePeriodEventId 挑戦可能な期間を指定するイベントマスター のGRN
         * @return this
         */
        public QuestModel WithChallengePeriodEventId(string challengePeriodEventId) {
            this.challengePeriodEventId = challengePeriodEventId;
            return this;
        }

        /** クエストの参加料 */
        public List<ConsumeAction> consumeActions { set; get; }

        /**
         * クエストの参加料を設定
         *
         * @param consumeActions クエストの参加料
         * @return this
         */
        public QuestModel WithConsumeActions(List<ConsumeAction> consumeActions) {
            this.consumeActions = consumeActions;
            return this;
        }

        /** クエスト失敗時の報酬 */
        public List<AcquireAction> failedAcquireActions { set; get; }

        /**
         * クエスト失敗時の報酬を設定
         *
         * @param failedAcquireActions クエスト失敗時の報酬
         * @return this
         */
        public QuestModel WithFailedAcquireActions(List<AcquireAction> failedAcquireActions) {
            this.failedAcquireActions = failedAcquireActions;
            return this;
        }

        /** クエストに挑戦するためにクリアしておく必要のあるクエスト名 */
        public List<string> premiseQuestNames { set; get; }

        /**
         * クエストに挑戦するためにクリアしておく必要のあるクエスト名を設定
         *
         * @param premiseQuestNames クエストに挑戦するためにクリアしておく必要のあるクエスト名
         * @return this
         */
        public QuestModel WithPremiseQuestNames(List<string> premiseQuestNames) {
            this.premiseQuestNames = premiseQuestNames;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.questModelId != null)
            {
                writer.WritePropertyName("questModelId");
                writer.Write(this.questModelId);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.metadata != null)
            {
                writer.WritePropertyName("metadata");
                writer.Write(this.metadata);
            }
            if(this.contents != null)
            {
                writer.WritePropertyName("contents");
                writer.WriteArrayStart();
                foreach(var item in this.contents)
                {
                    item.WriteJson(writer);
                }
                writer.WriteArrayEnd();
            }
            if(this.challengePeriodEventId != null)
            {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(this.challengePeriodEventId);
            }
            if(this.consumeActions != null)
            {
                writer.WritePropertyName("consumeActions");
                writer.WriteArrayStart();
                foreach(var item in this.consumeActions)
                {
                    item.WriteJson(writer);
                }
                writer.WriteArrayEnd();
            }
            if(this.failedAcquireActions != null)
            {
                writer.WritePropertyName("failedAcquireActions");
                writer.WriteArrayStart();
                foreach(var item in this.failedAcquireActions)
                {
                    item.WriteJson(writer);
                }
                writer.WriteArrayEnd();
            }
            if(this.premiseQuestNames != null)
            {
                writer.WritePropertyName("premiseQuestNames");
                writer.WriteArrayStart();
                foreach(var item in this.premiseQuestNames)
                {
                    writer.Write(item);
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

    public static string GetQuestNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):quest:(?<namespaceName>.*):group:(?<questGroupName>.*):quest:(?<questName>.*)");
        if (!match.Groups["questName"].Success)
        {
            return null;
        }
        return match.Groups["questName"].Value;
    }

    public static string GetQuestGroupNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):quest:(?<namespaceName>.*):group:(?<questGroupName>.*):quest:(?<questName>.*)");
        if (!match.Groups["questGroupName"].Success)
        {
            return null;
        }
        return match.Groups["questGroupName"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):quest:(?<namespaceName>.*):group:(?<questGroupName>.*):quest:(?<questName>.*)");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):quest:(?<namespaceName>.*):group:(?<questGroupName>.*):quest:(?<questName>.*)");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):quest:(?<namespaceName>.*):group:(?<questGroupName>.*):quest:(?<questName>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static QuestModel FromDict(JsonData data)
        {
            return new QuestModel()
                .WithQuestModelId(data.Keys.Contains("questModelId") && data["questModelId"] != null ? data["questModelId"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithMetadata(data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString() : null)
                .WithContents(data.Keys.Contains("contents") && data["contents"] != null ? data["contents"].Cast<JsonData>().Select(value =>
                    {
                        return Gs2.Gs2Quest.Model.Contents.FromDict(value);
                    }
                ).ToList() : null)
                .WithChallengePeriodEventId(data.Keys.Contains("challengePeriodEventId") && data["challengePeriodEventId"] != null ? data["challengePeriodEventId"].ToString() : null)
                .WithConsumeActions(data.Keys.Contains("consumeActions") && data["consumeActions"] != null ? data["consumeActions"].Cast<JsonData>().Select(value =>
                    {
                        return Gs2.Gs2Quest.Model.ConsumeAction.FromDict(value);
                    }
                ).ToList() : null)
                .WithFailedAcquireActions(data.Keys.Contains("failedAcquireActions") && data["failedAcquireActions"] != null ? data["failedAcquireActions"].Cast<JsonData>().Select(value =>
                    {
                        return Gs2.Gs2Quest.Model.AcquireAction.FromDict(value);
                    }
                ).ToList() : null)
                .WithPremiseQuestNames(data.Keys.Contains("premiseQuestNames") && data["premiseQuestNames"] != null ? data["premiseQuestNames"].Cast<JsonData>().Select(value =>
                    {
                        return value.ToString();
                    }
                ).ToList() : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as QuestModel;
            var diff = 0;
            if (questModelId == null && questModelId == other.questModelId)
            {
                // null and null
            }
            else
            {
                diff += questModelId.CompareTo(other.questModelId);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (metadata == null && metadata == other.metadata)
            {
                // null and null
            }
            else
            {
                diff += metadata.CompareTo(other.metadata);
            }
            if (contents == null && contents == other.contents)
            {
                // null and null
            }
            else
            {
                diff += contents.Count - other.contents.Count;
                for (var i = 0; i < contents.Count; i++)
                {
                    diff += contents[i].CompareTo(other.contents[i]);
                }
            }
            if (challengePeriodEventId == null && challengePeriodEventId == other.challengePeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += challengePeriodEventId.CompareTo(other.challengePeriodEventId);
            }
            if (consumeActions == null && consumeActions == other.consumeActions)
            {
                // null and null
            }
            else
            {
                diff += consumeActions.Count - other.consumeActions.Count;
                for (var i = 0; i < consumeActions.Count; i++)
                {
                    diff += consumeActions[i].CompareTo(other.consumeActions[i]);
                }
            }
            if (failedAcquireActions == null && failedAcquireActions == other.failedAcquireActions)
            {
                // null and null
            }
            else
            {
                diff += failedAcquireActions.Count - other.failedAcquireActions.Count;
                for (var i = 0; i < failedAcquireActions.Count; i++)
                {
                    diff += failedAcquireActions[i].CompareTo(other.failedAcquireActions[i]);
                }
            }
            if (premiseQuestNames == null && premiseQuestNames == other.premiseQuestNames)
            {
                // null and null
            }
            else
            {
                diff += premiseQuestNames.Count - other.premiseQuestNames.Count;
                for (var i = 0; i < premiseQuestNames.Count; i++)
                {
                    diff += premiseQuestNames[i].CompareTo(other.premiseQuestNames[i]);
                }
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["questModelId"] = questModelId;
            data["name"] = name;
            data["metadata"] = metadata;
            data["contents"] = new JsonData(contents.Select(item => item.ToDict()));
            data["challengePeriodEventId"] = challengePeriodEventId;
            data["consumeActions"] = new JsonData(consumeActions.Select(item => item.ToDict()));
            data["failedAcquireActions"] = new JsonData(failedAcquireActions.Select(item => item.ToDict()));
            data["premiseQuestNames"] = new JsonData(premiseQuestNames);
            return data;
        }
	}
}
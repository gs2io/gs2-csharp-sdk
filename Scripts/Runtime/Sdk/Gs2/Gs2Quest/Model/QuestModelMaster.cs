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
	public class QuestModelMaster : IComparable
	{

        /** クエストモデルマスター */
        public string questModelId { set; get; }

        /**
         * クエストモデルマスターを設定
         *
         * @param questModelId クエストモデルマスター
         * @return this
         */
        public QuestModelMaster WithQuestModelId(string questModelId) {
            this.questModelId = questModelId;
            return this;
        }

        /** クエストモデル名 */
        public string questGroupName { set; get; }

        /**
         * クエストモデル名を設定
         *
         * @param questGroupName クエストモデル名
         * @return this
         */
        public QuestModelMaster WithQuestGroupName(string questGroupName) {
            this.questGroupName = questGroupName;
            return this;
        }

        /** クエスト名 */
        public string name { set; get; }

        /**
         * クエスト名を設定
         *
         * @param name クエスト名
         * @return this
         */
        public QuestModelMaster WithName(string name) {
            this.name = name;
            return this;
        }

        /** クエストモデルの説明 */
        public string description { set; get; }

        /**
         * クエストモデルの説明を設定
         *
         * @param description クエストモデルの説明
         * @return this
         */
        public QuestModelMaster WithDescription(string description) {
            this.description = description;
            return this;
        }

        /** クエストのメタデータ */
        public string metadata { set; get; }

        /**
         * クエストのメタデータを設定
         *
         * @param metadata クエストのメタデータ
         * @return this
         */
        public QuestModelMaster WithMetadata(string metadata) {
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
        public QuestModelMaster WithContents(List<Contents> contents) {
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
        public QuestModelMaster WithChallengePeriodEventId(string challengePeriodEventId) {
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
        public QuestModelMaster WithConsumeActions(List<ConsumeAction> consumeActions) {
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
        public QuestModelMaster WithFailedAcquireActions(List<AcquireAction> failedAcquireActions) {
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
        public QuestModelMaster WithPremiseQuestNames(List<string> premiseQuestNames) {
            this.premiseQuestNames = premiseQuestNames;
            return this;
        }

        /** 作成日時 */
        public long? createdAt { set; get; }

        /**
         * 作成日時を設定
         *
         * @param createdAt 作成日時
         * @return this
         */
        public QuestModelMaster WithCreatedAt(long? createdAt) {
            this.createdAt = createdAt;
            return this;
        }

        /** 最終更新日時 */
        public long? updatedAt { set; get; }

        /**
         * 最終更新日時を設定
         *
         * @param updatedAt 最終更新日時
         * @return this
         */
        public QuestModelMaster WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
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
            if(this.questGroupName != null)
            {
                writer.WritePropertyName("questGroupName");
                writer.Write(this.questGroupName);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.description != null)
            {
                writer.WritePropertyName("description");
                writer.Write(this.description);
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
            if(this.createdAt.HasValue)
            {
                writer.WritePropertyName("createdAt");
                writer.Write(this.createdAt.Value);
            }
            if(this.updatedAt.HasValue)
            {
                writer.WritePropertyName("updatedAt");
                writer.Write(this.updatedAt.Value);
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
        public static QuestModelMaster FromDict(JsonData data)
        {
            return new QuestModelMaster()
                .WithQuestModelId(data.Keys.Contains("questModelId") && data["questModelId"] != null ? data["questModelId"].ToString() : null)
                .WithQuestGroupName(data.Keys.Contains("questGroupName") && data["questGroupName"] != null ? data["questGroupName"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithDescription(data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString() : null)
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
                ).ToList() : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as QuestModelMaster;
            var diff = 0;
            if (questModelId == null && questModelId == other.questModelId)
            {
                // null and null
            }
            else
            {
                diff += questModelId.CompareTo(other.questModelId);
            }
            if (questGroupName == null && questGroupName == other.questGroupName)
            {
                // null and null
            }
            else
            {
                diff += questGroupName.CompareTo(other.questGroupName);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (description == null && description == other.description)
            {
                // null and null
            }
            else
            {
                diff += description.CompareTo(other.description);
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
            if (createdAt == null && createdAt == other.createdAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(createdAt - other.createdAt);
            }
            if (updatedAt == null && updatedAt == other.updatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(updatedAt - other.updatedAt);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["questModelId"] = questModelId;
            data["questGroupName"] = questGroupName;
            data["name"] = name;
            data["description"] = description;
            data["metadata"] = metadata;
            data["contents"] = new JsonData(contents.Select(item => item.ToDict()));
            data["challengePeriodEventId"] = challengePeriodEventId;
            data["consumeActions"] = new JsonData(consumeActions.Select(item => item.ToDict()));
            data["failedAcquireActions"] = new JsonData(failedAcquireActions.Select(item => item.ToDict()));
            data["premiseQuestNames"] = new JsonData(premiseQuestNames);
            data["createdAt"] = createdAt;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}
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

namespace Gs2.Gs2Lottery.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string QueueNamespaceId { set; get; }
        public string KeyId { set; get; }
        public string LotteryTriggerScriptId { set; get; }
        public string ChoicePrizeTableScriptId { set; get; }
        public Gs2.Gs2Lottery.Model.LogSetting LogSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Namespace WithNamespaceId(string namespaceId) {
            this.NamespaceId = namespaceId;
            return this;
        }

        public Namespace WithName(string name) {
            this.Name = name;
            return this;
        }

        public Namespace WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public Namespace WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }

        public Namespace WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

        public Namespace WithLotteryTriggerScriptId(string lotteryTriggerScriptId) {
            this.LotteryTriggerScriptId = lotteryTriggerScriptId;
            return this;
        }

        public Namespace WithChoicePrizeTableScriptId(string choicePrizeTableScriptId) {
            this.ChoicePrizeTableScriptId = choicePrizeTableScriptId;
            return this;
        }

        public Namespace WithLogSetting(Gs2.Gs2Lottery.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

        public Namespace WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Namespace WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Namespace FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Namespace()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithLotteryTriggerScriptId(!data.Keys.Contains("lotteryTriggerScriptId") || data["lotteryTriggerScriptId"] == null ? null : data["lotteryTriggerScriptId"].ToString())
                .WithChoicePrizeTableScriptId(!data.Keys.Contains("choicePrizeTableScriptId") || data["choicePrizeTableScriptId"] == null ? null : data["choicePrizeTableScriptId"].ToString())
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Lottery.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["queueNamespaceId"] = QueueNamespaceId,
                ["keyId"] = KeyId,
                ["lotteryTriggerScriptId"] = LotteryTriggerScriptId,
                ["choicePrizeTableScriptId"] = ChoicePrizeTableScriptId,
                ["logSetting"] = LogSetting?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceId != null) {
                writer.WritePropertyName("namespaceId");
                writer.Write(NamespaceId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (LotteryTriggerScriptId != null) {
                writer.WritePropertyName("lotteryTriggerScriptId");
                writer.Write(LotteryTriggerScriptId.ToString());
            }
            if (ChoicePrizeTableScriptId != null) {
                writer.WritePropertyName("choicePrizeTableScriptId");
                writer.Write(ChoicePrizeTableScriptId.ToString());
            }
            if (LogSetting != null) {
                writer.WritePropertyName("logSetting");
                LogSetting.WriteJson(writer);
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
            var other = obj as Namespace;
            var diff = 0;
            if (NamespaceId == null && NamespaceId == other.NamespaceId)
            {
                // null and null
            }
            else
            {
                diff += NamespaceId.CompareTo(other.NamespaceId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (QueueNamespaceId == null && QueueNamespaceId == other.QueueNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += QueueNamespaceId.CompareTo(other.QueueNamespaceId);
            }
            if (KeyId == null && KeyId == other.KeyId)
            {
                // null and null
            }
            else
            {
                diff += KeyId.CompareTo(other.KeyId);
            }
            if (LotteryTriggerScriptId == null && LotteryTriggerScriptId == other.LotteryTriggerScriptId)
            {
                // null and null
            }
            else
            {
                diff += LotteryTriggerScriptId.CompareTo(other.LotteryTriggerScriptId);
            }
            if (ChoicePrizeTableScriptId == null && ChoicePrizeTableScriptId == other.ChoicePrizeTableScriptId)
            {
                // null and null
            }
            else
            {
                diff += ChoicePrizeTableScriptId.CompareTo(other.ChoicePrizeTableScriptId);
            }
            if (LogSetting == null && LogSetting == other.LogSetting)
            {
                // null and null
            }
            else
            {
                diff += LogSetting.CompareTo(other.LogSetting);
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
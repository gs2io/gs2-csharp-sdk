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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Lottery.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Lottery.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
        public string Name { set; get; }
        public string Description { set; get; }
        public string QueueNamespaceId { set; get; }
        public string KeyId { set; get; }
        public string LotteryTriggerScriptId { set; get; }
        public string ChoicePrizeTableScriptId { set; get; }
        public Gs2.Gs2Lottery.Model.LogSetting LogSetting { set; get; }

        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }

        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public CreateNamespaceRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }

        public CreateNamespaceRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

        public CreateNamespaceRequest WithLotteryTriggerScriptId(string lotteryTriggerScriptId) {
            this.LotteryTriggerScriptId = lotteryTriggerScriptId;
            return this;
        }

        public CreateNamespaceRequest WithChoicePrizeTableScriptId(string choicePrizeTableScriptId) {
            this.ChoicePrizeTableScriptId = choicePrizeTableScriptId;
            return this;
        }

        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Lottery.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateNamespaceRequest()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithLotteryTriggerScriptId(!data.Keys.Contains("lotteryTriggerScriptId") || data["lotteryTriggerScriptId"] == null ? null : data["lotteryTriggerScriptId"].ToString())
                .WithChoicePrizeTableScriptId(!data.Keys.Contains("choicePrizeTableScriptId") || data["choicePrizeTableScriptId"] == null ? null : data["choicePrizeTableScriptId"].ToString())
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Lottery.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["queueNamespaceId"] = QueueNamespaceId,
                ["keyId"] = KeyId,
                ["lotteryTriggerScriptId"] = LotteryTriggerScriptId,
                ["choicePrizeTableScriptId"] = ChoicePrizeTableScriptId,
                ["logSetting"] = LogSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}
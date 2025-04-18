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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Mission.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Mission.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public Gs2.Gs2Mission.Model.TransactionSetting TransactionSetting { set; get; } = null!;
         public Gs2.Gs2Mission.Model.ScriptSetting MissionCompleteScript { set; get; } = null!;
         public Gs2.Gs2Mission.Model.ScriptSetting CounterIncrementScript { set; get; } = null!;
         public Gs2.Gs2Mission.Model.ScriptSetting ReceiveRewardsScript { set; get; } = null!;
         public Gs2.Gs2Mission.Model.NotificationSetting CompleteNotification { set; get; } = null!;
         public Gs2.Gs2Mission.Model.LogSetting LogSetting { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public string QueueNamespaceId { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public string KeyId { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithTransactionSetting(Gs2.Gs2Mission.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public CreateNamespaceRequest WithMissionCompleteScript(Gs2.Gs2Mission.Model.ScriptSetting missionCompleteScript) {
            this.MissionCompleteScript = missionCompleteScript;
            return this;
        }
        public CreateNamespaceRequest WithCounterIncrementScript(Gs2.Gs2Mission.Model.ScriptSetting counterIncrementScript) {
            this.CounterIncrementScript = counterIncrementScript;
            return this;
        }
        public CreateNamespaceRequest WithReceiveRewardsScript(Gs2.Gs2Mission.Model.ScriptSetting receiveRewardsScript) {
            this.ReceiveRewardsScript = receiveRewardsScript;
            return this;
        }
        public CreateNamespaceRequest WithCompleteNotification(Gs2.Gs2Mission.Model.NotificationSetting completeNotification) {
            this.CompleteNotification = completeNotification;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Mission.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateNamespaceRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateNamespaceRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
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
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Mission.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithMissionCompleteScript(!data.Keys.Contains("missionCompleteScript") || data["missionCompleteScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["missionCompleteScript"]))
                .WithCounterIncrementScript(!data.Keys.Contains("counterIncrementScript") || data["counterIncrementScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["counterIncrementScript"]))
                .WithReceiveRewardsScript(!data.Keys.Contains("receiveRewardsScript") || data["receiveRewardsScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["receiveRewardsScript"]))
                .WithCompleteNotification(!data.Keys.Contains("completeNotification") || data["completeNotification"] == null ? null : Gs2.Gs2Mission.Model.NotificationSetting.FromJson(data["completeNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Mission.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["missionCompleteScript"] = MissionCompleteScript?.ToJson(),
                ["counterIncrementScript"] = CounterIncrementScript?.ToJson(),
                ["receiveRewardsScript"] = ReceiveRewardsScript?.ToJson(),
                ["completeNotification"] = CompleteNotification?.ToJson(),
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
            if (TransactionSetting != null) {
                TransactionSetting.WriteJson(writer);
            }
            if (MissionCompleteScript != null) {
                MissionCompleteScript.WriteJson(writer);
            }
            if (CounterIncrementScript != null) {
                CounterIncrementScript.WriteJson(writer);
            }
            if (ReceiveRewardsScript != null) {
                ReceiveRewardsScript.WriteJson(writer);
            }
            if (CompleteNotification != null) {
                CompleteNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Name + ":";
            key += Description + ":";
            key += TransactionSetting + ":";
            key += MissionCompleteScript + ":";
            key += CounterIncrementScript + ":";
            key += ReceiveRewardsScript + ":";
            key += CompleteNotification + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
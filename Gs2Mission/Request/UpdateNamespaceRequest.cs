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
using Gs2.Gs2Mission.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Mission.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
        public string NamespaceName { set; get; }
        public string Description { set; get; }
        public Gs2.Gs2Mission.Model.ScriptSetting MissionCompleteScript { set; get; }
        public Gs2.Gs2Mission.Model.ScriptSetting CounterIncrementScript { set; get; }
        public Gs2.Gs2Mission.Model.ScriptSetting ReceiveRewardsScript { set; get; }
        public string QueueNamespaceId { set; get; }
        public string KeyId { set; get; }
        public Gs2.Gs2Mission.Model.NotificationSetting CompleteNotification { set; get; }
        public Gs2.Gs2Mission.Model.LogSetting LogSetting { set; get; }

        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public UpdateNamespaceRequest WithMissionCompleteScript(Gs2.Gs2Mission.Model.ScriptSetting missionCompleteScript) {
            this.MissionCompleteScript = missionCompleteScript;
            return this;
        }

        public UpdateNamespaceRequest WithCounterIncrementScript(Gs2.Gs2Mission.Model.ScriptSetting counterIncrementScript) {
            this.CounterIncrementScript = counterIncrementScript;
            return this;
        }

        public UpdateNamespaceRequest WithReceiveRewardsScript(Gs2.Gs2Mission.Model.ScriptSetting receiveRewardsScript) {
            this.ReceiveRewardsScript = receiveRewardsScript;
            return this;
        }

        public UpdateNamespaceRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }

        public UpdateNamespaceRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

        public UpdateNamespaceRequest WithCompleteNotification(Gs2.Gs2Mission.Model.NotificationSetting completeNotification) {
            this.CompleteNotification = completeNotification;
            return this;
        }

        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Mission.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

    	[Preserve]
        public static UpdateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateNamespaceRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMissionCompleteScript(!data.Keys.Contains("missionCompleteScript") || data["missionCompleteScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["missionCompleteScript"]))
                .WithCounterIncrementScript(!data.Keys.Contains("counterIncrementScript") || data["counterIncrementScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["counterIncrementScript"]))
                .WithReceiveRewardsScript(!data.Keys.Contains("receiveRewardsScript") || data["receiveRewardsScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["receiveRewardsScript"]))
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithCompleteNotification(!data.Keys.Contains("completeNotification") || data["completeNotification"] == null ? null : Gs2.Gs2Mission.Model.NotificationSetting.FromJson(data["completeNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Mission.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["missionCompleteScript"] = MissionCompleteScript?.ToJson(),
                ["counterIncrementScript"] = CounterIncrementScript?.ToJson(),
                ["receiveRewardsScript"] = ReceiveRewardsScript?.ToJson(),
                ["queueNamespaceId"] = QueueNamespaceId,
                ["keyId"] = KeyId,
                ["completeNotification"] = CompleteNotification?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
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
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (CompleteNotification != null) {
                CompleteNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}
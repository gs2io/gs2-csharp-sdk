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
using Gs2.Gs2Quest.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Quest.Request
{
	[Preserve]
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
        public string Name { set; get; }
        public string Description { set; get; }
        public Gs2.Gs2Quest.Model.ScriptSetting StartQuestScript { set; get; }
        public Gs2.Gs2Quest.Model.ScriptSetting CompleteQuestScript { set; get; }
        public Gs2.Gs2Quest.Model.ScriptSetting FailedQuestScript { set; get; }
        public string QueueNamespaceId { set; get; }
        public string KeyId { set; get; }
        public Gs2.Gs2Quest.Model.LogSetting LogSetting { set; get; }

        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }

        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public CreateNamespaceRequest WithStartQuestScript(Gs2.Gs2Quest.Model.ScriptSetting startQuestScript) {
            this.StartQuestScript = startQuestScript;
            return this;
        }

        public CreateNamespaceRequest WithCompleteQuestScript(Gs2.Gs2Quest.Model.ScriptSetting completeQuestScript) {
            this.CompleteQuestScript = completeQuestScript;
            return this;
        }

        public CreateNamespaceRequest WithFailedQuestScript(Gs2.Gs2Quest.Model.ScriptSetting failedQuestScript) {
            this.FailedQuestScript = failedQuestScript;
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

        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Quest.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

    	[Preserve]
        public static CreateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateNamespaceRequest()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithStartQuestScript(!data.Keys.Contains("startQuestScript") || data["startQuestScript"] == null ? null : Gs2.Gs2Quest.Model.ScriptSetting.FromJson(data["startQuestScript"]))
                .WithCompleteQuestScript(!data.Keys.Contains("completeQuestScript") || data["completeQuestScript"] == null ? null : Gs2.Gs2Quest.Model.ScriptSetting.FromJson(data["completeQuestScript"]))
                .WithFailedQuestScript(!data.Keys.Contains("failedQuestScript") || data["failedQuestScript"] == null ? null : Gs2.Gs2Quest.Model.ScriptSetting.FromJson(data["failedQuestScript"]))
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Quest.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["startQuestScript"] = StartQuestScript?.ToJson(),
                ["completeQuestScript"] = CompleteQuestScript?.ToJson(),
                ["failedQuestScript"] = FailedQuestScript?.ToJson(),
                ["queueNamespaceId"] = QueueNamespaceId,
                ["keyId"] = KeyId,
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
            if (StartQuestScript != null) {
                StartQuestScript.WriteJson(writer);
            }
            if (CompleteQuestScript != null) {
                CompleteQuestScript.WriteJson(writer);
            }
            if (FailedQuestScript != null) {
                FailedQuestScript.WriteJson(writer);
            }
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}
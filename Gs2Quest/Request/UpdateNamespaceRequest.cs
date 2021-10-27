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

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Quest.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
        public string NamespaceName { set; get; }
        public string Description { set; get; }
        public Gs2.Gs2Quest.Model.ScriptSetting StartQuestScript { set; get; }
        public Gs2.Gs2Quest.Model.ScriptSetting CompleteQuestScript { set; get; }
        public Gs2.Gs2Quest.Model.ScriptSetting FailedQuestScript { set; get; }
        public string QueueNamespaceId { set; get; }
        public string KeyId { set; get; }
        public Gs2.Gs2Quest.Model.LogSetting LogSetting { set; get; }

        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public UpdateNamespaceRequest WithStartQuestScript(Gs2.Gs2Quest.Model.ScriptSetting startQuestScript) {
            this.StartQuestScript = startQuestScript;
            return this;
        }

        public UpdateNamespaceRequest WithCompleteQuestScript(Gs2.Gs2Quest.Model.ScriptSetting completeQuestScript) {
            this.CompleteQuestScript = completeQuestScript;
            return this;
        }

        public UpdateNamespaceRequest WithFailedQuestScript(Gs2.Gs2Quest.Model.ScriptSetting failedQuestScript) {
            this.FailedQuestScript = failedQuestScript;
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

        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Quest.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateNamespaceRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
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
                ["namespaceName"] = NamespaceName,
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
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
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
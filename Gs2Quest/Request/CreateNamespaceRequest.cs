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
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public Gs2.Gs2Quest.Model.TransactionSetting TransactionSetting { set; get; } = null!;
         public Gs2.Gs2Quest.Model.ScriptSetting StartQuestScript { set; get; } = null!;
         public Gs2.Gs2Quest.Model.ScriptSetting CompleteQuestScript { set; get; } = null!;
         public Gs2.Gs2Quest.Model.ScriptSetting FailedQuestScript { set; get; } = null!;
         public Gs2.Gs2Quest.Model.LogSetting LogSetting { set; get; } = null!;
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
        public CreateNamespaceRequest WithTransactionSetting(Gs2.Gs2Quest.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
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
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Quest.Model.LogSetting logSetting) {
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
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Quest.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithStartQuestScript(!data.Keys.Contains("startQuestScript") || data["startQuestScript"] == null ? null : Gs2.Gs2Quest.Model.ScriptSetting.FromJson(data["startQuestScript"]))
                .WithCompleteQuestScript(!data.Keys.Contains("completeQuestScript") || data["completeQuestScript"] == null ? null : Gs2.Gs2Quest.Model.ScriptSetting.FromJson(data["completeQuestScript"]))
                .WithFailedQuestScript(!data.Keys.Contains("failedQuestScript") || data["failedQuestScript"] == null ? null : Gs2.Gs2Quest.Model.ScriptSetting.FromJson(data["failedQuestScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Quest.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["startQuestScript"] = StartQuestScript?.ToJson(),
                ["completeQuestScript"] = CompleteQuestScript?.ToJson(),
                ["failedQuestScript"] = FailedQuestScript?.ToJson(),
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
            if (StartQuestScript != null) {
                StartQuestScript.WriteJson(writer);
            }
            if (CompleteQuestScript != null) {
                CompleteQuestScript.WriteJson(writer);
            }
            if (FailedQuestScript != null) {
                FailedQuestScript.WriteJson(writer);
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
            key += StartQuestScript + ":";
            key += CompleteQuestScript + ":";
            key += FailedQuestScript + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
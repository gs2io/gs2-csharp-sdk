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
using Gs2.Gs2Idle.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Idle.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public Gs2.Gs2Idle.Model.TransactionSetting TransactionSetting { set; get; } = null!;
         public Gs2.Gs2Idle.Model.ScriptSetting ReceiveScript { set; get; } = null!;
         public string OverrideAcquireActionsScriptId { set; get; } = null!;
         public Gs2.Gs2Idle.Model.LogSetting LogSetting { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithTransactionSetting(Gs2.Gs2Idle.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public CreateNamespaceRequest WithReceiveScript(Gs2.Gs2Idle.Model.ScriptSetting receiveScript) {
            this.ReceiveScript = receiveScript;
            return this;
        }
        public CreateNamespaceRequest WithOverrideAcquireActionsScriptId(string overrideAcquireActionsScriptId) {
            this.OverrideAcquireActionsScriptId = overrideAcquireActionsScriptId;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Idle.Model.LogSetting logSetting) {
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
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Idle.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithReceiveScript(!data.Keys.Contains("receiveScript") || data["receiveScript"] == null ? null : Gs2.Gs2Idle.Model.ScriptSetting.FromJson(data["receiveScript"]))
                .WithOverrideAcquireActionsScriptId(!data.Keys.Contains("overrideAcquireActionsScriptId") || data["overrideAcquireActionsScriptId"] == null ? null : data["overrideAcquireActionsScriptId"].ToString())
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Idle.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["receiveScript"] = ReceiveScript?.ToJson(),
                ["overrideAcquireActionsScriptId"] = OverrideAcquireActionsScriptId,
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
            if (ReceiveScript != null) {
                ReceiveScript.WriteJson(writer);
            }
            if (OverrideAcquireActionsScriptId != null) {
                writer.WritePropertyName("overrideAcquireActionsScriptId");
                writer.Write(OverrideAcquireActionsScriptId.ToString());
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Name + ":";
            key += Description + ":";
            key += TransactionSetting + ":";
            key += ReceiveScript + ":";
            key += OverrideAcquireActionsScriptId + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
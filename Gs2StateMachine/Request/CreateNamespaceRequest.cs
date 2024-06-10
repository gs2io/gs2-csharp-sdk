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
using Gs2.Gs2StateMachine.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2StateMachine.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string SupportSpeculativeExecution { set; get; } = null!;
         public Gs2.Gs2StateMachine.Model.TransactionSetting TransactionSetting { set; get; } = null!;
         public Gs2.Gs2StateMachine.Model.ScriptSetting StartScript { set; get; } = null!;
         public Gs2.Gs2StateMachine.Model.ScriptSetting PassScript { set; get; } = null!;
         public Gs2.Gs2StateMachine.Model.ScriptSetting ErrorScript { set; get; } = null!;
         public long? LowestStateMachineVersion { set; get; } = null!;
         public Gs2.Gs2StateMachine.Model.LogSetting LogSetting { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithSupportSpeculativeExecution(string supportSpeculativeExecution) {
            this.SupportSpeculativeExecution = supportSpeculativeExecution;
            return this;
        }
        public CreateNamespaceRequest WithTransactionSetting(Gs2.Gs2StateMachine.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public CreateNamespaceRequest WithStartScript(Gs2.Gs2StateMachine.Model.ScriptSetting startScript) {
            this.StartScript = startScript;
            return this;
        }
        public CreateNamespaceRequest WithPassScript(Gs2.Gs2StateMachine.Model.ScriptSetting passScript) {
            this.PassScript = passScript;
            return this;
        }
        public CreateNamespaceRequest WithErrorScript(Gs2.Gs2StateMachine.Model.ScriptSetting errorScript) {
            this.ErrorScript = errorScript;
            return this;
        }
        public CreateNamespaceRequest WithLowestStateMachineVersion(long? lowestStateMachineVersion) {
            this.LowestStateMachineVersion = lowestStateMachineVersion;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2StateMachine.Model.LogSetting logSetting) {
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
                .WithSupportSpeculativeExecution(!data.Keys.Contains("supportSpeculativeExecution") || data["supportSpeculativeExecution"] == null ? null : data["supportSpeculativeExecution"].ToString())
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2StateMachine.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithStartScript(!data.Keys.Contains("startScript") || data["startScript"] == null ? null : Gs2.Gs2StateMachine.Model.ScriptSetting.FromJson(data["startScript"]))
                .WithPassScript(!data.Keys.Contains("passScript") || data["passScript"] == null ? null : Gs2.Gs2StateMachine.Model.ScriptSetting.FromJson(data["passScript"]))
                .WithErrorScript(!data.Keys.Contains("errorScript") || data["errorScript"] == null ? null : Gs2.Gs2StateMachine.Model.ScriptSetting.FromJson(data["errorScript"]))
                .WithLowestStateMachineVersion(!data.Keys.Contains("lowestStateMachineVersion") || data["lowestStateMachineVersion"] == null ? null : (long?)(data["lowestStateMachineVersion"].ToString().Contains(".") ? (long)double.Parse(data["lowestStateMachineVersion"].ToString()) : long.Parse(data["lowestStateMachineVersion"].ToString())))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2StateMachine.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["supportSpeculativeExecution"] = SupportSpeculativeExecution,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["startScript"] = StartScript?.ToJson(),
                ["passScript"] = PassScript?.ToJson(),
                ["errorScript"] = ErrorScript?.ToJson(),
                ["lowestStateMachineVersion"] = LowestStateMachineVersion,
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
            if (SupportSpeculativeExecution != null) {
                writer.WritePropertyName("supportSpeculativeExecution");
                writer.Write(SupportSpeculativeExecution.ToString());
            }
            if (TransactionSetting != null) {
                TransactionSetting.WriteJson(writer);
            }
            if (StartScript != null) {
                StartScript.WriteJson(writer);
            }
            if (PassScript != null) {
                PassScript.WriteJson(writer);
            }
            if (ErrorScript != null) {
                ErrorScript.WriteJson(writer);
            }
            if (LowestStateMachineVersion != null) {
                writer.WritePropertyName("lowestStateMachineVersion");
                writer.Write((LowestStateMachineVersion.ToString().Contains(".") ? (long)double.Parse(LowestStateMachineVersion.ToString()) : long.Parse(LowestStateMachineVersion.ToString())));
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
            key += SupportSpeculativeExecution + ":";
            key += TransactionSetting + ":";
            key += StartScript + ":";
            key += PassScript + ":";
            key += ErrorScript + ":";
            key += LowestStateMachineVersion + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
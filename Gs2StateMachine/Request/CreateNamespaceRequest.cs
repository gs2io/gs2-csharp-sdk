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
        public string Name { set; get; }
        public string Description { set; get; }
        public Gs2.Gs2StateMachine.Model.ScriptSetting StartScript { set; get; }
        public Gs2.Gs2StateMachine.Model.ScriptSetting PassScript { set; get; }
        public Gs2.Gs2StateMachine.Model.ScriptSetting ErrorScript { set; get; }
        public long? LowestStateMachineVersion { set; get; }
        public Gs2.Gs2StateMachine.Model.LogSetting LogSetting { set; get; }

        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }

        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
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
            key += StartScript + ":";
            key += PassScript + ":";
            key += ErrorScript + ":";
            key += LowestStateMachineVersion + ":";
            key += LogSetting + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply CreateNamespaceRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (CreateNamespaceRequest)x;
            return this;
        }
    }
}
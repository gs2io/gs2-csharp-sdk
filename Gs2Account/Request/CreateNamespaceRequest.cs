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
using Gs2.Gs2Account.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Account.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public bool? ChangePasswordIfTakeOver { set; get; } = null!;
         public bool? DifferentUserIdForLoginAndDataRetention { set; get; } = null!;
         public Gs2.Gs2Account.Model.ScriptSetting CreateAccountScript { set; get; } = null!;
         public Gs2.Gs2Account.Model.ScriptSetting AuthenticationScript { set; get; } = null!;
         public Gs2.Gs2Account.Model.ScriptSetting CreateTakeOverScript { set; get; } = null!;
         public Gs2.Gs2Account.Model.ScriptSetting DoTakeOverScript { set; get; } = null!;
         public Gs2.Gs2Account.Model.LogSetting LogSetting { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithChangePasswordIfTakeOver(bool? changePasswordIfTakeOver) {
            this.ChangePasswordIfTakeOver = changePasswordIfTakeOver;
            return this;
        }
        public CreateNamespaceRequest WithDifferentUserIdForLoginAndDataRetention(bool? differentUserIdForLoginAndDataRetention) {
            this.DifferentUserIdForLoginAndDataRetention = differentUserIdForLoginAndDataRetention;
            return this;
        }
        public CreateNamespaceRequest WithCreateAccountScript(Gs2.Gs2Account.Model.ScriptSetting createAccountScript) {
            this.CreateAccountScript = createAccountScript;
            return this;
        }
        public CreateNamespaceRequest WithAuthenticationScript(Gs2.Gs2Account.Model.ScriptSetting authenticationScript) {
            this.AuthenticationScript = authenticationScript;
            return this;
        }
        public CreateNamespaceRequest WithCreateTakeOverScript(Gs2.Gs2Account.Model.ScriptSetting createTakeOverScript) {
            this.CreateTakeOverScript = createTakeOverScript;
            return this;
        }
        public CreateNamespaceRequest WithDoTakeOverScript(Gs2.Gs2Account.Model.ScriptSetting doTakeOverScript) {
            this.DoTakeOverScript = doTakeOverScript;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Account.Model.LogSetting logSetting) {
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
                .WithChangePasswordIfTakeOver(!data.Keys.Contains("changePasswordIfTakeOver") || data["changePasswordIfTakeOver"] == null ? null : (bool?)bool.Parse(data["changePasswordIfTakeOver"].ToString()))
                .WithDifferentUserIdForLoginAndDataRetention(!data.Keys.Contains("differentUserIdForLoginAndDataRetention") || data["differentUserIdForLoginAndDataRetention"] == null ? null : (bool?)bool.Parse(data["differentUserIdForLoginAndDataRetention"].ToString()))
                .WithCreateAccountScript(!data.Keys.Contains("createAccountScript") || data["createAccountScript"] == null ? null : Gs2.Gs2Account.Model.ScriptSetting.FromJson(data["createAccountScript"]))
                .WithAuthenticationScript(!data.Keys.Contains("authenticationScript") || data["authenticationScript"] == null ? null : Gs2.Gs2Account.Model.ScriptSetting.FromJson(data["authenticationScript"]))
                .WithCreateTakeOverScript(!data.Keys.Contains("createTakeOverScript") || data["createTakeOverScript"] == null ? null : Gs2.Gs2Account.Model.ScriptSetting.FromJson(data["createTakeOverScript"]))
                .WithDoTakeOverScript(!data.Keys.Contains("doTakeOverScript") || data["doTakeOverScript"] == null ? null : Gs2.Gs2Account.Model.ScriptSetting.FromJson(data["doTakeOverScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Account.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["changePasswordIfTakeOver"] = ChangePasswordIfTakeOver,
                ["differentUserIdForLoginAndDataRetention"] = DifferentUserIdForLoginAndDataRetention,
                ["createAccountScript"] = CreateAccountScript?.ToJson(),
                ["authenticationScript"] = AuthenticationScript?.ToJson(),
                ["createTakeOverScript"] = CreateTakeOverScript?.ToJson(),
                ["doTakeOverScript"] = DoTakeOverScript?.ToJson(),
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
            if (ChangePasswordIfTakeOver != null) {
                writer.WritePropertyName("changePasswordIfTakeOver");
                writer.Write(bool.Parse(ChangePasswordIfTakeOver.ToString()));
            }
            if (DifferentUserIdForLoginAndDataRetention != null) {
                writer.WritePropertyName("differentUserIdForLoginAndDataRetention");
                writer.Write(bool.Parse(DifferentUserIdForLoginAndDataRetention.ToString()));
            }
            if (CreateAccountScript != null) {
                CreateAccountScript.WriteJson(writer);
            }
            if (AuthenticationScript != null) {
                AuthenticationScript.WriteJson(writer);
            }
            if (CreateTakeOverScript != null) {
                CreateTakeOverScript.WriteJson(writer);
            }
            if (DoTakeOverScript != null) {
                DoTakeOverScript.WriteJson(writer);
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
            key += ChangePasswordIfTakeOver + ":";
            key += DifferentUserIdForLoginAndDataRetention + ":";
            key += CreateAccountScript + ":";
            key += AuthenticationScript + ":";
            key += CreateTakeOverScript + ":";
            key += DoTakeOverScript + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
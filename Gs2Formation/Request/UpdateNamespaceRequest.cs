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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Formation.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public Gs2.Gs2Formation.Model.TransactionSetting TransactionSetting { set; get; } = null!;
         public Gs2.Gs2Formation.Model.ScriptSetting UpdateMoldScript { set; get; } = null!;
         public Gs2.Gs2Formation.Model.ScriptSetting UpdateFormScript { set; get; } = null!;
         public Gs2.Gs2Formation.Model.ScriptSetting UpdatePropertyFormScript { set; get; } = null!;
         public Gs2.Gs2Formation.Model.LogSetting LogSetting { set; get; } = null!;
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithTransactionSetting(Gs2.Gs2Formation.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public UpdateNamespaceRequest WithUpdateMoldScript(Gs2.Gs2Formation.Model.ScriptSetting updateMoldScript) {
            this.UpdateMoldScript = updateMoldScript;
            return this;
        }
        public UpdateNamespaceRequest WithUpdateFormScript(Gs2.Gs2Formation.Model.ScriptSetting updateFormScript) {
            this.UpdateFormScript = updateFormScript;
            return this;
        }
        public UpdateNamespaceRequest WithUpdatePropertyFormScript(Gs2.Gs2Formation.Model.ScriptSetting updatePropertyFormScript) {
            this.UpdatePropertyFormScript = updatePropertyFormScript;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Formation.Model.LogSetting logSetting) {
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
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Formation.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithUpdateMoldScript(!data.Keys.Contains("updateMoldScript") || data["updateMoldScript"] == null ? null : Gs2.Gs2Formation.Model.ScriptSetting.FromJson(data["updateMoldScript"]))
                .WithUpdateFormScript(!data.Keys.Contains("updateFormScript") || data["updateFormScript"] == null ? null : Gs2.Gs2Formation.Model.ScriptSetting.FromJson(data["updateFormScript"]))
                .WithUpdatePropertyFormScript(!data.Keys.Contains("updatePropertyFormScript") || data["updatePropertyFormScript"] == null ? null : Gs2.Gs2Formation.Model.ScriptSetting.FromJson(data["updatePropertyFormScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Formation.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["updateMoldScript"] = UpdateMoldScript?.ToJson(),
                ["updateFormScript"] = UpdateFormScript?.ToJson(),
                ["updatePropertyFormScript"] = UpdatePropertyFormScript?.ToJson(),
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
            if (TransactionSetting != null) {
                TransactionSetting.WriteJson(writer);
            }
            if (UpdateMoldScript != null) {
                UpdateMoldScript.WriteJson(writer);
            }
            if (UpdateFormScript != null) {
                UpdateFormScript.WriteJson(writer);
            }
            if (UpdatePropertyFormScript != null) {
                UpdatePropertyFormScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Description + ":";
            key += TransactionSetting + ":";
            key += UpdateMoldScript + ":";
            key += UpdateFormScript + ":";
            key += UpdatePropertyFormScript + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
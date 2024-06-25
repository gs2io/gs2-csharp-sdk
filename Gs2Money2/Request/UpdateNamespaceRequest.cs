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
using Gs2.Gs2Money2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string CurrencyUsagePriority { set; get; } = null!;
         public string Description { set; get; } = null!;
         public Gs2.Gs2Money2.Model.PlatformSetting PlatformSetting { set; get; } = null!;
         public Gs2.Gs2Money2.Model.ScriptSetting ChangeBalanceScript { set; get; } = null!;
         public Gs2.Gs2Money2.Model.LogSetting LogSetting { set; get; } = null!;
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithCurrencyUsagePriority(string currencyUsagePriority) {
            this.CurrencyUsagePriority = currencyUsagePriority;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithPlatformSetting(Gs2.Gs2Money2.Model.PlatformSetting platformSetting) {
            this.PlatformSetting = platformSetting;
            return this;
        }
        public UpdateNamespaceRequest WithChangeBalanceScript(Gs2.Gs2Money2.Model.ScriptSetting changeBalanceScript) {
            this.ChangeBalanceScript = changeBalanceScript;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Money2.Model.LogSetting logSetting) {
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
                .WithCurrencyUsagePriority(!data.Keys.Contains("currencyUsagePriority") || data["currencyUsagePriority"] == null ? null : data["currencyUsagePriority"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithPlatformSetting(!data.Keys.Contains("platformSetting") || data["platformSetting"] == null ? null : Gs2.Gs2Money2.Model.PlatformSetting.FromJson(data["platformSetting"]))
                .WithChangeBalanceScript(!data.Keys.Contains("changeBalanceScript") || data["changeBalanceScript"] == null ? null : Gs2.Gs2Money2.Model.ScriptSetting.FromJson(data["changeBalanceScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Money2.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["currencyUsagePriority"] = CurrencyUsagePriority,
                ["description"] = Description,
                ["platformSetting"] = PlatformSetting?.ToJson(),
                ["changeBalanceScript"] = ChangeBalanceScript?.ToJson(),
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
            if (CurrencyUsagePriority != null) {
                writer.WritePropertyName("currencyUsagePriority");
                writer.Write(CurrencyUsagePriority.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (PlatformSetting != null) {
                PlatformSetting.WriteJson(writer);
            }
            if (ChangeBalanceScript != null) {
                ChangeBalanceScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CurrencyUsagePriority + ":";
            key += Description + ":";
            key += PlatformSetting + ":";
            key += ChangeBalanceScript + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
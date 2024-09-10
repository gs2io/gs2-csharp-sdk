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
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string CurrencyUsagePriority { set; get; } = null!;
         public string Description { set; get; } = null!;
         public bool? SharedFreeCurrency { set; get; } = null!;
         public Gs2.Gs2Money2.Model.PlatformSetting PlatformSetting { set; get; } = null!;
         public Gs2.Gs2Money2.Model.ScriptSetting DepositBalanceScript { set; get; } = null!;
         public Gs2.Gs2Money2.Model.ScriptSetting WithdrawBalanceScript { set; get; } = null!;
         public Gs2.Gs2Money2.Model.LogSetting LogSetting { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithCurrencyUsagePriority(string currencyUsagePriority) {
            this.CurrencyUsagePriority = currencyUsagePriority;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithSharedFreeCurrency(bool? sharedFreeCurrency) {
            this.SharedFreeCurrency = sharedFreeCurrency;
            return this;
        }
        public CreateNamespaceRequest WithPlatformSetting(Gs2.Gs2Money2.Model.PlatformSetting platformSetting) {
            this.PlatformSetting = platformSetting;
            return this;
        }
        public CreateNamespaceRequest WithDepositBalanceScript(Gs2.Gs2Money2.Model.ScriptSetting depositBalanceScript) {
            this.DepositBalanceScript = depositBalanceScript;
            return this;
        }
        public CreateNamespaceRequest WithWithdrawBalanceScript(Gs2.Gs2Money2.Model.ScriptSetting withdrawBalanceScript) {
            this.WithdrawBalanceScript = withdrawBalanceScript;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Money2.Model.LogSetting logSetting) {
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
                .WithCurrencyUsagePriority(!data.Keys.Contains("currencyUsagePriority") || data["currencyUsagePriority"] == null ? null : data["currencyUsagePriority"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithSharedFreeCurrency(!data.Keys.Contains("sharedFreeCurrency") || data["sharedFreeCurrency"] == null ? null : (bool?)bool.Parse(data["sharedFreeCurrency"].ToString()))
                .WithPlatformSetting(!data.Keys.Contains("platformSetting") || data["platformSetting"] == null ? null : Gs2.Gs2Money2.Model.PlatformSetting.FromJson(data["platformSetting"]))
                .WithDepositBalanceScript(!data.Keys.Contains("depositBalanceScript") || data["depositBalanceScript"] == null ? null : Gs2.Gs2Money2.Model.ScriptSetting.FromJson(data["depositBalanceScript"]))
                .WithWithdrawBalanceScript(!data.Keys.Contains("withdrawBalanceScript") || data["withdrawBalanceScript"] == null ? null : Gs2.Gs2Money2.Model.ScriptSetting.FromJson(data["withdrawBalanceScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Money2.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["currencyUsagePriority"] = CurrencyUsagePriority,
                ["description"] = Description,
                ["sharedFreeCurrency"] = SharedFreeCurrency,
                ["platformSetting"] = PlatformSetting?.ToJson(),
                ["depositBalanceScript"] = DepositBalanceScript?.ToJson(),
                ["withdrawBalanceScript"] = WithdrawBalanceScript?.ToJson(),
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
            if (CurrencyUsagePriority != null) {
                writer.WritePropertyName("currencyUsagePriority");
                writer.Write(CurrencyUsagePriority.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (SharedFreeCurrency != null) {
                writer.WritePropertyName("sharedFreeCurrency");
                writer.Write(bool.Parse(SharedFreeCurrency.ToString()));
            }
            if (PlatformSetting != null) {
                PlatformSetting.WriteJson(writer);
            }
            if (DepositBalanceScript != null) {
                DepositBalanceScript.WriteJson(writer);
            }
            if (WithdrawBalanceScript != null) {
                WithdrawBalanceScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Name + ":";
            key += CurrencyUsagePriority + ":";
            key += Description + ":";
            key += SharedFreeCurrency + ":";
            key += PlatformSetting + ":";
            key += DepositBalanceScript + ":";
            key += WithdrawBalanceScript + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
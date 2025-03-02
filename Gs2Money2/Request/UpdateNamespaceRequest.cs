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
         public Gs2.Gs2Money2.Model.ScriptSetting DepositBalanceScript { set; get; } = null!;
         public Gs2.Gs2Money2.Model.ScriptSetting WithdrawBalanceScript { set; get; } = null!;
         public string SubscribeScript { set; get; } = null!;
         public string RenewScript { set; get; } = null!;
         public string UnsubscribeScript { set; get; } = null!;
         public Gs2.Gs2Money2.Model.ScriptSetting TakeOverScript { set; get; } = null!;
         public Gs2.Gs2Money2.Model.NotificationSetting ChangeSubscriptionStatusNotification { set; get; } = null!;
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
        public UpdateNamespaceRequest WithDepositBalanceScript(Gs2.Gs2Money2.Model.ScriptSetting depositBalanceScript) {
            this.DepositBalanceScript = depositBalanceScript;
            return this;
        }
        public UpdateNamespaceRequest WithWithdrawBalanceScript(Gs2.Gs2Money2.Model.ScriptSetting withdrawBalanceScript) {
            this.WithdrawBalanceScript = withdrawBalanceScript;
            return this;
        }
        public UpdateNamespaceRequest WithSubscribeScript(string subscribeScript) {
            this.SubscribeScript = subscribeScript;
            return this;
        }
        public UpdateNamespaceRequest WithRenewScript(string renewScript) {
            this.RenewScript = renewScript;
            return this;
        }
        public UpdateNamespaceRequest WithUnsubscribeScript(string unsubscribeScript) {
            this.UnsubscribeScript = unsubscribeScript;
            return this;
        }
        public UpdateNamespaceRequest WithTakeOverScript(Gs2.Gs2Money2.Model.ScriptSetting takeOverScript) {
            this.TakeOverScript = takeOverScript;
            return this;
        }
        public UpdateNamespaceRequest WithChangeSubscriptionStatusNotification(Gs2.Gs2Money2.Model.NotificationSetting changeSubscriptionStatusNotification) {
            this.ChangeSubscriptionStatusNotification = changeSubscriptionStatusNotification;
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
                .WithDepositBalanceScript(!data.Keys.Contains("depositBalanceScript") || data["depositBalanceScript"] == null ? null : Gs2.Gs2Money2.Model.ScriptSetting.FromJson(data["depositBalanceScript"]))
                .WithWithdrawBalanceScript(!data.Keys.Contains("withdrawBalanceScript") || data["withdrawBalanceScript"] == null ? null : Gs2.Gs2Money2.Model.ScriptSetting.FromJson(data["withdrawBalanceScript"]))
                .WithSubscribeScript(!data.Keys.Contains("subscribeScript") || data["subscribeScript"] == null ? null : data["subscribeScript"].ToString())
                .WithRenewScript(!data.Keys.Contains("renewScript") || data["renewScript"] == null ? null : data["renewScript"].ToString())
                .WithUnsubscribeScript(!data.Keys.Contains("unsubscribeScript") || data["unsubscribeScript"] == null ? null : data["unsubscribeScript"].ToString())
                .WithTakeOverScript(!data.Keys.Contains("takeOverScript") || data["takeOverScript"] == null ? null : Gs2.Gs2Money2.Model.ScriptSetting.FromJson(data["takeOverScript"]))
                .WithChangeSubscriptionStatusNotification(!data.Keys.Contains("changeSubscriptionStatusNotification") || data["changeSubscriptionStatusNotification"] == null ? null : Gs2.Gs2Money2.Model.NotificationSetting.FromJson(data["changeSubscriptionStatusNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Money2.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["currencyUsagePriority"] = CurrencyUsagePriority,
                ["description"] = Description,
                ["platformSetting"] = PlatformSetting?.ToJson(),
                ["depositBalanceScript"] = DepositBalanceScript?.ToJson(),
                ["withdrawBalanceScript"] = WithdrawBalanceScript?.ToJson(),
                ["subscribeScript"] = SubscribeScript,
                ["renewScript"] = RenewScript,
                ["unsubscribeScript"] = UnsubscribeScript,
                ["takeOverScript"] = TakeOverScript?.ToJson(),
                ["changeSubscriptionStatusNotification"] = ChangeSubscriptionStatusNotification?.ToJson(),
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
            if (DepositBalanceScript != null) {
                DepositBalanceScript.WriteJson(writer);
            }
            if (WithdrawBalanceScript != null) {
                WithdrawBalanceScript.WriteJson(writer);
            }
            if (SubscribeScript != null) {
                writer.WritePropertyName("subscribeScript");
                writer.Write(SubscribeScript.ToString());
            }
            if (RenewScript != null) {
                writer.WritePropertyName("renewScript");
                writer.Write(RenewScript.ToString());
            }
            if (UnsubscribeScript != null) {
                writer.WritePropertyName("unsubscribeScript");
                writer.Write(UnsubscribeScript.ToString());
            }
            if (TakeOverScript != null) {
                TakeOverScript.WriteJson(writer);
            }
            if (ChangeSubscriptionStatusNotification != null) {
                ChangeSubscriptionStatusNotification.WriteJson(writer);
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
            key += DepositBalanceScript + ":";
            key += WithdrawBalanceScript + ":";
            key += SubscribeScript + ":";
            key += RenewScript + ":";
            key += UnsubscribeScript + ":";
            key += TakeOverScript + ":";
            key += ChangeSubscriptionStatusNotification + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
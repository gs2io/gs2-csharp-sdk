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
using Gs2.Gs2Money.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Priority { set; get; } = null!;
         public bool? ShareFree { set; get; } = null!;
         public string Currency { set; get; } = null!;
         public string AppleKey { set; get; } = null!;
         public string GoogleKey { set; get; } = null!;
         public bool? EnableFakeReceipt { set; get; } = null!;
         public Gs2.Gs2Money.Model.ScriptSetting CreateWalletScript { set; get; } = null!;
         public Gs2.Gs2Money.Model.ScriptSetting DepositScript { set; get; } = null!;
         public Gs2.Gs2Money.Model.ScriptSetting WithdrawScript { set; get; } = null!;
         public Gs2.Gs2Money.Model.LogSetting LogSetting { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithPriority(string priority) {
            this.Priority = priority;
            return this;
        }
        public CreateNamespaceRequest WithShareFree(bool? shareFree) {
            this.ShareFree = shareFree;
            return this;
        }
        public CreateNamespaceRequest WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }
        public CreateNamespaceRequest WithAppleKey(string appleKey) {
            this.AppleKey = appleKey;
            return this;
        }
        public CreateNamespaceRequest WithGoogleKey(string googleKey) {
            this.GoogleKey = googleKey;
            return this;
        }
        public CreateNamespaceRequest WithEnableFakeReceipt(bool? enableFakeReceipt) {
            this.EnableFakeReceipt = enableFakeReceipt;
            return this;
        }
        public CreateNamespaceRequest WithCreateWalletScript(Gs2.Gs2Money.Model.ScriptSetting createWalletScript) {
            this.CreateWalletScript = createWalletScript;
            return this;
        }
        public CreateNamespaceRequest WithDepositScript(Gs2.Gs2Money.Model.ScriptSetting depositScript) {
            this.DepositScript = depositScript;
            return this;
        }
        public CreateNamespaceRequest WithWithdrawScript(Gs2.Gs2Money.Model.ScriptSetting withdrawScript) {
            this.WithdrawScript = withdrawScript;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Money.Model.LogSetting logSetting) {
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
                .WithPriority(!data.Keys.Contains("priority") || data["priority"] == null ? null : data["priority"].ToString())
                .WithShareFree(!data.Keys.Contains("shareFree") || data["shareFree"] == null ? null : (bool?)bool.Parse(data["shareFree"].ToString()))
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithAppleKey(!data.Keys.Contains("appleKey") || data["appleKey"] == null ? null : data["appleKey"].ToString())
                .WithGoogleKey(!data.Keys.Contains("googleKey") || data["googleKey"] == null ? null : data["googleKey"].ToString())
                .WithEnableFakeReceipt(!data.Keys.Contains("enableFakeReceipt") || data["enableFakeReceipt"] == null ? null : (bool?)bool.Parse(data["enableFakeReceipt"].ToString()))
                .WithCreateWalletScript(!data.Keys.Contains("createWalletScript") || data["createWalletScript"] == null ? null : Gs2.Gs2Money.Model.ScriptSetting.FromJson(data["createWalletScript"]))
                .WithDepositScript(!data.Keys.Contains("depositScript") || data["depositScript"] == null ? null : Gs2.Gs2Money.Model.ScriptSetting.FromJson(data["depositScript"]))
                .WithWithdrawScript(!data.Keys.Contains("withdrawScript") || data["withdrawScript"] == null ? null : Gs2.Gs2Money.Model.ScriptSetting.FromJson(data["withdrawScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Money.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["priority"] = Priority,
                ["shareFree"] = ShareFree,
                ["currency"] = Currency,
                ["appleKey"] = AppleKey,
                ["googleKey"] = GoogleKey,
                ["enableFakeReceipt"] = EnableFakeReceipt,
                ["createWalletScript"] = CreateWalletScript?.ToJson(),
                ["depositScript"] = DepositScript?.ToJson(),
                ["withdrawScript"] = WithdrawScript?.ToJson(),
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
            if (Priority != null) {
                writer.WritePropertyName("priority");
                writer.Write(Priority.ToString());
            }
            if (ShareFree != null) {
                writer.WritePropertyName("shareFree");
                writer.Write(bool.Parse(ShareFree.ToString()));
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
            }
            if (AppleKey != null) {
                writer.WritePropertyName("appleKey");
                writer.Write(AppleKey.ToString());
            }
            if (GoogleKey != null) {
                writer.WritePropertyName("googleKey");
                writer.Write(GoogleKey.ToString());
            }
            if (EnableFakeReceipt != null) {
                writer.WritePropertyName("enableFakeReceipt");
                writer.Write(bool.Parse(EnableFakeReceipt.ToString()));
            }
            if (CreateWalletScript != null) {
                CreateWalletScript.WriteJson(writer);
            }
            if (DepositScript != null) {
                DepositScript.WriteJson(writer);
            }
            if (WithdrawScript != null) {
                WithdrawScript.WriteJson(writer);
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
            key += Priority + ":";
            key += ShareFree + ":";
            key += Currency + ":";
            key += AppleKey + ":";
            key += GoogleKey + ":";
            key += EnableFakeReceipt + ":";
            key += CreateWalletScript + ":";
            key += DepositScript + ":";
            key += WithdrawScript + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
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
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Priority { set; get; } = null!;
         public string AppleKey { set; get; } = null!;
         public string GoogleKey { set; get; } = null!;
         public bool? EnableFakeReceipt { set; get; } = null!;
         public Gs2.Gs2Money.Model.ScriptSetting CreateWalletScript { set; get; } = null!;
         public Gs2.Gs2Money.Model.ScriptSetting DepositScript { set; get; } = null!;
         public Gs2.Gs2Money.Model.ScriptSetting WithdrawScript { set; get; } = null!;
         public Gs2.Gs2Money.Model.LogSetting LogSetting { set; get; } = null!;
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithPriority(string priority) {
            this.Priority = priority;
            return this;
        }
        public UpdateNamespaceRequest WithAppleKey(string appleKey) {
            this.AppleKey = appleKey;
            return this;
        }
        public UpdateNamespaceRequest WithGoogleKey(string googleKey) {
            this.GoogleKey = googleKey;
            return this;
        }
        public UpdateNamespaceRequest WithEnableFakeReceipt(bool? enableFakeReceipt) {
            this.EnableFakeReceipt = enableFakeReceipt;
            return this;
        }
        public UpdateNamespaceRequest WithCreateWalletScript(Gs2.Gs2Money.Model.ScriptSetting createWalletScript) {
            this.CreateWalletScript = createWalletScript;
            return this;
        }
        public UpdateNamespaceRequest WithDepositScript(Gs2.Gs2Money.Model.ScriptSetting depositScript) {
            this.DepositScript = depositScript;
            return this;
        }
        public UpdateNamespaceRequest WithWithdrawScript(Gs2.Gs2Money.Model.ScriptSetting withdrawScript) {
            this.WithdrawScript = withdrawScript;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Money.Model.LogSetting logSetting) {
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
                .WithPriority(!data.Keys.Contains("priority") || data["priority"] == null ? null : data["priority"].ToString())
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
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["priority"] = Priority,
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
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Priority != null) {
                writer.WritePropertyName("priority");
                writer.Write(Priority.ToString());
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
            key += NamespaceName + ":";
            key += Description + ":";
            key += Priority + ":";
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
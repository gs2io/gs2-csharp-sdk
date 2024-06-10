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
using Gs2.Gs2Exchange.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Exchange.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public bool? EnableAwaitExchange { set; get; } = null!;
         public bool? EnableDirectExchange { set; get; } = null!;
         public Gs2.Gs2Exchange.Model.TransactionSetting TransactionSetting { set; get; } = null!;
         public Gs2.Gs2Exchange.Model.ScriptSetting ExchangeScript { set; get; } = null!;
         public Gs2.Gs2Exchange.Model.ScriptSetting IncrementalExchangeScript { set; get; } = null!;
         public Gs2.Gs2Exchange.Model.LogSetting LogSetting { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public string QueueNamespaceId { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public string KeyId { set; get; } = null!;
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithEnableAwaitExchange(bool? enableAwaitExchange) {
            this.EnableAwaitExchange = enableAwaitExchange;
            return this;
        }
        public UpdateNamespaceRequest WithEnableDirectExchange(bool? enableDirectExchange) {
            this.EnableDirectExchange = enableDirectExchange;
            return this;
        }
        public UpdateNamespaceRequest WithTransactionSetting(Gs2.Gs2Exchange.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public UpdateNamespaceRequest WithExchangeScript(Gs2.Gs2Exchange.Model.ScriptSetting exchangeScript) {
            this.ExchangeScript = exchangeScript;
            return this;
        }
        public UpdateNamespaceRequest WithIncrementalExchangeScript(Gs2.Gs2Exchange.Model.ScriptSetting incrementalExchangeScript) {
            this.IncrementalExchangeScript = incrementalExchangeScript;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Exchange.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public UpdateNamespaceRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public UpdateNamespaceRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
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
                .WithEnableAwaitExchange(!data.Keys.Contains("enableAwaitExchange") || data["enableAwaitExchange"] == null ? null : (bool?)bool.Parse(data["enableAwaitExchange"].ToString()))
                .WithEnableDirectExchange(!data.Keys.Contains("enableDirectExchange") || data["enableDirectExchange"] == null ? null : (bool?)bool.Parse(data["enableDirectExchange"].ToString()))
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Exchange.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithExchangeScript(!data.Keys.Contains("exchangeScript") || data["exchangeScript"] == null ? null : Gs2.Gs2Exchange.Model.ScriptSetting.FromJson(data["exchangeScript"]))
                .WithIncrementalExchangeScript(!data.Keys.Contains("incrementalExchangeScript") || data["incrementalExchangeScript"] == null ? null : Gs2.Gs2Exchange.Model.ScriptSetting.FromJson(data["incrementalExchangeScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Exchange.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["enableAwaitExchange"] = EnableAwaitExchange,
                ["enableDirectExchange"] = EnableDirectExchange,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["exchangeScript"] = ExchangeScript?.ToJson(),
                ["incrementalExchangeScript"] = IncrementalExchangeScript?.ToJson(),
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
            if (EnableAwaitExchange != null) {
                writer.WritePropertyName("enableAwaitExchange");
                writer.Write(bool.Parse(EnableAwaitExchange.ToString()));
            }
            if (EnableDirectExchange != null) {
                writer.WritePropertyName("enableDirectExchange");
                writer.Write(bool.Parse(EnableDirectExchange.ToString()));
            }
            if (TransactionSetting != null) {
                TransactionSetting.WriteJson(writer);
            }
            if (ExchangeScript != null) {
                ExchangeScript.WriteJson(writer);
            }
            if (IncrementalExchangeScript != null) {
                IncrementalExchangeScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Description + ":";
            key += EnableAwaitExchange + ":";
            key += EnableDirectExchange + ":";
            key += TransactionSetting + ":";
            key += ExchangeScript + ":";
            key += IncrementalExchangeScript + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
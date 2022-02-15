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
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
        public string Name { set; get; }
        public string Description { set; get; }
        public bool? EnableAwaitExchange { set; get; }
        public bool? EnableDirectExchange { set; get; }
        public string QueueNamespaceId { set; get; }
        public string KeyId { set; get; }
        public Gs2.Gs2Exchange.Model.ScriptSetting ExchangeScript { set; get; }
        public Gs2.Gs2Exchange.Model.LogSetting LogSetting { set; get; }

        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }

        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public CreateNamespaceRequest WithEnableAwaitExchange(bool? enableAwaitExchange) {
            this.EnableAwaitExchange = enableAwaitExchange;
            return this;
        }

        public CreateNamespaceRequest WithEnableDirectExchange(bool? enableDirectExchange) {
            this.EnableDirectExchange = enableDirectExchange;
            return this;
        }

        public CreateNamespaceRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }

        public CreateNamespaceRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

        public CreateNamespaceRequest WithExchangeScript(Gs2.Gs2Exchange.Model.ScriptSetting exchangeScript) {
            this.ExchangeScript = exchangeScript;
            return this;
        }

        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Exchange.Model.LogSetting logSetting) {
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
                .WithEnableAwaitExchange(!data.Keys.Contains("enableAwaitExchange") || data["enableAwaitExchange"] == null ? null : (bool?)bool.Parse(data["enableAwaitExchange"].ToString()))
                .WithEnableDirectExchange(!data.Keys.Contains("enableDirectExchange") || data["enableDirectExchange"] == null ? null : (bool?)bool.Parse(data["enableDirectExchange"].ToString()))
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithExchangeScript(!data.Keys.Contains("exchangeScript") || data["exchangeScript"] == null ? null : Gs2.Gs2Exchange.Model.ScriptSetting.FromJson(data["exchangeScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Exchange.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["enableAwaitExchange"] = EnableAwaitExchange,
                ["enableDirectExchange"] = EnableDirectExchange,
                ["queueNamespaceId"] = QueueNamespaceId,
                ["keyId"] = KeyId,
                ["exchangeScript"] = ExchangeScript?.ToJson(),
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
            if (EnableAwaitExchange != null) {
                writer.WritePropertyName("enableAwaitExchange");
                writer.Write(bool.Parse(EnableAwaitExchange.ToString()));
            }
            if (EnableDirectExchange != null) {
                writer.WritePropertyName("enableDirectExchange");
                writer.Write(bool.Parse(EnableDirectExchange.ToString()));
            }
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (ExchangeScript != null) {
                ExchangeScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}
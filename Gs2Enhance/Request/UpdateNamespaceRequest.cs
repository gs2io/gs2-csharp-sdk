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
using Gs2.Gs2Enhance.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Enhance.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
        public string NamespaceName { set; get; }
        public string Description { set; get; }
        public bool? EnableDirectEnhance { set; get; }
        public Gs2.Gs2Enhance.Model.TransactionSetting TransactionSetting { set; get; }
        public Gs2.Gs2Enhance.Model.ScriptSetting EnhanceScript { set; get; }
        public Gs2.Gs2Enhance.Model.LogSetting LogSetting { set; get; }
        public string QueueNamespaceId { set; get; }
        public string KeyId { set; get; }

        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public UpdateNamespaceRequest WithEnableDirectEnhance(bool? enableDirectEnhance) {
            this.EnableDirectEnhance = enableDirectEnhance;
            return this;
        }

        public UpdateNamespaceRequest WithTransactionSetting(Gs2.Gs2Enhance.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }

        public UpdateNamespaceRequest WithEnhanceScript(Gs2.Gs2Enhance.Model.ScriptSetting enhanceScript) {
            this.EnhanceScript = enhanceScript;
            return this;
        }

        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Enhance.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

        public UpdateNamespaceRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }

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
                .WithEnableDirectEnhance(!data.Keys.Contains("enableDirectEnhance") || data["enableDirectEnhance"] == null ? null : (bool?)bool.Parse(data["enableDirectEnhance"].ToString()))
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Enhance.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithEnhanceScript(!data.Keys.Contains("enhanceScript") || data["enhanceScript"] == null ? null : Gs2.Gs2Enhance.Model.ScriptSetting.FromJson(data["enhanceScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Enhance.Model.LogSetting.FromJson(data["logSetting"]))
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["enableDirectEnhance"] = EnableDirectEnhance,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["enhanceScript"] = EnhanceScript?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
                ["queueNamespaceId"] = QueueNamespaceId,
                ["keyId"] = KeyId,
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
            if (EnableDirectEnhance != null) {
                writer.WritePropertyName("enableDirectEnhance");
                writer.Write(bool.Parse(EnableDirectEnhance.ToString()));
            }
            if (TransactionSetting != null) {
                TransactionSetting.WriteJson(writer);
            }
            if (EnhanceScript != null) {
                EnhanceScript.WriteJson(writer);
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
    }
}
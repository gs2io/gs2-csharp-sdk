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
using Gs2.Gs2Inbox.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inbox.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public bool? IsAutomaticDeletingEnabled { set; get; } = null!;
         public Gs2.Gs2Inbox.Model.TransactionSetting TransactionSetting { set; get; } = null!;
         public Gs2.Gs2Inbox.Model.ScriptSetting ReceiveMessageScript { set; get; } = null!;
         public Gs2.Gs2Inbox.Model.ScriptSetting ReadMessageScript { set; get; } = null!;
         public Gs2.Gs2Inbox.Model.ScriptSetting DeleteMessageScript { set; get; } = null!;
         public Gs2.Gs2Inbox.Model.NotificationSetting ReceiveNotification { set; get; } = null!;
         public Gs2.Gs2Inbox.Model.LogSetting LogSetting { set; get; } = null!;
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
        public UpdateNamespaceRequest WithIsAutomaticDeletingEnabled(bool? isAutomaticDeletingEnabled) {
            this.IsAutomaticDeletingEnabled = isAutomaticDeletingEnabled;
            return this;
        }
        public UpdateNamespaceRequest WithTransactionSetting(Gs2.Gs2Inbox.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public UpdateNamespaceRequest WithReceiveMessageScript(Gs2.Gs2Inbox.Model.ScriptSetting receiveMessageScript) {
            this.ReceiveMessageScript = receiveMessageScript;
            return this;
        }
        public UpdateNamespaceRequest WithReadMessageScript(Gs2.Gs2Inbox.Model.ScriptSetting readMessageScript) {
            this.ReadMessageScript = readMessageScript;
            return this;
        }
        public UpdateNamespaceRequest WithDeleteMessageScript(Gs2.Gs2Inbox.Model.ScriptSetting deleteMessageScript) {
            this.DeleteMessageScript = deleteMessageScript;
            return this;
        }
        public UpdateNamespaceRequest WithReceiveNotification(Gs2.Gs2Inbox.Model.NotificationSetting receiveNotification) {
            this.ReceiveNotification = receiveNotification;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Inbox.Model.LogSetting logSetting) {
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
                .WithIsAutomaticDeletingEnabled(!data.Keys.Contains("isAutomaticDeletingEnabled") || data["isAutomaticDeletingEnabled"] == null ? null : (bool?)bool.Parse(data["isAutomaticDeletingEnabled"].ToString()))
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Inbox.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithReceiveMessageScript(!data.Keys.Contains("receiveMessageScript") || data["receiveMessageScript"] == null ? null : Gs2.Gs2Inbox.Model.ScriptSetting.FromJson(data["receiveMessageScript"]))
                .WithReadMessageScript(!data.Keys.Contains("readMessageScript") || data["readMessageScript"] == null ? null : Gs2.Gs2Inbox.Model.ScriptSetting.FromJson(data["readMessageScript"]))
                .WithDeleteMessageScript(!data.Keys.Contains("deleteMessageScript") || data["deleteMessageScript"] == null ? null : Gs2.Gs2Inbox.Model.ScriptSetting.FromJson(data["deleteMessageScript"]))
                .WithReceiveNotification(!data.Keys.Contains("receiveNotification") || data["receiveNotification"] == null ? null : Gs2.Gs2Inbox.Model.NotificationSetting.FromJson(data["receiveNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Inbox.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["isAutomaticDeletingEnabled"] = IsAutomaticDeletingEnabled,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["receiveMessageScript"] = ReceiveMessageScript?.ToJson(),
                ["readMessageScript"] = ReadMessageScript?.ToJson(),
                ["deleteMessageScript"] = DeleteMessageScript?.ToJson(),
                ["receiveNotification"] = ReceiveNotification?.ToJson(),
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
            if (IsAutomaticDeletingEnabled != null) {
                writer.WritePropertyName("isAutomaticDeletingEnabled");
                writer.Write(bool.Parse(IsAutomaticDeletingEnabled.ToString()));
            }
            if (TransactionSetting != null) {
                TransactionSetting.WriteJson(writer);
            }
            if (ReceiveMessageScript != null) {
                ReceiveMessageScript.WriteJson(writer);
            }
            if (ReadMessageScript != null) {
                ReadMessageScript.WriteJson(writer);
            }
            if (DeleteMessageScript != null) {
                DeleteMessageScript.WriteJson(writer);
            }
            if (ReceiveNotification != null) {
                ReceiveNotification.WriteJson(writer);
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
            key += IsAutomaticDeletingEnabled + ":";
            key += TransactionSetting + ":";
            key += ReceiveMessageScript + ":";
            key += ReadMessageScript + ":";
            key += DeleteMessageScript + ":";
            key += ReceiveNotification + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}
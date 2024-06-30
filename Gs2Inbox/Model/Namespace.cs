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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inbox.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public bool? IsAutomaticDeletingEnabled { set; get; } = null!;
        public Gs2.Gs2Inbox.Model.TransactionSetting TransactionSetting { set; get; } = null!;
        public Gs2.Gs2Inbox.Model.ScriptSetting ReceiveMessageScript { set; get; } = null!;
        public Gs2.Gs2Inbox.Model.ScriptSetting ReadMessageScript { set; get; } = null!;
        public Gs2.Gs2Inbox.Model.ScriptSetting DeleteMessageScript { set; get; } = null!;
        public Gs2.Gs2Inbox.Model.NotificationSetting ReceiveNotification { set; get; } = null!;
        public Gs2.Gs2Inbox.Model.LogSetting LogSetting { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public string QueueNamespaceId { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public string KeyId { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Namespace WithNamespaceId(string namespaceId) {
            this.NamespaceId = namespaceId;
            return this;
        }
        public Namespace WithName(string name) {
            this.Name = name;
            return this;
        }
        public Namespace WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public Namespace WithIsAutomaticDeletingEnabled(bool? isAutomaticDeletingEnabled) {
            this.IsAutomaticDeletingEnabled = isAutomaticDeletingEnabled;
            return this;
        }
        public Namespace WithTransactionSetting(Gs2.Gs2Inbox.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public Namespace WithReceiveMessageScript(Gs2.Gs2Inbox.Model.ScriptSetting receiveMessageScript) {
            this.ReceiveMessageScript = receiveMessageScript;
            return this;
        }
        public Namespace WithReadMessageScript(Gs2.Gs2Inbox.Model.ScriptSetting readMessageScript) {
            this.ReadMessageScript = readMessageScript;
            return this;
        }
        public Namespace WithDeleteMessageScript(Gs2.Gs2Inbox.Model.ScriptSetting deleteMessageScript) {
            this.DeleteMessageScript = deleteMessageScript;
            return this;
        }
        public Namespace WithReceiveNotification(Gs2.Gs2Inbox.Model.NotificationSetting receiveNotification) {
            this.ReceiveNotification = receiveNotification;
            return this;
        }
        public Namespace WithLogSetting(Gs2.Gs2Inbox.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }
        public Namespace WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Namespace WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Namespace WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Namespace WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }
        public Namespace WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Namespace FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Namespace()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithIsAutomaticDeletingEnabled(!data.Keys.Contains("isAutomaticDeletingEnabled") || data["isAutomaticDeletingEnabled"] == null ? null : (bool?)bool.Parse(data["isAutomaticDeletingEnabled"].ToString()))
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Inbox.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithReceiveMessageScript(!data.Keys.Contains("receiveMessageScript") || data["receiveMessageScript"] == null ? null : Gs2.Gs2Inbox.Model.ScriptSetting.FromJson(data["receiveMessageScript"]))
                .WithReadMessageScript(!data.Keys.Contains("readMessageScript") || data["readMessageScript"] == null ? null : Gs2.Gs2Inbox.Model.ScriptSetting.FromJson(data["readMessageScript"]))
                .WithDeleteMessageScript(!data.Keys.Contains("deleteMessageScript") || data["deleteMessageScript"] == null ? null : Gs2.Gs2Inbox.Model.ScriptSetting.FromJson(data["deleteMessageScript"]))
                .WithReceiveNotification(!data.Keys.Contains("receiveNotification") || data["receiveNotification"] == null ? null : Gs2.Gs2Inbox.Model.NotificationSetting.FromJson(data["receiveNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Inbox.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["isAutomaticDeletingEnabled"] = IsAutomaticDeletingEnabled,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["receiveMessageScript"] = ReceiveMessageScript?.ToJson(),
                ["readMessageScript"] = ReadMessageScript?.ToJson(),
                ["deleteMessageScript"] = DeleteMessageScript?.ToJson(),
                ["receiveNotification"] = ReceiveNotification?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceId != null) {
                writer.WritePropertyName("namespaceId");
                writer.Write(NamespaceId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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
                writer.WritePropertyName("transactionSetting");
                TransactionSetting.WriteJson(writer);
            }
            if (ReceiveMessageScript != null) {
                writer.WritePropertyName("receiveMessageScript");
                ReceiveMessageScript.WriteJson(writer);
            }
            if (ReadMessageScript != null) {
                writer.WritePropertyName("readMessageScript");
                ReadMessageScript.WriteJson(writer);
            }
            if (DeleteMessageScript != null) {
                writer.WritePropertyName("deleteMessageScript");
                DeleteMessageScript.WriteJson(writer);
            }
            if (ReceiveNotification != null) {
                writer.WritePropertyName("receiveNotification");
                ReceiveNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                writer.WritePropertyName("logSetting");
                LogSetting.WriteJson(writer);
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Namespace;
            var diff = 0;
            if (NamespaceId == null && NamespaceId == other.NamespaceId)
            {
                // null and null
            }
            else
            {
                diff += NamespaceId.CompareTo(other.NamespaceId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (IsAutomaticDeletingEnabled == null && IsAutomaticDeletingEnabled == other.IsAutomaticDeletingEnabled)
            {
                // null and null
            }
            else
            {
                diff += IsAutomaticDeletingEnabled == other.IsAutomaticDeletingEnabled ? 0 : 1;
            }
            if (TransactionSetting == null && TransactionSetting == other.TransactionSetting)
            {
                // null and null
            }
            else
            {
                diff += TransactionSetting.CompareTo(other.TransactionSetting);
            }
            if (ReceiveMessageScript == null && ReceiveMessageScript == other.ReceiveMessageScript)
            {
                // null and null
            }
            else
            {
                diff += ReceiveMessageScript.CompareTo(other.ReceiveMessageScript);
            }
            if (ReadMessageScript == null && ReadMessageScript == other.ReadMessageScript)
            {
                // null and null
            }
            else
            {
                diff += ReadMessageScript.CompareTo(other.ReadMessageScript);
            }
            if (DeleteMessageScript == null && DeleteMessageScript == other.DeleteMessageScript)
            {
                // null and null
            }
            else
            {
                diff += DeleteMessageScript.CompareTo(other.DeleteMessageScript);
            }
            if (ReceiveNotification == null && ReceiveNotification == other.ReceiveNotification)
            {
                // null and null
            }
            else
            {
                diff += ReceiveNotification.CompareTo(other.ReceiveNotification);
            }
            if (LogSetting == null && LogSetting == other.LogSetting)
            {
                // null and null
            }
            else
            {
                diff += LogSetting.CompareTo(other.LogSetting);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            if (QueueNamespaceId == null && QueueNamespaceId == other.QueueNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += QueueNamespaceId.CompareTo(other.QueueNamespaceId);
            }
            if (KeyId == null && KeyId == other.KeyId)
            {
                // null and null
            }
            else
            {
                diff += KeyId.CompareTo(other.KeyId);
            }
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (NamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.namespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.description.error.tooLong"),
                    });
                }
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inbox.namespace.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Namespace {
                NamespaceId = NamespaceId,
                Name = Name,
                Description = Description,
                IsAutomaticDeletingEnabled = IsAutomaticDeletingEnabled,
                TransactionSetting = TransactionSetting.Clone() as Gs2.Gs2Inbox.Model.TransactionSetting,
                ReceiveMessageScript = ReceiveMessageScript.Clone() as Gs2.Gs2Inbox.Model.ScriptSetting,
                ReadMessageScript = ReadMessageScript.Clone() as Gs2.Gs2Inbox.Model.ScriptSetting,
                DeleteMessageScript = DeleteMessageScript.Clone() as Gs2.Gs2Inbox.Model.ScriptSetting,
                ReceiveNotification = ReceiveNotification.Clone() as Gs2.Gs2Inbox.Model.NotificationSetting,
                LogSetting = LogSetting.Clone() as Gs2.Gs2Inbox.Model.LogSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                QueueNamespaceId = QueueNamespaceId,
                KeyId = KeyId,
                Revision = Revision,
            };
        }
    }
}
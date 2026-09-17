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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Exchange.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public bool? EnableDirectExchange { set; get; }
        public bool? EnableAwaitExchange { set; get; }
        [Obsolete("This method is deprecated")]
        public Gs2.Gs2Exchange.Model.TransactionSetting TransactionSetting { set; get; }
        public Gs2.Gs2Exchange.Model.TransactionSettingV2 TransactionSettingV2 { set; get; }
        public Gs2.Gs2Exchange.Model.ScriptSetting ExchangeScript { set; get; }
        public Gs2.Gs2Exchange.Model.ScriptSetting IncrementalExchangeScript { set; get; }
        public Gs2.Gs2Exchange.Model.ScriptSetting AcquireAwaitScript { set; get; }
        public Gs2.Gs2Exchange.Model.LogSetting LogSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        [Obsolete("This method is deprecated")]
        public string QueueNamespaceId { set; get; }
        [Obsolete("This method is deprecated")]
        public string KeyId { set; get; }
        public long? Revision { set; get; }
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
        public Namespace WithEnableDirectExchange(bool? enableDirectExchange) {
            this.EnableDirectExchange = enableDirectExchange;
            return this;
        }
        public Namespace WithEnableAwaitExchange(bool? enableAwaitExchange) {
            this.EnableAwaitExchange = enableAwaitExchange;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Namespace WithTransactionSetting(Gs2.Gs2Exchange.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public Namespace WithTransactionSettingV2(Gs2.Gs2Exchange.Model.TransactionSettingV2 transactionSettingV2) {
            this.TransactionSettingV2 = transactionSettingV2;
            return this;
        }
        public Namespace WithExchangeScript(Gs2.Gs2Exchange.Model.ScriptSetting exchangeScript) {
            this.ExchangeScript = exchangeScript;
            return this;
        }
        public Namespace WithIncrementalExchangeScript(Gs2.Gs2Exchange.Model.ScriptSetting incrementalExchangeScript) {
            this.IncrementalExchangeScript = incrementalExchangeScript;
            return this;
        }
        public Namespace WithAcquireAwaitScript(Gs2.Gs2Exchange.Model.ScriptSetting acquireAwaitScript) {
            this.AcquireAwaitScript = acquireAwaitScript;
            return this;
        }
        public Namespace WithLogSetting(Gs2.Gs2Exchange.Model.LogSetting logSetting) {
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+)",
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Namespace()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithEnableDirectExchange(!data.Keys.Contains("enableDirectExchange") || data["enableDirectExchange"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableBool(data["enableDirectExchange"].ToString()))
                .WithEnableAwaitExchange(!data.Keys.Contains("enableAwaitExchange") || data["enableAwaitExchange"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableBool(data["enableAwaitExchange"].ToString()))
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Exchange.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithTransactionSettingV2(!data.Keys.Contains("transactionSettingV2") || data["transactionSettingV2"] == null ? null : Gs2.Gs2Exchange.Model.TransactionSettingV2.FromJson(data["transactionSettingV2"]))
                .WithExchangeScript(!data.Keys.Contains("exchangeScript") || data["exchangeScript"] == null ? null : Gs2.Gs2Exchange.Model.ScriptSetting.FromJson(data["exchangeScript"]))
                .WithIncrementalExchangeScript(!data.Keys.Contains("incrementalExchangeScript") || data["incrementalExchangeScript"] == null ? null : Gs2.Gs2Exchange.Model.ScriptSetting.FromJson(data["incrementalExchangeScript"]))
                .WithAcquireAwaitScript(!data.Keys.Contains("acquireAwaitScript") || data["acquireAwaitScript"] == null ? null : Gs2.Gs2Exchange.Model.ScriptSetting.FromJson(data["acquireAwaitScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Exchange.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["enableDirectExchange"] = EnableDirectExchange,
                ["enableAwaitExchange"] = EnableAwaitExchange,
                ["transactionSettingV2"] = TransactionSettingV2?.ToJson(),
                ["exchangeScript"] = ExchangeScript?.ToJson(),
                ["incrementalExchangeScript"] = IncrementalExchangeScript?.ToJson(),
                ["acquireAwaitScript"] = AcquireAwaitScript?.ToJson(),
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
            if (EnableDirectExchange != null) {
                writer.WritePropertyName("enableDirectExchange");
                writer.Write(bool.Parse(EnableDirectExchange.ToString()));
            }
            if (EnableAwaitExchange != null) {
                writer.WritePropertyName("enableAwaitExchange");
                writer.Write(bool.Parse(EnableAwaitExchange.ToString()));
            }
            if (TransactionSetting != null) {
                writer.WritePropertyName("transactionSetting");
                TransactionSetting.WriteJson(writer);
            }
            if (TransactionSettingV2 != null) {
                writer.WritePropertyName("transactionSettingV2");
                TransactionSettingV2.WriteJson(writer);
            }
            if (ExchangeScript != null) {
                writer.WritePropertyName("exchangeScript");
                ExchangeScript.WriteJson(writer);
            }
            if (IncrementalExchangeScript != null) {
                writer.WritePropertyName("incrementalExchangeScript");
                IncrementalExchangeScript.WriteJson(writer);
            }
            if (AcquireAwaitScript != null) {
                writer.WritePropertyName("acquireAwaitScript");
                AcquireAwaitScript.WriteJson(writer);
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Namespace.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(NamespaceId, other.NamespaceId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EnableDirectExchange, other.EnableDirectExchange);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EnableAwaitExchange, other.EnableAwaitExchange);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TransactionSetting, other.TransactionSetting);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TransactionSettingV2, other.TransactionSettingV2);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ExchangeScript, other.ExchangeScript);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(IncrementalExchangeScript, other.IncrementalExchangeScript);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(AcquireAwaitScript, other.AcquireAwaitScript);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(LogSetting, other.LogSetting);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(QueueNamespaceId, other.QueueNamespaceId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(KeyId, other.KeyId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (NamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "exchange.namespace.namespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "exchange.namespace.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "exchange.namespace.description.error.tooLong"),
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
                        new RequestError("namespace", "exchange.namespace.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "exchange.namespace.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "exchange.namespace.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "exchange.namespace.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "exchange.namespace.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "exchange.namespace.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Namespace {
                NamespaceId = NamespaceId,
                Name = Name,
                Description = Description,
                EnableDirectExchange = EnableDirectExchange,
                EnableAwaitExchange = EnableAwaitExchange,
                TransactionSetting = TransactionSetting?.Clone() as Gs2.Gs2Exchange.Model.TransactionSetting,
                TransactionSettingV2 = TransactionSettingV2?.Clone() as Gs2.Gs2Exchange.Model.TransactionSettingV2,
                ExchangeScript = ExchangeScript?.Clone() as Gs2.Gs2Exchange.Model.ScriptSetting,
                IncrementalExchangeScript = IncrementalExchangeScript?.Clone() as Gs2.Gs2Exchange.Model.ScriptSetting,
                AcquireAwaitScript = AcquireAwaitScript?.Clone() as Gs2.Gs2Exchange.Model.ScriptSetting,
                LogSetting = LogSetting?.Clone() as Gs2.Gs2Exchange.Model.LogSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                QueueNamespaceId = QueueNamespaceId,
                KeyId = KeyId,
                Revision = Revision,
            };
        }
    }
}
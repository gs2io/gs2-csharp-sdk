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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string CurrencyUsagePriority { set; get; } = null!;
        public bool? SharedFreeCurrency { set; get; } = null!;
        public Gs2.Gs2Money2.Model.PlatformSetting PlatformSetting { set; get; } = null!;
        public Gs2.Gs2Money2.Model.ScriptSetting ChangeBalanceScript { set; get; } = null!;
        public Gs2.Gs2Money2.Model.LogSetting LogSetting { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
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
        public Namespace WithCurrencyUsagePriority(string currencyUsagePriority) {
            this.CurrencyUsagePriority = currencyUsagePriority;
            return this;
        }
        public Namespace WithSharedFreeCurrency(bool? sharedFreeCurrency) {
            this.SharedFreeCurrency = sharedFreeCurrency;
            return this;
        }
        public Namespace WithPlatformSetting(Gs2.Gs2Money2.Model.PlatformSetting platformSetting) {
            this.PlatformSetting = platformSetting;
            return this;
        }
        public Namespace WithChangeBalanceScript(Gs2.Gs2Money2.Model.ScriptSetting changeBalanceScript) {
            this.ChangeBalanceScript = changeBalanceScript;
            return this;
        }
        public Namespace WithLogSetting(Gs2.Gs2Money2.Model.LogSetting logSetting) {
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
        public Namespace WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+)",
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
                .WithCurrencyUsagePriority(!data.Keys.Contains("currencyUsagePriority") || data["currencyUsagePriority"] == null ? null : data["currencyUsagePriority"].ToString())
                .WithSharedFreeCurrency(!data.Keys.Contains("sharedFreeCurrency") || data["sharedFreeCurrency"] == null ? null : (bool?)bool.Parse(data["sharedFreeCurrency"].ToString()))
                .WithPlatformSetting(!data.Keys.Contains("platformSetting") || data["platformSetting"] == null ? null : Gs2.Gs2Money2.Model.PlatformSetting.FromJson(data["platformSetting"]))
                .WithChangeBalanceScript(!data.Keys.Contains("changeBalanceScript") || data["changeBalanceScript"] == null ? null : Gs2.Gs2Money2.Model.ScriptSetting.FromJson(data["changeBalanceScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Money2.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["currencyUsagePriority"] = CurrencyUsagePriority,
                ["sharedFreeCurrency"] = SharedFreeCurrency,
                ["platformSetting"] = PlatformSetting?.ToJson(),
                ["changeBalanceScript"] = ChangeBalanceScript?.ToJson(),
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
            if (CurrencyUsagePriority != null) {
                writer.WritePropertyName("currencyUsagePriority");
                writer.Write(CurrencyUsagePriority.ToString());
            }
            if (SharedFreeCurrency != null) {
                writer.WritePropertyName("sharedFreeCurrency");
                writer.Write(bool.Parse(SharedFreeCurrency.ToString()));
            }
            if (PlatformSetting != null) {
                writer.WritePropertyName("platformSetting");
                PlatformSetting.WriteJson(writer);
            }
            if (ChangeBalanceScript != null) {
                writer.WritePropertyName("changeBalanceScript");
                ChangeBalanceScript.WriteJson(writer);
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
            if (CurrencyUsagePriority == null && CurrencyUsagePriority == other.CurrencyUsagePriority)
            {
                // null and null
            }
            else
            {
                diff += CurrencyUsagePriority.CompareTo(other.CurrencyUsagePriority);
            }
            if (SharedFreeCurrency == null && SharedFreeCurrency == other.SharedFreeCurrency)
            {
                // null and null
            }
            else
            {
                diff += SharedFreeCurrency == other.SharedFreeCurrency ? 0 : 1;
            }
            if (PlatformSetting == null && PlatformSetting == other.PlatformSetting)
            {
                // null and null
            }
            else
            {
                diff += PlatformSetting.CompareTo(other.PlatformSetting);
            }
            if (ChangeBalanceScript == null && ChangeBalanceScript == other.ChangeBalanceScript)
            {
                // null and null
            }
            else
            {
                diff += ChangeBalanceScript.CompareTo(other.ChangeBalanceScript);
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
                        new RequestError("namespace", "money2.namespace.namespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 32) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "money2.namespace.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "money2.namespace.description.error.tooLong"),
                    });
                }
            }
            {
                switch (CurrencyUsagePriority) {
                    case "PrioritizeFree":
                    case "PrioritizePaid":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("namespace", "money2.namespace.currencyUsagePriority.error.invalid"),
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
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "money2.namespace.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "money2.namespace.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "money2.namespace.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "money2.namespace.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "money2.namespace.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "money2.namespace.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Namespace {
                NamespaceId = NamespaceId,
                Name = Name,
                Description = Description,
                CurrencyUsagePriority = CurrencyUsagePriority,
                SharedFreeCurrency = SharedFreeCurrency,
                PlatformSetting = PlatformSetting.Clone() as Gs2.Gs2Money2.Model.PlatformSetting,
                ChangeBalanceScript = ChangeBalanceScript.Clone() as Gs2.Gs2Money2.Model.ScriptSetting,
                LogSetting = LogSetting.Clone() as Gs2.Gs2Money2.Model.LogSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
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

namespace Gs2.Gs2Account.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public bool? ChangePasswordIfTakeOver { set; get; } = null!;
        public bool? DifferentUserIdForLoginAndDataRetention { set; get; } = null!;
        public Gs2.Gs2Account.Model.ScriptSetting CreateAccountScript { set; get; } = null!;
        public Gs2.Gs2Account.Model.ScriptSetting AuthenticationScript { set; get; } = null!;
        public Gs2.Gs2Account.Model.ScriptSetting CreateTakeOverScript { set; get; } = null!;
        public Gs2.Gs2Account.Model.ScriptSetting DoTakeOverScript { set; get; } = null!;
        public Gs2.Gs2Account.Model.LogSetting LogSetting { set; get; } = null!;
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
        public Namespace WithChangePasswordIfTakeOver(bool? changePasswordIfTakeOver) {
            this.ChangePasswordIfTakeOver = changePasswordIfTakeOver;
            return this;
        }
        public Namespace WithDifferentUserIdForLoginAndDataRetention(bool? differentUserIdForLoginAndDataRetention) {
            this.DifferentUserIdForLoginAndDataRetention = differentUserIdForLoginAndDataRetention;
            return this;
        }
        public Namespace WithCreateAccountScript(Gs2.Gs2Account.Model.ScriptSetting createAccountScript) {
            this.CreateAccountScript = createAccountScript;
            return this;
        }
        public Namespace WithAuthenticationScript(Gs2.Gs2Account.Model.ScriptSetting authenticationScript) {
            this.AuthenticationScript = authenticationScript;
            return this;
        }
        public Namespace WithCreateTakeOverScript(Gs2.Gs2Account.Model.ScriptSetting createTakeOverScript) {
            this.CreateTakeOverScript = createTakeOverScript;
            return this;
        }
        public Namespace WithDoTakeOverScript(Gs2.Gs2Account.Model.ScriptSetting doTakeOverScript) {
            this.DoTakeOverScript = doTakeOverScript;
            return this;
        }
        public Namespace WithLogSetting(Gs2.Gs2Account.Model.LogSetting logSetting) {
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+)",
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
                .WithChangePasswordIfTakeOver(!data.Keys.Contains("changePasswordIfTakeOver") || data["changePasswordIfTakeOver"] == null ? null : (bool?)bool.Parse(data["changePasswordIfTakeOver"].ToString()))
                .WithDifferentUserIdForLoginAndDataRetention(!data.Keys.Contains("differentUserIdForLoginAndDataRetention") || data["differentUserIdForLoginAndDataRetention"] == null ? null : (bool?)bool.Parse(data["differentUserIdForLoginAndDataRetention"].ToString()))
                .WithCreateAccountScript(!data.Keys.Contains("createAccountScript") || data["createAccountScript"] == null ? null : Gs2.Gs2Account.Model.ScriptSetting.FromJson(data["createAccountScript"]))
                .WithAuthenticationScript(!data.Keys.Contains("authenticationScript") || data["authenticationScript"] == null ? null : Gs2.Gs2Account.Model.ScriptSetting.FromJson(data["authenticationScript"]))
                .WithCreateTakeOverScript(!data.Keys.Contains("createTakeOverScript") || data["createTakeOverScript"] == null ? null : Gs2.Gs2Account.Model.ScriptSetting.FromJson(data["createTakeOverScript"]))
                .WithDoTakeOverScript(!data.Keys.Contains("doTakeOverScript") || data["doTakeOverScript"] == null ? null : Gs2.Gs2Account.Model.ScriptSetting.FromJson(data["doTakeOverScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Account.Model.LogSetting.FromJson(data["logSetting"]))
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
                ["changePasswordIfTakeOver"] = ChangePasswordIfTakeOver,
                ["differentUserIdForLoginAndDataRetention"] = DifferentUserIdForLoginAndDataRetention,
                ["createAccountScript"] = CreateAccountScript?.ToJson(),
                ["authenticationScript"] = AuthenticationScript?.ToJson(),
                ["createTakeOverScript"] = CreateTakeOverScript?.ToJson(),
                ["doTakeOverScript"] = DoTakeOverScript?.ToJson(),
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
            if (ChangePasswordIfTakeOver != null) {
                writer.WritePropertyName("changePasswordIfTakeOver");
                writer.Write(bool.Parse(ChangePasswordIfTakeOver.ToString()));
            }
            if (DifferentUserIdForLoginAndDataRetention != null) {
                writer.WritePropertyName("differentUserIdForLoginAndDataRetention");
                writer.Write(bool.Parse(DifferentUserIdForLoginAndDataRetention.ToString()));
            }
            if (CreateAccountScript != null) {
                writer.WritePropertyName("createAccountScript");
                CreateAccountScript.WriteJson(writer);
            }
            if (AuthenticationScript != null) {
                writer.WritePropertyName("authenticationScript");
                AuthenticationScript.WriteJson(writer);
            }
            if (CreateTakeOverScript != null) {
                writer.WritePropertyName("createTakeOverScript");
                CreateTakeOverScript.WriteJson(writer);
            }
            if (DoTakeOverScript != null) {
                writer.WritePropertyName("doTakeOverScript");
                DoTakeOverScript.WriteJson(writer);
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
            if (ChangePasswordIfTakeOver == null && ChangePasswordIfTakeOver == other.ChangePasswordIfTakeOver)
            {
                // null and null
            }
            else
            {
                diff += ChangePasswordIfTakeOver == other.ChangePasswordIfTakeOver ? 0 : 1;
            }
            if (DifferentUserIdForLoginAndDataRetention == null && DifferentUserIdForLoginAndDataRetention == other.DifferentUserIdForLoginAndDataRetention)
            {
                // null and null
            }
            else
            {
                diff += DifferentUserIdForLoginAndDataRetention == other.DifferentUserIdForLoginAndDataRetention ? 0 : 1;
            }
            if (CreateAccountScript == null && CreateAccountScript == other.CreateAccountScript)
            {
                // null and null
            }
            else
            {
                diff += CreateAccountScript.CompareTo(other.CreateAccountScript);
            }
            if (AuthenticationScript == null && AuthenticationScript == other.AuthenticationScript)
            {
                // null and null
            }
            else
            {
                diff += AuthenticationScript.CompareTo(other.AuthenticationScript);
            }
            if (CreateTakeOverScript == null && CreateTakeOverScript == other.CreateTakeOverScript)
            {
                // null and null
            }
            else
            {
                diff += CreateTakeOverScript.CompareTo(other.CreateTakeOverScript);
            }
            if (DoTakeOverScript == null && DoTakeOverScript == other.DoTakeOverScript)
            {
                // null and null
            }
            else
            {
                diff += DoTakeOverScript.CompareTo(other.DoTakeOverScript);
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
                        new RequestError("namespace", "account.namespace.namespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "account.namespace.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "account.namespace.description.error.tooLong"),
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
                        new RequestError("namespace", "account.namespace.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "account.namespace.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "account.namespace.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "account.namespace.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "account.namespace.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "account.namespace.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Namespace {
                NamespaceId = NamespaceId,
                Name = Name,
                Description = Description,
                ChangePasswordIfTakeOver = ChangePasswordIfTakeOver,
                DifferentUserIdForLoginAndDataRetention = DifferentUserIdForLoginAndDataRetention,
                CreateAccountScript = CreateAccountScript.Clone() as Gs2.Gs2Account.Model.ScriptSetting,
                AuthenticationScript = AuthenticationScript.Clone() as Gs2.Gs2Account.Model.ScriptSetting,
                CreateTakeOverScript = CreateTakeOverScript.Clone() as Gs2.Gs2Account.Model.ScriptSetting,
                DoTakeOverScript = DoTakeOverScript.Clone() as Gs2.Gs2Account.Model.ScriptSetting,
                LogSetting = LogSetting.Clone() as Gs2.Gs2Account.Model.LogSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
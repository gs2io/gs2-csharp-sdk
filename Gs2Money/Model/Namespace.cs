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

namespace Gs2.Gs2Money.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Priority { set; get; }
        public bool? ShareFree { set; get; }
        public string Currency { set; get; }
        public string AppleKey { set; get; }
        public string GoogleKey { set; get; }
        public bool? EnableFakeReceipt { set; get; }
        public Gs2.Gs2Money.Model.ScriptSetting CreateWalletScript { set; get; }
        public Gs2.Gs2Money.Model.ScriptSetting DepositScript { set; get; }
        public Gs2.Gs2Money.Model.ScriptSetting WithdrawScript { set; get; }
        public double? Balance { set; get; }
        public Gs2.Gs2Money.Model.LogSetting LogSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
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

        public Namespace WithPriority(string priority) {
            this.Priority = priority;
            return this;
        }

        public Namespace WithShareFree(bool? shareFree) {
            this.ShareFree = shareFree;
            return this;
        }

        public Namespace WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }

        public Namespace WithAppleKey(string appleKey) {
            this.AppleKey = appleKey;
            return this;
        }

        public Namespace WithGoogleKey(string googleKey) {
            this.GoogleKey = googleKey;
            return this;
        }

        public Namespace WithEnableFakeReceipt(bool? enableFakeReceipt) {
            this.EnableFakeReceipt = enableFakeReceipt;
            return this;
        }

        public Namespace WithCreateWalletScript(Gs2.Gs2Money.Model.ScriptSetting createWalletScript) {
            this.CreateWalletScript = createWalletScript;
            return this;
        }

        public Namespace WithDepositScript(Gs2.Gs2Money.Model.ScriptSetting depositScript) {
            this.DepositScript = depositScript;
            return this;
        }

        public Namespace WithWithdrawScript(Gs2.Gs2Money.Model.ScriptSetting withdrawScript) {
            this.WithdrawScript = withdrawScript;
            return this;
        }

        public Namespace WithBalance(double? balance) {
            this.Balance = balance;
            return this;
        }

        public Namespace WithLogSetting(Gs2.Gs2Money.Model.LogSetting logSetting) {
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+)",
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
                .WithPriority(!data.Keys.Contains("priority") || data["priority"] == null ? null : data["priority"].ToString())
                .WithShareFree(!data.Keys.Contains("shareFree") || data["shareFree"] == null ? null : (bool?)bool.Parse(data["shareFree"].ToString()))
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithAppleKey(!data.Keys.Contains("appleKey") || data["appleKey"] == null ? null : data["appleKey"].ToString())
                .WithGoogleKey(!data.Keys.Contains("googleKey") || data["googleKey"] == null ? null : data["googleKey"].ToString())
                .WithEnableFakeReceipt(!data.Keys.Contains("enableFakeReceipt") || data["enableFakeReceipt"] == null ? null : (bool?)bool.Parse(data["enableFakeReceipt"].ToString()))
                .WithCreateWalletScript(!data.Keys.Contains("createWalletScript") || data["createWalletScript"] == null ? null : Gs2.Gs2Money.Model.ScriptSetting.FromJson(data["createWalletScript"]))
                .WithDepositScript(!data.Keys.Contains("depositScript") || data["depositScript"] == null ? null : Gs2.Gs2Money.Model.ScriptSetting.FromJson(data["depositScript"]))
                .WithWithdrawScript(!data.Keys.Contains("withdrawScript") || data["withdrawScript"] == null ? null : Gs2.Gs2Money.Model.ScriptSetting.FromJson(data["withdrawScript"]))
                .WithBalance(!data.Keys.Contains("balance") || data["balance"] == null ? null : (double?)double.Parse(data["balance"].ToString()))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Money.Model.LogSetting.FromJson(data["logSetting"]))
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
                ["priority"] = Priority,
                ["shareFree"] = ShareFree,
                ["currency"] = Currency,
                ["appleKey"] = AppleKey,
                ["googleKey"] = GoogleKey,
                ["enableFakeReceipt"] = EnableFakeReceipt,
                ["createWalletScript"] = CreateWalletScript?.ToJson(),
                ["depositScript"] = DepositScript?.ToJson(),
                ["withdrawScript"] = WithdrawScript?.ToJson(),
                ["balance"] = Balance,
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
                writer.WritePropertyName("createWalletScript");
                CreateWalletScript.WriteJson(writer);
            }
            if (DepositScript != null) {
                writer.WritePropertyName("depositScript");
                DepositScript.WriteJson(writer);
            }
            if (WithdrawScript != null) {
                writer.WritePropertyName("withdrawScript");
                WithdrawScript.WriteJson(writer);
            }
            if (Balance != null) {
                writer.WritePropertyName("balance");
                writer.Write(double.Parse(Balance.ToString()));
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
            if (Priority == null && Priority == other.Priority)
            {
                // null and null
            }
            else
            {
                diff += Priority.CompareTo(other.Priority);
            }
            if (ShareFree == null && ShareFree == other.ShareFree)
            {
                // null and null
            }
            else
            {
                diff += ShareFree == other.ShareFree ? 0 : 1;
            }
            if (Currency == null && Currency == other.Currency)
            {
                // null and null
            }
            else
            {
                diff += Currency.CompareTo(other.Currency);
            }
            if (AppleKey == null && AppleKey == other.AppleKey)
            {
                // null and null
            }
            else
            {
                diff += AppleKey.CompareTo(other.AppleKey);
            }
            if (GoogleKey == null && GoogleKey == other.GoogleKey)
            {
                // null and null
            }
            else
            {
                diff += GoogleKey.CompareTo(other.GoogleKey);
            }
            if (EnableFakeReceipt == null && EnableFakeReceipt == other.EnableFakeReceipt)
            {
                // null and null
            }
            else
            {
                diff += EnableFakeReceipt == other.EnableFakeReceipt ? 0 : 1;
            }
            if (CreateWalletScript == null && CreateWalletScript == other.CreateWalletScript)
            {
                // null and null
            }
            else
            {
                diff += CreateWalletScript.CompareTo(other.CreateWalletScript);
            }
            if (DepositScript == null && DepositScript == other.DepositScript)
            {
                // null and null
            }
            else
            {
                diff += DepositScript.CompareTo(other.DepositScript);
            }
            if (WithdrawScript == null && WithdrawScript == other.WithdrawScript)
            {
                // null and null
            }
            else
            {
                diff += WithdrawScript.CompareTo(other.WithdrawScript);
            }
            if (Balance == null && Balance == other.Balance)
            {
                // null and null
            }
            else
            {
                diff += (int)(Balance - other.Balance);
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
    }
}
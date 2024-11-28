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
	public class Wallet : IComparable
	{
        public string WalletId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public int? Slot { set; get; } = null!;
        public Gs2.Gs2Money2.Model.WalletSummary Summary { set; get; } = null!;
        public Gs2.Gs2Money2.Model.DepositTransaction[] DepositTransactions { set; get; } = null!;
        public bool? SharedFreeCurrency { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Wallet WithWalletId(string walletId) {
            this.WalletId = walletId;
            return this;
        }
        public Wallet WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Wallet WithSlot(int? slot) {
            this.Slot = slot;
            return this;
        }
        public Wallet WithSummary(Gs2.Gs2Money2.Model.WalletSummary summary) {
            this.Summary = summary;
            return this;
        }
        public Wallet WithDepositTransactions(Gs2.Gs2Money2.Model.DepositTransaction[] depositTransactions) {
            this.DepositTransactions = depositTransactions;
            return this;
        }
        public Wallet WithSharedFreeCurrency(bool? sharedFreeCurrency) {
            this.SharedFreeCurrency = sharedFreeCurrency;
            return this;
        }
        public Wallet WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Wallet WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Wallet WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _slotRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSlotFromGrn(
            string grn
        )
        {
            var match = _slotRegex.Match(grn);
            if (!match.Success || !match.Groups["slot"].Success)
            {
                return null;
            }
            return match.Groups["slot"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Wallet FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Wallet()
                .WithWalletId(!data.Keys.Contains("walletId") || data["walletId"] == null ? null : data["walletId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)(data["slot"].ToString().Contains(".") ? (int)double.Parse(data["slot"].ToString()) : int.Parse(data["slot"].ToString())))
                .WithSummary(!data.Keys.Contains("summary") || data["summary"] == null ? null : Gs2.Gs2Money2.Model.WalletSummary.FromJson(data["summary"]))
                .WithDepositTransactions(!data.Keys.Contains("depositTransactions") || data["depositTransactions"] == null || !data["depositTransactions"].IsArray ? null : data["depositTransactions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Money2.Model.DepositTransaction.FromJson(v);
                }).ToArray())
                .WithSharedFreeCurrency(!data.Keys.Contains("sharedFreeCurrency") || data["sharedFreeCurrency"] == null ? null : (bool?)bool.Parse(data["sharedFreeCurrency"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData depositTransactionsJsonData = null;
            if (DepositTransactions != null && DepositTransactions.Length > 0)
            {
                depositTransactionsJsonData = new JsonData();
                foreach (var depositTransaction in DepositTransactions)
                {
                    depositTransactionsJsonData.Add(depositTransaction.ToJson());
                }
            }
            return new JsonData {
                ["walletId"] = WalletId,
                ["userId"] = UserId,
                ["slot"] = Slot,
                ["summary"] = Summary?.ToJson(),
                ["depositTransactions"] = depositTransactionsJsonData,
                ["sharedFreeCurrency"] = SharedFreeCurrency,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (WalletId != null) {
                writer.WritePropertyName("walletId");
                writer.Write(WalletId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Slot != null) {
                writer.WritePropertyName("slot");
                writer.Write((Slot.ToString().Contains(".") ? (int)double.Parse(Slot.ToString()) : int.Parse(Slot.ToString())));
            }
            if (Summary != null) {
                writer.WritePropertyName("summary");
                Summary.WriteJson(writer);
            }
            if (DepositTransactions != null) {
                writer.WritePropertyName("depositTransactions");
                writer.WriteArrayStart();
                foreach (var depositTransaction in DepositTransactions)
                {
                    if (depositTransaction != null) {
                        depositTransaction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (SharedFreeCurrency != null) {
                writer.WritePropertyName("sharedFreeCurrency");
                writer.Write(bool.Parse(SharedFreeCurrency.ToString()));
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
            var other = obj as Wallet;
            var diff = 0;
            if (WalletId == null && WalletId == other.WalletId)
            {
                // null and null
            }
            else
            {
                diff += WalletId.CompareTo(other.WalletId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Slot == null && Slot == other.Slot)
            {
                // null and null
            }
            else
            {
                diff += (int)(Slot - other.Slot);
            }
            if (Summary == null && Summary == other.Summary)
            {
                // null and null
            }
            else
            {
                diff += Summary.CompareTo(other.Summary);
            }
            if (DepositTransactions == null && DepositTransactions == other.DepositTransactions)
            {
                // null and null
            }
            else
            {
                diff += DepositTransactions.Length - other.DepositTransactions.Length;
                for (var i = 0; i < DepositTransactions.Length; i++)
                {
                    diff += DepositTransactions[i].CompareTo(other.DepositTransactions[i]);
                }
            }
            if (SharedFreeCurrency == null && SharedFreeCurrency == other.SharedFreeCurrency)
            {
                // null and null
            }
            else
            {
                diff += SharedFreeCurrency == other.SharedFreeCurrency ? 0 : 1;
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
                if (WalletId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.walletId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Slot < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.slot.error.invalid"),
                    });
                }
                if (Slot > 100000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.slot.error.invalid"),
                    });
                }
            }
            {
            }
            {
                if (DepositTransactions.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.depositTransactions.error.tooMany"),
                    });
                }
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money2.wallet.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Wallet {
                WalletId = WalletId,
                UserId = UserId,
                Slot = Slot,
                Summary = Summary.Clone() as Gs2.Gs2Money2.Model.WalletSummary,
                DepositTransactions = DepositTransactions.Clone() as Gs2.Gs2Money2.Model.DepositTransaction[],
                SharedFreeCurrency = SharedFreeCurrency,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
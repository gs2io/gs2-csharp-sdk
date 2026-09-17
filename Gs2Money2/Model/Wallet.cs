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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Wallet : IComparable
	{
        public string WalletId { set; get; }
        public string UserId { set; get; }
        public int? Slot { set; get; }
        public Gs2.Gs2Money2.Model.WalletSummary Summary { set; get; }
        public Gs2.Gs2Money2.Model.DepositTransaction[] DepositTransactions { set; get; }
        public bool? SharedFreeCurrency { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Wallet()
                .WithWalletId(!data.Keys.Contains("walletId") || data["walletId"] == null ? null : data["walletId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["slot"].ToString()))
                .WithSummary(!data.Keys.Contains("summary") || data["summary"] == null ? null : Gs2.Gs2Money2.Model.WalletSummary.FromJson(data["summary"]))
                .WithDepositTransactions(!data.Keys.Contains("depositTransactions") || data["depositTransactions"] == null || !data["depositTransactions"].IsArray ? null : data["depositTransactions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Money2.Model.DepositTransaction.FromJson(v);
                }).ToArray())
                .WithSharedFreeCurrency(!data.Keys.Contains("sharedFreeCurrency") || data["sharedFreeCurrency"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableBool(data["sharedFreeCurrency"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Wallet.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(WalletId, other.WalletId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Slot, other.Slot);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Summary, other.Summary);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(DepositTransactions, other.DepositTransactions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(SharedFreeCurrency, other.SharedFreeCurrency);
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
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
                Summary = Summary?.Clone() as Gs2.Gs2Money2.Model.WalletSummary,
                DepositTransactions = DepositTransactions?.Clone() as Gs2.Gs2Money2.Model.DepositTransaction[],
                SharedFreeCurrency = SharedFreeCurrency,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
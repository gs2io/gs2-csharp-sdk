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
	public class UnusedBalance : IComparable
	{
        public string UnusedBalanceId { set; get; } = null!;
        public string Currency { set; get; } = null!;
        public float? Balance { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public UnusedBalance WithUnusedBalanceId(string unusedBalanceId) {
            this.UnusedBalanceId = unusedBalanceId;
            return this;
        }
        public UnusedBalance WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }
        public UnusedBalance WithBalance(float? balance) {
            this.Balance = balance;
            return this;
        }
        public UnusedBalance WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public UnusedBalance WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):unused:(?<currency>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):unused:(?<currency>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):unused:(?<currency>.+)",
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

        private static System.Text.RegularExpressions.Regex _currencyRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):unused:(?<currency>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCurrencyFromGrn(
            string grn
        )
        {
            var match = _currencyRegex.Match(grn);
            if (!match.Success || !match.Groups["currency"].Success)
            {
                return null;
            }
            return match.Groups["currency"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnusedBalance FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UnusedBalance()
                .WithUnusedBalanceId(!data.Keys.Contains("unusedBalanceId") || data["unusedBalanceId"] == null ? null : data["unusedBalanceId"].ToString())
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithBalance(!data.Keys.Contains("balance") || data["balance"] == null ? null : (float?)float.Parse(data["balance"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["unusedBalanceId"] = UnusedBalanceId,
                ["currency"] = Currency,
                ["balance"] = Balance,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UnusedBalanceId != null) {
                writer.WritePropertyName("unusedBalanceId");
                writer.Write(UnusedBalanceId.ToString());
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
            }
            if (Balance != null) {
                writer.WritePropertyName("balance");
                writer.Write(float.Parse(Balance.ToString()));
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
            var other = obj as UnusedBalance;
            var diff = 0;
            if (UnusedBalanceId == null && UnusedBalanceId == other.UnusedBalanceId)
            {
                // null and null
            }
            else
            {
                diff += UnusedBalanceId.CompareTo(other.UnusedBalanceId);
            }
            if (Currency == null && Currency == other.Currency)
            {
                // null and null
            }
            else
            {
                diff += Currency.CompareTo(other.Currency);
            }
            if (Balance == null && Balance == other.Balance)
            {
                // null and null
            }
            else
            {
                diff += (int)(Balance - other.Balance);
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
                if (UnusedBalanceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unusedBalance", "money2.unusedBalance.unusedBalanceId.error.tooLong"),
                    });
                }
            }
            {
                if (Currency.Length > 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unusedBalance", "money2.unusedBalance.currency.error.tooLong"),
                    });
                }
            }
            {
                if (Balance < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unusedBalance", "money2.unusedBalance.balance.error.invalid"),
                    });
                }
                if (Balance > 16777214) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unusedBalance", "money2.unusedBalance.balance.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unusedBalance", "money2.unusedBalance.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unusedBalance", "money2.unusedBalance.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unusedBalance", "money2.unusedBalance.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unusedBalance", "money2.unusedBalance.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnusedBalance {
                UnusedBalanceId = UnusedBalanceId,
                Currency = Currency,
                Balance = Balance,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
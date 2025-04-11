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
	public partial class DailyTransactionHistory : IComparable
	{
        public string DailyTransactionHistoryId { set; get; }
        public int? Year { set; get; }
        public int? Month { set; get; }
        public int? Day { set; get; }
        public string Currency { set; get; }
        public double? DepositAmount { set; get; }
        public double? WithdrawAmount { set; get; }
        public long? IssueCount { set; get; }
        public long? ConsumeCount { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public DailyTransactionHistory WithDailyTransactionHistoryId(string dailyTransactionHistoryId) {
            this.DailyTransactionHistoryId = dailyTransactionHistoryId;
            return this;
        }
        public DailyTransactionHistory WithYear(int? year) {
            this.Year = year;
            return this;
        }
        public DailyTransactionHistory WithMonth(int? month) {
            this.Month = month;
            return this;
        }
        public DailyTransactionHistory WithDay(int? day) {
            this.Day = day;
            return this;
        }
        public DailyTransactionHistory WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }
        public DailyTransactionHistory WithDepositAmount(double? depositAmount) {
            this.DepositAmount = depositAmount;
            return this;
        }
        public DailyTransactionHistory WithWithdrawAmount(double? withdrawAmount) {
            this.WithdrawAmount = withdrawAmount;
            return this;
        }
        public DailyTransactionHistory WithIssueCount(long? issueCount) {
            this.IssueCount = issueCount;
            return this;
        }
        public DailyTransactionHistory WithConsumeCount(long? consumeCount) {
            this.ConsumeCount = consumeCount;
            return this;
        }
        public DailyTransactionHistory WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public DailyTransactionHistory WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):transaction:history:daily:(?<year>.+):(?<month>.+):(?<day>.+):currency:(?<currency>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):transaction:history:daily:(?<year>.+):(?<month>.+):(?<day>.+):currency:(?<currency>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):transaction:history:daily:(?<year>.+):(?<month>.+):(?<day>.+):currency:(?<currency>.+)",
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

        private static System.Text.RegularExpressions.Regex _yearRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):transaction:history:daily:(?<year>.+):(?<month>.+):(?<day>.+):currency:(?<currency>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetYearFromGrn(
            string grn
        )
        {
            var match = _yearRegex.Match(grn);
            if (!match.Success || !match.Groups["year"].Success)
            {
                return null;
            }
            return match.Groups["year"].Value;
        }

        private static System.Text.RegularExpressions.Regex _monthRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):transaction:history:daily:(?<year>.+):(?<month>.+):(?<day>.+):currency:(?<currency>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMonthFromGrn(
            string grn
        )
        {
            var match = _monthRegex.Match(grn);
            if (!match.Success || !match.Groups["month"].Success)
            {
                return null;
            }
            return match.Groups["month"].Value;
        }

        private static System.Text.RegularExpressions.Regex _dayRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):transaction:history:daily:(?<year>.+):(?<month>.+):(?<day>.+):currency:(?<currency>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetDayFromGrn(
            string grn
        )
        {
            var match = _dayRegex.Match(grn);
            if (!match.Success || !match.Groups["day"].Success)
            {
                return null;
            }
            return match.Groups["day"].Value;
        }

        private static System.Text.RegularExpressions.Regex _currencyRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):transaction:history:daily:(?<year>.+):(?<month>.+):(?<day>.+):currency:(?<currency>.+)",
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
        public static DailyTransactionHistory FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DailyTransactionHistory()
                .WithDailyTransactionHistoryId(!data.Keys.Contains("dailyTransactionHistoryId") || data["dailyTransactionHistoryId"] == null ? null : data["dailyTransactionHistoryId"].ToString())
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)(data["year"].ToString().Contains(".") ? (int)double.Parse(data["year"].ToString()) : int.Parse(data["year"].ToString())))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)(data["month"].ToString().Contains(".") ? (int)double.Parse(data["month"].ToString()) : int.Parse(data["month"].ToString())))
                .WithDay(!data.Keys.Contains("day") || data["day"] == null ? null : (int?)(data["day"].ToString().Contains(".") ? (int)double.Parse(data["day"].ToString()) : int.Parse(data["day"].ToString())))
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithDepositAmount(!data.Keys.Contains("depositAmount") || data["depositAmount"] == null ? null : (double?)double.Parse(data["depositAmount"].ToString()))
                .WithWithdrawAmount(!data.Keys.Contains("withdrawAmount") || data["withdrawAmount"] == null ? null : (double?)double.Parse(data["withdrawAmount"].ToString()))
                .WithIssueCount(!data.Keys.Contains("issueCount") || data["issueCount"] == null ? null : (long?)(data["issueCount"].ToString().Contains(".") ? (long)double.Parse(data["issueCount"].ToString()) : long.Parse(data["issueCount"].ToString())))
                .WithConsumeCount(!data.Keys.Contains("consumeCount") || data["consumeCount"] == null ? null : (long?)(data["consumeCount"].ToString().Contains(".") ? (long)double.Parse(data["consumeCount"].ToString()) : long.Parse(data["consumeCount"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["dailyTransactionHistoryId"] = DailyTransactionHistoryId,
                ["year"] = Year,
                ["month"] = Month,
                ["day"] = Day,
                ["currency"] = Currency,
                ["depositAmount"] = DepositAmount,
                ["withdrawAmount"] = WithdrawAmount,
                ["issueCount"] = IssueCount,
                ["consumeCount"] = ConsumeCount,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DailyTransactionHistoryId != null) {
                writer.WritePropertyName("dailyTransactionHistoryId");
                writer.Write(DailyTransactionHistoryId.ToString());
            }
            if (Year != null) {
                writer.WritePropertyName("year");
                writer.Write((Year.ToString().Contains(".") ? (int)double.Parse(Year.ToString()) : int.Parse(Year.ToString())));
            }
            if (Month != null) {
                writer.WritePropertyName("month");
                writer.Write((Month.ToString().Contains(".") ? (int)double.Parse(Month.ToString()) : int.Parse(Month.ToString())));
            }
            if (Day != null) {
                writer.WritePropertyName("day");
                writer.Write((Day.ToString().Contains(".") ? (int)double.Parse(Day.ToString()) : int.Parse(Day.ToString())));
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
            }
            if (DepositAmount != null) {
                writer.WritePropertyName("depositAmount");
                writer.Write(double.Parse(DepositAmount.ToString()));
            }
            if (WithdrawAmount != null) {
                writer.WritePropertyName("withdrawAmount");
                writer.Write(double.Parse(WithdrawAmount.ToString()));
            }
            if (IssueCount != null) {
                writer.WritePropertyName("issueCount");
                writer.Write((IssueCount.ToString().Contains(".") ? (long)double.Parse(IssueCount.ToString()) : long.Parse(IssueCount.ToString())));
            }
            if (ConsumeCount != null) {
                writer.WritePropertyName("consumeCount");
                writer.Write((ConsumeCount.ToString().Contains(".") ? (long)double.Parse(ConsumeCount.ToString()) : long.Parse(ConsumeCount.ToString())));
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
            var other = obj as DailyTransactionHistory;
            var diff = 0;
            if (DailyTransactionHistoryId == null && DailyTransactionHistoryId == other.DailyTransactionHistoryId)
            {
                // null and null
            }
            else
            {
                diff += DailyTransactionHistoryId.CompareTo(other.DailyTransactionHistoryId);
            }
            if (Year == null && Year == other.Year)
            {
                // null and null
            }
            else
            {
                diff += (int)(Year - other.Year);
            }
            if (Month == null && Month == other.Month)
            {
                // null and null
            }
            else
            {
                diff += (int)(Month - other.Month);
            }
            if (Day == null && Day == other.Day)
            {
                // null and null
            }
            else
            {
                diff += (int)(Day - other.Day);
            }
            if (Currency == null && Currency == other.Currency)
            {
                // null and null
            }
            else
            {
                diff += Currency.CompareTo(other.Currency);
            }
            if (DepositAmount == null && DepositAmount == other.DepositAmount)
            {
                // null and null
            }
            else
            {
                diff += (int)(DepositAmount - other.DepositAmount);
            }
            if (WithdrawAmount == null && WithdrawAmount == other.WithdrawAmount)
            {
                // null and null
            }
            else
            {
                diff += (int)(WithdrawAmount - other.WithdrawAmount);
            }
            if (IssueCount == null && IssueCount == other.IssueCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(IssueCount - other.IssueCount);
            }
            if (ConsumeCount == null && ConsumeCount == other.ConsumeCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(ConsumeCount - other.ConsumeCount);
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
                if (DailyTransactionHistoryId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.dailyTransactionHistoryId.error.tooLong"),
                    });
                }
            }
            {
                if (Year < 2000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.year.error.invalid"),
                    });
                }
                if (Year > 3000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.year.error.invalid"),
                    });
                }
            }
            {
                if (Month < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.month.error.invalid"),
                    });
                }
                if (Month > 12) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.month.error.invalid"),
                    });
                }
            }
            {
                if (Day < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.day.error.invalid"),
                    });
                }
                if (Day > 31) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.day.error.invalid"),
                    });
                }
            }
            {
                if (Currency.Length > 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.currency.error.tooLong"),
                    });
                }
            }
            {
                if (DepositAmount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.depositAmount.error.invalid"),
                    });
                }
                if (DepositAmount > 281474976710654) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.depositAmount.error.invalid"),
                    });
                }
            }
            {
                if (WithdrawAmount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.withdrawAmount.error.invalid"),
                    });
                }
                if (WithdrawAmount > 281474976710654) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.withdrawAmount.error.invalid"),
                    });
                }
            }
            {
                if (IssueCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.issueCount.error.invalid"),
                    });
                }
                if (IssueCount > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.issueCount.error.invalid"),
                    });
                }
            }
            {
                if (ConsumeCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.consumeCount.error.invalid"),
                    });
                }
                if (ConsumeCount > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.consumeCount.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dailyTransactionHistory", "money2.dailyTransactionHistory.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new DailyTransactionHistory {
                DailyTransactionHistoryId = DailyTransactionHistoryId,
                Year = Year,
                Month = Month,
                Day = Day,
                Currency = Currency,
                DepositAmount = DepositAmount,
                WithdrawAmount = WithdrawAmount,
                IssueCount = IssueCount,
                ConsumeCount = ConsumeCount,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}
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
	public partial class RefundHistory : IComparable
	{
        public string RefundHistoryId { set; get; }
        public string TransactionId { set; get; }
        public int? Year { set; get; }
        public int? Month { set; get; }
        public int? Day { set; get; }
        public string UserId { set; get; }
        public Gs2.Gs2Money2.Model.RefundEvent Detail { set; get; }
        public long? CreatedAt { set; get; }
        public RefundHistory WithRefundHistoryId(string refundHistoryId) {
            this.RefundHistoryId = refundHistoryId;
            return this;
        }
        public RefundHistory WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public RefundHistory WithYear(int? year) {
            this.Year = year;
            return this;
        }
        public RefundHistory WithMonth(int? month) {
            this.Month = month;
            return this;
        }
        public RefundHistory WithDay(int? day) {
            this.Day = day;
            return this;
        }
        public RefundHistory WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public RefundHistory WithDetail(Gs2.Gs2Money2.Model.RefundEvent detail) {
            this.Detail = detail;
            return this;
        }
        public RefundHistory WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):refundHistory:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):refundHistory:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):refundHistory:(?<transactionId>.+)",
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

        private static System.Text.RegularExpressions.Regex _transactionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):refundHistory:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTransactionIdFromGrn(
            string grn
        )
        {
            var match = _transactionIdRegex.Match(grn);
            if (!match.Success || !match.Groups["transactionId"].Success)
            {
                return null;
            }
            return match.Groups["transactionId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RefundHistory FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RefundHistory()
                .WithRefundHistoryId(!data.Keys.Contains("refundHistoryId") || data["refundHistoryId"] == null ? null : data["refundHistoryId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)(data["year"].ToString().Contains(".") ? (int)double.Parse(data["year"].ToString()) : int.Parse(data["year"].ToString())))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)(data["month"].ToString().Contains(".") ? (int)double.Parse(data["month"].ToString()) : int.Parse(data["month"].ToString())))
                .WithDay(!data.Keys.Contains("day") || data["day"] == null ? null : (int?)(data["day"].ToString().Contains(".") ? (int)double.Parse(data["day"].ToString()) : int.Parse(data["day"].ToString())))
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDetail(!data.Keys.Contains("detail") || data["detail"] == null ? null : Gs2.Gs2Money2.Model.RefundEvent.FromJson(data["detail"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["refundHistoryId"] = RefundHistoryId,
                ["transactionId"] = TransactionId,
                ["year"] = Year,
                ["month"] = Month,
                ["day"] = Day,
                ["userId"] = UserId,
                ["detail"] = Detail?.ToJson(),
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RefundHistoryId != null) {
                writer.WritePropertyName("refundHistoryId");
                writer.Write(RefundHistoryId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Detail != null) {
                writer.WritePropertyName("detail");
                Detail.WriteJson(writer);
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RefundHistory;
            var diff = 0;
            if (RefundHistoryId == null && RefundHistoryId == other.RefundHistoryId)
            {
                // null and null
            }
            else
            {
                diff += RefundHistoryId.CompareTo(other.RefundHistoryId);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
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
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Detail == null && Detail == other.Detail)
            {
                // null and null
            }
            else
            {
                diff += Detail.CompareTo(other.Detail);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (RefundHistoryId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.refundHistoryId.error.tooLong"),
                    });
                }
            }
            {
                if (TransactionId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (Year < 2000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.year.error.invalid"),
                    });
                }
                if (Year > 3000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.year.error.invalid"),
                    });
                }
            }
            {
                if (Month < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.month.error.invalid"),
                    });
                }
                if (Month > 12) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.month.error.invalid"),
                    });
                }
            }
            {
                if (Day < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.day.error.invalid"),
                    });
                }
                if (Day > 31) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.day.error.invalid"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.userId.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundHistory", "money2.refundHistory.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RefundHistory {
                RefundHistoryId = RefundHistoryId,
                TransactionId = TransactionId,
                Year = Year,
                Month = Month,
                Day = Day,
                UserId = UserId,
                Detail = Detail?.Clone() as Gs2.Gs2Money2.Model.RefundEvent,
                CreatedAt = CreatedAt,
            };
        }
    }
}
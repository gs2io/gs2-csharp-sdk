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
	public class Receipt : IComparable
	{
        public string ReceiptId { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public string PurchaseToken { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string Type { set; get; } = null!;
        public int? Slot { set; get; } = null!;
        public float? Price { set; get; } = null!;
        public int? Paid { set; get; } = null!;
        public int? Free { set; get; } = null!;
        public int? Total { set; get; } = null!;
        public string ContentsId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Receipt WithReceiptId(string receiptId) {
            this.ReceiptId = receiptId;
            return this;
        }
        public Receipt WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public Receipt WithPurchaseToken(string purchaseToken) {
            this.PurchaseToken = purchaseToken;
            return this;
        }
        public Receipt WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Receipt WithType(string type) {
            this.Type = type;
            return this;
        }
        public Receipt WithSlot(int? slot) {
            this.Slot = slot;
            return this;
        }
        public Receipt WithPrice(float? price) {
            this.Price = price;
            return this;
        }
        public Receipt WithPaid(int? paid) {
            this.Paid = paid;
            return this;
        }
        public Receipt WithFree(int? free) {
            this.Free = free;
            return this;
        }
        public Receipt WithTotal(int? total) {
            this.Total = total;
            return this;
        }
        public Receipt WithContentsId(string contentsId) {
            this.ContentsId = contentsId;
            return this;
        }
        public Receipt WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Receipt WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):receipt:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):receipt:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):receipt:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):receipt:(?<transactionId>.+)",
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

        private static System.Text.RegularExpressions.Regex _transactionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):receipt:(?<transactionId>.+)",
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
        public static Receipt FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Receipt()
                .WithReceiptId(!data.Keys.Contains("receiptId") || data["receiptId"] == null ? null : data["receiptId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithPurchaseToken(!data.Keys.Contains("purchaseToken") || data["purchaseToken"] == null ? null : data["purchaseToken"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)(data["slot"].ToString().Contains(".") ? (int)double.Parse(data["slot"].ToString()) : int.Parse(data["slot"].ToString())))
                .WithPrice(!data.Keys.Contains("price") || data["price"] == null ? null : (float?)float.Parse(data["price"].ToString()))
                .WithPaid(!data.Keys.Contains("paid") || data["paid"] == null ? null : (int?)(data["paid"].ToString().Contains(".") ? (int)double.Parse(data["paid"].ToString()) : int.Parse(data["paid"].ToString())))
                .WithFree(!data.Keys.Contains("free") || data["free"] == null ? null : (int?)(data["free"].ToString().Contains(".") ? (int)double.Parse(data["free"].ToString()) : int.Parse(data["free"].ToString())))
                .WithTotal(!data.Keys.Contains("total") || data["total"] == null ? null : (int?)(data["total"].ToString().Contains(".") ? (int)double.Parse(data["total"].ToString()) : int.Parse(data["total"].ToString())))
                .WithContentsId(!data.Keys.Contains("contentsId") || data["contentsId"] == null ? null : data["contentsId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["receiptId"] = ReceiptId,
                ["transactionId"] = TransactionId,
                ["purchaseToken"] = PurchaseToken,
                ["userId"] = UserId,
                ["type"] = Type,
                ["slot"] = Slot,
                ["price"] = Price,
                ["paid"] = Paid,
                ["free"] = Free,
                ["total"] = Total,
                ["contentsId"] = ContentsId,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ReceiptId != null) {
                writer.WritePropertyName("receiptId");
                writer.Write(ReceiptId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (PurchaseToken != null) {
                writer.WritePropertyName("purchaseToken");
                writer.Write(PurchaseToken.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (Slot != null) {
                writer.WritePropertyName("slot");
                writer.Write((Slot.ToString().Contains(".") ? (int)double.Parse(Slot.ToString()) : int.Parse(Slot.ToString())));
            }
            if (Price != null) {
                writer.WritePropertyName("price");
                writer.Write(float.Parse(Price.ToString()));
            }
            if (Paid != null) {
                writer.WritePropertyName("paid");
                writer.Write((Paid.ToString().Contains(".") ? (int)double.Parse(Paid.ToString()) : int.Parse(Paid.ToString())));
            }
            if (Free != null) {
                writer.WritePropertyName("free");
                writer.Write((Free.ToString().Contains(".") ? (int)double.Parse(Free.ToString()) : int.Parse(Free.ToString())));
            }
            if (Total != null) {
                writer.WritePropertyName("total");
                writer.Write((Total.ToString().Contains(".") ? (int)double.Parse(Total.ToString()) : int.Parse(Total.ToString())));
            }
            if (ContentsId != null) {
                writer.WritePropertyName("contentsId");
                writer.Write(ContentsId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Receipt;
            var diff = 0;
            if (ReceiptId == null && ReceiptId == other.ReceiptId)
            {
                // null and null
            }
            else
            {
                diff += ReceiptId.CompareTo(other.ReceiptId);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            if (PurchaseToken == null && PurchaseToken == other.PurchaseToken)
            {
                // null and null
            }
            else
            {
                diff += PurchaseToken.CompareTo(other.PurchaseToken);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += Type.CompareTo(other.Type);
            }
            if (Slot == null && Slot == other.Slot)
            {
                // null and null
            }
            else
            {
                diff += (int)(Slot - other.Slot);
            }
            if (Price == null && Price == other.Price)
            {
                // null and null
            }
            else
            {
                diff += (int)(Price - other.Price);
            }
            if (Paid == null && Paid == other.Paid)
            {
                // null and null
            }
            else
            {
                diff += (int)(Paid - other.Paid);
            }
            if (Free == null && Free == other.Free)
            {
                // null and null
            }
            else
            {
                diff += (int)(Free - other.Free);
            }
            if (Total == null && Total == other.Total)
            {
                // null and null
            }
            else
            {
                diff += (int)(Total - other.Total);
            }
            if (ContentsId == null && ContentsId == other.ContentsId)
            {
                // null and null
            }
            else
            {
                diff += ContentsId.CompareTo(other.ContentsId);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
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
                if (ReceiptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.receiptId.error.tooLong"),
                    });
                }
            }
            {
                if (TransactionId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (PurchaseToken.Length > 4096) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.purchaseToken.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.userId.error.tooLong"),
                    });
                }
            }
            {
                switch (Type) {
                    case "purchase":
                    case "deposit":
                    case "withdraw":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("receipt", "money.receipt.type.error.invalid"),
                        });
                }
            }
            if (Type != "purchase") {
                if (Slot < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.slot.error.invalid"),
                    });
                }
                if (Slot > 100000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.slot.error.invalid"),
                    });
                }
            }
            if (Type != "purchase") {
                if (Price < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.price.error.invalid"),
                    });
                }
                if (Price > 100000.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.price.error.invalid"),
                    });
                }
            }
            if (Type != "purchase") {
                if (Paid < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.paid.error.invalid"),
                    });
                }
                if (Paid > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.paid.error.invalid"),
                    });
                }
            }
            if (Type != "purchase") {
                if (Free < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.free.error.invalid"),
                    });
                }
                if (Free > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.free.error.invalid"),
                    });
                }
            }
            if (Type != "purchase") {
                if (Total < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.total.error.invalid"),
                    });
                }
                if (Total > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.total.error.invalid"),
                    });
                }
            }
            {
                if (ContentsId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.contentsId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money.receipt.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Receipt {
                ReceiptId = ReceiptId,
                TransactionId = TransactionId,
                PurchaseToken = PurchaseToken,
                UserId = UserId,
                Type = Type,
                Slot = Slot,
                Price = Price,
                Paid = Paid,
                Free = Free,
                Total = Total,
                ContentsId = ContentsId,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}
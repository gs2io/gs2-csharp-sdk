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
        public string ReceiptId { set; get; }
        public string TransactionId { set; get; }
        public string PurchaseToken { set; get; }
        public string UserId { set; get; }
        public string Type { set; get; }
        public int? Slot { set; get; }
        public float? Price { set; get; }
        public int? Paid { set; get; }
        public int? Free { set; get; }
        public int? Total { set; get; }
        public string ContentsId { set; get; }
        public long? CreatedAt { set; get; }
        public long? Revision { set; get; }
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
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)int.Parse(data["slot"].ToString()))
                .WithPrice(!data.Keys.Contains("price") || data["price"] == null ? null : (float?)float.Parse(data["price"].ToString()))
                .WithPaid(!data.Keys.Contains("paid") || data["paid"] == null ? null : (int?)int.Parse(data["paid"].ToString()))
                .WithFree(!data.Keys.Contains("free") || data["free"] == null ? null : (int?)int.Parse(data["free"].ToString()))
                .WithTotal(!data.Keys.Contains("total") || data["total"] == null ? null : (int?)int.Parse(data["total"].ToString()))
                .WithContentsId(!data.Keys.Contains("contentsId") || data["contentsId"] == null ? null : data["contentsId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)long.Parse(data["revision"].ToString()));
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
                writer.Write(int.Parse(Slot.ToString()));
            }
            if (Price != null) {
                writer.WritePropertyName("price");
                writer.Write(float.Parse(Price.ToString()));
            }
            if (Paid != null) {
                writer.WritePropertyName("paid");
                writer.Write(int.Parse(Paid.ToString()));
            }
            if (Free != null) {
                writer.WritePropertyName("free");
                writer.Write(int.Parse(Free.ToString()));
            }
            if (Total != null) {
                writer.WritePropertyName("total");
                writer.Write(int.Parse(Total.ToString()));
            }
            if (ContentsId != null) {
                writer.WritePropertyName("contentsId");
                writer.Write(ContentsId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write(long.Parse(Revision.ToString()));
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
    }
}
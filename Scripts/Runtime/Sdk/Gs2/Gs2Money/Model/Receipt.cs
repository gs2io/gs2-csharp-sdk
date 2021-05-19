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
using UnityEngine.Scripting;

namespace Gs2.Gs2Money.Model
{
	[Preserve]
	public class Receipt : IComparable
	{

        /** レシート */
        public string receiptId { set; get; }

        /**
         * レシートを設定
         *
         * @param receiptId レシート
         * @return this
         */
        public Receipt WithReceiptId(string receiptId) {
            this.receiptId = receiptId;
            return this;
        }

        /** トランザクションID */
        public string transactionId { set; get; }

        /**
         * トランザクションIDを設定
         *
         * @param transactionId トランザクションID
         * @return this
         */
        public Receipt WithTransactionId(string transactionId) {
            this.transactionId = transactionId;
            return this;
        }

        /** ユーザーID */
        public string userId { set; get; }

        /**
         * ユーザーIDを設定
         *
         * @param userId ユーザーID
         * @return this
         */
        public Receipt WithUserId(string userId) {
            this.userId = userId;
            return this;
        }

        /** 種類 */
        public string type { set; get; }

        /**
         * 種類を設定
         *
         * @param type 種類
         * @return this
         */
        public Receipt WithType(string type) {
            this.type = type;
            return this;
        }

        /** スロット番号 */
        public int? slot { set; get; }

        /**
         * スロット番号を設定
         *
         * @param slot スロット番号
         * @return this
         */
        public Receipt WithSlot(int? slot) {
            this.slot = slot;
            return this;
        }

        /** 単価 */
        public float? price { set; get; }

        /**
         * 単価を設定
         *
         * @param price 単価
         * @return this
         */
        public Receipt WithPrice(float? price) {
            this.price = price;
            return this;
        }

        /** 有償課金通貨 */
        public int? paid { set; get; }

        /**
         * 有償課金通貨を設定
         *
         * @param paid 有償課金通貨
         * @return this
         */
        public Receipt WithPaid(int? paid) {
            this.paid = paid;
            return this;
        }

        /** 無償課金通貨 */
        public int? free { set; get; }

        /**
         * 無償課金通貨を設定
         *
         * @param free 無償課金通貨
         * @return this
         */
        public Receipt WithFree(int? free) {
            this.free = free;
            return this;
        }

        /** 総数 */
        public int? total { set; get; }

        /**
         * 総数を設定
         *
         * @param total 総数
         * @return this
         */
        public Receipt WithTotal(int? total) {
            this.total = total;
            return this;
        }

        /** ストアプラットフォームで販売されているコンテンツID */
        public string contentsId { set; get; }

        /**
         * ストアプラットフォームで販売されているコンテンツIDを設定
         *
         * @param contentsId ストアプラットフォームで販売されているコンテンツID
         * @return this
         */
        public Receipt WithContentsId(string contentsId) {
            this.contentsId = contentsId;
            return this;
        }

        /** 作成日時 */
        public long? createdAt { set; get; }

        /**
         * 作成日時を設定
         *
         * @param createdAt 作成日時
         * @return this
         */
        public Receipt WithCreatedAt(long? createdAt) {
            this.createdAt = createdAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.receiptId != null)
            {
                writer.WritePropertyName("receiptId");
                writer.Write(this.receiptId);
            }
            if(this.transactionId != null)
            {
                writer.WritePropertyName("transactionId");
                writer.Write(this.transactionId);
            }
            if(this.userId != null)
            {
                writer.WritePropertyName("userId");
                writer.Write(this.userId);
            }
            if(this.type != null)
            {
                writer.WritePropertyName("type");
                writer.Write(this.type);
            }
            if(this.slot.HasValue)
            {
                writer.WritePropertyName("slot");
                writer.Write(this.slot.Value);
            }
            if(this.price.HasValue)
            {
                writer.WritePropertyName("price");
                writer.Write(this.price.Value);
            }
            if(this.paid.HasValue)
            {
                writer.WritePropertyName("paid");
                writer.Write(this.paid.Value);
            }
            if(this.free.HasValue)
            {
                writer.WritePropertyName("free");
                writer.Write(this.free.Value);
            }
            if(this.total.HasValue)
            {
                writer.WritePropertyName("total");
                writer.Write(this.total.Value);
            }
            if(this.contentsId != null)
            {
                writer.WritePropertyName("contentsId");
                writer.Write(this.contentsId);
            }
            if(this.createdAt.HasValue)
            {
                writer.WritePropertyName("createdAt");
                writer.Write(this.createdAt.Value);
            }
            writer.WriteObjectEnd();
        }

    public static string GetTransactionIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):money:(?<namespaceName>.*):user:(?<userId>.*):receipt:(?<transactionId>.*)");
        if (!match.Groups["transactionId"].Success)
        {
            return null;
        }
        return match.Groups["transactionId"].Value;
    }

    public static string GetUserIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):money:(?<namespaceName>.*):user:(?<userId>.*):receipt:(?<transactionId>.*)");
        if (!match.Groups["userId"].Success)
        {
            return null;
        }
        return match.Groups["userId"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):money:(?<namespaceName>.*):user:(?<userId>.*):receipt:(?<transactionId>.*)");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):money:(?<namespaceName>.*):user:(?<userId>.*):receipt:(?<transactionId>.*)");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):money:(?<namespaceName>.*):user:(?<userId>.*):receipt:(?<transactionId>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static Receipt FromDict(JsonData data)
        {
            return new Receipt()
                .WithReceiptId(data.Keys.Contains("receiptId") && data["receiptId"] != null ? data["receiptId"].ToString() : null)
                .WithTransactionId(data.Keys.Contains("transactionId") && data["transactionId"] != null ? data["transactionId"].ToString() : null)
                .WithUserId(data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString() : null)
                .WithType(data.Keys.Contains("type") && data["type"] != null ? data["type"].ToString() : null)
                .WithSlot(data.Keys.Contains("slot") && data["slot"] != null ? (int?)int.Parse(data["slot"].ToString()) : null)
                .WithPrice(data.Keys.Contains("price") && data["price"] != null ? (float?)float.Parse(data["price"].ToString()) : null)
                .WithPaid(data.Keys.Contains("paid") && data["paid"] != null ? (int?)int.Parse(data["paid"].ToString()) : null)
                .WithFree(data.Keys.Contains("free") && data["free"] != null ? (int?)int.Parse(data["free"].ToString()) : null)
                .WithTotal(data.Keys.Contains("total") && data["total"] != null ? (int?)int.Parse(data["total"].ToString()) : null)
                .WithContentsId(data.Keys.Contains("contentsId") && data["contentsId"] != null ? data["contentsId"].ToString() : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Receipt;
            var diff = 0;
            if (receiptId == null && receiptId == other.receiptId)
            {
                // null and null
            }
            else
            {
                diff += receiptId.CompareTo(other.receiptId);
            }
            if (transactionId == null && transactionId == other.transactionId)
            {
                // null and null
            }
            else
            {
                diff += transactionId.CompareTo(other.transactionId);
            }
            if (userId == null && userId == other.userId)
            {
                // null and null
            }
            else
            {
                diff += userId.CompareTo(other.userId);
            }
            if (type == null && type == other.type)
            {
                // null and null
            }
            else
            {
                diff += type.CompareTo(other.type);
            }
            if (slot == null && slot == other.slot)
            {
                // null and null
            }
            else
            {
                diff += (int)(slot - other.slot);
            }
            if (price == null && price == other.price)
            {
                // null and null
            }
            else
            {
                diff += (int)(price - other.price);
            }
            if (paid == null && paid == other.paid)
            {
                // null and null
            }
            else
            {
                diff += (int)(paid - other.paid);
            }
            if (free == null && free == other.free)
            {
                // null and null
            }
            else
            {
                diff += (int)(free - other.free);
            }
            if (total == null && total == other.total)
            {
                // null and null
            }
            else
            {
                diff += (int)(total - other.total);
            }
            if (contentsId == null && contentsId == other.contentsId)
            {
                // null and null
            }
            else
            {
                diff += contentsId.CompareTo(other.contentsId);
            }
            if (createdAt == null && createdAt == other.createdAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(createdAt - other.createdAt);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["receiptId"] = receiptId;
            data["transactionId"] = transactionId;
            data["userId"] = userId;
            data["type"] = type;
            data["slot"] = slot;
            data["price"] = price;
            data["paid"] = paid;
            data["free"] = free;
            data["total"] = total;
            data["contentsId"] = contentsId;
            data["createdAt"] = createdAt;
            return data;
        }
	}
}
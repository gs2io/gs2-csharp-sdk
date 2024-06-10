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

namespace Gs2.Gs2Project.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Receipt : IComparable
	{
        public string ReceiptId { set; get; } = null!;
        public string AccountName { set; get; } = null!;
        public string Name { set; get; } = null!;
        public long? Date { set; get; } = null!;
        public string Amount { set; get; } = null!;
        public string PdfUrl { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public Receipt WithReceiptId(string receiptId) {
            this.ReceiptId = receiptId;
            return this;
        }
        public Receipt WithAccountName(string accountName) {
            this.AccountName = accountName;
            return this;
        }
        public Receipt WithName(string name) {
            this.Name = name;
            return this;
        }
        public Receipt WithDate(long? date) {
            this.Date = date;
            return this;
        }
        public Receipt WithAmount(string amount) {
            this.Amount = amount;
            return this;
        }
        public Receipt WithPdfUrl(string pdfUrl) {
            this.PdfUrl = pdfUrl;
            return this;
        }
        public Receipt WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Receipt WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _accountNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):receipt:(?<receiptName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAccountNameFromGrn(
            string grn
        )
        {
            var match = _accountNameRegex.Match(grn);
            if (!match.Success || !match.Groups["accountName"].Success)
            {
                return null;
            }
            return match.Groups["accountName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _receiptNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):receipt:(?<receiptName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetReceiptNameFromGrn(
            string grn
        )
        {
            var match = _receiptNameRegex.Match(grn);
            if (!match.Success || !match.Groups["receiptName"].Success)
            {
                return null;
            }
            return match.Groups["receiptName"].Value;
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
                .WithAccountName(!data.Keys.Contains("accountName") || data["accountName"] == null ? null : data["accountName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDate(!data.Keys.Contains("date") || data["date"] == null ? null : (long?)(data["date"].ToString().Contains(".") ? (long)double.Parse(data["date"].ToString()) : long.Parse(data["date"].ToString())))
                .WithAmount(!data.Keys.Contains("amount") || data["amount"] == null ? null : data["amount"].ToString())
                .WithPdfUrl(!data.Keys.Contains("pdfUrl") || data["pdfUrl"] == null ? null : data["pdfUrl"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["receiptId"] = ReceiptId,
                ["accountName"] = AccountName,
                ["name"] = Name,
                ["date"] = Date,
                ["amount"] = Amount,
                ["pdfUrl"] = PdfUrl,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ReceiptId != null) {
                writer.WritePropertyName("receiptId");
                writer.Write(ReceiptId.ToString());
            }
            if (AccountName != null) {
                writer.WritePropertyName("accountName");
                writer.Write(AccountName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Date != null) {
                writer.WritePropertyName("date");
                writer.Write((Date.ToString().Contains(".") ? (long)double.Parse(Date.ToString()) : long.Parse(Date.ToString())));
            }
            if (Amount != null) {
                writer.WritePropertyName("amount");
                writer.Write(Amount.ToString());
            }
            if (PdfUrl != null) {
                writer.WritePropertyName("pdfUrl");
                writer.Write(PdfUrl.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
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
            if (AccountName == null && AccountName == other.AccountName)
            {
                // null and null
            }
            else
            {
                diff += AccountName.CompareTo(other.AccountName);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Date == null && Date == other.Date)
            {
                // null and null
            }
            else
            {
                diff += (int)(Date - other.Date);
            }
            if (Amount == null && Amount == other.Amount)
            {
                // null and null
            }
            else
            {
                diff += Amount.CompareTo(other.Amount);
            }
            if (PdfUrl == null && PdfUrl == other.PdfUrl)
            {
                // null and null
            }
            else
            {
                diff += PdfUrl.CompareTo(other.PdfUrl);
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
            return diff;
        }

        public void Validate() {
            {
                if (ReceiptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.receiptId.error.tooLong"),
                    });
                }
            }
            {
                if (AccountName.Length < 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.accountName.error.tooShort"),
                    });
                }
                if (AccountName.Length > 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.accountName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.name.error.tooLong"),
                    });
                }
            }
            {
                if (Date < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.date.error.invalid"),
                    });
                }
                if (Date > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.date.error.invalid"),
                    });
                }
            }
            {
                if (Amount.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.amount.error.tooLong"),
                    });
                }
            }
            {
                if (PdfUrl.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.pdfUrl.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "project.receipt.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Receipt {
                ReceiptId = ReceiptId,
                AccountName = AccountName,
                Name = Name,
                Date = Date,
                Amount = Amount,
                PdfUrl = PdfUrl,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}
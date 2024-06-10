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
	public class BillingMethod : IComparable
	{
        public string BillingMethodId { set; get; } = null!;
        public string AccountName { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string MethodType { set; get; } = null!;
        public string CardSignatureName { set; get; } = null!;
        public string CardBrand { set; get; } = null!;
        public string CardLast4 { set; get; } = null!;
        public string PartnerId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public BillingMethod WithBillingMethodId(string billingMethodId) {
            this.BillingMethodId = billingMethodId;
            return this;
        }
        public BillingMethod WithAccountName(string accountName) {
            this.AccountName = accountName;
            return this;
        }
        public BillingMethod WithName(string name) {
            this.Name = name;
            return this;
        }
        public BillingMethod WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public BillingMethod WithMethodType(string methodType) {
            this.MethodType = methodType;
            return this;
        }
        public BillingMethod WithCardSignatureName(string cardSignatureName) {
            this.CardSignatureName = cardSignatureName;
            return this;
        }
        public BillingMethod WithCardBrand(string cardBrand) {
            this.CardBrand = cardBrand;
            return this;
        }
        public BillingMethod WithCardLast4(string cardLast4) {
            this.CardLast4 = cardLast4;
            return this;
        }
        public BillingMethod WithPartnerId(string partnerId) {
            this.PartnerId = partnerId;
            return this;
        }
        public BillingMethod WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public BillingMethod WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _accountNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):billingMethod:(?<billingMethodName>.+)",
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

        private static System.Text.RegularExpressions.Regex _billingMethodNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):billingMethod:(?<billingMethodName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetBillingMethodNameFromGrn(
            string grn
        )
        {
            var match = _billingMethodNameRegex.Match(grn);
            if (!match.Success || !match.Groups["billingMethodName"].Success)
            {
                return null;
            }
            return match.Groups["billingMethodName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BillingMethod FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BillingMethod()
                .WithBillingMethodId(!data.Keys.Contains("billingMethodId") || data["billingMethodId"] == null ? null : data["billingMethodId"].ToString())
                .WithAccountName(!data.Keys.Contains("accountName") || data["accountName"] == null ? null : data["accountName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMethodType(!data.Keys.Contains("methodType") || data["methodType"] == null ? null : data["methodType"].ToString())
                .WithCardSignatureName(!data.Keys.Contains("cardSignatureName") || data["cardSignatureName"] == null ? null : data["cardSignatureName"].ToString())
                .WithCardBrand(!data.Keys.Contains("cardBrand") || data["cardBrand"] == null ? null : data["cardBrand"].ToString())
                .WithCardLast4(!data.Keys.Contains("cardLast4") || data["cardLast4"] == null ? null : data["cardLast4"].ToString())
                .WithPartnerId(!data.Keys.Contains("partnerId") || data["partnerId"] == null ? null : data["partnerId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["billingMethodId"] = BillingMethodId,
                ["accountName"] = AccountName,
                ["name"] = Name,
                ["description"] = Description,
                ["methodType"] = MethodType,
                ["cardSignatureName"] = CardSignatureName,
                ["cardBrand"] = CardBrand,
                ["cardLast4"] = CardLast4,
                ["partnerId"] = PartnerId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BillingMethodId != null) {
                writer.WritePropertyName("billingMethodId");
                writer.Write(BillingMethodId.ToString());
            }
            if (AccountName != null) {
                writer.WritePropertyName("accountName");
                writer.Write(AccountName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (MethodType != null) {
                writer.WritePropertyName("methodType");
                writer.Write(MethodType.ToString());
            }
            if (CardSignatureName != null) {
                writer.WritePropertyName("cardSignatureName");
                writer.Write(CardSignatureName.ToString());
            }
            if (CardBrand != null) {
                writer.WritePropertyName("cardBrand");
                writer.Write(CardBrand.ToString());
            }
            if (CardLast4 != null) {
                writer.WritePropertyName("cardLast4");
                writer.Write(CardLast4.ToString());
            }
            if (PartnerId != null) {
                writer.WritePropertyName("partnerId");
                writer.Write(PartnerId.ToString());
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
            var other = obj as BillingMethod;
            var diff = 0;
            if (BillingMethodId == null && BillingMethodId == other.BillingMethodId)
            {
                // null and null
            }
            else
            {
                diff += BillingMethodId.CompareTo(other.BillingMethodId);
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
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (MethodType == null && MethodType == other.MethodType)
            {
                // null and null
            }
            else
            {
                diff += MethodType.CompareTo(other.MethodType);
            }
            if (CardSignatureName == null && CardSignatureName == other.CardSignatureName)
            {
                // null and null
            }
            else
            {
                diff += CardSignatureName.CompareTo(other.CardSignatureName);
            }
            if (CardBrand == null && CardBrand == other.CardBrand)
            {
                // null and null
            }
            else
            {
                diff += CardBrand.CompareTo(other.CardBrand);
            }
            if (CardLast4 == null && CardLast4 == other.CardLast4)
            {
                // null and null
            }
            else
            {
                diff += CardLast4.CompareTo(other.CardLast4);
            }
            if (PartnerId == null && PartnerId == other.PartnerId)
            {
                // null and null
            }
            else
            {
                diff += PartnerId.CompareTo(other.PartnerId);
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
                if (BillingMethodId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.billingMethodId.error.tooLong"),
                    });
                }
            }
            {
                if (AccountName.Length < 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.accountName.error.tooShort"),
                    });
                }
                if (AccountName.Length > 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.accountName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.description.error.tooLong"),
                    });
                }
            }
            {
                switch (MethodType) {
                    case "credit_card":
                    case "invoice":
                    case "partner":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("billingMethod", "project.billingMethod.methodType.error.invalid"),
                        });
                }
            }
            if (MethodType == "credit_card") {
                if (CardSignatureName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.cardSignatureName.error.tooLong"),
                    });
                }
            }
            if (MethodType == "credit_card") {
                if (CardBrand.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.cardBrand.error.tooLong"),
                    });
                }
            }
            if (MethodType == "credit_card") {
                if (CardLast4.Length < 4) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.cardLast4.error.tooShort"),
                    });
                }
                if (CardLast4.Length > 4) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.cardLast4.error.tooLong"),
                    });
                }
            }
            if (MethodType == "partner") {
                if (PartnerId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.partnerId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("billingMethod", "project.billingMethod.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BillingMethod {
                BillingMethodId = BillingMethodId,
                AccountName = AccountName,
                Name = Name,
                Description = Description,
                MethodType = MethodType,
                CardSignatureName = CardSignatureName,
                CardBrand = CardBrand,
                CardLast4 = CardLast4,
                PartnerId = PartnerId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}
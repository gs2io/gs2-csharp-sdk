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
	public class DepositTransaction : IComparable
	{
        public float? Price { set; get; } = null!;
        public string Currency { set; get; } = null!;
        public int? Count { set; get; } = null!;
        public long? DepositedAt { set; get; } = null!;
        public DepositTransaction WithPrice(float? price) {
            this.Price = price;
            return this;
        }
        public DepositTransaction WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }
        public DepositTransaction WithCount(int? count) {
            this.Count = count;
            return this;
        }
        public DepositTransaction WithDepositedAt(long? depositedAt) {
            this.DepositedAt = depositedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DepositTransaction FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DepositTransaction()
                .WithPrice(!data.Keys.Contains("price") || data["price"] == null ? null : (float?)float.Parse(data["price"].ToString()))
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())))
                .WithDepositedAt(!data.Keys.Contains("depositedAt") || data["depositedAt"] == null ? null : (long?)(data["depositedAt"].ToString().Contains(".") ? (long)double.Parse(data["depositedAt"].ToString()) : long.Parse(data["depositedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["price"] = Price,
                ["currency"] = Currency,
                ["count"] = Count,
                ["depositedAt"] = DepositedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Price != null) {
                writer.WritePropertyName("price");
                writer.Write(float.Parse(Price.ToString()));
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            if (DepositedAt != null) {
                writer.WritePropertyName("depositedAt");
                writer.Write((DepositedAt.ToString().Contains(".") ? (long)double.Parse(DepositedAt.ToString()) : long.Parse(DepositedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DepositTransaction;
            var diff = 0;
            if (Price == null && Price == other.Price)
            {
                // null and null
            }
            else
            {
                diff += (int)(Price - other.Price);
            }
            if (Currency == null && Currency == other.Currency)
            {
                // null and null
            }
            else
            {
                diff += Currency.CompareTo(other.Currency);
            }
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            if (DepositedAt == null && DepositedAt == other.DepositedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(DepositedAt - other.DepositedAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Price < 0.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositTransaction", "money2.depositTransaction.price.error.invalid"),
                    });
                }
                if (Price > 100000.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositTransaction", "money2.depositTransaction.price.error.invalid"),
                    });
                }
            }
            if (Price > 0) {
                if (Currency.Length > 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositTransaction", "money2.depositTransaction.currency.error.tooLong"),
                    });
                }
            }
            {
                if (Count < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositTransaction", "money2.depositTransaction.count.error.invalid"),
                    });
                }
                if (Count > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositTransaction", "money2.depositTransaction.count.error.invalid"),
                    });
                }
            }
            {
                if (DepositedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositTransaction", "money2.depositTransaction.depositedAt.error.invalid"),
                    });
                }
                if (DepositedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositTransaction", "money2.depositTransaction.depositedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new DepositTransaction {
                Price = Price,
                Currency = Currency,
                Count = Count,
                DepositedAt = DepositedAt,
            };
        }
    }
}
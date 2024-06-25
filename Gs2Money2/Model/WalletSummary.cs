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
	public class WalletSummary : IComparable
	{
        public int? Paid { set; get; } = null!;
        public int? Free { set; get; } = null!;
        public int? Total { set; get; } = null!;
        public WalletSummary WithPaid(int? paid) {
            this.Paid = paid;
            return this;
        }
        public WalletSummary WithFree(int? free) {
            this.Free = free;
            return this;
        }
        public WalletSummary WithTotal(int? total) {
            this.Total = total;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WalletSummary FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WalletSummary()
                .WithPaid(!data.Keys.Contains("paid") || data["paid"] == null ? null : (int?)(data["paid"].ToString().Contains(".") ? (int)double.Parse(data["paid"].ToString()) : int.Parse(data["paid"].ToString())))
                .WithFree(!data.Keys.Contains("free") || data["free"] == null ? null : (int?)(data["free"].ToString().Contains(".") ? (int)double.Parse(data["free"].ToString()) : int.Parse(data["free"].ToString())))
                .WithTotal(!data.Keys.Contains("total") || data["total"] == null ? null : (int?)(data["total"].ToString().Contains(".") ? (int)double.Parse(data["total"].ToString()) : int.Parse(data["total"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["paid"] = Paid,
                ["free"] = Free,
                ["total"] = Total,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as WalletSummary;
            var diff = 0;
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
            return diff;
        }

        public void Validate() {
            {
                if (Paid < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("walletSummary", "money2.walletSummary.paid.error.invalid"),
                    });
                }
                if (Paid > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("walletSummary", "money2.walletSummary.paid.error.invalid"),
                    });
                }
            }
            {
                if (Free < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("walletSummary", "money2.walletSummary.free.error.invalid"),
                    });
                }
                if (Free > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("walletSummary", "money2.walletSummary.free.error.invalid"),
                    });
                }
            }
            {
                if (Total < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("walletSummary", "money2.walletSummary.total.error.invalid"),
                    });
                }
                if (Total > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("walletSummary", "money2.walletSummary.total.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new WalletSummary {
                Paid = Paid,
                Free = Free,
                Total = Total,
            };
        }
    }
}
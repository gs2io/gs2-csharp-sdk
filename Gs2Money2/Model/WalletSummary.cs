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
	public partial class WalletSummary : IComparable
	{
        public int? Paid { set; get; }
        public int? Free { set; get; }
        public int? Total { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new WalletSummary()
                .WithPaid(!data.Keys.Contains("paid") || data["paid"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["paid"].ToString()))
                .WithFree(!data.Keys.Contains("free") || data["free"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["free"].ToString()))
                .WithTotal(!data.Keys.Contains("total") || data["total"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["total"].ToString()));
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type WalletSummary.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Paid, other.Paid);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Free, other.Free);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Total, other.Total);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
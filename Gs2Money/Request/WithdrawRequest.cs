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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Money.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class WithdrawRequest : Gs2Request<WithdrawRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public int? Slot { set; get; } = null!;
         public int? Count { set; get; } = null!;
         public bool? PaidOnly { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public WithdrawRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public WithdrawRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public WithdrawRequest WithSlot(int? slot) {
            this.Slot = slot;
            return this;
        }
        public WithdrawRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }
        public WithdrawRequest WithPaidOnly(bool? paidOnly) {
            this.PaidOnly = paidOnly;
            return this;
        }

        public WithdrawRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WithdrawRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WithdrawRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)(data["slot"].ToString().Contains(".") ? (int)double.Parse(data["slot"].ToString()) : int.Parse(data["slot"].ToString())))
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())))
                .WithPaidOnly(!data.Keys.Contains("paidOnly") || data["paidOnly"] == null ? null : (bool?)bool.Parse(data["paidOnly"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["slot"] = Slot,
                ["count"] = Count,
                ["paidOnly"] = PaidOnly,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (Slot != null) {
                writer.WritePropertyName("slot");
                writer.Write((Slot.ToString().Contains(".") ? (int)double.Parse(Slot.ToString()) : int.Parse(Slot.ToString())));
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            if (PaidOnly != null) {
                writer.WritePropertyName("paidOnly");
                writer.Write(bool.Parse(PaidOnly.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += Slot + ":";
            key += Count + ":";
            key += PaidOnly + ":";
            return key;
        }
    }
}
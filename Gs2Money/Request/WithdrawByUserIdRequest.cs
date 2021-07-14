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
using UnityEngine.Scripting;

namespace Gs2.Gs2Money.Request
{
	[Preserve]
	[System.Serializable]
	public class WithdrawByUserIdRequest : Gs2Request<WithdrawByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public int? Slot { set; get; }
        public int? Count { set; get; }
        public bool? PaidOnly { set; get; }

        public WithdrawByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public WithdrawByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public WithdrawByUserIdRequest WithSlot(int? slot) {
            this.Slot = slot;
            return this;
        }

        public WithdrawByUserIdRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public WithdrawByUserIdRequest WithPaidOnly(bool? paidOnly) {
            this.PaidOnly = paidOnly;
            return this;
        }

    	[Preserve]
        public static WithdrawByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WithdrawByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)int.Parse(data["slot"].ToString()))
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)int.Parse(data["count"].ToString()))
                .WithPaidOnly(!data.Keys.Contains("paidOnly") || data["paidOnly"] == null ? null : (bool?)bool.Parse(data["paidOnly"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Slot != null) {
                writer.WritePropertyName("slot");
                writer.Write(int.Parse(Slot.ToString()));
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write(int.Parse(Count.ToString()));
            }
            if (PaidOnly != null) {
                writer.WritePropertyName("paidOnly");
                writer.Write(bool.Parse(PaidOnly.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}
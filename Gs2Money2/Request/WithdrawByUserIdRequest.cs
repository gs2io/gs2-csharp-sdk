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
using Gs2.Gs2Money2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class WithdrawByUserIdRequest : Gs2Request<WithdrawByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public int? Slot { set; get; } = null!;
         public int? WithdrawCount { set; get; } = null!;
         public bool? PaidOnly { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
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
        public WithdrawByUserIdRequest WithWithdrawCount(int? withdrawCount) {
            this.WithdrawCount = withdrawCount;
            return this;
        }
        public WithdrawByUserIdRequest WithPaidOnly(bool? paidOnly) {
            this.PaidOnly = paidOnly;
            return this;
        }
        public WithdrawByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public WithdrawByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WithdrawByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WithdrawByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)(data["slot"].ToString().Contains(".") ? (int)double.Parse(data["slot"].ToString()) : int.Parse(data["slot"].ToString())))
                .WithWithdrawCount(!data.Keys.Contains("withdrawCount") || data["withdrawCount"] == null ? null : (int?)(data["withdrawCount"].ToString().Contains(".") ? (int)double.Parse(data["withdrawCount"].ToString()) : int.Parse(data["withdrawCount"].ToString())))
                .WithPaidOnly(!data.Keys.Contains("paidOnly") || data["paidOnly"] == null ? null : (bool?)bool.Parse(data["paidOnly"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["slot"] = Slot,
                ["withdrawCount"] = WithdrawCount,
                ["paidOnly"] = PaidOnly,
                ["timeOffsetToken"] = TimeOffsetToken,
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
                writer.Write((Slot.ToString().Contains(".") ? (int)double.Parse(Slot.ToString()) : int.Parse(Slot.ToString())));
            }
            if (WithdrawCount != null) {
                writer.WritePropertyName("withdrawCount");
                writer.Write((WithdrawCount.ToString().Contains(".") ? (int)double.Parse(WithdrawCount.ToString()) : int.Parse(WithdrawCount.ToString())));
            }
            if (PaidOnly != null) {
                writer.WritePropertyName("paidOnly");
                writer.Write(bool.Parse(PaidOnly.ToString()));
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += Slot + ":";
            key += WithdrawCount + ":";
            key += PaidOnly + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}
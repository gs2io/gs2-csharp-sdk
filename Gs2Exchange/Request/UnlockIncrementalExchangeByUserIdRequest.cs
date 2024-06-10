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
using Gs2.Gs2Exchange.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Exchange.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UnlockIncrementalExchangeByUserIdRequest : Gs2Request<UnlockIncrementalExchangeByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RateName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string LockTransactionId { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public UnlockIncrementalExchangeByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UnlockIncrementalExchangeByUserIdRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public UnlockIncrementalExchangeByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public UnlockIncrementalExchangeByUserIdRequest WithLockTransactionId(string lockTransactionId) {
            this.LockTransactionId = lockTransactionId;
            return this;
        }
        public UnlockIncrementalExchangeByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public UnlockIncrementalExchangeByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnlockIncrementalExchangeByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UnlockIncrementalExchangeByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithLockTransactionId(!data.Keys.Contains("lockTransactionId") || data["lockTransactionId"] == null ? null : data["lockTransactionId"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["rateName"] = RateName,
                ["userId"] = UserId,
                ["lockTransactionId"] = LockTransactionId,
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
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (LockTransactionId != null) {
                writer.WritePropertyName("lockTransactionId");
                writer.Write(LockTransactionId.ToString());
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
            key += RateName + ":";
            key += UserId + ":";
            key += LockTransactionId + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}
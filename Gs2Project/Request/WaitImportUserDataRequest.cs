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
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Project.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class WaitImportUserDataRequest : Gs2Request<WaitImportUserDataRequest>
	{
         public string TransactionId { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string MicroserviceName { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public WaitImportUserDataRequest WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public WaitImportUserDataRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public WaitImportUserDataRequest WithMicroserviceName(string microserviceName) {
            this.MicroserviceName = microserviceName;
            return this;
        }
        public WaitImportUserDataRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public WaitImportUserDataRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WaitImportUserDataRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WaitImportUserDataRequest()
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMicroserviceName(!data.Keys.Contains("microserviceName") || data["microserviceName"] == null ? null : data["microserviceName"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["transactionId"] = TransactionId,
                ["userId"] = UserId,
                ["microserviceName"] = MicroserviceName,
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (MicroserviceName != null) {
                writer.WritePropertyName("microserviceName");
                writer.Write(MicroserviceName.ToString());
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += TransactionId + ":";
            key += UserId + ":";
            key += MicroserviceName + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}
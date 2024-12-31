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
	public class ArchiveDumpUserDataRequest : Gs2Request<ArchiveDumpUserDataRequest>
	{
         public string OwnerId { set; get; } = null!;
         public string TransactionId { set; get; } = null!;
        public ArchiveDumpUserDataRequest WithOwnerId(string ownerId) {
            this.OwnerId = ownerId;
            return this;
        }
        public ArchiveDumpUserDataRequest WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ArchiveDumpUserDataRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ArchiveDumpUserDataRequest()
                .WithOwnerId(!data.Keys.Contains("ownerId") || data["ownerId"] == null ? null : data["ownerId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["ownerId"] = OwnerId,
                ["transactionId"] = TransactionId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (OwnerId != null) {
                writer.WritePropertyName("ownerId");
                writer.Write(OwnerId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += OwnerId + ":";
            key += TransactionId + ":";
            return key;
        }
    }
}
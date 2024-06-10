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
	public class GetImportErrorLogRequest : Gs2Request<GetImportErrorLogRequest>
	{
         public string TransactionId { set; get; } = null!;
         public string ErrorLogName { set; get; } = null!;
        public GetImportErrorLogRequest WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public GetImportErrorLogRequest WithErrorLogName(string errorLogName) {
            this.ErrorLogName = errorLogName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetImportErrorLogRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetImportErrorLogRequest()
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithErrorLogName(!data.Keys.Contains("errorLogName") || data["errorLogName"] == null ? null : data["errorLogName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["transactionId"] = TransactionId,
                ["errorLogName"] = ErrorLogName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (ErrorLogName != null) {
                writer.WritePropertyName("errorLogName");
                writer.Write(ErrorLogName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += TransactionId + ":";
            key += ErrorLogName + ":";
            return key;
        }
    }
}
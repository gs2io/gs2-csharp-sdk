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
	public class DescribeImportErrorLogsRequest : Gs2Request<DescribeImportErrorLogsRequest>
	{
         public string TransactionId { set; get; } = null!;
         public string PageToken { set; get; } = null!;
         public int? Limit { set; get; } = null!;
        public DescribeImportErrorLogsRequest WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public DescribeImportErrorLogsRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public DescribeImportErrorLogsRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeImportErrorLogsRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeImportErrorLogsRequest()
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["transactionId"] = TransactionId,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += TransactionId + ":";
            key += PageToken + ":";
            key += Limit + ":";
            return key;
        }
    }
}
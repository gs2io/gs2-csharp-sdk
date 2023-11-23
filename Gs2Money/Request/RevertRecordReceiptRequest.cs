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
	public class RevertRecordReceiptRequest : Gs2Request<RevertRecordReceiptRequest>
	{
         public string NamespaceName { set; get; }
         public string UserId { set; get; }
         public string Receipt { set; get; }
        public string DuplicationAvoider { set; get; }
        public RevertRecordReceiptRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public RevertRecordReceiptRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public RevertRecordReceiptRequest WithReceipt(string receipt) {
            this.Receipt = receipt;
            return this;
        }

        public RevertRecordReceiptRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RevertRecordReceiptRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RevertRecordReceiptRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithReceipt(!data.Keys.Contains("receipt") || data["receipt"] == null ? null : data["receipt"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["receipt"] = Receipt,
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
            if (Receipt != null) {
                writer.WritePropertyName("receipt");
                writer.Write(Receipt.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += Receipt + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new RevertRecordReceiptRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                Receipt = Receipt,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (RevertRecordReceiptRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values RevertRecordReceiptRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values RevertRecordReceiptRequest::userId");
            }
            if (Receipt != y.Receipt) {
                throw new ArithmeticException("mismatch parameter values RevertRecordReceiptRequest::receipt");
            }
            return new RevertRecordReceiptRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                Receipt = Receipt,
            };
        }
    }
}
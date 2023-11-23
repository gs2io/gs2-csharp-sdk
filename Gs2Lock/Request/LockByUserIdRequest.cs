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
using Gs2.Gs2Lock.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Lock.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class LockByUserIdRequest : Gs2Request<LockByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string PropertyId { set; get; }
         public string UserId { set; get; }
         public string TransactionId { set; get; }
         public long? Ttl { set; get; }
        public string DuplicationAvoider { set; get; }
        public LockByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public LockByUserIdRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public LockByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public LockByUserIdRequest WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public LockByUserIdRequest WithTtl(long? ttl) {
            this.Ttl = ttl;
            return this;
        }

        public LockByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LockByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LockByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithTtl(!data.Keys.Contains("ttl") || data["ttl"] == null ? null : (long?)(data["ttl"].ToString().Contains(".") ? (long)double.Parse(data["ttl"].ToString()) : long.Parse(data["ttl"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["propertyId"] = PropertyId,
                ["userId"] = UserId,
                ["transactionId"] = TransactionId,
                ["ttl"] = Ttl,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (Ttl != null) {
                writer.WritePropertyName("ttl");
                writer.Write((Ttl.ToString().Contains(".") ? (long)double.Parse(Ttl.ToString()) : long.Parse(Ttl.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += PropertyId + ":";
            key += UserId + ":";
            key += TransactionId + ":";
            key += Ttl + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply LockByUserIdRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (LockByUserIdRequest)x;
            return this;
        }
    }
}
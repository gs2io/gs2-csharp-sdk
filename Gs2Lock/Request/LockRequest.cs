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
	public class LockRequest : Gs2Request<LockRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string PropertyId { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string TransactionId { set; get; } = null!;
         public long? Ttl { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public LockRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public LockRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public LockRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public LockRequest WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public LockRequest WithTtl(long? ttl) {
            this.Ttl = ttl;
            return this;
        }

        public LockRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LockRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LockRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithTtl(!data.Keys.Contains("ttl") || data["ttl"] == null ? null : (long?)(data["ttl"].ToString().Contains(".") ? (long)double.Parse(data["ttl"].ToString()) : long.Parse(data["ttl"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["propertyId"] = PropertyId,
                ["accessToken"] = AccessToken,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
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
            key += AccessToken + ":";
            key += TransactionId + ":";
            key += Ttl + ":";
            return key;
        }
    }
}
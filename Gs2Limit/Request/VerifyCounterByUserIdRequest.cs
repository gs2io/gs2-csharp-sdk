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
using Gs2.Gs2Limit.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Limit.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class VerifyCounterByUserIdRequest : Gs2Request<VerifyCounterByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string UserId { set; get; }
         public string LimitName { set; get; }
         public string CounterName { set; get; }
         public string VerifyType { set; get; }
         public int? Count { set; get; }
        public string DuplicationAvoider { set; get; }
        public VerifyCounterByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyCounterByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public VerifyCounterByUserIdRequest WithLimitName(string limitName) {
            this.LimitName = limitName;
            return this;
        }
        public VerifyCounterByUserIdRequest WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        public VerifyCounterByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyCounterByUserIdRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public VerifyCounterByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyCounterByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyCounterByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithLimitName(!data.Keys.Contains("limitName") || data["limitName"] == null ? null : data["limitName"].ToString())
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["limitName"] = LimitName,
                ["counterName"] = CounterName,
                ["verifyType"] = VerifyType,
                ["count"] = Count,
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
            if (LimitName != null) {
                writer.WritePropertyName("limitName");
                writer.Write(LimitName.ToString());
            }
            if (CounterName != null) {
                writer.WritePropertyName("counterName");
                writer.Write(CounterName.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += LimitName + ":";
            key += CounterName + ":";
            key += VerifyType + ":";
            key += Count + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new VerifyCounterByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                LimitName = LimitName,
                CounterName = CounterName,
                VerifyType = VerifyType,
                Count = Count,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (VerifyCounterByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values VerifyCounterByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values VerifyCounterByUserIdRequest::userId");
            }
            if (LimitName != y.LimitName) {
                throw new ArithmeticException("mismatch parameter values VerifyCounterByUserIdRequest::limitName");
            }
            if (CounterName != y.CounterName) {
                throw new ArithmeticException("mismatch parameter values VerifyCounterByUserIdRequest::counterName");
            }
            if (VerifyType != y.VerifyType) {
                throw new ArithmeticException("mismatch parameter values VerifyCounterByUserIdRequest::verifyType");
            }
            if (Count != y.Count) {
                throw new ArithmeticException("mismatch parameter values VerifyCounterByUserIdRequest::count");
            }
            return new VerifyCounterByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                LimitName = LimitName,
                CounterName = CounterName,
                VerifyType = VerifyType,
                Count = Count,
            };
        }
    }
}